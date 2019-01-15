namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;
   using System.Reflection;
   using Landorphan.Common;
   using Landorphan.Common.Threading;
   using Landorphan.Ioc.Resources;
   using Landorphan.Ioc.ServiceLocation.Internal;

   public sealed partial class IocServiceLocator : IIocServiceLocator, IIocServiceLocatorManager
   {
      private readonly AssemblyRegistrarRepository _assemblyRegistrarRepository = new AssemblyRegistrarRepository();
      private readonly SourceWeakEventHandlerSet<ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs> _listenersAfterContainerAssemblyCollectionSelfRegistrationInvoked =
         new SourceWeakEventHandlerSet<ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs>();
      private readonly SourceWeakEventHandlerSet<ContainerIndividualAssemblyRegistrarInvokedEventArgs> _listenersAfterContainerAssemblySingleSelfRegistrationInvoked =
         new SourceWeakEventHandlerSet<ContainerIndividualAssemblyRegistrarInvokedEventArgs>();

      private readonly SourceWeakEventHandlerSet<EventArgs> _listenersAmbientContainerChanged = new SourceWeakEventHandlerSet<EventArgs>();
      private readonly SourceWeakEventHandlerSet<ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs> _listenersBeforeContainerAssemblyCollectionSelfRegistrationInvoked =
         new SourceWeakEventHandlerSet<ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs>();
      private readonly SourceWeakEventHandlerSet<ContainerIndividualAssemblyRegistrarInvokedEventArgs> _listenersBeforeContainerAssemblySingleSelfRegistrationInvoked =
         new SourceWeakEventHandlerSet<ContainerIndividualAssemblyRegistrarInvokedEventArgs>();

      private readonly Object _selfRegistrationLockObject = new Object();

      private IIocContainer _ambientContainer;

      private InterlockedBoolean _assemblyLoaded = new InterlockedBoolean(true);

      /// <inheritdoc />
      event EventHandler<ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs> IIocServiceLocatorManager.AfterContainerAssemblyCollectionSelfRegistrationInvoked
      {
         add => _listenersAfterContainerAssemblyCollectionSelfRegistrationInvoked.Add(value);
         remove => _listenersAfterContainerAssemblyCollectionSelfRegistrationInvoked.Remove(value);
      }

      /// <inheritdoc />
      event EventHandler<ContainerIndividualAssemblyRegistrarInvokedEventArgs> IIocServiceLocatorManager.AfterContainerAssemblySingleSelfRegistrationInvoked
      {
         add => _listenersAfterContainerAssemblySingleSelfRegistrationInvoked.Add(value);
         remove => _listenersAfterContainerAssemblySingleSelfRegistrationInvoked.Remove(value);
      }

      /// <inheritdoc/>
      event EventHandler<EventArgs> IIocServiceLocatorManager.AmbientContainerChanged
      {
         add => _listenersAmbientContainerChanged.Add(value);
         remove => _listenersAmbientContainerChanged.Remove(value);
      }

      /// <inheritdoc />
      event EventHandler<ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs> IIocServiceLocatorManager.BeforeContainerAssemblyCollectionSelfRegistrationInvoked
      {
         add => _listenersBeforeContainerAssemblyCollectionSelfRegistrationInvoked.Add(value);
         remove => _listenersBeforeContainerAssemblyCollectionSelfRegistrationInvoked.Remove(value);
      }

      /// <inheritdoc />
      event EventHandler<ContainerIndividualAssemblyRegistrarInvokedEventArgs> IIocServiceLocatorManager.BeforeContainerAssemblySingleSelfRegistrationInvoked
      {
         add => _listenersBeforeContainerAssemblySingleSelfRegistrationInvoked.Add(value);
         remove => _listenersBeforeContainerAssemblySingleSelfRegistrationInvoked.Remove(value);
      }

      /// <inheritdoc/>
      IIocContainer IIocServiceLocatorManager.AmbientContainer => _ambientContainer;

      /// <inheritdoc/>
      IIocContainer IIocServiceLocatorManager.RootContainer => IocContainer.InternalRootContainer;

      /// <inheritdoc/>
      void IIocServiceLocatorManager.SetAmbientContainer(IIocContainer container)
      {
         container.ArgumentNotNull("container");

         if (!ReferenceEquals(_ambientContainer, container))
         {
            if (container is INotifyingQueryDisposable containerAsINotifyingQueryDisposable)
            {
               containerAsINotifyingQueryDisposable.Disposing += ContainerDisposing;
            }

            _ambientContainer = container;

            OnAmbientContainerChanged();
         }
      }

      internal void SelfRegisterAssembliesWithRootContainer()
      {
         if (IocContainer.InternalRootContainer == null || InternalInstance == null)
         {
            // Disposing or still initializing
            return;
         }

         lock (_selfRegistrationLockObject)
         {
            // check and clear the new assembly loaded flag.
            var assemblyLoaded = _assemblyLoaded.ExchangeValue(false);
            if (assemblyLoaded && _assemblyRegistrarRepository.AreNewAssemblyRegistrarsLoaded())
            {
               var snapshotNewRegistrars = _assemblyRegistrarRepository.RefreshAssemblyRegistrars();
               var uniqueAssemblies = (from asr in snapshotNewRegistrars select asr.GetType().Assembly).Distinct().ToImmutableHashSet();

               OnBeforeContainerAssemblyCollectionSelfRegistrationInvoked(IocContainer.InternalRootContainer, uniqueAssemblies);
               foreach (var registrar in snapshotNewRegistrars)
               {
                  OnBeforeContainerAssemblySingleSelfRegistrationInvoked(IocContainer.InternalRootContainer, registrar);

                  registrar.RegisterServiceInstances(IocContainer.InternalRootContainer.Registrar);

                  OnAfterContainerAssemblySingleSelfRegistrationInvoked(IocContainer.InternalRootContainer, registrar);
               }

               OnAfterContainerAssemblyCollectionSelfRegistrationInvoked(IocContainer.InternalRootContainer, uniqueAssemblies);
            }
         }
      }

      private void OnAfterContainerAssemblyCollectionSelfRegistrationInvoked(IIocContainerMetaIdentity container, IEnumerable<Assembly> assemblies)
      {
         // fires the event
         TryLogContainerAssemblyCollectionSelfRegistrationsInvoked(IocEventIdCodes.ServiceLocator.ContainerAssemblyCollectionSelfRegistrationsInvokedAfter, assemblies);

         // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
         var e = new ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs(container, assemblies);
         _listenersAfterContainerAssemblyCollectionSelfRegistrationInvoked?.Invoke(this, e);
      }

      private void OnAfterContainerAssemblySingleSelfRegistrationInvoked(IIocContainerMetaIdentity container, IAssemblySelfRegistration assemblySelfRegistration)
      {
         // fires the event
         TryLogContainerSingleAssemblySelfRegistrationInvoked(IocEventIdCodes.ServiceLocator.ContainerSingleAssemblySelfRegistrationInvokedAfter, assemblySelfRegistration);

         // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
         var e = new ContainerIndividualAssemblyRegistrarInvokedEventArgs(container, assemblySelfRegistration);
         _listenersAfterContainerAssemblySingleSelfRegistrationInvoked?.Invoke(this, e);
      }

      private void OnAmbientContainerChanged()
      {
         TryLogAmbientContainerChanged(IocEventIdCodes.ServiceLocator.AmbientContainerChanged, _ambientContainer);

         // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
         _listenersAmbientContainerChanged?.Invoke(this, EventArgs.Empty);
      }

      private void OnBeforeContainerAssemblyCollectionSelfRegistrationInvoked(IIocContainerMetaIdentity container, IEnumerable<Assembly> assemblies)
      {
         // fires the event
         TryLogContainerAssemblyCollectionSelfRegistrationsInvoked(IocEventIdCodes.ServiceLocator.ContainerAssemblyCollectionSelfRegistrationInvokedBefore, assemblies);

         // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
         var e = new ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs(container, assemblies);
         _listenersBeforeContainerAssemblyCollectionSelfRegistrationInvoked?.Invoke(this, e);
      }

      private void OnBeforeContainerAssemblySingleSelfRegistrationInvoked(IIocContainerMetaIdentity container, IAssemblySelfRegistration assemblySelfRegistration)
      {
         // fires the event
         TryLogContainerSingleAssemblySelfRegistrationInvoked(IocEventIdCodes.ServiceLocator.ContainerSingleAssemblySelfRegistrationsInvokedBefore, assemblySelfRegistration);

         // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
         var e = new ContainerIndividualAssemblyRegistrarInvokedEventArgs(container, assemblySelfRegistration);
         _listenersBeforeContainerAssemblySingleSelfRegistrationInvoked?.Invoke(this, e);
      }

      internal void CurrentDomainAssemblyLoad(Object sender, AssemblyLoadEventArgs args)
      {
         // Normal use:  an event handler fired when assemblies are loaded.
         // Kludge used by IocContainer.CheckForNewRegistrations() to ensure assemblies are loaded before accessing container registrations.
         // This works in concert with SelfRegisterAssembliesWithRootContainer()
         _assemblyLoaded.SetValue(true);
         SelfRegisterAssembliesWithRootContainer();
      }

      private void ContainerDisposing(Object sender, EventArgs e)
      {
         // maintain _ambientContainer:
         if (ReferenceEquals(sender, _ambientContainer))
         {
            // First, try to set it to the parent
            var parent = _ambientContainer.Parent;
            if (parent != null)
            {
               ((IIocServiceLocatorManager)this).SetAmbientContainer(parent);
            }
            else
            {
               // Fall back to root container
               if (IocContainer.InternalRootContainer != null && !ReferenceEquals(sender, IocContainer.InternalRootContainer))
               {
                  ((IIocServiceLocatorManager)this).SetAmbientContainer(IocContainer.InternalRootContainer);
               }
               else
               {
                  // give up the root container is being/has been disposed...
                  _ambientContainer = null;
               }
            }
         }
      }
   }
}
