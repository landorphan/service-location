namespace Landorphan.InstrumentationManagement.Tests.HelperClasses
{
    using System.Collections.Generic;
    using Landorphan.InstrumentationManagement.Interfaces;
    using Landorphan.InstrumentationManagement.PlugIns;

    public class TestEntryPointStorage : IInstrumentationPluginEntryPointStorage
   {
       public IDictionary<string, IEntryPointExecution> cotextEntryPoints = new Dictionary<string, IEntryPointExecution>();

       public string CurrentContext { get; set; }

       public IEntryPointExecution CurrentEntryPoint => cotextEntryPoints[CurrentContext];
   }
}
