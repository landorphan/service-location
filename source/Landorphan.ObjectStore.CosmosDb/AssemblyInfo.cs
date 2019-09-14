using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct("Landorphan.ObjectStore.CosmosDb")]
[assembly: AssemblyTitle("Landorphan.ObjectStore.CosmosDb")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("en-US")]

#if BuildServer
[assembly: InternalsVisibleTo("Landorphan.ObjectStore.CosmosDb.Tests, PublicKey=" + Landorphan.ObjectStore.CosmosDb.Resources.StringResources.PublicKey)]
#else
[assembly: InternalsVisibleTo("Landorphan.ObjectStore.CosmosDb.Tests")]
#endif
