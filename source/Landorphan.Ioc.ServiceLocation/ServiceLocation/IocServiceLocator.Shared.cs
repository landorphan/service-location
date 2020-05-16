namespace Landorphan.Ioc.ServiceLocation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Landorphan.Ioc.Logging;
    using Landorphan.Ioc.Logging.Internal;
    using Landorphan.Ioc.Resources;
    using Landorphan.Ioc.ServiceLocation.Interfaces;
    using Landorphan.Ioc.ServiceLocation.Internal;

    /// <summary>
    /// An inversion of control service locator implementation for dependency injection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Service location allows for the narrow* resolution of registered types (and only registered types) with AppDomain life-time.
    /// Typical usage is as follows:
    /// <example><code>IocServiceLocator.Resolve(typeof(IService))</code></example>
    /// It also allows for overrides of default implementation to support test scenarios.  To support test scenarios, implementations should perform the following:
    ///    1) Create a child container.
    ///    2) Set the ambient container to the child container.
    ///    3) Register services with the child container that should override the default (production/implementation) interfaces.
    ///    4) Execute tests
    ///    5) Dispose of the child container.
    /// An implementation of the above pattern in provided in Landorphan.Ioc.ServiceLocation.Testability.
    /// </para>
    /// <para>
    /// *narrow:  Suppose InterfaceB descends from InterfaceA, and suppose InterfaceB is registered.  Attempting to resolve InterfaceA will fail.
    /// </para>
    /// <para>
    /// This implementation of the service locator allows class libraries to self-register default implementations.  If you need to manually add a registration, you do so as follows:
    /// <example><code>IocServiceLocator.RootContainer.Registrar.RegisterInstance(typeof(IService), new Service());</code></example>
    /// </para>
    /// <para>
    /// Type discovery is limited to registered types.  Inheritance is not considered.
    /// </para>
    /// <para>
    /// Exposes service location capacities as a collection of capacities/roles in <see cref="IIocServiceLocatorMetaSharedCapacities" />.  For typical client scenarios, static methods are available
    /// for simplified syntax.  For example, <see cref="IocServiceLocator.Resolve(Type)"/>.  For more advanced usage, use <see cref="IocServiceLocator.Manager"/> to monitor and manipulate the behavior
    /// of the service locator.
    /// </para>
    /// </remarks>
    public sealed partial class IocServiceLocator : IIocServiceLocator, IIocServiceLocatorManager
    {
        /// <summary>
        /// Initializes static members of the <see cref="IocServiceLocator"/> class.
        /// </summary>
        [SuppressMessage("SonarLint.CodeSmell", "S3963: static fields should be initialized inline")]
        [SuppressMessage("Microsoft.Performance", "CA1810: Initialize reference type static fields inline")]
        static IocServiceLocator()
        {
            // NOTE:  when initialized in-line, the optimizer was allowing calls into the instance methods of IocServiceLocator and IocContainer before initialization was completed.
            // This behavior is not in-line with MSDocs.
            // Use care when modifying the instance and static constructors of both IocServiceLocator and IocContainer.
            // Unexpected behavior:  Static and instance fields are null when client side methods are invoked from tests => NullReferenceExceptions and ResolutionFailedExceptions.
            // This malbehavior does not show in tests of this project.  Only in tests of clients of this library.
            // ---
            // "it (static) is guaranteed to be loaded and to have its fields initialized and its static constructor called before the class is referenced for the first time in your program. MSDocs"
            // This is not true!  (IocServiceLocator.t_RootContainer is null at this point, use IocContainer.RootContainer instead).
            t_singletonInstance = new IocServiceLocator();

            var rootContainer = IocContainer.RootContainer;
            //'/'ILoggerFactory loggerFactory = new LoggerFactory();
            //'/'rootContainer.Registrar.RegisterInstance(loggerFactory);

            //'/'ILogEntryFactory logEntryFactory = new LogEntryFactory();
            //'/'rootContainer.Registrar.RegisterInstance(logEntryFactory);

            //'/'IIocLoggingUtilitiesService loggingUtils = new IocLoggingUtilitiesService();
            //'/'rootContainer.Registrar.RegisterInstance(loggingUtils);

            // ..preclude selected IOC types.
            var iocInterfaces = GetIocInterfacesAndAbstractTypesExceptLoggingInterfaces();
            foreach (var type in iocInterfaces)
            {
                rootContainer.Manager.AddPrecludedType(type);
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="IocServiceLocator" /> class from being created.
        /// </summary>
        /// <remarks>
        /// Initialize the singleton instance.
        /// </remarks>
        private IocServiceLocator()
        {
            // NOTE:  when initialized in-line, the optimizer was allowing calls into the instance methods of IocServiceLocator and IocContainer before initialization was completed.
            // This behavior is not in-line with MSDocs.
            // Use care when modifying the instance and static constructors of both IocServiceLocator and IocContainer.
            // Unexpected behavior:  Static and instance fields are null when client side methods are invoked from tests => NullReferenceExceptions and ResolutionFailedExceptions.
            // This malbehavior does not show in tests of this project.  Only in tests of clients of this library.
            // ---
            // "it (static) is guaranteed to be loaded and to have its fields initialized and its static constructor called before the class is referenced for the first time in your program. MSDocs"
            // This is not true!  (IocServiceLocator.t_RootContainer is null at this point, use IocContainer.RootContainer instead).
            _ambientContainer = IocContainer.RootContainer;

            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainAssemblyLoad;
        }

        /// <inheritdoc />
        public IIocServiceLocatorManager Manager => this;

        /// <inheritdoc />
        public IIocServiceLocator ServiceLocator => this;

        private bool CanLog(out IIocLogger<IocServiceLocator> logger, out IIocLoggingUtilitiesService loggingUtilitiesService)
        {
            logger = null;
            loggingUtilitiesService = null;

            if (IocContainer.InternalRootContainer == null || InternalInstance == null)
            {
                // Disposing or still initializing
                return false;
            }

            if (Instance == null || _ambientContainer == null)
            {
                // still bootstrapping
                return false;
            }

            if (!_ambientContainer.Resolver.IsRegisteredChain<IIocLoggerFactory>())
            {
                // prevent an infinite loop
                return false;
            }

            // lazily create the logger for this class instance (a  singleton).
            // (the logger factory is registered by the IocServiceLocator static ctor)
            var loggerFactory = _ambientContainer.Resolver.Resolve<IIocLoggerManager>();
            logger = loggerFactory.GetLogger<IocServiceLocator>();

            if (_ambientContainer.Resolver.TryResolve(out loggingUtilitiesService))
            {
                return true;
            }
            return false;
        }

        private void TryLogAmbientContainerChanged(int eventId, IIocContainerMetaIdentity newContainer)
        {
            // reduce the complexities of in-line logging

            // do not use _logger, grab the instance form CanLog(...)
            if (CanLog(out var logger, out var loggingUtils))
            {
                string message;
                switch (eventId)
                {
                    case IocEventIdCodes.ServiceLocator.AmbientContainerChanged:
                        loggingUtils.LoggingUtilitiesForIocIocServiceLocator.GetMessageAmbientContainerChanged(newContainer, out message);
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

        private void TryLogContainerAssemblyCollectionSelfRegistrationsInvoked(int eventId, IEnumerable<Assembly> assemblies)
        {
            // reduce the complexities of in-line logging

            // do not use _logger, grab the instance form CanLog(...)
            if (CanLog(out var logger, out var loggingUtils))
            {
                string message;
                switch (eventId)
                {
                    case IocEventIdCodes.ServiceLocator.ContainerAssemblyCollectionSelfRegistrationInvokedBefore:
                        loggingUtils.LoggingUtilitiesForIocIocServiceLocator.
                            GetMessageContainerAssemblyCollectionSelfRegistrationInvokedBefore(IocContainer.InternalRootContainer, assemblies, out message);
                        break;

                    case IocEventIdCodes.ServiceLocator.ContainerAssemblyCollectionSelfRegistrationsInvokedAfter:
                        loggingUtils.LoggingUtilitiesForIocIocServiceLocator.
                            GetMessageContainerAssemblyCollectionSelfRegistrationInvokedAfter(IocContainer.InternalRootContainer, assemblies, out message);
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

        private void TryLogContainerSingleAssemblySelfRegistrationInvoked(int eventId, IAssemblySelfRegistration assemblySelfRegistration)
        {
            // reduce the complexities of in-line logging

            // do not use _logger, grab the instance form CanLog(...)
            if (CanLog(out var logger, out var loggingUtils))
            {
                string message;
                switch (eventId)
                {
                    case IocEventIdCodes.ServiceLocator.ContainerSingleAssemblySelfRegistrationsInvokedBefore:
                        loggingUtils.LoggingUtilitiesForIocIocServiceLocator.GetMessageContainerSingleAssemblySelfRegistrationInvokedBefore(
                           IocContainer.InternalRootContainer,
                           assemblySelfRegistration,
                           out message);
                        break;

                    case IocEventIdCodes.ServiceLocator.ContainerSingleAssemblySelfRegistrationInvokedAfter:
                        loggingUtils.LoggingUtilitiesForIocIocServiceLocator.GetMessageContainerSingleAssemblySelfRegistrationInvokedAfter(
                           IocContainer.InternalRootContainer,
                           assemblySelfRegistration,
                           out message);
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
    }
}
