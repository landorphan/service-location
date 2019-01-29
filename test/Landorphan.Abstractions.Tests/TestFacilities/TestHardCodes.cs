namespace Landorphan.Abstractions.Tests.TestFacilities
{
   using System;
   using System.Collections.Generic;

   // ReSharper disable CommentTypo

   internal static class TestHardCodes
   {
      /* ****************************************************************************************************************************************************************
      Starting with apps running under the .NET Framework 4.6.2, the .NET Framework supports long paths in excess of 260 (or MAX_PATH) characters. 
      The conditions under which a PathTooLongException exception are thrown depend on the version of the .NET Framework that an app targets:

      Apps that target the .NET Framework 4.6.2 and later versions
         Long paths are supported by default. The runtime throws a PathTooLongException under the following conditions:

      The operating system returns COR_E_PATHTOOLONG or its equivalent.

         The length of the path exceeds Int16.MaxValue (32,767) characters.

         Apps that target the .NET Framework 4.6.1 and earlier versions
         Long paths are disabled by default, and the legacy behavior is maintained. The runtime throws a PathTooLongException whenever a path exceeds 260 characters. 
      
         >> 32767 or something smaller

      **************************************************************************************************************************************************************** */
      internal const Int32 PathAlwaysTooLong = 32768;
      // it could be something smaller based on the operating system.
      internal const Int32 PathMaxDirLengthWithoutTrailingSepChar = PathMaxDirLengthWithTrailingSepChar - 1;
      // it could be something smaller based on the operating system.
      internal const Int32 PathMaxDirLengthWithTrailingSepChar = Int16.MaxValue;

      internal const String WindowsInvalidPathCharacter = "|";

      internal static IEnumerable<Char> GetDirSepChars()
      {
         return new[] {'\\', '/'};
      }

      internal static class WindowsTestPaths
      {
         // TODO: local and remote UNC paths
         internal const String TodoRethinkNetworkShareEveryoneFullControl = @"\\localhost\SharedEveryoneFullControl";

         internal static String LocalOuterFolderWithoutPermissions { get; private set; }
         internal static String LocalOuterFolderWithoutPermissionsChildFile { get; private set; }
         internal static String LocalOuterFolderWithoutPermissionsChildFolder { get; private set; }
         internal static String LocalOuterFolderWithoutPermissionsChildFolderChildFile { get; private set; }
         internal static String LocalReadExecuteListFolderContentsFolder { get; private set; }
         internal static String LocalSharedEveryoneFolderDeniedToAllButOwnerFile { get; private set; }
         internal static String LocalSharedFolderEveryoneFullControl { get; private set; }
         internal static String LocalTestTargetRootFolder { get; private set; }

         internal static void SetFilePathLocalOuterFolderWithoutPermissionsChildFile(String value)
         {
            LocalOuterFolderWithoutPermissionsChildFile = value;
         }

         internal static void SetFilePathLocalOuterFolderWithoutPermissionsChildFolderChildFile(String value)
         {
            LocalOuterFolderWithoutPermissionsChildFolderChildFile = value;
         }

         internal static void SetFilePathLocalSharedEveryoneFolderDeniedToAllButOwnerFile(String value)
         {
            LocalSharedEveryoneFolderDeniedToAllButOwnerFile = value;
         }

         internal static void SetFolderPathLocalOuterFolderWithoutPermissions(String value)
         {
            LocalOuterFolderWithoutPermissions = value;
         }

         internal static void SetFolderPathLocalOuterFolderWithoutPermissionsChildFolder(String value)
         {
            LocalOuterFolderWithoutPermissionsChildFolder = value;
         }

         // Setters are called by TestAssemblyInitializeCleanup

         internal static void SetFolderPathLocalReadExecuteListFolderContentsFolder(String value)
         {
            LocalReadExecuteListFolderContentsFolder = value;
         }

         internal static void SetFolderPathLocalSharedFolderEveryoneFullControl(String value)
         {
            LocalSharedFolderEveryoneFullControl = value;
         }

         internal static void SetFolderPathLocalTestTargetRootFolder(String value)
         {
            LocalTestTargetRootFolder = value;
         }
      }
   }
}
