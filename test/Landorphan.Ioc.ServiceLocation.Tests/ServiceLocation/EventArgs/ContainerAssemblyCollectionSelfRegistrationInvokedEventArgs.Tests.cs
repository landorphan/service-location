namespace Landorphan.Ioc.Tests.ServiceLocation.EventArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using Landorphan.Ioc.ServiceLocation.EventArguments;
    using Landorphan.Ioc.ServiceLocation.Interfaces;
    using Landorphan.Ioc.ServiceLocation.Internal;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

   public static class ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs_Tests
   {
       // tests the IocContainer implementation of <see cref = "IIocContainer"/>

       [TestClass]
      public class When_I_create_an_ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs_using_the_container_assembly_constructor : DisposableArrangeActAssert
      {
          private readonly string containerName = "Isolated Test Container: ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs (container, assembly) constructor tests";
          private readonly Guid containerUid = Guid.NewGuid();
          private IEnumerable<Assembly> assemblies;
          private IOwnedIocContainer container;

          private ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs target;

          protected override void ArrangeMethod()
         {
            assemblies = new[] {GetType().Assembly};
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
         }

          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_assemblies_collection()
         {
            target = new ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs(container, null);
            target.Container.Should().Be(container);
            target.Assemblies.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_container()
         {
            target = new ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs(null, assemblies);
            target.Container.Should().BeNull();
            target.Assemblies.Should().OnlyContain(element => assemblies.Contains(element));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_provided_Container_and_assemblies()
         {
            target = new ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs(container, assemblies);
            target.Container.Should().Be(container);
            target.Assemblies.Should().OnlyContain(element => assemblies.Contains(element));
         }

         public void It_should_accept_both_a_null_container_and_assembly_collection()
         {
            target = new ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs(null, null);
            target.Container.Should().BeNull();
            target.Assemblies.Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_create_an_ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs_using_the_default_constructor : ArrangeActAssert
      {
          private ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs target;

          protected override void ArrangeMethod()
         {
            target = new ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs();
         }

          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_Container()
         {
            target.Container.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_an_empty_Assemblies_collection()
         {
            target.Assemblies.Should().NotBeNull();
            target.Assemblies.Should().BeEmpty();
         }
      }
   }
}
