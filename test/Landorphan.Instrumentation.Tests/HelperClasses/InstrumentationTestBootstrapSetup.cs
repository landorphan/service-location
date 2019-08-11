namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using System;

   public class InstrumentationTestBootstrapSetup
   {
      public ApplicationEnum Application { get; set; }
      public bool SetAsyncStorage { get; set; }
      public bool SetSessionStorage { get; set; }
      public bool SetBootstrapData { get; set; }

      public bool SetIdentityManager { get; set; }
   }
}
