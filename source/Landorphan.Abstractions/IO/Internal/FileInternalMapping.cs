#define IO_PRECHECKS

namespace Landorphan.Abstractions.IO.Internal
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using System.Security;
   using System.Text;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.Resources;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Provides methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of
   /// <see cref="FileStream"/> objects.
   /// </summary>
   /// <remarks>
   /// Provides a near one-to-one mapping to <see cref="File"/> but in an object instance (as opposed to static) to support testability.
   /// </remarks>
   // [SuppressMessage("SonarLint.CodeSmell", "S2148: Underscores should be used to make large numbers readable")]
   internal sealed class FileInternalMapping : IFileInternalMapping
   {
      // Use IO_PRECHECKS to enable/disable non-canonical validation before the BCL call.  These are used to improve the exception messaging.

      ///<inheritdoc/>
      public DateTimeOffset MaximumFileTimeAsDateTimeOffset => FileTimeHelper.MaximumFileTimeAsDateTimeOffset;

      ///<inheritdoc/>
      public Int64 MaximumPrecisionFileSystemTicks => FileTimeHelper.MaximumPrecisionFileSystemTicks;

      ///<inheritdoc/>
      public DateTimeOffset MinimumFileTimeAsDateTimeOffset => FileTimeHelper.MinimumFileTimeAsDateTimeOffset;

      /// <inheritdoc/>
      public void AppendAllLines(String path, IEnumerable<String> contents, Encoding encoding)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         contents.ArgumentNotNull(nameof(contents));
         encoding.ArgumentNotNull(nameof(encoding));

         var dirUtilities = IocServiceLocator.Instance.Resolve<IDirectoryUtilities>();

#if IO_PRECHECKS
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            var msg = String.Format(CultureInfo.InvariantCulture, StringResources.CannotCreateFileDirectoryAlreadyExistsFmt, cleanedPath);
            throw new IOException(msg);
         }
#endif

         var pathUtilities = IocServiceLocator.Instance.Resolve<IPathUtilities>();
         var dirName = pathUtilities.GetParentPath(cleanedPath);

         if (!dirUtilities.DirectoryExists(dirName))
         {
            dirUtilities.CreateDirectory(dirName);
         }

         try
         {
            File.AppendAllLines(cleanedPath, contents, encoding);
         }
         catch (UnauthorizedAccessException uae)
         {
            // BCL sometimes reports the file name, sometimes does not.  Standardize this to:
            //    file name is reported when caller has write access to the parent folder, otherwise no file name is reported.
            var backslashFileName = pathUtilities.PathSeparatorCharacter + pathUtilities.GetFileName(cleanedPath);
            if (uae.Message.Contains(backslashFileName))
            {
               var msg = uae.Message.Replace(backslashFileName, String.Empty);
               throw new UnauthorizedAccessException(msg, uae);
            }

            throw;
         }
      }

      /// <inheritdoc/>
      public void AppendAllText(String path, String contents, Encoding encoding)
      {
         contents.ArgumentNotNull(nameof(contents));

         AppendAllLines(path, new[] {contents}, encoding);
      }

      /// <inheritdoc/>
      public void CopyNoOverwrite(String sourceFileName, String destFileName)
      {
         CopyImplementation(sourceFileName, destFileName, false);
      }

      /// <inheritdoc/>
      public void CopyWithOverwrite(String sourceFileName, String destFileName)
      {
         CopyImplementation(sourceFileName, destFileName, true);
      }

      /// <inheritdoc/>
      public String CreateFile(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var rv = pathUtilities.GetFullPath(cleanedPath);

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         var directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         if (directoryUtilities.DirectoryExists(cleanedPath))
         {
            var msg = String.Format(
               CultureInfo.InvariantCulture,
               "Cannot create the file '{0}' because a directory with the same name already exists.",
               cleanedPath);
            throw new IOException(msg);
         }
#endif
         var dir = pathUtilities.GetParentPath(cleanedPath).Trim();
         if (!String.IsNullOrEmpty(dir) && !directoryUtilities.DirectoryExists(dir))
         {
            directoryUtilities.CreateDirectory(dir);
         }

         const Int32 fourK = 4096;
         using (File.Create(cleanedPath, fourK, FileOptions.None))
         {
            // file created.
         }

         return rv;
      }

      /// <inheritdoc/>
      public String CreateTemporaryFile()
      {
         return Path.GetTempFileName();
      }

      /// <inheritdoc/>
      public String CreateText(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var rv = pathUtilities.GetFullPath(cleanedPath);

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         var directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         if (directoryUtilities.DirectoryExists(cleanedPath))
         {
            var msg = String.Format(
               CultureInfo.InvariantCulture,
               "Cannot create the file '{0}' because a directory with the same name already exists.",
               cleanedPath);
            throw new IOException(msg);
         }
#endif
         var dir = pathUtilities.GetParentPath(cleanedPath).Trim();
         if (!String.IsNullOrEmpty(dir) && !directoryUtilities.DirectoryExists(dir))
         {
            directoryUtilities.CreateDirectory(dir);
         }

         using (File.CreateText(cleanedPath))
         {
            // file created.
         }

         return rv;
      }

      /// <inheritdoc/>
      public void DeleteFile(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         var dir = pathUtilities.GetParentPath(cleanedPath);

         // the directory containing the file does not exist.
         // Historical bug here.  No directory was returning when deleting a file in the present working directory.
         // the revised condition fixes the bug.
         if (!String.IsNullOrEmpty(dir) && !dirUtilities.DirectoryExists(dir))
         {
            return;
         }

         // the path is a directory, not a file
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            throw new IOException(String.Format(CultureInfo.InvariantCulture, StringResources.FileNameInvalidMatchesDirectoryNameFmt, cleanedPath));
         }
#endif

         File.Delete(cleanedPath);
      }

      /// <inheritdoc/>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      public Boolean FileExists(String path)
      {
         try
         {
            var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
            return File.Exists(cleanedPath);
         }
         catch (ArgumentException)
         {
            // ignore exception
         }
         catch (NotSupportedException)
         {
            // ignore exception
         }
         catch (SecurityException)
         {
            // ignore exception
         }
         catch (PathTooLongException)
         {
            // ignore exception
         }
         catch (DirectoryNotFoundException)
         {
            // ignore exception
         }
         catch (FileNotFoundException)
         {
            // ignore exception
         }
         catch (IOException)
         {
            // ignore exception
         }
         catch (UnauthorizedAccessException)
         {
            // ignore exception
         }

         return false;
      }

      /// <inheritdoc/>
      public DateTimeOffset GetCreationTime(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!FileExists(path))
         {
            ThrowFileNotFoundException(cleanedPath, nameof(path));
         }
#endif

         var rv = new DateTimeOffset(File.GetCreationTime(cleanedPath).ToUtc());
         return rv;
      }

      /// <inheritdoc/>
      public DateTimeOffset GetLastAccessTime(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!FileExists(cleanedPath))
         {
            ThrowFileNotFoundException(cleanedPath, nameof(path));
         }
#endif

         var rv = new DateTimeOffset(File.GetLastAccessTime(cleanedPath).ToUtc());
         return rv;
      }

      /// <inheritdoc/>
      public DateTimeOffset GetLastWriteTime(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!FileExists(cleanedPath))
         {
            ThrowFileNotFoundException(cleanedPath, nameof(path));
         }
#endif

         var rv = new DateTimeOffset(File.GetLastWriteTime(cleanedPath).ToUtc());
         return rv;
      }

      /// <inheritdoc/>
      public String GetRandomFileName()
      {
         return Path.GetRandomFileName();
      }

      /// <inheritdoc/>
      public void Move(String sourceFileName, String destFileName)
      {
         var cleanedSourceFileName = IOStringUtilities.ValidateCanonicalPath(sourceFileName, "sourceFileName");
         var cleanedDestFileName = IOStringUtilities.ValidateCanonicalPath(destFileName, "destFileName");

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedSourceFileName, "sourceFileName");
         ThrowIfOnUnmappedDrive(cleanedDestFileName, "destFileName");

         if (!FileExists(cleanedSourceFileName))
         {
            ThrowFileNotFoundException(cleanedSourceFileName, "sourceFileName");
         }

         var directoryUtilities = IocServiceLocator.Instance.Resolve<IDirectoryUtilities>();
         if (directoryUtilities.DirectoryExists(cleanedDestFileName))
         {
            throw new IOException(
               String.Format(CultureInfo.InvariantCulture, "Cannot move to destination file '{0}' because it is a directory.", cleanedDestFileName));
         }

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var src = pathUtilities.GetFullPath(cleanedSourceFileName).ToUpperInvariant();
         var dst = pathUtilities.GetFullPath(cleanedDestFileName).ToUpperInvariant();
         if (String.Equals(src, dst, StringComparison.Ordinal))
         {
            throw new IOException(
               String.Format(
                  CultureInfo.InvariantCulture,
                  "Cannot move to destination file '{0}' because the source file name and destination file name are the same.",
                  cleanedDestFileName));
         }

         if (FileExists(cleanedDestFileName))
         {
            throw new IOException(
               String.Format(
                  CultureInfo.InvariantCulture,
                  "Cannot move to destination file '{0}' because the file already exists.",
                  cleanedDestFileName));
         }
#endif

         // BCL Implementation notes.
         // TODO: consider improving messaging to include the denied path.
         // Unlike other methods, when this method throws UnauthorizedAccessException, it only provides: "Access to the path is denied."
         File.Move(cleanedSourceFileName, cleanedDestFileName);
      }

      /// <inheritdoc/>
      public FileStream Open(String path, FileMode mode)
      {
         return Open(path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
      }

      /// <inheritdoc/>
      public FileStream Open(String path, FileMode mode, FileAccess access)
      {
         return Open(path, mode, access, FileShare.None);
      }

      /// <inheritdoc/>
      public FileStream Open(String path, FileMode mode, FileAccess access, FileShare share)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         mode.ArgumentMustBeValidEnumValue(nameof(mode));
         access.ArgumentMustBeValidFlagsEnumValue(nameof(access));
         share.ArgumentMustBeValidFlagsEnumValue(nameof(share));

#if IO_PRECHECKS
         var dirUtilities = IocServiceLocator.Instance.Resolve<IDirectoryUtilities>();
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            if (mode == FileMode.Create)
            {
               var msg = String.Format(CultureInfo.InvariantCulture, StringResources.CannotCreateFileDirectoryAlreadyExistsFmt, cleanedPath);
               throw new IOException(msg);
            }
            else
            {
               var msg = String.Format(CultureInfo.InvariantCulture, StringResources.CannotOpenFileDirectoryAlreadyExistsFmt, cleanedPath);
               throw new IOException(msg);
            }
         }
#endif
         var rv = File.Open(cleanedPath, mode, access, share);
         return rv;
      }

      /// <inheritdoc/>
      public FileStream OpenRead(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         var dirUtilities = IocServiceLocator.Instance.Resolve<IDirectoryUtilities>();
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            var msg = String.Format(CultureInfo.InvariantCulture, StringResources.CannotOpenFileDirectoryAlreadyExistsFmt, cleanedPath);
            throw new IOException(msg);
         }
#endif

         var rv = File.OpenRead(cleanedPath);
         return rv;
      }

      /// <inheritdoc/>
      public StreamReader OpenText(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         var dirUtilities = IocServiceLocator.Instance.Resolve<IDirectoryUtilities>();
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            var msg = String.Format(CultureInfo.InvariantCulture, StringResources.CannotOpenFileDirectoryAlreadyExistsFmt, cleanedPath);
            throw new IOException(msg);
         }
#endif
         var rv = File.OpenText(cleanedPath);
         return rv;
      }

      /// <inheritdoc/>
      public FileStream OpenWrite(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
#if IO_PRECHECKS
         var dirUtilities = IocServiceLocator.Instance.Resolve<IDirectoryUtilities>();
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            var msg = String.Format(CultureInfo.InvariantCulture, StringResources.CannotOpenFileDirectoryAlreadyExistsFmt, cleanedPath);
            throw new IOException(msg);
         }
#endif
         var rv = File.OpenWrite(cleanedPath);
         return rv;
      }

      /// <inheritdoc/>
      public IImmutableList<Byte> ReadAllBytes(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();

         // the path is a directory, not a file
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            throw new IOException(
               String.Format(CultureInfo.InvariantCulture, StringResources.FileNameInvalidMatchesDirectoryNameFmt, cleanedPath));
         }
#endif

         var bytes = File.ReadAllBytes(cleanedPath);
         var builder = ImmutableList<Byte>.Empty.ToBuilder();
         foreach (var b in bytes)
         {
            builder.Add(b);
         }

         return builder.ToImmutable();
      }

      /// <inheritdoc/>
      public IImmutableList<String> ReadAllLines(String path, Encoding encoding)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();

         // the path is a directory, not a file
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            throw new IOException(
               String.Format(CultureInfo.InvariantCulture, StringResources.FileNameInvalidMatchesDirectoryNameFmt, cleanedPath));
         }
#endif

         var lines = File.ReadAllLines(cleanedPath, encoding);
         var builder = ImmutableList<String>.Empty.ToBuilder();
         foreach (var line in lines)
         {
            builder.Add(line);
         }

         return builder.ToImmutable();
      }

      /// <inheritdoc/>
      public String ReadAllText(String path, Encoding encoding)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();

         // the path is a directory, not a file
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            throw new IOException(
               String.Format(CultureInfo.InvariantCulture, StringResources.FileNameInvalidMatchesDirectoryNameFmt, cleanedPath));
         }
#endif

         return File.ReadAllText(cleanedPath, encoding);
      }

      /// <inheritdoc/>
      public IEnumerable<String> ReadLines(String path)
      {
         return ReadLines(path, Encoding.UTF8);
      }

      /// <inheritdoc/>
      public IEnumerable<String> ReadLines(String path, Encoding encoding)
      {
         // .Net Standard 2.0  File.ReadLines has at least 2 bugs
         // It leaks a file handle
         // It does not support multiple enumerations of the results
         //
         // This is a rewrite

         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         encoding.ArgumentNotNull(nameof(encoding));
#if IO_PRECHECKS
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var dirUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         var dir = pathUtilities.GetParentPath(cleanedPath);

         // the directory containing the file does not exist.
         if (!String.IsNullOrEmpty(dir) && !dirUtilities.DirectoryExists(dir))
         {
            ThrowDirectoryNotFoundException(dir, nameof(path));
         }

         // the path is a directory, not a file
         if (dirUtilities.DirectoryExists(cleanedPath))
         {
            throw new IOException(
               String.Format(CultureInfo.InvariantCulture, StringResources.FileNameInvalidMatchesDirectoryNameFmt, cleanedPath));
         }

         if (!FileExists(cleanedPath))
         {
            throw new FileNotFoundException(null, cleanedPath);
         }
#endif
         var builder = ImmutableList<String>.Empty.ToBuilder();
         using (var sr = new StreamReader(cleanedPath, encoding))
         {
            var line = sr.ReadLine();
            while (line != null)
            {
               builder.Add(line);
               line = sr.ReadLine();
            }
         }

         return builder.ToImmutable();
      }

      /// <inheritdoc/>
      public void ReplaceContentsNoBackup(String sourceFileName, String destinationFileName)
      {
         ReplaceContentsImplementation(sourceFileName, destinationFileName, null, false);
      }

      /// <inheritdoc/>
      public void ReplaceContentsNoBackupIgnoringMetadataErrors(String sourceFileName, String destinationFileName)
      {
         ReplaceContentsImplementation(sourceFileName, destinationFileName, null, true);
      }

      /// <inheritdoc/>
      public void ReplaceContentsWithBackup(String sourceFileName, String destinationFileName, String destinationBackupFileName)
      {
         destinationBackupFileName.ArgumentNotNull(nameof(destinationBackupFileName));
         ReplaceContentsImplementation(sourceFileName, destinationFileName, destinationBackupFileName, false);
      }

      /// <inheritdoc/>
      public void ReplaceContentsWithBackupIgnoringMetadataErrors(String sourceFileName, String destinationFileName, String destinationBackupFileName)
      {
         destinationBackupFileName.ArgumentNotNull(nameof(destinationBackupFileName));
         ReplaceContentsImplementation(sourceFileName, destinationFileName, destinationBackupFileName, true);
      }

      /// <inheritdoc/>
      public void SetCreationTime(String path, DateTimeOffset creationTime)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         creationTime = FileTimeHelper.TruncateTicksToFileSystemPrecision(creationTime);

         if (creationTime < MinimumFileTimeAsDateTimeOffset)
         {
            throw new ArgumentOutOfRangeException(
               nameof(creationTime),
               String.Format(
                  CultureInfo.InvariantCulture,
                  "The value must be greater than or equal to ({0} ticks).",
                  MinimumFileTimeAsDateTimeOffset.Ticks.ToString("N0", CultureInfo.InvariantCulture)));
         }

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!FileExists(cleanedPath))
         {
            ThrowFileNotFoundException(cleanedPath, nameof(path));
         }
#endif

         File.SetCreationTimeUtc(cleanedPath, creationTime.UtcDateTime);
      }

      /// <inheritdoc/>
      public void SetLastAccessTime(String path, DateTimeOffset lastAccessTime)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         lastAccessTime = FileTimeHelper.TruncateTicksToFileSystemPrecision(lastAccessTime);

         if (lastAccessTime < MinimumFileTimeAsDateTimeOffset)
         {
            throw new ArgumentOutOfRangeException(
               nameof(lastAccessTime),
               String.Format(
                  CultureInfo.InvariantCulture,
                  "The value must be greater than or equal to ({0} ticks).",
                  MinimumFileTimeAsDateTimeOffset.Ticks.ToString("N0", CultureInfo.InvariantCulture)));
         }

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!FileExists(cleanedPath))
         {
            ThrowFileNotFoundException(cleanedPath, nameof(path));
         }
#endif

         File.SetLastAccessTimeUtc(cleanedPath, lastAccessTime.UtcDateTime);
      }

      /// <inheritdoc/>
      public void SetLastWriteTime(String path, DateTimeOffset lastWriteTime)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         lastWriteTime = FileTimeHelper.TruncateTicksToFileSystemPrecision(lastWriteTime);

         if (lastWriteTime < MinimumFileTimeAsDateTimeOffset)
         {
            throw new ArgumentOutOfRangeException(
               nameof(lastWriteTime),
               String.Format(
                  CultureInfo.InvariantCulture,
                  "The value must be greater than or equal to ({0} ticks).",
                  MinimumFileTimeAsDateTimeOffset.Ticks.ToString("N0", CultureInfo.InvariantCulture)));
         }

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!FileExists(cleanedPath))
         {
            ThrowFileNotFoundException(cleanedPath, nameof(path));
         }
#endif

         File.SetLastWriteTimeUtc(cleanedPath, lastWriteTime.UtcDateTime);
      }

      /// <inheritdoc/>
      public void WriteAllBytes(String path, IImmutableList<Byte> bytes)
      {
         bytes.ArgumentNotNull(nameof(bytes));

         WriteAllBytes(path, bytes.ToArray());
      }

      /// <inheritdoc/>
      public void WriteAllBytes(String path, Byte[] bytes)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         bytes.ArgumentNotNull(nameof(bytes));

         var createdFile = false;
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!FileExists(cleanedPath))
         {
            // BCL will create the file but not the directories leading to it.
            CreateFile(cleanedPath);
            createdFile = true;
         }

#endif
         var exceptionThrown = false;
         try
         {
            File.WriteAllBytes(cleanedPath, bytes);
         }
         catch (Exception)
         {
            exceptionThrown = true;
            throw;
         }
         finally
         {
            if (exceptionThrown && createdFile)
            {
               DeleteFile(cleanedPath);
            }
         }
      }

      /// <inheritdoc/>
      public void WriteAllLines(String path, IImmutableList<String> contents, Encoding encoding)
      {
         contents.ArgumentNotNull(nameof(contents));

         WriteAllLines(path, contents.ToArray(), encoding);
      }

      /// <inheritdoc/>
      public void WriteAllLines(String path, IEnumerable<String> contents, Encoding encoding)
      {
         contents.ArgumentNotNull(nameof(contents));

         WriteAllLines(path, contents.ToArray(), encoding);
      }

      /// <inheritdoc/>
      public void WriteAllLines(String path, String[] contents, Encoding encoding)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         contents.ArgumentNotNull(nameof(contents));
         encoding.ArgumentNotNull(nameof(encoding));

         var createdFile = false;
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!FileExists(cleanedPath))
         {
            // BCL will create the file but not the directories leading to it.
            CreateFile(cleanedPath);
            createdFile = true;
         }
#endif
         var exceptionThrown = false;
         try
         {
            File.WriteAllLines(cleanedPath, contents, encoding);
         }
         catch (Exception)
         {
            exceptionThrown = true;
            throw;
         }
         finally
         {
            if (exceptionThrown && createdFile)
            {
               DeleteFile(cleanedPath);
            }
         }
      }

      /// <inheritdoc/>
      public void WriteAllText(String path, String contents, Encoding encoding)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         contents.ArgumentNotNull(nameof(contents));
         encoding.ArgumentNotNull(nameof(encoding));
         var createdFile = false;
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!FileExists(cleanedPath))
         {
            // BCL will create the file but not the directories leading to it.
            CreateFile(cleanedPath);
            createdFile = true;
         }
#endif
         var exceptionThrown = false;
         try
         {
            File.WriteAllText(cleanedPath, contents, encoding);
         }
         catch (Exception)
         {
            exceptionThrown = true;
            throw;
         }
         finally
         {
            if (exceptionThrown && createdFile)
            {
               DeleteFile(cleanedPath);
            }
         }
      }

      private static Boolean PathContainsUnmappedDrive(String path)
      {
         if (path == null)
         {
            return false;
         }

         path = path.RightTrim(' ');
         if (path.Length == 0)
         {
            return false;
         }

         Boolean rv;
         var idxColon = path.IndexOf(':');
         switch (idxColon)
         {
            case -1:

               // not found
               rv = false;
               break;

            case 1:

               // drive label
               rv = !DriveInfo.GetDrives().Any(d => path.StartsWith(d.Name, StringComparison.OrdinalIgnoreCase));
               break;

            default:
               rv = false;
               break;
         }

         return rv;
      }

      private static void ThrowDirectoryNotFoundException(String directoryPath, String argumentName)
      {
         throw new DirectoryNotFoundException(
            String.Format(
               CultureInfo.InvariantCulture,
               StringResources.CouldNotFindAllOrPartDirectoryPathParamNameFmt,
               directoryPath ?? String.Empty,
               argumentName ?? String.Empty));
      }

      private static void ThrowFileNotFoundException(String filePath, String argumentName)
      {
         throw new FileNotFoundException(
            String.Format(
               CultureInfo.InvariantCulture,
               "Could not find a part of the file path '{0}'.\r\nParameter name: {1}",
               filePath ?? String.Empty,
               argumentName ?? String.Empty));
      }

      private static void ThrowIfOnUnmappedDrive(String filePath, String argumentName)
      {
         var cleanedPath = (filePath ?? String.Empty).RightTrim(' ');
         if (PathContainsUnmappedDrive(cleanedPath))
         {
            ThrowFileNotFoundException(cleanedPath, argumentName);
         }
      }

      private void CopyImplementation(String sourceFileName, String destFileName, Boolean overwrite)
      {
         var cleanedSourceFileName = IOStringUtilities.ValidateCanonicalPath(sourceFileName, nameof(sourceFileName));
         var cleanedDestFileName = IOStringUtilities.ValidateCanonicalPath(destFileName, nameof(destFileName));

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedSourceFileName, nameof(sourceFileName));
         ThrowIfOnUnmappedDrive(cleanedDestFileName, nameof(destFileName));

         if (!FileExists(cleanedSourceFileName))
         {
            ThrowFileNotFoundException(cleanedSourceFileName, nameof(sourceFileName));
         }

         var directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();
         if (directoryUtilities.DirectoryExists(cleanedDestFileName))
         {
            throw new IOException(
               String.Format(
                  CultureInfo.InvariantCulture,
                  "Cannot create the destination file '{0}' because a directory with the same name already exists.",
                  cleanedDestFileName));
         }

         if (FileExists(cleanedDestFileName) && !overwrite)
         {
            throw new IOException(
               String.Format(CultureInfo.InvariantCulture, "Cannot create the destination file '{0}' because it already exists.", cleanedDestFileName));
         }
#endif

         File.Copy(cleanedSourceFileName, cleanedDestFileName, overwrite);
      }

      [SuppressMessage("SonarLint.CodeSmell", "S138: Functions should not have too many lines of code")]
      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      private void ReplaceContentsImplementation(
         String sourceFileName,
         String destinationFileName,
         String destinationBackupFileName,
         Boolean ignoreMetadataErrors)
      {
         var cleanedSourceFileName = IOStringUtilities.ValidateCanonicalPath(sourceFileName, nameof(sourceFileName));
         var cleanedDestinationFileName = IOStringUtilities.ValidateCanonicalPath(destinationFileName, nameof(destinationFileName));
         String cleanedDestinationBackupFileName = null;
         if (destinationBackupFileName != null)
         {
            cleanedDestinationBackupFileName = IOStringUtilities.ValidateCanonicalPath(destinationBackupFileName, "destinationBackupFileName");
         }

         var createdDestinationBackupFile = false;

#if IO_PRECHECKS

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var directoryUtilities = IocServiceLocator.Resolve<IDirectoryUtilities>();

         ThrowIfOnUnmappedDrive(cleanedSourceFileName, "sourceFileName");
         ThrowIfOnUnmappedDrive(cleanedDestinationFileName, "destinationFileName");
         if (cleanedDestinationBackupFileName != null)
         {
            ThrowIfOnUnmappedDrive(cleanedDestinationBackupFileName, "destinationBackupFileName");
         }

         if (directoryUtilities.DirectoryExists(cleanedSourceFileName))
         {
            throw new IOException(
               String.Format(CultureInfo.InvariantCulture, "The file name is invalid.  The source file '{0}' is a directory.", cleanedSourceFileName));
         }

         if (!FileExists(cleanedSourceFileName))
         {
            ThrowFileNotFoundException(cleanedSourceFileName, "sourceFileName");
         }

         if (directoryUtilities.DirectoryExists(cleanedDestinationFileName))
         {
            throw new IOException(
               String.Format(
                  CultureInfo.InvariantCulture,
                  "The file name is invalid.  The destination file '{0}' is a directory.",
                  cleanedDestinationFileName));
         }

         if (!FileExists(cleanedDestinationFileName))
         {
            ThrowFileNotFoundException(cleanedDestinationFileName, "destinationFileName");
         }

         var src = pathUtilities.GetFullPath(cleanedSourceFileName).ToUpperInvariant();
         var dst = pathUtilities.GetFullPath(cleanedDestinationFileName).ToUpperInvariant();
         if (String.Equals(src, dst, StringComparison.Ordinal))
         {
            throw new IOException(
               String.Format(
                  CultureInfo.InvariantCulture,
                  "Cannot replace the contents of destination file '{0}' because the source file and destination file are the same.",
                  cleanedDestinationFileName));
         }

         if (cleanedDestinationBackupFileName != null)
         {
            if (directoryUtilities.DirectoryExists(cleanedDestinationBackupFileName))
            {
               throw new IOException(
                  String.Format(
                     CultureInfo.InvariantCulture,
                     "The file name is invalid.  The destination backup file name '{0}' is a directory.",
                     cleanedDestinationBackupFileName));
            }

            var bck = pathUtilities.GetFullPath(cleanedDestinationBackupFileName).ToUpperInvariant();

            if (String.Equals(bck, src, StringComparison.Ordinal))
            {
               throw new IOException(
                  String.Format(
                     CultureInfo.InvariantCulture,
                     "Cannot replace the contents of destination file '{0}' because the source file and destination backup file are the same ('{1}').",
                     cleanedDestinationFileName,
                     cleanedSourceFileName));
            }

            if (String.Equals(bck, dst, StringComparison.Ordinal))
            {
               throw new IOException(
                  String.Format(
                     CultureInfo.InvariantCulture,
                     "Cannot replace the contents of destination file '{0}' because the destination file and destination backup file are the same.",
                     cleanedDestinationBackupFileName));
            }

            if (!FileExists(cleanedDestinationBackupFileName))
            {
               // to avoid an IOException with a confusing message.
               CreateFile(cleanedDestinationBackupFileName);
               createdDestinationBackupFile = true;
            }
         }

         // what if cleanedDestinationBackupFileName exists?

#endif

         // Windows API ReplaceFile wrapper
         // https://msdn.microsoft.com/en-us/library/windows/desktop/aa365512(v=vs.85).aspx
         // backup the contents of destinationFileName to destinationBackupFileName
         // replace the contents of destinationFileName with the contents of sourceFileName 
         // and delete sourceFileName

         // BCL notes
         // When source and destination are the same, an IOException is thrown stating: The process cannot access the file because it is being used by another process.
         // When backup is in a directory that does not exist, an IOException is thrown stating: Unable to remove the file to be replaced.
         // When backup and source are the same, an IOException is thrown stating:  Unable to remove the file to be replaced.
         // When backup and destination are the same, an IOException is thrown stating:  Unable to move the replacement file to the file to be replaced.
         //    The file to be replaced has retained its original name.

         var exceptionThrown = false;
         try
         {
            File.Replace(cleanedSourceFileName, cleanedDestinationFileName, cleanedDestinationBackupFileName, ignoreMetadataErrors);
         }
         catch (Exception)
         {
            exceptionThrown = true;
            throw;
         }
         finally
         {
            if (exceptionThrown && createdDestinationBackupFile)
            {
               DeleteFile(cleanedDestinationBackupFileName);
            }
         }
      }

      [SuppressMessage("SonarLint.CodeSmell", "S100: Methods and properties should be named in PascalCase")]
      [SuppressMessage("SonarLint.CodeSmell", "S3400: Methods should not return constants")]
      internal Boolean TestHookGetIOPrechecksEnabled()
      {
         return true;
      }
   }
}
