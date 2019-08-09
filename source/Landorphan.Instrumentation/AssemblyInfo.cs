using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Landorphan.Instrumentation")]
[assembly: AssemblyProduct("Landorphan.Instrumentation")]
[assembly: AssemblyTitle("Landorphan.Instrumentation")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en-US")]

#if BuildServer
[assembly: InternalsVisibleTo("Landorphan.Instrumentation.Tests, PublicKey=" + Landorphan.Instrumentation.Resources.StringResources.PublicKey)]
#else
[assembly: InternalsVisibleTo("Landorphan.Instrumentation.Tests")]
#endif
