namespace Landorphan.Ioc.ServiceLocation.Internal
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using Landorphan.Common;
   using Landorphan.Ioc.Resources;

   // ReSharper disable ConvertToAutoProperty
   // ReSharper disable InheritdocConsiderUsage
   // ReSharper disable RedundantExtendsListEntry

   internal sealed partial class IocContainer : DisposableObject, IOwnedIocContainer, IIocContainerManager, IIocContainerRegistrar, IIocContainerResolver
   {
      private readonly SourceWeakEventHandlerSet<ContainerParentChildEventArgs> _listenersContainerChildAdded = new SourceWeakEventHandlerSet<ContainerParentChildEventArgs>();
      private readonly SourceWeakEventHandlerSet<ContainerParentChildEventArgs> _listenersContainerChildRemoved = new SourceWeakEventHandlerSet<ContainerParentChildEventArgs>();
      private readonly SourceWeakEventHandlerSet<ContainerConfigurationEventArgs> _listenersContainerConfigurationChange = new SourceWeakEventHandlerSet<ContainerConfigurationEventArgs>();
      private readonly SourceWeakEventHandlerSet<ContainerTypeEventArgs> _listenersContainerPrecludedTypeAdded = new SourceWeakEventHandlerSet<ContainerTypeEventArgs>();
      private readonly SourceWeakEventHandlerSet<ContainerTypeEventArgs> _listenersContainerPrecludedTypeRemoved = new SourceWeakEventHandlerSet<ContainerTypeEventArgs>();

      /// <inheritdoc />
      event EventHandler<ContainerParentChildEventArgs> IIocContainerManager.ContainerChildAdded
      {
         add => _listenersContainerChildAdded.Add(value);
         remove => _listenersContainerChildAdded.Remove(value);
      }

      /// <inheritdoc />
      event EventHandler<ContainerParentChildEventArgs> IIocContainerManager.ContainerChildRemoved
      {
         add => _listenersContainerChildRemoved.Add(value);
         remove => _listenersContainerChildRemoved.Remove(value);
      }

      /// <inheritdoc />
      event EventHandler<ContainerConfigurationEventArgs> IIocContainerManager.ContainerConfigurationChanged
      {
         add => _listenersContainerConfigurationChange.Add(value);
         remove => _listenersContainerConfigurationChange.Remove(value);
      }

      /// <inheritdoc />
      event EventHandler<ContainerTypeEventArgs> IIocContainerManager.ContainerPrecludedTypeAdded
      {
         add => _listenersContainerPrecludedTypeAdded.Add(value);
         remove => _listenersContainerPrecludedTypeAdded.Remove(value);
      }

      /// <inheritdoc />
      event EventHandler<ContainerTypeEventArgs> IIocContainerManager.ContainerPrecludedTypeRemoved
      {
         add => _listenersContainerPrecludedTypeRemoved.Add(value);
         remove => _listenersContainerPrecludedTypeRemoved.Remove(value);
      }

      /// <inheritdoc />
      IIocContainerConfiguration IIocContainerManager.Configuration => _configuration;

      /// <inheritdoc />
      Boolean IIocContainerManager.IsConfigurationLocked => _configuration.IsReadOnly;

      IReadOnlyCollection<Type> IIocContainerManager.PrecludedTypes => _precludedTypes;

      [SuppressMessage("Microsoft.Usage", "CA2208: Instantiate argument exceptions correctly", Justification = "Using precludedType parameters (MWP)")]
      Boolean IIocContainerManager.AddPrecludedType<TPrecluded>()
      {
         if (!_configuration.AllowPreclusionOfTypes)
         {
            throw new ContainerConfigurationPrecludedTypesDisabledException(this, null, null);
         }

         if (IsRegisteredDefaultOrNamedInThisOrAncestorContainer(this, typeof(TPrecluded)))
         {
            throw new CannotPrecludeRegisteredTypeArgumentException(typeof(TPrecluded), nameof(TPrecluded));
         }

         var was = _precludedTypes;
         var precludedType = typeof(TPrecluded);
         if (precludedType.IsInterface || precludedType.IsAbstract)
         {
            _precludedTypes = _precludedTypes.Add(precludedType);
         }

         var rv = !ReferenceEquals(was, _precludedTypes);
         if (rv)
         {
            OnContainerPrecludedTypeAdded(precludedType);
         }

         return rv;
      }

      /// <inheritdoc/>
      Boolean IIocContainerManager.AddPrecludedType(Type precludedType)
      {
         if (!_configuration.AllowPreclusionOfTypes)
         {
            throw new ContainerConfigurationPrecludedTypesDisabledException(this, null, null);
         }

         if (IsRegisteredDefaultOrNamedInThisOrAncestorContainer(this, precludedType))
         {
            throw new CannotPrecludeRegisteredTypeArgumentException(precludedType, nameof(precludedType));
         }

         var was = _precludedTypes;

         if (precludedType != null && (precludedType.IsInterface || precludedType.IsAbstract))
         {
            _precludedTypes = _precludedTypes.Add(precludedType);
         }

         var rv = !ReferenceEquals(was, _precludedTypes);
         if (rv)
         {
            OnContainerPrecludedTypeAdded(precludedType);
         }

         return rv;
      }

      /// <inheritdoc/>
      IIocContainer IIocContainerManager.CreateChildContainer(String name)
      {
         IOwnedIocContainer ownedChildContainer = new IocContainer(this, name);
         _children = _children.Add(ownedChildContainer);

         OnContainerChildAdded(this, ownedChildContainer);
         return ownedChildContainer;
      }

      /// <inheritdoc />
      Boolean IIocContainerManager.LockConfiguration()
      {
         var was = _configuration.IsReadOnly;
         if (!was)
         {
            _configuration.MakeReadOnly();
         }

         return was;
      }

      /// <inheritdoc/>
      Boolean IIocContainerManager.RemovePrecludedType<TPrecluded>()
      {
         // does not check whether preclusion of types configuration is disabled by design.
         var was = _precludedTypes;
         var precludedType = typeof(TPrecluded);

         _precludedTypes = _precludedTypes.Remove(precludedType);

         var rv = !ReferenceEquals(was, _precludedTypes);
         if (rv)
         {
            OnContainerPrecludedTypeRemoved(precludedType);
         }

         return rv;
      }

      /// <inheritdoc/>
      Boolean IIocContainerManager.RemovePrecludedType(Type precludedType)
      {
         // does not check whether preclusion of types configuration is disabled by design.
         var was = _precludedTypes;
         _precludedTypes = _precludedTypes.Remove(precludedType);
         var rv = !ReferenceEquals(was, _precludedTypes);
         if (rv)
         {
            OnContainerPrecludedTypeRemoved(precludedType);
         }

         return rv;
      }

      private Boolean IsRegisteredDefaultOrNamedInThisOrAncestorContainer(IIocContainerRegistrationRepository container, Type type)
      {
         if (type == null || !(type.IsInterface || type.IsAbstract) || type.ContainsGenericParameters)
         {
            return false;
         }

         var thisRepository = container;
         while (thisRepository != null)
         {
            var immutableDictionary = thisRepository.Registrations;
            var q = from key in immutableDictionary.Keys where key.RegisteredType == type select key;
            if (q.Any())
            {
               return true;
            }

            // loop maintenance
            var parent = thisRepository.Container.Parent;
            thisRepository = parent?.Resolver;
         }

         return false;
      }

      private void OnContainerChildAdded(IIocContainerMetaIdentity parent, IIocContainerMetaIdentity child)
      {
         // fires the event
         TryLogChildAddedOrRemoved(IocEventIdCodes.IocContainer.ChildContainerAdded, parent, child);

         // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
         var e = new ContainerParentChildEventArgs(parent, child);
         _listenersContainerChildAdded?.Invoke(this, e);
      }

      private void OnContainerChildRemoved(IIocContainerMetaIdentity parent, IIocContainerMetaIdentity child)
      {
         // fires the event
         TryLogChildAddedOrRemoved(IocEventIdCodes.IocContainer.ChildContainerRemoved, parent, child);

         // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
         var e = new ContainerParentChildEventArgs(parent, child);
         _listenersContainerChildRemoved?.Invoke(this, e);
      }

      private void OnContainerConfigurationChanged()
      {
         // fires the event

         var configuration = _configuration;
         TryLogConfigurationChanged(IocEventIdCodes.IocContainer.ConfigurationChanged, configuration);

         var e = new ContainerConfigurationEventArgs(configuration);
         _listenersContainerConfigurationChange?.Invoke(this, e);
      }

      private void OnContainerPrecludedTypeAdded(Type precludedType)
      {
         // fires the event
         TryLogPrecludedTypeAddedOrRemoved(IocEventIdCodes.IocContainer.PrecludedTypeAdded, this, precludedType);

         // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
         var e = new ContainerTypeEventArgs(this, precludedType);
         _listenersContainerPrecludedTypeAdded?.Invoke(this, e);
      }

      private void OnContainerPrecludedTypeRemoved(Type precludedType)
      {
         // fires the event
         TryLogPrecludedTypeAddedOrRemoved(IocEventIdCodes.IocContainer.PrecludedTypeRemoved, this, precludedType);

         // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
         var e = new ContainerTypeEventArgs(this, precludedType);
         _listenersContainerPrecludedTypeRemoved?.Invoke(this, e);
      }
   }
}
