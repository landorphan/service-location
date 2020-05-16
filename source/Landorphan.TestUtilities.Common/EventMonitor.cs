namespace Landorphan.TestUtilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;
    using Landorphan.Common;
    using Landorphan.TestUtilities.Internal.EventMonitoring;

    /// <summary>
   /// Monitors events from one or more event sources.
   /// </summary>
   /// <remarks>
   /// Used to track events both public and non-public that follow the void (Object sender, EventArgs e) pattern.
   /// </remarks>
   [SuppressMessage("Microsoft.Naming", "CA1710: Identifiers should have correct suffix")]
   public sealed class EventMonitor : IEnumerable<IRecordedEvent>, IEventStore

   {
       private readonly object _recordedEventsLock = new object();
       private readonly object _recordersLock = new object();
       private readonly object _sourceLock = new object();
       private ImmutableHashSet<IEventRecorder> _eventRecorders = ImmutableHashSet<IEventRecorder>.Empty;
       private long _eventSequenceNumber;
       private ImmutableHashSet<object> _eventSources = ImmutableHashSet<object>.Empty.WithComparer(new ReferenceEqualityComparer());
       private ImmutableQueue<IRecordedEvent> _recordedEvents = ImmutableQueue<IRecordedEvent>.Empty;

       /// <inheritdoc />
      public IEnumerator<IRecordedEvent> GetEnumerator()
      {
         return ((IEnumerable<IRecordedEvent>)_recordedEvents).GetEnumerator();
      }

       /// <inheritdoc />
      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

       /// <inheritdoc />
      void IEventStore.Add(IRecordedEvent recordedEvent)
      {
         lock (_recordedEventsLock)
         {
            _recordedEvents = _recordedEvents.Enqueue(recordedEvent);
         }
      }

       long IEventStore.TakeSequenceNumber()
      {
         var rv = Interlocked.Increment(ref _eventSequenceNumber);
         return rv;
      }

       /// <summary>
      /// Adds an event source.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// Thrown when <paramref name="eventSource" /> is null.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// Thrown when <paramref name="eventSource" /> is of a type that does not source events.
      /// </exception>
      /// <param name="eventSource">
      /// The event source.
      /// </param>
      /// <returns>
      /// <c>true</c> if the event source is added; otherwise <c>false</c> (the instance was previously added).
      /// </returns>
      [SuppressMessage("SonarLint.CodeSmell", "S4056:Overloads with a 'CultureInfo' or an 'IFormatProvider' parameter should be used", Justification = "reflection (MWP)")]
      public bool AddEventSource(object eventSource)
      {
         eventSource.ArgumentNotNull(nameof(eventSource));

         // Find all events for the type of the event source, throwing if the event source does not have any declared events.
         var eventSourceType = eventSource.GetType();
         var eventInfos =
            eventSourceType.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

         if (eventInfos.Length < 1)
         {
            throw new ArgumentException(
               string.Format(
                  CultureInfo.InvariantCulture,
                  "This instance is of type '{0}' which does not source any events.",
                  eventSourceType.FullName),
               nameof(eventSource));
         }

         // Avoid duplicates and warn the test author when such arises
         if (_eventSources.Contains(eventSource))
         {
            Trace.TraceWarning("The eventSource has already been registered.  Is this a test bug?");
            return false;
         }

         // register the event source and subscribe to its events
         lock (_sourceLock)
         {
            _eventSources = _eventSources.Add(eventSource);
         }

         foreach (var eventInfo in eventInfos)
         {
            var handlerInvokeMethod = eventInfo.EventHandlerType.GetMethod("Invoke");

            // {Object sender, EventArgs (or derived ) e }
            // ReSharper disable once PossibleNullReferenceException
            var eventArgsType = handlerInvokeMethod.GetParameters()[1].ParameterType;

            var genericRecorderType = typeof(EventRecorder<>);
            var concreteRecorderType = genericRecorderType.MakeGenericType(eventArgsType);

            // EventRecorder(Object expectedEventSource, EventInfo eventInfo, IEventStore eventStore)
            var recorderCtorTypes = new[] {typeof(object), typeof(EventInfo), typeof(IEventStore)};
            var ctor = concreteRecorderType.GetConstructor(
               BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
               null,
               recorderCtorTypes,
               null);
            var recorderInstance = (IEventRecorder)ctor.Invoke(new[] {eventSource, eventInfo, this});
            lock (_recordersLock)
            {
               _eventRecorders = _eventRecorders.Add(recorderInstance);
            }
         }

         return true;
      }

       private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
      {
          /// <inheritdoc />
         bool IEqualityComparer<object>.Equals(object x, object y)
         {
            return ReferenceEquals(x, y);
         }

          /// <inheritdoc />
         int IEqualityComparer<object>.GetHashCode(object obj)
         {
            return ReferenceEquals(obj, null) ? 0 : obj.GetHashCode();
         }
      }
   }
}
