namespace Landorphan.Ioc.ServiceLocation.Internal
{
    using System;
    using Landorphan.Common;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    // ReSharper disable ConvertToAutoProperty
   // ReSharper disable InheritdocConsiderUsage
   // ReSharper disable RedundantExtendsListEntry

   internal sealed partial class IocContainer : DisposableObject, IOwnedIocContainer, IIocContainerManager, IIocContainerRegistrar, IIocContainerResolver
   {
       private static readonly IocContainer t_root;

       public static IIocContainer RootContainer => t_root;

       internal static IocContainer InternalRootContainer => t_root;

       /// <value>
      /// Factory method used by tests to create an isolated container with no pardon.
      /// </value>
      internal static IocContainer TestHookCreateIsolatedContainer(Guid uid, string name)
      {
         var rv = new IocContainer(uid, name);
         return rv;
      }
   }
}
