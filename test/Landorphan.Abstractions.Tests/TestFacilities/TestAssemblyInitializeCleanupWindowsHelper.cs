namespace Landorphan.Abstractions.Tests.TestFacilities
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Runtime.InteropServices;
   using System.Text;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.Win32;

   [SuppressMessage("Platform.Compat", "PC001: API not supported on all platforms")]
   internal class TestAssemblyInitializeCleanupWindowsHelper
   {
      /// <summary>
      /// Gets the PowerShell 32-bit execution policy.
      /// </summary>
      /// <remarks>
      /// Expected values are RemoteSigned, Unrestricted, Bypass, Restricted, AllSigned, Undefined
      /// </remarks>
      /// <returns>
      /// The execution policy as a string.
      /// "PowerShell 32-bit ExecutionPolicy not found." when the registry value was not found.
      /// "Not on Windows" when executed in a non-Windows environment.
      /// </returns>
      [SuppressMessage("SonarLint.Naming", "S100: Methods and properties should be named in PascalCase", Justification = "Sonar lint does not handle abbreviations and acronyms (MWP)")]
      public static String GetPSExecutionPolicyUser()
      {
         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            const String defaultValue = "PowerShell 32-bit ExecutionPolicy not found.";

            // for machine, use @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell"
            var rv = (String)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy", defaultValue);
            return rv;
         }

         return "Not on Windows";
      }

      // this class is called by TestAssemblyInitializeCleanup to arrange or teardown Windows-Specific resources.
      // Except for GetPSExecutionPolicyUser() it is not intended to be called by test classes, it is a "friend" class to TestAssemblyInitializeCleanup

      internal Boolean Arrange()
      {
         // returns true when arrange completed, otherwise false
         if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            throw new InvalidOperationException($"Cannot perform {nameof(Arrange)} on non-Windows platforms");
         }

         //var allowedExecutionPolicies = new HashSet<String>(StringComparer.OrdinalIgnoreCase) { "RemoteSigned", "Unrestricted", "Bypass" };
         //var executionPolicy = GetPSExecutionPolicyUser();
         //if (!allowedExecutionPolicies.Contains(executionPolicy))
         //{
         //   // TODO: TSG, why not throw an exception?
         //   return false;
         //}

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
         if (0 == exitCode)
         {
            InitializeTestHardCodesWindowsLocalTestPaths();
            InitializeTestHardCodesWindowsUncTestPaths();
            return true;
         }

         return false;
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
      private ProcessStartInfo BuildPSStartInfoElevated()
      {
         // returns null when the PowerShell executable path cannot be found.

         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         var pwd = dirUtilities.GetCurrentDirectory();

         var psExeFullPath = GetPowerShellPath();
         if (psExeFullPath == null)
         {
            return null;
         }

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

      // ReSharper disable once UnusedMember.Local
      [SuppressMessage("SonarLint.Naming", "S100: Methods and properties should be named in PascalCase", Justification = "False positive, PS is an abbreviation of PowerShell")]
      private ProcessStartInfo BuildPSStartInfoNotElevated()
      {
         // returns null when the PowerShell executable path cannot be found.

         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         var pwd = dirUtilities.GetCurrentDirectory();

         var psExeFullPath = GetPowerShellPath();
         if (psExeFullPath == null)
         {
            return null;
         }

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
         var startInfo = BuildPSStartInfoElevated();
         if (startInfo == null)
         {
            const String err = "failed to create PowerShell start info";
            Trace.WriteLine($"WARNING: {err}.");
            output = String.Empty;
            error = err;
            return -1;
         }

         var exitCode = ExecutePowerShellScriptElevated(startInfo, scriptPath, out output, out error);
         return exitCode;
      }

      private Int32 ExecutePowerShellScriptElevated(ProcessStartInfo startInfo, String scriptPath, out String output, out String error)
      {
         // in order to elevate, must use ShellExecute = true
         // when ShellExecute, redirection of StandardOutput and StandardError is not allowed.
         // redirecting via powershell to 2 files to capture stderr and stdout and sucking those files into the instance.
         //
         // Queried the AzureDevOps build server:
         //    EnableLUA                  : 1
         //    ConsentPromptBehaviorAdmin : 0
         // >> No change other than timeout explains the avoidance of timeout error on build server
         // TODO: find a happy medium between 15 seconds and 120 seconds for PowerShell timeout
         // 15 seconds was timing out on the build server
         // 2 minutes works
         var twoMinutes = new TimeSpan(0, 2, 0);
         const Int32 oneSecondInMilliseconds = 1000;

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         var fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
         var fileReaderUtilities = IocServiceLocator.Resolve<IFileReaderUtilities>();
         var pwd = dirUtilities.GetCurrentDirectory();

         var stdErrLogFileName = fileUtilities.CreateTemporaryFile();
         var stdOutLogFileName = fileUtilities.CreateTemporaryFile();

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

         error = String.Empty;
         output = String.Empty;
         Int32 exitCode;

         using (var ps = new Process())
         {
            ps.StartInfo = startInfo;
            ps.Start();
            // allow 15 seconds for execution (do this after UAC prompts with dialog on UAC enabled machines)
            var expiry = DateTime.UtcNow + twoMinutes;
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
         var startInfo = BuildPSStartInfoElevated();
         if (startInfo == null)
         {
            const String err = "failed to create PowerShell start info";
            Trace.WriteLine($"WARNING: {err}.");
            output = String.Empty;
            error = err;
            return -1;
         }

         var exitCode = ExecutePowerShellScriptElevated(startInfo, scriptPath, out output, out error);
         return exitCode;
      }

      private String GetPowerShellPath()
      {
         if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            return null;
         }

         var environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

         String rv;
         if (environmentUtilities.Is64BitOperatingSystem)
         {
            // 32-bit powershell exe on 64-bit Windows %SystemRoot%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe
            // 64-bit powershell exe on 64-bit Windows %SystemRoot%\system32\WindowsPowerShell\v1.0\powershell.exe
            rv = pathUtilities.Combine(environmentUtilities.GetSpecialFolderPath(Environment.SpecialFolder.Windows), @"SysWOW64\WindowsPowerShell\v1.0\powershell.exe");
         }
         else
         {
            // 32-bit powershell on 32-bit Windows %SystemRoot%\system32\WindowsPowerShell\v1.0\powershell.exe
            rv = pathUtilities.Combine(environmentUtilities.GetSpecialFolderPath(Environment.SpecialFolder.Windows), @"system32\WindowsPowerShell\v1.0\powershell.exe");
         }

         return rv;
      }

      private void InitializeTestHardCodesWindowsLocalTestPaths()
      {
         var envUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

         // parallel knowledge/maintenance here
         var systemDrive = pathUtilities.GetRootPath(envUtilities.GetSpecialFolderPath(Environment.SpecialFolder.System));

         string localFolderRoot = null;
         if (RuntimePlatform.IsWindows())
         {
            localFolderRoot = pathUtilities.Combine(systemDrive, @"Landorphan.Abstractions.Test.UnitTestTarget");
         }
         else
         {
            localFolderRoot = "/";
         }

         // NOTE:  cannot check for the existence of a file(s)/folder(s) in this block because the current user does not have access to many of the extant paths by design.
         TestHardCodes.WindowsLocalTestPaths.SetLocalFolderRoot(localFolderRoot);

         var localFolderEveryoneFullControl = pathUtilities.Combine(localFolderRoot, @"EveryoneFullControl");
         TestHardCodes.WindowsLocalTestPaths.SetLocalFolderEveryoneFullControl(localFolderEveryoneFullControl);

         var localFileFullControlFolderOwnerOnlyFile = pathUtilities.Combine(localFolderEveryoneFullControl, @"OwnerOnly.txt");
         TestHardCodes.WindowsLocalTestPaths.SetLocalFileFullControlFolderOwnerOnlyFile(localFileFullControlFolderOwnerOnlyFile);

         var localFolderOuterFolderNoPermissions = pathUtilities.Combine(localFolderRoot, @"OuterNoPermissions");
         TestHardCodes.WindowsLocalTestPaths.SetLocalFolderOuterFolderNoPermissions(localFolderOuterFolderNoPermissions);

         var localFileOuterFolderNoPermissionsChildFile = pathUtilities.Combine(localFolderOuterFolderNoPermissions, @"OuterExtantFile.txt");
         TestHardCodes.WindowsLocalTestPaths.SetLocalFileOuterFolderNoPermissionsChildFile(localFileOuterFolderNoPermissionsChildFile);

         var localFolderOuterFolderNoPermissionsInnerFolderNoPermissions = pathUtilities.Combine(localFolderOuterFolderNoPermissions, "InnerNoPermissions");
         TestHardCodes.WindowsLocalTestPaths.SetLocalFolderOuterFolderNoPermissionsInnerFolderNoPermissions(localFolderOuterFolderNoPermissionsInnerFolderNoPermissions);

         var localFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile = pathUtilities.Combine(localFolderOuterFolderNoPermissionsInnerFolderNoPermissions, @"InnerExtantFile.txt");
         TestHardCodes.WindowsLocalTestPaths.SetLocalFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile(localFileOuterFolderNoPermissionsInnerFolderNoPermissionsChildFile);

         var localFolderOuterFolderNoPermissionsReadExecuteListFolderContents = pathUtilities.Combine(localFolderOuterFolderNoPermissions, @"\ReadExecuteListFolderContents");
         TestHardCodes.WindowsLocalTestPaths.SetLocalFolderReadExecuteListFolderContents(localFolderOuterFolderNoPermissionsReadExecuteListFolderContents);

         var localFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder = pathUtilities.Combine(localFolderOuterFolderNoPermissionsReadExecuteListFolderContents, @"\ExtantFolder");
         TestHardCodes.WindowsLocalTestPaths.SetLocalFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder(
            localFolderOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFolder);

         var localFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile = pathUtilities.Combine(localFolderOuterFolderNoPermissionsReadExecuteListFolderContents, @"\ExtantFile.txt");
         TestHardCodes.WindowsLocalTestPaths.SetLocalFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile(localFileOuterFolderNoPermissionsReadExecuteListFolderContentsExtantFile);
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
