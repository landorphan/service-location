namespace Landorphan.Ioc.ServiceLocation.Logging.Microsoft.Internal
{
    using System.Diagnostics.CodeAnalysis;
    using global::Microsoft.Extensions.Logging;
    using Landorphan.Common;
    using Landorphan.Ioc.Logging;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    [SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Called by Service Locator")]
    internal class AssemblySelfRegistration : IAssemblySelfRegistration
    {
        public void RegisterServiceInstances(IIocContainerRegistrar registrar)
        {
            registrar.ArgumentNotNull(nameof(registrar));
            registrar.RegisterImplementation<ILoggerFactory, LoggerFactory>();
            registrar.RegisterImplementation<IIocLoggerFactory, IocLoggerFactory>();
        }
    }
}
