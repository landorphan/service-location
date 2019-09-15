using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using System.Security.Cryptography.X509Certificates;
   using Landorphan.Instrumentation.Interfaces;
   using Landorphan.Instrumentation.PlugIns;

   public class TestEntryPointStorage : IInstrumentationPluginEntryPointStorage
   {
      public IDictionary<string, IEntryPointExecution> cotextEntryPoints = new Dictionary<string, IEntryPointExecution>();

      public string CurrentContext { get; set; }

      public IEntryPointExecution CurrentEntryPoint => this.cotextEntryPoints[CurrentContext];
   }
}
