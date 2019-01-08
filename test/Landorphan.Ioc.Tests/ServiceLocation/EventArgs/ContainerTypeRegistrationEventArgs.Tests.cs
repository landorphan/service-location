namespace Landorphan.Ioc.Tests.ServiceLocation.EventArgs
{
   using System;
   using System.Globalization;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   public static class ContainerTypeRegistrationEventArgs_Tests
   {
      [TestClass]
      public class When_I_create_an_ContainerTypeRegistrationEventArgs_using_the_container_assembly_constructor : DisposableArrangeActAssert
      {
         private readonly String containerName = "Isolated Test Container: ContainerTypeEventArgs (container, fromType, name, toType, instance) constructor tests";
         private readonly Guid containerUid = Guid.NewGuid();

         private IOwnedIocContainer container;
         private Type fromType;
         private Object instance;
         private String name;
         private ContainerTypeRegistrationEventArgs target;
         private Type toType;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            fromType = typeof(IInterface);
            instance = new Implementation();
            name = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
            toType = typeof(Implementation);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_container()
         {
            target = new ContainerTypeRegistrationEventArgs(null, fromType, name, toType, instance);
            target.Container.Should().BeNull();
            target.FromType.Should().Be(fromType);
            target.Name.Should().Be(name);
            target.ToType.Should().Be(toType);
            target.Instance.Should().Be(instance);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_fromType()
         {
            target = new ContainerTypeRegistrationEventArgs(container, null, name, toType, instance);
            target.Container.Should().Be(container);
            target.FromType.Should().BeNull();
            target.Name.Should().Be(name);
            target.ToType.Should().Be(toType);
            target.Instance.Should().Be(instance);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_instance()
         {
            target = new ContainerTypeRegistrationEventArgs(container, fromType, name, toType, null);
            target.Container.Should().Be(container);
            target.FromType.Should().Be(fromType);
            target.Name.Should().Be(name);
            target.ToType.Should().Be(toType);
            target.Instance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_name()
         {
            target = new ContainerTypeRegistrationEventArgs(container, fromType, null, toType, instance);
            target.Container.Should().Be(container);
            target.FromType.Should().Be(fromType);
            target.Name.Should().BeEmpty();
            target.ToType.Should().Be(toType);
            target.Instance.Should().Be(instance);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_toType()
         {
            target = new ContainerTypeRegistrationEventArgs(container, fromType, name, null, instance);
            target.Container.Should().Be(container);
            target.FromType.Should().Be(fromType);
            target.Name.Should().Be(name);
            target.ToType.Should().BeNull();
            target.Instance.Should().Be(instance);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_provided_values()
         {
            target = new ContainerTypeRegistrationEventArgs(container, fromType, name, toType, instance);
            target.Container.Should().Be(container);
            target.FromType.Should().Be(fromType);
            target.Name.Should().Be(name);
            target.ToType.Should().Be(toType);
            target.Instance.Should().Be(instance);
         }
      }

      [TestClass]
      public class When_I_create_an_ContainerTypeRegistrationEventArgs_using_the_default_constructor : ArrangeActAssert
      {
         private ContainerTypeRegistrationEventArgs target;

         protected override void ArrangeMethod()
         {
            target = new ContainerTypeRegistrationEventArgs();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_default_values()
         {
            target.Container.Should().BeNull();
            target.FromType.Should().BeNull();
            target.Name.Should().BeEmpty();
            target.ToType.Should().BeNull();
            target.Instance.Should().BeNull();
         }
      }

      private interface IInterface
      {
      }

      private class Implementation : IInterface
      {
      }
   }
}