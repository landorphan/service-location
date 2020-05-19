namespace Landorphan.Ioc.Example.ConsoleApp
{
    using System.Collections.Generic;

    public interface ICurrencyConverter
    {
        decimal Convert(string currency, decimal amount, string baseCurrency);
        IEnumerable<string> GetSupportedCurrencies();
    }
}
