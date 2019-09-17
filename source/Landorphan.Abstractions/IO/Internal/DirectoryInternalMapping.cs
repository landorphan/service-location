#define IO_PRECHECKS

namespace Landorphan.Abstractions.IO.Internal
{
   using System;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Abstractions.Resources;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Exposes methods for creating, moving, and enumerating through directories and subdirectories.
   /// </summary>
   /// <remarks>
   /// Provides a near one-to-one mapping to <see cref="Directory"/> but in an object instance (as opposed to static) to support testability.
   /// </remarks>
   [SuppressMessage("SonarLint.CodeSmell", "S2148: Underscores should be used to make large numbers readable")]
   internal sealed class DirectoryInternalMapping : IDirectoryInternalMapping
   {
      // Use IO_PRECHECKS to enable/disable non-canonical validation before the BCL call.  These are used to improve the exception messaging.

      ///<inheritdoc/>
      public DateTimeOffset MaximumFileTimeAsDateTimeOffset => FileTimeHelper.MaximumFileTimeAsDateTimeOffset;

      ///<inheritdoc/>
      public Int64 MaximumPrecisionFileSystemTicks => FileTimeHelper.MaximumPrecisionFileSystemTicks;

      ///<inheritdoc/>
      public DateTimeOffset MinimumFileTimeAsDateTimeOffset => FileTimeHelper.MinimumFileTimeAsDateTimeOffset;

      /// <inheritdoc/>
      public void Copy(String sourceDirName, String destDirName)
      {
         var cleanedSourceDirName = IOStringUtilities.ValidateCanonicalPath(sourceDirName, "sourceDirName");
         var cleanedDestDirName = IOStringUtilities.ValidateCanonicalPath(destDirName, "destDirName");

         var fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedSourceDirName, "sourceDirName");
         ThrowIfOnUnmappedDrive(cleanedDestDirName, "destDirName");

         if (!DirectoryExists(cleanedSourceDirName))
         {
            ThrowDirectoryNotFoundException(cleanedSourceDirName, "sourceDirName");
         }

         if (fileUtilities.FileExists(cleanedDestDirName))
         {
            var msg = String.Format(
               CultureInfo.InvariantCulture,
               StringResources.CannotCopyToDestinationDirectoryFileAlreadyExistsFmt,
               cleanedDestDirName);
            throw new IOException(msg);
         }

         var sourceFullPath = pathUtilities.GetFullPath(cleanedSourceDirName);
         var destinationFullPath = pathUtilities.GetFullPath(cleanedDestDirName);
         if (String.Equals(sourceFullPath, destinationFullPath, StringComparison.OrdinalIgnoreCase))
         {
            // nothing to do
            return;
         }

         if (pathUtilities.GetFullPath(cleanedDestDirName).Contains(pathUtilities.GetFullPath(cleanedSourceDirName)))
         {
            var msg = String.Format(
               CultureInfo.InvariantCulture,
               StringResources.CannotCopyToDestinationDirectorySubfolderOfSourceFmt,
               cleanedDestDirName,
               cleanedSourceDirName);
            throw new IOException(msg);
         }
#endif
         // Throws expected errors when user lacks permissions.
         CreateDirectory(cleanedDestDirName);

         foreach (var fullDirectoryPath in Directory.GetDirectories(cleanedSourceDirName, "*", SearchOption.AllDirectories))
         {
            CreateDirectory(cleanedDestDirName + fullDirectoryPath.Substring(cleanedSourceDirName.Length));
         }

         var fileWriterUtilities = IocServiceLocator.Resolve<IFileWriterUtilities>();
         foreach (var fullFilePath in Directory.GetFiles(cleanedSourceDirName, "*.*", SearchOption.AllDirectories))
         {
            fileWriterUtilities.CopyWithOverwrite(fullFilePath, cleanedDestDirName + fullFilePath.Substring(cleanedSourceDirName.Length));
         }
      }

      /// <inheritdoc/>
      public String CreateDirectory(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));
#endif

         // BCL behavior.  
         //    Trailing spaces are trimmed, leading spaces are not on each folder
         //    @"c:\temp\ abc \ def \" creates @"c:\temp\ abc\ def\"
         //    Maximum length is governed by the string length of full path, which results in
         //       Exceptions when the share name is longer than the local path when the share name is used, and
         //       Files that cannot be used with the local path, when the share name is shorter.
         var di = Directory.CreateDirectory(cleanedPath);
         return di.FullName;
      }

      /// <inheritdoc/>
      public void DeleteEmpty(String path)
      {
         try
         {
            DeleteImplementation(path, false);
         }
         catch (IOException ioe)
         {
            // change the type to the same type used by other methods.
            if (ioe.Message.Contains("Access to the path") && ioe.Message.Contains("is denied") && ioe.Message.Contains(path))
            {
               throw new UnauthorizedAccessException(ioe.Message, ioe);
            }

            throw;
         }
      }

      /// <inheritdoc/>
      public void DeleteRecursively(String path)
      {
         DeleteImplementation(path, true);
      }

      /// <inheritdoc/>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SuppressMessage("SonarLint.CodeSmell", "S2221: Exception should not be caught when not required by called methods")]
      public Boolean DirectoryExists(String path)
      {
         try
         {
            var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
            // will return false on  @"\\localhost" and @"\\localhost\"
            return Directory.Exists(cleanedPath);
         }
         catch
         {
            return false;
         }
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateDirectories(String path)
      {
         return EnumerateDirectories(path, "*", SearchOption.AllDirectories);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateDirectories(String path, String searchPattern)
      {
         return EnumerateDirectories(path, searchPattern, SearchOption.AllDirectories);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateDirectories(String path, String searchPattern, SearchOption searchOption)
      {
         path.ArgumentNotNull(nameof(path));
         searchPattern.ArgumentNotNull(nameof(searchPattern));
         searchOption.ArgumentMustBeValidEnumValue(nameof(searchOption));

         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         // BCL Directory.EnumerateDirectories no longer throws ArgumentException on invalid searchOption
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         const Int32 indexNotFound = -1;
         if (indexNotFound != searchPattern.IndexOfAny(pathUtilities.GetInvalidPathCharacters().ToArray()))
         {
            throw new ArgumentException(StringResources.SearchPatternContainsInvalidCharacters, nameof(searchPattern));
         }
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif

         try
         {
            var rv = Directory.EnumerateDirectories(cleanedPath, searchPattern, searchOption);
            return rv.ToImmutableHashSet();
         }
         catch (ArgumentException ae)
         {
            // TODO: check .Net Standard 2.0 implementation, this is from .Net Fx 4.6.1
            if (ae.Message.StartsWith(StringResources.SearchPatternCannotContain, StringComparison.Ordinal) && ae.ParamName == null)
            {
               // add the parameter name.
               throw new ArgumentException(ae.Message, nameof(searchPattern), ae);
            }

            throw;
         }
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFiles(String path)
      {
         return EnumerateFiles(path, "*", SearchOption.AllDirectories);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFiles(String path, String searchPattern)
      {
         return EnumerateFiles(path, searchPattern, SearchOption.AllDirectories);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFiles(String path, String searchPattern, SearchOption searchOption)
      {
         path.ArgumentNotNull(nameof(path));
         searchPattern.ArgumentNotNull(nameof(searchPattern));
         searchOption.ArgumentMustBeValidEnumValue(nameof(searchOption));

         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         // BCL Directory.EnumerateFiles no longer throws ArgumentException on invalid searchOption
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         const Int32 indexNotFound = -1;

         // must not use GetInvalidFileNameCharacters, because that excludes valid search characters such as " * / : < > ? \
         if (indexNotFound != searchPattern.IndexOfAny(pathUtilities.GetInvalidPathCharacters().ToArray()))
         {
            throw new ArgumentException(@"The search pattern is not well-formed (contains invalid characters).", nameof(searchPattern));
         }
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif

         try
         {
            var rv = Directory.EnumerateFiles(cleanedPath, searchPattern, searchOption);
            return rv.ToImmutableHashSet();
         }
         catch (ArgumentException ae)
         {
            if (ae.Message.StartsWith(StringResources.SearchPatternCannotContain, StringComparison.Ordinal) && ae.ParamName == null)
            {
               // add the parameter name.
               throw new ArgumentException(ae.Message, nameof(searchPattern), ae);
            }

            throw;
         }
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFileSystemEntries(String path)
      {
         return EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFileSystemEntries(String path, String searchPattern)
      {
         return EnumerateFileSystemEntries(path, searchPattern, SearchOption.AllDirectories);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> EnumerateFileSystemEntries(String path, String searchPattern, SearchOption searchOption)
      {
         path.ArgumentNotNull(nameof(path));
         searchPattern.ArgumentNotNull(nameof(searchPattern));
         searchOption.ArgumentMustBeValidEnumValue(nameof(searchOption));

         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         // BCL Directory.EnumerateFileSystemEntries no longer throws ArgumentException on invalid searchOption
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         const Int32 indexNotFound = -1;
         if (indexNotFound != searchPattern.IndexOfAny(pathUtilities.GetInvalidPathCharacters().ToArray()))
         {
            throw new ArgumentException(@"The search pattern is not well-formed (contains invalid characters).", nameof(searchPattern));
         }
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif

         try
         {
            var rv = Directory.EnumerateFileSystemEntries(cleanedPath, searchPattern, searchOption);
            return rv.ToImmutableHashSet();
         }
         catch (ArgumentException ae)
         {
            if (ae.Message.StartsWith(StringResources.SearchPatternCannotContain, StringComparison.Ordinal) && ae.ParamName == null)
            {
               // add the parameter name.
               throw new ArgumentException(ae.Message, nameof(searchPattern), ae);
            }

            throw;
         }
      }

      /// <inheritdoc/>
      public DateTimeOffset GetCreationTime(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif

         var rv = new DateTimeOffset(Directory.GetCreationTime(cleanedPath).ToUtc());
         return rv;
      }

      /// <inheritdoc/>
      public String GetCurrentDirectory()
      {
         return Directory.GetCurrentDirectory();
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetDirectories(String path)
      {
         return GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetDirectories(String path, String searchPattern)
      {
         return GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetDirectories(String path, String searchPattern, SearchOption searchOption)
      {
         path.ArgumentNotNull(nameof(path));
         searchPattern.ArgumentNotNull(nameof(searchPattern));
         searchOption.ArgumentMustBeValidEnumValue(nameof(searchOption));

         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         // BCL no longer throws ArgumentException on invalid searchOption
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         const Int32 indexNotFound = -1;
         if (indexNotFound != searchPattern.IndexOfAny(pathUtilities.GetInvalidPathCharacters().ToArray()))
         {
            throw new ArgumentException(StringResources.SearchPatternContainsInvalidCharacters, nameof(searchPattern));
         }
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif

         try
         {
            var rv = Directory.GetDirectories(cleanedPath, searchPattern, searchOption);
            return rv.ToImmutableHashSet();
         }
         catch (ArgumentException ae)
         {
            // TODO: check .Net Standard 2.0 implementation, this is from .Net Fx 4.6.1
            if (ae.Message.StartsWith(StringResources.SearchPatternCannotContain, StringComparison.Ordinal) && ae.ParamName == null)
            {
               // add the parameter name.
               throw new ArgumentException(ae.Message, nameof(searchPattern), ae);
            }

            throw;
         }
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFiles(String path)
      {
         return GetFiles(path, "*", SearchOption.TopDirectoryOnly);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFiles(String path, String searchPattern)
      {
         return GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFiles(String path, String searchPattern, SearchOption searchOption)
      {
         path.ArgumentNotNull(nameof(path));
         searchPattern.ArgumentNotNull(nameof(searchPattern));
         searchOption.ArgumentMustBeValidEnumValue(nameof(searchOption));

         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         // BCL no longer throws ArgumentException on invalid searchOption
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         const Int32 indexNotFound = -1;
         if (indexNotFound != searchPattern.IndexOfAny(pathUtilities.GetInvalidPathCharacters().ToArray()))
         {
            throw new ArgumentException(StringResources.SearchPatternContainsInvalidCharacters, nameof(searchPattern));
         }
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif

         try
         {
            var rv = Directory.GetFiles(cleanedPath, searchPattern, searchOption);
            return rv.ToImmutableHashSet();
         }
         catch (ArgumentException ae)
         {
            // TODO: check .Net Standard 2.0 implementation, this is from .Net Fx 4.6.1
            if (ae.Message.StartsWith(StringResources.SearchPatternCannotContain, StringComparison.Ordinal) && ae.ParamName == null)
            {
               // add the parameter name.
               throw new ArgumentException(ae.Message, nameof(searchPattern), ae);
            }

            throw;
         }
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFileSystemEntries(String path)
      {
         return GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFileSystemEntries(String path, String searchPattern)
      {
         return GetFileSystemEntries(path, searchPattern, SearchOption.TopDirectoryOnly);
      }

      /// <inheritdoc/>
      public IImmutableSet<String> GetFileSystemEntries(String path, String searchPattern, SearchOption searchOption)
      {
         path.ArgumentNotNull(nameof(path));
         searchPattern.ArgumentNotNull(nameof(searchPattern));
         searchOption.ArgumentMustBeValidEnumValue(nameof(searchOption));

         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         // BCL no longer throws ArgumentException on invalid searchOption
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         const Int32 indexNotFound = -1;
         if (indexNotFound != searchPattern.IndexOfAny(pathUtilities.GetInvalidPathCharacters().ToArray()))
         {
            throw new ArgumentException(StringResources.SearchPatternContainsInvalidCharacters, nameof(searchPattern));
         }
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif

         try
         {
            var rv = Directory.GetFileSystemEntries(cleanedPath, searchPattern, searchOption);
            return rv.ToImmutableHashSet();
         }
         catch (ArgumentException ae)
         {
            // TODO: check .Net Standard 2.0 implementation, this is from .Net Fx 4.6.1
            if (ae.Message.StartsWith(StringResources.SearchPatternCannotContain, StringComparison.Ordinal) && ae.ParamName == null)
            {
               // add the parameter name.
               throw new ArgumentException(ae.Message, nameof(searchPattern), ae);
            }

            throw;
         }
      }

      /// <inheritdoc/>
      public DateTimeOffset GetLastAccessTime(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }

#endif

         var rv = new DateTimeOffset(Directory.GetLastAccessTime(cleanedPath).ToUtc());

         return rv;
      }

      /// <inheritdoc/>
      public DateTimeOffset GetLastWriteTime(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif
         var rv = new DateTimeOffset(Directory.GetLastWriteTime(cleanedPath).ToUtc());

         return rv;
      }

      /// <inheritdoc/>
      public String GetRandomDirectoryName()
      {
         return Path.GetRandomFileName();
      }

      /// <inheritdoc/>
      public String GetTemporaryDirectoryPath()
      {
         return Path.GetTempPath();
      }

      /// <inheritdoc/>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SuppressMessage("SonarLint.CodeSmell", "S2221: Exception should not be caught when not required by called methods")]
      public void Move(String sourceDirName, String destDirName)
      {
         var cleanedSourceDirName = IOStringUtilities.ValidateCanonicalPath(sourceDirName, "sourceDirName");
         var cleanedDestDirName = IOStringUtilities.ValidateCanonicalPath(destDirName, "destDirName");

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedSourceDirName, "sourceDirName");
         ThrowIfOnUnmappedDrive(cleanedDestDirName, "destDirName");

         if (!DirectoryExists(cleanedSourceDirName))
         {
            ThrowDirectoryNotFoundException(cleanedSourceDirName, "sourceDirName");
         }

         var fileUtilities = IocServiceLocator.Instance.Resolve<IFileUtilities>();
         if (fileUtilities.FileExists(cleanedDestDirName))
         {
            var msg = String.Format(
               CultureInfo.InvariantCulture,
               StringResources.CannotMoveToDestinationDirectoryFileAlreadyExistsFmt,
               cleanedDestDirName);
            throw new IOException(msg);
         }

         if (DirectoryExists(cleanedDestDirName))
         {
            throw new IOException(
               String.Format(
                  CultureInfo.InvariantCulture,
                  StringResources.CannotMoveToDestinationDirectoryDirectoryAlreadyExistsFmt,
                  cleanedDestDirName));
         }

         var pathUtilities = IocServiceLocator.Instance.Resolve<IPathUtilities>();
         if (pathUtilities.GetFullPath(cleanedDestDirName).Contains(pathUtilities.GetFullPath(cleanedSourceDirName)))
         {
            throw new IOException(
               String.Format(
                  CultureInfo.InvariantCulture,
                  StringResources.CannotMoveToDestinationDirectorySubfolderOfSourceFmt,
                  cleanedDestDirName,
                  cleanedSourceDirName));
         }
#endif

         try
         {
            Directory.Move(cleanedSourceDirName, cleanedDestDirName);
         }
         catch (IOException ioe)
         {
            // BCL reports the sourceDirName when caller does not have access to the destDirName path.

            var sourceDirNameMessage = String.Format(
               CultureInfo.InvariantCulture,
               StringResources.AccessToThePathIsDeniedFmt,
               cleanedSourceDirName);
            if (String.Equals(ioe.Message, sourceDirNameMessage, StringComparison.Ordinal))
            {
               Boolean haveAccessToSourceDirName;
               try
               {
                  haveAccessToSourceDirName = true;
               }
               catch
               {
                  haveAccessToSourceDirName = false;
               }

               // Standardize behavior on UnauthorizedAccessException (like other Directory methods).
               if (haveAccessToSourceDirName)
               {
                  // have access to sourceDirName so correct the message.
                  // (changes HResult from 0x80070005 to 0x80131620) 
                  throw new UnauthorizedAccessException(
                     String.Format(CultureInfo.InvariantCulture, StringResources.AccessToThePathIsDeniedFmt, cleanedDestDirName),
                     ioe);
               }

               throw new UnauthorizedAccessException(sourceDirNameMessage, ioe);
            }

            throw;
         }
      }

      /// <inheritdoc/>
      [Obsolete("Currently not reliable")]
      public void SetCreationTime(String path, DateTimeOffset creationTime)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         creationTime = FileTimeHelper.TruncateTicksToFileSystemPrecision(creationTime);

         if (creationTime < MinimumFileTimeAsDateTimeOffset)
         {
            var msg = String.Format(
               CultureInfo.InvariantCulture,
               StringResources.ValueMustBeGreaterThanOrEqualToTicksFmt,
               MinimumFileTimeAsDateTimeOffset.Ticks.ToString("N0", CultureInfo.InvariantCulture));
            throw new ArgumentOutOfRangeException(nameof(creationTime), msg);
         }

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }

#endif

         Directory.SetCreationTimeUtc(cleanedPath, creationTime.UtcDateTime);
      }

      /// <inheritdoc/>
      public void SetCurrentDirectory(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif
         Directory.SetCurrentDirectory(cleanedPath);
      }

      /// <inheritdoc/>
      [Obsolete("Currently not reliable")]
      public void SetLastAccessTime(String path, DateTimeOffset lastAccessTime)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         lastAccessTime = FileTimeHelper.TruncateTicksToFileSystemPrecision(lastAccessTime);

         if (lastAccessTime < MinimumFileTimeAsDateTimeOffset)
         {
            var msg = String.Format(
               CultureInfo.InvariantCulture,
               StringResources.ValueMustBeGreaterThanOrEqualToTicksFmt,
               MinimumFileTimeAsDateTimeOffset.Ticks.ToString("N0", CultureInfo.InvariantCulture));
            throw new ArgumentOutOfRangeException(nameof(lastAccessTime), msg);
         }

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif
         Directory.SetLastAccessTimeUtc(cleanedPath, lastAccessTime.UtcDateTime);
      }

      /// <inheritdoc/>
      [Obsolete("Currently not reliable")]
      [SuppressMessage("SonarLint.CodeSmell", "S109: Magic numbers should not be used")]
      public void SetLastWriteTime(String path, DateTimeOffset lastWriteTime)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         lastWriteTime = FileTimeHelper.TruncateTicksToFileSystemPrecision(lastWriteTime);

         if (lastWriteTime < MinimumFileTimeAsDateTimeOffset)
         {
            var msg = String.Format(
               CultureInfo.InvariantCulture,
               StringResources.ValueMustBeGreaterThanOrEqualToTicksFmt,
               MinimumFileTimeAsDateTimeOffset.Ticks.ToString("N0", CultureInfo.InvariantCulture));
            throw new ArgumentOutOfRangeException(nameof(lastWriteTime), msg);
         }

#if IO_PRECHECKS
         ThrowIfOnUnmappedDrive(cleanedPath, nameof(path));

         if (!DirectoryExists(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, nameof(path));
         }
#endif

         Directory.SetLastWriteTimeUtc(cleanedPath, lastWriteTime.UtcDateTime);
      }

      [SuppressMessage("SonarLint.CodeSmell", "S109: Magic numbers should not be used")]
      private static void DeleteImplementation(String path, Boolean recursive)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));

         // choice:  whether or not to throw when path is on an unmapped drive, currently not throwing.
#if IO_PRECHECKS
         var fileUtilities = IocServiceLocator.Instance.Resolve<IFileUtilities>();
         if (fileUtilities.FileExists(cleanedPath))
         {
            // improve the BCL message: 'The directory name is invalid.'
            var msg = String.Format(CultureInfo.InvariantCulture, StringResources.TheDirectoryNameIsInvalidFileAlreadyExistsFmt, cleanedPath);
            throw new IOException(msg, unchecked((Int32)0x8007010b));
         }
#endif

         if (Directory.Exists(cleanedPath))
         {
            // exists used to get expected exceptions
            try
            {
               Directory.Delete(cleanedPath, recursive);
            }
            catch (DirectoryNotFoundException)
            {
               // eat the exception
            }
         }
      }

      private static Boolean PathContainsUnmappedDrive(String path)
      {
         // Assumes path has been trimmed of leading

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
               // RCA of previous bug:  DriveInfo.GetDrives() returns an array with {DriveLetter}:{DirectorySeparatorCharacter} paths.
               var possiblePathDriveLabel = path.Substring(0, 2);
               var driveLabels = (from di in DriveInfo.GetDrives() select di.Name.Substring(0, 2)).ToList();
               rv = !driveLabels.Contains(possiblePathDriveLabel, StringComparer.OrdinalIgnoreCase);
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

      private static void ThrowIfOnUnmappedDrive(String directoryPath, String argumentName)
      {
         var cleanedPath = (directoryPath ?? String.Empty).RightTrim(' ');

         // most BCL Directory methods throw new DirectoryNotFoundException("Could not find a part of the path '<path>'.");
         // some throw new InvalidOperationException("Method failed with unexpected error code 3.");
         // standardize on one behavior.
         if (PathContainsUnmappedDrive(cleanedPath))
         {
            ThrowDirectoryNotFoundException(cleanedPath, argumentName);
         }
      }

      internal static Boolean TestHookPathContainsUnmappedDrive(String path)
      {
         return PathContainsUnmappedDrive(path);
      }
   }
}
