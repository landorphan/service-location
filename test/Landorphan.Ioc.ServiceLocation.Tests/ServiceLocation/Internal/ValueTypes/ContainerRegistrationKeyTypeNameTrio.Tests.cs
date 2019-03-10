namespace Landorphan.Ioc.Tests.ServiceLocation.Internal.ValueTypes
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation.Exceptions;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.Ioc.Tests.Mocks;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.ReusableTestImplementations;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable ObjectCreationAsStatement

   public static class ContainerRegistrationKeyTypeNameTrio_Tests
   {
      private const String Whitespace = " \t ";

      [TestClass]
      public class When_I_call_ContainerRegistrationKeyTypeNameTrio_Clone : CloneableArrangeActAssert<IContainerRegistrationKey>
      {
         private const String containerName = "Mocking Container";

         private readonly IIocContainerMetaIdentity container = new MockContainerImplementingIIocContainerMetaIdentity(containerName, Guid.NewGuid());
         private readonly String registeredName = Guid.NewGuid().ToString("D");
         private readonly Type registeredType = typeof(IInterface);

         private Object actualObject;

         protected override IContainerRegistrationKey Target { get; set; }

         protected override void ArrangeMethod()
         {
            var obj = new ContainerRegistrationKeyTypeNameTrio(container, registeredType, registeredName);
            Target = obj;
         }

         protected override void ActMethod()
         {
            actualObject = Target.Clone();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_Should_Clone_Correctly()
         {
            It_Should_Clone_Correctly_Implementation();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_an_equivalent_untyped_clone_and_set_IsReadOnly_to_false()
         {
            actualObject.Should().BeOfType<ContainerRegistrationKeyTypeNameTrio>();

            var actualInterface = (IContainerRegistrationKey)actualObject;
            actualInterface.Equals(Target).Should().BeTrue();

            actualInterface.RegisteredType.Should().Be(registeredType);
            actualInterface.RegisteredName.Should().Be(registeredName);
            actualInterface.IsDefaultRegistration.Should().Be(Target.IsDefaultRegistration);

            // This struct type is read-only
            actualInterface.IsReadOnly.Should().BeTrue();
            actualInterface.GetHashCode().Should().Be(Target.GetHashCode());
         }
      }

      [TestClass]
      public class When_I_create_an_ContainerRegistrationKeyTypeNameTrio_using_the_default_constructor : ArrangeActAssert
      {
         private ContainerRegistrationKeyTypeNameTrio target;

         protected override void ArrangeMethod()
         {
            target = new ContainerRegistrationKeyTypeNameTrio();
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
            target.CompareTo((IContainerRegistrationKey)target).Should().Be(0);
            target.CompareTo(new ContainerRegistrationKeyTypeNameTrio()).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_greater_than_null()
         {
            ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
            // ReSharper disable once RedundantCast
            target.CompareTo((IContainerRegistrationKey)null).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_equal_empty()
         {
            target.Equals(ContainerRegistrationKeyTypeNameTrio.Empty).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_RegisteredName()
         {
            target.RegisteredName.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_a_null_RegisteredType()
         {
            target.RegisteredType.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_null()
         {
            target.Equals((Object)null).Should().BeFalse();
            // ReSharper disable once RedundantCast
            target.Equals((IContainerRegistrationKey)null).Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_create_an_ContainerRegistrationKeyTypeNameTrio_using_the_type_constructor : ArrangeActAssert
      {
         private const String containerName = "Mocking Container";

         private readonly IIocContainerMetaIdentity container = new MockContainerImplementingIIocContainerMetaIdentity(containerName, Guid.NewGuid());

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_be_equivalent_when_comparing_itself_when_I_IComparable_CompareTo()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            Object o = target;
            ((IComparable)target).CompareTo(o).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_be_greater_than_null_when_I_call_IComparable_CompareTo()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_an_abstract_class()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(AbstractClass));
            target.RegisteredType.Should().Be<AbstractClass>();
            target.RegisteredName.Should().Be(String.Empty);
            target.IsEmpty.Should().BeFalse();
            target.Equals(ContainerRegistrationKeyTypeNameTrio.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_an_interface()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            target.RegisteredType.Should().Be<IInterface>();
            target.RegisteredName.Should().Be(String.Empty);
            target.IsEmpty.Should().BeFalse();
            target.Equals(ContainerRegistrationKeyTypeNameTrio.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_dissimilar_instances_as_nonequivalent()
         {
            var structTarget = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            var structOther = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IAnotherInterface));
            IContainerRegistrationKey interfaceOther = new ContainerRegistrationKeyTypeNameTrio(container, typeof(AbstractClass));

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
            var structTarget = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            var structOther = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            IContainerRegistrationKey interfaceOther = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));

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
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            // ReSharper disable once RedundantCast
            target.CompareTo((IContainerRegistrationKey)target).Should().Be(0);
            target.CompareTo(new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface))).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_greater_than_null()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
            // ReSharper disable once RedundantCast
            target.CompareTo((IContainerRegistrationKey)null).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_equal_another_instance_with_type_same_type()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            var other = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));

            target.Equals(other).Should().BeTrue();
            other.Equals(target).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_another_instance_with_a_different_type()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
            var other = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IAnotherInterface));

            target.Equals(other).Should().BeFalse();
            other.Equals(target).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_null()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(AbstractClass));
            target.Equals((Object)null).Should().BeFalse();
            // ReSharper disable once RedundantCast
            target.Equals((IContainerRegistrationKey)null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_a_concrete_type()
         {
            Action throwingAction = () => new ContainerRegistrationKeyTypeNameTrio(container, typeof(ConcreteClass));
            var e = throwingAction.Should()
               .Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>()
               .WithMessage("*service location only supports the location of interfaces and abstract types*");
            e.And.ParamName.Should().Be("registeredType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_null_container()
         {
            Action throwingAction = () => new ContainerRegistrationKeyTypeNameTrio(null, typeof(IInterface));
            var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
            e.And.ParamName.Should().Be("container");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_null_type()
         {
            Action throwingAction = () => new ContainerRegistrationKeyTypeNameTrio(container, null);
            var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
            e.And.ParamName.Should().Be("registeredType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_I_call_IComparable_CompareTo_on_the_wrong_type()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));

            Action throwingAction = () => ((IComparable)target).CompareTo(new Object());
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("obj");
         }
      }

      [TestClass]
      public class When_I_create_an_ContainerRegistrationKeyTypeNameTrio_using_the_type_name_constructor : ArrangeActAssert
      {
         private const String containerName = "Mocking Container";

         private readonly IIocContainerMetaIdentity container = new MockContainerImplementingIIocContainerMetaIdentity(containerName, Guid.NewGuid());

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_an_abstract_class()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(AbstractClass), name);
            target.RegisteredType.Should().Be<AbstractClass>();
            target.RegisteredName.Should().Be(name);
            target.IsEmpty.Should().BeFalse();
            target.Equals(ContainerRegistrationKeyTypeNameTrio.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_an_interface()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);
            target.RegisteredType.Should().Be<IInterface>();
            target.RegisteredName.Should().Be(name);
            target.IsEmpty.Should().BeFalse();
            target.Equals(ContainerRegistrationKeyTypeNameTrio.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_coalesce_a_null_name()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), null);
            target.RegisteredType.Should().Be<IInterface>();
            target.RegisteredName.Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_dissimilar_instances_as_nonequivalent()
         {
            var name = Guid.NewGuid().ToString("D");
            var anotherName = Guid.NewGuid().ToString("D");
            var structTarget = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);
            var differsInName = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), anotherName);
            var differsInType = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IAnotherInterface), name);

            structTarget.CompareTo(differsInName).Should().NotBe(0);
            structTarget.CompareTo(differsInType).Should().NotBe(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_equivalent_instances_as_equivalent()
         {
            var name = Guid.NewGuid().ToString("D");
            var structTarget = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);
            var structOther = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);
            IContainerRegistrationKey interfaceOther = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);

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
            var name = Guid.NewGuid().ToString("D");
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);
            // ReSharper disable once RedundantCast
            target.CompareTo((IContainerRegistrationKey)target).Should().Be(0);
            target.CompareTo(new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name)).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_greater_than_null()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);
            ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
            // ReSharper disable once RedundantCast
            target.CompareTo((IContainerRegistrationKey)null).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_equal_another_instance_with_type_same_type_and_name()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);
            var other = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);

            target.Equals(other).Should().BeTrue();
            other.Equals(target).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_another_instance_with_a_different_name()
         {
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), Guid.NewGuid().ToString("D"));
            var other = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), Guid.NewGuid().ToString("D"));

            target.Equals(other).Should().BeFalse();
            other.Equals(target).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_another_instance_with_a_different_type()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);
            var other = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IAnotherInterface), name);

            target.Equals(other).Should().BeFalse();
            other.Equals(target).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_null()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(AbstractClass), name);
            target.Equals((Object)null).Should().BeFalse();
            // ReSharper disable once RedundantCast
            target.Equals((IContainerRegistrationKey)null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_a_concrete_class()
         {
            var name = Guid.NewGuid().ToString("D");
            Action throwingAction = () => new ContainerRegistrationKeyTypeNameTrio(container, typeof(ConcreteClass), name);
            var e = throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();
            e.And.ParamName.Should().Be("registeredType");
            e.And.Message.Should().Contain("Landorphan.Ioc service location only supports the location of interfaces and abstract types.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_null_container()
         {
            var name = Guid.NewGuid().ToString("D");
            Action throwingAction = () => new ContainerRegistrationKeyTypeNameTrio(null, typeof(IInterface), name);
            var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
            e.And.ParamName.Should().Be("container");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_null_type()
         {
            var name = Guid.NewGuid().ToString("D");
            Action throwingAction = () => new ContainerRegistrationKeyTypeNameTrio(container, null, name);
            var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
            e.And.ParamName.Should().Be("registeredType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_I_compare_to_the_wrong_type()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), name);

            Action throwingAction = () => ((IComparable)target).CompareTo(new Object());
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("obj");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_trim_a_name()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), Whitespace + name + Whitespace);
            target.RegisteredType.Should().Be<IInterface>();
            target.RegisteredName.Should().Be(name);
         }
      }

      [TestClass]
      public class When_I_use_operators_on_ContainerRegistrationKeyTypeNameTrio : ArrangeActAssert
      {
         private const String containerName = "Mocking Container";

         private readonly IIocContainerMetaIdentity container = new MockContainerImplementingIIocContainerMetaIdentity(containerName, Guid.NewGuid());

         private ContainerRegistrationKeyTypeNameTrio abstractTypeEmptyName;
         private ContainerRegistrationKeyTypeNameTrio abstractTypeNameA;
         private ContainerRegistrationKeyTypeNameTrio abstractTypeNameZ;
         private ContainerRegistrationKeyTypeNameTrio abstractTypeNoName;
         private ContainerRegistrationKeyTypeNameTrio emptyEquivalent;
         private ContainerRegistrationKeyTypeNameTrio interfaceTypeEmptyName;
         private ContainerRegistrationKeyTypeNameTrio interfaceTypeNameA;
         private ContainerRegistrationKeyTypeNameTrio interfaceTypeNameZ;
         private ContainerRegistrationKeyTypeNameTrio interfaceTypeNoName;

         protected override void ArrangeMethod()
         {
            abstractTypeEmptyName = new ContainerRegistrationKeyTypeNameTrio(container, typeof(AbstractClass), String.Empty);
            abstractTypeNameA = new ContainerRegistrationKeyTypeNameTrio(container, typeof(AbstractClass), "A");
            abstractTypeNameZ = new ContainerRegistrationKeyTypeNameTrio(container, typeof(AbstractClass), "Z");
            abstractTypeNoName = new ContainerRegistrationKeyTypeNameTrio(container, typeof(AbstractClass));

            emptyEquivalent = new ContainerRegistrationKeyTypeNameTrio();

            interfaceTypeEmptyName = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), String.Empty);
            interfaceTypeNameA = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), "A");
            interfaceTypeNameZ = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface), "Z");
            interfaceTypeNoName = new ContainerRegistrationKeyTypeNameTrio(container, typeof(IInterface));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
         [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
         [SuppressMessage("ReSharper", "EqualExpressionComparison")]
         public void The_Equality_operator_should_perform_as_expected()
         {
            (abstractTypeEmptyName == abstractTypeEmptyName).Should().BeTrue();

            (abstractTypeNameA == abstractTypeNameA).Should().BeTrue();
            (abstractTypeNameZ == abstractTypeNameZ).Should().BeTrue();
            (abstractTypeNoName == abstractTypeNoName).Should().BeTrue();

            (abstractTypeEmptyName == (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeNameA == (IContainerRegistrationKey)abstractTypeNameA).Should().BeTrue();
            (abstractTypeNameZ == (IContainerRegistrationKey)abstractTypeNameZ).Should().BeTrue();
            (abstractTypeNoName == (IContainerRegistrationKey)abstractTypeNoName).Should().BeTrue();

            ((IContainerRegistrationKey)abstractTypeEmptyName == abstractTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameA == abstractTypeNameA).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameZ == abstractTypeNameZ).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNoName == abstractTypeNoName).Should().BeTrue();

            (emptyEquivalent == emptyEquivalent).Should().BeTrue();
            (emptyEquivalent == (IContainerRegistrationKey)emptyEquivalent).Should().BeTrue();
            ((IContainerRegistrationKey)emptyEquivalent == emptyEquivalent).Should().BeTrue();

            (interfaceTypeEmptyName == interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA == interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameZ == interfaceTypeNameZ).Should().BeTrue();
            (interfaceTypeNoName == interfaceTypeNoName).Should().BeTrue();

            (interfaceTypeEmptyName == (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA == (IContainerRegistrationKey)interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameZ == (IContainerRegistrationKey)interfaceTypeNameZ).Should().BeTrue();
            (interfaceTypeNoName == (IContainerRegistrationKey)interfaceTypeNoName).Should().BeTrue();

            ((IContainerRegistrationKey)interfaceTypeEmptyName == interfaceTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameA == interfaceTypeNameA).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameZ == interfaceTypeNameZ).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNoName == interfaceTypeNoName).Should().BeTrue();

            (emptyEquivalent == ContainerRegistrationKeyTypeNameTrio.Empty).Should().BeTrue();

            (abstractTypeEmptyName == interfaceTypeEmptyName).Should().BeFalse();
            (abstractTypeNameA == interfaceTypeNameA).Should().BeFalse();
            (abstractTypeNameZ == interfaceTypeNameZ).Should().BeFalse();
            (abstractTypeNoName == interfaceTypeNoName).Should().BeFalse();

            (abstractTypeEmptyName == (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (abstractTypeNameA == (IContainerRegistrationKey)interfaceTypeNameA).Should().BeFalse();
            (abstractTypeNameZ == (IContainerRegistrationKey)interfaceTypeNameZ).Should().BeFalse();
            (abstractTypeNoName == (IContainerRegistrationKey)interfaceTypeNoName).Should().BeFalse();

            ((IContainerRegistrationKey)abstractTypeEmptyName == interfaceTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameA == interfaceTypeNameA).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameZ == interfaceTypeNameZ).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNoName == interfaceTypeNoName).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
         [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
         [SuppressMessage("ReSharper", "EqualExpressionComparison")]
         public void The_GreaterThan_operator_should_perform_as_expected()
         {
            // Testing:  FromType.FullName then ImplementationName in order of comparison
            (abstractTypeEmptyName > abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeEmptyName > abstractTypeNoName).Should().BeFalse();
            (abstractTypeNameA > abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeNameZ > abstractTypeNameA).Should().BeTrue();
            (abstractTypeNameA > abstractTypeNameZ).Should().BeFalse();

            (abstractTypeEmptyName > (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeEmptyName > (IContainerRegistrationKey)abstractTypeNoName).Should().BeFalse();
            (abstractTypeNameA > (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeNameZ > (IContainerRegistrationKey)abstractTypeNameA).Should().BeTrue();
            (abstractTypeNameA > (IContainerRegistrationKey)abstractTypeNameZ).Should().BeFalse();

            ((IContainerRegistrationKey)abstractTypeEmptyName > abstractTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeEmptyName > abstractTypeNoName).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameA > abstractTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameZ > abstractTypeNameA).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameA > abstractTypeNameZ).Should().BeFalse();

            (emptyEquivalent > emptyEquivalent).Should().BeFalse();
            (abstractTypeEmptyName > emptyEquivalent).Should().BeTrue();
            (abstractTypeNoName > emptyEquivalent).Should().BeTrue();

            (emptyEquivalent > (IContainerRegistrationKey)emptyEquivalent).Should().BeFalse();
            (abstractTypeEmptyName > (IContainerRegistrationKey)emptyEquivalent).Should().BeTrue();
            (abstractTypeNoName > (IContainerRegistrationKey)emptyEquivalent).Should().BeTrue();

            ((IContainerRegistrationKey)emptyEquivalent > emptyEquivalent).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeEmptyName > emptyEquivalent).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNoName > emptyEquivalent).Should().BeTrue();

            (emptyEquivalent > (IContainerRegistrationKey)null).Should().BeTrue();
            (abstractTypeEmptyName > (IContainerRegistrationKey)null).Should().BeTrue();
            (abstractTypeNoName > (IContainerRegistrationKey)null).Should().BeTrue();

            (interfaceTypeEmptyName > interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeEmptyName > interfaceTypeNoName).Should().BeFalse();
            (interfaceTypeNameA > interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameZ > interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameA > interfaceTypeNameZ).Should().BeFalse();

            (interfaceTypeEmptyName > (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeEmptyName > (IContainerRegistrationKey)interfaceTypeNoName).Should().BeFalse();
            (interfaceTypeNameA > (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameZ > (IContainerRegistrationKey)interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameA > (IContainerRegistrationKey)interfaceTypeNameZ).Should().BeFalse();

            ((IContainerRegistrationKey)interfaceTypeEmptyName > interfaceTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeEmptyName > interfaceTypeNoName).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameA > interfaceTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameZ > interfaceTypeNameA).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameA > interfaceTypeNameZ).Should().BeFalse();

            // interfaces are checked before names
            (interfaceTypeEmptyName > abstractTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA > abstractTypeNameA).Should().BeTrue();

            (interfaceTypeEmptyName > (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA > (IContainerRegistrationKey)abstractTypeNameA).Should().BeTrue();

            ((IContainerRegistrationKey)interfaceTypeEmptyName > abstractTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameA > abstractTypeNameA).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
         [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
         [SuppressMessage("ReSharper", "EqualExpressionComparison")]
         public void The_GreaterThanOrEqualTo_operator_should_perform_as_expected()
         {
            // Testing:  FromType.FullName then ImplementationName in order of comparison
            (abstractTypeEmptyName >= abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeEmptyName >= abstractTypeNoName).Should().BeTrue();
            (abstractTypeNameA >= abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeNameZ >= abstractTypeNameA).Should().BeTrue();
            (abstractTypeNameA >= abstractTypeNameZ).Should().BeFalse();

            (abstractTypeEmptyName >= (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeEmptyName >= (IContainerRegistrationKey)abstractTypeNoName).Should().BeTrue();
            (abstractTypeNameA >= (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeNameZ >= (IContainerRegistrationKey)abstractTypeNameA).Should().BeTrue();
            (abstractTypeNameA >= (IContainerRegistrationKey)abstractTypeNameZ).Should().BeFalse();

            ((IContainerRegistrationKey)abstractTypeEmptyName >= abstractTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeEmptyName >= abstractTypeNoName).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameA >= abstractTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameZ >= abstractTypeNameA).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameA >= abstractTypeNameZ).Should().BeFalse();

            (emptyEquivalent >= emptyEquivalent).Should().BeTrue();
            (abstractTypeEmptyName >= emptyEquivalent).Should().BeTrue();
            (abstractTypeNoName >= emptyEquivalent).Should().BeTrue();

            (emptyEquivalent >= (IContainerRegistrationKey)emptyEquivalent).Should().BeTrue();
            (abstractTypeEmptyName >= (IContainerRegistrationKey)emptyEquivalent).Should().BeTrue();
            (abstractTypeNoName >= (IContainerRegistrationKey)emptyEquivalent).Should().BeTrue();

            ((IContainerRegistrationKey)emptyEquivalent >= emptyEquivalent).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeEmptyName >= emptyEquivalent).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNoName >= emptyEquivalent).Should().BeTrue();

            (emptyEquivalent >= (IContainerRegistrationKey)null).Should().BeTrue();
            (abstractTypeEmptyName >= (IContainerRegistrationKey)null).Should().BeTrue();
            (abstractTypeNoName >= (IContainerRegistrationKey)null).Should().BeTrue();

            (interfaceTypeEmptyName >= interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeEmptyName >= interfaceTypeNoName).Should().BeTrue();
            (interfaceTypeNameA >= interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameZ >= interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameA >= interfaceTypeNameZ).Should().BeFalse();

            (interfaceTypeEmptyName >= (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeEmptyName >= (IContainerRegistrationKey)interfaceTypeNoName).Should().BeTrue();
            (interfaceTypeNameA >= (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameZ >= (IContainerRegistrationKey)interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameA >= (IContainerRegistrationKey)interfaceTypeNameZ).Should().BeFalse();

            ((IContainerRegistrationKey)interfaceTypeEmptyName >= interfaceTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeEmptyName >= interfaceTypeNoName).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameA >= interfaceTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameZ >= interfaceTypeNameA).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameA >= interfaceTypeNameZ).Should().BeFalse();

            // interfaces are checked before names
            (interfaceTypeEmptyName >= abstractTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA >= abstractTypeNameA).Should().BeTrue();

            (interfaceTypeEmptyName >= (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA >= (IContainerRegistrationKey)abstractTypeNameA).Should().BeTrue();

            ((IContainerRegistrationKey)interfaceTypeEmptyName >= abstractTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameA >= abstractTypeNameA).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
         [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
         [SuppressMessage("ReSharper", "EqualExpressionComparison")]
         public void The_Inequality_operator_should_perform_as_expected()
         {
            (abstractTypeEmptyName != abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeNameA != abstractTypeNameA).Should().BeFalse();
            (abstractTypeNameZ != abstractTypeNameZ).Should().BeFalse();
            (abstractTypeNoName != abstractTypeNoName).Should().BeFalse();

            (abstractTypeEmptyName != (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeNameA != (IContainerRegistrationKey)abstractTypeNameA).Should().BeFalse();
            (abstractTypeNameZ != (IContainerRegistrationKey)abstractTypeNameZ).Should().BeFalse();
            (abstractTypeNoName != (IContainerRegistrationKey)abstractTypeNoName).Should().BeFalse();

            ((IContainerRegistrationKey)abstractTypeEmptyName != abstractTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameA != abstractTypeNameA).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameZ != abstractTypeNameZ).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNoName != abstractTypeNoName).Should().BeFalse();

            (emptyEquivalent != emptyEquivalent).Should().BeFalse();

            (interfaceTypeEmptyName != interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA != interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameZ != interfaceTypeNameZ).Should().BeFalse();
            (interfaceTypeNoName != interfaceTypeNoName).Should().BeFalse();

            (interfaceTypeEmptyName != (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA != (IContainerRegistrationKey)interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameZ != (IContainerRegistrationKey)interfaceTypeNameZ).Should().BeFalse();
            (interfaceTypeNoName != (IContainerRegistrationKey)interfaceTypeNoName).Should().BeFalse();

            ((IContainerRegistrationKey)interfaceTypeEmptyName != interfaceTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameA != interfaceTypeNameA).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameZ != interfaceTypeNameZ).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNoName != interfaceTypeNoName).Should().BeFalse();

            (emptyEquivalent != ContainerRegistrationKeyTypeNameTrio.Empty).Should().BeFalse();

            (abstractTypeEmptyName != interfaceTypeEmptyName).Should().BeTrue();
            (abstractTypeNameA != interfaceTypeNameA).Should().BeTrue();
            (abstractTypeNameZ != interfaceTypeNameZ).Should().BeTrue();
            (abstractTypeNoName != interfaceTypeNoName).Should().BeTrue();

            (abstractTypeEmptyName != (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (abstractTypeNameA != (IContainerRegistrationKey)interfaceTypeNameA).Should().BeTrue();
            (abstractTypeNameZ != (IContainerRegistrationKey)interfaceTypeNameZ).Should().BeTrue();
            (abstractTypeNoName != (IContainerRegistrationKey)interfaceTypeNoName).Should().BeTrue();

            ((IContainerRegistrationKey)abstractTypeEmptyName != interfaceTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameA != interfaceTypeNameA).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameZ != interfaceTypeNameZ).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNoName != interfaceTypeNoName).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
         [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
         [SuppressMessage("ReSharper", "EqualExpressionComparison")]
         public void The_LessThan_operator_should_perform_as_expected()
         {
            // Testing:  FromType.FullName then ImplementationName in order of comparison
            (abstractTypeEmptyName < abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeEmptyName < abstractTypeNoName).Should().BeFalse();
            (abstractTypeNameA < abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeNameZ < abstractTypeNameA).Should().BeFalse();
            (abstractTypeNameA < abstractTypeNameZ).Should().BeTrue();

            (abstractTypeEmptyName < (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeEmptyName < (IContainerRegistrationKey)abstractTypeNoName).Should().BeFalse();
            (abstractTypeNameA < (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeNameZ < (IContainerRegistrationKey)abstractTypeNameA).Should().BeFalse();
            (abstractTypeNameA < (IContainerRegistrationKey)abstractTypeNameZ).Should().BeTrue();

            ((IContainerRegistrationKey)abstractTypeEmptyName < abstractTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeEmptyName < abstractTypeNoName).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameA < abstractTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameZ < abstractTypeNameA).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameA < abstractTypeNameZ).Should().BeTrue();

            (emptyEquivalent < emptyEquivalent).Should().BeFalse();
            (abstractTypeEmptyName < emptyEquivalent).Should().BeFalse();
            (abstractTypeNoName < emptyEquivalent).Should().BeFalse();

            (emptyEquivalent < (IContainerRegistrationKey)emptyEquivalent).Should().BeFalse();
            (abstractTypeEmptyName < (IContainerRegistrationKey)emptyEquivalent).Should().BeFalse();
            (abstractTypeNoName < (IContainerRegistrationKey)emptyEquivalent).Should().BeFalse();

            ((IContainerRegistrationKey)emptyEquivalent < emptyEquivalent).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeEmptyName < emptyEquivalent).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNoName < emptyEquivalent).Should().BeFalse();

            (emptyEquivalent < (IContainerRegistrationKey)null).Should().BeFalse();
            (abstractTypeEmptyName < (IContainerRegistrationKey)null).Should().BeFalse();
            (abstractTypeNoName < (IContainerRegistrationKey)null).Should().BeFalse();

            (interfaceTypeEmptyName < interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeEmptyName < interfaceTypeNoName).Should().BeFalse();
            (interfaceTypeNameA < interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameZ < interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameA < interfaceTypeNameZ).Should().BeTrue();

            (interfaceTypeEmptyName < (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeEmptyName < (IContainerRegistrationKey)interfaceTypeNoName).Should().BeFalse();
            (interfaceTypeNameA < (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameZ < (IContainerRegistrationKey)interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameA < (IContainerRegistrationKey)interfaceTypeNameZ).Should().BeTrue();

            ((IContainerRegistrationKey)interfaceTypeEmptyName < interfaceTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeEmptyName < interfaceTypeNoName).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameA < interfaceTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameZ < interfaceTypeNameA).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameA < interfaceTypeNameZ).Should().BeTrue();

            // interfaces are checked before names
            (interfaceTypeEmptyName < abstractTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA < abstractTypeNameA).Should().BeFalse();

            (interfaceTypeEmptyName < (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA < (IContainerRegistrationKey)abstractTypeNameA).Should().BeFalse();

            ((IContainerRegistrationKey)interfaceTypeEmptyName < abstractTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameA < abstractTypeNameA).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Compiler.Warning", "CS1718: Comparison made to same variable", Justification = "Intended (MWP)")]
         [SuppressMessage("SonarLint.CodeSmell", "S1764: Identical expressions should not be used on both sides of a binary operator", Justification = "Intended (MWP)")]
         [SuppressMessage("ReSharper", "EqualExpressionComparison")]
         public void The_LessThanOrEqualTo_operator_should_perform_as_expected()
         {
            // Testing:  FromType.FullName then ImplementationName in order of comparison

            (abstractTypeEmptyName <= abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeEmptyName <= abstractTypeNoName).Should().BeTrue();
            (abstractTypeNameA <= abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeNameZ <= abstractTypeNameA).Should().BeFalse();
            (abstractTypeNameA <= abstractTypeNameZ).Should().BeTrue();

            (abstractTypeEmptyName <= (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeEmptyName <= (IContainerRegistrationKey)abstractTypeNoName).Should().BeTrue();
            (abstractTypeNameA <= (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeNameZ <= (IContainerRegistrationKey)abstractTypeNameA).Should().BeFalse();
            (abstractTypeNameA <= (IContainerRegistrationKey)abstractTypeNameZ).Should().BeTrue();

            ((IContainerRegistrationKey)abstractTypeEmptyName <= abstractTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeEmptyName <= abstractTypeNoName).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeNameA <= abstractTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameZ <= abstractTypeNameA).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNameA <= abstractTypeNameZ).Should().BeTrue();

            (emptyEquivalent <= emptyEquivalent).Should().BeTrue();
            (abstractTypeEmptyName <= emptyEquivalent).Should().BeFalse();
            (abstractTypeNoName <= emptyEquivalent).Should().BeFalse();

            (emptyEquivalent <= (IContainerRegistrationKey)emptyEquivalent).Should().BeTrue();
            (abstractTypeEmptyName <= (IContainerRegistrationKey)emptyEquivalent).Should().BeFalse();
            (abstractTypeNoName <= (IContainerRegistrationKey)emptyEquivalent).Should().BeFalse();

            ((IContainerRegistrationKey)emptyEquivalent <= emptyEquivalent).Should().BeTrue();
            ((IContainerRegistrationKey)abstractTypeEmptyName <= emptyEquivalent).Should().BeFalse();
            ((IContainerRegistrationKey)abstractTypeNoName <= emptyEquivalent).Should().BeFalse();

            (emptyEquivalent <= (IContainerRegistrationKey)null).Should().BeFalse();
            (abstractTypeEmptyName <= (IContainerRegistrationKey)null).Should().BeFalse();
            (abstractTypeNoName <= (IContainerRegistrationKey)null).Should().BeFalse();

            (interfaceTypeEmptyName <= interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeEmptyName <= interfaceTypeNoName).Should().BeTrue();
            (interfaceTypeNameA <= interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameZ <= interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameA <= interfaceTypeNameZ).Should().BeTrue();

            (interfaceTypeEmptyName <= (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeEmptyName <= (IContainerRegistrationKey)interfaceTypeNoName).Should().BeTrue();
            (interfaceTypeNameA <= (IContainerRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameZ <= (IContainerRegistrationKey)interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameA <= (IContainerRegistrationKey)interfaceTypeNameZ).Should().BeTrue();

            ((IContainerRegistrationKey)interfaceTypeEmptyName <= interfaceTypeEmptyName).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeEmptyName <= interfaceTypeNoName).Should().BeTrue();
            ((IContainerRegistrationKey)interfaceTypeNameA <= interfaceTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameZ <= interfaceTypeNameA).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameA <= interfaceTypeNameZ).Should().BeTrue();

            // interfaces are checked before names
            (interfaceTypeEmptyName <= abstractTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA <= abstractTypeNameA).Should().BeFalse();

            (interfaceTypeEmptyName <= (IContainerRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA <= (IContainerRegistrationKey)abstractTypeNameA).Should().BeFalse();

            ((IContainerRegistrationKey)interfaceTypeEmptyName <= abstractTypeEmptyName).Should().BeFalse();
            ((IContainerRegistrationKey)interfaceTypeNameA <= abstractTypeNameA).Should().BeFalse();
         }
      }

      private abstract class AbstractClass
      {
      }

      private sealed class ConcreteClass
      {
      }

      private interface IAnotherInterface
      {
      }

      private interface IInterface
      {
      }
   }
}
