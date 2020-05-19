namespace Landorphan.Ioc.Example.Service
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA2227 // Collection properties should be read only -- This is needed for serialization to work properly for the test project.
#pragma warning disable S4004 // Collection properties should be readonly -- This is needed for serialization to work properly for the test project.

    public class ExchangeRates
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
