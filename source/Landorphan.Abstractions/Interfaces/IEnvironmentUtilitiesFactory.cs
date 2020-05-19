namespace Landorphan.Abstractions.Interfaces
{
    /// <summary>
    /// Factory for instantiating <see cref="IEnvironmentUtilities"/> instances.
    /// </summary>
    public interface IEnvironmentUtilitiesFactory
    {
        /// <summary>
        /// Creates a <see cref="IEnvironmentUtilities"/> instance.
        /// </summary>
        /// <returns>
        /// A non-null <see cref="IEnvironmentUtilities"/> instance.
        /// </returns>
        IEnvironmentUtilities Create();
    }
}
