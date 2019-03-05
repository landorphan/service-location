namespace Landorphan.Abstractions.Tests.TestFacilities
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Linq;
   using System.Runtime.InteropServices;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Common.Threading;
   using Landorphan.Ioc.ServiceLocation;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

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

      private static InterlockedBoolean t_windowsPathsInitialized = false;

      internal static Boolean AreWindowsPathsInitialized => t_windowsPathsInitialized.GetValue();

      internal static IEnumerable<Char> GetDirSepChars()
      {
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         return new[] {pathUtilities.DirectorySeparatorCharacter, pathUtilities.AltDirectorySeparatorCharacter};
      }

      private static void InitializeWindowsTestPaths()
      {
         // On Windows machines the arrange and teardown script for both UncTestPaths and WindowsTestsPaths is the same.
         // I choose to suffer with the class cohesion to avoid spreading the knowledge across many power shell scripts.

         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            var windowsArrange = new TestAssemblyInitializeCleanupWindowsHelper();
            try
            {
               var arragned = windowsArrange.Arrange();
               t_windowsPathsInitialized.SetValue(arragned);
            }
            catch (AssertInconclusiveException)
            {
               throw;
            }
            catch (Exception e)
            {
               var envUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
               Trace.WriteLine("ERROR:" + envUtilities.NewLine + e);
            }
         }
         else
         {
            Trace.WriteLine("WARNING: No local or UNC files and folders created for this OSPlatform");
         }
      }

      internal static class WindowsLocalTestPaths
      {
         private static String t_localFileFullControlFolderOwnerOnlyFile;
         private static String t_localFileOuterFolderNoPermissionsChildFile;
         private static String t_localFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile;
         private static String t_localFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile;
         private static String t_localFolderEveryoneFullControl;
         private static String t_localFolderOuterFolderNoPermissions;
         private static String t_localFolderOuterFolderNoPermissionsInnerFolderNoPermissions;
         private static String t_localFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder;
         private static String t_localFolderReadExecuteListFolderContents;
         private static String t_localFolderRoot;
         private static readonly Lazy<String> t_unmappedDrive = new Lazy<String>(FindUnmappedDrive);
         private static readonly Lazy<String> t_mappedDrive = new Lazy<String>(FindMappedDrive);

         internal static String LocalFileFullControlFolderOwnerOnlyFile
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFileFullControlFolderOwnerOnlyFile;
            }
            private set => t_localFileFullControlFolderOwnerOnlyFile = value;
         }

         internal static String LocalFileOuterFolderNoPermissionsChildFile
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFileOuterFolderNoPermissionsChildFile;
            }
            private set => t_localFileOuterFolderNoPermissionsChildFile = value;
         }

         internal static String LocalFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile;
            }
            private set => t_localFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile = value;
         }

         internal static String LocalFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile;
            }
            private set => t_localFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile = value;
         }

         internal static String LocalFolderEveryoneFullControl
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFolderEveryoneFullControl;
            }
            private set => t_localFolderEveryoneFullControl = value;
         }

         internal static String LocalFolderOuterFolderNoPermissions
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFolderOuterFolderNoPermissions;
            }
            private set => t_localFolderOuterFolderNoPermissions = value;
         }

         internal static String LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFolderOuterFolderNoPermissionsInnerFolderNoPermissions;
            }
            private set => t_localFolderOuterFolderNoPermissionsInnerFolderNoPermissions = value;
         }

         internal static String LocalFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder;
            }
            private set => t_localFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder = value;
         }

         internal static String LocalFolderReadExecuteListFolderContents
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFolderReadExecuteListFolderContents;
            }
            private set => t_localFolderReadExecuteListFolderContents = value;
         }

         internal static String LocalFolderRoot
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_localFolderRoot;
            }
            private set => t_localFolderRoot = value;
         }

         internal static String MappedDrive => t_mappedDrive.Value;
         internal static String UnmappedDrive => t_unmappedDrive.Value;

         // Setters are called by TestAssemblyInitializeCleanupWindowsHelper which arranges the local test directories and files.

         internal static void SetLocalFileFullControlFolderOwnerOnlyFile(String value)
         {
            LocalFileFullControlFolderOwnerOnlyFile = value;
         }

         internal static void SetLocalFileOuterFolderNoPermissionsChildFile(String value)
         {
            LocalFileOuterFolderNoPermissionsChildFile = value;
         }

         internal static void SetLocalFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile(String value)
         {
            LocalFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile = value;
         }

         internal static void SetLocalFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile(String value)
         {
            LocalFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile = value;
         }

         internal static void SetLocalFolderEveryoneFullControl(String value)
         {
            LocalFolderEveryoneFullControl = value;
         }

         internal static void SetLocalFolderOuterFolderNoPermissions(String value)
         {
            LocalFolderOuterFolderNoPermissions = value;
         }

         internal static void SetLocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions(String value)
         {
            LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions = value;
         }

         internal static void SetLocalFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder(String value)
         {
            LocalFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder = value;
         }

         internal static void SetLocalFolderReadExecuteListFolderContents(String value)
         {
            LocalFolderReadExecuteListFolderContents = value;
         }

         internal static void SetLocalFolderRoot(String value)
         {
            LocalFolderRoot = value;
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
            else
            {
               Trace.WriteLine("WARNING: No mapped drive for this OSPlatform");
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
            else
            {
               Trace.WriteLine("WARNING: No unmapped drive for this OSPlatform");
            }

            return rv;
         }
      }

      internal static class WindowsUncTestPaths
      {
         private static String t_uncFileFullControlFolderOwnerOnlyFile;
         private static String t_uncFileOuterFolderNoPermissionsChildFile;
         private static String t_uncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile;
         private static String t_uncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile;
         private static String t_uncFolderEveryoneFullControl;
         private static String t_uncFolderOuterFolderNoPermissions;
         private static String t_uncFolderOuterFolderNoPermissionsInnerFolderNoPermissions;
         private static String t_uncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder;
         private static String t_uncFolderReadExecuteListFolderContents;
         private static String t_uncShareRoot;

         internal static String UncFileFullControlFolderOwnerOnlyFile
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncFileFullControlFolderOwnerOnlyFile;
            }
            private set => t_uncFileFullControlFolderOwnerOnlyFile = value;
         }

         internal static String UncFileOuterFolderNoPermissionsChildFile
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncFileOuterFolderNoPermissionsChildFile;
            }
            private set => t_uncFileOuterFolderNoPermissionsChildFile = value;
         }

         internal static String UncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile;
            }
            private set => t_uncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile = value;
         }

         internal static String UncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile;
            }
            private set => t_uncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile = value;
         }

         internal static String UncFolderEveryoneFullControl
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncFolderEveryoneFullControl;
            }
            private set => t_uncFolderEveryoneFullControl = value;
         }

         internal static String UncFolderOuterFolderNoPermissions
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncFolderOuterFolderNoPermissions;
            }
            private set => t_uncFolderOuterFolderNoPermissions = value;
         }

         internal static String UncFolderOuterFolderNoPermissionsInnerFolderNoPermissions
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncFolderOuterFolderNoPermissionsInnerFolderNoPermissions;
            }
            private set => t_uncFolderOuterFolderNoPermissionsInnerFolderNoPermissions = value;
         }

         internal static String UncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder;
            }
            private set => t_uncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder = value;
         }

         internal static String UncFolderReadExecuteListFolderContents
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncFolderReadExecuteListFolderContents;
            }
            private set => t_uncFolderReadExecuteListFolderContents = value;
         }

         internal static String UncShareRoot
         {
            get
            {
               if (!AreWindowsPathsInitialized)
               {
                  InitializeWindowsTestPaths();
               }

               return t_uncShareRoot;
            }
            private set => t_uncShareRoot = value;
         }

         // Setters are called by TestAssemblyInitializeCleanupWindowsHelper which arranges the local test directories and files.

         internal static void SetUncFileFullControlFolderOwnerOnlyFile(String value)
         {
            UncFileFullControlFolderOwnerOnlyFile = value;
         }

         internal static void SetUncFileOuterFolderNoPermissionsChildFile(String value)
         {
            UncFileOuterFolderNoPermissionsChildFile = value;
         }

         internal static void SetUncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile(String value)
         {
            UncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile = value;
         }

         internal static void SetUncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile(String value)
         {
            UncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile = value;
         }

         internal static void SetUncFolderEveryoneFullControl(String value)
         {
            UncFolderEveryoneFullControl = value;
         }

         internal static void SetUncFolderOuterFolderNoPermissions(String value)
         {
            UncFolderOuterFolderNoPermissions = value;
         }

         internal static void SetUncFolderOuterFolderNoPermissionsInnerFolderNoPermissions(String value)
         {
            UncFolderOuterFolderNoPermissionsInnerFolderNoPermissions = value;
         }

         internal static void SetUncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder(String value)
         {
            UncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder = value;
         }

         internal static void SetUncFolderReadExecuteListFolderContents(String value)
         {
            UncFolderReadExecuteListFolderContents = value;
         }

         internal static void SetUncShareRoot(String value)
         {
            UncShareRoot = value;
         }
      }
   }
}
