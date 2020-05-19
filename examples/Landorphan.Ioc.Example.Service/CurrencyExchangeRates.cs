namespace Landorphan.Ioc.Example.Service
{
    using System;
    using System.Collections.Generic;
    using Landorphan.Ioc.ServiceLocation;
    using RestSharp;

    public class CurrencyExchangeRates : ICurrencyExchangeRates
    {
        private const string DefaultBaseCurrency = "EUR";

        public IEnumerable<string> GetCurrencyCodes()
        {
            var rateData = GetRates(DefaultBaseCurrency);
            return rateData.Keys;
        }

        public decimal GetRate(string baseCurrency, string exchangeCurrency)
        {
            var rateData = GetExchangeRates(baseCurrency);
            return rateData.Rates[exchangeCurrency];
        }

        public DateTime GetRateDate()
        {
            var rateData = GetExchangeRates(DefaultBaseCurrency);
            return rateData.Date;
        }

        public IDictionary<string, decimal> GetRates(string baseCurrency)
        {
            var rateData = GetExchangeRates(baseCurrency);
            if (!rateData.Rates.ContainsKey(baseCurrency))
            {
                rateData.Rates[baseCurrency] = 1;
            }

            return rateData.Rates;
        }

        internal ExchangeRates GetExchangeRates(string baseCurrency)
        {
            var client = IocServiceLocator.Resolve<IRestClient>();
            var request = new RestRequest(Method.GET);
            request.AddParameter("base", baseCurrency);
            var result = client.Execute<ExchangeRates>(request);
            return result.Data;
        }
    }
}
