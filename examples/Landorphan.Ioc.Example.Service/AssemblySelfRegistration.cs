namespace Landorphan.Ioc.Example.Service
{
    using System;
    using Landorphan.Common;
    using Landorphan.Ioc.ServiceLocation.Interfaces;
    using RestSharp;

#pragma warning disable S1075 // URIs should not be hardcoded -- Used strictly to create an example service not for production code.

    public class AssemblySelfRegistration : IAssemblySelfRegistration
    {
        public void RegisterServiceInstances(IIocContainerRegistrar registrar)
        {
            registrar.ArgumentNotNull(nameof(registrar));

            registrar.RegisterInstance<IRestClient>(new RestClient(new Uri(@"https://api.exchangeratesapi.io/latest")));
            registrar.RegisterImplementation<IRestRequestFactory, RestRequestFactory>();
            registrar.RegisterImplementation<ICurrencyExchangeRates, CurrencyExchangeRates>();
        }
    }
}
