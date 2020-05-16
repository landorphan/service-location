namespace Landorphan.Ioc.Tests.ServiceLocation.Internal
{
    using System;
    using FluentAssertions;
    using Landorphan.Ioc.ServiceLocation.EventArguments;
    using Landorphan.Ioc.ServiceLocation.Interfaces;
    using Landorphan.Ioc.ServiceLocation.Internal;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

   public static partial class IocContainer_IsolatedContainer_Tests
   {
       [TestClass]
      public class When_I_have_an_isolated_IIocContainer : DisposableArrangeActAssert
      {
          private readonly string containerName = "Isolated Test Container: IIocContainer Tests";
          private readonly Guid containerUid = Guid.NewGuid();
          private IOwnedIocContainer target;

          protected override void ArrangeMethod()
         {
            target = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
         }

          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_parent()
         {
            target.Parent.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_no_children()
         {
            target.Children.Should().NotBeNull();
            target.Children.Should().NotContainNulls();
            target.Children.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_indicate_IsRoot()
         {
            target.IsRoot.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_IIocContainer_and_call_CreateChildContainer_and_Dispose_of_the_child_container : DisposableArrangeActAssert
      {
          private readonly string childContainerName = "Isolated Child Test Container";
          private readonly string containerName = "Isolated Test Container: IIocContainer.Children Tests";
          private readonly Guid containerUid = Guid.NewGuid();
          private IIocContainerManager manager;
          private IOwnedIocContainer target;

          protected override void ArrangeMethod()
         {
            target = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            manager = target.Manager;
         }

          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_the_ContainerChildRemoved_event()
         {
            object actualSender = null;
            ContainerParentChildEventArgs actualEventArgs = null;
            var eh = new EventHandler<ContainerParentChildEventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            manager.ContainerChildRemoved += eh;

            var child = manager.CreateChildContainer(childContainerName);
            ((IDisposable)child).Dispose();

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);
            actualEventArgs.Should().NotBeNull();
            actualEventArgs.Parent.Should().Be(target);
            actualEventArgs.Child.Should().Be(child);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_child_when_I_call_AddChildContainer()
         {
            var child = manager.CreateChildContainer(childContainerName);
            child.Name.Should().Be(childContainerName);
            child.IsRoot.Should().BeFalse();
            child.Parent.Should().Be(target);
            child.Uid.Should().NotBe(Guid.Empty);
            child.Uid.Should().NotBe(target.Uid);
            target.Children.Should().Contain(child);
            target.Children.Count.Should().Be(1);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_remove_the_child_when_I_dispose_the_child()
         {
            var child = manager.CreateChildContainer(childContainerName);
            ((IDisposable)child).Dispose();

            target.Children.Should().BeEmpty();
         }
      }
   }
}
