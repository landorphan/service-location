namespace Landorphan.Ioc.ServiceLocation.Internal
{
    using Landorphan.Ioc.Logging.Internal;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    internal class AssemblySelfRegistration : IAssemblySelfRegistration
    {
        public void RegisterServiceInstances(IIocContainerRegistrar registrar)
        {
            registrar.RegisterImplementation<IIocLoggerManager, IocLoggerManager>();
        }
    }
}
