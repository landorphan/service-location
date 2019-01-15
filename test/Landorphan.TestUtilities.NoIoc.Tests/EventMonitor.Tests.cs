namespace Landorphan.TestUtilities.Tests
{
   using System;
   using System.Diagnostics;
   using System.IO;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Common;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class EventMonitor_Tests
   {
      [TestClass]
      public sealed class When_I_attempt_to_monitor_the_events_of_an_object_that_does_not_source_events : ArrangeActAssert
      {
         private TestClassSourcingNoEvents _eventSource;

         protected override void ArrangeMethod()
         {
            _eventSource = new TestClassSourcingNoEvents();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw()
         {
            Action throwingAction = () => MonitoredEvents.AddEventSource(_eventSource);
            throwingAction.Should()
               .Throw<ArgumentException>()
               .WithMessage("*which does not source any events*");
         }
      }

      [TestClass]
      public sealed class When_I_monitor_the_events_of_an_object_that_sources_events_with_data : ArrangeActAssert
      {
         private TestClassSourcingAnEventArgsDerivedEvent _eventSource;
         private Guid _expected;

         protected override void ArrangeMethod()
         {
            _expected = Guid.NewGuid();
            _eventSource = new TestClassSourcingAnEventArgsDerivedEvent();
            MonitoredEvents.AddEventSource(_eventSource);
         }

         protected override void ActMethod()
         {
            _eventSource.FireMyEvent(_expected);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_record_that_the_event_fired()
         {
            var recordedEvents = (from re in MonitoredEvents select re).ToList();
            recordedEvents.Count.Should().Be(1);
            var evt = recordedEvents.First();
            evt.ActualEventSource.Should().Be(_eventSource);
            evt.ExpectedEventSource.Should().Be(_eventSource);
            evt.EventName.Should().Be("MyEvent");
            evt.SequenceNumber.Should().Be(1);
            evt.AdditionalData.Should().BeOfType(typeof(TestClassAdditionalDataEventArgs));
            ((TestClassAdditionalDataEventArgs)evt.AdditionalData).Value.Should().Be(_expected);
         }
      }

      [TestClass]
      public sealed class When_I_monitor_the_events_of_an_object_that_sources_non_public_events : ArrangeActAssert
      {
         private TestClassSourcingANonPublicEventArgsEvent _eventSource;

         protected override void ArrangeMethod()
         {
            _eventSource = new TestClassSourcingANonPublicEventArgsEvent();
            MonitoredEvents.AddEventSource(_eventSource);
         }

         protected override void ActMethod()
         {
            _eventSource.FireMyEvent();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_record_that_the_event_fired()
         {
            var recordedEvents = (from re in MonitoredEvents select re).ToList();
            recordedEvents.Count.Should().Be(1);
            var evt = recordedEvents.First();
            evt.ActualEventSource.Should().Be(_eventSource);
            evt.ExpectedEventSource.Should().Be(_eventSource);
            evt.AdditionalData.Should().Be(EventArgs.Empty);
            evt.EventName.Should().Be("MyEvent");
            evt.SequenceNumber.Should().Be(1);
         }
      }

      [TestClass]
      public sealed class When_I_monitor_the_events_of_an_object_that_sources_public_events : ArrangeActAssert
      {
         private TestClassSourcingAPublicEventArgsEvent _eventSource;

         protected override void ArrangeMethod()
         {
            _eventSource = new TestClassSourcingAPublicEventArgsEvent();
            MonitoredEvents.AddEventSource(_eventSource);
         }

         protected override void ActMethod()
         {
            _eventSource.FireMyEvent();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_record_that_the_event_fired()
         {
            var recordedEvents = (from re in MonitoredEvents select re).ToList();
            recordedEvents.Count.Should().Be(1);
            var evt = recordedEvents.First();
            evt.ActualEventSource.Should().Be(_eventSource);
            evt.ExpectedEventSource.Should().Be(_eventSource);
            evt.AdditionalData.Should().Be(EventArgs.Empty);
            evt.EventName.Should().Be("MyEvent");
            evt.SequenceNumber.Should().Be(1);
         }
      }

      [TestClass]
      public sealed class When_I_monitor_the_events_of_the_same_instance_more_than_once : DisposableArrangeActAssert
      {
         private readonly MemoryStream _memoryStream = new MemoryStream(1024);
         private TestClassSourcingAPublicEventArgsEvent _eventSource;

         protected override void ArrangeMethod()
         {
            var listener = new TextWriterTraceListener(_memoryStream) {Filter = new EventTypeFilter(SourceLevels.Warning)};

            Trace.Listeners.Add(listener);
            _eventSource = new TestClassSourcingAPublicEventArgsEvent();
         }

         protected override void ActMethod()
         {
            MonitoredEvents.AddEventSource(_eventSource);
            MonitoredEvents.AddEventSource(_eventSource);

            _eventSource.FireMyEvent();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_emit_a_trace_warning()
         {
            Trace.Flush();
            _memoryStream.Position = 0;
            var reader = new StreamReader(_memoryStream);
            var foo = reader.ReadToEnd();
            foo.Should().Contain("The eventSource has already been registered.  Is this a test bug?");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_record_that_the_event_fired_once()
         {
            var recordedEvents = (from re in MonitoredEvents select re).ToList();
            recordedEvents.Count.Should().Be(1);
            var evt = recordedEvents.First();
            evt.ActualEventSource.Should().Be(_eventSource);
            evt.ExpectedEventSource.Should().Be(_eventSource);
            evt.AdditionalData.Should().Be(EventArgs.Empty);
            evt.EventName.Should().Be("MyEvent");
            evt.SequenceNumber.Should().Be(1);
         }
      }

      internal sealed class TestClassAdditionalDataEventArgs : EventArgs
      {
         internal TestClassAdditionalDataEventArgs(Guid value)
         {
            Value = value;
         }

         internal Guid Value { get; }
      }

      private class TestClassSourcingAnEventArgsDerivedEvent
      {
         private readonly SourceWeakEventHandlerSet<TestClassAdditionalDataEventArgs> _eventListeners =
            new SourceWeakEventHandlerSet<TestClassAdditionalDataEventArgs>();

         // This event is dynamically subscribed to
         // ReSharper disable once EventNeverSubscribedTo.Local
         internal event EventHandler<TestClassAdditionalDataEventArgs> MyEvent
         {
            add => _eventListeners.Add(value);

            remove => _eventListeners.Remove(value);
         }

         internal void FireMyEvent(Guid additionalData)
         {
            var e = new TestClassAdditionalDataEventArgs(additionalData);
            _eventListeners.Invoke(this, e);
         }
      }

      private class TestClassSourcingANonPublicEventArgsEvent
      {
         // This event is dynamically subscribed to
         // ReSharper disable once EventNeverSubscribedTo.Local
         internal event EventHandler<EventArgs> MyEvent;

         internal void FireMyEvent()
         {
            var listeners = MyEvent;
            listeners?.Invoke(this, EventArgs.Empty);
         }
      }

      private class TestClassSourcingAPublicEventArgsEvent
      {
         // This event is dynamically subscribed to
         // ReSharper disable once EventNeverSubscribedTo.Local
         public event EventHandler<EventArgs> MyEvent;

         internal void FireMyEvent()
         {
            var listeners = MyEvent;
            listeners?.Invoke(this, EventArgs.Empty);
         }
      }

      private class TestClassSourcingNoEvents
      {
      }
   }
}
