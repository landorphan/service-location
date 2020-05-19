namespace Landorphan.Abstractions.IO.Interfaces
{
    /// <summary>
    /// Factory for instantiating <see cref="IFileReaderUtilities"/> instances.
    /// </summary>
    public interface IFileReaderUtilitiesFactory
    {
        /// <summary>
        /// Creates a <see cref="IFileReaderUtilities"/> instance.
        /// </summary>
        /// <returns>
        /// A non-null <see cref="IFileReaderUtilities"/> instance.
        /// </returns>
        IFileReaderUtilities Create();
    }
}
