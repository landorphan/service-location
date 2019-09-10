namespace Landorphan.Ioc.Example.Service
{
   using Landorphan.Ioc.Example.ConsoleApp;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using RestSharp;

   public class AssemblySelfRegistration : IAssemblySelfRegistration
   {
      public void RegisterServiceInstances(IIocContainerRegistrar registrar)
      {
         IocServiceLocator.AmbientContainer.Registrar.RegisterImplementation<ICurrencyConverter, CurrencyConverter>();
      }
   }
}
