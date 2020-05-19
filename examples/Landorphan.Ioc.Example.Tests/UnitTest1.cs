namespace Landorphan.Ioc.Example.Tests
{
    using FluentAssertions;
    using Landorphan.Ioc.Example.ConsoleApp;
    using Landorphan.Ioc.Example.Service;
    using Landorphan.Ioc.ServiceLocation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void The_Service_Should_Convert_Rates_Properly()
        {
            var exchangeRateService = IocServiceLocator.Resolve<ICurrencyExchangeRates>();
            var zzzRate = exchangeRateService.GetRate("USD", "ZZZ");
            var expected = zzzRate * 101;
            var converter = IocServiceLocator.Resolve<ICurrencyConverter>();
            var actual = converter.Convert("ZZZ", 101, "USD");
            zzzRate.Should().Be(999.999M);
            actual.Should().Be(expected);
        }
    }
}
