using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Ioc.Example.ConsoleApp
{
   public interface ICurrencyConverter
   {
      decimal Convert(string currency, decimal amount, string baseCurrency);
      IEnumerable<string> GetSupportedCurrencies();
   }
}
