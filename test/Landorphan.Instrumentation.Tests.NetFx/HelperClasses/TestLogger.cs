using System;
using System.Collections.Generic;

namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using System.Diagnostics;
   using Landorphan.Instrumentation.Interfaces;
   using Landorphan.Instrumentation.PlugIns;

   public class TestLogger : IInstrumentationPluginLog
   {
      public void LogMethodEntry(IMethodCompilationData methodData, IInstrumentationContext context, ArgumentData[] arguments)
      {
         Trace.WriteLine($"Entering Method: {methodData.MethodName}");
      }

      public void LogMethodExit(IMethodCompilationData methodData, IInstrumentationContext context, ArgumentData[] arguments, object returnValue)
      {
         Trace.WriteLine($"Exiting Method : {methodData.MethodName}");
      }

      public void LogMethodException(IMethodCompilationData methodData, IInstrumentationContext context, ArgumentData[] arguments, Exception exception)
      {
         Trace.WriteLine($"Error in Method: {methodData.MethodName}");
      }

      public void LogAction(string name, IInstrumentationContext context, IEnumerable<KeyValuePair<string, string>> tags)
      {
         Trace.WriteLine($"Action Occurred: {name}");
      }
   }
}
