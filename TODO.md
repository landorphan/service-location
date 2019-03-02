To Enable code analysis in .Net Core:
   install Nuget Microsoft.NetCore.Analyzers
Create a ruleset (ape the existing implementation).
   Create a file named "ProjectName.RuleSet" next to the csproj file
   Include it in the project
   Set the build action to C# analyzer additional file
   Edit the CSPROJ file adding "<CodeAnalysisRuleSet>Project.Name.ruleset</CodeAnalysisRuleSet>"
   In .Net Core projects, there is no need to add <RunCodeAnalysis>true</RunCodeAnalysis>, it has no effect.

GOTCHA:  takeaway use /// <inheritdoc/> above any [SuppressMessage] attributes

SYNTAX UNFLAGGED:
      /// <inheritdoc/>
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S4018: Generic methods should provide type parameters",
         Justification = "This generic method delegates implementation to the non-generic version.  I want one implementation (MWP)")]

SYNTAX FLAGGED:
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S4018: Generic methods should provide type parameters",
         Justification = "This generic method delegates implementation to the non-generic version.  I want one implementation (MWP)")]
      /// <inheritdoc/>


Strange Test Failure:
Configuration system failed to initialize
   at System.Configuration.ClientConfigurationSystem.EnsureInit(String configKey)
   at System.Configuration.ClientConfigurationSystem.PrepareClientConfigSystem(String sectionName)
   at System.Configuration.ClientConfigurationSystem.System.Configuration.Internal.IInternalConfigSystem.GetSection(String sectionName)
   at System.Configuration.ConfigurationManager.GetSection(String sectionName)
   at System.Configuration.PrivilegedConfigurationManager.GetSection(String sectionName)
   at System.Diagnostics.DiagnosticsConfiguration.GetConfigSection()
   at System.Diagnostics.DiagnosticsConfiguration.Initialize()
   at System.Diagnostics.DiagnosticsConfiguration.get_IndentSize()
   at System.Diagnostics.TraceInternal.InitializeSettings()
   at System.Diagnostics.TraceInternal.get_Listeners()
   at System.Diagnostics.Trace.get_Listeners()
   at Microsoft.VisualStudio.TestPlatform.MSTestAdapter.PlatformServices.TraceListenerManager.Add(ITraceListener traceListener)

This started affecting all Landorphan.Ioc.Tests even though the App.Config file for the same had not been changed (specflow only).  Deleting the config file resolved the issue.