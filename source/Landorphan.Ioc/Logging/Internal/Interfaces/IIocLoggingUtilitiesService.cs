namespace Landorphan.Ioc.Logging.Internal
{
   using System;
   using Microsoft.Extensions.Logging;

   internal interface IIocLoggingUtilitiesService
   {
      ILoggingUtilitiesForIocServiceLocator LoggingUtilitiesForIocIocServiceLocator { get; }
      ILoggingUtilitiesForIocContainer LoggingUtilitiesForIocContainer { get; }
      EventId GetEventId(Int32 iocEventIdCode);
      String GetThreadId();
      String GetTimestamp();
   }
}