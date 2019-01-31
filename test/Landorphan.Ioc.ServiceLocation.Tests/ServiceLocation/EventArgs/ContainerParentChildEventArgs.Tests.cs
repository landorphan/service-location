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

   public static class ContainerParentChildEventArgs_Tests
   {
      [TestClass]
      public class When_I_create_an_ContainerParentChildEventArgs_using_the_container_container_constructor : DisposableArrangeActAssert
      {
         private readonly String childContainerName = "Isolated Test Child";
         private readonly String containerName = "Isolated Test Parent: ContainerParentChildEventArgs (parent, child) constructor tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer child;
         private IOwnedIocContainer parent;

         private ContainerParentChildEventArgs target;

         protected override void ArrangeMethod()
         {
            parent = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            var manager = parent.Manager;
            child = (IOwnedIocContainer)manager.CreateChildContainer(childContainerName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_child()
         {
            target = new ContainerParentChildEventArgs(parent, null);
            target.Parent.Should().Be(parent);
            target.Child.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_a_null_parent()
         {
            target = new ContainerParentChildEventArgs(null, child);
            target.Parent.Should().BeNull();
            target.Child.Should().Be(child);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_provided_parent_and_child()
         {
            target = new ContainerParentChildEventArgs(parent, child);
            target.Parent.Should().Be(parent);
            target.Child.Should().Be(child);
         }

         public void It_should_accept_both_a_null_parent_and_child()
         {
            target = new ContainerParentChildEventArgs(null, null);
            target.Parent.Should().BeNull();
            target.Child.Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_create_an_ContainerParentChildEventArgs_using_the_default_constructor : ArrangeActAssert
      {
         private ContainerParentChildEventArgs target;

         protected override void ArrangeMethod()
         {
            target = new ContainerParentChildEventArgs();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_Child()
         {
            target.Child.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_Parent()
         {
            target.Parent.Should().BeNull();
         }
      }
   }
}
