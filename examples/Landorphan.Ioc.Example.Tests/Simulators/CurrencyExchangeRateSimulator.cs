using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Ioc.Example.Tests.Simulators
{
   using Landorphan.Ioc.Example.Service;

   class CurrencyExchangeRateSimulator : ICurrencyExchangeRates
   {
      private Dictionary<string, decimal> usdRates = new Dictionary<string, decimal>();

      public void SetUsdRate(string currency, decimal ammount)
      {
         usdRates[currency] = ammount;
      }

      public decimal GetUsdRate(string currency)
      {
         return usdRates[currency];
      }

      public IEnumerable<string> GetCurrencyCodes()
      {
         return usdRates.Keys;
      }

      public void Clear()
      {
         usdRates.Clear();
      }

      public decimal GetRate(string baseCurrency, string exchangeCurrency)
      {
         // Invert the base rate so that the conversion will work.
         var baseRate = 1 / GetUsdRate(baseCurrency);
         var exchangeRate = GetUsdRate(exchangeCurrency);
         return baseRate * exchangeRate;
      }

      public IDictionary<string, decimal> GetRates(string baseCurrency)
      {
         Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>();
         foreach (var rate in usdRates)
         {
            var exchangeRate = GetRate(baseCurrency, rate.Key);
            exchangeRates.Add(rate.Key, exchangeRate);
         }

         return exchangeRates;
      }

      public DateTime GetRateDate()
      {
         return DateTime.Now;
      }
   }
}
