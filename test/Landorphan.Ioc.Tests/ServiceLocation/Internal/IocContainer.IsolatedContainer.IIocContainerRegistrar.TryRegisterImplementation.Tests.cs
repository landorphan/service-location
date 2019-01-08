namespace Landorphan.Ioc.Tests.ServiceLocation.Internal
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static partial class IocContainer_IsolatedContainer_Tests
   {
      [TestClass]
      public class When_I_have_an_isolated_container_and_call_TryRegisterImplementation : DisposableArrangeActAssert
      {
         // tests of RegisterImplementation that do not depend on the configuration state

         private readonly String containerName = "Isolated Test Container: TryRegisterImplementation Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private IIocContainerRegistrar target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            target = container.Registrar;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_with_a_null_name_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            var actual = target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(null);
            actual.Should().BeTrue();

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(String.Empty);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_with_a_null_name_non_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            var actual = target.TryRegisterImplementation(fromType, null, toType);
            actual.Should().BeTrue();

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(String.Empty);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_with_a_whitespace_name()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            var actual = target.TryRegisterImplementation(fromType, Whitespace, toType);
            actual.Should().BeTrue();

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(String.Empty);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_with_a_whitespace_name_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            var actual = target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(Whitespace);
            actual.Should().BeTrue();

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(String.Empty);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_with_an_empty_name()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            var actual = target.TryRegisterImplementation(fromType, String.Empty, toType);
            actual.Should().BeTrue();

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(String.Empty);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_with_an_empty_name_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            var actual = target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(String.Empty);
            actual.Should().BeTrue();

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(String.Empty);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_without_a_name()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            var actual = target.TryRegisterImplementation(fromType, toType);
            actual.Should().BeTrue();

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(String.Empty);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_without_a_name_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            var actual = target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>();
            actual.Should().BeTrue();

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(String.Empty);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_fromType_that_is_an_open_generic()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            var actual = target.TryRegisterImplementation(typeof(IList<>), typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IList<>), null, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IList<>), String.Empty, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IList<>), registeredName, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_fromType_that_is_concrete()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            var actual = target.TryRegisterImplementation(typeof(ConcreteClass), typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(ConcreteClass), null, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(ConcreteClass), String.Empty, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(ConcreteClass), registeredName, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_fromType_that_is_null()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            var actual = target.TryRegisterImplementation(null, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(null, null, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(null, String.Empty, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(null, registeredName, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_toType_that_does_not_have_a_public_default_ctor()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            // target.RegisterImplementation<IRegisteredType, RegisteredTypeWithoutDefaultPublicCtor>(); is barred by compiler due to type constraints.
            // target.RegisterImplementation<IRegisteredType, RegisteredTypeWithoutDefaultPublicCtor>(name); is barred by compiler due to type constraints.

            var actual = target.TryRegisterImplementation(typeof(IRegisteredType), typeof(RegisteredTypeWithoutDefaultPublicCtor));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), null, typeof(RegisteredTypeWithoutDefaultPublicCtor));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), String.Empty, typeof(RegisteredTypeWithoutDefaultPublicCtor));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), registeredName, typeof(RegisteredTypeWithoutDefaultPublicCtor));
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_toType_that_does_not_implement_the_fromType()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            // the compiler prevents:
            // target.RegisterImplementation<IRegisteredType, ConcreteClass>();
            // target.RegisterImplementation<IRegisteredType, ConcreteClass>(registeredName);
            var actual = target.TryRegisterImplementation(typeof(IRegisteredType), typeof(ConcreteClass));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), null, typeof(ConcreteClass));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), String.Empty, typeof(ConcreteClass));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), registeredName, typeof(ConcreteClass));
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_toType_that_is_an_abstract_type()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            // target.RegisterImplementation<IRegisteredType, AbstractRegisteredType>(); is barred by compiler due to type constraints.
            // target.RegisterImplementation<IRegisteredType, AbstractRegisteredType>(name); is barred by compiler due to type constraints.

            Action throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), typeof(AbstractRegisteredType));
            var e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), null, typeof(AbstractRegisteredType));
            e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), String.Empty, typeof(AbstractRegisteredType));
            e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), registeredName, typeof(AbstractRegisteredType));
            e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>();
            e.And.ParamName.Should().Be("toType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_toType_that_is_an_interface_type()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            // target.RegisterImplementation<IRegisteredType, IRegisteredType>(); is barred by compiler due to type constraints.
            // target.RegisterImplementation<IRegisteredType, IRegisteredType>(name); is barred by compiler due to type constraints.

            Action throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), typeof(IRegisteredType));
            var e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), null, typeof(IRegisteredType));
            e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), String.Empty, typeof(IRegisteredType));
            e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), registeredName, typeof(IRegisteredType));
            e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>();
            e.And.ParamName.Should().Be("toType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_toType_that_is_an_open_generic()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            // The type system prevents usage of open generic types in function calls:
            // e.g. target.RegisterImplementation<IRegisteredType,IList<>>() is flagged
            // when passing types using Type, must check manually.
            // This test verifies those manual validations in production code.
            Action throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), typeof(IList<>));
            var e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), null, typeof(IList<>));
            e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), String.Empty, typeof(IList<>));
            e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), registeredName, typeof(IList<>));
            e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
            e.And.ParamName.Should().Be("toType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_toType_that_is_null()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            Action throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), null, null);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), String.Empty, null);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("toType");

            throwingAction = () => target.RegisterImplementation(typeof(IRegisteredType), registeredName, null);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("toType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_the_ContainerRegistrationAdded_event_generic()
         {
            Object actualSender = null;
            ContainerTypeRegistrationEventArgs actualEventArgs = null;

            var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ContainerRegistrationAdded += eh;
            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>();

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);
            actualEventArgs.Should().NotBeNull();
            actualEventArgs.Container.Should().Be(container);
            actualEventArgs.FromType.Should().Be<IRegisteredType>();
            actualEventArgs.ToType.Should().Be<RegisteredTypeImplementingIRegisteredType>();
            actualEventArgs.Instance.Should().BeNull();
            actualEventArgs.Name.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_the_ContainerRegistrationAdded_event_generic_named()
         {
            Object actualSender = null;
            ContainerTypeRegistrationEventArgs actualEventArgs = null;
            var registeredName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

            var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ContainerRegistrationAdded += eh;
            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(registeredName);

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);
            actualEventArgs.Should().NotBeNull();
            actualEventArgs.Container.Should().Be(container);
            actualEventArgs.FromType.Should().Be<IRegisteredType>();
            actualEventArgs.ToType.Should().Be<RegisteredTypeImplementingIRegisteredType>();
            actualEventArgs.Instance.Should().BeNull();
            actualEventArgs.Name.Should().Be(registeredName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_the_ContainerRegistrationAdded_event_non_generic()
         {
            Object actualSender = null;
            ContainerTypeRegistrationEventArgs actualEventArgs = null;
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);
            var registeredName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
            var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ContainerRegistrationAdded += eh;
            target.TryRegisterImplementation(fromType, registeredName, toType);

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);
            actualEventArgs.Should().NotBeNull();
            actualEventArgs.Container.Should().Be(container);
            actualEventArgs.FromType.Should().Be(fromType);
            actualEventArgs.ToType.Should().Be(toType);
            actualEventArgs.Instance.Should().BeNull();
            actualEventArgs.Name.Should().Be(registeredName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_the_ContainerRegistrationAdded_event_non_generic_named()
         {
            Object actualSender = null;
            ContainerTypeRegistrationEventArgs actualEventArgs = null;
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);
            var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ContainerRegistrationAdded += eh;
            target.TryRegisterImplementation(fromType, toType);

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);
            actualEventArgs.Should().NotBeNull();
            actualEventArgs.Container.Should().Be(container);
            actualEventArgs.FromType.Should().Be(fromType);
            actualEventArgs.ToType.Should().Be(toType);
            actualEventArgs.Instance.Should().BeNull();
            actualEventArgs.Name.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_the_ContainerTypeRegistered_event_generic_no_name()
         {
            Object actualSender = null;
            ContainerTypeRegistrationEventArgs actualEventArgs = null;

            var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ContainerRegistrationAdded += eh;

            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>();

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);

            actualEventArgs.Should().NotBeNull();
            actualEventArgs.Container.Should().Be(target);
            actualEventArgs.FromType.Should().Be<IRegisteredType>();
            actualEventArgs.Instance.Should().BeNull();
            actualEventArgs.ToType.Should().Be<RegisteredTypeImplementingIRegisteredType>();
            actualEventArgs.Name.Should().NotBeNull();
            actualEventArgs.Name.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_the_ContainerTypeRegistered_event_generic_with_name()
         {
            var registeredName = Guid.NewGuid().ToString("D");
            Object actualSender = null;
            ContainerTypeRegistrationEventArgs actualEventArgs = null;

            var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ContainerRegistrationAdded += eh;

            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(registeredName);

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);

            actualEventArgs.Should().NotBeNull();
            actualEventArgs.Container.Should().Be(target);
            actualEventArgs.FromType.Should().Be<IRegisteredType>();
            actualEventArgs.Instance.Should().BeNull();
            actualEventArgs.ToType.Should().Be<RegisteredTypeImplementingIRegisteredType>();
            actualEventArgs.Name.Should().NotBeNull();
            actualEventArgs.Name.Should().Be(registeredName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_the_ContainerTypeRegistered_event_non_generic_no_name()
         {
            Object actualSender = null;
            ContainerTypeRegistrationEventArgs actualEventArgs = null;

            var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ContainerRegistrationAdded += eh;

            target.TryRegisterImplementation(typeof(IRegisteredType), typeof(RegisteredTypeImplementingIRegisteredType));

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);

            actualEventArgs.Should().NotBeNull();
            actualEventArgs.Container.Should().Be(target);
            actualEventArgs.FromType.Should().Be<IRegisteredType>();
            actualEventArgs.Instance.Should().BeNull();
            actualEventArgs.ToType.Should().Be<RegisteredTypeImplementingIRegisteredType>();
            actualEventArgs.Name.Should().NotBeNull();
            actualEventArgs.Name.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_the_ContainerTypeRegistered_event_non_generic_with_name()
         {
            var registeredName = Guid.NewGuid().ToString("D");
            Object actualSender = null;
            ContainerTypeRegistrationEventArgs actualEventArgs = null;

            var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ContainerRegistrationAdded += eh;

            target.TryRegisterImplementation(typeof(IRegisteredType), registeredName, typeof(RegisteredTypeImplementingIRegisteredType));

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);

            actualEventArgs.Should().NotBeNull();
            actualEventArgs.Container.Should().Be(target);
            actualEventArgs.FromType.Should().Be<IRegisteredType>();
            actualEventArgs.Instance.Should().BeNull();
            actualEventArgs.ToType.Should().Be<RegisteredTypeImplementingIRegisteredType>();
            actualEventArgs.Name.Should().NotBeNull();
            actualEventArgs.Name.Should().Be(registeredName);
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_AllowNamedImplementations_false_and_call_TryRegisterImplementation : DisposableArrangeActAssert
      {
         // tests of TryRegisterImplementation that depend on the configuration state:  AllowNamedImplementations == false

         private readonly String containerName = "Isolated Test Container: TryRegisterImplementation Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private IIocContainerRegistrar target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            container.Manager.Configuration.AllowNamedImplementations = false;
            target = container.Registrar;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void it_should_fail_when_I_attempt_to_register_an_implementation_with_a_name_generic()
         {
            var actual = target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>("a name");
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void it_should_fail_when_I_attempt_to_register_an_implementation_with_a_name_non_generic()
         {
            var actual = target.TryRegisterImplementation(typeof(IRegisteredType), "a name", typeof(RegisteredTypeImplementingIRegisteredType));
            actual.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_AllowNamedImplementations_true_and_call_TryRegisterImplementation : DisposableArrangeActAssert
      {
         // tests of TryRegisterImplementation that depend on the configuration state:  AllowNamedImplementations == true

         private readonly String containerName = "Isolated Test Container: TryRegisterImplementation Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private IIocContainerRegistrar target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            container.Manager.Configuration.AllowNamedImplementations = true;
            target = container.Registrar;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_multiple_times_with_different_names()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);
            var registeredName0 = String.Empty;
            var registeredName1 = Guid.NewGuid().ToString("D");
            var registeredName2 = Guid.NewGuid().ToString("D");

            // no name
            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>();
            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(registeredName1);
            target.TryRegisterImplementation(fromType, registeredName2, toType);

            var registrations = target.Registrations;
            var expectedKeys = new IRegistrationKey[]
            {
               new RegistrationKeyTypeNamePair(fromType, registeredName0),
               new RegistrationKeyTypeNamePair(fromType, registeredName1),
               new RegistrationKeyTypeNamePair(fromType, registeredName2)
            };

            registrations.Count.Should().Be(3);
            registrations.Keys.Should().OnlyContain(element => expectedKeys.Contains(element));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_with_a_name_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);
            var registeredName = Guid.NewGuid().ToString("D");

            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(registeredName);

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(registeredName);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_an_implementation_with_a_name_non_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);
            var registeredName = Guid.NewGuid().ToString("D");

            target.TryRegisterImplementation(fromType, registeredName, toType);

            var registrations = target.Registrations;
            registrations.Count.Should().Be(1);
            var kvp = registrations.First();
            var key = kvp.Key;
            var value = kvp.Value;
            key.RegisteredType.Should().Be(fromType);
            key.RegisteredName.Should().Be(registeredName);
            value.ImplementationType.Should().Be(toType);
            value.ImplementationInstance.Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_AllowPreclusionOfTypes_false_and_call_TryRegisterImplementation : DisposableArrangeActAssert
      {
         // tests of TryRegisterImplementation that depend on the configuration state:  AllowPreclusionOfTypes == false

         private readonly String containerName = "Isolated Test Container: TryRegisterImplementation Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private IIocContainerRegistrar target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            container.Manager.Configuration.AllowPreclusionOfTypes = true;
            container.Manager.AddPrecludedType<IRegisteredType>();
            container.Manager.Configuration.AllowPreclusionOfTypes = false;
            target = container.Registrar;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_a_type_that_was_previously_precluded()
         {
            // Disabling preclusion of types should clear all precluded types.

            var registeredName = Guid.NewGuid().ToString("D");

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterImplementation<IRegisteredType, AnotherRegisteredDescendingFromIRegisteredType>();
            target.IsRegistered<IRegisteredType>().Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterImplementation<IRegisteredType, AnotherRegisteredDescendingFromIRegisteredType>(registeredName);
            target.IsRegistered<IRegisteredType>(registeredName).Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterImplementation(typeof(IRegisteredType), typeof(AnotherRegisteredDescendingFromIRegisteredType));
            target.IsRegistered<IRegisteredType>().Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterImplementation(typeof(IRegisteredType), null, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            target.IsRegistered<IRegisteredType>().Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterImplementation(typeof(IRegisteredType), String.Empty, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            target.IsRegistered<IRegisteredType>().Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterImplementation(typeof(IRegisteredType), registeredName, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            target.IsRegistered<IRegisteredType>(registeredName).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_AllowPreclusionOfTypes_true_and_call_TryRegisterImplementation : DisposableArrangeActAssert
      {
         // tests of TryRegisterImplementation that depend on the configuration state:  AllowPreclusionOfTypes == true

         private readonly String containerName = "Isolated Test Container: TryRegisterImplementation Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private IIocContainerRegistrar target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            container.Manager.Configuration.AllowPreclusionOfTypes = true;
            target = container.Registrar;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_with_a_fromType_that_is_precluded()
         {
            target.Manager.AddPrecludedType<IRegisteredType>();

            var registeredName = Guid.NewGuid().ToString("D");

            var actual = target.TryRegisterImplementation<IRegisteredType, AnotherRegisteredDescendingFromIRegisteredType>();
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation<IRegisteredType, AnotherRegisteredDescendingFromIRegisteredType>(registeredName);
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), null, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), String.Empty, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterImplementation(typeof(IRegisteredType), registeredName, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_ThrowOnRegistrationCollision_false_and_call_TryRegisterImplementation : DisposableArrangeActAssert
      {
         // tests of TryRegisterImplementation that depend on the configuration state:  ThrowOnRegistrationCollision == false

         private readonly String containerName = "Isolated Test Container: TryRegisterImplementation Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private IIocContainerRegistrar target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            container.Manager.Configuration.ThrowOnRegistrationCollision = false;
            target = container.Registrar;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_generic()
         {
            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>();
            target.TryRegisterImplementation<IRegisteredType, AnotherRegisteredDescendingFromIRegisteredType>();
            var instance = target.Resolver.Resolve<IRegisteredType>();
            instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_generic_named()
         {
            var registeredName = "My registered name";
            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(registeredName);
            target.TryRegisterImplementation<IRegisteredType, AnotherRegisteredDescendingFromIRegisteredType>(registeredName);
            var instance = target.Resolver.Resolve<IRegisteredType>(registeredName);
            instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_non_generic()
         {
            target.TryRegisterImplementation(typeof(IRegisteredType), typeof(RegisteredTypeImplementingIRegisteredType));
            target.TryRegisterImplementation(typeof(IRegisteredType), typeof(AnotherRegisteredDescendingFromIRegisteredType));
            var instance = target.Resolver.Resolve<IRegisteredType>();
            instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_non_generic_named()
         {
            var registeredName = "My registered name";
            target.TryRegisterImplementation(typeof(IRegisteredType), registeredName, typeof(RegisteredTypeImplementingIRegisteredType));
            target.TryRegisterImplementation(typeof(IRegisteredType), registeredName, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            var instance = target.Resolver.Resolve<IRegisteredType>(registeredName);
            instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_ThrowOnRegistrationCollision_true_and_call_TryRegisterImplementation : DisposableArrangeActAssert
      {
         // tests of TryRegisterImplementation that depend on the configuration state:  ThrowOnRegistrationCollision == true

         private readonly String containerName = "Isolated Test Container: TryRegisterImplementation Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IOwnedIocContainer container;
         private IIocContainerRegistrar target;

         protected override void ArrangeMethod()
         {
            container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
            container.Manager.Configuration.ThrowOnRegistrationCollision = true;
            target = container.Registrar;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_again_with_the_same_empty_name_generic()
         {
            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(String.Empty);

            var actual = target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(String.Empty);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_again_with_the_same_empty_name_non_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            target.TryRegisterImplementation(fromType, null, toType);

            var actual = target.TryRegisterImplementation(fromType, String.Empty, toType);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_again_with_the_same_name_generic()
         {
            var name = "my name";
            target.Manager.Configuration.AllowNamedImplementations = true;

            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(name);

            var actual = target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(name);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_again_with_the_same_name_non_generic()
         {
            var name = "my name";
            target.Manager.Configuration.AllowNamedImplementations = true;
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            target.TryRegisterImplementation(fromType, name, toType);

            var actual = target.TryRegisterImplementation(fromType, name, toType);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_again_with_the_same_no_name_generic()
         {
            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>();

            var actual = target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>();
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_again_with_the_same_no_name_non_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            target.TryRegisterImplementation(fromType, null, toType);

            var actual = target.TryRegisterImplementation(fromType, toType);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_again_with_the_same_null_name_generic()
         {
            target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(null);

            var actual = target.TryRegisterImplementation<IRegisteredType, RegisteredTypeImplementingIRegisteredType>(null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_implementation_again_with_the_same_null_name_non_generic()
         {
            var fromType = typeof(IRegisteredType);
            var toType = typeof(RegisteredTypeImplementingIRegisteredType);

            target.TryRegisterImplementation(fromType, null, toType);

            var actual = target.TryRegisterImplementation(fromType, null, toType);
            actual.Should().BeFalse();
         }
      }
   }
}