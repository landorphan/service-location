namespace Landorphan.Ioc.Tests.ServiceLocation.Internal
{
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class AssemblyRegistrarRepository_Tests
   {
      [TestClass]
      public class When_I_create_an_AssemblyRegistrarRepository_using_the_default_ctor : ArrangeActAssert
      {
         private AssemblyRegistrarRepository target;

         protected override void ArrangeMethod()
         {
            // TODO: (mwp) NEED MORE TESTS OF AssemblyRegistrarRepository
            target = new AssemblyRegistrarRepository();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_zero_entries()
         {
            target.AssemblyRegistrarInstances.Should().BeEmpty();
            target.AssemblyRegistrarTypes.Should().BeEmpty();
         }
      }
   }
}
