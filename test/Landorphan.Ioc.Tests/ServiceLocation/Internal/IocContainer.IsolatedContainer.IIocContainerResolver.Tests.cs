namespace Landorphan.Ioc.Tests.ServiceLocation.Internal
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class IocContainer_IsolatedContainer_Tests
   {
      // TryResolve X 4

      [TestClass]
      public class When_I_have_an_isolated_container_and_call_Resolve : DisposableArrangeActAssert
      {
         private readonly String containerName = "Isolated Test Container: IIocContainerResolver.Resolve Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private RegisteredTypeImplementingIRegisteredType defaultInstance;
         private RegisteredTypeImplementingIRegisteredType namedInstance;
         private String registeredName;
         private IIocContainerRegistrar registrar;
         private IIocContainerResolver target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            target = container.Resolver;
            registrar = container.Registrar;

            defaultInstance = new RegisteredTypeImplementingIRegisteredType();
            namedInstance = new RegisteredTypeImplementingIRegisteredType();
            registeredName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            // create a "default" registration
            registrar.RegisterInstance<IRegisteredType>(defaultInstance);

            // create a named registration.
            registrar.RegisterInstance<IRegisteredType>(registeredName, namedInstance);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_Resolve_a_concrete_type()
         {
            // the type system does not allow restriction to interfaces and abstract types,
            // only to reference types (where T: class)

            Action throwingAction = () => target.Resolve<ConcreteClass>();
            var e = throwingAction.Should()
               .Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>()
               .WithMessage("*service location only supports the location of interfaces and abstract types*");
            e.And.ActualType.Should().Be<ConcreteClass>();
            e.And.ParamName.Should().Be("TFrom");

            throwingAction = () => target.Resolve<ConcreteClass>(registeredName);
            e = throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>().WithMessage("*service location only supports the location of interfaces and abstract types*");
            e.And.ActualType.Should().Be<ConcreteClass>();
            e.And.ParamName.Should().Be("TFrom");

            throwingAction = () => target.Resolve(typeof(ConcreteClass));
            e = throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>().WithMessage("*service location only supports the location of interfaces and abstract types*");
            e.And.ActualType.Should().Be<ConcreteClass>();
            e.And.ParamName.Should().Be("fromType");

            throwingAction = () => target.Resolve(typeof(ConcreteClass), registeredName);
            e = throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>().WithMessage("*service location only supports the location of interfaces and abstract types*");
            e.And.ActualType.Should().Be<ConcreteClass>();
            e.And.ParamName.Should().Be("fromType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_Resolve_a_null()
         {
            // blocked by type system. target.Resolve<null>();
            // blocked by type system. target.Resolve<null>(registeredName);

            Action throwingAction = () => target.Resolve(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("fromType");

            throwingAction = () => target.Resolve(null, registeredName);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("fromType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_Resolve_a_PrecludedType()
         {
            target.Manager.AddPrecludedType<AbstractRegisteredType>();

            Action throwingAction = () => target.Resolve<AbstractRegisteredType>();
            var e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>().WithMessage("*precludes the registration of type*");
            e.And.PrecludedType.Should().Be<AbstractRegisteredType>();
            e.And.ParamName.Should().Be("TFrom");
            e.And.Container.Should().Be(container);

            throwingAction = () => target.Resolve<AbstractRegisteredType>(registeredName);
            e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>().WithMessage("*precludes the registration of type*");
            e.And.PrecludedType.Should().Be<AbstractRegisteredType>();
            e.And.ParamName.Should().Be("TFrom");
            e.And.Container.Should().Be(container);

            throwingAction = () => target.Resolve(typeof(AbstractRegisteredType));
            e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>().WithMessage("*precludes the registration of type*");
            e.And.PrecludedType.Should().Be<AbstractRegisteredType>();
            e.And.ParamName.Should().Be("fromType");
            e.And.Container.Should().Be(container);

            throwingAction = () => target.Resolve(typeof(AbstractRegisteredType), registeredName);
            e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>().WithMessage("*precludes the registration of type*");
            e.And.PrecludedType.Should().Be<AbstractRegisteredType>();
            e.And.ParamName.Should().Be("fromType");
            e.And.Container.Should().Be(container);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_Resolve_an_open_generic()
         {
            // blocked by type system. target.Resolve<IList<>>();
            // blocked by type system. target.Resolve<IList<>>(registeredName);

            Action throwingAction = () => target.Resolve(typeof(IList<>));
            var e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>().WithMessage("The given type '*' must not be an open generic, but is an open generic*");
            e.And.ParamName.Should().Be("fromType");
            e.And.ActualType.Should().Be(typeof(IList<>));

            throwingAction = () => target.Resolve(typeof(IList<>), registeredName);
            e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>().WithMessage("The given type '*' must not be an open generic, but is an open generic*");
            e.And.ParamName.Should().Be("fromType");
            e.And.ActualType.Should().Be(typeof(IList<>));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_there_is_no_matching_default_registration()
         {
            // remove the default registration.
            registrar.Unregister<IRegisteredType>();

            // requesting the default registration should fail.
            Action throwingAction = () => target.Resolve<IRegisteredType>();
            var e = throwingAction.Should().Throw<ResolutionException>().WithMessage("Unable to resolve the requested type/name:*");
            e.And.RequestedType.Should().Be<IRegisteredType>();
            e.And.Name.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_there_is_no_matching_named_registration()
         {
            // remove the named registration.
            registrar.Unregister<IRegisteredType>(registeredName);

            // requesting the default registration should fail.
            Action throwingAction = () => target.Resolve<IRegisteredType>(registeredName);
            var e = throwingAction.Should().Throw<ResolutionException>().WithMessage("Unable to resolve the requested type/name:*");
            e.And.RequestedType.Should().Be<IRegisteredType>();
            e.And.Name.Should().Be(registeredName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_instantiate_an_instance_when_I_register_an_implementation_type_and_call_Resolve()
         {
            // Arrange the state for this method

            // ..remove the arranged registrations.
            target.Registrar.Unregister<IRegisteredType>();
            target.Registrar.Unregister<IRegisteredType>(registeredName);
            target.Registrar.Registrations.Count.Should().Be(0);

            // .. create a default registration with an implementation type
            target.Registrar.RegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>();

            // .. create a named registration with an implementation type
            target.Registrar.RegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(registeredName);

            var actualDefaultInstance = target.Resolve<IRegisteredType>();
            actualDefaultInstance.Should().NotBeNull();

            var actualNamedInstance = target.Resolve<IRegisteredType>(registeredName);
            actualNamedInstance.Should().NotBeNull();

            ReferenceEquals(actualDefaultInstance, actualNamedInstance).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Resolve_a_default_registration()
         {
            var actualInstance = target.Resolve<IRegisteredType>();
            actualInstance.Should().Be(defaultInstance);

            actualInstance = (IRegisteredType)target.Resolve(typeof(IRegisteredType));
            actualInstance.Should().Be(defaultInstance);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Resolve_a_named_registration()
         {
            var actualInstance = target.Resolve<IRegisteredType>(registeredName);
            actualInstance.Should().Be(namedInstance);

            actualInstance = (IRegisteredType)target.Resolve(typeof(IRegisteredType), registeredName);
            actualInstance.Should().Be(namedInstance);
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_and_call_TryResolve : DisposableArrangeActAssert
      {
         private readonly String containerName = "Isolated Test Container: IIocContainerResolver.TryResolve Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private RegisteredTypeImplementingIRegisteredType defaultInstance;
         private RegisteredTypeImplementingIRegisteredType namedInstance;
         private String registeredName;
         private IIocContainerRegistrar registrar;
         private IIocContainerResolver target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            target = container.Resolver;
            registrar = container.Registrar;

            defaultInstance = new RegisteredTypeImplementingIRegisteredType();
            namedInstance = new RegisteredTypeImplementingIRegisteredType();
            registeredName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            // create a "default" registration
            registrar.RegisterInstance<IRegisteredType>(defaultInstance);

            // create a named registration.
            registrar.RegisterInstance<IRegisteredType>(registeredName, namedInstance);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S1854: Dead stores should be removed", Justification = "By design (MWP)")]
         public void It_should_return_false_when_I_attempt_TryResolve_on_a_concrete_type()
         {
            // the type system does not allow restriction to interfaces and abstract types,
            // only to reference types (where T: class)
            var name = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            // ReSharper disable once RedundantAssignment
            // ReSharper disable once InlineOutVariableDeclaration
            var instance = new ConcreteClass();
            var actual = target.TryResolve(out instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            // ReSharper disable once RedundantAssignment
            // ReSharper disable once InlineOutVariableDeclaration
            instance = new ConcreteClass();
            actual = target.TryResolve(name, out instance);
            actual.Should().BeFalse();
            instance.Should().BeNull();

            // ReSharper disable once RedundantAssignment
            // ReSharper disable once InlineOutVariableDeclaration
            var obj = new Object();
            actual = target.TryResolve(typeof(ConcreteClass), out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            // ReSharper disable once RedundantAssignment
            // ReSharper disable once InlineOutVariableDeclaration
            obj = new Object();
            actual = target.TryResolve(typeof(ConcreteClass), name, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S1854: Dead stores should be removed", Justification = "By design (MWP)")]
         public void It_should_return_false_when_I_attempt_TryResolve_on_a_null()
         {
            // blocked by type system. target.TryResolve<null>(out foo);
            // blocked by type system. target.TryResolve<null>(registeredName, out foo);

            var name = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            // ReSharper disable once RedundantAssignment
            // ReSharper disable once InlineOutVariableDeclaration
            var obj = new Object();
            var actual = target.TryResolve((Type)null, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            // ReSharper disable once RedundantAssignment
            // ReSharper disable once InlineOutVariableDeclaration
            obj = new Object();
            actual = target.TryResolve(null, name, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S1854: Dead stores should be removed", Justification = "By design (MWP)")]
         public void It_should_return_false_when_I_attempt_TryResolve_on_a_PrecludedType()
         {
            target.Manager.AddPrecludedType<AbstractRegisteredType>();
            var name = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            var actual = target.TryResolve<AbstractRegisteredType>(out var instance0);
            actual.Should().BeFalse();
            instance0.Should().BeNull();

            actual = target.TryResolve<AbstractRegisteredType>(name, out var instance1);
            actual.Should().BeFalse();
            instance1.Should().BeNull();

            // ReSharper disable once RedundantAssignment
            // ReSharper disable once InlineOutVariableDeclaration
            var obj = new Object();
            actual = target.TryResolve(typeof(AbstractRegisteredType), out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();

            // ReSharper disable once RedundantAssignment
            // ReSharper disable once InlineOutVariableDeclaration
            obj = new Object();
            actual = target.TryResolve(typeof(AbstractRegisteredType), name, out obj);
            actual.Should().BeFalse();
            obj.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S1854: Dead stores should be removed", Justification = "By design (MWP)")]
         // ReSharper disable InlineOutVariableDeclaration
         // ReSharper disable RedundantAssignment
         public void It_should_return_false_when_I_attempt_TryResolve_on_an_open_generic()
         {
            // blocked by type system. target.TryResolve<IList<>>(out var instance);
            // blocked by type system. target.TryResolve<IList<>>(registeredName, out var instance);

            var name = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            var actual = target.TryResolve(typeof(IList<>), out var instance0);
            actual.Should().BeFalse();
            instance0.Should().BeNull();

            actual = target.TryResolve(typeof(IList<>), name, out var instance1);
            actual.Should().BeFalse();
            instance1.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_I_attempt_TryResolve_and_there_is_no_matching_default_registration()
         {
            // remove the default registration.
            registrar.Unregister<IRegisteredType>();

            var actual = target.TryResolve<IRegisteredType>(out var instance0);
            actual.Should().BeFalse();
            instance0.Should().BeNull();

            actual = target.TryResolve(typeof(IRegisteredType), out var instance1);
            actual.Should().BeFalse();
            instance1.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_I_attempt_TryResolve_and_there_is_no_matching_named_registration()
         {
            // remove the named registration.
            registrar.Unregister<IRegisteredType>(registeredName);

            var actual = target.TryResolve<IRegisteredType>(registeredName, out var instance0);
            actual.Should().BeFalse();
            instance0.Should().BeNull();

            actual = target.TryResolve(typeof(IRegisteredType), registeredName, out var instance1);
            actual.Should().BeFalse();
            instance1.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_I_attempt_TryResolve_and_there_is_a_matching_default_registration()
         {
            var actualInstance = target.Resolve<IRegisteredType>();
            actualInstance.Should().Be(defaultInstance);

            actualInstance = (IRegisteredType)target.Resolve(typeof(IRegisteredType));
            actualInstance.Should().Be(defaultInstance);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_I_attempt_TryResolve_and_there_is_a_matching_named_registration()
         {
            var actualInstance = target.Resolve<IRegisteredType>(registeredName);
            actualInstance.Should().Be(namedInstance);

            actualInstance = (IRegisteredType)target.Resolve(typeof(IRegisteredType), registeredName);
            actualInstance.Should().Be(namedInstance);
         }
      }
   }
}
