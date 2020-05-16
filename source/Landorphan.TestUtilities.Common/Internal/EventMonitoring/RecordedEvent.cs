namespace Landorphan.TestUtilities.Internal.EventMonitoring
{
    using System;

    internal sealed class RecordedEvent : IRecordedEvent
   {
       /// <inheritdoc/>
      public object ActualEventSource { get; set; }

       /// <inheritdoc/>
      public EventArgs AdditionalData { get; set; }

       /// <inheritdoc/>
      public string EventName { get; set; }

       /// <inheritdoc/>
      public object ExpectedEventSource { get; set; }

       /// <inheritdoc/>
      public int ManagedThreadId { get; set; }

       /// <inheritdoc/>
      public long SequenceNumber { get; set; }

       /// <inheritdoc/>
      public DateTimeOffset Timestamp { get; set; }
   }
}
