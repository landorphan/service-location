namespace Landorphan.Common.Tests.Extensions
{
   using System;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class When_I_call_IsNotNull : ArrangeActAssert
   {
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_return_false_when_the_value_is_null()
      {
         const String value = null;
         var isNotNull = value.IsNotNull();
         isNotNull.Should().BeFalse();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_return_true_when_the_value_is_not_null()
      {
         const String value = "MyValue";
         var isNotNull = value.IsNotNull();
         isNotNull.Should().BeTrue();
      }
   }

   [TestClass]
   public class When_I_call_IsNull : ArrangeActAssert
   {
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_return_false_when_the_value_is_not_null()
      {
         const String value = "MyValue";
         var isNull = value.IsNull();
         isNull.Should().BeFalse();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_return_true_when_the_value_is_null()
      {
         const String value = null;
         var isNull = value.IsNull();
         isNull.Should().BeTrue();
      }
   }
}