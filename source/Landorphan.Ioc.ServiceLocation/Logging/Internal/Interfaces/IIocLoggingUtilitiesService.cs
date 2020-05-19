namespace Landorphan.Ioc.Logging.Internal
{
    internal interface IIocLoggingUtilitiesService
    {
        ILoggingUtilitiesForIocContainer LoggingUtilitiesForIocContainer { get; }

        ILoggingUtilitiesForIocServiceLocator LoggingUtilitiesForIocIocServiceLocator { get; }

        //EventId GetEventId(Int32 iocEventIdCode);
        string GetThreadId();
        string GetTimestamp();
    }
}
