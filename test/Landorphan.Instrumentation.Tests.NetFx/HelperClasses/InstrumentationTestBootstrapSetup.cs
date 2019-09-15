namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using System;

   public class InstrumentationTestBootstrapSetup
   {
      public Application Application { get; set; }
      public bool SetAsyncStorage { get; set; }
      public bool SetSessionStorage { get; set; }
      public bool SetBootstrapData { get; set; }

      public bool SetIdentityManager { get; set; }

      public bool SetEntryPointStorage { get; set; }

      public bool SetPerfManager { get; set; }

      public bool SetLogger { get; set; }

      public string ApplicationEntryPointName { get; set; }
   }
}
