using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.TestFacilities
{
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public static class TestAssemblyInitializeCleanup
   {
      [AssemblyInitialize]
      public static void AssemblyInitialize(TestContext context)
      {
      }
   }
}
