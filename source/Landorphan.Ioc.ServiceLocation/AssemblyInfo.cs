using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct("Landorphan.Ioc.ServiceLocation")]
[assembly: AssemblyTitle("Landorphan.Ioc.ServiceLocation")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en-US")]

#if BuildServer
[assembly: InternalsVisibleTo("Landorphan.Ioc.ServiceLocation.Testability, PublicKey=" + Landorphan.Ioc.Resources.StringResources.PublicKey)]
[assembly: InternalsVisibleTo("Landorphan.Ioc.ServiceLocation.Tests, PublicKey=" + Landorphan.Ioc.Resources.StringResources.PublicKey)]
[assembly: InternalsVisibleTo("IocPerf, PublicKey=" + Landorphan.Ioc.Resources.StringResources.PublicKey)]
#else
[assembly: InternalsVisibleTo("Landorphan.Ioc.ServiceLocation.Tests")]
[assembly: InternalsVisibleTo("Landorphan.Ioc.ServiceLocation.Testability")]
[assembly: InternalsVisibleTo("IocPerf")]
#endif
