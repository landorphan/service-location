namespace Landorphan.Abstractions.IO.Interfaces
{
    /// <summary>
    /// Factory for instantiating <see cref="IDirectoryReaderUtilities"/> instances.
    /// </summary>
    public interface IDirectoryReaderUtilitiesFactory
    {
        /// <summary>
        /// Creates a <see cref="IDirectoryReaderUtilities"/> instance.
        /// </summary>
        /// <returns>
        /// A non-null <see cref="IDirectoryReaderUtilities"/> instance.
        /// </returns>
        IDirectoryReaderUtilities Create();
    }
}
