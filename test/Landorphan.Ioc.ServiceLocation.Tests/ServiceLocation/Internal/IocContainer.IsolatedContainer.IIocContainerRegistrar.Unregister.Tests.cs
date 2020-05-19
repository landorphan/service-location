namespace Landorphan.Ioc.Tests.ServiceLocation.Internal
{
    using System;
    using System.Globalization;
    using FluentAssertions;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.Ioc.ServiceLocation.EventArguments;
    using Landorphan.Ioc.ServiceLocation.Interfaces;
    using Landorphan.Ioc.ServiceLocation.Internal;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

    public static partial class IocContainer_IsolatedContainer_Tests
    {
        [TestClass]
        public class When_I_have_an_isolated_container_and_call_Unregister : DisposableArrangeActAssert
        {
            private readonly string containerName = "Isolated Test Container: Unregister Tests";
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
            public void It_should_fire_the_ContainerRegistrationRemoved_event_generic_no_name()
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

                target.ContainerRegistrationRemoved += eh;

                target.RegisterInstance<IRegisteredType>(instance);
                target.Unregister<IRegisteredType>();

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);

                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.Instance.Should().BeNull();
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Name.Should().NotBeNull();
                actualEventArgs.Name.Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerRegistrationRemoved_event_generic_with_name()
            {
                var instance = new RegisteredTypeImplementingIRegisteredType();
                var registeredName = Guid.NewGuid().ToString("D");
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.ContainerRegistrationRemoved += eh;

                target.RegisterInstance<IRegisteredType>(registeredName, instance);
                target.Unregister<IRegisteredType>(registeredName);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);

                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.Instance.Should().BeNull();
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Name.Should().Be(registeredName);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerRegistrationRemoved_event_non_generic_no_name()
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

                target.ContainerRegistrationRemoved += eh;

                target.RegisterInstance(typeof(IRegisteredType), instance);
                target.Unregister(typeof(IRegisteredType));

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);

                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.Instance.Should().BeNull();
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Name.Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerRegistrationRemoved_event_non_generic_with_name()
            {
                var instance = new RegisteredTypeImplementingIRegisteredType();
                var registeredName = Guid.NewGuid().ToString("D");
                object actualSender = null;
                ContainerTypeRegistrationEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerTypeRegistrationEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.ContainerRegistrationRemoved += eh;

                target.RegisterInstance(typeof(IRegisteredType), registeredName, instance);
                target.Unregister(typeof(IRegisteredType), registeredName);

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);

                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.FromType.Should().Be<IRegisteredType>();
                actualEventArgs.Instance.Should().BeNull();
                actualEventArgs.ToType.Should().BeNull();
                actualEventArgs.Name.Should().Be(registeredName);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_ignore_a_subsequent_calls_to_Unregister()
            {
                var registeredName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

                // create a "default" registration
                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

                // create a named registration.
                target.RegisterInstance<IRegisteredType>(registeredName, new RegisteredTypeImplementingIRegisteredType());

                // "first" group of unregister
                var actual0 = target.Unregister<IRegisteredType>();
                actual0.Should().BeTrue();

                var actual1 = target.Unregister<IRegisteredType>(registeredName);
                actual1.Should().BeTrue();

                // try unregistering again (4 signatures):
                var actual = target.Unregister<IRegisteredType>();
                actual.Should().BeFalse();

                actual = target.Unregister<IRegisteredType>(registeredName);
                actual.Should().BeFalse();

                actual = target.Unregister(typeof(IRegisteredType));
                actual.Should().BeFalse();

                actual = target.Unregister(typeof(IRegisteredType), registeredName);
                actual.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_ignore_null_types()
            {
                var registrationName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

                // create a "default" registration
                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

                // create a named registration.
                target.RegisterInstance<IRegisteredType>(registrationName, new RegisteredTypeImplementingIRegisteredType());

                var actual = target.Unregister(null);
                actual.Should().BeFalse();

                target.IsRegistered<IRegisteredType>().Should().BeTrue();
                target.IsRegistered<IRegisteredType>(registrationName).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_ignore_unregistered_types()
            {
                var registrationName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

                // create a "default" registration
                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

                // create a named registration.
                target.RegisterInstance<IRegisteredType>(registrationName, new RegisteredTypeImplementingIRegisteredType());

                var actual = target.Unregister(typeof(AbstractRegisteredType));
                actual.Should().BeFalse();

                target.IsRegistered<IRegisteredType>().Should().BeTrue();
                target.IsRegistered<IRegisteredType>(registrationName).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_remove_the_default_registration_generic()
            {
                var registrationName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

                // create a "default" registration
                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

                // create a named registration.
                target.RegisterInstance<IRegisteredType>(registrationName, new RegisteredTypeImplementingIRegisteredType());

                var actual = target.Unregister<IRegisteredType>();
                actual.Should().BeTrue();

                target.IsRegistered<IRegisteredType>().Should().BeFalse();
                target.IsRegistered<IRegisteredType>(registrationName).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_remove_the_default_registration_non_generic()
            {
                var registrationName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

                // create a "default" registration
                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

                // create a named registration.
                target.RegisterInstance<IRegisteredType>(registrationName, new RegisteredTypeImplementingIRegisteredType());

                var actual = target.Unregister(typeof(IRegisteredType));
                actual.Should().BeTrue();

                target.IsRegistered<IRegisteredType>().Should().BeFalse();
                target.IsRegistered<IRegisteredType>(registrationName).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_remove_the_named_registration_generic()
            {
                var registrationName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

                // create a "default" registration
                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

                // create a named registration.
                target.RegisterInstance<IRegisteredType>(registrationName, new RegisteredTypeImplementingIRegisteredType());

                var actual = target.Unregister<IRegisteredType>(registrationName);
                actual.Should().BeTrue();

                target.IsRegistered<IRegisteredType>().Should().BeTrue();
                target.IsRegistered<IRegisteredType>(registrationName).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_remove_the_named_registration_non_generic()
            {
                var registrationName = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

                // create a "default" registration
                target.RegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

                // create a named registration.
                target.RegisterInstance<IRegisteredType>(registrationName, new RegisteredTypeImplementingIRegisteredType());

                var actual = target.Unregister(typeof(IRegisteredType), registrationName);
                actual.Should().BeTrue();

                target.IsRegistered<IRegisteredType>().Should().BeTrue();
                target.IsRegistered<IRegisteredType>(registrationName).Should().BeFalse();
            }
        }
    }
}
