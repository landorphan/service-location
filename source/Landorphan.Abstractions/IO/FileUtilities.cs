namespace Landorphan.Abstractions.IO
{
    using System;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Abstractions.IO.Internal;
    using Landorphan.Ioc.ServiceLocation;

    /// <summary>
   /// Default implementation of <see cref="IFileUtilities"/>.
   /// </summary>
   public sealed class FileUtilities : IFileUtilities
   {
       /// <inheritdoc/>
      public DateTimeOffset MaximumFileTimeAsDateTimeOffset
      {
         get
         {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.MaximumFileTimeAsDateTimeOffset;
         }
      }

       /// <inheritdoc/>
      public long MaximumPrecisionFileSystemTicks
      {
         get
         {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.MaximumPrecisionFileSystemTicks;
         }
      }

       /// <inheritdoc/>
      public DateTimeOffset MinimumFileTimeAsDateTimeOffset
      {
         get
         {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.MinimumFileTimeAsDateTimeOffset;
         }
      }

       /// <inheritdoc/>
      public string CreateFile(string path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.CreateFile(path);
      }

       /// <inheritdoc/>
      public string CreateTemporaryFile()
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.CreateTemporaryFile();
      }

       /// <inheritdoc/>
      public string CreateText(string path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.CreateText(path);
      }

       /// <inheritdoc/>
      public void DeleteFile(string path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.DeleteFile(path);
      }

       /// <inheritdoc/>
      public bool FileExists(string path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.FileExists(path);
      }

       /// <inheritdoc/>
      public DateTimeOffset GetCreationTime(string path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.GetCreationTime(path);
      }

       /// <inheritdoc/>
      public DateTimeOffset GetLastAccessTime(string path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.GetLastAccessTime(path);
      }

       /// <inheritdoc/>
      public DateTimeOffset GetLastWriteTime(string path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.GetLastWriteTime(path);
      }

       /// <inheritdoc/>
      public string GetRandomFileName()
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.GetRandomFileName();
      }

       /*  REMOVE BECAUSE IT IS UNRELIABLE, ESPECIALLY ON LINUX
       /// <inheritdoc/>
       public void SetCreationTime(String path, DateTimeOffset creationTime)
       {
          var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
          fileInternalMapping.SetCreationTime(path, creationTime);
       }
       */

       /// <inheritdoc/>
      public void SetLastAccessTime(string path, DateTimeOffset lastAccessTime)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.SetLastAccessTime(path, lastAccessTime);
      }

       /// <inheritdoc/>
      public void SetLastWriteTime(string path, DateTimeOffset lastWriteTime)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.SetLastWriteTime(path, lastWriteTime);
      }
   }
}
