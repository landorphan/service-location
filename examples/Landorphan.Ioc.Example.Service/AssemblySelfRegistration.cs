namespace Landorphan.Ioc.Example.Service
{
   using System;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using RestSharp;

   public class AssemblySelfRegistration : IAssemblySelfRegistration
   {
      public void RegisterServiceInstances(IIocContainerRegistrar registrar)
      {
         registrar.RegisterInstance<IRestClient>(new RestClient(new Uri(@"https://api.exchangeratesapi.io/latest")));
         registrar.RegisterImplementation<IRestRequestFactory, RestRequestFactory>();
         registrar.RegisterImplementation<ICurrencyExchangeRates, CurrencyExchangeRates>();
      }
   }
}
