namespace Landorphan.Ioc.Tests.ServiceLocation.Internal.ValueTypes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Landorphan.Ioc.ServiceLocation.Exceptions;
    using Landorphan.Ioc.ServiceLocation.Interfaces;
    using Landorphan.Ioc.ServiceLocation.Internal;
    using Landorphan.TestUtilities;
    using Landorphan.TestUtilities.ReusableTestImplementations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming
    // ReSharper disable ObjectCreationAsStatement

    public static class RegistrationValueTypeInstancePair_Tests
    {
        [TestClass]
        public class When_I_call_RegistrationValueTypeInstancePair_Clone : CloneableArrangeActAssert<IRegistrationValue>
        {
            private readonly object instance = new ConcreteClassImplementingInterface();
            private readonly Type type = typeof(ConcreteClassImplementingInterface);
            private object actualObject;
            protected override IRegistrationValue Target { get; set; }

            protected override void ArrangeMethod()
            {
                var obj = new RegistrationValueTypeInstancePair(type, instance);
                Target = obj;
            }

            protected override void ActMethod()
            {
                actualObject = Target.Clone();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_clone_correctly()
            {
                It_Should_Clone_Correctly_Implementation();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_create_an_equivalent_untyped_clone_and_set_IsReadOnly_to_false()
            {
                actualObject.Should().BeOfType<RegistrationValueTypeInstancePair>();

                var actualInterface = (IRegistrationValue)actualObject;
                actualInterface.Equals(Target).Should().BeTrue();

                actualInterface.ImplementationType.Should().Be(type);
                actualInterface.ImplementationInstance.Should().Be(instance);

                // This struct type is read-only
                actualInterface.IsReadOnly.Should().BeTrue();
                actualInterface.GetHashCode().Should().Be(Target.GetHashCode());
            }
        }

        [TestClass]
        public class When_I_create_an_RegistrationValueTypeInstancePair_using_the_default_constructor : ArrangeActAssert
        {
            private RegistrationValueTypeInstancePair target;

            protected override void ArrangeMethod()
            {
                target = new RegistrationValueTypeInstancePair();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_be_empty()
            {
                target.IsEmpty.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_be_readonly()
            {
                target.IsReadOnly.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_equivalent_to_itself_and_an_equivalent_instance()
            {
                // ReSharper disable once RedundantCast
                target.CompareTo((IRegistrationValue)target).Should().Be(0);
                target.CompareTo(new RegistrationValueTypeInstancePair()).Should().Be(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_greater_than_null()
            {
                ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
                // ReSharper disable once RedundantCast
                target.CompareTo(null).Should().BeGreaterThan(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_equal_empty()
            {
                target.Equals(RegistrationValueTypeInstancePair.Empty).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_have_a_null_RegisteredName()
            {
                target.ImplementationInstance.Should().BeNull();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_have_a_null_RegisteredType()
            {
                target.ImplementationType.Should().BeNull();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_equal_null()
            {
                target.Equals((object)null).Should().BeFalse();
                // ReSharper disable once RedundantCast
                target.Equals(null).Should().BeFalse();
            }
        }

        [TestClass]
        public class When_I_create_an_RegistrationValueTypeInstancePair_using_the_implementation_instance_constructor : ArrangeActAssert
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_accept_an_abstract_class()
            {
                var instance = new ConcreteClassImplementingInterface();
                var target = new RegistrationValueTypeInstancePair(instance);
                target.ImplementationType.Should().BeNull();
                target.ImplementationInstance.Should().Be(instance);
                target.IsEmpty.Should().BeFalse();
                target.Equals(RegistrationValueTypeInstancePair.Empty).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
            public void It_should_compare_dissimilar_instances_as_nonequivalent()
            {
                ConcreteClassImplementingInterface instanceX = new ConcreteClassImplementingInterface();
                ConcreteClassImplementingInterface instanceY = new ConcreteClassImplementingInterface();
                IInterface instanceZ = new ConcreteClassImplementingInterface();

                var structTarget = new RegistrationValueTypeInstancePair(instanceX);
                var differsInInstance = new RegistrationValueTypeInstancePair(instanceY);
                var differsInInstanceInterface = new RegistrationValueTypeInstancePair(instanceZ);

                structTarget.CompareTo(differsInInstance).Should().NotBe(0);
                structTarget.CompareTo(differsInInstanceInterface).Should().NotBe(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_equivalent_instances_as_equivalent()
            {
                var instance = new ConcreteClassImplementingInterface();
                var structTarget = new RegistrationValueTypeInstancePair(instance);
                var structOther = new RegistrationValueTypeInstancePair(instance);
                IRegistrationValue interfaceOther = new RegistrationValueTypeInstancePair(instance);

                structTarget.CompareTo(structOther).Should().Be(0);
                structTarget.CompareTo(interfaceOther).Should().Be(0);
                structOther.CompareTo(structTarget).Should().Be(0);
                structOther.CompareTo(interfaceOther).Should().Be(0);
                interfaceOther.CompareTo(structTarget).Should().Be(0);
                interfaceOther.CompareTo(structOther).Should().Be(0);

                structTarget.GetHashCode().Should().Be(interfaceOther.GetHashCode());
                interfaceOther.GetHashCode().Should().Be(structOther.GetHashCode());
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_equivalent_to_itself_and_an_equivalent_instance()
            {
                var instance = new ConcreteClass();
                var target = new RegistrationValueTypeInstancePair(instance);
                target.CompareTo((IRegistrationValue)target).Should().Be(0);
                target.CompareTo(new RegistrationValueTypeInstancePair(instance)).Should().Be(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_greater_than_null()
            {
                var instance = new ConcreteClass();
                var target = new RegistrationValueTypeInstancePair(instance);
                ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
                // ReSharper disable once RedundantCast
                target.CompareTo(null).Should().BeGreaterThan(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_equal_another_instance_with_a_different_name()
            {
                var instanceX = new ConcreteClassImplementingInterface();
                var instanceY = new ConcreteClassImplementingInterface();
                var target = new RegistrationValueTypeInstancePair(instanceX);
                var other = new RegistrationValueTypeInstancePair(instanceY);

                target.Equals(other).Should().BeFalse();
                other.Equals(target).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_equal_another_instance_with_a_different_type()
            {
                var target = new RegistrationValueTypeInstancePair(new ConcreteClassImplementingInterface());
                var other = new RegistrationValueTypeInstancePair(new ConcreteClass());

                target.Equals(other).Should().BeFalse();
                other.Equals(target).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_equal_null()
            {
                var target = new RegistrationValueTypeInstancePair(new ConcreteClass());
                target.Equals((object)null).Should().BeFalse();
                // ReSharper disable once RedundantCast
                target.Equals(null).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_reject_null()
            {
                Action throwingAction = () => new RegistrationValueTypeInstancePair((object)null);
                var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
                e.And.ParamName.Should().Be("implementationInstance");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_I_compare_to_the_wrong_type()
            {
                var target = new RegistrationValueTypeInstancePair(new ConcreteClassImplementingInterface());

                Action throwingAction = () => ((IComparable)target).CompareTo(new object());
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("obj");
            }
        }

        [TestClass]
        public class When_I_create_an_RegistrationValueTypeInstancePair_using_the_implementation_type_and_instance_constructor : ArrangeActAssert
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_accept_a_concrete_class()
            {
                var type = typeof(ConcreteClassImplementingInterface);
                var instance = new ConcreteClassImplementingInterface();
                var target = new RegistrationValueTypeInstancePair(type, instance);
                target.ImplementationType.Should().Be<ConcreteClassImplementingInterface>();
                target.ImplementationInstance.Should().Be(instance);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_be_equivalent_when_comparing_itself_when_I_IComparable_CompareTo()
            {
                var type = typeof(ConcreteClassImplementingInterface);
                var instance = new ConcreteClassImplementingInterface();
                var target = new RegistrationValueTypeInstancePair(type, instance);
                object o = target;
                ((IComparable)target).CompareTo(o).Should().Be(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_be_greater_than_null_when_I_call_IComparable_CompareTo()
            {
                var type = typeof(ConcreteClassImplementingInterface);
                var instance = new ConcreteClassImplementingInterface();
                var target = new RegistrationValueTypeInstancePair(type, instance);
                ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_dissimilar_instances_as_nonequivalent()
            {
                var structTarget = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface), new ConcreteClassImplementingInterface());
                var structOther = new RegistrationValueTypeInstancePair(typeof(ConcreteClass), new ConcreteClass());
                IRegistrationValue interfaceOther = new RegistrationValueTypeInstancePair(typeof(AnotherConcreteClass), new AnotherConcreteClass());

                structTarget.CompareTo(structOther).Should().NotBe(0);
                structTarget.CompareTo(interfaceOther).Should().NotBe(0);
                structOther.CompareTo(structTarget).Should().NotBe(0);
                structOther.CompareTo(interfaceOther).Should().NotBe(0);
                interfaceOther.CompareTo(structTarget).Should().NotBe(0);
                interfaceOther.CompareTo(structOther).Should().NotBe(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_equivalent_instances_as_equivalent()
            {
                var type = typeof(ConcreteClassImplementingInterface);
                var instance = new ConcreteClassImplementingInterface();
                var structTarget = new RegistrationValueTypeInstancePair(type, instance);
                var structOther = new RegistrationValueTypeInstancePair(type, instance);
                IRegistrationValue interfaceOther = new RegistrationValueTypeInstancePair(type, instance);

                structTarget.CompareTo(structOther).Should().Be(0);
                structTarget.CompareTo(interfaceOther).Should().Be(0);
                structOther.CompareTo(structTarget).Should().Be(0);
                structOther.CompareTo(interfaceOther).Should().Be(0);
                interfaceOther.CompareTo(structTarget).Should().Be(0);
                interfaceOther.CompareTo(structOther).Should().Be(0);

                structTarget.GetHashCode().Should().Be(interfaceOther.GetHashCode());
                interfaceOther.GetHashCode().Should().Be(structOther.GetHashCode());
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_equivalent_to_itself_and_an_equivalent_instance()
            {
                var type = typeof(ConcreteClassImplementingInterface);
                var instance = new ConcreteClassImplementingInterface();
                var target = new RegistrationValueTypeInstancePair(type, instance);
                // ReSharper disable once RedundantCast
                target.CompareTo((IRegistrationValue)target).Should().Be(0);
                target.CompareTo(new RegistrationValueTypeInstancePair(type, instance)).Should().Be(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_greater_than_null()
            {
                var type = typeof(ConcreteClassImplementingInterface);
                var instance = new ConcreteClassImplementingInterface();
                var target = new RegistrationValueTypeInstancePair(type, instance);
                ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
                // ReSharper disable once RedundantCast
                target.CompareTo(null).Should().BeGreaterThan(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_equal_another_instance_with_type_same_values()
            {
                var type = typeof(ConcreteClassImplementingInterface);
                var instance = new ConcreteClassImplementingInterface();
                var target = new RegistrationValueTypeInstancePair(type, instance);
                var other = new RegistrationValueTypeInstancePair(type, instance);

                target.Equals(other).Should().BeTrue();
                other.Equals(target).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_equal_another_instance_with_a_different_type()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface), new ConcreteClassImplementingInterface());
                var other = new RegistrationValueTypeInstancePair(typeof(ConcreteClass), new ConcreteClass());

                target.Equals(other).Should().BeFalse();
                other.Equals(target).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_equal_null()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClass), new ConcreteClass());
                target.Equals((object)null).Should().BeFalse();
                // ReSharper disable once RedundantCast
                target.Equals(null).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_reject_a_null_instance()
            {
                Action throwingAction = () => new RegistrationValueTypeInstancePair(typeof(IInterface), null);
                var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
                e.And.ParamName.Should().Be("implementationInstance");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage("ReSharper", "RedundantCast")]
            public void It_should_reject_a_null_type()
            {
                Action throwingAction = () => new RegistrationValueTypeInstancePair(null, new object());
                var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
                e.And.ParamName.Should().Be("implementationType");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_reject_an_abstract_class()
            {
                Action throwingAction = () => new RegistrationValueTypeInstancePair(typeof(AbstractClass), new object());
                var e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>().WithMessage("*must not be an abstract or interface type, but is such a type.*");
                e.And.ToType.Should().Be<AbstractClass>();
                e.And.ParamName.Should().Be("implementationType");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_reject_an_interface()
            {
                Action throwingAction = () => new RegistrationValueTypeInstancePair(typeof(IInterface), new ConcreteClassImplementingInterface());
                var e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>().WithMessage("*must not be an abstract or interface type, but is such a type.*");
                e.And.ToType.Should().Be<IInterface>();
                e.And.ParamName.Should().Be("implementationType");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_require_the_implementing_instance_be_of_the_implementing_type()
            {
                var type = typeof(ConcreteClassImplementingInterface);
                var instance = new AnotherConcreteClass();
                Action throwingAction = () => new RegistrationValueTypeInstancePair(type, instance);
                var e = throwingAction.Should().Throw<InstanceMustImplementTypeArgumentException>();
                e.And.ParamName.Should().Be("implementationInstance");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_I_call_IComparable_CompareTo_on_the_wrong_type()
            {
                var type = typeof(ConcreteClassImplementingInterface);
                var instance = new ConcreteClassImplementingInterface();
                var target = new RegistrationValueTypeInstancePair(type, instance);

                Action throwingAction = () => ((IComparable)target).CompareTo(new object());
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("obj");
            }
        }

        [TestClass]
        public class When_I_create_an_RegistrationValueTypeInstancePair_using_the_implementation_type_constructor : ArrangeActAssert
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_be_equivalent_when_comparing_itself_when_I_IComparable_CompareTo()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                object o = target;
                ((IComparable)target).CompareTo(o).Should().Be(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_be_greater_than_null_when_I_call_IComparable_CompareTo()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_accept_a_concrete_class()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClass));
                target.ImplementationType.Should().Be<ConcreteClass>();
                target.ImplementationInstance.Should().BeNull();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_dissimilar_instances_as_nonequivalent()
            {
                var structTarget = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                var structOther = new RegistrationValueTypeInstancePair(typeof(ConcreteClass));
                IRegistrationValue interfaceOther = new RegistrationValueTypeInstancePair(typeof(AnotherConcreteClass));

                structTarget.CompareTo(structOther).Should().NotBe(0);
                structTarget.CompareTo(interfaceOther).Should().NotBe(0);
                structOther.CompareTo(structTarget).Should().NotBe(0);
                structOther.CompareTo(interfaceOther).Should().NotBe(0);
                interfaceOther.CompareTo(structTarget).Should().NotBe(0);
                interfaceOther.CompareTo(structOther).Should().NotBe(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_equivalent_instances_as_equivalent()
            {
                var structTarget = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                var structOther = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                IRegistrationValue interfaceOther = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));

                structTarget.CompareTo(structOther).Should().Be(0);
                structTarget.CompareTo(interfaceOther).Should().Be(0);
                structOther.CompareTo(structTarget).Should().Be(0);
                structOther.CompareTo(interfaceOther).Should().Be(0);
                interfaceOther.CompareTo(structTarget).Should().Be(0);
                interfaceOther.CompareTo(structOther).Should().Be(0);

                structTarget.GetHashCode().Should().Be(interfaceOther.GetHashCode());
                interfaceOther.GetHashCode().Should().Be(structOther.GetHashCode());
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_equivalent_to_itself_and_an_equivalent_instance()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                // ReSharper disable once RedundantCast
                target.CompareTo((IRegistrationValue)target).Should().Be(0);
                target.CompareTo(new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface))).Should().Be(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_compare_greater_than_null()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
                // ReSharper disable once RedundantCast
                target.CompareTo(null).Should().BeGreaterThan(0);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_equal_another_instance_with_type_same_type()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                var other = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));

                target.Equals(other).Should().BeTrue();
                other.Equals(target).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_equal_another_instance_with_a_different_type()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                var other = new RegistrationValueTypeInstancePair(typeof(ConcreteClass));

                target.Equals(other).Should().BeFalse();
                other.Equals(target).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_not_equal_null()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClass));
                target.Equals((object)null).Should().BeFalse();
                // ReSharper disable once RedundantCast
                target.Equals(null).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_reject_an_abstract_class()
            {
                Action throwingAction = () => new RegistrationValueTypeInstancePair(typeof(AbstractClass));
                var e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>().WithMessage("*must not be an abstract or interface type, but is such a type.*");
                e.And.ToType.Should().Be<AbstractClass>();
                e.And.ParamName.Should().Be("implementationType");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_reject_an_interface()
            {
                Action throwingAction = () => new RegistrationValueTypeInstancePair(typeof(IInterface));
                var e = throwingAction.Should().Throw<ToTypeMustNotBeInterfaceNorAbstractArgumentException>().WithMessage("*must not be an abstract or interface type, but is such a type.*");
                e.And.ToType.Should().Be<IInterface>();
                e.And.ParamName.Should().Be("implementationType");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage("ReSharper", "RedundantCast")]
            public void It_should_reject_null()
            {
                Action throwingAction = () => new RegistrationValueTypeInstancePair(null);
                var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
                e.And.ParamName.Should().Be("implementationType");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_throw_when_I_call_IComparable_CompareTo_on_the_wrong_type()
            {
                var target = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));

                Action throwingAction = () => ((IComparable)target).CompareTo(new object());
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("obj");
            }
        }

        [TestClass]
        public class When_I_use_operators_on_RegistrationValueTypeInstancePair : ArrangeActAssert
        {
            private RegistrationValueTypeInstancePair emptyEquivalent;
            private RegistrationValueTypeInstancePair implementedTypeAndInstance;
            private RegistrationValueTypeInstancePair implementedTypeNoInstance;
            private RegistrationValueTypeInstancePair instanceOnly;

            protected override void ArrangeMethod()
            {
                implementedTypeNoInstance = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface));
                implementedTypeAndInstance = new RegistrationValueTypeInstancePair(typeof(ConcreteClassImplementingInterface), new ConcreteClassImplementingInterface());
                instanceOnly = new RegistrationValueTypeInstancePair(new ConcreteClassImplementingInterface());
                emptyEquivalent = new RegistrationValueTypeInstancePair();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
            [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
            [SuppressMessage("ReSharper", "EqualExpressionComparison")]
            public void The_Equality_operator_should_perform_as_expected()
            {
                (implementedTypeNoInstance == implementedTypeNoInstance).Should().BeTrue();
                (implementedTypeAndInstance == implementedTypeAndInstance).Should().BeTrue();
                (instanceOnly == instanceOnly).Should().BeTrue();

                (implementedTypeNoInstance == (IRegistrationValue)implementedTypeNoInstance).Should().BeTrue();
                (implementedTypeAndInstance == (IRegistrationValue)implementedTypeAndInstance).Should().BeTrue();
                (instanceOnly == (IRegistrationValue)instanceOnly).Should().BeTrue();

                ((IRegistrationValue)implementedTypeNoInstance == implementedTypeNoInstance).Should().BeTrue();
                ((IRegistrationValue)implementedTypeAndInstance == implementedTypeAndInstance).Should().BeTrue();
                ((IRegistrationValue)instanceOnly == instanceOnly).Should().BeTrue();

                (emptyEquivalent == emptyEquivalent).Should().BeTrue();
                (emptyEquivalent == (IRegistrationValue)emptyEquivalent).Should().BeTrue();
                ((IRegistrationValue)emptyEquivalent == emptyEquivalent).Should().BeTrue();

                (emptyEquivalent == RegistrationValueTypeInstancePair.Empty).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
            [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
            [SuppressMessage("ReSharper", "EqualExpressionComparison")]
            public void The_GreaterThan_operator_should_perform_as_expected()
            {
                // Testing:  RequiredType.FullName then ImplementationInstance in order of comparison
                (implementedTypeNoInstance > implementedTypeNoInstance).Should().BeFalse();
                (implementedTypeAndInstance > implementedTypeNoInstance).Should().BeTrue();
                (instanceOnly > implementedTypeAndInstance).Should().BeFalse();
                (implementedTypeAndInstance > instanceOnly).Should().BeTrue();

                (implementedTypeNoInstance > (IRegistrationValue)implementedTypeNoInstance).Should().BeFalse();
                (implementedTypeAndInstance > (IRegistrationValue)implementedTypeNoInstance).Should().BeTrue();
                (instanceOnly > (IRegistrationValue)implementedTypeAndInstance).Should().BeFalse();
                (implementedTypeAndInstance > (IRegistrationValue)instanceOnly).Should().BeTrue();

                ((IRegistrationValue)implementedTypeNoInstance > implementedTypeNoInstance).Should().BeFalse();
                ((IRegistrationValue)implementedTypeAndInstance > implementedTypeNoInstance).Should().BeTrue();
                ((IRegistrationValue)instanceOnly > implementedTypeAndInstance).Should().BeFalse();
                ((IRegistrationValue)implementedTypeAndInstance > instanceOnly).Should().BeTrue();

                (emptyEquivalent > emptyEquivalent).Should().BeFalse();
                (implementedTypeNoInstance > emptyEquivalent).Should().BeTrue();

                (emptyEquivalent > (IRegistrationValue)emptyEquivalent).Should().BeFalse();
                (implementedTypeNoInstance > (IRegistrationValue)emptyEquivalent).Should().BeTrue();

                ((IRegistrationValue)emptyEquivalent > emptyEquivalent).Should().BeFalse();
                ((IRegistrationValue)implementedTypeNoInstance > emptyEquivalent).Should().BeTrue();

                (emptyEquivalent > (IRegistrationValue)null).Should().BeTrue();
                (implementedTypeNoInstance > (IRegistrationValue)null).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
            [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
            [SuppressMessage("ReSharper", "EqualExpressionComparison")]
            public void The_GreaterThanOrEqualTo_operator_should_perform_as_expected()
            {
                // Testing:  RequiredType.FullName then ImplementationInstance in order of comparison
                (implementedTypeNoInstance >= implementedTypeNoInstance).Should().BeTrue();
                (implementedTypeAndInstance >= implementedTypeNoInstance).Should().BeTrue();
                (instanceOnly >= implementedTypeAndInstance).Should().BeFalse();
                (implementedTypeAndInstance >= instanceOnly).Should().BeTrue();

                (implementedTypeNoInstance >= (IRegistrationValue)implementedTypeNoInstance).Should().BeTrue();
                (implementedTypeAndInstance >= (IRegistrationValue)implementedTypeNoInstance).Should().BeTrue();
                (instanceOnly >= (IRegistrationValue)implementedTypeAndInstance).Should().BeFalse();
                (implementedTypeAndInstance >= (IRegistrationValue)instanceOnly).Should().BeTrue();

                ((IRegistrationValue)implementedTypeNoInstance >= implementedTypeNoInstance).Should().BeTrue();
                ((IRegistrationValue)implementedTypeAndInstance >= implementedTypeNoInstance).Should().BeTrue();
                ((IRegistrationValue)instanceOnly >= implementedTypeAndInstance).Should().BeFalse();
                ((IRegistrationValue)implementedTypeAndInstance >= instanceOnly).Should().BeTrue();

                (emptyEquivalent >= emptyEquivalent).Should().BeTrue();
                (implementedTypeNoInstance >= emptyEquivalent).Should().BeTrue();

                (emptyEquivalent >= (IRegistrationValue)emptyEquivalent).Should().BeTrue();
                (implementedTypeNoInstance >= (IRegistrationValue)emptyEquivalent).Should().BeTrue();

                ((IRegistrationValue)emptyEquivalent >= emptyEquivalent).Should().BeTrue();
                ((IRegistrationValue)implementedTypeNoInstance >= emptyEquivalent).Should().BeTrue();

                (emptyEquivalent >= (IRegistrationValue)null).Should().BeTrue();
                (implementedTypeNoInstance >= (IRegistrationValue)null).Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
            [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
            [SuppressMessage("ReSharper", "EqualExpressionComparison")]
            public void The_Inequality_operator_should_perform_as_expected()
            {
                (implementedTypeNoInstance != implementedTypeNoInstance).Should().BeFalse();
                (implementedTypeAndInstance != implementedTypeAndInstance).Should().BeFalse();
                (instanceOnly != instanceOnly).Should().BeFalse();

                (implementedTypeNoInstance != (IRegistrationValue)implementedTypeNoInstance).Should().BeFalse();
                (implementedTypeAndInstance != (IRegistrationValue)implementedTypeAndInstance).Should().BeFalse();
                (instanceOnly != (IRegistrationValue)instanceOnly).Should().BeFalse();

                ((IRegistrationValue)implementedTypeNoInstance != implementedTypeNoInstance).Should().BeFalse();
                ((IRegistrationValue)implementedTypeAndInstance != implementedTypeAndInstance).Should().BeFalse();
                ((IRegistrationValue)instanceOnly != instanceOnly).Should().BeFalse();

                (emptyEquivalent != emptyEquivalent).Should().BeFalse();

                (emptyEquivalent != RegistrationValueTypeInstancePair.Empty).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
            [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
            [SuppressMessage("ReSharper", "EqualExpressionComparison")]
            public void The_LessThan_operator_should_perform_as_expected()
            {
                // Testing:  RequiredType.FullName then ImplementationInstance in order of comparison
                (implementedTypeNoInstance < implementedTypeNoInstance).Should().BeFalse();
                (implementedTypeAndInstance < implementedTypeNoInstance).Should().BeFalse();
                (instanceOnly < implementedTypeAndInstance).Should().BeTrue();
                (implementedTypeAndInstance < instanceOnly).Should().BeFalse();

                (implementedTypeNoInstance < (IRegistrationValue)implementedTypeNoInstance).Should().BeFalse();
                (implementedTypeAndInstance < (IRegistrationValue)implementedTypeNoInstance).Should().BeFalse();
                (instanceOnly < (IRegistrationValue)implementedTypeAndInstance).Should().BeTrue();
                (implementedTypeAndInstance < (IRegistrationValue)instanceOnly).Should().BeFalse();

                ((IRegistrationValue)implementedTypeNoInstance < implementedTypeNoInstance).Should().BeFalse();
                ((IRegistrationValue)implementedTypeAndInstance < implementedTypeNoInstance).Should().BeFalse();
                ((IRegistrationValue)instanceOnly < implementedTypeAndInstance).Should().BeTrue();
                ((IRegistrationValue)implementedTypeAndInstance < instanceOnly).Should().BeFalse();

                (emptyEquivalent < emptyEquivalent).Should().BeFalse();
                (implementedTypeNoInstance < emptyEquivalent).Should().BeFalse();

                (emptyEquivalent < (IRegistrationValue)emptyEquivalent).Should().BeFalse();
                (implementedTypeNoInstance < (IRegistrationValue)emptyEquivalent).Should().BeFalse();

                ((IRegistrationValue)emptyEquivalent < emptyEquivalent).Should().BeFalse();
                ((IRegistrationValue)implementedTypeNoInstance < emptyEquivalent).Should().BeFalse();

                (emptyEquivalent < (IRegistrationValue)null).Should().BeFalse();
                (implementedTypeNoInstance < (IRegistrationValue)null).Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
            [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
            [SuppressMessage("ReSharper", "EqualExpressionComparison")]
            public void The_LessThanOrEqualTo_operator_should_perform_as_expected()
            {
                // Testing:  RequiredType.FullName then ImplementationInstance in order of comparison
                (implementedTypeNoInstance <= implementedTypeNoInstance).Should().BeTrue();
                (implementedTypeAndInstance <= implementedTypeNoInstance).Should().BeFalse();
                (instanceOnly <= implementedTypeAndInstance).Should().BeTrue();
                (implementedTypeAndInstance <= instanceOnly).Should().BeFalse();

                (implementedTypeNoInstance <= (IRegistrationValue)implementedTypeNoInstance).Should().BeTrue();
                (implementedTypeAndInstance <= (IRegistrationValue)implementedTypeNoInstance).Should().BeFalse();
                (instanceOnly <= (IRegistrationValue)implementedTypeAndInstance).Should().BeTrue();
                (implementedTypeAndInstance <= (IRegistrationValue)instanceOnly).Should().BeFalse();

                ((IRegistrationValue)implementedTypeNoInstance <= implementedTypeNoInstance).Should().BeTrue();
                ((IRegistrationValue)implementedTypeAndInstance <= implementedTypeNoInstance).Should().BeFalse();
                ((IRegistrationValue)instanceOnly <= implementedTypeAndInstance).Should().BeTrue();
                ((IRegistrationValue)implementedTypeAndInstance <= instanceOnly).Should().BeFalse();

                (emptyEquivalent <= emptyEquivalent).Should().BeTrue();
                (implementedTypeNoInstance <= emptyEquivalent).Should().BeFalse();

                (emptyEquivalent <= (IRegistrationValue)emptyEquivalent).Should().BeTrue();
                (implementedTypeNoInstance <= (IRegistrationValue)emptyEquivalent).Should().BeFalse();

                ((IRegistrationValue)emptyEquivalent <= emptyEquivalent).Should().BeTrue();
                ((IRegistrationValue)implementedTypeNoInstance <= emptyEquivalent).Should().BeFalse();

                (emptyEquivalent <= (IRegistrationValue)null).Should().BeFalse();
                (implementedTypeNoInstance <= (IRegistrationValue)null).Should().BeFalse();
            }
        }

        private abstract class AbstractClass
        {
        }

        private sealed class AnotherConcreteClass
        {
        }

        private sealed class ConcreteClass
        {
        }

        private sealed class ConcreteClassImplementingInterface : IInterface
        {
        }

        [SuppressMessage("Sonar.CodeSmell", "S4275: Getters and setters should access the expected fields", Justification = "false positive")]
        private interface IAnotherInterface
        {
        }

        private interface IInterface
        {
        }
    }
}
