namespace Landorphan.Abstractions.IO.Interfaces
{
    /// <summary>
    /// Factory for instantiating <see cref="IDirectoryUtilities"/> instances.
    /// </summary>
    public interface IDirectoryUtilitiesFactory
    {
        /// <summary>
        /// Creates a <see cref="IDirectoryUtilities"/> instance.
        /// </summary>
        /// <returns>
        /// A non-null <see cref="IDirectoryUtilities"/> instance.
        /// </returns>
        IDirectoryUtilities Create();
    }
}
