namespace Landorphan.TestUtilities.Internal.EventMonitoring
{
   using System;

   internal interface IEventStore
   {
      void Add(IRecordedEvent recordedEvent);
      Int64 TakeSequenceNumber();
   }
}