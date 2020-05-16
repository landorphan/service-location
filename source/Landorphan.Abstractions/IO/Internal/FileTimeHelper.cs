namespace Landorphan.Abstractions.IO.Internal
{
    using System;
    using System.Runtime.InteropServices;

    internal static class FileTimeHelper
   {
       /// <summary>
      /// Gets the maximum file time as a <see cref="DateTimeOffset"/>.
      /// </summary>
      /// <value>
      /// The maximum file time as a <see cref="DateTimeOffset"/>.
      /// </value>
      internal static DateTimeOffset MaximumFileTimeAsDateTimeOffset { get; } = new Func<DateTimeOffset>(
         () =>
         {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               // Windows:
               //    precision is too the 100 ns
               //    precision in ticks is 1
               //
               //                                     12/31/9999 23:59:59.9999999
               //                                     3_155_378_975_999_999_999 confirmed on Window10x64 2019.03.06
               return new DateTimeOffset(new DateTime(DateTimeOffset.MaxValue.Ticks, DateTimeKind.Utc));
            }

            // TODO: need confirmation on OSX
            // linux:
            //    precision is to the 1s
            //    precision in ticks is Timespan.TicksPerSecond or 10_000_000
            //                                     12/31/9999 23:59:59.0000000
            //                                     3_155_378_975_990_000_000 confirmed on Ubuntu 18.04 2019.03.07
            return new DateTimeOffset(new DateTime(3_155_378_975_990_000_000, DateTimeKind.Utc));
         })();

       /// <summary>
      /// Gets the maximum precision file system ticks supported by the host operating system.
      /// </summary>
      /// <value>
      /// The maximum precision file system ticks.
      /// </value>
      /// <remarks>
      /// On Windows, the file system supports precision down to 1 tick, or 100 nanoseconds, on linux, the precision is to the second.
      /// </remarks>
      internal static long MaximumPrecisionFileSystemTicks { get; } = new Func<long>(
         () =>
         {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               return 1;
            }

            return TimeSpan.TicksPerSecond;
         })();

       /// <summary>
      /// Gets the minimum file time as a <see cref="DateTimeOffset"/>.
      /// </summary>
      /// <value>
      /// The minimum file time as a <see cref="DateTimeOffset"/>.
      /// </value>
      internal static DateTimeOffset MinimumFileTimeAsDateTimeOffset { get; } = new Func<DateTimeOffset>(
         () =>
         {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               // Windows:
               //    precision is too the 100 ns
               //    precision in ticks is 1
               //
               //                                     504_911_232_000_000_001 confirmed on Window10x64 2019.03.06
               //                                     Midnight, January 1st, 1601, AKA Windows Epoch
               return new DateTimeOffset(new DateTime(504_911_232_000_000_001, DateTimeKind.Utc));
            }

            // TODO: need confirmation on OSX
            // linux:
            //    precision is to the 1s
            //    precision in ticks is Timespan.TicksPerSecond or 10_000_000
            //
            //                                        268_800_000_000 confirmed on Ubuntu 18.04 2019.03.07
            //                                        0001-01-01T07:28:01.0000000Z
            return new DateTimeOffset(new DateTime(1, 1, 1, 7, 28, 1, DateTimeKind.Utc));
         })();

       /// <summary>
      /// Truncates the given value to the maximum precision supported by the current platform.
      /// </summary>
      /// <param name="value">
      /// The value to transform.
      /// </param>
      /// <returns>DateTimeOffset.</returns>
      internal static DateTimeOffset TruncateTicksToFileSystemPrecision(DateTimeOffset value)
      {
         var ticks = value.UtcDateTime.Ticks;
         ticks = ticks - ticks % MaximumPrecisionFileSystemTicks;
         var utc = new DateTime(ticks, DateTimeKind.Utc);
         var rv = new DateTimeOffset(utc);
         return rv;
      }
   }
}
