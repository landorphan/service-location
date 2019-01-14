namespace Landorphan.Common.Tests.Extensions
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class DateTimeOffset_Behavior_Tests
   {
      public static class BclBehavior
      {
         [TestClass]
         public class Bcl_DateTimeOffset_Expected_Behavior_DateTime_Property_Get : TestBase
         {
            // whether you create a DateTimeOffset with a local, utc, or unspecified DateTime
            // it returns a DateTime with Kind value of unspecified.

            private readonly DateTimeOffset _local = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Local));
            private readonly DateTimeOffset _unspecified = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Unspecified));
            private readonly DateTimeOffset _utc = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc));

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_Treats_Local_DateTime_As_Unspecified()
            {
               _local.DateTime.Ticks.Should().Be(_local.Ticks);
               _local.DateTime.Kind.Should().Be(DateTimeKind.Unspecified);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_Treats_Unspecified_DateTime_As_Unspecified()
            {
               _unspecified.DateTime.Ticks.Should().Be(_unspecified.Ticks);
               _unspecified.DateTime.Kind.Should().Be(DateTimeKind.Unspecified);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_Treats_Utc_DateTime_As_Unspecified()
            {
               _utc.DateTime.Ticks.Should().Be(_utc.Ticks);
               _utc.DateTime.Kind.Should().Be(DateTimeKind.Unspecified);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void HereBeDragons()
            {
               _local.Ticks.Should().Be(_unspecified.Ticks);
               _unspecified.Ticks.Should().Be(_utc.Ticks);

               _local.DateTime.Should().Be(_unspecified.DateTime);
               _unspecified.DateTime.Should().Be(_utc.DateTime);
            }
         }

         [TestClass]
         public class Bcl_DateTimeOffset_Expected_Behavior_Equality : TestBase
         {
            private readonly DateTimeOffset _local = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Local));
            private readonly DateTimeOffset _unspecified = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Unspecified));
            private readonly DateTimeOffset _utc = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc));

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_Treats_Local_And_Unspecified_As_Equal_When_Ticks_Are_Equal()
            {
               _local.Should().Be(_unspecified);
               _local.Ticks.Should().Be(_unspecified.Ticks);
               _local.UtcTicks.Should().Be(_unspecified.UtcTicks);
            }

            [TestMethod]
            [Ignore("Failes on Build Server, needs further Investigation.  tistocks")]
			[TestCategory(TestTiming.CheckIn)]
            public void Bcl_Treats_Local_And_Utc_As_NOT_Equal_When_Ticks_Are_Equal()
            {
               _local.Should().NotBe(_utc);
               _local.Ticks.Should().Be(_utc.Ticks);
               _local.UtcTicks.Should().NotBe(_utc.UtcTicks);
            }

            [TestMethod]
            [Ignore("Failes on Build Server, needs further Investigation. tistocks")]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_Treats_Unspecified_And_Utc_As_NOT_Equal_When_Ticks_Are_Equal()
            {
               _unspecified.Should().NotBe(_utc);
               _unspecified.Ticks.Should().Be(_utc.Ticks);
               _unspecified.UtcTicks.Should().NotBe(_utc.UtcTicks);
            }
         }

         [TestClass]
         public class Bcl_DateTimeOffset_Expected_Behavior_ToLocalTime : ArrangeActAssert
         {
            private readonly DateTimeOffset _local = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Local));
            private readonly DateTimeOffset _unspecified = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Unspecified));
            private readonly DateTimeOffset _utc = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc));

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTimeOffset_ToLocalTime_On_Local_HasNoEffect()
            {
               var convertedToLocal = _local.ToLocalTime();
               convertedToLocal.Should().Be(_local);
               convertedToLocal.Ticks.Should().Be(_local.Ticks);
               convertedToLocal.UtcTicks.Should().Be(_local.UtcTicks);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTimeOffset_ToLocalTime_On_Unspecified_ChangesTicks_As_If_It_Were_Local()
            {
               // this behavior is the opposite of how DateTime treats unspecified
               if (TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
               {
                  Assert.Inconclusive("This test is inconclusive when run in UTC(0).");
               }

               var convertedToLocal = _unspecified.ToLocalTime();
               convertedToLocal.Should().Be(_unspecified);
               // here be the rub:
               convertedToLocal.Ticks.Should().Be(_unspecified.Ticks);
               convertedToLocal.UtcTicks.Should().NotBe(_utc.UtcTicks);

               // here be the rub:  
               convertedToLocal.Should().NotBe(_utc.ToLocalTime());
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTimeOffset_ToLocalTime_On_Utc_Groks_Releative_Ticks()
            {
               // this behavior is the opposite of how DateTime treats UTC
               if (TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
               {
                  Assert.Inconclusive("This test is inconclusive when run in UTC(0).");
               }

               var convertedToLocal = _utc.ToLocalTime();
               convertedToLocal.Should().Be(_utc);
               convertedToLocal.Ticks.Should().NotBe(_utc.Ticks);
               convertedToLocal.UtcTicks.Should().Be(_utc.UtcTicks);
            }
         }

         [TestClass]
         public class Bcl_DateTimeOffset_Expected_Behavior_ToString : ArrangeActAssert
         {
            private readonly DateTimeOffset _local = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Local));
            private readonly DateTimeOffset _unspecified = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Unspecified));
            private readonly DateTimeOffset _utc = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc));

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage(
               "Microsoft.Globalization",
               "CA1305: Specify IFormatProvide",
               Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
            public void Bcl_DateTimeOffset_ToString_Expresses_Local_TimeZone()
            {
                 // Switched to Regex match as date time offset can be either positive or negative
               _local.ToString("o").Should().MatchRegex(@"0001-02-03T04:05:06\.0000000[-+]\d\d:\d\d");
               _local.ToString("u").Should().Match("0001-02-?? ??:05:06Z");
            }

            [TestMethod]
            [Ignore]
           	[TestCategory(TestTiming.CheckIn)]
            [SuppressMessage(
               "Microsoft.Globalization",
               "CA1305: Specify IFormatProvide",
               Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
            public void Bcl_DateTimeOffset_ToString_Expresses_Unspecified_AsLocal_SomeOFTheTime()
            {
               if (TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
               {
                  Assert.Inconclusive("This test is inconclusive when run in UTC(0).");
               }

               // here be the rub:
               _unspecified.ToString("o").Should().Match("0001-02-03T04:05:06.0000000-??:00");
               _unspecified.ToString("u").Should().Be("0001-02-03 11:05:06Z");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [SuppressMessage(
               "Microsoft.Globalization",
               "CA1305: Specify IFormatProvide",
               Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
            public void Bcl_DateTimeOffset_ToString_Expresses_Utc_As_Zero()
            {
               // here be the rub:
               // This does not match DateTime.ToString("o")
               _utc.ToString("o").Should().Be("0001-02-03T04:05:06.0000000+00:00");
               // This does match DateTime.ToString("u")
               _utc.ToString("u").Should().Be("0001-02-03 04:05:06Z");
            }
         }

         [TestClass]
         public class Bcl_DateTimeOffset_Expected_Behavior_ToUniversalTime : ArrangeActAssert
         {
            private readonly DateTimeOffset _local = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Local));
            private readonly DateTimeOffset _unspecified = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Unspecified));
            private readonly DateTimeOffset _utc = new DateTimeOffset(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc));

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTime_ToUniversalTime_On_Local_Groks_Relative_Ticks()
            {
               if (TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
               {
                  Assert.Inconclusive("This test is inconclusive when run in UTC(0).");
               }

               var convertedToUniversal = _local.ToUniversalTime();
               convertedToUniversal.Should().Be(_local);
               convertedToUniversal.Ticks.Should().NotBe(_local.Ticks);
               convertedToUniversal.UtcTicks.Should().Be(_local.UtcTicks);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Bcl_DateTime_ToUniversalTime_On_Unspecified_Kind_ChangesTicks_As_If_It_Were_Utc()
            {
               if (TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
               {
                  Assert.Inconclusive("This test is inconclusive when run in UTC(0).");
               }

               var convertedToUniversal = _unspecified.ToUniversalTime();
               convertedToUniversal.Should().Be(_unspecified);
               convertedToUniversal.Ticks.Should().NotBe(_unspecified.Ticks);
               convertedToUniversal.UtcTicks.Should().Be(_unspecified.UtcTicks);

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
               convertedToUniversal.UtcTicks.Should().Be(_utc.UtcTicks);
            }
         }
      }
   }
}
