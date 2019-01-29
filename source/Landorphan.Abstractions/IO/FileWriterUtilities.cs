namespace Landorphan.Abstractions.IO
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Text;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Default implementation of <see cref="IFileWriterUtilities"/>.
   /// </summary>
   public sealed class FileWriterUtilities : IFileWriterUtilities
   {
      /// <inheritdoc/>
      public void AppendAllLines(String path, IEnumerable<String> contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.AppendAllLines(path, contents, encoding);
      }

      /// <inheritdoc/>
      public void AppendAllText(String path, String contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.AppendAllText(path, contents, encoding);
      }

      /// <inheritdoc/>
      public void CopyNoOverwrite(String sourceFileName, String destFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.CopyNoOverwrite(sourceFileName, destFileName);
      }

      /// <inheritdoc/>
      public void CopyWithOverwrite(String sourceFileName, String destFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.CopyWithOverwrite(sourceFileName, destFileName);
      }

      /// <inheritdoc/>
      public void Move(String sourceFileName, String destFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.Move(sourceFileName, destFileName);
      }

      /// <inheritdoc/>
      public void ReplaceContentsNoBackup(String sourceFileName, String destinationFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
      }

      /// <inheritdoc/>
      public void ReplaceContentsNoBackupIgnoringMetadataErrors(String sourceFileName, String destinationFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
      }

      /// <inheritdoc/>
      public void ReplaceContentsWithBackup(String sourceFileName, String destinationFileName, String destinationBackupFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
      }

      /// <inheritdoc/>
      public void ReplaceContentsWithBackupIgnoringMetadataErrors(String sourceFileName, String destinationFileName, String destinationBackupFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
      }

      /// <inheritdoc/>
      public void WriteAllBytes(String path, Byte[] bytes)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllBytes(path, bytes);
      }

      /// <inheritdoc/>
      public void WriteAllBytes(String path, IImmutableList<Byte> bytes)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllBytes(path, bytes);
      }

      /// <inheritdoc/>
      public void WriteAllLines(String path, String[] contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllLines(path, contents, encoding);
      }

      /// <inheritdoc/>
      public void WriteAllLines(String path, IImmutableList<String> contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllLines(path, contents, encoding);
      }

      /// <inheritdoc/>
      public void WriteAllLines(String path, IEnumerable<String> contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllLines(path, contents, encoding);
      }

      /// <inheritdoc/>
      public void WriteAllText(String path, String contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllText(path, contents, encoding);
      }
   }
}
