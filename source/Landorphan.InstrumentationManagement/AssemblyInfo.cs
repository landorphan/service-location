using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct("Landorphan.InstrumentationManagement")]
[assembly: AssemblyTitle("Landorphan.InstrumentationManagement")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en-US")]

#if BuildServer
[assembly: InternalsVisibleTo("Landorphan.InstrumentationManagement.Tests, PublicKey=" + Landorphan.InstrumentationManagement.Resources.StringResources.PublicKey)]
#else
[assembly: InternalsVisibleTo("Landorphan.InstrumentationManagement.Tests")]
#endif
