namespace Landorphan.TestUtilities.Internal.EventMonitoring
{
    using System;

    internal interface IEventRecorder
   {
       Type EventArgsType { get; }
       string EventName { get; }
       object ExpectedEventSource { get; }
   }
}
