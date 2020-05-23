namespace Landorphan.Abstractions.AssemblySelfRegistration
{
    using System.Diagnostics.CodeAnalysis;
    using Landorphan.Abstractions.Interfaces;
    using Landorphan.Common;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    /// <summary>
    /// Registers the services of this assembly with the <see cref="IocServiceLocator"/>.
    /// </summary>
    [SuppressMessage("SonarLint.CodeSmell", "S1200: Classes should not be coupled to too many other classes (Single Responsibility Principle)")]
    [SuppressMessage("Microsoft.Maintainability", "CA1506: Avoid excessive class coupling")]
    public sealed class AbstractionsServiceLocationSelfRegistration : IAssemblySelfRegistration
    {
        /// <inheritdoc/>
        public void RegisterServiceInstances(IIocContainerRegistrar registrar)
        {
            registrar.ArgumentNotNull(nameof(registrar));

            //
            // Landorphan.Abstractions
            //
            IEnvironmentUtilitiesFactory environmentUtilitiesFactory = new EnvironmentUtilitiesFactory();
            registrar.RegisterInstance(environmentUtilitiesFactory);

            var environmentUtilities = environmentUtilitiesFactory.Create();
            registrar.RegisterInstance(environmentUtilities);
        }
    }
}
