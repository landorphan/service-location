namespace Landorphan.Abstractions.IO
{
   using System;
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
      public IImmutableSet<String> EnumerateDirectories(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.EnumerateDirectories(path);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateDirectories(String path, String searchPattern)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.EnumerateDirectories(path, searchPattern);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateDirectories(String path, String searchPattern, SearchOption searchOption)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.EnumerateDirectories(path, searchPattern, searchOption);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFiles(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.EnumerateFiles(path);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFiles(String path, String searchPattern)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.EnumerateFiles(path, searchPattern);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFiles(String path, String searchPattern, SearchOption searchOption)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.EnumerateFiles(path, searchPattern, searchOption);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFileSystemEntries(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.EnumerateFileSystemEntries(path);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFileSystemEntries(String path, String searchPattern)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.EnumerateFileSystemEntries(path, searchPattern);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFileSystemEntries(String path, String searchPattern, SearchOption searchOption)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.EnumerateFileSystemEntries(path, searchPattern, searchOption);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetDirectories(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetDirectories(path);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetDirectories(String path, String searchPattern)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetDirectories(path, searchPattern);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetDirectories(String path, String searchPattern, SearchOption searchOption)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetDirectories(path, searchPattern, searchOption);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFiles(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetFiles(path);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFiles(String path, String searchPattern)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetFiles(path, searchPattern);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFiles(String path, String searchPattern, SearchOption searchOption)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetFiles(path, searchPattern, searchOption);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFileSystemEntries(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetFileSystemEntries(path);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFileSystemEntries(String path, String searchPattern)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetFileSystemEntries(path, searchPattern);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFileSystemEntries(String path, String searchPattern, SearchOption searchOption)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetFileSystemEntries(path, searchPattern, searchOption);
      }
   }
}
