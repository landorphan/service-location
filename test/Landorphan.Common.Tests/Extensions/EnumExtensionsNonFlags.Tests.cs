namespace Landorphan.Common.Tests.Extensions
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable UnusedMember.Local

   [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
   public static class EnumExtensionsNonFlags_Tests
   {
      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      [SuppressMessage("SonarLint.CodeSmell", "S4022: Enumerations should have Int32 storage")]
      private enum TestTargetByteEnum : byte
      {
         None,
         First,
         Second,
         Third,
         Fourth,
         Fifth
      }

      private enum TestTargetEnumWithDuplicates
      {
         None,
         First = 1,
         One = 1,
         Second
      }

      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      [Flags]
      private enum TestTargetFlagsEnum
      {
         None = 0,
         One = 1,
         Two = 2,
         Three = 3
      }

      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      [SuppressMessage("SonarLint.CodeSmell", "S4022: Enumerations should have Int32 storage")]
      private enum TestTargetInt16Enum : short
      {
         None,
         First,
         Second,
         Third,
         Fourth,
         Fifth
      }

      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      private enum TestTargetInt32Enum
      {
         None,
         First,
         Second,
         Third,
         Fourth,
         Fifth
      }

      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      [SuppressMessage("SonarLint.CodeSmell", "S4022: Enumerations should have Int32 storage")]
      private enum TestTargetInt64Enum : long
      {
         None,
         First,
         Second,
         Third,
         Fourth,
         Fifth
      }

      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      [SuppressMessage("SonarLint.CodeSmell", "S4022: Enumerations should have Int32 storage")]
      private enum TestTargetMaxUnsignedEnum : ulong
      {
         None,
         First,
         Second,
         Third,
         Fourth,
         Fifth,
         Max = UInt64.MaxValue
      }

      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      [SuppressMessage("SonarLint.CodeSmell", "S4022: Enumerations should have Int32 storage")]
      private enum TestTargetSByteEnum : sbyte
      {
         None,
         First,
         Second,
         Third,
         Fourth,
         Fifth
      }

      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      [SuppressMessage("SonarLint.CodeSmell", "S4022: Enumerations should have Int32 storage")]
      private enum TestTargetUInt16Enum : ushort
      {
         None,
         First,
         Second,
         Third,
         Fourth,
         Fifth
      }

      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      [SuppressMessage("SonarLint.CodeSmell", "S4022: Enumerations should have Int32 storage")]
      private enum TestTargetUInt32Enum : uint
      {
         None,
         First,
         Second,
         Third,
         Fourth,
         Fifth
      }

      [SuppressMessage("SonarLint.Naming", "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes", Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S2342: Enumeration types should comply with a naming convention")]
      [SuppressMessage("SonarLint.CodeSmell", "S4022: Enumerations should have Int32 storage")]
      private enum TestTargetUInt64Enum : ulong
      {
         None,
         First,
         Second,
         Third,
         Fourth,
         Fifth
      }

      [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
      [SuppressMessage("SonarLint.Naming", "S101:Types should be named in PascalCase", Justification = "Test code (MWP)")]
      [SuppressMessage("Microsoft.Design", "CA1034:Nested types should not be visible", Justification = "Test code (MWP)")]
      [TestClass]
      public class When_I_call_NonFlagsEnumExtensions_ArgumentMustBeValidFlagsEnumValue : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_enum_value_is_valid()
         {
            const String parameterName = "arg";

            // ensure UInt64.MaxValue does not cause overflow.
            TestTargetMaxUnsignedEnum.Max.ArgumentMustBeValidEnumValue(parameterName);

            // test more typical scenarios
            TestTargetSByteEnum.Fifth.ArgumentMustBeValidEnumValue(parameterName);
            TestTargetByteEnum.Fifth.ArgumentMustBeValidEnumValue(parameterName);
            TestTargetInt16Enum.Fifth.ArgumentMustBeValidEnumValue(parameterName);
            TestTargetUInt16Enum.Fifth.ArgumentMustBeValidEnumValue(parameterName);
            TestTargetInt32Enum.Fifth.ArgumentMustBeValidEnumValue(parameterName);
            TestTargetUInt32Enum.Fifth.ArgumentMustBeValidEnumValue(parameterName);
            TestTargetInt64Enum.Fifth.ArgumentMustBeValidEnumValue(parameterName);
            TestTargetUInt64Enum.Fifth.ArgumentMustBeValidEnumValue(parameterName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ExtendedInvalidEnumArgumentException_when_the_enum_value_is_invalid()
         {
            const String parameterName = "arg";

            const Int32 invalidValue = 1 + (Int32)TestTargetInt32Enum.Fifth;

            Action throwingAction = () => ((TestTargetInt32Enum)invalidValue).ArgumentMustBeValidEnumValue(parameterName);
            var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.EnumType.Should().Be<TestTargetInt32Enum>();
            e.And.UnderlyingType.Should().Be<Int32>();
            e.And.InvalidValue.Should().Be(invalidValue);
            e.And.ParamName.Should().Be(parameterName);
            e.And.Message.Should()
               .Contain("The value of argument")
               .And.Contain("is invalid for Enum type")
               .And.Contain(parameterName)
               .And.Contain(((Int64)invalidValue).ToString(CultureInfo.InvariantCulture))
               .And.Contain(typeof(TestTargetInt32Enum).Name);

            // The value of argument 'arg' (6) is invalid for Enum type 'TestTargetInt32Enum'.
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_InvalidOperationException_when_the_value_is_an_enum_but_is_flags()
         {
            const TestTargetFlagsEnum value = TestTargetFlagsEnum.Three;
            Action throwingAction = () => value.ArgumentMustBeValidEnumValue(null);
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*is attributed with [Flags].");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_InvalidOperationException_when_the_value_is_not_an_enum()
         {
            const Int32 value = 666;

            Action throwingAction = () => value.ArgumentMustBeValidEnumValue(null);
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*is not an enumeration type.");
         }
      }

      [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
      [SuppressMessage("SonarLint.Naming", "S101:Types should be named in PascalCase", Justification = "Test code (MWP)")]
      public class When_I_call_NonFlagsEnumExtensions_IsValidEnumValue : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_return_true_when_the_enum_value_is_valid()
         {
            // ensure UInt64.MaxValue does not cause overflow.
            TestTargetMaxUnsignedEnum.Max.IsValidEnumValue().Should().BeTrue();

            // test more typical scenarios
            TestTargetSByteEnum.None.IsValidEnumValue().Should().BeTrue();
            TestTargetByteEnum.First.IsValidEnumValue().Should().BeTrue();
            TestTargetInt16Enum.Second.IsValidEnumValue().Should().BeTrue();
            TestTargetUInt16Enum.Third.IsValidEnumValue().Should().BeTrue();
            TestTargetInt32Enum.Fourth.IsValidEnumValue().Should().BeTrue();
            TestTargetUInt32Enum.Fifth.IsValidEnumValue().Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_enum_value_is_invalid()
         {
            const Int32 invalidValue = 1 + (Int32)TestTargetInt32Enum.Fifth;
            const TestTargetInt32Enum invalidEnum = (TestTargetInt32Enum)invalidValue;
            invalidEnum.IsValidEnumValue().Should().BeFalse();
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_InvalidOperationException_when_the_value_is_an_enum_but_is_flags()
         {
            const TestTargetFlagsEnum value = TestTargetFlagsEnum.Three;
            Action throwingAction = () => value.ArgumentMustBeValidEnumValue(null);
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*is attributed with [Flags].");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_InvalidOperationException_when_the_value_is_not_an_enum()
         {
            const Int32 value = 666;

            Action throwingAction = () => value.IsValidFlagsValue();
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*is not an enumeration type*");
         }
      }
   }
}
