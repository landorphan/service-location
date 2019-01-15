namespace Landorphan.TestUtilities.WithIoc
{
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities.WithIoc.Internal;

   /// <summary>
   /// Registers services with the IoC for use by this assembly.
   /// </summary>
   /// <remarks>
   /// "Auto-registers" these interfaces (currently only <see cref="ITestMockingService"/>) in the root container.
   /// </remarks>
   public sealed class TestUtilitiesServiceLocationSelfRegistration : IAssemblySelfRegistration
   {
      /// <inheritdoc/>
      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
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
