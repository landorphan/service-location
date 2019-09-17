namespace Landorphan.Abstractions.IO
{
   using System;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Default implementation of <see cref="IDirectoryUtilities"/>.
   /// </summary>
   public sealed class DirectoryUtilities : IDirectoryUtilities
   {
      /// <inheritdoc/>
      public DateTimeOffset MaximumFileTimeAsDateTimeOffset
      {
         get
         {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.MaximumFileTimeAsDateTimeOffset;
         }
      }

      /// <inheritdoc/>
      public Int64 MaximumPrecisionFileSystemTicks
      {
         get
         {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.MaximumPrecisionFileSystemTicks;
         }
      }

      /// <inheritdoc/>
      public DateTimeOffset MinimumFileTimeAsDateTimeOffset
      {
         get
         {
            var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
            return directoryInternalMapping.MinimumFileTimeAsDateTimeOffset;
         }
      }

      /// <inheritdoc/>
      public String CreateDirectory(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.CreateDirectory(path);
      }

      /// <inheritdoc/>
      public void DeleteEmpty(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         directoryInternalMapping.DeleteEmpty(path);
      }

      /// <inheritdoc/>
      public void DeleteRecursively(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         directoryInternalMapping.DeleteRecursively(path);
      }

      /// <inheritdoc/>
      public Boolean DirectoryExists(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.DirectoryExists(path);
      }

      /// <inheritdoc/>
      public DateTimeOffset GetCreationTime(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetCreationTime(path);
      }

      /// <inheritdoc/>
      public String GetCurrentDirectory()
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetCurrentDirectory();
      }

      /// <inheritdoc/>
      public DateTimeOffset GetLastAccessTime(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetLastAccessTime(path);
      }

      /// <inheritdoc/>
      public DateTimeOffset GetLastWriteTime(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetLastWriteTime(path);
      }

      /// <inheritdoc/>
      public String GetRandomDirectoryName()
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetRandomDirectoryName();
      }

      /// <inheritdoc/>
      public String GetTemporaryDirectoryPath()
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         return directoryInternalMapping.GetTemporaryDirectoryPath();
      }

      /*  REMOVE BECAUSE IT IS UNRELIABLE, ESPECIALLY ON LINUX
      /// <inheritdoc/>
      public void SetCreationTime(String path, DateTimeOffset creationTime)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         directoryInternalMapping.SetCreationTime(path, creationTime);
      }
      */

      /// <inheritdoc/>
      public void SetCurrentDirectory(String path)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         directoryInternalMapping.SetCurrentDirectory(path);
      }

      /// <inheritdoc/>
      public void SetLastAccessTime(String path, DateTimeOffset lastAccessTime)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         directoryInternalMapping.SetLastAccessTime(path, lastAccessTime);
      }

      /// <inheritdoc/>
      public void SetLastWriteTime(String path, DateTimeOffset lastWriteTime)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         directoryInternalMapping.SetLastWriteTime(path, lastWriteTime);
      }
   }
}
