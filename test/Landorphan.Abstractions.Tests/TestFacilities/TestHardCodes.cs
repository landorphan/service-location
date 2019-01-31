namespace Landorphan.Abstractions.Tests.TestFacilities
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Linq;
   using System.Runtime.InteropServices;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Ioc.ServiceLocation;

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
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         return new[] {pathUtilities.DirectorySeparatorCharacter, pathUtilities.AltDirectorySeparatorCharacter};
      }

      internal static class WindowsTestPaths
      {
         // TODO: local and remote UNC paths
         internal const String TodoRethinkUncShareEveryoneFullControl = @"\\localhost\SharedEveryoneFullControl";
         private static readonly Lazy<String> t_unmappedDrive = new Lazy<String>(FindUnmappedDrive);
         private static readonly Lazy<String> t_mappedDrive = new Lazy<String>(FindMappedDrive);

         internal static String LocalOuterFolderWithoutPermissions { get; private set; }
         internal static String LocalOuterFolderWithoutPermissionsChildFile { get; private set; }
         internal static String LocalOuterFolderWithoutPermissionsChildFolder { get; private set; }
         internal static String LocalOuterFolderWithoutPermissionsChildFolderChildFile { get; private set; }
         internal static String LocalReadExecuteListFolderContentsFolder { get; private set; }
         internal static String LocalSharedEveryoneFolderDeniedToAllButOwnerFile { get; private set; }
         internal static String LocalSharedFolderEveryoneFullControl { get; private set; }
         internal static String LocalTestTargetRootFolder { get; private set; }
         internal static String MappedDrive => t_mappedDrive.Value;
         internal static String UnmappedDrive => t_unmappedDrive.Value;

         // Setters are called by TestAssemblyInitializeCleanup

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

         [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
         private static String FindMappedDrive()
         {
            // returns null if not on a windows platform
            // returns null if no mapped letters exist
            // returns an mapped drive (e.g. "C:\") that is a fixed (preferred) or network drive

            // on the Azure DevOps server it was throwing: IOException: The device is not ready : 'A:\'
            // at System.Environment.set_CurrentDirectoryCore(String value)
            // best guess:  somewhere in the implementation of System.IO.Path, the current directory is changed, and then changed back
            // it would be best to avoid anything but a writable HD.

            String rv = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               // prefer fixed over network but accept both
               DriveInfo driveInfo = null;
               DriveInfo firstNetwork = null;
               var drives = DriveInfo.GetDrives();
               foreach (var d in drives)
               {
                  try
                  {
                     var x = d.DriveType;
                     if (x == DriveType.Fixed)
                     {
                        driveInfo = d;
                        break;
                     }

                     if (x == DriveType.Network && firstNetwork == null)
                     {
                        firstNetwork = d;
                     }
                  }
                  catch (IOException)
                  {
                     // eat it DriveInfo is one of those ancient types that throws IOException on property accessors
                  }
               }

               if (driveInfo != null)
               {
                  rv = driveInfo.Name;
               }
               else if (firstNetwork != null)
               {
                  rv = firstNetwork.Name;
               }
            }

            return rv;
         }

         private static String FindUnmappedDrive()
         {
            // returns null if not on a windows platform
            // returns null if no unmapped letters exist between A and Z
            // returns an unmapped drive (e.g. "A:\")
            String rv = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               var builder = ImmutableHashSet<String>.Empty.ToBuilder();
               builder.KeyComparer = StringComparer.InvariantCultureIgnoreCase;
               for (var c = 'A'; c <= 'Z'; c++)
               {
                  builder.Add(c + @":\");
               }

               var possible = builder.ToImmutable();
               var drives = Environment.GetLogicalDrives();
               foreach (var d in drives)
               {
                  possible = possible.Remove(d);
               }

               if (possible.Count > 0)
               {
                  var sorted = possible.ToList();
                  sorted.Sort(StringComparer.InvariantCultureIgnoreCase);
                  rv = sorted.First();
               }
            }

            return rv;
         }
      }
   }
}
