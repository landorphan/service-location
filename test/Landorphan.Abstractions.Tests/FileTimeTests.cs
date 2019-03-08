namespace Landorphan.Abstractions.Tests
{
   using System;
   using System.Diagnostics;
   using System.Globalization;
   using System.IO;
   using System.Runtime.InteropServices;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class FileTimeTests
   {
      // ReSharper disable InconsistentNaming

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void TODO_REMOVE_MIN_CREATION()
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
            var start = FileTimeHelper.MinimumFileTimeAsDateTimeOffset;
            var ticks = start.Ticks - start.Ticks % TimeSpan.TicksPerSecond;
            lastGoodDt = new DateTime(ticks, DateTimeKind.Utc);
            lastGoodDt = lastGoodDt.AddSeconds(1);
            Trace.WriteLine($"Initial ticks = {ticks}");
            Trace.WriteLine($"Initial lastGoodDt = {lastGoodDt.ToString("o", CultureInfo.InvariantCulture)}");
         }

         try
         {
            var counter = 60;
            while (counter > 0)
            {
               counter--;
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
                     adjustedDt = lastGoodDt.AddSeconds(-1);
                  }

                  File.SetCreationTimeUtc(tempFile, adjustedDt);
                  var getDt = File.GetCreationTimeUtc(tempFile);
                  if (adjustedDt != getDt)
                  {
                     // supposed to throw but does not on Windows
                     Trace.WriteLine($"getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
                     break;
                  }

                  lastGoodDt = adjustedDt;
                  // Trace.WriteLine($"interim getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
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
         Trace.WriteLine($"lastGoodDt.Ticks = {lastGoodDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");

         TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void TODO_REMOVE_TOO_MAX_CREATION()
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
            var start = FileTimeHelper.MaximumFileTimeAsDateTimeOffset;
            var ticks = start.Ticks - start.Ticks % TimeSpan.TicksPerSecond;
            lastGoodDt = new DateTime(ticks, DateTimeKind.Utc);
            lastGoodDt = lastGoodDt.AddSeconds(-1);
            Trace.WriteLine($"Initial ticks = {ticks}");
            Trace.WriteLine($"Initial lastGoodDt = {lastGoodDt.ToString("o", CultureInfo.InvariantCulture)}");
         }

         try
         {
            var counter = 60;
            while (counter > 0)
            {
               counter--;
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
                     adjustedDt = lastGoodDt.AddSeconds(1);
                  }

                  File.SetCreationTimeUtc(tempFile, adjustedDt);
                  var getDt = File.GetCreationTimeUtc(tempFile);
                  if (adjustedDt != getDt)
                  {
                     Trace.WriteLine($"getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
                     break;
                  }

                  lastGoodDt = adjustedDt;
                  // Trace.WriteLine($"interim getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
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
         Trace.WriteLine($"lastGoodDt.Ticks = {lastGoodDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");

         TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void TODO_REMOVE_MIN_ACCESS()
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
            var start = FileTimeHelper.MinimumFileTimeAsDateTimeOffset;
            var ticks = start.Ticks - start.Ticks % TimeSpan.TicksPerSecond;
            lastGoodDt = new DateTime(ticks, DateTimeKind.Utc);
            lastGoodDt = lastGoodDt.AddSeconds(1);
            Trace.WriteLine($"Initial ticks = {ticks}");
            Trace.WriteLine($"Initial lastGoodDt = {lastGoodDt.ToString("o", CultureInfo.InvariantCulture)}");
         }

         try
         {
            var counter = 60;
            while (counter > 0)
            {
               counter--;
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
                     adjustedDt = lastGoodDt.AddSeconds(-1);
                  }

                  File.SetLastAccessTime(tempFile, adjustedDt);
                  var getDt = File.GetLastAccessTime(tempFile);
                  if (adjustedDt != getDt)
                  {
                     // supposed to throw but does not on Windows
                     Trace.WriteLine($"getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
                     break;
                  }

                  lastGoodDt = adjustedDt;
                  // Trace.WriteLine($"interim getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
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
         Trace.WriteLine($"lastGoodDt.Ticks = {lastGoodDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");

         TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void TODO_REMOVE_TOO_MAX_ACCESS()
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
            var start = FileTimeHelper.MaximumFileTimeAsDateTimeOffset;
            var ticks = start.Ticks - start.Ticks % TimeSpan.TicksPerSecond;
            lastGoodDt = new DateTime(ticks, DateTimeKind.Utc);
            lastGoodDt = lastGoodDt.AddSeconds(-1);
            Trace.WriteLine($"Initial ticks = {ticks}");
            Trace.WriteLine($"Initial lastGoodDt = {lastGoodDt.ToString("o", CultureInfo.InvariantCulture)}");
         }

         try
         {
            var counter = 60;
            while (counter > 0)
            {
               counter--;
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
                     adjustedDt = lastGoodDt.AddSeconds(1);
                  }

                  File.SetLastAccessTime(tempFile, adjustedDt);
                  var getDt = File.GetLastAccessTime(tempFile);
                  if (adjustedDt != getDt)
                  {
                     Trace.WriteLine($"getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
                     break;
                  }

                  lastGoodDt = adjustedDt;
                  // Trace.WriteLine($"interim getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
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
         Trace.WriteLine($"lastGoodDt.Ticks = {lastGoodDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");

         TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void TODO_REMOVE_MIN_WRITE()
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
            var start = FileTimeHelper.MinimumFileTimeAsDateTimeOffset;
            var ticks = start.Ticks - start.Ticks % TimeSpan.TicksPerSecond;
            lastGoodDt = new DateTime(ticks, DateTimeKind.Utc);
            lastGoodDt = lastGoodDt.AddSeconds(1);
            Trace.WriteLine($"Initial ticks = {ticks}");
            Trace.WriteLine($"Initial lastGoodDt = {lastGoodDt.ToString("o", CultureInfo.InvariantCulture)}");
         }

         try
         {
            var counter = 60;
            while (counter > 0)
            {
               counter--;
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
                     adjustedDt = lastGoodDt.AddSeconds(-1);
                  }

                  File.SetLastWriteTimeUtc(tempFile, adjustedDt);
                  var getDt = File.GetLastWriteTimeUtc(tempFile);
                  if (adjustedDt != getDt)
                  {
                     // supposed to throw but does not on Windows
                     Trace.WriteLine($"getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
                     break;
                  }

                  lastGoodDt = adjustedDt;
                  // Trace.WriteLine($"interim getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
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
         Trace.WriteLine($"lastGoodDt.Ticks = {lastGoodDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");

         TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void TODO_REMOVE_TOO_MAX_WRITE()
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
            var start = FileTimeHelper.MaximumFileTimeAsDateTimeOffset;
            var ticks = start.Ticks - start.Ticks % TimeSpan.TicksPerSecond;
            lastGoodDt = new DateTime(ticks, DateTimeKind.Utc);
            lastGoodDt = lastGoodDt.AddSeconds(-1);
            Trace.WriteLine($"Initial ticks = {ticks}");
            Trace.WriteLine($"Initial lastGoodDt = {lastGoodDt.ToString("o", CultureInfo.InvariantCulture)}");
         }

         try
         {
            var counter = 60;
            while (counter > 0)
            {
               counter--;
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
                     adjustedDt = lastGoodDt.AddSeconds(1);
                  }

                  File.SetLastWriteTimeUtc(tempFile, adjustedDt);
                  var getDt = File.GetLastWriteTimeUtc(tempFile);
                  if (adjustedDt != getDt)
                  {
                     Trace.WriteLine($"getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
                     break;
                  }

                  lastGoodDt = adjustedDt;
                  // Trace.WriteLine($"interim getDt.Ticks = {getDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");
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
         Trace.WriteLine($"lastGoodDt.Ticks = {lastGoodDt.Ticks.ToString("N0", CultureInfo.InvariantCulture)}");

         TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
      }
   }
}
