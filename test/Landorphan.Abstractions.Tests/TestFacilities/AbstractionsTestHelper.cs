namespace Landorphan.Abstractions.Tests.TestFacilities
{
   using System;

   internal static class AbstractionsTestHelper
   {
      /// <summary>
      /// Returns the current (Now) DateTimeOffset for a file test.  This implies
      /// that the precision is limited to Linux/Unix file time precision so multi
      /// platform tests are able to leverage the time correctly.
      /// </summary>
      /// <returns></returns>
      public static DateTimeOffset GetUtcNowForFileTest()
      {
         return DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
      }
   }
}
