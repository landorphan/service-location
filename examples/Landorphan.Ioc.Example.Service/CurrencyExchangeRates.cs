namespace Landorphan.Ioc.Example.Service
{
   using System;
   using System.Collections.Generic;
   using RestSharp;

   public class ExchangeRates
   {
      public ExchangeRates() { }
      public Dictionary<string, decimal> Rates { get; set; }
      public string Base { get; set; }
      public DateTime Date { get; set; }
   }

   public class CurrencyExchangeRates : ICurrencyExchangeRates
   {
      private const string DefaultBaseCurrency = "EUR";

      internal ExchangeRates GetExchangeRates(string baseCurrency)
      {
         var client = new RestClient(@"https://api.exchangeratesapi.io/latest");
         var request = new RestRequest(Method.GET);
         request.AddParameter("base", baseCurrency);
         var result = client.Execute<ExchangeRates>(request);
         return result.Data;
      }

      public IEnumerable<string> GetCurrencyCodes()
      {
         var rateData = GetExchangeRates(DefaultBaseCurrency);
         return rateData.Rates.Keys;
      }

      public decimal GetRate(string baseCurrency, string exchangeCurrency)
      {
         var rateData = GetExchangeRates(baseCurrency);
         return rateData.Rates[exchangeCurrency];
      }

      public IEnumerable<KeyValuePair<string, decimal>> GetRates(string baseCurrency)
      {
         var rateData = GetExchangeRates(baseCurrency);
         return rateData.Rates;
      }

      public DateTime GetRateDate()
      {
         var rateData = GetExchangeRates(DefaultBaseCurrency);
         return rateData.Date;
      }
   }
}
