namespace Landorphan.Common.Tests.Extensions
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class DateTimeExtension_Tests
   {
      public static class BclBehavior
      {
         [TestClass]
         public class Bcl_DateTime_Expected_Behavior_Equality : ArrangeActAssert
         {
            private readonly DateTime _local = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Local);
            private readonly DateTime _unspecified = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Unspecified);
            private readonly DateTime _utc = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc);

            // for better or worse, the BCL only checks for tick equality, and ignores value.Kind
            // equivalent dates expressed in local and UTC will be considered NOT equal as a result

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_Treats_Local_And_Unspecified_As_Equals_When_Ticks_Are_Equal()
            {
               _local.Should().Be(_unspecified);
               _local.Ticks.Should().Be(_unspecified.Ticks);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_Treats_Local_And_Utc_As_Equals_When_Ticks_Are_Equal()
            {
               _local.Should().Be(_utc);
               _local.Ticks.Should().Be(_utc.Ticks);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_Treats_Unspecified_And_Utc_As_Equals_When_Ticks_Are_Equal()
            {
               _unspecified.Should().Be(_utc);
               _unspecified.Ticks.Should().Be(_utc.Ticks);
            }
         }

         [TestClass]
         public class Bcl_DateTime_Expected_Behavior_ToLocalTime : ArrangeActAssert
         {
            private readonly DateTime _local = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Local);
            private readonly DateTime _unspecified = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Unspecified);
            private readonly DateTime _utc = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc);

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTime_ToLocalTime_On_Local_Kind_HasNoEffect()
            {
               var convertedToLocal = _local.ToLocalTime();
               convertedToLocal.Should().Be(_local);
               convertedToLocal.Ticks.Should().Be(_local.Ticks);
               convertedToLocal.Kind.Should().Be(DateTimeKind.Local);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTime_ToLocalTime_On_Unspecified_Kind_ChangesBothKindAndTicks_As_If_It_Were_Utc()
            {
               // this behavior is opposite of how DateTimeOffset treats unspecified
               if (TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
               {
                  Assert.Inconclusive("This test is inconclusive when run in UTC(0).");
               }

               var convertedToLocal = _unspecified.ToLocalTime();
               convertedToLocal.Should().NotBe(_unspecified);
               // here be the rub:
               convertedToLocal.Ticks.Should().NotBe(_unspecified.Ticks);
               convertedToLocal.Kind.Should().Be(DateTimeKind.Local);

               // here be the rub:  unspecified to Local, treats source as UTC.  unspecified to UTC treats, source as local.
               convertedToLocal.Should().Be(_utc.ToLocalTime());
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTime_ToLocalTime_On_Utc_Kind_ChangesBothKindAndTicks()
            {
               if (TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
               {
                  Assert.Inconclusive("This test is inconclusive when run in UTC(0).");
               }

               var convertedToLocal = _utc.ToLocalTime();
               convertedToLocal.Should().NotBe(_utc);
               convertedToLocal.Ticks.Should().NotBe(_utc.Ticks);
               convertedToLocal.Kind.Should().Be(DateTimeKind.Local);
            }
         }

         [TestClass]
         [SuppressMessage(
            "Microsoft.Globalization",
            "CA1305: Specify IFormatProvide",
            Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
         public class Bcl_DateTime_Expected_Behavior_ToString : ArrangeActAssert
         {
            // ToString("o") => "0001-02-03T04:05:06.0000000-07:00"
            // ToString("u") => "0001-02-03 04:05:06Z"
            private readonly DateTime _local = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Local);
            private readonly DateTime _unspecified = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Unspecified);
            private readonly DateTime _utc = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc);

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage(
               "Microsoft.Globalization",
               "CA1305: Specify IFormatProvide",
               Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
            public void Bcl_DateTime_ToString_Expresses_The_Local_TimeZone()
            {
               // not testing default ToString() because it is affected by user settings and does not match ToString("g")
               _local.ToString("o").Should().Match("0001-02-03T04:05:06.0000000-??:00");
               _local.ToString("u").Should().Be("0001-02-03 04:05:06Z");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage(
               "Microsoft.Globalization",
               "CA1305: Specify IFormatProvide",
               Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
            public void Bcl_DateTime_ToString_Expresses_The_Unspecified_WithoutATimeZone_SomeOFTheTime()
            {
               // not testing default ToString() because it is affected by user settings and does not match ToString("g")

               // here be the rub:  the next line differs from what would happen with a dateTime of Kind UTC (no trailing Z)
               _unspecified.ToString("o").Should().Be("0001-02-03T04:05:06.0000000");
               // but the next line is the same:
               _unspecified.ToString("u").Should().Be("0001-02-03 04:05:06Z");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage(
               "Microsoft.Globalization",
               "CA1305: Specify IFormatProvide",
               Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
            public void Bcl_DateTime_ToString_Expresses_The_Utc_TimeZone()
            {
               // not testing default ToString() because it is affected by user settings and does not match ToString("g")
               _utc.ToString("o").Should().Be("0001-02-03T04:05:06.0000000Z");
               _utc.ToString("u").Should().Be("0001-02-03 04:05:06Z");
            }
         }

         [TestClass]
         public class Bcl_DateTime_Expected_Behavior_ToUniversalTime : ArrangeActAssert
         {
            private readonly DateTime _local = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Local);
            private readonly DateTime _unspecified = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Unspecified);
            private readonly DateTime _utc = new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc);

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTime_ToUniversalTime_On_Local_Kind_HasNoEffect()
            {
               if (TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
               {
                  Assert.Inconclusive("This test is inconclusive when run in UTC(0).");
               }

               var convertedToUniversal = _local.ToUniversalTime();
               convertedToUniversal.Should().NotBe(_local);
               convertedToUniversal.Ticks.Should().NotBe(_local.Ticks);
               convertedToUniversal.Kind.Should().Be(DateTimeKind.Utc);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTime_ToUniversalTime_On_Unspecified_Kind_ChangesBothKindAndTicks_As_If_It_Were_Local()
            {
               if (TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
               {
                  Assert.Inconclusive("This test is inconclusive when run in UTC(0).");
               }

               var convertedToUniversal = _unspecified.ToUniversalTime();
               convertedToUniversal.Should().NotBe(_unspecified);
               convertedToUniversal.Ticks.Should().NotBe(_unspecified.Ticks);
               convertedToUniversal.Kind.Should().Be(DateTimeKind.Utc);

               // here be the rub:  unspecified to Local, treats source as UTC.  unspecified to UTC treats, source as local.
               convertedToUniversal.Should().Be(_local.ToUniversalTime());
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTime_ToUniversalTime_On_Utc_Kind_HasNoEffect()
            {
               var convertedToUniversal = _utc.ToUniversalTime();
               convertedToUniversal.Should().Be(_utc);
               convertedToUniversal.Ticks.Should().Be(_utc.Ticks);
               convertedToUniversal.Kind.Should().Be(DateTimeKind.Utc);
            }
         }
      }

      [TestClass]
      public class When_I_Call_ToRoundTripString_On_A_Local_DateTime : ArrangeActAssert
      {
         private String actual;
         private String expected;
         private DateTime original;

         protected override void ArrangeMethod()
         {
            original = new DateTime(1, 2, 3, 4, 5, 6, 7, DateTimeKind.Local);
         }

         protected override void ActMethod()
         {
            actual = original.ToRoundtripString();
            expected = original.ToString("o", CultureInfo.InvariantCulture);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Convert_From_Local_To_Utc_Representation()
         {
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_Call_ToRoundTripString_On_A_Unspecified_DateTime : ArrangeActAssert
      {
         private String actual;
         private String expected;
         private DateTime original;

         protected override void ArrangeMethod()
         {
            original = new DateTime(1, 2, 3, 4, 5, 6, 7, DateTimeKind.Unspecified);
         }

         protected override void ActMethod()
         {
            actual = original.ToRoundtripString();
            expected = new DateTime(original.Ticks, DateTimeKind.Utc).ToRoundtripString();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Treat_The_Original_As_If_It_Were_Utc()
         {
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_Call_ToRoundTripString_On_A_Utc_DateTime : ArrangeActAssert
      {
         private String actual;
         private String expected;
         private DateTime original;

         protected override void ArrangeMethod()
         {
            original = new DateTime(1, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc);
         }

         protected override void ActMethod()
         {
            actual = original.ToRoundtripString();
            expected = original.ToString("o", CultureInfo.InvariantCulture);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Give_The_Original_Value()
         {
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_Call_ToUtc_On_A_Local_DateTime : ArrangeActAssert
      {
         private DateTime actual;
         private DateTime expected;
         private DateTime original;

         protected override void ArrangeMethod()
         {
            original = new DateTime(1, 2, 3, 4, 5, 6, 7, DateTimeKind.Local);
         }

         protected override void ActMethod()
         {
            actual = original.ToUtc();
            var localZone = TimeZoneInfo.Local;
            expected = new DateTime(original.AddTicks(localZone.BaseUtcOffset.Negate().Ticks).Ticks, DateTimeKind.Utc);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Convert_From_Local_To_Utc_Representation()
         {
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_Call_ToUtc_On_A_Unspecified_DateTime : ArrangeActAssert
      {
         private DateTime actual;
         private DateTime expected;
         private DateTime original;

         protected override void ArrangeMethod()
         {
            original = new DateTime(1, 2, 3, 4, 5, 6, 7, DateTimeKind.Unspecified);
         }

         protected override void ActMethod()
         {
            actual = original.ToUtc();
            expected = new DateTime(original.Ticks, DateTimeKind.Utc);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Treat_The_Original_As_If_It_Were_Utc()
         {
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_Call_ToUtc_On_A_Utc_DateTime : ArrangeActAssert
      {
         private DateTime actual;
         private DateTime expected;
         private DateTime original;

         protected override void ArrangeMethod()
         {
            original = new DateTime(1, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc);
         }

         protected override void ActMethod()
         {
            actual = original.ToUtc();
            expected = original;
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Give_The_Original_Value()
         {
            actual.Should().Be(expected);
            actual.Should().Be(original);
         }
      }

      [TestClass]
      public class When_I_Call_ToUtcString_On_A_Local_DateTime : ArrangeActAssert
      {
         private String actual;
         private String expected;
         private DateTime original;

         protected override void ArrangeMethod()
         {
            original = new DateTime(1, 2, 3, 4, 5, 6, 7, DateTimeKind.Local);
         }

         protected override void ActMethod()
         {
            actual = original.ToUtcString();
            expected = original.ToString("u", CultureInfo.InvariantCulture);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Convert_From_Local_To_Utc_Representation()
         {
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_Call_ToUtcString_On_A_Unspecified_DateTime : ArrangeActAssert
      {
         private String actual;
         private String expected;
         private DateTime original;

         protected override void ArrangeMethod()
         {
            original = new DateTime(1, 2, 3, 4, 5, 6, 7, DateTimeKind.Unspecified);
         }

         protected override void ActMethod()
         {
            actual = original.ToUtcString();
            expected = new DateTime(original.Ticks, DateTimeKind.Utc).ToUtcString();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Treat_The_Original_As_If_It_Were_Utc()
         {
            actual.Should().Be(expected);
         }
      }

      [TestClass]
      public class When_I_Call_ToUtcString_On_A_Utc_DateTime : ArrangeActAssert
      {
         private String actual;
         private String expected;
         private DateTime original;

         protected override void ArrangeMethod()
         {
            original = new DateTime(1, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc);
         }

         protected override void ActMethod()
         {
            actual = original.ToUtcString();
            expected = original.ToString("u", CultureInfo.InvariantCulture);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_Give_The_Original_Value()
         {
            actual.Should().Be(expected);
         }
      }
   }
}
