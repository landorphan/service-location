namespace Landorphan.Ioc.ServiceLocation.Testability
{
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   /// <summary>
   /// Registers the services of this assembly with the <see cref="IocServiceLocator"/>.
   /// </summary>
   /// <remarks>
   /// "Auto-registers" these interfaces (currently only <see cref="ITestMockingService"/>) in the root container.
   /// </remarks>
   public sealed class TestabilityServiceLocationSelfRegistration : IAssemblySelfRegistration
   {
      /// <inheritdoc/>
      public void RegisterServiceInstances(IIocContainerRegistrar registrar)
      {
         registrar.ArgumentNotNull(nameof(registrar));

         if (!registrar.IsRegistered<ITestMockingService>())
         {
            registrar.RegisterInstance<ITestMockingService>(new TestMockingService());
         }
      }
   }
}
