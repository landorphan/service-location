namespace Landorphan.Ioc.Tests.ServiceLocation.Extensions
{
   using System;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.Ioc.Tests.Copy.Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class IsPrecuded_Tests
   {
      [TestClass]
      public class When_I_have_an_isolated_IIocContainer_and_call_IsPrecluded_extension_methods : DisposableArrangeActAssert
      {
         private readonly String containerName = "Isolated Test Container: IsPrecluded Extension Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private IIocContainerManager manager;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            manager = container.Manager;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_type_is_null()
         {
            var actual = manager.IsPrecluded(null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_no_types_have_been_precluded()
         {
            manager.IsPrecluded<IAnInterface>().Should().BeFalse();
            manager.IsPrecluded(typeof(IAnInterface)).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_the_type_has_been_precluded()
         {
            manager.AddPrecludedType<IAnInterface>();
            manager.IsPrecluded<IAnInterface>().Should().BeTrue();
            manager.IsPrecluded(typeof(IAnInterface)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_manager_is_null()
         {
            Action throwingAction = () => IsPrecludedExtensions.IsPrecluded<IAnInterface>(null);
            throwingAction.Should().Throw<ArgumentNullException>();

            throwingAction = () => IsPrecludedExtensions.IsPrecluded(null, typeof(IAnInterface));
            throwingAction.Should().Throw<ArgumentNullException>();
         }
      }

      private interface IAnInterface
      {
      }
   }
}
