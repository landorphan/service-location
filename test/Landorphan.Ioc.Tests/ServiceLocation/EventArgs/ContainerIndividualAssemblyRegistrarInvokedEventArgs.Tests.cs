namespace Landorphan.Ioc.Tests.ServiceLocation.EventArgs
{
   using System;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   public static class ContainerIndividualAssemblyRegistrarInvokedEventArgs_Tests
   {
      [TestClass]
      public class When_I_create_an_ContainerIndividualAssemblyRegistrarInvokedEventArgs_using_the_container_assembly_constructor : DisposableArrangeActAssert
      {
         private readonly String containerName = "Isolated Test Container: ContainerIndividualAssemblyRegistrarInvokedEventArgs (container, assemblyRegistrar) constructor tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IAssemblySelfRegistration assemblySelfRegistration;
         private IOwnedIocContainer container;

         private ContainerIndividualAssemblyRegistrarInvokedEventArgs target;

         protected override void ArrangeMethod()
         {
            assemblySelfRegistration = new ATestTypeAssemblySelfRegistration();
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_assembly()
         {
            target = new ContainerIndividualAssemblyRegistrarInvokedEventArgs(container, null);
            target.Container.Should().Be(container);
            target.SelfRegistration.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_container()
         {
            target = new ContainerIndividualAssemblyRegistrarInvokedEventArgs(null, assemblySelfRegistration);
            target.Container.Should().BeNull();
            target.SelfRegistration.Should().Be(assemblySelfRegistration);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_provided_Assembly_and_container()
         {
            target = new ContainerIndividualAssemblyRegistrarInvokedEventArgs(container, assemblySelfRegistration);
            target.Container.Should().Be(container);
            target.SelfRegistration.Should().Be(assemblySelfRegistration);
         }

         public void It_should_accept_both_a_null_container_and_assembly()
         {
            target = new ContainerIndividualAssemblyRegistrarInvokedEventArgs(null, null);
            target.Container.Should().BeNull();
            target.SelfRegistration.Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_create_an_ContainerIndividualAssemblyRegistrarInvokedEventArgs_using_the_default_constructor : ArrangeActAssert
      {
         private ContainerIndividualAssemblyRegistrarInvokedEventArgs target;

         protected override void ArrangeMethod()
         {
            target = new ContainerIndividualAssemblyRegistrarInvokedEventArgs();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_Container()
         {
            target.Container.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_Registrar()
         {
            target.SelfRegistration.Should().BeNull();
         }
      }

      private class ATestTypeAssemblySelfRegistration : IAssemblySelfRegistration
      {
         public void RegisterServiceInstances(IIocContainerRegistrar registrar)
         {
            // no implementation.
         }
      }
   }
}