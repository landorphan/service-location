namespace Landorphan.Ioc.Tests.ServiceLocation
{
   using System;
   using System.Collections.Generic;
   using System.Globalization;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Exceptions;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   public static class IocServiceLocator_IIocServiceLocator_Tests
   {
      [TestClass]
      public class When_I_call_IocServiceLocator_GetService : ArrangeActAssert
      {
         private IIocServiceLocator target;
         private IServiceProvider targetAsIServiceProvider;

         protected override void TeardownTestMethod()
         {
            IocServiceLocator.AmbientContainer.Registrar.Unregister<IMyService>();
            IocServiceLocator.AmbientContainer.Manager.RemovePrecludedType(typeof(IPrecludedService));

            base.TeardownTestMethod();
         }

         protected override void ArrangeMethod()
         {
            target = IocServiceLocator.Instance;
            targetAsIServiceProvider = target;

            IocServiceLocator.AmbientContainer.Registrar.RegisterImplementation<IMyService, MyService>();
            IocServiceLocator.AmbientContainer.Manager.AddPrecludedType(typeof(IPrecludedService));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_resolve_and_not_throw_when_I_as_for_an_unregistered_type()
         {
            var actual = targetAsIServiceProvider.GetService(typeof(IUnregisteredService));
            actual.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_resolve_and_not_throw_when_I_ask_for_a_concrete_type()
         {
            var actual = targetAsIServiceProvider.GetService(typeof(ConcreteClass));
            actual.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_resolve_and_not_throw_when_I_ask_for_a_precluded_type()
         {
            var actual = targetAsIServiceProvider.GetService(typeof(IPrecludedService));
            actual.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_resolve_and_not_throw_when_I_ask_for_an_open_generic_type()
         {
            var actual = targetAsIServiceProvider.GetService(typeof(List<>));
            actual.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_resolve_a_registered_type()
         {
            var actual = targetAsIServiceProvider.GetService(typeof(IMyService));
            (actual is IMyService).Should().BeTrue();
            actual.GetType().Should().Be<MyService>();
         }
      }

      [TestClass]
      public class When_I_call_IocServiceLocator_Resolve : ArrangeActAssert
      {
         private readonly String registeredName = "My registered name";
         private IIocServiceLocator target;

         protected override void TeardownTestMethod()
         {
            IocServiceLocator.AmbientContainer.Registrar.Unregister<IMyService>();
            IocServiceLocator.AmbientContainer.Registrar.Unregister<IMyService>(registeredName);
            IocServiceLocator.AmbientContainer.Manager.RemovePrecludedType(typeof(IPrecludedService));

            base.TeardownTestMethod();
         }

         protected override void ArrangeMethod()
         {
            target = IocServiceLocator.Instance;

            IocServiceLocator.AmbientContainer.Registrar.RegisterImplementation<IMyService, MyService>();
            IocServiceLocator.AmbientContainer.Registrar.RegisterImplementation<IMyService, MyService>(registeredName);
            IocServiceLocator.AmbientContainer.Manager.AddPrecludedType(typeof(IPrecludedService));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_resolve_a_registered_type_name()
         {
            var actual = target.Resolve<IMyService>();
            actual.GetType().Should().Be<MyService>();

            actual = target.Resolve<IMyService>(registeredName);
            actual.GetType().Should().Be<MyService>();

            var actualObject = target.Resolve(typeof(IMyService));
            (actualObject is IMyService).Should().BeTrue();
            actualObject.GetType().Should().Be<MyService>();

            actualObject = target.Resolve(typeof(IMyService), registeredName);
            (actualObject is IMyService).Should().BeTrue();
            actualObject.GetType().Should().Be<MyService>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_I_attempt_to_resolve_a_concrete_type()
         {
            Action throwingAction = () => target.Resolve<ConcreteClass>();
            throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();

            throwingAction = () => target.Resolve<ConcreteClass>(registeredName);
            throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();

            throwingAction = () => target.Resolve(typeof(ConcreteClass));
            throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();

            throwingAction = () => target.Resolve(typeof(ConcreteClass), registeredName);
            throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_I_attempt_to_resolve_a_precluded_type()
         {
            Action throwingAction = () => target.Resolve<IPrecludedService>();
            throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();

            throwingAction = () => target.Resolve<IPrecludedService>(registeredName);
            throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();

            throwingAction = () => target.Resolve(typeof(IPrecludedService));
            throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();

            throwingAction = () => target.Resolve(typeof(IPrecludedService), registeredName);
            throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_I_attempt_to_resolve_an_open_generic_type()
         {
            // barred by type system: target.Resolve<IList<>>();
            // barred by type system: target.Resolve<IList<>>(name);

            Action throwingAction = () => target.Resolve(typeof(List<>));
            throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();

            throwingAction = () => target.Resolve(typeof(List<>), registeredName);
            throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_I_attempt_to_resolve_an_unmatched_type_and_or_name()
         {
            var randomName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            Action throwingAction = () => target.Resolve<IUnregisteredService>();
            throwingAction.Should().Throw<ResolutionException>();

            throwingAction = () => target.Resolve<IMyService>(randomName);
            throwingAction.Should().Throw<ResolutionException>();

            throwingAction = () => target.Resolve(typeof(IUnregisteredService));
            throwingAction.Should().Throw<ResolutionException>();

            throwingAction = () => target.Resolve(typeof(IMyService), randomName);
            throwingAction.Should().Throw<ResolutionException>();
         }
      }

      [TestClass]
      public class When_I_call_IocServiceLocator_TryResolve : ArrangeActAssert
      {
         private readonly String registeredName = "My registered name";
         private IIocServiceLocator target;

         protected override void TeardownTestMethod()
         {
            IocServiceLocator.AmbientContainer.Registrar.Unregister<IMyService>();
            IocServiceLocator.AmbientContainer.Registrar.Unregister<IMyService>(registeredName);
            IocServiceLocator.AmbientContainer.Manager.RemovePrecludedType(typeof(IPrecludedService));

            base.TeardownTestMethod();
         }

         protected override void ArrangeMethod()
         {
            target = IocServiceLocator.Instance;

            IocServiceLocator.AmbientContainer.Registrar.RegisterImplementation<IMyService, MyService>();
            IocServiceLocator.AmbientContainer.Registrar.RegisterImplementation<IMyService, MyService>(registeredName);
            IocServiceLocator.AmbientContainer.Manager.AddPrecludedType(typeof(IPrecludedService));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_I_attempt_to_resolve_a_concrete_type()
         {
            var actual = target.TryResolve(out ConcreteClass instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            actual = target.TryResolve(registeredName, out instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            actual = target.TryResolve(typeof(ConcreteClass), out var obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            actual = target.TryResolve(typeof(ConcreteClass), registeredName, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_I_attempt_to_resolve_a_precluded_type()
         {
            var actual = target.TryResolve(out IPrecludedService instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            actual = target.TryResolve(registeredName, out instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            target.TryResolve(typeof(IPrecludedService), out var obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            actual = target.TryResolve(typeof(IPrecludedService), registeredName, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_I_attempt_to_resolve_an_open_generic_type()
         {
            // barred by type system: target.TryResolve<IList<>>();
            // barred by type system: target.TryResolve<IList<>>(name);

            var actual = target.TryResolve(typeof(List<>), out var obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            actual = target.TryResolve(typeof(List<>), registeredName, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_I_attempt_to_resolve_an_unmatched_type_and_or_name()
         {
            var randomName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            var actual = target.TryResolve<IUnregisteredService>(out var instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            actual = target.TryResolve<IMyService>(randomName, out var instance2);
            actual.Should().BeFalse();
            instance2.Should().BeNull();

            actual = target.TryResolve(typeof(IUnregisteredService), out var obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            actual = target.TryResolve(typeof(IMyService), randomName, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_resolve_a_registered_type_name()
         {
            var actual = target.TryResolve<IMyService>(out var instance);
            actual.Should().BeTrue();
            instance.GetType().Should().Be<MyService>();

            actual = target.TryResolve(registeredName, out instance);
            actual.Should().BeTrue();
            instance.GetType().Should().Be<MyService>();

            actual = target.TryResolve(typeof(IMyService), out var obj);
            actual.Should().BeTrue();
            (obj is IMyService).Should().BeTrue();
            obj.GetType().Should().Be<MyService>();

            actual = target.TryResolve(typeof(IMyService), registeredName, out obj);
            actual.Should().BeTrue();
            (obj is IMyService).Should().BeTrue();
            obj.GetType().Should().Be<MyService>();
         }
      }

      private class ConcreteClass
      {
      }

      private interface IMyService
      {
         Guid GetUid();
      }

      private interface IPrecludedService
      {
      }

      private interface IUnregisteredService
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
