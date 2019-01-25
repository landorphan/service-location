namespace Landorphan.Ioc.Tests.ServiceLocation
{
   using System;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class IocServiceLocator_IIocServiceLocatorManager_Tests
   {
      [TestClass]
      public class When_I_have_an_IIocServiceLocatorManager : ArrangeActAssert
      {
         private IIocContainer childContainer;
         private IIocServiceLocatorManager target;
         private IIocServiceLocator targetInstance;

         protected override void TeardownTestMethod()
         {
            IocServiceLocator.Instance.Manager.RootContainer.Registrar.Unregister<IMyService>();
            IocServiceLocator.Instance.Manager.RootContainer.Manager.RemovePrecludedType(typeof(IPrecludedService));
            (childContainer as IDisposable)?.Dispose();

            base.TeardownTestMethod();
         }

         protected override void ArrangeMethod()
         {
            targetInstance = IocServiceLocator.Instance;
            target = targetInstance.Manager;
            childContainer = target.RootContainer.Manager.CreateChildContainer("Child Container");
            IocServiceLocator.Instance.Manager.SetAmbientContainer(childContainer);

            IocServiceLocator.Instance.Manager.RootContainer.Registrar.RegisterImplementation<IMyService, MyService>();
            IocServiceLocator.Instance.Manager.RootContainer.Manager.AddPrecludedType(typeof(IPrecludedService));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_set_the_ambient_container()
         {
            Object actualSender = null;
            System.EventArgs actualEventArgs = null;
            var eh = new EventHandler<System.EventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.AmbientContainerChanged += eh;

            target.SetAmbientContainer(target.RootContainer);
            ReferenceEquals(target.RootContainer, target.AmbientContainer).Should().BeTrue();
            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);
            actualEventArgs.Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_root_container()
         {
            target.RootContainer.Should().NotBeNull();
            ReferenceEquals(target.RootContainer, target.AmbientContainer).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_an_ambient_container()
         {
            target.AmbientContainer.Should().NotBeNull();
            ReferenceEquals(target.RootContainer, target.AmbientContainer).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_ambient_container_to_the_root_container_when_I_dispose_the_ambient_container()
         {
            ((IDisposable)childContainer).Dispose();
            ReferenceEquals(target.RootContainer, target.AmbientContainer).Should().BeTrue();
         }
      }

      private interface IMyService
      {
         Guid GetUid();
      }

      private interface IPrecludedService
      {
      }

      private class MyService : IMyService
      {
         public Guid GetUid()
         {
            return Guid.NewGuid();
         }
      }
   }
}
