namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
    /// <summary>
    /// Represents the meta capacities shared by
    /// <see cref="IIocServiceLocator"/> in concert with
    /// and a <see cref="IIocServiceLocatorManager"/>,
    /// </summary>
    /// <remarks>
    /// These interfaces work in tandem in 1:1 relationships but with separate responsibilities:
    /// <see cref="IIocServiceLocator"/> represents the existence and basic state of the service locator,
    /// and <see cref="IIocServiceLocatorManager"/> represents the additional management capacities.
    /// </remarks>
    public interface IIocServiceLocatorMetaSharedCapacities
    {
        /// <summary>
        /// Gets the manager of the service locator.
        /// </summary>
        /// <value>
        /// The manager of the service locator.
        /// </value>
        /// <remarks>
        /// Used to monitor and manipulate the behavior of <see cref="IocServiceLocator"/>.
        /// </remarks>
        IIocServiceLocatorManager Manager { get; }

        /// <summary>
        /// Gets the service locator.
        /// </summary>
        /// <value>
        /// The service locator client interface.
        /// </value>
        /// <remarks>
        /// Used to resolve interfaces to implementations.
        /// </remarks>
        IIocServiceLocator ServiceLocator { get; }
    }
}
