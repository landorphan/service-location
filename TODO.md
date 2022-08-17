Rebuild Example projects

PostSharp strategy

GOTCHA:  takeaway use /// <inheritdoc/> above any [SuppressMessage] attributes

SYNTAX UNFLAGGED:
```csharp
      /// <inheritdoc/>
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S4018: Generic methods should provide type parameters",
         Justification = "This generic method delegates implementation to the non-generic version.  I want one implementation (MWP)")]
```

SYNTAX FLAGGED:
```csharp
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S4018: Generic methods should provide type parameters",
         Justification = "This generic method delegates implementation to the non-generic version.  I want one implementation (MWP)")]
      /// <inheritdoc/>
```

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
