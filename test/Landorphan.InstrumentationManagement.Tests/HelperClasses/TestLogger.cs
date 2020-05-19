namespace Landorphan.InstrumentationManagement.Tests.HelperClasses
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Landorphan.InstrumentationManagement.Interfaces;
    using Landorphan.InstrumentationManagement.PlugIns;

    public class TestLogger : IInstrumentationPluginLog
    {
        public void LogAction(string name, IInstrumentationContext context, KeyValuePair<string, string>[] tags)
        {
            Trace.WriteLine($"Action Occurred: {name}");
        }

        public void LogMethodEntry(IMethodCompilationData methodData, IInstrumentationContext context, ArgumentData[] arguments)
        {
            Trace.WriteLine($"Entering Method: {methodData.MethodName}");
        }

        public void LogMethodException(IMethodCompilationData methodData, IInstrumentationContext context, ArgumentData[] arguments, Exception exception)
        {
            Trace.WriteLine($"Error in Method: {methodData.MethodName}");
        }

        public void LogMethodExit(IMethodCompilationData methodData, IInstrumentationContext context, ArgumentData[] arguments, object returnValue)
        {
            Trace.WriteLine($"Exiting Method : {methodData.MethodName}");
        }
    }
}
