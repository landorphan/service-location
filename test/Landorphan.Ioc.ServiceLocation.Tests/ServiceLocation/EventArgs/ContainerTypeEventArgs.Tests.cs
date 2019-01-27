namespace Landorphan.Ioc.Tests.ServiceLocation.EventArgs
{
   // ReSharper disable InconsistentNaming
   using System;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.Ioc.Tests.Copy.Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   public static class ContainerTypeEventArgs_Tests
   {
      [TestClass]
      public class When_I_create_an_ContainerTypeEventArgs_using_the_container_type_constructor : DisposableArrangeActAssert
      {
         private readonly String containerName = "Isolated Test Container: ContainerTypeEventArgs (container, type) constructor tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;

         private ContainerTypeEventArgs target;
         private Type type;

         protected override void ArrangeMethod()
         {
            type = typeof(IType);
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_container()
         {
            target = new ContainerTypeEventArgs(null, type);
            target.Container.Should().BeNull();
            target.Type.Should().Be(type);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_type()
         {
            target = new ContainerTypeEventArgs(container, null);
            target.Container.Should().Be(container);
            target.Type.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_provided_container_and_type()
         {
            target = new ContainerTypeEventArgs(container, type);
            target.Container.Should().Be(container);
            target.Type.Should().Be(type);
         }

         public void It_should_accept_both_a_null_container_and_type()
         {
            target = new ContainerTypeEventArgs(null, null);
            target.Container.Should().BeNull();
            target.Type.Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_create_an_ContainerTypeEventArgs_using_the_default_constructor : ArrangeActAssert
      {
         private ContainerTypeEventArgs target;

         protected override void ArrangeMethod()
         {
            target = new ContainerTypeEventArgs();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_Container()
         {
            target.Container.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_Type()
         {
            target.Type.Should().BeNull();
         }
      }

      private interface IType
      {
      }
   }
}
