namespace Landorphan.Abstractions.IO
{
   using System;
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
      public FileStream Open(String path, FileMode mode)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.Open(path, mode);
      }

      /// <inheritdoc/>
      public FileStream Open(String path, FileMode mode, FileAccess access)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.Open(path, mode, access);
      }

      /// <inheritdoc/>
      public FileStream Open(String path, FileMode mode, FileAccess access, FileShare share)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.Open(path, mode, access);
      }

      /// <inheritdoc/>
      public FileStream OpenRead(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.OpenRead(path);
      }

      /// <inheritdoc/>
      public StreamReader OpenText(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.OpenText(path);
      }

      /// <inheritdoc/>
      public IImmutableList<Byte> ReadAllBytes(String path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.ReadAllBytes(path);
      }

      /// <inheritdoc/>
      public IImmutableList<String> ReadAllLines(String path, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.ReadAllLines(path, encoding);
      }

      /// <inheritdoc/>
      public String ReadAllText(String path, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.ReadAllText(path, encoding);
      }

      /// <inheritdoc/>
      public IEnumerable<String> ReadLines(String path, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.ReadLines(path, encoding);
      }
   }
}
