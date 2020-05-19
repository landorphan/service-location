namespace Landorphan.Abstractions.IO
{
    using System.Collections.Immutable;
    using System.IO;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Abstractions.IO.Internal;
    using Landorphan.Ioc.ServiceLocation;

    /// <summary>
    /// Default implementation of <see cref="IDirectoryReaderUtilities"/>.
    /// </summary>
    public sealed class DirectoryReaderUtilities : IDirectoryReaderUtilities
    {
        /// <inheritdoc/>
        public IImmutableSet<string> EnumerateDirectories(string path)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.EnumerateDirectories(path);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> EnumerateDirectories(string path, string searchPattern)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.EnumerateDirectories(path, searchPattern);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.EnumerateDirectories(path, searchPattern, searchOption);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> EnumerateFiles(string path)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.EnumerateFiles(path);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> EnumerateFiles(string path, string searchPattern)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.EnumerateFiles(path, searchPattern);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.EnumerateFiles(path, searchPattern, searchOption);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> EnumerateFileSystemEntries(string path)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.EnumerateFileSystemEntries(path);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.EnumerateFileSystemEntries(path, searchPattern);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.EnumerateFileSystemEntries(path, searchPattern, searchOption);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> GetDirectories(string path)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.GetDirectories(path);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> GetDirectories(string path, string searchPattern)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.GetDirectories(path, searchPattern);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.GetDirectories(path, searchPattern, searchOption);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> GetFiles(string path)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.GetFiles(path);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> GetFiles(string path, string searchPattern)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.GetFiles(path, searchPattern);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.GetFiles(path, searchPattern, searchOption);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> GetFileSystemEntries(string path)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.GetFileSystemEntries(path);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> GetFileSystemEntries(string path, string searchPattern)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.GetFileSystemEntries(path, searchPattern);
        }

        /// <inheritdoc/>
        public IImmutableSet<string> GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.GetFileSystemEntries(path, searchPattern, searchOption);
        }
    }
}
