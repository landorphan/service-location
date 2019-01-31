namespace Landorphan.Ioc.Tests.ServiceLocation.EventArgs
{
   using System;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation.EventArguments;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class ContainerConfigurationEventArgs_Tests
   {
      [TestClass]
      public class When_I_create_an_ContainerConfigurationEventArgs_using_the_container_assembly_constructor : DisposableArrangeActAssert
      {
         private readonly String containerName = "Isolated Test Container: ContainerConfigurationEventArgs (configuration) constructor tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IIocContainerConfiguration configuration;
         private IOwnedIocContainer container;

         private ContainerConfigurationEventArgs target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            configuration = new IocContainerConfiguration(container);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_container()
         {
            target = new ContainerConfigurationEventArgs(null);
            target.Configuration.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_provided_configuration()
         {
            target = new ContainerConfigurationEventArgs(configuration);
            target.Configuration.Should().Be(configuration);
         }
      }

      [TestClass]
      public class When_I_create_an_ContainerConfigurationEventArgs_using_the_default_constructor : ArrangeActAssert
      {
         private ContainerConfigurationEventArgs target;

         protected override void ArrangeMethod()
         {
            target = new ContainerConfigurationEventArgs();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_Configuration()
         {
            target.Configuration.Should().BeNull();
         }
      }
   }
}
