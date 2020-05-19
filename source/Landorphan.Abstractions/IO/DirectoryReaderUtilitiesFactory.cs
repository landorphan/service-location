namespace Landorphan.Abstractions.IO
{
    using Landorphan.Abstractions.IO.Interfaces;

    /// <summary>
    /// Factory for instantiating <see cref="IDirectoryUtilities"/> instances.
    /// </summary>
    public sealed class DirectoryReaderUtilitiesFactory : IDirectoryReaderUtilitiesFactory
    {
        /// <inheritdoc/>
        public IDirectoryReaderUtilities Create()
        {
            return new DirectoryReaderUtilities();
        }
    }
}
