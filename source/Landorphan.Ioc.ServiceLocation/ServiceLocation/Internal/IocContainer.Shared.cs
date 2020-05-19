namespace Landorphan.Ioc.ServiceLocation.Internal
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Landorphan.Common;
    using Landorphan.Common.Threading;
    using Landorphan.Ioc.Logging;
    using Landorphan.Ioc.Logging.Internal;
    using Landorphan.Ioc.Resources;
    using Landorphan.Ioc.ServiceLocation.EventArguments;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    // ReSharper disable ConvertToAutoProperty
    // ReSharper disable InheritdocConsiderUsage
    // ReSharper disable RedundantExtendsListEntry

    /// <summary>
    /// <para>
    /// Default implementation of <see cref="IOwnedIocContainer"/>, <see cref="IIocContainer"/>, <see cref="IIocContainerRegistrar" />, and <see cref="IIocContainerResolver" />.
    /// </para>
    /// <para>
    /// Represents a scope of service location registration and resolution.
    /// </para>
    /// </summary>
    [SuppressMessage("SonarLint.CodeSmell", "S1200: Classes should not be coupled to too many other classes (Single Responsibility Principle)")]
    internal sealed partial class IocContainer : DisposableObject, IOwnedIocContainer, IIocContainerManager, IIocContainerRegistrar, IIocContainerResolver
    {
        private readonly IocContainerConfiguration _configuration;
        private readonly SourceWeakEventHandlerSet<ContainerTypeRegistrationEventArgs> _listenersContainerRegistrationAdded =
            new SourceWeakEventHandlerSet<ContainerTypeRegistrationEventArgs>();
        private readonly SourceWeakEventHandlerSet<ContainerTypeRegistrationEventArgs> _listenersContainerRegistrationRemoved =
            new SourceWeakEventHandlerSet<ContainerTypeRegistrationEventArgs>();
        private readonly string _name;

        // Parents own children, reverse references are not owned.
        [DoNotDispose]
        private readonly IocContainer _parent;
        private readonly NonRecursiveLock _registrationsLock = new NonRecursiveLock();
        private readonly Guid _uid;
        private IImmutableSet<IOwnedIocContainer> _children = ImmutableHashSet<IOwnedIocContainer>.Empty;
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
            _name = string.Empty;
            _parent = null;

            _configuration = new IocContainerConfiguration(this);
            _configuration.ConfigurationChanged += ThisContainerConfigurationChanged;
        }

        private IocContainer(Guid uid, string name)
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
        private IocContainer(IocContainer parent, string name)
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
        public string Name => _name;

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
                IImmutableDictionary<IRegistrationKey, IRegistrationValue> rv;
                using (_registrationsLock.EnterReadLock())
                {
                    rv = _registrations.ToImmutableDictionary(
                        kvp => (IRegistrationKey)kvp.Key,
                        kvp => (IRegistrationValue)kvp.Value
                    );
                }

                return rv;
            }
        }

        private bool CanLog(out IIocLogger<IocContainer> logger, out IIocLoggingUtilitiesService loggingUtilitiesService)
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
            if (ambientContainer.Resolver.TryResolve(out IIocLoggerManager loggerFactory))
            {
                logger = loggerFactory.GetLogger<IocContainer>();

                if (ambientContainer.Resolver.TryResolve(out loggingUtilitiesService))
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckForNewRegistrations()
        {
            // Kludge to address initialization race.
            IocServiceLocator.InternalInstance.CurrentDomainAssemblyLoad(null, null);
        }

        private void TryLogChildAddedOrRemoved(int eventId, IIocContainerMetaIdentity parentContainer, IIocContainerMetaIdentity childContainer)
        {
            // reduce the complexities of in-line logging

            // do not use _logger, grab the instance form CanLog(...)
            if (CanLog(out var logger, out var loggingUtils))
            {
                string message;
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

        private void TryLogConfigurationChanged(int eventId, IIocContainerConfiguration configuration)
        {
            // reduce the complexities of in-line logging

            // do not use _logger, grab the instance form CanLog(...)
            if (CanLog(out var logger, out var loggingUtils))
            {
                string message;
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

        private void TryLogPrecludedTypeAddedOrRemoved(int eventId, IIocContainerMetaIdentity container, Type precludedType)
        {
            // reduce the complexities of in-line logging

            // do not use _logger, grab the instance form CanLog(...)
            if (CanLog(out var logger, out var loggingUtils))
            {
                string message;
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

        private void TryLogRegistrationAddedOrRemoved(int eventId, IRegistrationKey registrationKey, Type toType, object instance)
        {
            // reduce the complexities of in-line logging

            // do not use _logger, grab the instance form CanLog(...)
            if (CanLog(out var logger, out var loggingUtils))
            {
                string message;
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

        private void ChildContainerDisposing(object sender, EventArgs events)
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

        private void ThisContainerConfigurationChanged(object sender, EventArgs events)
        {
            // handles container configuration changed event.

            // maintain state
            if (!_configuration.AllowNamedImplementations)
            {
                // removed named registrations...
                CheckForNewRegistrations();
                IImmutableDictionary<RegistrationKeyTypeNamePair, RegistrationValueTypeInstancePair> was;
                using (_registrationsLock.EnterWriteLock())
                {
                    was = _registrations;
                    _registrations = (from reg in _registrations where reg.Key.IsDefaultRegistration select reg).ToImmutableDictionary();
                }

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
