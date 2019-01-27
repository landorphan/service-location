namespace Landorphan.Ioc.Tests.ServiceLocation
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using FluentAssertions;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Exceptions;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using Landorphan.Ioc.Tests.Copy.Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   // IocServiceLocator.TryResolve<>()
   // IocServiceLocator.TryResolve()

   public static class IocServiceLocator_Static_Tests
   {
      [TestClass]
      public class When_I_have_an_IocServiceLocator : ArrangeActAssert
      {
         private readonly String nonUniqueName = "Registered Name";
         private IIocContainer childContainer;
         private IMyService registeredInChildDefault;
         private IMyService registeredInChildNamed;
         private IMyService registeredInRootDefault;
         private IMyService registeredInRootNamed;

         protected override void TeardownTestMethod()
         {
            IocServiceLocator.Instance.Manager.RootContainer.Registrar.Unregister<IMyService>();
            IocServiceLocator.Instance.Manager.RootContainer.Registrar.Unregister<IMyService>(nonUniqueName);
            IocServiceLocator.Instance.Manager.RootContainer.Manager.RemovePrecludedType(typeof(IPrecludedService));
            (childContainer as IDisposable)?.Dispose();

            base.TeardownTestMethod();
         }

         protected override void ArrangeMethod()
         {
            registeredInRootDefault = new MyService("Registered In Root Default");
            registeredInRootNamed = new MyService("Registered In Root Named");
            registeredInChildDefault = new MyService("Registered In Child Default");
            registeredInChildNamed = new MyService("Registered In Child Named");

            childContainer = IocServiceLocator.RootContainer.Manager.CreateChildContainer("Child Container");
            childContainer.Registrar.RegisterInstance(registeredInChildDefault);
            childContainer.Registrar.RegisterInstance(nonUniqueName, registeredInChildNamed);

            IocServiceLocator.Instance.Manager.SetAmbientContainer(childContainer);

            IocServiceLocator.Instance.Manager.RootContainer.Registrar.RegisterInstance(registeredInRootDefault);
            IocServiceLocator.Instance.Manager.RootContainer.Registrar.RegisterInstance(nonUniqueName, registeredInRootNamed);
            IocServiceLocator.Instance.Manager.RootContainer.Manager.AddPrecludedType(typeof(IPrecludedService));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_Resolve_static_It_should_resolve_a_registered_type_name()
         {
            var actual = IocServiceLocator.Resolve<IMyService>();
            actual.GetType().Should().Be<MyService>();

            actual = IocServiceLocator.Resolve<IMyService>(nonUniqueName);
            actual.GetType().Should().Be<MyService>();

            var actualObject = IocServiceLocator.Resolve(typeof(IMyService));
            (actualObject is IMyService).Should().BeTrue();
            actualObject.GetType().Should().Be<MyService>();

            actualObject = IocServiceLocator.Resolve(typeof(IMyService), nonUniqueName);
            (actualObject is IMyService).Should().BeTrue();
            actualObject.GetType().Should().Be<MyService>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_Resolve_static_It_should_throw_when_I_attempt_to_resolve_a_concrete_type()
         {
            Action throwingAction = () => IocServiceLocator.Resolve<ConcreteClass>();
            throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();

            throwingAction = () => IocServiceLocator.Resolve<ConcreteClass>(nonUniqueName);
            throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();

            throwingAction = () => IocServiceLocator.Resolve(typeof(ConcreteClass));
            throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();

            throwingAction = () => IocServiceLocator.Resolve(typeof(ConcreteClass), nonUniqueName);
            throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_Resolve_static_It_should_throw_when_I_attempt_to_resolve_a_precluded_type()
         {
            Action throwingAction = () => IocServiceLocator.Resolve<IPrecludedService>();
            throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();

            throwingAction = () => IocServiceLocator.Resolve<IPrecludedService>(nonUniqueName);
            throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();

            throwingAction = () => IocServiceLocator.Resolve(typeof(IPrecludedService));
            throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();

            throwingAction = () => IocServiceLocator.Resolve(typeof(IPrecludedService), nonUniqueName);
            throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_Resolve_static_It_should_throw_when_I_attempt_to_resolve_an_open_generic_type()
         {
            // barred by type system: target.Resolve<IList<>>();
            // barred by type system: target.Resolve<IList<>>(name);

            Action throwingAction = () => IocServiceLocator.Resolve(typeof(List<>));
            throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();

            throwingAction = () => IocServiceLocator.Resolve(typeof(List<>), nonUniqueName);
            throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_Resolve_static_It_should_throw_when_I_attempt_to_resolve_an_unmatched_type_and_or_name()
         {
            var randomName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            Action throwingAction = () => IocServiceLocator.Resolve<IUnregisteredService>();
            throwingAction.Should().Throw<ResolutionException>();

            throwingAction = () => IocServiceLocator.Resolve<IMyService>(randomName);
            throwingAction.Should().Throw<ResolutionException>();

            throwingAction = () => IocServiceLocator.Resolve(typeof(IUnregisteredService));
            throwingAction.Should().Throw<ResolutionException>();

            throwingAction = () => IocServiceLocator.Resolve(typeof(IMyService), randomName);
            throwingAction.Should().Throw<ResolutionException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_Resolve_static_It_should_use_the_ambient_container()
         {
            var container = IocServiceLocator.AmbientContainer;
            container.Name.Should().Be("Child Container");

            var actual = IocServiceLocator.Resolve<IMyService>();
            actual.GetType().Should().Be<MyService>();
            ReferenceEquals(actual, registeredInChildDefault).Should().BeTrue();

            actual = IocServiceLocator.Resolve<IMyService>(nonUniqueName);
            actual.GetType().Should().Be<MyService>();
            ReferenceEquals(actual, registeredInChildNamed).Should().BeTrue();

            var actualObject = IocServiceLocator.Resolve(typeof(IMyService));
            (actualObject is IMyService).Should().BeTrue();
            actualObject.GetType().Should().Be<MyService>();
            ReferenceEquals(actualObject, registeredInChildDefault).Should().BeTrue();

            actualObject = IocServiceLocator.Resolve(typeof(IMyService), nonUniqueName);
            (actualObject is IMyService).Should().BeTrue();
            actualObject.GetType().Should().Be<MyService>();
            ReferenceEquals(actualObject, registeredInChildNamed).Should().BeTrue();

            ((IDisposable)childContainer).Dispose();

            actual = IocServiceLocator.Resolve<IMyService>();
            actual.GetType().Should().Be<MyService>();
            ReferenceEquals(actual, registeredInRootDefault).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_TryResolve_static_It_should_not_throw_when_I_attempt_to_resolve_a_concrete_type()
         {
            var actual = IocServiceLocator.TryResolve(out ConcreteClass instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            actual = IocServiceLocator.TryResolve(nonUniqueName, out instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            actual = IocServiceLocator.TryResolve(typeof(ConcreteClass), out var obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            actual = IocServiceLocator.TryResolve(typeof(ConcreteClass), nonUniqueName, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_TryResolve_static_It_should_not_throw_when_I_attempt_to_resolve_a_precluded_type()
         {
            var actual = IocServiceLocator.TryResolve(out IPrecludedService instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            actual = IocServiceLocator.TryResolve(nonUniqueName, out instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            IocServiceLocator.TryResolve(typeof(IPrecludedService), out var obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            actual = IocServiceLocator.TryResolve(typeof(IPrecludedService), nonUniqueName, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_TryResolve_static_It_should_not_throw_when_I_attempt_to_resolve_an_open_generic_type()
         {
            // barred by type system: target.TryResolve<IList<>>();
            // barred by type system: target.TryResolve<IList<>>(name);

            var actual = IocServiceLocator.TryResolve(typeof(List<>), out var obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            actual = IocServiceLocator.TryResolve(typeof(List<>), nonUniqueName, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_TryResolve_static_It_should_not_throw_when_I_attempt_to_resolve_an_unmatched_type_and_or_name()
         {
            var randomName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            var actual = IocServiceLocator.TryResolve<IUnregisteredService>(out var instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            actual = IocServiceLocator.TryResolve<IMyService>(randomName, out var instance2);
            actual.Should().BeFalse();
            instance2.Should().BeNull();

            actual = IocServiceLocator.TryResolve(typeof(IUnregisteredService), out var obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            actual = IocServiceLocator.TryResolve(typeof(IMyService), randomName, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_call_TryResolve_static_It_should_resolve_a_registered_type_name()
         {
            var actual = IocServiceLocator.TryResolve<IMyService>(out var instance);
            actual.Should().BeTrue();
            instance.GetType().Should().Be<MyService>();

            actual = IocServiceLocator.TryResolve(nonUniqueName, out instance);
            actual.Should().BeTrue();
            instance.GetType().Should().Be<MyService>();

            actual = IocServiceLocator.TryResolve(typeof(IMyService), out var obj);
            actual.Should().BeTrue();
            (obj is IMyService).Should().BeTrue();
            obj.GetType().Should().Be<MyService>();

            actual = IocServiceLocator.TryResolve(typeof(IMyService), nonUniqueName, out obj);
            actual.Should().BeTrue();
            (obj is IMyService).Should().BeTrue();
            obj.GetType().Should().Be<MyService>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_root_container_static()
         {
            IocServiceLocator.RootContainer.Should().NotBeNull();
            ReferenceEquals(IocServiceLocator.RootContainer, IocServiceLocator.AmbientContainer).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_singleton_instance_static()
         {
            IocServiceLocator.Instance.Should().NotBeNull();
            IocServiceLocator.Instance.Should().BeAssignableTo<IIocServiceLocator>();
            IocServiceLocator.Instance.Should().BeOfType<IocServiceLocator>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_an_ambient_container_static()
         {
            IocServiceLocator.AmbientContainer.Should().NotBeNull();
            ReferenceEquals(IocServiceLocator.RootContainer, IocServiceLocator.AmbientContainer).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_set_the_ambient_container_to_the_root_container_when_I_dispose_the_ambient_container()
         {
            ((IDisposable)childContainer).Dispose();
            ReferenceEquals(IocServiceLocator.RootContainer, IocServiceLocator.AmbientContainer).Should().BeTrue();
         }
      }

      private class ConcreteClass
      {
      }

      private interface IMyService
      {
         String GeName();
      }

      private interface IPrecludedService
      {
      }

      private interface IUnregisteredService
      {
      }

      private class MyService : IMyService
      {
         private readonly String _name;

         public MyService(String name)
         {
            _name = name.TrimNullToEmpty();
         }

         public String GeName()
         {
            return _name;
         }
      }
   }
}
