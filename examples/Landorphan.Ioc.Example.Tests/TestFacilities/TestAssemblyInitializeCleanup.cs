namespace Landorphan.TestUtilities.MSTest.Tests.TestFacilities
{
   using System.Collections.Immutable;
   using System.Reflection;
   using Landorphan.Ioc.Example.ConsoleApp;
   using Landorphan.Ioc.Example.Service;
   using Landorphan.Ioc.Example.Tests.Simulators;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Testability;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public static class TestAssemblyInitializeCleanup
   {
      internal static IImmutableSet<Assembly> AssembliesUnderTest { get; private set; }

      /// <summary>
      /// Performs assembly level initialization.
      /// </summary>
      /// <remarks>
      /// Executes once, before any tests to be executed are run.
      /// </remarks>
      [AssemblyInitialize]
      public static void AssemblyInitialize(TestContext context)
      {
         // acquire assemblies under test
         var assemblies = ImmutableHashSet<Assembly>.Empty.ToBuilder();

         // Landorphan.TestUtilities.MSTest
         assemblies.Add(typeof(ArrangeActAssert).Assembly);


         var mockingService = IocServiceLocator.Instance.Resolve<ITestMockingService>();
         var sim = new CurrencyExchangeRateSimulator();
         sim.SetUsdRate("GBP", 0.8091634188M);
         sim.SetUsdRate("EUR", 0.9063717937M);
         sim.SetUsdRate("USD", 1);
         sim.SetUsdRate("JPY", 106.9609353757M);
         sim.SetUsdRate("ZZZ", 999.999M);
         mockingService.TestRunContainer.Registrar.RegisterInstance<ICurrencyExchangeRates>(sim);
         mockingService.TestRunContainer.Registrar.RegisterImplementation<IConsole, ConsoleSimulator>();
         mockingService.ApplyTestInstanceMockingOnTopOfTestRunMocking();

         AssembliesUnderTest = assemblies.ToImmutable();
      }

      /// <summary>
      /// Frees resources obtained by the test assembly.
      /// </summary>
      /// <remarks>
      /// Executes once, after all tests to be executed are run.
      /// </remarks>
      [AssemblyCleanup]
      public static void AssemblyCleanup()
      {
         // currently no resources to clean up.
      }
   }
}
