namespace Landorphan.TestUtilities.Internal.EventMonitoring
{
   using System;

   internal interface IEventRecorder
   {
      Type EventArgsType { get; }
      String EventName { get; }
      Object ExpectedEventSource { get; }
   }
}