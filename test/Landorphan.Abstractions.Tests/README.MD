Ported a .Net Framework 4.61 library to .Net Standard 2.0

ACLs are not available in .Net Standard
Some of the behaviors around invalid inputs have changed
   Exceptions are thrown whereas before no exception was thrown
   Exceptions are not being thrown where as before an exception was thrown
   Exception messages are changed
   
Long file support is now on by default
The set of invalid characters for directory and file names has changed

Environment, EnvironmentVariable, and Path will be polished first
Additional components will be polished over time.
   File
   Directory
   Console
   FileInfo (contemplated)
   DirectoryInfo (contemplated)

Added NuGet Microsoft.Windows.Compatibility (for ACLs) and Microsoft.DotNet.Analyzers.Compatibility

Local and UNC test paths arranged on demand for Windows platforms
   The first test to touch read a TestHardCodes.WindowsLocalTestPaths value or TestHardCodes.WindowsUncTestPaths value causes a shell out to powershell to occur, on UAC enabled systems, 
   this pops a UAC confirmation dialog box.  Both of these affect the execution timing of the first test that touches the arranged on demand properties.  
   (you can expect the first test to take 6+ seconds).
