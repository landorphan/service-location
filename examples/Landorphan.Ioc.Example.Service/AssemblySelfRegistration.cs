namespace Landorphan.Ioc.Example.Service
{
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using RestSharp;

   public class AssemblySelfRegistration : IAssemblySelfRegistration
   {
      public void RegisterServiceInstances(IIocContainerRegistrar registrar)
      {
         registrar.RegisterInstance<IRestClient>(new RestClient(@"https://api.exchangeratesapi.io/latest"));
         registrar.RegisterImplementation<IRestRequestFactory, RestRequestFactory>();
         registrar.RegisterImplementation<ICurrencyExchangeRates, CurrencyExchangeRates>();
      }
   }
}
