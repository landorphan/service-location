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
        public class When_I_have_an_isolated_container_and_call_AddPrecludedType : DisposableArrangeActAssert
        {
            private readonly string containerName = "Isolated Test Container: IIocContainerManager.AddPrecludedType Tests";
            private readonly Guid containerUid = Guid.NewGuid();
            private IOwnedIocContainer container;
            private IIocContainerManager target;

            protected override void ArrangeMethod()
            {
                container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
                target = container.Manager;
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_add_the_same_type_twice()
            {
                var first = target.AddPrecludedType<IRegisteredType>();
                first.Should().BeTrue();
                var second = target.AddPrecludedType<IRegisteredType>();
                second.Should().BeFalse();

                target.PrecludedTypes.Count.Should().Be(1);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_accept_an_abstract_type()
            {
                var actual = target.AddPrecludedType<AbstractRegisteredType>();

                actual.Should().BeTrue();
                target.PrecludedTypes.Should().Contain(typeof(AbstractRegisteredType));
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_accept_an_interface_type()
            {
                var actual = target.AddPrecludedType<IRegisteredType>();

                actual.Should().BeTrue();
                target.PrecludedTypes.Should().Contain(typeof(IRegisteredType));
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerChildAdded_event()
            {
                object actualSender = null;
                ContainerParentChildEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerParentChildEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.Manager.ContainerChildAdded += eh;
                var child = target.CreateChildContainer("child") as IOwnedIocContainer;
                try
                {
                    actualSender.Should().NotBeNull();
                    actualSender.Should().Be(target);
                    actualEventArgs.Should().NotBeNull();
                    actualEventArgs.Child.Should().Be(child);
                    actualEventArgs.Parent.Should().Be(target);
                }
                finally
                {
                    child?.Dispose();
                }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerChildRemoved()
            {
                object actualSender = null;
                ContainerParentChildEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerParentChildEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.Manager.ContainerChildRemoved += eh;
                var child = target.CreateChildContainer("child") as IOwnedIocContainer;
                child?.Dispose();
                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Child.Should().Be(child);
                actualEventArgs.Parent.Should().Be(target);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerPrecludedTypeAdded_event_generic()
            {
                object actualSender = null;
                ContainerTypeEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerTypeEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.Manager.ContainerPrecludedTypeAdded += eh;

                target.Manager.AddPrecludedType<IRegisteredType>();

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.Type.Should().Be<IRegisteredType>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerPrecludedTypeAdded_event_non_generic()
            {
                object actualSender = null;
                ContainerTypeEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerTypeEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.Manager.ContainerPrecludedTypeAdded += eh;

                target.Manager.AddPrecludedType(typeof(IRegisteredType));

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.Type.Should().Be<IRegisteredType>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerPrecludedTypeRemoved_event_generic()
            {
                object actualSender = null;
                ContainerTypeEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerTypeEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.Manager.ContainerPrecludedTypeRemoved += eh;

                target.Manager.AddPrecludedType<IRegisteredType>();
                target.Manager.RemovePrecludedType<IRegisteredType>();

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.Type.Should().Be<IRegisteredType>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerPrecludedTypeRemoved_event_non_generic()
            {
                object actualSender = null;
                ContainerTypeEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerTypeEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.Manager.ContainerPrecludedTypeRemoved += eh;

                target.Manager.AddPrecludedType(typeof(IRegisteredType));
                target.Manager.RemovePrecludedType(typeof(IRegisteredType));

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.Type.Should().Be<IRegisteredType>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_ignore_a_concrete_type()
            {
                var actual = target.AddPrecludedType<ConcreteClass>();
                actual.Should().BeFalse();
                target.PrecludedTypes.Should().BeEmpty();

                actual = target.AddPrecludedType(typeof(ConcreteClass));
                actual.Should().BeFalse();
                target.PrecludedTypes.Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_ignore_a_null_type()
            {
                var actual = target.AddPrecludedType(null);
                actual.Should().BeFalse();
                target.PrecludedTypes.Should().BeEmpty();
            }
        }

        [TestClass]
        public class When_I_have_an_isolated_container_and_call_CreateChildContainer : DisposableArrangeActAssert
        {
            private readonly string childContainerName = "Isolated Child Test Container";
            private readonly string containerName = "Isolated Test Container: IIocContainerManager.CreateChildContainer Tests";
            private readonly Guid containerUid = Guid.NewGuid();
            private ContainerParentChildEventArgs actualEventArgs;
            private object actualSender;
            private IOwnedIocContainer child;
            private IOwnedIocContainer container;
            private IIocContainerManager target;

            protected override void ArrangeMethod()
            {
                container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
                target = container.Manager;

                var eh = new EventHandler<ContainerParentChildEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.ContainerChildAdded += eh;
                child = (IOwnedIocContainer)target.CreateChildContainer(childContainerName);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_create_the_child_container()
            {
                child.IsRoot.Should().BeFalse();
                child.Parent.Should().Be(target);
                child.Uid.Should().NotBe(Guid.Empty);
                child.Uid.Should().NotBe(target.Uid);
                container.Children.Should().Contain(child);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_ContainerChildAdded_event()
            {
                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Parent.Should().Be(container);
                actualEventArgs.Child.Should().Be(child);
            }
        }

        [TestClass]
        public class When_I_have_an_isolated_container_and_call_RemovePrecludedType : DisposableArrangeActAssert
        {
            private readonly string containerName = "Isolated Test Container: RemovePrecludedType Tests";
            private readonly Guid containerUid = Guid.NewGuid();
            private IOwnedIocContainer container;
            private IIocContainerManager target;

            protected override void ArrangeMethod()
            {
                container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
                target = container.Manager;
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_PrecludedTypeRemoved_event_generic()
            {
                object actualSender = null;
                ContainerTypeEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerTypeEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.Manager.ContainerPrecludedTypeRemoved += eh;

                target.Manager.AddPrecludedType<IRegisteredType>();
                target.Manager.RemovePrecludedType<IRegisteredType>();

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.Type.Should().Be<IRegisteredType>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_fire_the_PrecludedTypeRemoved_event_non_generic()
            {
                object actualSender = null;
                ContainerTypeEventArgs actualEventArgs = null;

                var eh = new EventHandler<ContainerTypeEventArgs>(
                    (o, e) =>
                    {
                        actualSender = o;
                        actualEventArgs = e;
                    });

                target.Manager.ContainerPrecludedTypeRemoved += eh;

                target.Manager.AddPrecludedType(typeof(IRegisteredType));
                target.Manager.RemovePrecludedType(typeof(IRegisteredType));

                actualSender.Should().NotBeNull();
                actualSender.Should().Be(target);
                actualEventArgs.Should().NotBeNull();
                actualEventArgs.Container.Should().Be(target);
                actualEventArgs.Type.Should().Be<IRegisteredType>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_ignore_a_concrete_type()
            {
                var actual = target.RemovePrecludedType<ConcreteClass>();
                actual.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_ignore_nulls()
            {
                var actual = target.RemovePrecludedType(null);
                actual.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_return_false_when_given_precluded_type_is_not_precluded()
            {
                var actual = target.Manager.RemovePrecludedType<IRegisteredType>();
                actual.Should().BeFalse();

                actual = target.Manager.RemovePrecludedType(typeof(IRegisteredType));
                actual.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_return_true_and_remove_the_entry_when_given_type_is_precluded()
            {
                target.Manager.AddPrecludedType<IRegisteredType>();
                var actual = target.Manager.RemovePrecludedType<IRegisteredType>();
                actual.Should().BeTrue();

                target.Manager.AddPrecludedType<IRegisteredType>();
                actual = target.Manager.RemovePrecludedType(typeof(IRegisteredType));
                actual.Should().BeTrue();
            }
        }

        [TestClass]
        public class When_I_have_an_isolated_container_and_inspect_its_configuration : ArrangeActAssert
        {
            private readonly string containerName = "Isolated Test Container: IIocContainerManager.Configuration Tests";
            private readonly Guid containerUid = Guid.NewGuid();
            private IOwnedIocContainer container;
            private IIocContainerManager target;

            protected override void ArrangeMethod()
            {
                container = IocContainer.TestHookCreateIsolatedContainer(containerUid, containerName);
                target = container.Manager;
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_have_a_configuration()
            {
                target.Configuration.Should().NotBeNull();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Its_configuration_should_AllowNamedImplementations_by_default()
            {
                target.Configuration.AllowNamedImplementations.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Its_configuration_should_AllowPreclusionOfTypes_by_default()
            {
                target.Configuration.AllowPreclusionOfTypes.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Its_configuration_should_not_be_ReadOnly_by_default()
            {
                target.Configuration.IsReadOnly.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Its_configuration_should_not_ThrowOnRegistrationCollision_by_default()
            {
                target.Configuration.ThrowOnRegistrationCollision.Should().BeFalse();
            }
        }
    }
}
