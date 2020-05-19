namespace Landorphan.Ioc.Example.Tests.Simulators
{
    using System;
    using System.Collections.Generic;
    using Landorphan.Ioc.Example.Service;

    public class CurrencyExchangeRateSimulator : ICurrencyExchangeRates
    {
        private readonly Dictionary<string, decimal> usdRates = new Dictionary<string, decimal>();

        public IEnumerable<string> GetCurrencyCodes()
        {
            return usdRates.Keys;
        }

        public decimal GetRate(string baseCurrency, string exchangeCurrency)
        {
            // Invert the base rate so that the conversion will work.
            var baseRate = 1 / GetUsdRate(baseCurrency);
            var exchangeRate = GetUsdRate(exchangeCurrency);
            return baseRate * exchangeRate;
        }

        public DateTime GetRateDate()
        {
            return DateTime.Now;
        }

        public IDictionary<string, decimal> GetRates(string baseCurrency)
        {
            var exchangeRates = new Dictionary<string, decimal>();
            foreach (var rate in usdRates)
            {
                var exchangeRate = GetRate(baseCurrency, rate.Key);
                exchangeRates.Add(rate.Key, exchangeRate);
            }

            return exchangeRates;
        }

        public void Clear()
        {
            usdRates.Clear();
        }

        public decimal GetUsdRate(string currency)
        {
            return usdRates[currency];
        }

        public void SetUsdRate(string currency, decimal ammount)
        {
            usdRates[currency] = ammount;
        }
    }
}
