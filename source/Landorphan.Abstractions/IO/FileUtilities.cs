namespace Landorphan.Abstractions.IO
{
   using System;
   using System.IO;
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
      public DateTimeOffset MinimumFileTimeAsDateTimeOffset
      {
         get
         {
            var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
            return fileInternalMapping.MinimumFileTimeAsDateTimeOffset;
         }
      }

      /// <inheritdoc/>
      public String CreateFile(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.CreateFile(path);
      }

      /// <inheritdoc/>
      public String CreateTemporaryFile()
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.CreateTemporaryFile();
      }

      /// <inheritdoc/>
      public String CreateText(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.CreateText(path);
      }

      /// <inheritdoc/>
      public void DeleteFile(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.DeleteFile(path);
      }

      /// <inheritdoc/>
      public Boolean FileExists(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.FileExists(path);
      }

      /// <inheritdoc/>
      public FileAttributes GetAttributes(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.GetAttributes(path);
      }

      /// <inheritdoc/>
      public DateTimeOffset GetCreationTime(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.GetCreationTime(path);
      }

      /// <inheritdoc/>
      public DateTimeOffset GetLastAccessTime(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.GetLastAccessTime(path);
      }

      /// <inheritdoc/>
      public DateTimeOffset GetLastWriteTime(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.GetLastWriteTime(path);
      }

      /// <inheritdoc/>
      public String GetRandomFileName()
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.GetRandomFileName();
      }

      /// <inheritdoc/>
      public void SetAttributes(String path, FileAttributes fileAttributes)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.SetAttributes(path, fileAttributes);
      }

      /// <inheritdoc/>
      public void SetCreationTime(String path, DateTimeOffset creationTime)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.SetCreationTime(path, creationTime);
      }

      /// <inheritdoc/>
      public void SetLastAccessTime(String path, DateTimeOffset lastAccessTime)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.SetLastAccessTime(path, lastAccessTime);
      }

      /// <inheritdoc/>
      public void SetLastWriteTime(String path, DateTimeOffset lastWriteTime)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.SetLastWriteTime(path, lastWriteTime);
      }
   }
}
