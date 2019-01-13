namespace Landorphan.Ioc.ServiceLocation.Internal
{
   using System;
   using Landorphan.Common;

   public sealed partial class IocContainer : DisposableObject, IOwnedIocContainer, IIocContainerManager, IIocContainerRegistrar, IIocContainerResolver
   {
      private static readonly IocContainer t_root;

      public static IIocContainer RootContainer => t_root;

      internal static IocContainer InternalRootContainer => t_root;

      /// <value>
      /// Factory method used by tests to create an isolated container with no pardon.
      /// </value>
      public static IocContainer TestHookCreateIsolatedContainer(Guid uid, String name)
      {
         var rv = new IocContainer(uid, name);
         return rv;
      }
   }
}