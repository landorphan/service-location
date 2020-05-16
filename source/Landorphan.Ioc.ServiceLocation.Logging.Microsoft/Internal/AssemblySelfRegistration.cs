namespace Landorphan.Ioc.ServiceLocation.Logging.Microsoft.Internal
{
    using global::Microsoft.Extensions.Logging;
    using Landorphan.Ioc.Logging;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

#pragma warning disable CA1812 // -- This is intentional for a registration clases.
    internal class AssemblySelfRegistration : IAssemblySelfRegistration
#pragma warning restore CA1812
    {
        public void RegisterServiceInstances(IIocContainerRegistrar registrar)
        {
            registrar.RegisterImplementation<ILoggerFactory, LoggerFactory>();
            registrar.RegisterImplementation<IIocLoggerFactory, IocLoggerFactory>();
        }
    }
}
