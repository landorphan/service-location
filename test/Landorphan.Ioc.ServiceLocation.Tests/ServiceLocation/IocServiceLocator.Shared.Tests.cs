namespace Landorphan.Ioc.Tests.ServiceLocation
{
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class IocServiceLocator_Shared_Tests
   {
      [TestClass]
      public class When_I_have_an_IocServiceLocator : ArrangeActAssert
      {
         private readonly IocServiceLocator target = (IocServiceLocator)IocServiceLocator.Instance;

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_let_me_navigate_to_the_manager()
         {
            var actual = target.Manager;
            actual.Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_let_me_navigate_to_the_service_locator()
         {
            var actual = target.ServiceLocator;
            actual.Should().NotBeNull();
         }
      }
   }
}
