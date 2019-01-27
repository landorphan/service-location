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

   public static class RegistrationKeyTypeNamePair_Tests
   {
      private const String Whitespace = " \t ";

      [TestClass]
      public class When_I_call_RegistrationKeyTypeNamePair_Clone : CloneableArrangeActAssert<IRegistrationKey>
      {
         private readonly String name = Guid.NewGuid().ToString("D");
         private readonly Type type = typeof(IInterface);
         private Object actualObject;

         protected override IRegistrationKey Target { get; set; }

         protected override void ArrangeMethod()
         {
            var obj = new RegistrationKeyTypeNamePair(type, name);
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
            actualObject.Should().BeOfType<RegistrationKeyTypeNamePair>();

            var actualInterface = (IRegistrationKey)actualObject;
            actualInterface.Equals(Target).Should().BeTrue();

            actualInterface.RegisteredType.Should().Be(type);
            actualInterface.RegisteredName.Should().Be(name);
            actualInterface.IsDefaultRegistration.Should().Be(Target.IsDefaultRegistration);

            // This struct type is read-only
            actualInterface.IsReadOnly.Should().BeTrue();
            actualInterface.GetHashCode().Should().Be(Target.GetHashCode());
         }
      }

      [TestClass]
      public class When_I_create_an_RegistrationKeyTypeNamePair_using_the_default_constructor : ArrangeActAssert
      {
         private RegistrationKeyTypeNamePair target;

         protected override void ArrangeMethod()
         {
            target = new RegistrationKeyTypeNamePair();
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
            target.CompareTo((IRegistrationKey)target).Should().Be(0);
            target.CompareTo(new RegistrationKeyTypeNamePair()).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_greater_than_null()
         {
            ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
            // ReSharper disable once RedundantCast
            target.CompareTo((IRegistrationKey)null).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_equal_empty()
         {
            target.Equals(RegistrationKeyTypeNamePair.Empty).Should().BeTrue();
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
            target.Equals((IRegistrationKey)null).Should().BeFalse();
         }
      }

      [TestClass]
      public class When_I_create_an_RegistrationKeyTypeNamePair_using_the_type_constructor : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_be_equivalent_when_comparing_itself_when_I_IComparable_CompareTo()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface));
            Object o = target;
            ((IComparable)target).CompareTo(o).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_be_greater_than_null_when_I_call_IComparable_CompareTo()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface));
            ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_an_abstract_class()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(AbstractClass));
            target.RegisteredType.Should().Be<AbstractClass>();
            target.RegisteredName.Should().Be(String.Empty);
            target.IsEmpty.Should().BeFalse();
            target.Equals(RegistrationKeyTypeNamePair.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_an_interface()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface));
            target.RegisteredType.Should().Be<IInterface>();
            target.RegisteredName.Should().Be(String.Empty);
            target.IsEmpty.Should().BeFalse();
            target.Equals(RegistrationKeyTypeNamePair.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_dissimilar_instances_as_nonequivalent()
         {
            var structTarget = new RegistrationKeyTypeNamePair(typeof(IInterface));
            var structOther = new RegistrationKeyTypeNamePair(typeof(IAnotherInterface));
            IRegistrationKey interfaceOther = new RegistrationKeyTypeNamePair(typeof(AbstractClass));

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
            var structTarget = new RegistrationKeyTypeNamePair(typeof(IInterface));
            var structOther = new RegistrationKeyTypeNamePair(typeof(IInterface));
            IRegistrationKey interfaceOther = new RegistrationKeyTypeNamePair(typeof(IInterface));

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
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface));
            // ReSharper disable once RedundantCast
            target.CompareTo((IRegistrationKey)target).Should().Be(0);
            target.CompareTo(new RegistrationKeyTypeNamePair(typeof(IInterface))).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_greater_than_null()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface));
            ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
            // ReSharper disable once RedundantCast
            target.CompareTo((IRegistrationKey)null).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_equal_another_instance_with_type_same_type()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface));
            var other = new RegistrationKeyTypeNamePair(typeof(IInterface));

            target.Equals(other).Should().BeTrue();
            other.Equals(target).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_another_instance_with_a_different_type()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface));
            var other = new RegistrationKeyTypeNamePair(typeof(IAnotherInterface));

            target.Equals(other).Should().BeFalse();
            other.Equals(target).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_null()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(AbstractClass));
            target.Equals((Object)null).Should().BeFalse();
            // ReSharper disable once RedundantCast
            target.Equals((IRegistrationKey)null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_a_concrete_class()
         {
            Action throwingAction = () => new RegistrationKeyTypeNamePair(typeof(ConcreteClass));
            var e = throwingAction.Should()
               .Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>()
               .WithMessage("*service location only supports the location of interfaces and abstract types*");
            e.And.ParamName.Should().Be("registeredType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_null()
         {
            Action throwingAction = () => new RegistrationKeyTypeNamePair(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
            e.And.ParamName.Should().Be("registeredType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_I_call_IComparable_CompareTo_on_the_wrong_type()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface));

            Action throwingAction = () => ((IComparable)target).CompareTo(new Object());
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("obj");
         }
      }

      [TestClass]
      public class When_I_create_an_RegistrationKeyTypeNamePair_using_the_type_name_constructor : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_an_abstract_class()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new RegistrationKeyTypeNamePair(typeof(AbstractClass), name);
            target.RegisteredType.Should().Be<AbstractClass>();
            target.RegisteredName.Should().Be(name);
            target.IsEmpty.Should().BeFalse();
            target.Equals(RegistrationKeyTypeNamePair.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_an_interface()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface), name);
            target.RegisteredType.Should().Be<IInterface>();
            target.RegisteredName.Should().Be(name);
            target.IsEmpty.Should().BeFalse();
            target.Equals(RegistrationKeyTypeNamePair.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_coalesce_a_null_name()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface), null);
            target.RegisteredType.Should().Be<IInterface>();
            target.RegisteredName.Should().Be(String.Empty);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_dissimilar_instances_as_nonequivalent()
         {
            var name = Guid.NewGuid().ToString("D");
            var anotherName = Guid.NewGuid().ToString("D");
            var structTarget = new RegistrationKeyTypeNamePair(typeof(IInterface), name);
            var differsInName = new RegistrationKeyTypeNamePair(typeof(IInterface), anotherName);
            var differsInType = new RegistrationKeyTypeNamePair(typeof(IAnotherInterface), name);

            structTarget.CompareTo(differsInName).Should().NotBe(0);
            structTarget.CompareTo(differsInType).Should().NotBe(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_equivalent_instances_as_equivalent()
         {
            var name = Guid.NewGuid().ToString("D");
            var structTarget = new RegistrationKeyTypeNamePair(typeof(IInterface), name);
            var structOther = new RegistrationKeyTypeNamePair(typeof(IInterface), name);
            IRegistrationKey interfaceOther = new RegistrationKeyTypeNamePair(typeof(IInterface), name);

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
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface), name);
            // ReSharper disable once RedundantCast
            target.CompareTo((IRegistrationKey)target).Should().Be(0);
            target.CompareTo(new RegistrationKeyTypeNamePair(typeof(IInterface), name)).Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_compare_greater_than_null()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface), name);
            ((IComparable)target).CompareTo(null).Should().BeGreaterThan(0);
            // ReSharper disable once RedundantCast
            target.CompareTo((IRegistrationKey)null).Should().BeGreaterThan(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_equal_another_instance_with_type_same_type_and_name()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface), name);
            var other = new RegistrationKeyTypeNamePair(typeof(IInterface), name);

            target.Equals(other).Should().BeTrue();
            other.Equals(target).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_another_instance_with_a_different_name()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface), Guid.NewGuid().ToString("D"));
            var other = new RegistrationKeyTypeNamePair(typeof(IInterface), Guid.NewGuid().ToString("D"));

            target.Equals(other).Should().BeFalse();
            other.Equals(target).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_another_instance_with_a_different_type()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface), name);
            var other = new RegistrationKeyTypeNamePair(typeof(IAnotherInterface), name);

            target.Equals(other).Should().BeFalse();
            other.Equals(target).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_equal_null()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new RegistrationKeyTypeNamePair(typeof(AbstractClass), name);
            target.Equals((Object)null).Should().BeFalse();
            // ReSharper disable once RedundantCast
            target.Equals((IRegistrationKey)null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_a_concrete_class()
         {
            var name = Guid.NewGuid().ToString("D");
            Action throwingAction = () => new RegistrationKeyTypeNamePair(typeof(ConcreteClass), name);
            var e = throwingAction.Should().Throw<FromTypeMustBeInterfaceOrAbstractTypeArgumentException>();
            e.And.ParamName.Should().Be("registeredType");
            e.And.Message.Should().Contain("Landorphan.Ioc service location only supports the location of interfaces and abstract types.");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_reject_null()
         {
            var name = Guid.NewGuid().ToString("D");
            Action throwingAction = () => new RegistrationKeyTypeNamePair(null, name);
            var e = throwingAction.Should().Throw<ArgumentNullException>().WithMessage("*cannot be null*");
            e.And.ParamName.Should().Be("registeredType");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_when_I_compare_to_the_wrong_type()
         {
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface));

            Action throwingAction = () => ((IComparable)target).CompareTo(new Object());
            var e = throwingAction.Should().Throw<ArgumentException>();
            e.And.ParamName.Should().Be("obj");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_trim_a_null_name()
         {
            var name = Guid.NewGuid().ToString("D");
            var target = new RegistrationKeyTypeNamePair(typeof(IInterface), Whitespace + name + Whitespace);
            target.RegisteredType.Should().Be<IInterface>();
            target.RegisteredName.Should().Be(name);
         }
      }

      [TestClass]
      public class When_I_use_operators_on_RegistrationKeyTypeNamePair : ArrangeActAssert
      {
         private RegistrationKeyTypeNamePair abstractTypeEmptyName;
         private RegistrationKeyTypeNamePair abstractTypeNameA;
         private RegistrationKeyTypeNamePair abstractTypeNameZ;
         private RegistrationKeyTypeNamePair abstractTypeNoName;
         private RegistrationKeyTypeNamePair emptyEquivalent;
         private RegistrationKeyTypeNamePair interfaceTypeEmptyName;
         private RegistrationKeyTypeNamePair interfaceTypeNameA;
         private RegistrationKeyTypeNamePair interfaceTypeNameZ;
         private RegistrationKeyTypeNamePair interfaceTypeNoName;

         protected override void ArrangeMethod()
         {
            abstractTypeEmptyName = new RegistrationKeyTypeNamePair(typeof(AbstractClass), String.Empty);
            abstractTypeNameA = new RegistrationKeyTypeNamePair(typeof(AbstractClass), "A");
            abstractTypeNameZ = new RegistrationKeyTypeNamePair(typeof(AbstractClass), "Z");
            abstractTypeNoName = new RegistrationKeyTypeNamePair(typeof(AbstractClass));

            emptyEquivalent = new RegistrationKeyTypeNamePair();

            interfaceTypeEmptyName = new RegistrationKeyTypeNamePair(typeof(IInterface), String.Empty);
            interfaceTypeNameA = new RegistrationKeyTypeNamePair(typeof(IInterface), "A");
            interfaceTypeNameZ = new RegistrationKeyTypeNamePair(typeof(IInterface), "Z");
            interfaceTypeNoName = new RegistrationKeyTypeNamePair(typeof(IInterface));
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

            (abstractTypeEmptyName == (IRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeNameA == (IRegistrationKey)abstractTypeNameA).Should().BeTrue();
            (abstractTypeNameZ == (IRegistrationKey)abstractTypeNameZ).Should().BeTrue();
            (abstractTypeNoName == (IRegistrationKey)abstractTypeNoName).Should().BeTrue();

            ((IRegistrationKey)abstractTypeEmptyName == abstractTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameA == abstractTypeNameA).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameZ == abstractTypeNameZ).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNoName == abstractTypeNoName).Should().BeTrue();

            (emptyEquivalent == emptyEquivalent).Should().BeTrue();
            (emptyEquivalent == (IRegistrationKey)emptyEquivalent).Should().BeTrue();
            ((IRegistrationKey)emptyEquivalent == emptyEquivalent).Should().BeTrue();

            (interfaceTypeEmptyName == interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA == interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameZ == interfaceTypeNameZ).Should().BeTrue();
            (interfaceTypeNoName == interfaceTypeNoName).Should().BeTrue();

            (interfaceTypeEmptyName == (IRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA == (IRegistrationKey)interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameZ == (IRegistrationKey)interfaceTypeNameZ).Should().BeTrue();
            (interfaceTypeNoName == (IRegistrationKey)interfaceTypeNoName).Should().BeTrue();

            ((IRegistrationKey)interfaceTypeEmptyName == interfaceTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameA == interfaceTypeNameA).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameZ == interfaceTypeNameZ).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNoName == interfaceTypeNoName).Should().BeTrue();

            (emptyEquivalent == RegistrationKeyTypeNamePair.Empty).Should().BeTrue();

            (abstractTypeEmptyName == interfaceTypeEmptyName).Should().BeFalse();
            (abstractTypeNameA == interfaceTypeNameA).Should().BeFalse();
            (abstractTypeNameZ == interfaceTypeNameZ).Should().BeFalse();
            (abstractTypeNoName == interfaceTypeNoName).Should().BeFalse();

            (abstractTypeEmptyName == (IRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (abstractTypeNameA == (IRegistrationKey)interfaceTypeNameA).Should().BeFalse();
            (abstractTypeNameZ == (IRegistrationKey)interfaceTypeNameZ).Should().BeFalse();
            (abstractTypeNoName == (IRegistrationKey)interfaceTypeNoName).Should().BeFalse();

            ((IRegistrationKey)abstractTypeEmptyName == interfaceTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameA == interfaceTypeNameA).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameZ == interfaceTypeNameZ).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNoName == interfaceTypeNoName).Should().BeFalse();
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

            (abstractTypeEmptyName > (IRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeEmptyName > (IRegistrationKey)abstractTypeNoName).Should().BeFalse();
            (abstractTypeNameA > (IRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeNameZ > (IRegistrationKey)abstractTypeNameA).Should().BeTrue();
            (abstractTypeNameA > (IRegistrationKey)abstractTypeNameZ).Should().BeFalse();

            ((IRegistrationKey)abstractTypeEmptyName > abstractTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)abstractTypeEmptyName > abstractTypeNoName).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameA > abstractTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameZ > abstractTypeNameA).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameA > abstractTypeNameZ).Should().BeFalse();

            (emptyEquivalent > emptyEquivalent).Should().BeFalse();
            (abstractTypeEmptyName > emptyEquivalent).Should().BeTrue();
            (abstractTypeNoName > emptyEquivalent).Should().BeTrue();

            (emptyEquivalent > (IRegistrationKey)emptyEquivalent).Should().BeFalse();
            (abstractTypeEmptyName > (IRegistrationKey)emptyEquivalent).Should().BeTrue();
            (abstractTypeNoName > (IRegistrationKey)emptyEquivalent).Should().BeTrue();

            ((IRegistrationKey)emptyEquivalent > emptyEquivalent).Should().BeFalse();
            ((IRegistrationKey)abstractTypeEmptyName > emptyEquivalent).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNoName > emptyEquivalent).Should().BeTrue();

            (emptyEquivalent > (IRegistrationKey)null).Should().BeTrue();
            (abstractTypeEmptyName > (IRegistrationKey)null).Should().BeTrue();
            (abstractTypeNoName > (IRegistrationKey)null).Should().BeTrue();

            (interfaceTypeEmptyName > interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeEmptyName > interfaceTypeNoName).Should().BeFalse();
            (interfaceTypeNameA > interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameZ > interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameA > interfaceTypeNameZ).Should().BeFalse();

            (interfaceTypeEmptyName > (IRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeEmptyName > (IRegistrationKey)interfaceTypeNoName).Should().BeFalse();
            (interfaceTypeNameA > (IRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameZ > (IRegistrationKey)interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameA > (IRegistrationKey)interfaceTypeNameZ).Should().BeFalse();

            ((IRegistrationKey)interfaceTypeEmptyName > interfaceTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeEmptyName > interfaceTypeNoName).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameA > interfaceTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameZ > interfaceTypeNameA).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameA > interfaceTypeNameZ).Should().BeFalse();

            // interfaces are checked before names
            (interfaceTypeEmptyName > abstractTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA > abstractTypeNameA).Should().BeTrue();

            (interfaceTypeEmptyName > (IRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA > (IRegistrationKey)abstractTypeNameA).Should().BeTrue();

            ((IRegistrationKey)interfaceTypeEmptyName > abstractTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameA > abstractTypeNameA).Should().BeTrue();
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

            (abstractTypeEmptyName >= (IRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeEmptyName >= (IRegistrationKey)abstractTypeNoName).Should().BeTrue();
            (abstractTypeNameA >= (IRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeNameZ >= (IRegistrationKey)abstractTypeNameA).Should().BeTrue();
            (abstractTypeNameA >= (IRegistrationKey)abstractTypeNameZ).Should().BeFalse();

            ((IRegistrationKey)abstractTypeEmptyName >= abstractTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)abstractTypeEmptyName >= abstractTypeNoName).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameA >= abstractTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameZ >= abstractTypeNameA).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameA >= abstractTypeNameZ).Should().BeFalse();

            (emptyEquivalent >= emptyEquivalent).Should().BeTrue();
            (abstractTypeEmptyName >= emptyEquivalent).Should().BeTrue();
            (abstractTypeNoName >= emptyEquivalent).Should().BeTrue();

            (emptyEquivalent >= (IRegistrationKey)emptyEquivalent).Should().BeTrue();
            (abstractTypeEmptyName >= (IRegistrationKey)emptyEquivalent).Should().BeTrue();
            (abstractTypeNoName >= (IRegistrationKey)emptyEquivalent).Should().BeTrue();

            ((IRegistrationKey)emptyEquivalent >= emptyEquivalent).Should().BeTrue();
            ((IRegistrationKey)abstractTypeEmptyName >= emptyEquivalent).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNoName >= emptyEquivalent).Should().BeTrue();

            (emptyEquivalent >= (IRegistrationKey)null).Should().BeTrue();
            (abstractTypeEmptyName >= (IRegistrationKey)null).Should().BeTrue();
            (abstractTypeNoName >= (IRegistrationKey)null).Should().BeTrue();

            (interfaceTypeEmptyName >= interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeEmptyName >= interfaceTypeNoName).Should().BeTrue();
            (interfaceTypeNameA >= interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameZ >= interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameA >= interfaceTypeNameZ).Should().BeFalse();

            (interfaceTypeEmptyName >= (IRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeEmptyName >= (IRegistrationKey)interfaceTypeNoName).Should().BeTrue();
            (interfaceTypeNameA >= (IRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameZ >= (IRegistrationKey)interfaceTypeNameA).Should().BeTrue();
            (interfaceTypeNameA >= (IRegistrationKey)interfaceTypeNameZ).Should().BeFalse();

            ((IRegistrationKey)interfaceTypeEmptyName >= interfaceTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeEmptyName >= interfaceTypeNoName).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameA >= interfaceTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameZ >= interfaceTypeNameA).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameA >= interfaceTypeNameZ).Should().BeFalse();

            // interfaces are checked before names
            (interfaceTypeEmptyName >= abstractTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA >= abstractTypeNameA).Should().BeTrue();

            (interfaceTypeEmptyName >= (IRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (interfaceTypeNameA >= (IRegistrationKey)abstractTypeNameA).Should().BeTrue();

            ((IRegistrationKey)interfaceTypeEmptyName >= abstractTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameA >= abstractTypeNameA).Should().BeTrue();
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

            (abstractTypeEmptyName != (IRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeNameA != (IRegistrationKey)abstractTypeNameA).Should().BeFalse();
            (abstractTypeNameZ != (IRegistrationKey)abstractTypeNameZ).Should().BeFalse();
            (abstractTypeNoName != (IRegistrationKey)abstractTypeNoName).Should().BeFalse();

            ((IRegistrationKey)abstractTypeEmptyName != abstractTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameA != abstractTypeNameA).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameZ != abstractTypeNameZ).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNoName != abstractTypeNoName).Should().BeFalse();

            (emptyEquivalent != emptyEquivalent).Should().BeFalse();

            (interfaceTypeEmptyName != interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA != interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameZ != interfaceTypeNameZ).Should().BeFalse();
            (interfaceTypeNoName != interfaceTypeNoName).Should().BeFalse();

            (interfaceTypeEmptyName != (IRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA != (IRegistrationKey)interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameZ != (IRegistrationKey)interfaceTypeNameZ).Should().BeFalse();
            (interfaceTypeNoName != (IRegistrationKey)interfaceTypeNoName).Should().BeFalse();

            ((IRegistrationKey)interfaceTypeEmptyName != interfaceTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameA != interfaceTypeNameA).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameZ != interfaceTypeNameZ).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNoName != interfaceTypeNoName).Should().BeFalse();

            (emptyEquivalent != RegistrationKeyTypeNamePair.Empty).Should().BeFalse();

            (abstractTypeEmptyName != interfaceTypeEmptyName).Should().BeTrue();
            (abstractTypeNameA != interfaceTypeNameA).Should().BeTrue();
            (abstractTypeNameZ != interfaceTypeNameZ).Should().BeTrue();
            (abstractTypeNoName != interfaceTypeNoName).Should().BeTrue();

            (abstractTypeEmptyName != (IRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (abstractTypeNameA != (IRegistrationKey)interfaceTypeNameA).Should().BeTrue();
            (abstractTypeNameZ != (IRegistrationKey)interfaceTypeNameZ).Should().BeTrue();
            (abstractTypeNoName != (IRegistrationKey)interfaceTypeNoName).Should().BeTrue();

            ((IRegistrationKey)abstractTypeEmptyName != interfaceTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameA != interfaceTypeNameA).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameZ != interfaceTypeNameZ).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNoName != interfaceTypeNoName).Should().BeTrue();
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

            (abstractTypeEmptyName < (IRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeEmptyName < (IRegistrationKey)abstractTypeNoName).Should().BeFalse();
            (abstractTypeNameA < (IRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeNameZ < (IRegistrationKey)abstractTypeNameA).Should().BeFalse();
            (abstractTypeNameA < (IRegistrationKey)abstractTypeNameZ).Should().BeTrue();

            ((IRegistrationKey)abstractTypeEmptyName < abstractTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)abstractTypeEmptyName < abstractTypeNoName).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameA < abstractTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameZ < abstractTypeNameA).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameA < abstractTypeNameZ).Should().BeTrue();

            (emptyEquivalent < emptyEquivalent).Should().BeFalse();
            (abstractTypeEmptyName < emptyEquivalent).Should().BeFalse();
            (abstractTypeNoName < emptyEquivalent).Should().BeFalse();

            (emptyEquivalent < (IRegistrationKey)emptyEquivalent).Should().BeFalse();
            (abstractTypeEmptyName < (IRegistrationKey)emptyEquivalent).Should().BeFalse();
            (abstractTypeNoName < (IRegistrationKey)emptyEquivalent).Should().BeFalse();

            ((IRegistrationKey)emptyEquivalent < emptyEquivalent).Should().BeFalse();
            ((IRegistrationKey)abstractTypeEmptyName < emptyEquivalent).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNoName < emptyEquivalent).Should().BeFalse();

            (emptyEquivalent < (IRegistrationKey)null).Should().BeFalse();
            (abstractTypeEmptyName < (IRegistrationKey)null).Should().BeFalse();
            (abstractTypeNoName < (IRegistrationKey)null).Should().BeFalse();

            (interfaceTypeEmptyName < interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeEmptyName < interfaceTypeNoName).Should().BeFalse();
            (interfaceTypeNameA < interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameZ < interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameA < interfaceTypeNameZ).Should().BeTrue();

            (interfaceTypeEmptyName < (IRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeEmptyName < (IRegistrationKey)interfaceTypeNoName).Should().BeFalse();
            (interfaceTypeNameA < (IRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameZ < (IRegistrationKey)interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameA < (IRegistrationKey)interfaceTypeNameZ).Should().BeTrue();

            ((IRegistrationKey)interfaceTypeEmptyName < interfaceTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeEmptyName < interfaceTypeNoName).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameA < interfaceTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameZ < interfaceTypeNameA).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameA < interfaceTypeNameZ).Should().BeTrue();

            // interfaces are checked before names
            (interfaceTypeEmptyName < abstractTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA < abstractTypeNameA).Should().BeFalse();

            (interfaceTypeEmptyName < (IRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA < (IRegistrationKey)abstractTypeNameA).Should().BeFalse();

            ((IRegistrationKey)interfaceTypeEmptyName < abstractTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameA < abstractTypeNameA).Should().BeFalse();
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

            (abstractTypeEmptyName <= (IRegistrationKey)abstractTypeEmptyName).Should().BeTrue();
            (abstractTypeEmptyName <= (IRegistrationKey)abstractTypeNoName).Should().BeTrue();
            (abstractTypeNameA <= (IRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (abstractTypeNameZ <= (IRegistrationKey)abstractTypeNameA).Should().BeFalse();
            (abstractTypeNameA <= (IRegistrationKey)abstractTypeNameZ).Should().BeTrue();

            ((IRegistrationKey)abstractTypeEmptyName <= abstractTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)abstractTypeEmptyName <= abstractTypeNoName).Should().BeTrue();
            ((IRegistrationKey)abstractTypeNameA <= abstractTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameZ <= abstractTypeNameA).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNameA <= abstractTypeNameZ).Should().BeTrue();

            (emptyEquivalent <= emptyEquivalent).Should().BeTrue();
            (abstractTypeEmptyName <= emptyEquivalent).Should().BeFalse();
            (abstractTypeNoName <= emptyEquivalent).Should().BeFalse();

            (emptyEquivalent <= (IRegistrationKey)emptyEquivalent).Should().BeTrue();
            (abstractTypeEmptyName <= (IRegistrationKey)emptyEquivalent).Should().BeFalse();
            (abstractTypeNoName <= (IRegistrationKey)emptyEquivalent).Should().BeFalse();

            ((IRegistrationKey)emptyEquivalent <= emptyEquivalent).Should().BeTrue();
            ((IRegistrationKey)abstractTypeEmptyName <= emptyEquivalent).Should().BeFalse();
            ((IRegistrationKey)abstractTypeNoName <= emptyEquivalent).Should().BeFalse();

            (emptyEquivalent <= (IRegistrationKey)null).Should().BeFalse();
            (abstractTypeEmptyName <= (IRegistrationKey)null).Should().BeFalse();
            (abstractTypeNoName <= (IRegistrationKey)null).Should().BeFalse();

            (interfaceTypeEmptyName <= interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeEmptyName <= interfaceTypeNoName).Should().BeTrue();
            (interfaceTypeNameA <= interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameZ <= interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameA <= interfaceTypeNameZ).Should().BeTrue();

            (interfaceTypeEmptyName <= (IRegistrationKey)interfaceTypeEmptyName).Should().BeTrue();
            (interfaceTypeEmptyName <= (IRegistrationKey)interfaceTypeNoName).Should().BeTrue();
            (interfaceTypeNameA <= (IRegistrationKey)interfaceTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameZ <= (IRegistrationKey)interfaceTypeNameA).Should().BeFalse();
            (interfaceTypeNameA <= (IRegistrationKey)interfaceTypeNameZ).Should().BeTrue();

            ((IRegistrationKey)interfaceTypeEmptyName <= interfaceTypeEmptyName).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeEmptyName <= interfaceTypeNoName).Should().BeTrue();
            ((IRegistrationKey)interfaceTypeNameA <= interfaceTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameZ <= interfaceTypeNameA).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameA <= interfaceTypeNameZ).Should().BeTrue();

            // interfaces are checked before names
            (interfaceTypeEmptyName <= abstractTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA <= abstractTypeNameA).Should().BeFalse();

            (interfaceTypeEmptyName <= (IRegistrationKey)abstractTypeEmptyName).Should().BeFalse();
            (interfaceTypeNameA <= (IRegistrationKey)abstractTypeNameA).Should().BeFalse();

            ((IRegistrationKey)interfaceTypeEmptyName <= abstractTypeEmptyName).Should().BeFalse();
            ((IRegistrationKey)interfaceTypeNameA <= abstractTypeNameA).Should().BeFalse();
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
