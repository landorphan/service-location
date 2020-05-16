namespace Landorphan.Ioc.Example.Service
{
    using System;
    using System.Collections.Generic;

    public interface ICurrencyExchangeRates
   {
       IEnumerable<string> GetCurrencyCodes();
       decimal GetRate(string baseCurrency, string exchangeCurrency);
       DateTime GetRateDate();
       IDictionary<string, decimal> GetRates(string baseCurrency);
   }
}
