namespace Landorphan.Ioc.Logging.Internal
{
   using System;

   internal interface IIocLoggingUtilitiesService
   {
      ILoggingUtilitiesForIocContainer LoggingUtilitiesForIocContainer { get; }
      ILoggingUtilitiesForIocServiceLocator LoggingUtilitiesForIocIocServiceLocator { get; }
      //EventId GetEventId(Int32 iocEventIdCode);
      String GetThreadId();
      String GetTimestamp();
   }
}
