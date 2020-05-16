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
   /// Default implementation of <see cref="IFileWriterUtilities"/>.
   /// </summary>
   public sealed class FileWriterUtilities : IFileWriterUtilities
   {
       /// <inheritdoc/>
      public void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.AppendAllLines(path, contents, encoding);
      }

       /// <inheritdoc/>
      public void AppendAllText(string path, string contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.AppendAllText(path, contents, encoding);
      }

       /// <inheritdoc/>
      public void CopyNoOverwrite(string sourceFileName, string destFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.CopyNoOverwrite(sourceFileName, destFileName);
      }

       /// <inheritdoc/>
      public void CopyWithOverwrite(string sourceFileName, string destFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.CopyWithOverwrite(sourceFileName, destFileName);
      }

       /// <inheritdoc/>
      public void Move(string sourceFileName, string destFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.Move(sourceFileName, destFileName);
      }

       /// <inheritdoc/>
      public FileStream OpenWrite(string path)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         return fileInternalMapping.OpenWrite(path);
      }

       /// <inheritdoc/>
      public void ReplaceContentsNoBackup(string sourceFileName, string destinationFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.ReplaceContentsNoBackup(sourceFileName, destinationFileName);
      }

       /// <inheritdoc/>
      public void ReplaceContentsNoBackupIgnoringMetadataErrors(string sourceFileName, string destinationFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);
      }

       /// <inheritdoc/>
      public void ReplaceContentsWithBackup(string sourceFileName, string destinationFileName, string destinationBackupFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.ReplaceContentsWithBackup(sourceFileName, destinationFileName, destinationBackupFileName);
      }

       /// <inheritdoc/>
      public void ReplaceContentsWithBackupIgnoringMetadataErrors(string sourceFileName, string destinationFileName, string destinationBackupFileName)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, destinationBackupFileName);
      }

       /// <inheritdoc/>
      public void WriteAllBytes(string path, byte[] bytes)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllBytes(path, bytes);
      }

       /// <inheritdoc/>
      public void WriteAllBytes(string path, IImmutableList<byte> bytes)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllBytes(path, bytes);
      }

       /// <inheritdoc/>
      public void WriteAllLines(string path, string[] contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllLines(path, contents, encoding);
      }

       /// <inheritdoc/>
      public void WriteAllLines(string path, IImmutableList<string> contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllLines(path, contents, encoding);
      }

       /// <inheritdoc/>
      public void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllLines(path, contents, encoding);
      }

       /// <inheritdoc/>
      public void WriteAllText(string path, string contents, Encoding encoding)
      {
         var fileInternalMapping = IocServiceLocator.Resolve<IFileInternalMapping>();
         fileInternalMapping.WriteAllText(path, contents, encoding);
      }
   }
}
