namespace Landorphan.TestUtilities.NUnit.Tests
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using global::NUnit.Framework;
    using Landorphan.Common;

    // ReSharper disable InconsistentNaming

   public static class EventMonitor_Tests
   {
       [TestFixture]
      public sealed class When_I_attempt_to_monitor_the_events_of_an_object_that_does_not_source_events : ArrangeActAssert
      {
          private TestClassSourcingNoEvents _eventSource;

          [Test]
         [Category(TestTiming.CheckIn)]
         public void It_should_throw()
         {
            Action throwingAction = () => MonitoredEvents.AddEventSource(_eventSource);
            throwingAction.Should().Throw<ArgumentException>().WithMessage("*which does not source any events*");
         }

         protected override void ArrangeMethod()
         {
            _eventSource = new TestClassSourcingNoEvents();
         }
      }

      [TestFixture]
      public sealed class When_I_monitor_the_events_of_an_object_that_sources_events_with_data : ArrangeActAssert
      {
          private TestClassSourcingAnEventArgsDerivedEvent _eventSource;
          private Guid _expected;

          [Test]
         [Category(TestTiming.CheckIn)]
         public void It_should_record_that_the_event_fired()
         {
            var recordedEvents = (from re in MonitoredEvents select re).ToList();
            recordedEvents.Count.Should().Be(1);
            var evt = recordedEvents.First();
            evt.ActualEventSource.Should().BeSameAs(_eventSource);
            evt.ExpectedEventSource.Should().BeSameAs(_eventSource);
            evt.EventName.Should().Be("MyEvent");
            evt.SequenceNumber.Should().Be(1);
            evt.AdditionalData.Should().BeOfType(typeof(TestClassAdditionalDataEventArgs));
            ((TestClassAdditionalDataEventArgs)evt.AdditionalData).Value.Should().Be(_expected);
         }

         protected override void ActMethod()
         {
            _eventSource.FireMyEvent(_expected);
         }

         protected override void ArrangeMethod()
         {
            _expected = Guid.NewGuid();
            _eventSource = new TestClassSourcingAnEventArgsDerivedEvent();
            MonitoredEvents.AddEventSource(_eventSource);
         }
      }

      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "nonpublic")]
      [TestFixture]
      public sealed class When_I_monitor_the_events_of_an_object_that_sources_non_public_events : ArrangeActAssert
      {
          private TestClassSourcingANonPublicEventArgsEvent _eventSource;

          [Test]
         [Category(TestTiming.CheckIn)]
         public void It_should_record_that_the_event_fired()
         {
            var recordedEvents = (from re in MonitoredEvents select re).ToList();
            recordedEvents.Count.Should().Be(1);
            var evt = recordedEvents.First();
            evt.ActualEventSource.Should().BeSameAs(_eventSource);
            evt.ExpectedEventSource.Should().BeSameAs(_eventSource);
            evt.AdditionalData.Should().Be(EventArgs.Empty);
            evt.EventName.Should().Be("MyEvent");
            evt.SequenceNumber.Should().Be(1);
         }

         protected override void ActMethod()
         {
            _eventSource.FireMyEvent();
         }

         protected override void ArrangeMethod()
         {
            _eventSource = new TestClassSourcingANonPublicEventArgsEvent();
            MonitoredEvents.AddEventSource(_eventSource);
         }
      }

      [TestFixture]
      public sealed class When_I_monitor_the_events_of_an_object_that_sources_public_events : ArrangeActAssert
      {
          private TestClassSourcingAPublicEventArgsEvent _eventSource;

          [Test]
         [Category(TestTiming.CheckIn)]
         public void It_should_record_that_the_event_fired()
         {
            var recordedEvents = (from re in MonitoredEvents select re).ToList();
            recordedEvents.Count.Should().Be(1);
            var evt = recordedEvents.First();
            evt.ActualEventSource.Should().BeSameAs(_eventSource);
            evt.ExpectedEventSource.Should().BeSameAs(_eventSource);
            evt.AdditionalData.Should().Be(EventArgs.Empty);
            evt.EventName.Should().Be("MyEvent");
            evt.SequenceNumber.Should().Be(1);
         }

         protected override void ActMethod()
         {
            _eventSource.FireMyEvent();
         }

         protected override void ArrangeMethod()
         {
            _eventSource = new TestClassSourcingAPublicEventArgsEvent();
            MonitoredEvents.AddEventSource(_eventSource);
         }
      }

      [TestFixture]
      public sealed class When_I_monitor_the_events_of_the_same_instance_more_than_once : DisposableArrangeActAssert
      {
          private readonly MemoryStream _memoryStream = new MemoryStream(1024);
          private TestClassSourcingAPublicEventArgsEvent _eventSource;

          [Test]
         [Category(TestTiming.CheckIn)]
         public void It_should_emit_a_trace_warning()
         {
            Trace.Flush();
            _memoryStream.Position = 0;
            string foo;
            using (var reader = new StreamReader(_memoryStream))
            {
                foo = reader.ReadToEnd();
            }

            foo.Should().Contain("The eventSource has already been registered.  Is this a test bug?");
         }

         [Test]
         [Category(TestTiming.CheckIn)]
         public void It_should_record_that_the_event_fired_once()
         {
            var recordedEvents = (from re in MonitoredEvents select re).ToList();
            recordedEvents.Count.Should().Be(1);
            var evt = recordedEvents.First();
            evt.ActualEventSource.Should().BeSameAs(_eventSource);
            evt.ExpectedEventSource.Should().BeSameAs(_eventSource);
            evt.AdditionalData.Should().Be(EventArgs.Empty);
            evt.EventName.Should().Be("MyEvent");
            evt.SequenceNumber.Should().Be(1);
         }

         protected override void ActMethod()
         {
            MonitoredEvents.AddEventSource(_eventSource);
            MonitoredEvents.AddEventSource(_eventSource);

            _eventSource.FireMyEvent();
         }

         protected override void ArrangeMethod()
         {
            var listener = new TextWriterTraceListener(_memoryStream) {Filter = new EventTypeFilter(SourceLevels.Warning)};

            Trace.Listeners.Add(listener);
            _eventSource = new TestClassSourcingAPublicEventArgsEvent();
         }
      }

      [Serializable]
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
          [SuppressMessage("SonarLint.CodeSmell", "S1144: Unused private types or members should be removed")]
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
      {}
   }
}
