namespace Landorphan.Abstractions.Tests.Internal.Environment
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using FluentAssertions;
   using Landorphan.Abstractions.Internal;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class EnvironmentVariable_Tests
   {
      private const String Whitespace = " \t ";

      [TestClass]
      public class When_I_call_EnvironmentVariable_ToString : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_convert_as_expected()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var name = "myName";
            var value = "1";
            var target = new EnvironmentVariable(name, value);
            target.ToString().Should().Be(String.Format(CultureInfo.InvariantCulture, "Name: {0}, Value: {1}", name, value));

            name = "Another Name";
            value = null;
            target = new EnvironmentVariable(name, value);
            target.ToString().Should().Be(String.Format(CultureInfo.InvariantCulture, "Name: {0}, Value: {1}", name, String.Empty));

            // ReSharper restore ExpressionIsAlwaysNull
         }
      }

      [TestClass]
      public class When_I_compare_for_EnvironmentVariable_for_equality : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_distinguish_dissimilar_instances()
         {
            // ReSharper disable ExpressionIsAlwaysNull
            var first = new EnvironmentVariable(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), null);
            var second = new EnvironmentVariable(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), null);
            EnvironmentVariable third = null;
            Object fourth = new EnvironmentVariable(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), null);
            var obj = new Object();

            first.Equals(second).Should().BeFalse();
            second.Equals(first).Should().BeFalse();
            first.GetHashCode().Should().NotBe(second.GetHashCode());

            first.Equals(third).Should().BeFalse();

            first.Equals(fourth).Should().BeFalse();
            fourth.Equals(first).Should().BeFalse();

            first.Equals(obj).Should().BeFalse();
            obj.Equals(first).Should().BeFalse();

            // ReSharper restore ExpressionIsAlwaysNull
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Test Code (MWP)")]
         public void It_should_employ_value_semantics_without_case_sensitivity()
         {
            var name = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var value = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var first = new EnvironmentVariable(name.ToUpperInvariant(), value.ToUpperInvariant());
            var second = new EnvironmentVariable(name.ToLowerInvariant(), value.ToLowerInvariant());

            first.Equals(second).Should().BeTrue();
            second.Equals(first).Should().BeTrue();

            first.GetHashCode().Should().Be(second.GetHashCode());

            first.Should().NotBeSameAs(second);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Test Code (MWP)")]
         public void It_should_handle_null_values()
         {
            var name = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var first = new EnvironmentVariable(name.ToUpperInvariant(), String.Empty);
            var second = new EnvironmentVariable(name.ToLowerInvariant(), String.Empty);

            first.Equals(second).Should().BeTrue();
            second.Equals(first).Should().BeTrue();

            first.GetHashCode().Should().Be(second.GetHashCode());

            first.Should().NotBeSameAs(second);
         }
      }

      [TestClass]
      public class When_I_construct_an_EnvironmentVariable : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_accept_null_values()
         {
            var expectedName = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            String expectedValue = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var actual = new EnvironmentVariable(expectedName, expectedValue);
            actual.Name.Should().Be(expectedName);
            actual.Value.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_coalesce_names_to_empty()
         {
            var expectedName = String.Empty;
            var expectedValue = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var actual = new EnvironmentVariable(null, expectedValue);
            actual.Name.Should().Be(expectedName);
            actual.Value.Should().Be(expectedValue);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_faithfully_transpose_the_argument_values_to_properties()
         {
            var expectedName = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var expectedValue = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var actual = new EnvironmentVariable(expectedName, expectedValue);
            actual.Name.Should().Be(expectedName);
            actual.Value.Should().Be(expectedValue);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_trim_names()
         {
            var expectedName = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var expectedValue = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            var actual = new EnvironmentVariable(Whitespace + expectedName + Whitespace, expectedValue);
            actual.Name.Should().Be(expectedName);
            actual.Value.Should().Be(expectedValue);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_trim_values()
         {
            var expectedName = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var expectedValue = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            // ReSharper disable once ExpressionIsAlwaysNull
            var actual = new EnvironmentVariable(expectedName, Whitespace + expectedValue + Whitespace);
            actual.Name.Should().Be(expectedName);
            actual.Value.Should().Be(expectedValue);
         }
      }
   }
}
