namespace Landorphan.TestUtilities.Internal.EventMonitoring
{
   using System;

   internal sealed class RecordedEvent : IRecordedEvent
   {
      /// <inheritdoc/>
      public Object ActualEventSource { get; set; }

      /// <inheritdoc/>
      public EventArgs AdditionalData { get; set; }

      /// <inheritdoc/>
      public String EventName { get; set; }

      /// <inheritdoc/>
      public Object ExpectedEventSource { get; set; }

      /// <inheritdoc/>
      public Int32 ManagedThreadId { get; set; }

      /// <inheritdoc/>
      public Int64 SequenceNumber { get; set; }

      /// <inheritdoc/>
      public DateTimeOffset Timestamp { get; set; }
   }
}