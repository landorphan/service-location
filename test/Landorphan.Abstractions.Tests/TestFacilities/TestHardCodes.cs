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

    // ReSharper disable CommentTypo

    internal static class TestHardCodes
    {
        internal const int PathAlwaysTooLong = 32768;
        // it could be something smaller based on the operating system.
        internal const int PathMaxDirLengthWithoutTrailingSepChar = PathMaxDirLengthWithTrailingSepChar - 1;
        // it could be something smaller based on the operating system.
        internal const int PathMaxDirLengthWithTrailingSepChar = short.MaxValue;
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

        // Used to signal that the OSPlatform was neither OSX, Windows or Linux
        // should only ocure if a new OSPlatform is added to dotnet core support.
        internal const string UnrecognizedPlatform = "The test was unable to recognize the OS Platform, the test result can not be validated.";

        internal const string WindowsInvalidPathCharacter = "|";

        private static InterlockedBoolean t_windowsPathsInitialized = false;

        internal static bool AreWindowsPathsInitialized => t_windowsPathsInitialized.GetValue();

        internal static IEnumerable<char> GetDirSepChars()
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
                    var arranged = windowsArrange.Arrange();
                    t_windowsPathsInitialized.SetValue(arranged);
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
            private static string t_localFileFullControlFolderOwnerOnlyFile;
            private static string t_localFileOuterFolderNoPermissionsChildFile;
            private static string t_localFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile;
            private static string t_localFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile;
            private static string t_localFolderEveryoneFullControl;
            private static string t_localFolderOuterFolderNoPermissions;
            private static string t_localFolderOuterFolderNoPermissionsInnerFolderNoPermissions;
            private static string t_localFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder;
            private static string t_localFolderReadExecuteListFolderContents;
            private static string t_localFolderRoot;
            private static readonly Lazy<string> t_unmappedDrive = new Lazy<string>(FindUnmappedDrive);
            private static readonly Lazy<string> t_mappedDrive = new Lazy<string>(FindMappedDrive);

            internal static string LocalFileFullControlFolderOwnerOnlyFile
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

            internal static string LocalFileOuterFolderNoPermissionsChildFile
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

            internal static string LocalFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile
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

            internal static string LocalFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile
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

            internal static string LocalFolderEveryoneFullControl
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

            internal static string LocalFolderOuterFolderNoPermissions
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

            internal static string LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions
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

            internal static string LocalFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder
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

            internal static string LocalFolderReadExecuteListFolderContents
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

            internal static string LocalFolderRoot
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

            internal static string MappedDrive => t_mappedDrive.Value;
            internal static string UnmappedDrive => t_unmappedDrive.Value;

            // Setters are called by TestAssemblyInitializeCleanupWindowsHelper which arranges the local test directories and files.

            internal static void SetLocalFileFullControlFolderOwnerOnlyFile(string value)
            {
                LocalFileFullControlFolderOwnerOnlyFile = value;
            }

            internal static void SetLocalFileOuterFolderNoPermissionsChildFile(string value)
            {
                LocalFileOuterFolderNoPermissionsChildFile = value;
            }

            internal static void SetLocalFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile(string value)
            {
                LocalFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile = value;
            }

            internal static void SetLocalFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile(string value)
            {
                LocalFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile = value;
            }

            internal static void SetLocalFolderEveryoneFullControl(string value)
            {
                LocalFolderEveryoneFullControl = value;
            }

            internal static void SetLocalFolderOuterFolderNoPermissions(string value)
            {
                LocalFolderOuterFolderNoPermissions = value;
            }

            internal static void SetLocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions(string value)
            {
                LocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions = value;
            }

            internal static void SetLocalFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder(string value)
            {
                LocalFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder = value;
            }

            internal static void SetLocalFolderReadExecuteListFolderContents(string value)
            {
                LocalFolderReadExecuteListFolderContents = value;
            }

            internal static void SetLocalFolderRoot(string value)
            {
                LocalFolderRoot = value;
            }

            [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
            private static string FindMappedDrive()
            {
                // returns null if not on a windows platform
                // returns null if no mapped letters exist
                // returns an mapped drive (e.g. "C:\") that is a fixed (preferred) or network drive

                // on the Azure DevOps server it was throwing: IOException: The device is not ready : 'A:\'
                // at System.Environment.set_CurrentDirectoryCore(String value)
                // best guess:  somewhere in the implementation of System.IO.Path, the current directory is changed, and then changed back
                // it would be best to avoid anything but a writable HD.

                string rv = null;
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

            private static string FindUnmappedDrive()
            {
                // returns null if not on a windows platform
                // returns null if no unmapped letters exist between A and Z
                // returns an unmapped drive (e.g. "A:\")
                string rv = null;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var builder = ImmutableHashSet<string>.Empty.ToBuilder();
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
            private static string t_uncFileFullControlFolderOwnerOnlyFile;
            private static string t_uncFileOuterFolderNoPermissionsChildFile;
            private static string t_uncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile;
            private static string t_uncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile;
            private static string t_uncFolderEveryoneFullControl;
            private static string t_uncFolderOuterFolderNoPermissions;
            private static string t_uncFolderOuterFolderNoPermissionsInnerFolderNoPermissions;
            private static string t_uncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder;
            private static string t_uncFolderReadExecuteListFolderContents;
            private static string t_uncShareRoot;

            internal static string UncFileFullControlFolderOwnerOnlyFile
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

            internal static string UncFileOuterFolderNoPermissionsChildFile
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

            internal static string UncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile
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

            internal static string UncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile
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

            internal static string UncFolderEveryoneFullControl
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

            internal static string UncFolderOuterFolderNoPermissions
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

            internal static string UncFolderOuterFolderNoPermissionsInnerFolderNoPermissions
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

            internal static string UncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder
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

            internal static string UncFolderReadExecuteListFolderContents
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

            internal static string UncShareRoot
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

            internal static void SetUncFileFullControlFolderOwnerOnlyFile(string value)
            {
                UncFileFullControlFolderOwnerOnlyFile = value;
            }

            internal static void SetUncFileOuterFolderNoPermissionsChildFile(string value)
            {
                UncFileOuterFolderNoPermissionsChildFile = value;
            }

            internal static void SetUncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile(string value)
            {
                UncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile = value;
            }

            internal static void SetUncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile(string value)
            {
                UncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile = value;
            }

            internal static void SetUncFolderEveryoneFullControl(string value)
            {
                UncFolderEveryoneFullControl = value;
            }

            internal static void SetUncFolderOuterFolderNoPermissions(string value)
            {
                UncFolderOuterFolderNoPermissions = value;
            }

            internal static void SetUncFolderOuterFolderNoPermissionsInnerFolderNoPermissions(string value)
            {
                UncFolderOuterFolderNoPermissionsInnerFolderNoPermissions = value;
            }

            internal static void SetUncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder(string value)
            {
                UncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder = value;
            }

            internal static void SetUncFolderReadExecuteListFolderContents(string value)
            {
                UncFolderReadExecuteListFolderContents = value;
            }

            internal static void SetUncShareRoot(string value)
            {
                UncShareRoot = value;
            }
        }
    }
}
