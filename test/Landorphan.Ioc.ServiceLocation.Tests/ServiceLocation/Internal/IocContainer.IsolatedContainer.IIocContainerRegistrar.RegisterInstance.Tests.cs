namespace Landorphan.Ioc.Tests.ServiceLocation.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using FluentAssertions;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.Ioc.ServiceLocation.EventArguments;
    using Landorphan.Ioc.ServiceLocation.Exceptions;
    using Landorphan.Ioc.ServiceLocation.Interfaces;
    using Landorphan.Ioc.ServiceLocation.Internal;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

    public static partial class IocContainer_IsolatedContainer_Tests
    {
        [TestClass]
        public class When_I_have_an_isolated_container_and_call_RegisterInstance : DisposableArrangeActAssert
        {
            // tests of RegisterImplementation that do not depend on the configuration state

            private readonly string containerName = "Isolated Test Container: RegisterInstance Tests";
            private readonly Guid containerUid = Guid.NewGuid();
            private IOwnedIocContainer container;
            private IIocContainerRegistrar target;

            protected override void ArrangeMethod()
            {
                container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);

                // ensure matching the default configuration
                container.Manager.Configuration.AllowNamedImplementations = true;
                container.Manager.Configuration.AllowPreclusionOfTypes = true;
                container.Manager.Configuration.ThrowOnRegistrationCollision = false;

                target = container.Registrar;
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_an_instance_with_a_null_name_generic()
            {
                var fromType = typeof(IRegisteredType);
                var instance = new RegisteredTypeImplementingIRegisteredType();

                target.RegisterInstance<IRegisteredType>(null, instance);

                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(string.Empty);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_an_instance_with_a_null_name_non_generic()
            {
                var fromType = typeof(IRegisteredType);
                var instance = new RegisteredTypeImplementingIRegisteredType();

                target.RegisterInstance(fromType, null, instance);

                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(string.Empty);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_an_instance_with_a_whitespace_name_generic()
            {
                var fromType = typeof(IRegisteredType);
                var instance = new RegisteredTypeImplementingIRegisteredType();

                target.RegisterInstance<IRegisteredType>(Whitespace, instance);

                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(string.Empty);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_an_instance_with_a_whitespace_name_non_generic()
            {
                var fromType = typeof(IRegisteredType);
                var instance = new RegisteredTypeImplementingIRegisteredType();

                target.RegisterInstance(fromType, Whitespace, instance);
                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(string.Empty);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_an_instance_with_an_empty_name_generic()
            {
                var fromType = typeof(IRegisteredType);
                var instance = new RegisteredTypeImplementingIRegisteredType();

                target.RegisterInstance<IRegisteredType>(string.Empty, instance);

                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(string.Empty);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_an_instance_with_an_empty_name_non_generic()
            {
                var fromType = typeof(IRegisteredType);
                var instance = new RegisteredTypeImplementingIRegisteredType();

                target.RegisterInstance(fromType, string.Empty, instance);

                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(string.Empty);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_an_instance_without_a_name_generic()
            {
                var fromType = typeof(IRegisteredType);
                var instance = new RegisteredTypeImplementingIRegisteredType();

                target.RegisterInstance<IRegisteredType>(instance);

                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(string.Empty);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_an_instance_without_a_name_non_generic()
            {
                var fromType = typeof(IRegisteredType);
                var instance = new RegisteredTypeImplementingIRegisteredType();

                target.RegisterInstance(typeof(IRegisteredType), instance);

                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(string.Empty);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_with_a_fromType_that_is_an_open_generic()
            {
                var instance = new AnotherRegisteredDescendingFromIRegisteredType();
                var registeredName = Guid.NewGuid().ToString("D");

                Action throwingAction = () => target.RegisterInstance(typeof(IList<>), instance);
                var e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(typeof(IList<>), null, instance);
                e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(typeof(IList<>), string.Empty, instance);
                e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(typeof(IList<>), registeredName, instance);
                e = throwingAction.Should().Throw<TypeMustNotBeAnOpenGenericArgumentException>();
                e.And.ParamName.Should().Be("fromType");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_with_a_fromType_that_is_concrete()
            {
                var instance = new AnotherRegisteredDescendingFromIRegisteredType();
                var registeredName = Guid.NewGuid().ToString("D");

                Action throwingAction = () => target.RegisterInstance(typeof(ConcreteClass), instance);
                var e = throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(typeof(ConcreteClass), null, instance);
                e = throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(typeof(ConcreteClass), string.Empty, instance);
                e = throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(typeof(ConcreteClass), registeredName, instance);
                e = throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();
                e.And.ParamName.Should().Be("fromType");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_with_a_fromType_that_is_null()
            {
                var instance = new RegisteredTypeImplementingIRegisteredType();
                var registeredName = Guid.NewGuid().ToString("D");

                Action throwingAction = () => target.RegisterInstance((Type)null, instance);
                var e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(null, null, instance);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(null, string.Empty, instance);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(null, registeredName, instance);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("fromType");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_with_an_instance_that_does_not_implement_the_fromType()
            {
                var instance = new ConcreteClass();
                var registeredName = Guid.NewGuid().ToString("D");

                // the compiler prevents:
                // target.RegisterInstance<IRegisteredType>(instance)
                // target.RegisterInstance<IRegisteredType>(registeredName, instance)
                Action throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), instance);
                var e = throwingAction.Should().Throw<InstanceMustImplementTypeArgumentException>();
                e.And.ParamName.Should().Be("instance");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), null, instance);
                e = throwingAction.Should().Throw<InstanceMustImplementTypeArgumentException>();
                e.And.ParamName.Should().Be("instance");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), string.Empty, instance);
                e = throwingAction.Should().Throw<InstanceMustImplementTypeArgumentException>();
                e.And.ParamName.Should().Be("instance");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), registeredName, instance);
                e = throwingAction.Should().Throw<InstanceMustImplementTypeArgumentException>();
                e.And.ParamName.Should().Be("instance");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_with_an_instance_that_is_null()
            {
                var registeredName = Guid.NewGuid().ToString("D");

                Action throwingAction = () => target.RegisterInstance<IRegisteredType>(null);
                var e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("instance");

                throwingAction = () => target.RegisterInstance<IRegisteredType>(registeredName, null);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("instance");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), null);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("instance");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), null, null);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("instance");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), string.Empty, null);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("instance");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), registeredName, null);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("instance");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerRegistrationAdded_event_generic_no_name()
            {
                var instance = new RegisteredTypeImplementingIRegisteredType();
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.ContainerRegistrationAdded += eh;

                target.RegisterInstance<IRegisteredType>(instance);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);

                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.Instance.Should().Be(instance);
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Name.Should().NotBeNull();
                actualEventArgs.Name.Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerRegistrationAdded_event_generic_with_name()
            {
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;
                var registeredName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                var instance = new RegisteredTypeImplementingIRegisteredType();
                target.ContainerRegistrationAdded += eh;
                target.RegisterInstance<IRegisteredType>(registeredName, instance);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(container);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Instance.Should().Be(instance);
                actualEventArgs.Name.Should().Be(registeredName);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerRegistrationAdded_event_non_generic_named()
            {
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;
                var fromType = typeof(IRegisteredType);
                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                var instance = new RegisteredTypeImplementingIRegisteredType();
                target.ContainerRegistrationAdded += eh;
                target.RegisterInstance(fromType, instance);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(container);
                actualEventArgs.FromType.Should().Be(fromType);
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Instance.Should().Be(instance);
                actualEventArgs.Name.Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerRegistrationAdded_event_non_generic_no_name()
            {
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;
                var fromType = typeof(IRegisteredType);
                var registeredName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                var instance = new RegisteredTypeImplementingIRegisteredType();
                target.ContainerRegistrationAdded += eh;
                target.RegisterInstance(fromType, registeredName, instance);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(container);
                actualEventArgs.FromType.Should().Be(fromType);
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Instance.Should().Be(instance);
                actualEventArgs.Name.Should().Be(registeredName);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerTypeRegistered_event_generic_no_name()
            {
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;
                var instance = new RegisteredTypeImplementingIRegisteredType();

                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.ContainerRegistrationAdded += eh;

                target.RegisterInstance<IRegisteredType>(instance);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);

                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.Instance.Should().Be(instance);
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Name.Should().NotBeNull();
                actualEventArgs.Name.Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerTypeRegistered_event_generic_with_name()
            {
                var registeredName = Guid.NewGuid().ToString("D");
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;
                var instance = new RegisteredTypeImplementingIRegisteredType();

                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.ContainerRegistrationAdded += eh;

                target.RegisterInstance<IRegisteredType>(registeredName, instance);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);

                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.Instance.Should().Be(instance);
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Name.Should().NotBeNull();
                actualEventArgs.Name.Should().Be(registeredName);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerTypeRegistered_event_non_generic_no_name()
            {
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;
                var instance = new RegisteredTypeImplementingIRegisteredType();

                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.ContainerRegistrationAdded += eh;

                target.RegisterInstance(typeof(IRegisteredType), instance);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);

                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.Instance.Should().Be(instance);
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Name.Should().NotBeNull();
                actualEventArgs.Name.Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerTypeRegistered_event_non_generic_with_name()
            {
                var registeredName = Guid.NewGuid().ToString("D");
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;
                var instance = new RegisteredTypeImplementingIRegisteredType();

                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.ContainerRegistrationAdded += eh;

                target.RegisterInstance(typeof(IRegisteredType), registeredName, instance);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);

                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.Instance.Should().Be(instance);
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Name.Should().NotBeNull();
                actualEventArgs.Name.Should().Be(registeredName);
            }
        }

        [TestClass]
        public class When_I_have_an_isolated_container_configured_with_AllowNamedImplementations_false_and_call_RegisterInstance : DisposableArrangeActAssert
        {
            // tests of RegisterInstance that depend on the configuration state:  AllowNamedImplementations == false

            private readonly string containerName = "Isolated Test Container: RegisterInstance Tests";
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
            public void it_should_throw_when_I_attempt_to_register_an_instance_with_a_name_generic()
            {
                Action throwingAction = () => target.RegisterInstance<IRegisteredType>("a name", new RegisteredTypeImplementingIRegisteredType());
                throwingAction.Should().Throw<ContainerConfigurationNamedImplementationsDisabledException>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void it_should_throw_when_I_attempt_to_register_an_instance_with_a_name_non_generic()
            {
                Action throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), "a name", new RegisteredTypeImplementingIRegisteredType());
                throwingAction.Should().Throw<ContainerConfigurationNamedImplementationsDisabledException>();
            }
        }

        [TestClass]
        public class When_I_have_an_isolated_container_configured_with_AllowNamedImplementations_true_and_call_RegisterInstance : DisposableArrangeActAssert
        {
            // tests of RegisterInstance that depend on the configuration state:  AllowNamedImplementations == true

            private readonly string containerName = "Isolated Test Container: RegisterInstance Tests";
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
            public void It_should_allow_me_to_register_an_instance_multiple_times_with_different_names()
            {
                var fromType = typeof(IRegisteredType);
                var registeredName0 = string.Empty;
                var registeredName1 = Guid.NewGuid().ToString("D");
                var registeredName2 = Guid.NewGuid().ToString("D");

                // no name
                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());
                target.RegisterInstance<IRegisteredType>(registeredName1, new RegisteredTypeImplementingIRegisteredType());
                target.RegisterInstance(fromType, registeredName2, new RegisteredTypeImplementingIRegisteredType());

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
            public void It_should_allow_me_to_register_an_instance_with_a_name_generic()
            {
                var fromType = typeof(IRegisteredType);
                var registeredName = Guid.NewGuid().ToString("D");

                var instance = new RegisteredTypeImplementingIRegisteredType();
                target.RegisterInstance<IRegisteredType>(registeredName, instance);

                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(registeredName);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_an_instance_with_a_name_non_generic()
            {
                var fromType = typeof(IRegisteredType);
                var registeredName = Guid.NewGuid().ToString("D");

                var instance = new RegisteredTypeImplementingIRegisteredType();
                target.RegisterInstance(fromType, registeredName, instance);

                var registrations = target.Registrations;
                registrations.Count.Should().Be(1);
                var kvp = registrations.First();
                var key = kvp.Key;
                var value = kvp.Value;
                key.RegisteredType.Should().Be(fromType);
                key.RegisteredName.Should().Be(registeredName);
                value.ImplementationType.Should().BeNull();
                value.ImplementationInstance.Should().Be(instance);
            }
        }

        [TestClass]
        public class When_I_have_an_isolated_container_configured_with_AllowPreclusionOfTypes_false_and_call_RegisterInstance : DisposableArrangeActAssert
        {
            // tests of RegisterInstance that depend on the configuration state:  AllowPreclusionOfTypes == false

            private readonly string containerName = "Isolated Test Container: RegisterInstance Tests";
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
                target.RegisterInstance<IRegisteredType>(new AnotherRegisteredDescendingFromIRegisteredType());
                target.IsRegistered<IRegisteredType>().Should().BeTrue();

                target.Unregister<IRegisteredType>();
                target.Unregister<IRegisteredType>(registeredName);
                target.RegisterInstance<IRegisteredType>(registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
                target.IsRegistered<IRegisteredType>(registeredName).Should().BeTrue();

                target.Unregister<IRegisteredType>();
                target.Unregister<IRegisteredType>(registeredName);
                target.RegisterInstance(typeof(IRegisteredType), new AnotherRegisteredDescendingFromIRegisteredType());
                target.IsRegistered<IRegisteredType>().Should().BeTrue();

                target.Unregister<IRegisteredType>();
                target.Unregister<IRegisteredType>(registeredName);
                target.RegisterInstance(typeof(IRegisteredType), null, new AnotherRegisteredDescendingFromIRegisteredType());
                target.IsRegistered<IRegisteredType>().Should().BeTrue();

                target.Unregister<IRegisteredType>();
                target.Unregister<IRegisteredType>(registeredName);
                target.RegisterInstance(typeof(IRegisteredType), string.Empty, new AnotherRegisteredDescendingFromIRegisteredType());
                target.IsRegistered<IRegisteredType>().Should().BeTrue();

                target.Unregister<IRegisteredType>();
                target.Unregister<IRegisteredType>(registeredName);
                target.RegisterInstance(typeof(IRegisteredType), registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
                target.IsRegistered<IRegisteredType>(registeredName).Should().BeTrue();
            }
        }

        [TestClass]
        public class When_I_have_an_isolated_container_configured_with_AllowPreclusionOfTypes_true_and_call_RegisterInstance : DisposableArrangeActAssert
        {
            // tests of RegisterInstance that depend on the configuration state:  AllowPreclusionOfTypes == true

            private readonly string containerName = "Isolated Test Container: RegisterInstance Tests";
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
            public void It_should_fail_when_I_attempt_to_register_an_instance_with_a_fromType_that_is_precluded()
            {
                target.Manager.AddPrecludedType<IRegisteredType>();

                var registeredName = Guid.NewGuid().ToString("D");

                Action throwingAction = () => target.RegisterInstance<IRegisteredType>(new AnotherRegisteredDescendingFromIRegisteredType());
                var e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();
                e.And.ParamName.Should().Be("TFrom");

                throwingAction = () => target.RegisterInstance<IRegisteredType>(registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
                e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();
                e.And.ParamName.Should().Be("TFrom");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), typeof(AnotherRegisteredDescendingFromIRegisteredType));
                e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), null, typeof(AnotherRegisteredDescendingFromIRegisteredType));
                e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), string.Empty, typeof(AnotherRegisteredDescendingFromIRegisteredType));
                e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();
                e.And.ParamName.Should().Be("fromType");

                throwingAction = () => target.RegisterInstance(typeof(IRegisteredType), registeredName, typeof(AnotherRegisteredDescendingFromIRegisteredType));
                e = throwingAction.Should().Throw<ContainerFromTypePrecludedArgumentException>();
                e.And.ParamName.Should().Be("fromType");
            }
        }

        [TestClass]
        public class When_I_have_an_isolated_container_configured_with_ThrowOnRegistrationCollision_false_and_call_RegisterInstance : DisposableArrangeActAssert
        {
            // tests of RegisterInstance that depend on the configuration state:  ThrowOnRegistrationCollision == false

            private readonly string containerName = "Isolated Test Container: RegisterInstance Tests";
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
                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());
                target.RegisterInstance<IRegisteredType>(new AnotherRegisteredDescendingFromIRegisteredType());
                var instance = target.Resolver.Resolve<IRegisteredType>();
                instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_generic_named()
            {
                var registeredName = "My registered name";
                target.RegisterInstance<IRegisteredType>(registeredName, new RegisteredTypeImplementingIRegisteredType());
                target.RegisterInstance<IRegisteredType>(registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
                var instance = target.Resolver.Resolve<IRegisteredType>(registeredName);
                instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_non_generic()
            {
                target.RegisterInstance(typeof(IRegisteredType), new RegisteredTypeImplementingIRegisteredType());
                target.RegisterInstance(typeof(IRegisteredType), new AnotherRegisteredDescendingFromIRegisteredType());
                var instance = target.Resolver.Resolve<IRegisteredType>();
                instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_non_generic_named()
            {
                var registeredName = "My registered name";
                target.RegisterInstance(typeof(IRegisteredType), registeredName, new RegisteredTypeImplementingIRegisteredType());
                target.RegisterInstance(typeof(IRegisteredType), registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
                var instance = target.Resolver.Resolve<IRegisteredType>(registeredName);
                instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
            }
        }

        [TestClass]
        public class When_I_have_an_isolated_container_configured_with_ThrowOnRegistrationCollision_true_and_call_RegisterInstance : DisposableArrangeActAssert
        {
            // tests of RegisterInstance that depend on the configuration state:  ThrowOnRegistrationCollision == true

            private readonly string containerName = "Isolated Test Container: RegisterInstance Tests";
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
            public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_empty_name_generic()
            {
                var fromType = typeof(IRegisteredType);

                target.RegisterInstance<IRegisteredType>(string.Empty, new RegisteredTypeImplementingIRegisteredType());

                Action throwingAction = () => target.RegisterInstance<IRegisteredType>(string.Empty, new RegisteredTypeImplementingIRegisteredType());

                var e = throwingAction.Should().Throw<ContainerFromTypeNameAlreadyRegisteredArgumentException>();
                e.And.Container.Should().Be(target);
                e.And.RegisteredName.Should().Be(string.Empty);
                e.And.RegisteredType.Should().Be(fromType);
                e.And.Message.Should().Contain("has already registered the type");
                e.And.Message.Should().Contain("under the name");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_empty_name_non_generic()
            {
                var fromType = typeof(IRegisteredType);

                target.RegisterInstance(fromType, string.Empty, new RegisteredTypeImplementingIRegisteredType());

                Action throwingAction = () => target.RegisterInstance(fromType, string.Empty, new RegisteredTypeImplementingIRegisteredType());

                var e = throwingAction.Should().Throw<ContainerFromTypeNameAlreadyRegisteredArgumentException>();
                e.And.Container.Should().Be(target);
                e.And.RegisteredName.Should().Be(string.Empty);
                e.And.RegisteredType.Should().Be<IRegisteredType>();
                e.And.Message.Should().Contain("has already registered the type");
                e.And.Message.Should().Contain("under the name");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_name_generic()
            {
                var name = "my name";
                target.Manager.Configuration.AllowNamedImplementations = true;
                var fromType = typeof(IRegisteredType);

                target.RegisterInstance<IRegisteredType>(name, new RegisteredTypeImplementingIRegisteredType());

                Action throwingAction = () => target.RegisterInstance<IRegisteredType>(name, new RegisteredTypeImplementingIRegisteredType());

                var e = throwingAction.Should().Throw<ContainerFromTypeNameAlreadyRegisteredArgumentException>();
                e.And.Container.Should().Be(target);
                e.And.RegisteredName.Should().Be(name);
                e.And.RegisteredType.Should().Be(fromType);
                e.And.Message.Should().Contain("has already registered the type");
                e.And.Message.Should().Contain("under the name");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_name_non_generic()
            {
                var name = "my name";
                target.Manager.Configuration.AllowNamedImplementations = true;
                var fromType = typeof(IRegisteredType);

                target.RegisterInstance(fromType, name, new RegisteredTypeImplementingIRegisteredType());

                Action throwingAction = () => target.RegisterInstance(fromType, name, new RegisteredTypeImplementingIRegisteredType());

                var e = throwingAction.Should().Throw<ContainerFromTypeNameAlreadyRegisteredArgumentException>();
                e.And.Container.Should().Be(target);
                e.And.RegisteredName.Should().Be(name);
                e.And.RegisteredType.Should().Be(fromType);
                e.And.Message.Should().Contain("has already registered the type");
                e.And.Message.Should().Contain("under the name");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_no_name_generic()
            {
                var fromType = typeof(IRegisteredType);

                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

                Action throwingAction = () => target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

                var e = throwingAction.Should().Throw<ContainerFromTypeNameAlreadyRegisteredArgumentException>();
                e.And.Container.Should().Be(target);
                e.And.RegisteredName.Should().Be(string.Empty);
                e.And.RegisteredType.Should().Be(fromType);
                e.And.Message.Should().Contain("has already registered the type");
                e.And.Message.Should().Contain("under the name");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_no_name_non_generic()
            {
                var fromType = typeof(IRegisteredType);

                target.RegisterInstance(fromType, null, new RegisteredTypeImplementingIRegisteredType());

                Action throwingAction = () => target.RegisterInstance(fromType, new RegisteredTypeImplementingIRegisteredType());

                var e = throwingAction.Should().Throw<ContainerFromTypeNameAlreadyRegisteredArgumentException>();
                e.And.Container.Should().Be(target);
                e.And.RegisteredName.Should().Be(string.Empty);
                e.And.RegisteredType.Should().Be(fromType);
                e.And.Message.Should().Contain("has already registered the type");
                e.And.Message.Should().Contain("under the name");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_null_name_generic()
            {
                var fromType = typeof(IRegisteredType);

                target.RegisterInstance<IRegisteredType>(null, new RegisteredTypeImplementingIRegisteredType());

                Action throwingAction = () => target.RegisterInstance<IRegisteredType>(null, new RegisteredTypeImplementingIRegisteredType());

                var e = throwingAction.Should().Throw<ContainerFromTypeNameAlreadyRegisteredArgumentException>();
                e.And.Container.Should().Be(target);
                e.And.RegisteredName.Should().Be(string.Empty);
                e.And.RegisteredType.Should().Be(fromType);
                e.And.Message.Should().Contain("has already registered the type");
                e.And.Message.Should().Contain("under the name");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_null_name_non_generic()
            {
                var fromType = typeof(IRegisteredType);

                target.RegisterInstance(fromType, null, new RegisteredTypeImplementingIRegisteredType());

                Action throwingAction = () => target.RegisterInstance(fromType, null, new RegisteredTypeImplementingIRegisteredType());

                var e = throwingAction.Should().Throw<ContainerFromTypeNameAlreadyRegisteredArgumentException>();
                e.And.Container.Should().Be(target);
                e.And.RegisteredName.Should().Be(string.Empty);
                e.And.RegisteredType.Should().Be(fromType);
                e.And.Message.Should().Contain("has already registered the type");
                e.And.Message.Should().Contain("under the name");
            }
        }
    }
}
