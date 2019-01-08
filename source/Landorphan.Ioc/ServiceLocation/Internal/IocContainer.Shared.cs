namespace Landorphan.Ioc.ServiceLocation.Internal
{
   using System;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using Landorphan.Common;
   using Landorphan.Ioc.Logging.Internal;
   using Landorphan.Ioc.Resources;
   using Microsoft.Extensions.Logging;

   // ReSharper disable ConvertToAutoProperty

   /// <summary>
   /// <para>
   /// Default implementation of <see cref="IOwnedIocContainer"/>, <see cref="IIocContainer"/>, <see cref="IIocContainerRegistrar" />, and <see cref="IIocContainerResolver" />.
   /// </para>
   /// <para>
   /// Represents a scope of service location registration and resolution.
   /// </para>
   /// </summary>
   // ReSharper disable once InheritdocConsiderUsage
   [SuppressMessage("SonarLint.CodeSmell", "S1200: Classes should not be coupled to too many other classes (Single Responsibility Principle)")]
   internal sealed partial class IocContainer : DisposableObject, IOwnedIocContainer, IIocContainerManager, IIocContainerRegistrar, IIocContainerResolver
   {
      private readonly IocContainerConfiguration _configuration;

      private readonly SourceWeakEventHandlerSet<ContainerTypeRegistrationEventArgs> _listenersContainerRegistrationAdded =
         new SourceWeakEventHandlerSet<ContainerTypeRegistrationEventArgs>();
      private readonly SourceWeakEventHandlerSet<ContainerTypeRegistrationEventArgs> _listenersContainerRegistrationRemoved =
         new SourceWeakEventHandlerSet<ContainerTypeRegistrationEventArgs>();

      private readonly String _name;

      // Parents own children, reverse references are not owned.
      [DoNotDispose] private readonly IocContainer _parent;

      private readonly Guid _uid;

      private IImmutableSet<IOwnedIocContainer> _children = ImmutableHashSet<IOwnedIocContainer>.Empty;

      private ILogger<IocContainer> _logger;

      private IImmutableSet<Type> _precludedTypes = ImmutableHashSet<Type>.Empty;

      private IImmutableDictionary<RegistrationKeyTypeNamePair, RegistrationValueTypeInstancePair> _registrations =
         ImmutableDictionary<RegistrationKeyTypeNamePair, RegistrationValueTypeInstancePair>.Empty;

      /// <summary>
      /// Initializes static members of the <see cref="IocServiceLocator"/> class.
      /// </summary>
      [SuppressMessage("SonarLint.CodeSmell", "S3963: static fields should be initialized inline")]
      [SuppressMessage("Microsoft.Performance", "CA1810: Initialize reference type static fields inline")]
      static IocContainer()
      {
         // NOTE:  when initialized in-line, the optimizer was allowing calls into the instance methods of IocServiceLocator and IocContainer before initialization was completed.
         // This behavior is not in-line with MSDocs.
         // Use care when modifying the instance and static constructors of both IocServiceLocator and IocContainer.
         // Unexpected behavior:  Static and instance fields are null when client side methods are invoked from tests => NullReferenceExceptions and ResolutionFailedExceptions.
         // This malbehavior does not show in tests of this project.  Only in tests of clients of this library.
         // ---
         // "it (static) is guaranteed to be loaded and to have its fields initialized and its static constructor called before the class is referenced for the first time in your program. MSDocs"
         // This is not true!  (IocServiceLocator.t_RootContainer is null at this point, use IocContainer.RootContainer instead).
         t_root = new IocContainer();
      }

      /// <summary>
      /// Prevents a default instance of the <see cref="IocContainer" /> class from being created.
      /// </summary>
      /// <remarks>
      /// This constructor is used to initialize only the singleton root container.
      /// </remarks>
      private IocContainer()
      {
         // NOTE:  when initialized in-line, the optimizer was allowing calls into the instance methods of IocServiceLocator and IocContainer before initialization was completed.
         // This behavior is not in-line with MSDocs.
         // Use care when modifying the instance and static constructors of both IocServiceLocator and IocContainer.
         // Unexpected behavior:  Static and instance fields are null when client side methods are invoked from tests => NullReferenceExceptions and ResolutionFailedExceptions.
         // This malbehavior does not show in tests of this project.  Only in tests of clients of this library.
         // ---
         // "it (static) is guaranteed to be loaded and to have its fields initialized and its static constructor called before the class is referenced for the first time in your program. MSDocs"
         // This is not true!  (IocServiceLocator.t_RootContainer is null at this point, use IocContainer.RootContainer instead).

         // used to instantiate the root container
         // This assembly's implementation of IocServiceLocator requires a single root container.
         _uid = Guid.Empty;
         _name = String.Empty;
         _parent = null;

         _configuration = new IocContainerConfiguration(this);
         _configuration.ConfigurationChanged += ThisContainerConfigurationChanged;
      }

      private IocContainer(Guid uid, String name)
      {
         // NOTE:  when initialized in-line, the optimizer was allowing calls into the instance methods of IocServiceLocator and IocContainer before initialization was completed.
         // This behavior is not in-line with MSDocs.
         // Use care when modifying the instance and static constructors of both IocServiceLocator and IocContainer.
         // Unexpected behavior:  Static and instance fields are null when client side methods are invoked from tests => NullReferenceExceptions and ResolutionFailedExceptions.
         // This malbehavior does not show in tests of this project.  Only in tests of clients of this library.
         // ---
         // "it (static) is guaranteed to be loaded and to have its fields initialized and its static constructor called before the class is referenced for the first time in your program. MSDocs"
         // This is not true!  (IocServiceLocator.t_RootContainer is null at this point, use IocContainer.RootContainer instead).

         // this constructor is for use solely by tests.
         _parent = null;
         _uid = uid;
         _name = name.TrimNullToEmpty();
         _configuration = new IocContainerConfiguration(this);
         _configuration.ConfigurationChanged += ThisContainerConfigurationChanged;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="IocContainer" /> class.
      /// </summary>
      /// <param name="parent">
      /// The parent container (required).
      /// </param>
      /// <param name="name">
      /// The name of this container (not required, will be <see cref="string.Empty" />).
      /// </param>
      /// <exception cref="ArgumentNullException">
      /// Thrown when <paramref name="parent" />is null.
      /// </exception>
      private IocContainer(IocContainer parent, String name)
      {
         // NOTE:  when initialized in-line, the optimizer was allowing calls into the instance methods of IocServiceLocator and IocContainer before initialization was completed.
         // This behavior is not in-line with MSDocs.
         // Use care when modifying the instance and static constructors of both IocServiceLocator and IocContainer.
         // Unexpected behavior:  Static and instance fields are null when client side methods are invoked from tests => NullReferenceExceptions and ResolutionFailedExceptions.
         // This malbehavior does not show in tests of this project.  Only in tests of clients of this library.
         // ---
         // "it (static) is guaranteed to be loaded and to have its fields initialized and its static constructor called before the class is referenced for the first time in your program. MSDocs"
         // This is not true!  (IocServiceLocator.t_RootContainer is null at this point, use IocContainer.RootContainer instead).

         // used by CreateChildContainer()

         parent.ArgumentNotNull(nameof(parent));

         _uid = Guid.NewGuid();
         _name = name.TrimNullToEmpty();
         _parent = parent;

         _configuration = new IocContainerConfiguration(this);
         _configuration.ConfigurationChanged += ThisContainerConfigurationChanged;

         Disposing += parent.ChildContainerDisposing;
      }

      /// <inheritdoc />
      public IIocContainer Container => this;

      /// <inheritdoc />
      public IIocContainerManager Manager => this;

      /// <inheritdoc />
      public String Name => _name;

      /// <inheritdoc />
      public IIocContainerRegistrar Registrar => this;

      /// <inheritdoc />
      public IIocContainerResolver Resolver => this;

      /// <inheritdoc />
      public Guid Uid => _uid;

      /// <inheritdoc />
      IImmutableDictionary<IRegistrationKey, IRegistrationValue> IIocContainerRegistrationRepository.Registrations
      {
         get
         {
            CheckForNewRegistrations();
            var rv = _registrations.ToImmutableDictionary(
               kvp => (IRegistrationKey) kvp.Key,
               kvp => (IRegistrationValue) kvp.Value
            );
            return rv;
         }
      }

      private Boolean CanLog(out ILogger<IocContainer> logger, out IIocLoggingUtilitiesService loggingUtilitiesService)
      {
         logger = null;
         loggingUtilitiesService = null;

         if (IocServiceLocator.Instance == null)
         {
            // still bootstrapping
            return false;
         }

         var ambientContainer = IocServiceLocator.AmbientContainer;
         if (ambientContainer == null)
         {
            // still bootstrapping
            return false;
         }

         // lazily create the logger
         // using ambientContainer.Resolver.TryResolve creates an infinite loop.
         if (ambientContainer.Resolver.TryResolve(out ILoggerFactory loggerFactory))
         {
            if (_logger == null)
            {
               _logger = loggerFactory?.CreateLogger<IocContainer>();
            }

            logger = _logger;

            if (ambientContainer.Resolver.TryResolve(out loggingUtilitiesService))
            {
               return true;
            }
         }

         return false;
      }

      private void CheckForNewRegistrations()
      {
         var thisAssembly = GetType().Assembly;
         var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToImmutableHashSet();
         // TODO: figure out the performance cost of this kludge.  
         // This optimization breaks dependent assemblies.  Perhaps locating the assembly of the requested typ and checking for the self-registrar.
         //if (loadedAssemblies.Contains(thisAssembly))
         //{
         //   // stop using the kludge, rely on the CurrentDomain.AssemblyLoaded event to maintain the state of service locator and the root container.
         //   return;
         //}

         // To make things even more interesting, the misbehavior this kludge guards against seems to be intermittent.

         // Kludge to address initialization race.
         IocServiceLocator.InternalInstance.CurrentDomainAssemblyLoad(null, null);
      }

      private void TryLogChildAddedOrRemoved(Int32 eventId, IIocContainerMetaIdentity parentContainer, IIocContainerMetaIdentity childContainer)
      {
         // reduce the complexities of in-line logging

         // do not use _logger, grab the instance form CanLog(...)
         if (CanLog(out var logger, out var loggingUtils))
         {
            String message;
            switch (eventId)
            {
               case IocEventIdCodes.IocContainer.ChildContainerAdded:
                  loggingUtils.LoggingUtilitiesForIocContainer.GetMessageChildContainerAdded(parentContainer, childContainer, out message);
                  break;

               case IocEventIdCodes.IocContainer.ChildContainerRemoved:
                  loggingUtils.LoggingUtilitiesForIocContainer.GetMessageChildContainerRemoved(parentContainer, childContainer, out message);
                  break;

               default:
                  message = null;
                  break;
            }

            if (message != null)
            {
               logger.LogInformation(eventId, message);
            }
         }
      }

      private void TryLogConfigurationChanged(Int32 eventId, IIocContainerConfiguration configuration)
      {
         // reduce the complexities of in-line logging

         // do not use _logger, grab the instance form CanLog(...)
         if (CanLog(out var logger, out var loggingUtils))
         {
            String message;
            switch (eventId)
            {
               case IocEventIdCodes.IocContainer.ConfigurationChanged:
                  loggingUtils.LoggingUtilitiesForIocContainer.GetMessageConfigurationChanged(configuration, out message);
                  break;

               default:
                  message = null;
                  break;
            }

            if (message != null)
            {
               logger.LogInformation(eventId, message);
            }
         }
      }

      private void TryLogPrecludedTypeAddedOrRemoved(Int32 eventId, IIocContainerMetaIdentity container, Type precludedType)
      {
         // reduce the complexities of in-line logging

         // do not use _logger, grab the instance form CanLog(...)
         if (CanLog(out var logger, out var loggingUtils))
         {
            String message;
            switch (eventId)
            {
               case IocEventIdCodes.IocContainer.PrecludedTypeAdded:
                  loggingUtils.LoggingUtilitiesForIocContainer.GetMessagePrecludedTypeAdded(container, precludedType, out message);
                  break;

               case IocEventIdCodes.IocContainer.PrecludedTypeRemoved:
                  loggingUtils.LoggingUtilitiesForIocContainer.GetMessagePrecludedTypeRemoved(container, precludedType, out message);
                  break;

               default:
                  message = null;
                  break;
            }

            if (message != null)
            {
               logger.LogInformation(eventId, message);
            }
         }
      }

      private void TryLogRegistrationAddedOrRemoved(Int32 eventId, IRegistrationKey registrationKey, Type toType, Object instance)
      {
         // reduce the complexities of in-line logging

         // do not use _logger, grab the instance form CanLog(...)
         if (CanLog(out var logger, out var loggingUtils))
         {
            String message;
            switch (eventId)
            {
               case IocEventIdCodes.IocContainer.RegistrationAdded:
                  loggingUtils.LoggingUtilitiesForIocContainer.GetMessageRegistrationAdded(this, registrationKey, toType, instance, out message);
                  break;

               case IocEventIdCodes.IocContainer.RegistrationRemoved:
                  loggingUtils.LoggingUtilitiesForIocContainer.GetMessageRegistrationRemoved(this, registrationKey, out message);
                  break;

               default:
                  message = null;
                  break;
            }

            if (message != null)
            {
               logger.LogInformation(eventId, message);
            }
         }
      }

      private void ChildContainerDisposing(Object sender, EventArgs events)
      {
         // handles Disposing events fired by child containers.
         if (sender is IOwnedIocContainer senderAsIOwnedIocContainer)
         {
            _children = _children.Remove(senderAsIOwnedIocContainer);
         }

         if (sender is IIocContainerMetaIdentity senderAsMetaIdentity)
         {
            OnContainerChildRemoved(this, senderAsMetaIdentity);
         }
      }

      private void ThisContainerConfigurationChanged(Object sender, EventArgs events)
      {
         // handles container configuration changed event.

         // maintain state
         if (!_configuration.AllowNamedImplementations)
         {
            // removed named registrations...
            CheckForNewRegistrations();
            var was = _registrations;
            _registrations = (from reg in _registrations where reg.Key.IsDefaultRegistration select reg).ToImmutableDictionary();
            var removedKeys = (from reg in was where !reg.Key.IsDefaultRegistration select reg.Key).ToImmutableHashSet();
            foreach (var key in removedKeys)
            {
               OnContainerRegistrationRemoved(key);
            }
         }

         if (!_configuration.AllowPreclusionOfTypes)
         {
            // removed precluded types...
            var was = _precludedTypes;
            _precludedTypes = _precludedTypes.Clear();
            var removed = (from precludedType in was select precludedType).ToImmutableHashSet();
            foreach (var precludedType in removed)
            {
               OnContainerPrecludedTypeRemoved(precludedType);
            }
         }

         // no other maintenance needed.

         // Fire the event for this instance.
         OnContainerConfigurationChanged();
      }
   }
}