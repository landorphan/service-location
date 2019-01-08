namespace Landorphan.Common.Tests.Extensions
{
   using System.Diagnostics.CodeAnalysis;
   using System.Reflection;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [SuppressMessage("SonarLint.Naming", "S101:Types should be named in PascalCase", Justification = "Test code (MWP)")]
   [TestClass]
   public class When_I_call_SafeGetTypes : ArrangeActAssert
   {
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_not_throw_ReflectionTypeLoadException()
      {
         // TODO: find an assembly with dependencies that are not loaded so that the exception must be eaten.
         var assembly = Assembly.Load("System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
         assembly.GetTypes();
         assembly.SafeGetTypes();
      }
   }
}