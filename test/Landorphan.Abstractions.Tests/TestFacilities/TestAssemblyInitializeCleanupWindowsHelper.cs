namespace Landorphan.Abstractions.Tests.TestFacilities
{
   using System;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Runtime.InteropServices;
   using System.Text;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Ioc.ServiceLocation;

   [SuppressMessage("Platform.Compat", "PC001: API not supported on all platforms")]
   internal class TestAssemblyInitializeCleanupWindowsHelper
   {
      // this class is called by TestAssemblyInitializeCleanup to arrange or teardown Windows-Specific resources.
      // it is not intended to be called by test classes, it is a "friend" class to TestAssemblyInitializeCleanup

      internal void Arrange()
      {
         if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            throw new InvalidOperationException($"Cannot perform {nameof(Arrange)} on non-Windows platforms");
         }

         /* **************************************************************************************************************
         Create the following folder\file structure with ACLs (C:\ is not hard-coded)
            C:\Landorphan.Abstractions.Test.UnitTestTarget
            C:\Landorphan.Abstractions.Test.UnitTestTarget\EveryoneFullControl
            C:\Landorphan.Abstractions.Test.UnitTestTarget\EveryoneFullControl\OwnerOnly.txt
            C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions
            C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\OuterExtantFile.txt
            C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\InnerNoPermissions
            C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\InnerNoPermissions\InnerExtantFile.txt
            C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\ReadExecuteListFolderContents
            C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\ReadExecuteListFolderContents\ExtantFolder
            C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\ReadExecuteListFolderContents\ExtantFile.txt
         ************************************************************************************************************** */
         var exitCode = ExecuteArrangeScript(out var output, out var error);
         Trace.WriteLine($"PowerShell: arrange script has exited: {exitCode}");
         Trace.WriteLine(output);
         Trace.WriteLine(error);
         InitializeTestHardCodesWindowsLocalTestPaths();
         InitializeTestHardCodesWindowsUncTestPaths();
      }

      internal void Teardown()
      {
         if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            throw new InvalidOperationException($"Cannot perform {nameof(Arrange)} on non-Windows platforms");
         }

         // Recursively delete C:\Landorphan.Abstractions.Test.UnitTestTarget (C:\ is not hard-coded)
         var exitCode = ExecuteTeardownScript(out var output, out var error);
         Trace.WriteLine($"PowerShell: teardown script has exited: {exitCode}");
         Trace.WriteLine(output);
         Trace.WriteLine(error);
      }

      [SuppressMessage("SonarLint.Naming", "S100: Methods and properties should be named in PascalCase", Justification = "False positive, PS is an abbreviation of PowerShell")]
      private ProcessStartInfo BuildPSElevatedStartInfo()
      {
         var envUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         var pwd = dirUtilities.GetCurrentDirectory();

         // NEIN:  can this be improved for 32-bit machines?
         var psExeFullPath = pathUtilities.Combine(
            envUtilities.GetSpecialFolderPath(Environment.SpecialFolder.Windows),
            @"SysWOW64\WindowsPowerShell\v1.0\powershell.exe");

         var startInfo = new ProcessStartInfo
         {
            // CreateNoWindow = true seems to be ignored on Windows with UAC enabled.  UAC prompts for confirmation, then a PowerShell window pops and closes.
            CreateNoWindow = true,
            ErrorDialog = false,
            FileName = psExeFullPath,
            // Set UseShellExecute to false for redirection of output streams
            // Set UseShellExecute to true if you want Verb to be honored
            // >> you cannot redirect output and elevate
            // >> if you do not redirect output, you can create a deadlock by accessing both streams
            UseShellExecute = true,
            // this verb elevates the execution context for the script
            Verb = "runas",
            WorkingDirectory = pwd
         };

         startInfo.RedirectStandardOutput = !startInfo.UseShellExecute;
         startInfo.RedirectStandardError = !startInfo.UseShellExecute;
         return startInfo;
      }

      private Int32 ExecuteArrangeScript(out String output, out String error)
      {
         var fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();

         const String scriptPath = @".\Arrange-LocalFiles.ps1";
         if (!fileUtilities.FileExists(scriptPath))
         {
            Trace.WriteLine($"WARNING: could not find {scriptPath}, local file path tests will be inconclusive.");
         }

         // Execute an Administrator-Only script to create local files and folders for tests.
         var startInfo = BuildPSElevatedStartInfo();
         var exitCode = ExecutePowerShellScript(startInfo, scriptPath, out output, out error);
         return exitCode;
      }

      private Int32 ExecutePowerShellScript(ProcessStartInfo startInfo, String scriptPath, out String output, out String error)
      {
         // TODO: This causes a UAC confirmation dialog box to be displayed on UAC enabled machines, fine on a dev box, what about the build server?
         //
         // in order to elevate, must use ShellExecute = true
         // when ShellExecute, redirection of StandardOutput and StandardError is not allowed.
         // trying to redirect powershell output to a file

         const Int32 oneSecondInMilliseconds = 1000;
         var fifteenSeconds = new TimeSpan(0, 0, 15);

         // ReSharper disable once StringLiteralTypo
         const String stdErrLogFileName = @"PSLogStdErr.txt";
         const String stdOutLogFileName = @"PSLogStdOut.txt";

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         var fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
         var fileReaderUtilities = IocServiceLocator.Resolve<IFileReaderUtilities>();

         var pwd = dirUtilities.GetCurrentDirectory();

         var scriptFullPath = pathUtilities.GetFullPath(scriptPath);
         // 2 is standard error, 1 is standard output
         startInfo.Arguments = scriptFullPath + " 2> " + stdErrLogFileName + " 1> " + stdOutLogFileName;
         if (!fileUtilities.FileExists(scriptPath))
         {
            var err = $"ERROR: could not find {scriptPath}.";
            Trace.WriteLine(err);
            output = String.Empty;
            error = err;
            return -1;
         }

         Int32 exitCode;
         using (var ps = new Process())
         {
            ps.StartInfo = startInfo;
            ps.Start();
            // allow 15 seconds for execution (do this after UAC prompts with dialog on UAC enabled machines)
            var expiry = DateTime.UtcNow + fifteenSeconds;
            do
            {
               if (!ps.Responding)
               {
                  Trace.WriteLine($"PowerShell: {scriptPath} status: Not Responding");
               }

               if (expiry < DateTime.UtcNow)
               {
                  Trace.WriteLine("PowerShell: failed to return in time.");
                  try
                  {
                     ps.Kill();
                  }
                  catch
                  {
                     // eat any exception that results from killing the ps instance.
                  }

                  break;
               }
            }
            while (!ps.WaitForExit(oneSecondInMilliseconds));

            exitCode = ps.ExitCode;
         }

         dirUtilities.SetCurrentDirectory(pwd);
         if (fileUtilities.FileExists(stdErrLogFileName))
         {
            error = fileReaderUtilities.ReadAllText(stdErrLogFileName, Encoding.UTF8);
            fileUtilities.DeleteFile(stdErrLogFileName);
         }
         else
         {
            error = String.Empty;
         }

         if (fileUtilities.FileExists(stdOutLogFileName))
         {
            output = fileReaderUtilities.ReadAllText(stdOutLogFileName, Encoding.UTF8);
            fileUtilities.DeleteFile(stdOutLogFileName);
         }
         else
         {
            output = String.Empty;
         }

         return exitCode;
      }

      private Int32 ExecuteTeardownScript(out String output, out String error)
      {
         var fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();

         const String scriptPath = @".\Teardown-LocalFiles.ps1";
         if (!fileUtilities.FileExists(scriptPath))
         {
            Trace.WriteLine($"WARNING: could not find {scriptPath}.");
         }

         // Execute an Administrator-Only script to delete local files and folders for tests.
         var startInfo = BuildPSElevatedStartInfo();
         var exitCode = ExecutePowerShellScript(startInfo, scriptPath, out output, out error);
         return exitCode;
      }

      private void InitializeTestHardCodesWindowsLocalTestPaths()
      {
         var envUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         var fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();

         // parallel knowledge/maintenance here
         var systemDrive = pathUtilities.GetRootPath(envUtilities.GetSpecialFolderPath(Environment.SpecialFolder.System));
         var localFolderRoot = pathUtilities.Combine(systemDrive, @"Landorphan.Abstractions.Test.UnitTestTarget");
         if (dirUtilities.DirectoryExists(localFolderRoot))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFolderRoot(localFolderRoot);
         }

         var localFolderEveryoneFullControl = pathUtilities.Combine(localFolderRoot, @"EveryoneFullControl");
         if (dirUtilities.DirectoryExists(localFolderEveryoneFullControl))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFolderEveryoneFullControl(localFolderEveryoneFullControl);
         }

         var localFileFullControlFolderOwnerOnlyFile = pathUtilities.Combine(localFolderEveryoneFullControl, @"OwnerOnly.txt");
         if (fileUtilities.FileExists(localFileFullControlFolderOwnerOnlyFile))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFileFullControlFolderOwnerOnlyFile(localFileFullControlFolderOwnerOnlyFile);
         }

         var localFolderOuterFolderNoPermissions = pathUtilities.Combine(localFolderRoot, @"OuterNoPermissions");
         if (dirUtilities.DirectoryExists(localFolderOuterFolderNoPermissions))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFolderOuterFolderNoPermissions(localFolderOuterFolderNoPermissions);
         }

         var localFileOuterFolderNoPermissionsChildFile = pathUtilities.Combine(localFolderOuterFolderNoPermissions, @"OuterExtantFile.txt");
         if (fileUtilities.FileExists(localFileOuterFolderNoPermissionsChildFile))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFileOuterFolderNoPermissionsChildFile(localFileOuterFolderNoPermissionsChildFile);
         }

         var localFolderOuterFolderNoPermissionsInnerFolderNoPermissions = pathUtilities.Combine(localFolderOuterFolderNoPermissions, "InnerNoPermissions");
         if (dirUtilities.DirectoryExists(localFolderOuterFolderNoPermissionsInnerFolderNoPermissions))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions(localFolderOuterFolderNoPermissionsInnerFolderNoPermissions);
         }

         var localFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile = pathUtilities.Combine(localFolderOuterFolderNoPermissionsInnerFolderNoPermissions, @"InnerExtantFile.txt");
         if (fileUtilities.FileExists(localFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile(localFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile);
         }

         var localFolderOuterFolderNoPermissionsReadExecuteListFolderContents = pathUtilities.Combine(localFolderOuterFolderNoPermissions, @"\ReadExecuteListFolderContents");
         if (dirUtilities.DirectoryExists(localFolderOuterFolderNoPermissionsReadExecuteListFolderContents))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFolderReadExecuteListFolderContents(localFolderOuterFolderNoPermissionsReadExecuteListFolderContents);
         }

         var localFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder =
            pathUtilities.Combine(localFolderOuterFolderNoPermissionsReadExecuteListFolderContents, @"\ExtantFolder");
         if (dirUtilities.DirectoryExists(localFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder(
               localFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder);
         }

         var localFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile =
            pathUtilities.Combine(localFolderOuterFolderNoPermissionsReadExecuteListFolderContents, @"\ExtantFile.txt");
         if (fileUtilities.FileExists(localFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile))
         {
            TestHardCodes.WindowsLocalTestPaths.SetLocalFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile(localFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile);
         }
      }

      private void InitializeTestHardCodesWindowsUncTestPaths()
      {
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

         // parallel knowledge/maintenance here
         var uncFolderRoot = @"\\localhost\Landorphan.Abstractions.Test.UnitTestTarget";
         TestHardCodes.WindowsUncTestPaths.SetUncShareRoot(uncFolderRoot);

         var uncFolderEveryoneFullControl = pathUtilities.Combine(uncFolderRoot, @"EveryoneFullControl");
         TestHardCodes.WindowsUncTestPaths.SetUncFolderEveryoneFullControl(uncFolderEveryoneFullControl);

         var uncFileFullControlFolderOwnerOnlyFile = pathUtilities.Combine(uncFolderEveryoneFullControl, @"OwnerOnly.txt");
         TestHardCodes.WindowsUncTestPaths.SetUncFileFullControlFolderOwnerOnlyFile(uncFileFullControlFolderOwnerOnlyFile);

         var uncFolderOuterFolderNoPermissions = pathUtilities.Combine(uncFolderRoot, @"OuterNoPermissions");
         TestHardCodes.WindowsUncTestPaths.SetUncFolderOuterFolderNoPermissions(uncFolderOuterFolderNoPermissions);

         var uncFileOuterFolderNoPermissionsChildFile = pathUtilities.Combine(uncFolderOuterFolderNoPermissions, @"OuterExtantFile.txt");
         TestHardCodes.WindowsUncTestPaths.SetUncFileOuterFolderNoPermissionsChildFile(uncFileOuterFolderNoPermissionsChildFile);

         var uncFolderOuterFolderNoPermissionsInnerFolderNoPermissions = pathUtilities.Combine(uncFolderOuterFolderNoPermissions, "InnerNoPermissions");
         TestHardCodes.WindowsUncTestPaths.SetUncFolderOuterFolderNoPermissionsInnerFolderNoPermissions(uncFolderOuterFolderNoPermissionsInnerFolderNoPermissions);

         var uncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile = pathUtilities.Combine(uncFolderOuterFolderNoPermissionsInnerFolderNoPermissions, @"InnerExtantFile.txt");
         TestHardCodes.WindowsUncTestPaths.SetUncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile(uncFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile);

         var uncFolderOuterFolderNoPermissionsReadExecuteListFolderContents = pathUtilities.Combine(uncFolderOuterFolderNoPermissions, @"\ReadExecuteListFolderContents");
         TestHardCodes.WindowsUncTestPaths.SetUncFolderReadExecuteListFolderContents(uncFolderOuterFolderNoPermissionsReadExecuteListFolderContents);

         var uncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder = pathUtilities.Combine(uncFolderOuterFolderNoPermissionsReadExecuteListFolderContents, @"\ExtantFolder");
         TestHardCodes.WindowsUncTestPaths.SetUncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder(uncFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder);

         var uncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile = pathUtilities.Combine(uncFolderOuterFolderNoPermissionsReadExecuteListFolderContents, @"\ExtantFile.txt");
         TestHardCodes.WindowsUncTestPaths.SetUncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile(uncFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile);
      }
   }
}
