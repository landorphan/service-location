namespace Landorphan.Abstractions.Tests
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.IO;
   using System.Runtime.InteropServices;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class When_I_call_EnvironmentUtilitiesFactory_Create : ArrangeActAssert
   {
      private readonly EnvironmentUtilitiesFactory target = new EnvironmentUtilitiesFactory();
      private IEnvironmentUtilities actual;

      protected override void ActMethod()
      {
         actual = target.Create();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_create_an_IEnvironment_instance()
      {
         actual.Should().BeAssignableTo<IEnvironmentUtilities>();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void TODO_REMOVE()
      {
         // get min file date/time per platform
         var fileUtils = IocServiceLocator.Resolve<IFileUtilities>();

         var tempFile = fileUtils.CreateTemporaryFile();
         var lastGoodDt = DateTime.UtcNow;
         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            lastGoodDt = new DateTime(504_911_232_000_000_002, DateTimeKind.Utc);
         }

         if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
         {
            lastGoodDt = new DateTime(621_355_968_020_000_000);
         }

         try
         {
            while (true)
            {
               try
               {
                  var adjustedDt = DateTime.UtcNow;
                  if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                  {
                     adjustedDt = lastGoodDt.AddTicks(-1);
                  }

                  if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                  {
                     // linux file precision is to the second
                     adjustedDt = lastGoodDt.AddTicks(-1 * TimeSpan.TicksPerSecond);
                  }

                  File.SetCreationTimeUtc(tempFile, adjustedDt);
                  var getDt = File.GetCreationTimeUtc(tempFile);
                  if (adjustedDt != getDt)
                  {
                     // supposed to throw but does not on Windows
                     break;
                  }

                  lastGoodDt = adjustedDt;
               }
               catch (ArgumentOutOfRangeException)
               {
                  break;
               }
            }
         }
         finally
         {
            fileUtils.DeleteFile(tempFile);
         }

         Trace.WriteLine($"lastGoodDt = {lastGoodDt.ToString("o", CultureInfo.InvariantCulture)}");
         Trace.WriteLine($"lastGoodDt.Ticks = {lastGoodDt.Ticks}");
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void TODO_REMOVE_TOO()
      {
         // get max file date/time per platform
         var fileUtils = IocServiceLocator.Resolve<IFileUtilities>();

         var tempFile = fileUtils.CreateTemporaryFile();
         var lastGoodDt = DateTime.UtcNow;
         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            lastGoodDt = new DateTime(3_155_378_975_999_999_998, DateTimeKind.Utc);
         }

         if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
         {
            var epoch_ticks = 504_911_232_000_000_001;
            var max_32bit_seconds_to_ticks = (Int64)(Math.Pow(2, 32) - 1) * TimeSpan.TicksPerSecond;
            var giveRoomTicks = epoch_ticks + max_32bit_seconds_to_ticks - TimeSpan.TicksPerSecond;
            lastGoodDt = new DateTime(giveRoomTicks, DateTimeKind.Utc);
         }

         try
         {
            while (true)
            {
               try
               {
                  var adjustedDt = DateTime.UtcNow;
                  if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                  {
                     // this does throw ArgumentOutOfRangeException
                     adjustedDt = lastGoodDt.AddTicks(1);
                  }

                  if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                  {
                     // linux file precision is to the second
                  }

                  File.SetCreationTimeUtc(tempFile, adjustedDt);
                  var getDt = File.GetCreationTimeUtc(tempFile);
                  if (adjustedDt != getDt)
                  {
                     break;
                  }

                  lastGoodDt = adjustedDt;
               }
               catch (ArgumentOutOfRangeException)
               {
                  break;
               }
            }
         }
         finally
         {
            fileUtils.DeleteFile(tempFile);
         }

         Trace.WriteLine($"lastGoodDt = {lastGoodDt.ToString("o", CultureInfo.InvariantCulture)}");
         Trace.WriteLine($"lastGoodDt.Ticks = {lastGoodDt.Ticks}");
      }
   }

   [TestClass]
   public class When_I_call_EnvironmentUtilitiesFactory_Create_multiple_times : ArrangeActAssert
   {
      private readonly EnvironmentUtilitiesFactory target = new EnvironmentUtilitiesFactory();
      private HashSet<IEnvironmentUtilities> actuals;

      protected override void ArrangeMethod()
      {
         actuals = new HashSet<IEnvironmentUtilities>(new ReferenceEqualityComparer<IEnvironmentUtilities>());
      }

      protected override void ActMethod()
      {
         actuals.Add(target.Create());
         actuals.Add(target.Create());
         actuals.Add(target.Create());
      }

      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "anew")]
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_return_a_new_instance_each_time()
      {
         actuals.Count.Should().Be(3);
      }
   }

   [TestClass]
   public class When_I_service_locate_IEnvironmentUtilitiesFactory : ArrangeActAssert
   {
      private IEnvironmentUtilitiesFactory actual;

      protected override void ActMethod()
      {
         actual = IocServiceLocator.Resolve<IEnvironmentUtilitiesFactory>();
      }

      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "mean")]
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_give_me_a_EnvironmentUtilitiesFactory()
      {
         actual.Should().BeOfType<EnvironmentUtilitiesFactory>();
      }
   }
}
