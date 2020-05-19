namespace Landorphan.Abstractions.IO
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Text;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Abstractions.IO.Internal;
    using Landorphan.Ioc.ServiceLocation;

    /// <summary>
    /// Default implementation of <see cref="IFileReaderUtilities"/>.
    /// </summary>
    public sealed class FileReaderUtilities : IFileReaderUtilities
    {
        /// <inheritdoc/>
        public FileStream Open(string path, FileMode mode)
        {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.Open(path, mode);
        }

        /// <inheritdoc/>
        public FileStream Open(string path, FileMode mode, FileAccess access)
        {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.Open(path, mode, access);
        }

        /// <inheritdoc/>
        public FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.Open(path, mode, access);
        }

        /// <inheritdoc/>
        public FileStream OpenRead(string path)
        {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.OpenRead(path);
        }

        /// <inheritdoc/>
        public StreamReader OpenText(string path)
        {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.OpenText(path);
        }

        /// <inheritdoc/>
        public IImmutableList<byte> ReadAllBytes(string path)
        {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.ReadAllBytes(path);
        }

        /// <inheritdoc/>
        public IImmutableList<string> ReadAllLines(string path, Encoding encoding)
        {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.ReadAllLines(path, encoding);
        }

        /// <inheritdoc/>
        public string ReadAllText(string path, Encoding encoding)
        {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.ReadAllText(path, encoding);
        }

        /// <inheritdoc/>
        public IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.ReadLines(path, encoding);
        }
    }
}
