namespace Landorphan.Abstractions.AssemblySelfRegistration
{
    using System.Diagnostics.CodeAnalysis;
    using Landorphan.Abstractions.Interfaces;
    using Landorphan.Abstractions.IO;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Abstractions.IO.Internal;
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

            //
            // Landorphan.Abstractions.IO
            //

            // Directory
            registrar.RegisterInstance<IDirectoryReaderUtilitiesFactory>(new DirectoryReaderUtilitiesFactory());
            registrar.RegisterInstance(registrar.Resolver.Resolve<IDirectoryReaderUtilitiesFactory>().Create());

            registrar.RegisterInstance<IDirectoryUtilitiesFactory>(new DirectoryUtilitiesFactory());
            registrar.RegisterInstance(registrar.Resolver.Resolve<IDirectoryUtilitiesFactory>().Create());

            registrar.RegisterInstance<IDirectoryWriterUtilitiesFactory>(new DirectoryWriterUtilitiesFactory());
            registrar.RegisterInstance(registrar.Resolver.Resolve<IDirectoryWriterUtilitiesFactory>().Create());

            // File
            registrar.RegisterInstance<IFileReaderUtilitiesFactory>(new FileReaderUtilitiesFactory());
            registrar.RegisterInstance(registrar.Resolver.Resolve<IFileReaderUtilitiesFactory>().Create());

            registrar.RegisterInstance<IFileUtilitiesFactory>(new FileUtilitiesFactory());
            registrar.RegisterInstance(registrar.Resolver.Resolve<IFileUtilitiesFactory>().Create());

            registrar.RegisterInstance<IFileWriterUtilitiesFactory>(new FileWriterUtilitiesFactory());
            registrar.RegisterInstance(registrar.Resolver.Resolve<IFileWriterUtilitiesFactory>().Create());

            // Path
            registrar.RegisterInstance<IPathUtilitiesFactory>(new PathUtilitiesFactory());
            registrar.RegisterInstance(registrar.Resolver.Resolve<IPathUtilitiesFactory>().Create());

            //
            // Landorphan.Abstractions.IO.Internal
            //
            registrar.RegisterInstance<IDirectoryInternalMapping>(new DirectoryInternalMapping());
            registrar.RegisterInstance<IFileInternalMapping>(new FileInternalMapping());
        }
    }
}
