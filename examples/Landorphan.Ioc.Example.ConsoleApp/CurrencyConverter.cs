namespace Landorphan.Ioc.Example.ConsoleApp
{
    using System.Collections.Generic;
    using Landorphan.Ioc.Example.Service;
    using Landorphan.Ioc.ServiceLocation;

    public class CurrencyConverter : ICurrencyConverter
    {
        public decimal Convert(string currency, decimal amount, string baseCurrency)
        {
            var rateService = IocServiceLocator.Resolve<ICurrencyExchangeRates>();
            var exchangeRate = rateService.GetRate(baseCurrency, currency);
            return amount * exchangeRate;
        }

        public IEnumerable<string> GetSupportedCurrencies()
        {
            var rateService = IocServiceLocator.Resolve<ICurrencyExchangeRates>();
            return rateService.GetCurrencyCodes();
        }
    }
}
