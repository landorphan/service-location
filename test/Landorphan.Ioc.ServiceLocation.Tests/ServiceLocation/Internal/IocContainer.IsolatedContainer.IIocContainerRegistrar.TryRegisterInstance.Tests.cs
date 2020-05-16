namespace Landorphan.Ioc.Tests.ServiceLocation.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
      public class When_I_have_an_isolated_container_and_call_TryRegisterInstance : DisposableArrangeActAssert
      {
          // tests of RegisterImplementation that do not depend on the configuration state

          private readonly string containerName = "Isolated Test Container: TryRegisterInstance Tests";
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

            target.TryRegisterInstance<IRegisteredType>(null, instance);

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

            target.TryRegisterInstance(fromType, null, instance);

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

            target.TryRegisterInstance<IRegisteredType>(Whitespace, instance);

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

            target.TryRegisterInstance(fromType, Whitespace, instance);
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

            target.TryRegisterInstance<IRegisteredType>(string.Empty, instance);

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

            target.TryRegisterInstance(fromType, string.Empty, instance);

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

            target.TryRegisterInstance<IRegisteredType>(instance);

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

            target.TryRegisterInstance(typeof(IRegisteredType), instance);

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

            var actual = target.TryRegisterInstance(typeof(IList<>), instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IList<>), null, instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IList<>), string.Empty, instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IList<>), registeredName, instance);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_with_a_fromType_that_is_concrete()
         {
            var instance = new AnotherRegisteredDescendingFromIRegisteredType();
            var registeredName = Guid.NewGuid().ToString("D");

            var actual = target.TryRegisterInstance(typeof(ConcreteClass), instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(ConcreteClass), null, instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(ConcreteClass), string.Empty, instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(ConcreteClass), registeredName, instance);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_with_a_fromType_that_is_null()
         {
            var instance = new RegisteredTypeImplementingIRegisteredType();
            var registeredName = Guid.NewGuid().ToString("D");

            var actual = target.TryRegisterInstance((Type)null, instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(null, null, instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(null, string.Empty, instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(null, registeredName, instance);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_with_an_instance_that_does_not_implement_the_fromType()
         {
            var instance = new ConcreteClass();
            var registeredName = Guid.NewGuid().ToString("D");

            // the compiler prevents:
            // target.TryRegisterInstance<IRegisteredType>(instance)
            // target.TryRegisterInstance<IRegisteredType>(registeredName, instance)
            var actual = target.TryRegisterInstance(typeof(IRegisteredType), instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), null, instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), string.Empty, instance);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), registeredName, instance);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_with_an_instance_that_is_null()
         {
            var registeredName = Guid.NewGuid().ToString("D");

            var actual = target.TryRegisterInstance<IRegisteredType>(null);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance<IRegisteredType>(registeredName, null);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), null);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), null, null);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), string.Empty, null);
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), registeredName, null);
            actual.Should().BeFalse();
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

            target.TryRegisterInstance<IRegisteredType>(instance);

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
            target.TryRegisterInstance<IRegisteredType>(registeredName, instance);

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
            target.TryRegisterInstance(fromType, instance);

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
            target.TryRegisterInstance(fromType, registeredName, instance);

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

            target.TryRegisterInstance<IRegisteredType>(instance);

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

            target.TryRegisterInstance<IRegisteredType>(registeredName, instance);

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

            target.TryRegisterInstance(typeof(IRegisteredType), instance);

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

            target.TryRegisterInstance(typeof(IRegisteredType), registeredName, instance);

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
      public class When_I_have_an_isolated_container_configured_with_AllowNamedImplementations_false_and_call_TryRegisterInstance : DisposableArrangeActAssert
      {
          // tests of TryRegisterInstance that depend on the configuration state:  AllowNamedImplementations == false

          private readonly string containerName = "Isolated Test Container: TryRegisterInstance Tests";
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
            var actual = target.TryRegisterInstance<IRegisteredType>("a name", new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void it_should_throw_when_I_attempt_to_register_an_instance_with_a_name_non_generic()
         {
            var actual = target.TryRegisterInstance(typeof(IRegisteredType), "a name", new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_AllowNamedImplementations_true_and_call_TryRegisterInstance : DisposableArrangeActAssert
      {
          // tests of TryRegisterInstance that depend on the configuration state:  AllowNamedImplementations == true

          private readonly string containerName = "Isolated Test Container: TryRegisterInstance Tests";
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
            target.TryRegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());
            target.TryRegisterInstance<IRegisteredType>(registeredName1, new RegisteredTypeImplementingIRegisteredType());
            target.TryRegisterInstance(fromType, registeredName2, new RegisteredTypeImplementingIRegisteredType());

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
            target.TryRegisterInstance<IRegisteredType>(registeredName, instance);

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
            target.TryRegisterInstance(fromType, registeredName, instance);

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
      public class When_I_have_an_isolated_container_configured_with_AllowPreclusionOfTypes_false_and_call_TryRegisterInstance : DisposableArrangeActAssert
      {
          // tests of TryRegisterInstance that depend on the configuration state:  AllowPreclusionOfTypes == false

          private readonly string containerName = "Isolated Test Container: TryRegisterInstance Tests";
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
            target.TryRegisterInstance<IRegisteredType>(new AnotherRegisteredDescendingFromIRegisteredType());
            target.IsRegistered<IRegisteredType>().Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterInstance<IRegisteredType>(registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
            target.IsRegistered<IRegisteredType>(registeredName).Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterInstance(typeof(IRegisteredType), new AnotherRegisteredDescendingFromIRegisteredType());
            target.IsRegistered<IRegisteredType>().Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterInstance(typeof(IRegisteredType), null, new AnotherRegisteredDescendingFromIRegisteredType());
            target.IsRegistered<IRegisteredType>().Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterInstance(typeof(IRegisteredType), string.Empty, new AnotherRegisteredDescendingFromIRegisteredType());
            target.IsRegistered<IRegisteredType>().Should().BeTrue();

            target.Unregister<IRegisteredType>();
            target.Unregister<IRegisteredType>(registeredName);
            target.TryRegisterInstance(typeof(IRegisteredType), registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
            target.IsRegistered<IRegisteredType>(registeredName).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_AllowPreclusionOfTypes_true_and_call_TryRegisterInstance : DisposableArrangeActAssert
      {
          // tests of TryRegisterInstance that depend on the configuration state:  AllowPreclusionOfTypes == true

          private readonly string containerName = "Isolated Test Container: TryRegisterInstance Tests";
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

            var actual = target.TryRegisterInstance<IRegisteredType>(new AnotherRegisteredDescendingFromIRegisteredType());
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance<IRegisteredType>(registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), null, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), string.Empty, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();

            actual = target.TryRegisterInstance(typeof(IRegisteredType), registeredName, typeof(AnotherRegisteredDescendingFromIRegisteredType));
            actual.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_ThrowOnRegistrationCollision_false_and_call_TryRegisterInstance : DisposableArrangeActAssert
      {
          // tests of TryRegisterInstance that depend on the configuration state:  ThrowOnRegistrationCollision == false

          private readonly string containerName = "Isolated Test Container: TryRegisterInstance Tests";
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
            target.TryRegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());
            target.TryRegisterInstance<IRegisteredType>(new AnotherRegisteredDescendingFromIRegisteredType());
            var instance = target.Resolver.Resolve<IRegisteredType>();
            instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_generic_named()
         {
            var registeredName = "My registered name";
            target.TryRegisterInstance<IRegisteredType>(registeredName, new RegisteredTypeImplementingIRegisteredType());
            target.TryRegisterInstance<IRegisteredType>(registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
            var instance = target.Resolver.Resolve<IRegisteredType>(registeredName);
            instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_non_generic()
         {
            target.TryRegisterInstance(typeof(IRegisteredType), new RegisteredTypeImplementingIRegisteredType());
            target.TryRegisterInstance(typeof(IRegisteredType), new AnotherRegisteredDescendingFromIRegisteredType());
            var instance = target.Resolver.Resolve<IRegisteredType>();
            instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_me_to_register_with_the_same_key_twice_using_last_updater_non_generic_named()
         {
            var registeredName = "My registered name";
            target.TryRegisterInstance(typeof(IRegisteredType), registeredName, new RegisteredTypeImplementingIRegisteredType());
            target.TryRegisterInstance(typeof(IRegisteredType), registeredName, new AnotherRegisteredDescendingFromIRegisteredType());
            var instance = target.Resolver.Resolve<IRegisteredType>(registeredName);
            instance.Should().BeOfType<AnotherRegisteredDescendingFromIRegisteredType>();
         }
      }

      [TestClass]
      public class When_I_have_an_isolated_container_configured_with_ThrowOnRegistrationCollision_true_and_call_TryRegisterInstance : DisposableArrangeActAssert
      {
          // tests of TryRegisterInstance that depend on the configuration state:  ThrowOnRegistrationCollision == true

          private readonly string containerName = "Isolated Test Container: TryRegisterInstance Tests";
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
            target.TryRegisterInstance<IRegisteredType>(string.Empty, new RegisteredTypeImplementingIRegisteredType());

            var actual = target.TryRegisterInstance<IRegisteredType>(string.Empty, new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_empty_name_non_generic()
         {
            var fromType = typeof(IRegisteredType);

            target.TryRegisterInstance(fromType, string.Empty, new RegisteredTypeImplementingIRegisteredType());

            var actual = target.TryRegisterInstance(fromType, string.Empty, new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_name_generic()
         {
            var name = "my name";
            target.Manager.Configuration.AllowNamedImplementations = true;

            target.TryRegisterInstance<IRegisteredType>(name, new RegisteredTypeImplementingIRegisteredType());

            var actual = target.TryRegisterInstance<IRegisteredType>(name, new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_name_non_generic()
         {
            var name = "my name";
            target.Manager.Configuration.AllowNamedImplementations = true;
            var fromType = typeof(IRegisteredType);

            target.TryRegisterInstance(fromType, name, new RegisteredTypeImplementingIRegisteredType());

            var actual = target.TryRegisterInstance(fromType, name, new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_no_name_generic()
         {
            target.TryRegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());

            var actual = target.TryRegisterInstance<IRegisteredType>(new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_no_name_non_generic()
         {
            var fromType = typeof(IRegisteredType);

            target.TryRegisterInstance(fromType, null, new RegisteredTypeImplementingIRegisteredType());

            var actual = target.TryRegisterInstance(fromType, new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_null_name_generic()
         {
            target.TryRegisterInstance<IRegisteredType>(null, new RegisteredTypeImplementingIRegisteredType());

            var actual = target.TryRegisterInstance<IRegisteredType>(null, new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fail_when_I_attempt_to_register_an_instance_again_with_the_same_null_name_non_generic()
         {
            var fromType = typeof(IRegisteredType);

            target.TryRegisterInstance(fromType, null, new RegisteredTypeImplementingIRegisteredType());

            var actual = target.TryRegisterInstance(fromType, null, new RegisteredTypeImplementingIRegisteredType());
            actual.Should().BeFalse();
         }
      }
   }
}
