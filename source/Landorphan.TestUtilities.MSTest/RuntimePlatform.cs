namespace Landorphan.TestUtilities
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using System.Runtime.InteropServices;

   /// <summary>
   /// Provides helper methods for interacting with the runtime information
   /// regarding the operating platform. 
   /// </summary>
   public static class RuntimePlatform
   {

      /// <summary>
      /// Determines if the current runtime platform is a Mac (OSX)
      /// </summary>
      /// <returns> 
      /// True if the runtime operating system platform is OSX, otherwise false.
      /// </returns>
      [SuppressMessage("SonarLint.CodeSmell", "S100: Name does not match camel case rules",
         Justification = "This name is consistent with the name chosen by dotnet core for OSPlatform")]
      public static bool IsOSX()
      {
         return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
      }

      /// <summary>
      /// Determines if the current runtime platform is Windows
      /// </summary>
      /// <returns>
      /// True if the current runtime operating system platform is Windows, otherwise false.
      /// </returns>
      public static bool IsWindows()
      {
         return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
      }

      /// <summary>
      /// Determines if the current runtime platform is Linux
      /// </summary>
      /// <returns>
      /// True if the current runtime operating system platform is Linux, otherwise false.
      /// </returns>
      public static bool IsLinux()
      {
         return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
      }

      /// <summary>
      /// Determines if the runtime operating system platform is one of the
      /// supplied operating system platforms.
      /// </summary>
      /// <param name="platforms">
      /// A list of possible operating system platforms to be evaluated. 
      /// </param>
      /// <returns>
      /// True if the current runtime operating system platform one of the provided
      /// platforms, otherwise false.
      /// </returns>
      [SuppressMessage("SonarLint.CodeSmell", "S100: Name does not match camel case rules",
         Justification = "This name is consistent with the name chosen by dotnet core for OSPlatform")]
      public static bool IsOSPlatform(params OSPlatform[] platforms)
      {
         return (from p in platforms
                where RuntimeInformation.IsOSPlatform(p)
               select p).Any();
      }

      /// <summary>
      /// Determines if the current runtime operating system platform is not any
      /// of the ones provided.
      /// </summary>
      /// <param name="platforms">
      /// A list of possible operating system platforms to be evaluated. 
      /// </param>
      /// <returns>
      /// True if the current runtime operating system platform is not any of the
      /// platforms provided, otherwise false.
      /// </returns>
      [SuppressMessage("SonarLint.CodeSmell", "S100: Name does not match camel case rules",
         Justification = "This name is consistent with the name chosen by dotnet core for OSPlatform")]
      public static bool IsNotOSPlatform(params OSPlatform[] platforms)
      {
         return !IsOSPlatform(platforms);
      }
   }
}
