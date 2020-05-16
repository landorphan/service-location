namespace Landorphan.Abstractions.IO.Interfaces
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    /// <summary>
   /// Represents methods for working with files.
   /// </summary>
   public interface IFileUtilities
   {
       /// <summary>
      /// Gets the maximum file time as a <see cref="DateTimeOffset"/>.
      /// </summary>
      /// <value>
      /// The maximum file time as a <see cref="DateTimeOffset"/>.
      /// </value>
      DateTimeOffset MaximumFileTimeAsDateTimeOffset { get; }

       /// <summary>
      /// Gets the maximum precision file system ticks supported by the host operating system.
      /// </summary>
      /// <value>
      /// The maximum precision file system ticks.
      /// </value>
      /// <remarks>
      /// On Windows, the file system supports precision down to 1 tick, or 100 nanoseconds, on linux, the precision is to the second.
      /// </remarks>
      long MaximumPrecisionFileSystemTicks { get; }

       /// <summary>
      /// Gets the minimum file time as a <see cref="DateTimeOffset"/>.
      /// </summary>
      /// <value>
      /// The minimum file time as a <see cref="DateTimeOffset"/>.
      /// </value>
      DateTimeOffset MinimumFileTimeAsDateTimeOffset { get; }

       /// <summary>
      /// Creates or overwrites a file in the specified path as a zero-byte file, and then closes the file.  If the specified path root exists, 
      /// this method will create intermediate directories, as well as the file itself as needed.
      /// </summary>
      /// <exception cref="UnauthorizedAccessException">
      /// The caller does not have the required permission.
      /// -or-
      /// <paramref name="path"/> specified a file that is read-only.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
      /// more invalid characters as defined by
      /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is null.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <exception cref="DirectoryNotFoundException">
      /// The specified path is invalid (for example, it is on an unmapped drive).
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred while creating the file.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <param name="path">
      /// The path and name of the file to create.
      /// </param>
      /// <returns>
      /// The full path of the temporary file.
      /// </returns>
      string CreateFile(string path);

       /// <summary>
      /// Creates a uniquely named, zero-byte temporary file on disk and returns the full path of that file.
      /// </summary>
      /// <exception cref="IOException">
      /// An I/O error occurs, such as no unique temporary file name is available.
      /// - or -
      /// This method was unable to create a temporary file.
      /// </exception>
      /// <returns>
      /// The full path of the temporary file.
      /// </returns>
      string CreateTemporaryFile();

       /// <summary>
      /// Creates or opens a file for writing UTF-8 encoded text. If the file already exists, its contents are overwritten.  If the specified path root exists, this method will create intermediate
      /// directories, as well as the file itself as needed.
      /// </summary>
      /// <param name="path">
      /// The path and name of the file to create.
      /// </param>
      /// <returns>
      /// The full path of the file.
      /// </returns>
      /// <exception cref="UnauthorizedAccessException">
      /// The caller does not have the required permission.
      /// -or-
      /// <paramref name="path"/> specified a file that is read-only.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
      /// more invalid characters as defined by
      /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is null.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <exception cref="DirectoryNotFoundException">
      /// The specified path is invalid (for example, it is on an unmapped drive).
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred while creating the file.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      string CreateText(string path);

       /// <summary>
      /// Deletes the specified file.
      /// </summary>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
      /// more invalid characters as defined by
      /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is null.
      /// </exception>
      /// <exception cref="DirectoryNotFoundException">
      /// The specified path is invalid (for example, it is on an unmapped drive).
      /// </exception>
      /// <exception cref="IOException">
      /// The specified file is in use. -or-There is an open handle on the file, and the operating system is
      /// Windows XP or earlier. This open handle can result from enumerating directories and files. For more
      /// information, see How to: Enumerate Directories and Files.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <exception cref="UnauthorizedAccessException">
      /// The caller does not have the required permission.-or- <paramref name="path"/> is a
      /// directory.
      /// -or-
      /// <paramref name="path"/> specified a read-only file.
      /// </exception>
      /// <param name="path">
      /// The name of the file to be deleted. Wild-card characters are not supported.
      /// </param>
      void DeleteFile(string path);

       /// <summary>
      /// Attempts to determine whether the specified file exists.
      /// </summary>
      /// <param name="path">
      /// The file to check.
      /// </param>
      /// <returns>
      /// <c>true</c> if the caller has the required permissions and <paramref name="path"/> contains the name of an existing file; otherwise,
      /// <c>false</c>.  
      /// <p>This method also returns <c>false</c> when <paramref name="path"/> is null, an invalid path, or a zero-length string.</p>
      /// <p>This method will also return <c>false</c> when the caller does not have sufficient permissions to read the specified file.</p>
      /// </returns>
      bool FileExists(string path);

       /// <summary>
      /// Returns the creation date and time of the specified file or directory.
      /// </summary>
      /// <exception cref="UnauthorizedAccessException">
      /// The caller does not have the required permission.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
      /// more invalid characters as defined by
      /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is null.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <param name="path">
      /// The file or directory for which to obtain creation date and time information.
      /// </param>
      /// <returns>
      /// A <see cref="DateTimeOffset"/> structure set to the creation date and time for the specified file or directory.
      /// </returns>
      DateTimeOffset GetCreationTime(string path);

       /// <summary>
      /// Returns the date and time the specified file or directory was last accessed.
      /// </summary>
      /// <exception cref="UnauthorizedAccessException">
      /// The caller does not have the required permission.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
      /// more invalid characters as defined by
      /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is null.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <param name="path">
      /// The file or directory for which to obtain access date and time information.
      /// </param>
      /// <returns>
      /// A <see cref="DateTimeOffset"/> structure set to the date and time that the specified file or directory was last accessed.
      /// </returns>
      DateTimeOffset GetLastAccessTime(string path);

       /// <summary>
      /// Returns the date and time the specified file or directory was last written to.
      /// </summary>
      /// <exception cref="UnauthorizedAccessException">
      /// The caller does not have the required permission.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
      /// more invalid characters as defined by
      /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is null.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <param name="path">
      /// The file or directory for which to obtain write date and time information.
      /// </param>
      /// <returns>
      /// A <see cref="DateTimeOffset"/> structure set to the date and time that the specified file or directory was last written to.
      /// </returns>
      DateTimeOffset GetLastWriteTime(string path);

       /// <summary>
      /// Generates a a cryptographically strong, random string that can be used as a file name.
      /// </summary>
      /// <returns>
      /// A random file name.
      /// </returns>
      /// <remarks>
      /// Does not create a file.
      /// </remarks>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      string GetRandomFileName();

       /*  REMOVE BECAUSE IT IS UNRELIABLE, ESPECIALLY ON LINUX
       /// <summary>
       /// Sets the date and time the file was created.
       /// </summary>
       /// <exception cref="FileNotFoundException">
       /// The specified path was not found.
       /// </exception>
       /// <exception cref="ArgumentException">
       /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
       /// more invalid characters as defined by
       /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
       /// </exception>
       /// <exception cref="ArgumentNullException">
       /// <paramref name="path"/> is null.
       /// </exception>
       /// <exception cref="PathTooLongException">
       /// The specified path, file name, or both exceed the system-defined maximum length.
       /// </exception>
       /// <exception cref="IOException">
       /// An I/O error occurred while performing the operation.
       /// </exception>
       /// <exception cref="ArgumentOutOfRangeException">
       /// <paramref name="creationTime"/> specifies a value outside the range of dates, times,
       /// or both permitted for this operation.
       /// </exception>
       /// <exception cref="UnauthorizedAccessException">
       /// The caller does not have the required permission.
       /// </exception>
       /// <exception cref="NotSupportedException">
       /// <paramref name="path"/> is in an invalid format.
       /// </exception>
       /// <param name="path">
       /// The file for which to set the creation date and time information.
       /// </param>
       /// <param name="creationTime">
       /// A <see cref="DateTimeOffset"/> containing the value to set for the creation date and time of
       /// <paramref name="path"/>.
       /// </param>
       void SetCreationTime(String path, DateTimeOffset creationTime);
       */

       /// <summary>
      /// Sets the date and time the specified file was last accessed.
      /// </summary>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
      /// more invalid characters as defined by
      /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is null.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <exception cref="FileNotFoundException">
      /// The specified path was not found.
      /// </exception>
      /// <exception cref="UnauthorizedAccessException">
      /// The caller does not have the required permission.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <exception cref="ArgumentOutOfRangeException">
      /// <paramref name="lastAccessTime"/> specifies a value outside the range of dates or
      /// times permitted for this operation.
      /// </exception>
      /// <param name="path">
      /// The file for which to set the access date and time information.
      /// </param>
      /// <param name="lastAccessTime">
      /// A <see cref="DateTimeOffset"/> containing the value to set for the last access date and time of
      /// <paramref name="path"/>. 
      /// </param>
      void SetLastAccessTime(string path, DateTimeOffset lastAccessTime);

       /// <summary>
      /// Sets the date and time that the specified file was last written to.
      /// </summary>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
      /// more invalid characters as defined by
      /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is null.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <exception cref="FileNotFoundException">
      /// The specified path was not found.
      /// </exception>
      /// <exception cref="UnauthorizedAccessException">
      /// The caller does not have the required permission.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <exception cref="ArgumentOutOfRangeException">
      /// <paramref name="lastWriteTime"/> specifies a value outside the range of dates or times
      /// permitted for this operation.
      /// </exception>
      /// <param name="path">
      /// The file for which to set the date and time information.
      /// </param>
      /// <param name="lastWriteTime">
      /// A <see cref="DateTimeOffset"/> containing the value to set for the last write date and time of
      /// <paramref name="path"/>. 
      /// </param>
      void SetLastWriteTime(string path, DateTimeOffset lastWriteTime);
   }
}
