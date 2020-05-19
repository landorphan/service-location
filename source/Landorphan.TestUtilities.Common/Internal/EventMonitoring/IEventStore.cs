namespace Landorphan.TestUtilities.Internal.EventMonitoring
{
    internal interface IEventStore
    {
        void Add(IRecordedEvent recordedEvent);
        long TakeSequenceNumber();
    }
}
