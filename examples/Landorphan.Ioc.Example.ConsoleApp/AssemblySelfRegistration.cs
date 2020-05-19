namespace Landorphan.Ioc.Example.ConsoleApp
{
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    public class AssemblySelfRegistration : IAssemblySelfRegistration
    {
        public void RegisterServiceInstances(IIocContainerRegistrar registrar)
        {
            IocServiceLocator.AmbientContainer.Registrar.RegisterImplementation<ICurrencyConverter, CurrencyConverter>();
        }
    }
}
