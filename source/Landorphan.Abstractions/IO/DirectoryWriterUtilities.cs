namespace Landorphan.Abstractions.IO
{
   using System;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Default implementation of <see cref="IDirectoryWriterUtilities"/>.
   /// </summary>
   public sealed class DirectoryWriterUtilities : IDirectoryWriterUtilities
   {
      /// <inheritdoc/>
      public void Copy(String sourceDirName, String destDirName)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         directoryInternalMapping.Copy(sourceDirName, destDirName);
      }

      /// <inheritdoc/>
      public void Move(String sourceDirName, String destDirName)
      {
         var directoryInternalMapping = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         directoryInternalMapping.Move(sourceDirName, destDirName);
      }
   }
}
