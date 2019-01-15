namespace Landorphan.Common.Tests
{
   using System;
   using System.Globalization;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class ReferenceEqualityComparer_Tests
   {
      [TestClass]
      public class When_I_call_GetHashCode : ArrangeActAssert
      {
         private ReferenceEqualityComparer<MyClass> target;

         protected override void ArrangeMethod()
         {
            target = new ReferenceEqualityComparer<MyClass>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_object_GetHashCode_for_non_null_values()
         {
            var instance = new MyClass(Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));
            var instanceHashCode = instance.GetHashCode();

            var actual = target.GetHashCode(instance);
            actual.Should().Be(instanceHashCode);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_zero_for_null_values()
         {
            // ReSharper disable once AssignNullToNotNullAttribute
            var actual = target.GetHashCode(null);
            actual.Should().Be(0);
         }
      }

      [TestClass]
      public class When_I_call_ReferenceEqualityComparer_Equals : ArrangeActAssert
      {
         private readonly ReferenceEqualityComparer<Object> target = new ReferenceEqualityComparer<Object>();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_when_x_and_y_are_unique_instances()
         {
            var actual = target.Equals(new Object(), new Object());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_when_x_is_not_null_and_y_is_null()
         {
            var actual = target.Equals(new Object(), null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_false_when_x_is_null_and_y_is_not_null()
         {
            var actual = target.Equals(null, new Object());
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_true_when_both_x_and_y_are_null()
         {
            var actual = target.Equals(null, null);
            actual.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_true_when_x_and_y_are_the_same_instance()
         {
            var obj = new Object();
            var actual = target.Equals(obj, obj);
            actual.Should().BeTrue();
         }
      }

      private class MyClass
      {
         public MyClass(String name)
         {
            Name = name;
         }

         // ReSharper disable once MemberCanBePrivate.Local
         internal String Name { get; }

         public override Int32 GetHashCode()
         {
            if (Name == null)
            {
               return 0;
            }

            var rv = Name.GetHashCode();
            return rv;
         }
      }
   }
}
