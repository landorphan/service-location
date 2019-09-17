using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct("Landorphan.Abstractions")]
[assembly: AssemblyTitle("Landorphan.Abstractions")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en-US")]

#if BuildServer
[assembly: InternalsVisibleTo("Landorphan.Abstractions.Tests, PublicKey=" + Landorphan.Abstractions.Resources.StringResources.PublicKey)]
#else
[assembly: InternalsVisibleTo("Landorphan.Abstractions.Tests")]
#endif
