namespace Landorphan.Abstractions.Tests.TestFacilities
{
   using System;
   using System.Collections.Generic;

   // ReSharper disable CommentTypo

   internal static class TestHardCodes
   {
      internal const String DeniedToAllButOwnerFile = @"x:\UnitTestTarget\SharedEveryoneFullControl\DeniedToAllButOwner.txt";
      internal const String ExistingOuterDirectoryEveryoneFullControl = @"x:\UnitTestTarget\SharedEveryoneFullControl";
      internal const String ExistingOuterDirectoryWithoutPermissions = @"x:\UnitTestTarget\Outer";
      internal const String ExistingOuterDirectoryWithoutPermissionsChildDirectory = @"x:\UnitTestTarget\Outer\Inner";
      internal const String ExistingOuterDirectoryWithoutPermissionsChildDirectoryChildFile = @"x:\UnitTestTarget\Outer\Inner\InnerExistingFile.txt";
      internal const String ExistingOuterDirectoryWithoutPermissionsChildFile = @"x:\UnitTestTarget\Outer\OuterExistingFile.txt";
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

      internal const String ReadExecuteListFolderContentsDirectory = @"X:\UnitTestTarget\ReadExecuteListFolderContents";

      // TODO: need to think about creating known good UNC paths both locally and on build server in a mixed environment
      internal const String TodoRethinkNetworkShareEveryoneFullControl = @"\\localhost\SharedEveryoneFullControl";
      internal const String UnitTestTargetDirectory = @"x:\UnitTestTarget";

      internal const String WindowsInvalidPathCharacter = "|";

      internal static IEnumerable<Char> GetDirSepChars()
      {
         return new[] {'\\', '/'};
      }
   }
}
