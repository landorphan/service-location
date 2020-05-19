namespace Landorphan.Abstractions.IO.Interfaces
{
    /// <summary>
    /// Factory for instantiating <see cref="IDirectoryWriterUtilities"/> instances.
    /// </summary>
    public interface IDirectoryWriterUtilitiesFactory
    {
        /// <summary>
        /// Creates a <see cref="IDirectoryWriterUtilities"/> instance.
        /// </summary>
        /// <returns>
        /// A non-null <see cref="IDirectoryWriterUtilities"/> instance.
        /// </returns>
        IDirectoryWriterUtilities Create();
    }
}
