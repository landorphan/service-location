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
   public static class FlagsEnumExtensions_Tests
   {
      [SuppressMessage(
         "SonarLint.Naming",
         "S2342:Enumeration types should comply with a naming convention",
         Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.Design", "S4022:Enumerations should have 'Int32' storage", Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Design",
         "S4070:Non-flags enums should not be marked with 'FlagsAttribute'",
         Justification = "Test target (MWP)")]
      [Flags]
      private enum TestTargetFilledUnsignedFlagsEnum : ulong

      {
         None = 0x00,
         First = 0x01,
         Second = 0x02,
         Third = 0x04,
         Fourth = 0x08,
         Fifth = 0x10,
         All = UInt64.MaxValue
      }

      [Flags]
      private enum TestTargetFlagsEnumWithDuplicates
      {
         None = 0x00,
         First = 0x01,
         One = 0x01,
         Second = 0x02
      }

      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      private enum TestTargetNonFlagsEnum
      {
         None = 0,
         One = 1,
         Two = 2,
         Three = 3
      }

      [SuppressMessage(
         "SonarLint.Naming",
         "S2342:Enumeration types should comply with a naming convention",
         Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      [Flags]
      [SuppressMessage("SonarLint.Design", "S4022:Enumerations should have 'Int32' storage", Justification = "Test target (MWP)")]
      private enum TestTargetUnfilledByteFlagsEnum : byte
      {
         None = 0x00,
         First = 0x01,
         Second = 0x02,
         Third = 0x04,
         Fourth = 0x08,
         Fifth = 0x10
      }

      [SuppressMessage(
         "SonarLint.Naming",
         "S2342:Enumeration types should comply with a naming convention",
         Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.Design", "S4022:Enumerations should have 'Int32' storage", Justification = "Test target (MWP)")]
      [Flags]
      private enum TestTargetUnfilledInt16FlagsEnum : short
      {
         None = 0x00,
         First = 0x01,
         Second = 0x02,
         Third = 0x04,
         Fourth = 0x08,
         Fifth = 0x10
      }

      [SuppressMessage(
         "SonarLint.Naming",
         "S2342:Enumeration types should comply with a naming convention",
         Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      [Flags]
      private enum TestTargetUnfilledInt32FlagsEnum
      {
         None = 0x00,
         First = 0x01,
         Second = 0x02,
         Third = 0x04,
         Fourth = 0x08,
         Fifth = 0x10
      }

      [SuppressMessage(
         "SonarLint.Naming",
         "S2342:Enumeration types should comply with a naming convention",
         Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.Design", "S4022:Enumerations should have 'Int32' storage", Justification = "Test target (MWP)")]
      [Flags]
      private enum TestTargetUnfilledInt64FlagsEnum : long
      {
         None = 0x00,
         First = 0x01,
         Second = 0x02,
         Third = 0x04,
         Fourth = 0x08,
         Fifth = 0x10
      }

      [SuppressMessage(
         "SonarLint.Naming",
         "S2342:Enumeration types should comply with a naming convention",
         Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.Design", "S4022:Enumerations should have 'Int32' storage", Justification = "Test target (MWP)")]
      [Flags]
      private enum TestTargetUnfilledSByteFlagsEnum : sbyte
      {
         None = 0x00,
         First = 0x01,
         Second = 0x02,
         Third = 0x04,
         Fourth = 0x08,
         Fifth = 0x10
      }

      [SuppressMessage(
         "SonarLint.Naming",
         "S2342:Enumeration types should comply with a naming convention",
         Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.Design", "S4022:Enumerations should have 'Int32' storage", Justification = "Test target (MWP)")]
      [Flags]
      private enum TestTargetUnfilledUInt16FlagsEnum : ushort
      {
         None = 0x00,
         First = 0x01,
         Second = 0x02,
         Third = 0x04,
         Fourth = 0x08,
         Fifth = 0x10
      }

      [SuppressMessage(
         "SonarLint.Naming",
         "S2342:Enumeration types should comply with a naming convention",
         Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.Design", "S4022:Enumerations should have 'Int32' storage", Justification = "Test target (MWP)")]
      [Flags]
      private enum TestTargetUnfilledUInt32FlagsEnum : uint
      {
         None = 0x00,
         First = 0x01,
         Second = 0x02,
         Third = 0x04,
         Fourth = 0x08,
         Fifth = 0x10
      }

      [SuppressMessage(
         "SonarLint.Naming",
         "S2342:Enumeration types should comply with a naming convention",
         Justification = "Test target (MWP)")]
      [SuppressMessage(
         "SonarLint.Naming",
         "S2344:Enumeration type names should not have 'Flags' or 'Enum' suffixes",
         Justification = "Test target (MWP)")]
      [SuppressMessage("SonarLint.Design", "S4022:Enumerations should have 'Int32' storage", Justification = "Test target (MWP)")]
      [Flags]
      private enum TestTargetUnfilledUInt64FlagsEnum : ulong
      {
         None = 0x00,
         First = 0x01,
         Second = 0x02,
         Third = 0x04,
         Fourth = 0x08,
         Fifth = 0x10
      }

      //[SuppressMessage("SonarLint.Naming", "S101:Types should be named in PascalCase", Justification = "Test code (MWP)")]
      [SuppressMessage("Microsoft.Design", "CA1034:Nested types should not be visible", Justification = "Test code (MWP)")]
      [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
      [TestClass]
      public class When_I_call_FlagsEnumExtensions_ArgumentMustBeValidFlagsEnumValue : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_when_the_enum_value_is_valid()
         {
            const String parameterName = "arg";

            // ensure UInt64.MaxValue does not cause overflow.
            const TestTargetFilledUnsignedFlagsEnum value = TestTargetFilledUnsignedFlagsEnum.All;
            value.ArgumentMustBeValidFlagsEnumValue(parameterName);

            // test more typical scenarios

            Int64 allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledSByteFlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            ((TestTargetUnfilledSByteFlagsEnum)allFlags).ArgumentMustBeValidFlagsEnumValue(parameterName);

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledByteFlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            ((TestTargetUnfilledByteFlagsEnum)allFlags).ArgumentMustBeValidFlagsEnumValue(parameterName);

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledInt16FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            ((TestTargetUnfilledInt16FlagsEnum)allFlags).ArgumentMustBeValidFlagsEnumValue(parameterName);

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledUInt16FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            ((TestTargetUnfilledUInt16FlagsEnum)allFlags).ArgumentMustBeValidFlagsEnumValue(parameterName);

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledInt32FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            ((TestTargetUnfilledInt32FlagsEnum)allFlags).ArgumentMustBeValidFlagsEnumValue(parameterName);

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledUInt32FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            ((TestTargetUnfilledUInt32FlagsEnum)allFlags).ArgumentMustBeValidFlagsEnumValue(parameterName);

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledInt64FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            ((TestTargetUnfilledInt64FlagsEnum)allFlags).ArgumentMustBeValidFlagsEnumValue(parameterName);

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledUInt64FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            ((TestTargetUnfilledUInt64FlagsEnum)allFlags).ArgumentMustBeValidFlagsEnumValue(parameterName);
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ExtendedInvalidEnumArgumentException_when_evaluating_invalid_flags()
         {
            const String parameterName = "arg";

            Int64 allFlagsCombined = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledInt32FlagsEnum)))
            {
               allFlagsCombined |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            var value = (TestTargetUnfilledInt32FlagsEnum)(allFlagsCombined + 1);
            Action throwingAction = () => value.ArgumentMustBeValidFlagsEnumValue(parameterName);
            var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.EnumType.Should().Be(typeof(TestTargetUnfilledInt32FlagsEnum));
            e.And.UnderlyingType.Should().Be<Int32>();
            e.And.InvalidValue.Should().Be(allFlagsCombined + 1);
            e.And.ParamName.Should().Be(parameterName);
            e.And.Message.Should()
               .Contain("The value of argument")
               .And.Contain("is invalid for Enum type")
               .And.Contain(parameterName)
               .And.Contain(((Int64)value).ToString(CultureInfo.InvariantCulture))
               .And.Contain(typeof(TestTargetUnfilledInt32FlagsEnum).Name);
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_InvalidOperationException_when_the_value_is_an_enum_but_not_flags()
         {
            const ConsoleColor value = ConsoleColor.Black;
            Action throwingAction = () => value.ArgumentMustBeValidFlagsEnumValue(null);
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*is not an attributed with [Flags].");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_InvalidOperationException_when_the_value_is_not_an_enum()
         {
            const Int32 value = 666;

            Action throwingAction = () => value.ArgumentMustBeValidFlagsEnumValue(null);
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*is not an enumeration type.");
         }
      }

      [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
      [SuppressMessage("Microsoft.Design", "CA1034:Nested types should not be visible", Justification = "Test code (MWP)")]
      [SuppressMessage("SonarLint.Naming", "S101:Types should be named in PascalCase", Justification = "Test code (MWP)")]
      [TestClass]
      public class When_I_call_FlagsEnumExtensions_IsFlagSet : TestBase
      {
         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_enums_with_duplicate_flags()
         {
            const TestTargetFlagsEnumWithDuplicates value =
               TestTargetFlagsEnumWithDuplicates.First | TestTargetFlagsEnumWithDuplicates.Second;
            value.IsFlagSet(TestTargetFlagsEnumWithDuplicates.One).Should().BeTrue();
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_multiple_flags_are_set_excluding_the_examined_flag()
         {
            const TestTargetFilledUnsignedFlagsEnum value =
               TestTargetFilledUnsignedFlagsEnum.First | TestTargetFilledUnsignedFlagsEnum.Third;
            value.IsFlagSet(TestTargetFilledUnsignedFlagsEnum.Second).Should().BeFalse();
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_multiple_mixed_flags_are_queried()
         {
            const TestTargetFilledUnsignedFlagsEnum value =
               TestTargetFilledUnsignedFlagsEnum.First | TestTargetFilledUnsignedFlagsEnum.Second | TestTargetFilledUnsignedFlagsEnum.Third;
            value.IsFlagSet(
                  TestTargetFilledUnsignedFlagsEnum.First |
                  TestTargetFilledUnsignedFlagsEnum.Third |
                  TestTargetFilledUnsignedFlagsEnum.Fourth)
               .Should()
               .BeFalse();
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_no_flags_are_set()
         {
            const TestTargetFilledUnsignedFlagsEnum value = TestTargetFilledUnsignedFlagsEnum.None;
            value.IsFlagSet(TestTargetFilledUnsignedFlagsEnum.Fifth).Should().BeFalse();
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_many_flags_are_set_including_the_examined_flag()
         {
            const TestTargetFilledUnsignedFlagsEnum value =
               TestTargetFilledUnsignedFlagsEnum.First | TestTargetFilledUnsignedFlagsEnum.Second | TestTargetFilledUnsignedFlagsEnum.Third;
            value.IsFlagSet(TestTargetFilledUnsignedFlagsEnum.Second).Should().BeTrue();
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_multiple_set_flags_are_queried()
         {
            const TestTargetFilledUnsignedFlagsEnum value =
               TestTargetFilledUnsignedFlagsEnum.First | TestTargetFilledUnsignedFlagsEnum.Second | TestTargetFilledUnsignedFlagsEnum.Third;
            value.IsFlagSet(TestTargetFilledUnsignedFlagsEnum.First | TestTargetFilledUnsignedFlagsEnum.Third).Should().BeTrue();
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_only_the_flag_itself_is_set()
         {
            const TestTargetFilledUnsignedFlagsEnum value = TestTargetFilledUnsignedFlagsEnum.Second;
            value.IsFlagSet(TestTargetFilledUnsignedFlagsEnum.Second).Should().BeTrue();
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_InvalidOperationException_when_the_value_passed_is_not_a_flags_enumeration()
         {
            const TestTargetNonFlagsEnum value = TestTargetNonFlagsEnum.Three;
            Action throwingAction = () => value.IsFlagSet(TestTargetNonFlagsEnum.Two);
            throwingAction.Should()
               .Throw<InvalidOperationException>()
               .WithMessage("*TestTargetNonFlagsEnum is not an attributed with [Flags]*");
         }
      }

      [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
      [SuppressMessage("Microsoft.Design", "CA1034:Nested types should not be visible", Justification = "Test code (MWP)")]
      [SuppressMessage("SonarLint.Naming", "S101:Types should be named in PascalCase", Justification = "Test code (MWP)")]
      [TestClass]
      public class When_I_call_FlagsEnumExtensions_IsValidFlagsValue : TestBase
      {
         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_return_true_when_the_enum_value_is_valid()
         {
            // ensure UInt64.MaxValue does not cause overflow.
            const TestTargetFilledUnsignedFlagsEnum value = TestTargetFilledUnsignedFlagsEnum.All;
            value.IsValidFlagsValue().Should().BeTrue();

            // test more typical scenarios

            Int64 allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledSByteFlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            var sbyteValue = (TestTargetUnfilledSByteFlagsEnum)allFlags;
            sbyteValue.IsValidFlagsValue().Should().BeTrue();

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledByteFlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            var byteValue = (TestTargetUnfilledByteFlagsEnum)allFlags;
            byteValue.IsValidFlagsValue().Should().BeTrue();

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledInt16FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            var int16Value = (TestTargetUnfilledInt16FlagsEnum)allFlags;
            int16Value.IsValidFlagsValue().Should().BeTrue();

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledUInt16FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            var uint16Value = (TestTargetUnfilledUInt16FlagsEnum)allFlags;
            uint16Value.IsValidFlagsValue().Should().BeTrue();

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledInt32FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            var int32Value = (TestTargetUnfilledInt32FlagsEnum)allFlags;
            int32Value.IsValidFlagsValue().Should().BeTrue();

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledUInt32FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            var uint32Value = (TestTargetUnfilledUInt32FlagsEnum)allFlags;
            uint32Value.IsValidFlagsValue().Should().BeTrue();

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledInt64FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            var int64Value = (TestTargetUnfilledInt64FlagsEnum)allFlags;
            int64Value.IsValidFlagsValue().Should().BeTrue();

            // ---

            allFlags = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledUInt64FlagsEnum)))
            {
               allFlags |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            allFlags.Should().Be(31);
            var uint64Value = (TestTargetUnfilledUInt64FlagsEnum)allFlags;
            uint64Value.IsValidFlagsValue().Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_enum_value_is_invalid()
         {
            Int64 allFlagsCombined = 0;
            foreach (var flag in Enum.GetValues(typeof(TestTargetUnfilledInt32FlagsEnum)))
            {
               allFlagsCombined |= Convert.ToInt64(flag, CultureInfo.InvariantCulture);
            }

            var outOfRangeValue = allFlagsCombined + 1;
            var value = (TestTargetUnfilledInt32FlagsEnum)outOfRangeValue;
            value.IsValidFlagsValue().Should().BeFalse();
         }

         [SuppressMessage("Microsoft.Naming", "CA1726: Use preferred terms")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_InvalidOperationException_when_the_value_is_an_enum_but_not_flags()
         {
            const ConsoleColor value = ConsoleColor.Black;
            Action throwingAction = () => value.IsValidFlagsValue();
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*is not an attributed with [Flags].");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_InvalidOperationException_when_the_value_is_not_an_enum()
         {
            const Int32 value = 666;

            Action throwingAction = () => value.IsValidFlagsValue();
            throwingAction.Should().Throw<InvalidOperationException>().WithMessage("*is not an enumeration type.*");
         }
      }
   }
}
