using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Landorphan.Common")]
[assembly: AssemblyProduct("Landorphan.Common")]
[assembly: AssemblyTitle("Landorphan.Common")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en-US")]

#if BuildServer
[assembly: InternalsVisibleTo("Landorphan.Abstractions.Tests, PublicKey=" + Landorphan.Abstractions.Resources.StringResources.PublicKey)]
#else
[assembly: InternalsVisibleTo("Landorphan.Abstractions.Tests")]
#endif