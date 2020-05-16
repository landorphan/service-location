namespace Landorphan.Abstractions.Tests.BclBaseline
{
    using Landorphan.TestUtilities;

    // ReSharper disable InconsistentNaming

   public class Environment_Baseline_Tests : TestBase
   {
       // MAPPING:
       // Environment:                  IEnvironmentUtilities:
       // ------------------------------------------------------------------------------------------
       // CommandLine                   CommandLine
       // CurrentDirectory              (IDirectoryUtilities.CurrentDirectory)     
       // CurrentManagedThreadId        CurrentManagedThreadId           
       // Exit                          Exit
       // ExitCode                      ExitCode
       // ExpandEnvironmentVariables    ExpandEnvironmentVariables                   
       // FailFast                      FailFast
       // GetCommandLineArgs            GetCommandLineArgs       
       // GetEnvironmentVariable        GetEnvironmentVariable           
       // GetEnvironmentVariables       GetEnvironmentVariables            
       // GetFolderPath                 GetSpecialFolderPath  
       // GetLogicalDrives              GetLogicalDrives     
       // HasShutdownStarted            HasShutdownStarted       
       // Is64BitOperatingSystem        Is64BitOperatingSystem           
       // Is64BitProcess                Is64BitProcess   
       // MachineName                   MachineName
       // NewLine                       NewLine
       // OSVersion                     OSVersion
       // ProcessorCount                ProcessorCount   
       // SetEnvironmentVariable        SetEnvironmentVariable           
       // StackTrace                    StackTrace
       // SystemDirectory               SystemDirectory         
       // SystemPageSize                SystemPageSizeBytes   
       // TickCount                     ElapsedSinceSystemStartupMS
       // UserDomainName                UserDomainName   
       // UserInteractive               UserInteractive    
       // UserName                      UserName
       // Version                       ClrVersion
       // WorkingSet                    WorkingSetBytes

       // These tests document what is:  test failures means an implementation detail has changed
       // change the assertion to document "what is"
       // if you believe the behavior to be incorrect, modify the behavior of the abstraction, fix the abstraction tests, and update these documentation tests
       // to show "what is"

       //[TestMethod]
       //public void Foo()
       //{
       //   var t = typeof(IEnvironmentUtilities);
       //   var members = t.GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
       //   var lst = new List<String>();
       //   foreach (var m in members)
       //   {
       //      lst.Add(m.Name);
       //   }

       //   lst.Sort();
       //   foreach (var s in lst)
       //   {
       //      Trace.WriteLine(s);
       //   }
       //}
   }
}
