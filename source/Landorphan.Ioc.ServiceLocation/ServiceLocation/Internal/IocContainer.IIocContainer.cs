﻿namespace Landorphan.Ioc.ServiceLocation.Internal
{
    using System.Collections.Generic;
    using Landorphan.Common;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    // ReSharper disable ConvertToAutoProperty
   // ReSharper disable InheritdocConsiderUsage
   // ReSharper disable RedundantExtendsListEntry

   internal sealed partial class IocContainer : DisposableObject, IOwnedIocContainer, IIocContainerManager, IIocContainerRegistrar, IIocContainerResolver
   {
       /// <inheritdoc/>
      IReadOnlyCollection<IIocContainer> IIocContainer.Children => _children;

       /// <inheritdoc/>
      bool IIocContainer.IsRoot => _parent.IsNull();

       /// <inheritdoc/>
      IIocContainer IIocContainer.Parent => _parent;
   }
}
