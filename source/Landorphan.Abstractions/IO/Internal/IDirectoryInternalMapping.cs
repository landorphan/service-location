namespace Landorphan.Abstractions.IO.Internal
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security;
    using Landorphan.Abstractions.IO.Interfaces;

    /// <summary>
    /// Represents the internal mapping from the static BCL <see cref="Directory"/> class to an interface.
    /// </summary>
    internal interface IDirectoryInternalMapping
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
        /// Copies a directory and its contents to a new location.
        /// </summary>
        /// <param name="sourceDirName">
        /// The path of directory to copy.
        /// </param>
        /// <param name="destDirName">
        /// The path to the destination directory.
        /// </param>
        /// <exception cref="IOException">
        /// <paramref name="destDirName"/> already exists.
        /// -or-
        /// The <paramref name="sourceDirName"/> and <paramref name="destDirName"/> parameters refer to the same file or directory.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permissions.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="sourceDirName"/>
        /// -or-
        /// <paramref name="destDirName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by
        /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceDirName"/> or <paramref name="destDirName"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The path specified by <paramref name="sourceDirName"/> is invalid (for example, it on an unmapped drive).
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
        void Copy(string sourceDirName, string destDirName);

        /// <summary>
        /// Creates all directories and subdirectories in the specified path.
        /// </summary>
        /// <param name="path">
        /// The directory path to create.
        /// </param>
        /// <returns>
        /// An object that represents the directory for the specified path.
        /// </returns>
        /// <exception cref="IOException">
        /// The directory specified by <paramref name="path"/> is a file.
        /// -or-
        /// The network name is not known.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by
        /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
        /// -or-
        /// <paramref name="path"/> is prefixed with, or contains only a colon character (:).
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
        /// <exception cref="NotSupportedException">
        /// <paramref name="path"/> contains a colon character (:) that is not part of a drive label (e.g., "C:\").
        /// </exception>
        string CreateDirectory(string path);

        /// <summary>
        /// Deletes an empty directory from a specified path.
        /// </summary>
        /// <param name="path">
        /// The name of the empty directory to remove. This directory must be writable or empty.
        /// </param>
        /// <exception cref="IOException">
        /// A file with the same name and location specified by <paramref name="path"/> exists.
        /// -or-
        /// The directory is the application's current working directory.
        /// -or-
        /// The directory specified by <paramref name="path"/> is not empty.
        /// -or-
        /// The directory is read-only or contains a read-only file.
        /// -or-
        /// The directory is being used by another process.
        /// -or-
        /// There is an open handle on the directory, and the operating system is Windows XP or earlier.  This open handle can result from directories. For more information,
        /// see How to: Enumerate Directories and Files.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by
        /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> does not exist or could not be found.
        /// -or-
        /// <paramref name="path"/> refers to a file instead of a directory.
        /// -or-
        /// The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        void DeleteEmpty(string path);

        /// <summary>
        /// Deletes the specified directory and any subdirectories and files in the directory.
        /// </summary>
        /// <param name="path">
        /// The name of the directory to remove.
        /// </param>
        /// <exception cref="IOException">
        /// A file with the same name and location specified by <paramref name="path"/> exists.
        /// -or-
        /// The directory specified by <paramref name="path"/> is read-only.
        /// -or-
        /// The directory is the application's current working directory.
        /// -or-
        /// The directory contains a read-only file.
        /// -or-
        /// The directory is being used by another process.
        /// There is an open handle on the directory or on one of its files, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories and
        /// files. For more information, see How to: Enumerate Directories and Files.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> does not exist or could not be found.
        /// -or-
        /// <paramref name="path"/> refers to a file instead of a directory.
        /// -or-
        /// The specified path is invalid (for example, it is on an unmapped drive).
        /// </exception>
        void DeleteRecursively(string path);

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">
        /// The path to test.
        /// </param>
        /// <returns>
        /// true if <paramref name="path"/> refers to an existing directory; otherwise, false.
        /// </returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Returns an enumerable collection of directory names in a specified path.
        /// </summary>
        /// <param name="path">
        /// The directory to search.
        /// </param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories in the directory specified by
        /// <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> EnumerateDirectories(string path);

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path.
        /// </summary>
        /// <param name="path">
        /// The directory to search.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of directories in <paramref name="path"/>.
        /// </param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories in the directory specified by
        /// <paramref name="path"/> and that match the specified search pattern.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// - or -
        /// <paramref name="searchPattern"/> does not contain a valid pattern.
        /// (.Net Standard 2.0 does not throw <see cref="ArgumentException"/> on invalid patterns, it simply returns no results.  This wrapper still throws <see cref="ArgumentException"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        ///  -or-
        /// <paramref name="searchPattern"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> EnumerateDirectories(string path, string searchPattern);

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">
        /// The directory to search.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of directories in <paramref name="path"/>.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.The default value is
        /// <see cref="SearchOption.TopDirectoryOnly"/>.
        /// </param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified
        /// search pattern and option.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by
        /// <see cref="Path.GetInvalidPathChars"/>.
        /// - or -
        /// <paramref name="searchPattern"/> does not contain a valid pattern.
        /// (.Net Standard 2.0 does not throw <see cref="ArgumentException"/> on invalid patterns, it simply returns no results.  This wrapper still throws <see cref="ArgumentException"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="searchOption"/> is not a valid <see cref="SearchOption"/>
        /// value.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns an enumerable collection of file names in a specified path.
        /// </summary>
        /// <param name="path">
        /// The directory to search.
        /// </param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files in the directory specified by
        /// <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by
        /// <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> EnumerateFiles(string path);

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path.
        /// </summary>
        /// <param name="path">
        /// The directory to search.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of directories in <paramref name="path"/>.
        /// </param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files in the directory specified by
        /// <paramref name="path"/> and that match the specified search pattern.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by
        /// <see cref="Path.GetInvalidPathChars"/>.
        /// - or -
        /// <paramref name="searchPattern"/> does not contain a valid pattern.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> EnumerateFiles(string path, string searchPattern);

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches
        /// subdirectories.
        /// </summary>
        /// <param name="path">
        /// The directory to search.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of directories in <paramref name="path"/>.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.  The default value is
        /// <see cref="SearchOption.TopDirectoryOnly"/>.
        /// </param>
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref  name="path"/> and that match the specified search pattern and option.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by
        /// <see cref="Path.GetInvalidPathChars"/>.
        /// - or -<paramref name="searchPattern"/> does not contain a valid pattern.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="searchOption"/> is not a valid <see cref="SearchOption"/>
        /// value.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns an enumerable collection of file-system entries in a specified path.
        /// </summary>
        /// <param name="path">
        /// The directory to search.
        /// </param>
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by
        /// <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> EnumerateFileSystemEntries(string path);

        /// <summary>
        /// Returns an enumerable collection of file-system entries that match a search pattern in a specified path.
        /// </summary>
        /// <param name="path">
        /// The directory to search.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of directories in <paramref name="path"/>.
        /// </param>
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified
        /// search pattern.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by
        /// <see cref="Path.GetInvalidPathChars"/>.
        /// - or -
        /// <paramref name="searchPattern"/> does not contain a valid pattern.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> EnumerateFileSystemEntries(string path, string searchPattern);

        /// <summary>
        /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally
        /// searches subdirectories.
        /// </summary>
        /// <param name="path">
        /// The directory to search.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of directories in <paramref name="path"/>.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values  that specifies whether the search operation should include only the current
        /// directory or should include all subdirectories.The default value is
        /// <see cref="SearchOption.TopDirectoryOnly"/>.
        /// </param>
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified
        /// search pattern and option.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by
        /// <see cref="Path.GetInvalidPathChars"/>.
        /// - or -<paramref name="searchPattern"/> does not contain a valid pattern.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="searchOption"/> is not a valid <see cref="SearchOption"/>
        /// value.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Gets the creation date and time of a directory.
        /// </summary>
        /// <param name="path">
        /// The path of the directory.
        /// </param>
        /// <returns>
        /// A structure that is set to the creation date and time for the specified directory.
        /// </returns>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by
        /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        DateTimeOffset GetCreationTime(string path);

        /// <summary>
        /// Gets the current working directory of the application.
        /// </summary>
        /// <returns>
        /// A string that contains the path of the current working directory, and does not end with a backslash (\).
        /// </returns>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The operating system is Windows CE, which does not have current directory functionality. This
        /// method is available in the .NET Compact Framework, but is not currently supported.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        string GetCurrentDirectory();

        /// <summary>
        /// Returns the names of subdirectories (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <returns>
        /// A non-null collection of unique subdirectories in the specified path, or an empty set if no directories are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> GetDirectories(string path);

        /// <summary>
        /// Returns the names of subdirectories (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of subdirectories in <paramref name="path"/>.  This parameter can contain a combination of valid literal and wildcard characters, but it
        /// does not support regular expressions.
        /// </param>
        /// <returns>
        /// A non-null collection of unique subdirectories in the specified path, or an empty set if no directories are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> GetDirectories(string path, string searchPattern);

        /// <summary>
        /// Returns the names of subdirectories (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of subdirectories in <paramref name="path"/>.  This parameter can contain a combination of valid literal and wildcard characters, but it
        /// does not support regular expressions.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.
        /// </param>
        /// <returns>
        /// A non-null collection of unique subdirectories in the specified path, or an empty set if no directories are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> GetDirectories(string path, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns the names of files (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <returns>
        /// A non-null collection of unique subdirectories in the specified path, or an empty set if no directories are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> GetFiles(string path);

        /// <summary>
        /// Returns the names of files (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of subdirectories in <paramref name="path"/>.  This parameter can contain a combination of valid literal and wildcard characters, but it
        /// does not support regular expressions.
        /// </param>
        /// <returns>
        /// A non-null collection of unique subdirectories in the specified path, or an empty set if no directories are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> GetFiles(string path, string searchPattern);

        /// <summary>
        /// Returns the names of files (including their paths) in the specified directory.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of subdirectories in <paramref name="path"/>.  This parameter can contain a combination of valid literal and wildcard characters, but it
        /// does not support regular expressions.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.
        /// </param>
        /// <returns>
        /// A non-null collection of unique subdirectories in the specified path, or an empty set if no directories are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> GetFiles(string path, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns the names of all files and subdirectories in a specified directory.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <returns>
        /// A non-null collection of unique subdirectories in the specified path, or an empty set if no directories are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> GetFileSystemEntries(string path);

        /// <summary>
        /// Returns the names of all files and subdirectories in a specified directory.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of subdirectories in <paramref name="path"/>.  This parameter can contain a combination of valid literal and wildcard characters, but it
        /// does not support regular expressions.
        /// </param>
        /// <returns>
        /// A non-null collection of unique subdirectories in the specified path, or an empty set if no directories are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> GetFileSystemEntries(string path, string searchPattern);

        /// <summary>
        /// Returns the names of all files and subdirectories in a specified directory.
        /// </summary>
        /// <param name="path">
        /// The relative or absolute path to the directory to search. This string is not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        /// The search string to match against the names of subdirectories in <paramref name="path"/>.  This parameter can contain a combination of valid literal and wildcard characters, but it
        /// does not support regular expressions.
        /// </param>
        /// <param name="searchOption">
        /// One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.
        /// </param>
        /// <returns>
        /// A non-null collection of unique subdirectories in the specified path, or an empty set if no directories are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
        /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <paramref name="path"/> is invalid, such as referring to an unmapped drive.
        /// </exception>
        /// <exception cref="IOException">
        /// <paramref name="path"/> is a file name.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or combined exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        IImmutableSet<string> GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Returns the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">
        /// The file or directory for which to obtain access date and time information.
        /// </param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory was last accessed.
        /// time.
        /// </returns>
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
        /// The <paramref name="path"/> parameter is in an invalid format.
        /// </exception>
        DateTimeOffset GetLastAccessTime(string path);

        /// <summary>
        /// Returns the date and time the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">
        /// The file or directory for which to obtain modification date and time information.
        /// </param>
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory was last written to.
        /// time.
        /// </returns>
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
        DateTimeOffset GetLastWriteTime(string path);

        /// <summary>
        /// Generates a a cryptographically strong, random string that can be used as a directory name.
        /// </summary>
        /// <returns>
        /// A random directory name.
        /// </returns>
        /// <remarks>
        /// Does not create a directory.
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        string GetRandomDirectoryName();

        /// <summary>
        /// Returns the path of the current user's temporary folder.
        /// </summary>
        /// <returns>
        /// The path to the temporary folder, ending with a backslash.
        /// </returns>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permissions.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        string GetTemporaryDirectoryPath();

        /// <summary>
        /// Moves a directory and its contents to a new location.
        /// </summary>
        /// <param name="sourceDirName">
        /// The path of the directory to move.
        /// </param>
        /// <param name="destDirName">
        /// The path to the new location for <paramref name="sourceDirName"/>.
        /// </param>
        /// <exception cref="IOException">
        /// An attempt was made to move a directory to a different volume.
        /// -or-
        /// <paramref name="destDirName"/> already exists.
        /// -or-
        /// The <paramref name="sourceDirName"/> and <paramref name="destDirName"/> parameters refer to the same directory.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permissions.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="sourceDirName"/> or <paramref name="destDirName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by
        /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceDirName"/> or <paramref name="destDirName"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The path specified by <paramref name="sourceDirName"/> is invalid (for example, it
        /// is on an unmapped drive).
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dir")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dest")]
        void Move(string sourceDirName, string destDirName);

        /// <summary>
        /// Sets the creation date and time for the specified file or directory.
        /// </summary>
        /// <param name="path">
        /// The file or directory for which to set the creation date and time information.
        /// </param>
        /// <param name="creationTime">
        /// An object that contains the value to set for the creation date and time of <paramref name="path"/>.
        /// </param>
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
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="creationTime"/> specifies a value outside the range of dates or times
        /// permitted for this operation.
        /// </exception>
        /// <exception cref="PlatformNotSupportedException">
        /// The current operating system is not Windows NT or later.
        /// </exception>
        [Obsolete("Currently not reliable")]
        void SetCreationTime(string path, DateTimeOffset creationTime);

        /// <summary>
        /// Sets the application's current working directory to the specified directory.
        /// </summary>
        /// <param name="path">
        /// The path to which the current working directory is set.
        /// </param>
        /// <exception cref="IOException">
        /// An I/O error occurred.
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
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission to access unmanaged code.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The specified path was not found.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified directory was not found.
        /// </exception>
        void SetCurrentDirectory(string path);

        /// <summary>
        /// Sets the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">
        /// The file or directory for which to set the access date and time information.
        /// </param>
        /// <param name="lastAccessTime">
        /// An object that contains the value to set for the access date and time of <paramref name="path"/>.
        /// </param>
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
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="PlatformNotSupportedException">
        /// The current operating system is not Windows NT or later.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="lastAccessTime"/> specifies a value outside the range of dates or times
        /// permitted for this operation.
        /// </exception>
        void SetLastAccessTime(string path, DateTimeOffset lastAccessTime);

        /// <summary>
        /// Sets the date and time a directory was last written to.
        /// </summary>
        /// <param name="path">
        /// The path of the directory.
        /// </param>
        /// <param name="lastWriteTime">
        /// The date and time the directory was last written to.
        /// </param>
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
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="PlatformNotSupportedException">
        /// The current operating system is not Windows NT or later.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="lastWriteTime"/> specifies a value outside the range of dates or times
        /// permitted for this operation.
        /// </exception>
        void SetLastWriteTime(string path, DateTimeOffset lastWriteTime);
    }
}
