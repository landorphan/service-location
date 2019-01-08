namespace Landorphan.TestUtilities.Internal.EventMonitoring
{
   using System;
   using System.Reflection;
   using System.Threading;
   using Landorphan.Common;

   internal class EventRecorder<TEventArgs> : IEventRecorder where TEventArgs : EventArgs
   {
      private readonly EventInfo _eventInfo;
      private readonly IEventStore _eventStore;

      internal EventRecorder(Object expectedEventSource, EventInfo eventInfo, IEventStore eventStore)
      {
         expectedEventSource.ArgumentNotNull(nameof(expectedEventSource));
         eventInfo.ArgumentNotNull(nameof(eventInfo));

         ExpectedEventSource = expectedEventSource;
         _eventInfo = eventInfo;
         _eventStore = eventStore;

         var handler = Delegate.CreateDelegate(_eventInfo.EventHandlerType, this, "RaisedEventHandler");
         AddEventHandler(handler);
      }

      /// <inheritdoc/>
      public Type EventArgsType => typeof(TEventArgs);

      /// <inheritdoc/>
      public String EventName => _eventInfo.Name;

      /// <inheritdoc/>
      public Object ExpectedEventSource { get; }

      private void AddEventHandler(Delegate handler)
      {
         // System.Reflection.EventInfo.AddEventHandler only works with public events
         // so duplicate the functionality and allow non-public events
         var addMethod = _eventInfo.GetAddMethod(true);
         addMethod.Invoke(ExpectedEventSource, new Object[] {handler});
      }

      public void RaisedEventHandler(Object sender, TEventArgs e)
      {
         var timestamp = DateTimeOffset.UtcNow;

         var rec = new RecordedEvent
         {
            ActualEventSource = sender,
            AdditionalData = e,
            EventName = EventName,
            ExpectedEventSource = ExpectedEventSource,
            ManagedThreadId = Thread.CurrentThread.ManagedThreadId,
            SequenceNumber = _eventStore.TakeSequenceNumber(),
            Timestamp = timestamp
         };
         _eventStore.Add(rec);
      }
   }
}