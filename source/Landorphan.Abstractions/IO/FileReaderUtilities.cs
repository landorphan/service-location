namespace Landorphan.Abstractions.IO
{
   using System;
   using System.Collections.Immutable;
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
   }
}
