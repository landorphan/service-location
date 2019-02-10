namespace Landorphan.Abstractions.IO.Interfaces
{
   using System;
   using System.Collections.Immutable;
   using System.IO;
   using System.Security;

   /// <summary>
   /// Represents methods for reading from directories.
   /// </summary>
   public interface IDirectoryReaderUtilities
   {
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
      IImmutableSet<String> EnumerateDirectories(String path);

      /// <summary>
      /// Returns an enumerable collection of directory names that match a search pattern in a specified path.
      /// </summary>
      /// <exception cref="ArgumentException">
      /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid
      /// characters as defined by <see cref="Path.GetInvalidPathChars"/>.
      /// - or -
      /// <paramref name="searchPattern"/> does not contain a valid pattern.
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
      IImmutableSet<String> EnumerateDirectories(String path, String searchPattern);

      /// <summary>
      /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches
      /// subdirectories.
      /// </summary>
      /// <exception cref="ArgumentException">
      /// <paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by
      /// <see cref="Path.GetInvalidPathChars"/>.
      /// - or -
      /// <paramref name="searchPattern"/> does not contain a valid pattern.
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
      /// An enumerable collection of the full names (including paths) for the directories in the directory specified by
      /// <paramref name="path"/> and that match the specified search pattern and option.
      /// </returns>
      IImmutableSet<String> EnumerateDirectories(String path, String searchPattern, SearchOption searchOption);

      /// <summary>
      /// Returns an enumerable collection of file names in a specified path.
      /// </summary>
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
      /// <param name="path">
      /// The directory to search.
      /// </param>
      /// <returns>
      /// An enumerable collection of the full names (including paths) for the files in the directory specified by
      /// <paramref name="path"/>.
      /// </returns>
      IImmutableSet<String> EnumerateFiles(String path);

      /// <summary>
      /// Returns an enumerable collection of file names that match a search pattern in a specified path.
      /// </summary>
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
      IImmutableSet<String> EnumerateFiles(String path, String searchPattern);

      /// <summary>
      /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches
      /// subdirectories.
      /// </summary>
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
      /// <param name="path">
      /// The directory to search.
      /// </param>
      /// <param name="searchPattern">
      /// The search string to match against the names of directories in <paramref name="path"/>.
      /// </param>
      /// <param name="searchOption">
      /// One of the enumeration values that specifies whether the search operation should include only the current
      /// directory or should include all subdirectories.The default value is
      /// <see cref="SearchOption.TopDirectoryOnly"/>.
      /// </param>
      /// <returns>
      /// An enumerable collection of the full names (including paths) for the files in the directory specified by
      /// <paramref  name="path"/> and that match the specified search pattern and option.
      /// </returns>
      IImmutableSet<String> EnumerateFiles(String path, String searchPattern, SearchOption searchOption);

      /// <summary>
      /// Returns an enumerable collection of file-system entries in a specified path.
      /// </summary>
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
      /// <param name="path">
      /// The directory to search.
      /// </param>
      /// <returns>
      /// An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.
      /// </returns>
      IImmutableSet<String> EnumerateFileSystemEntries(String path);

      /// <summary>
      /// Returns an enumerable collection of file-system entries that match a search pattern in a specified path.
      /// </summary>
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
      IImmutableSet<String> EnumerateFileSystemEntries(String path, String searchPattern);

      /// <summary>
      /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally
      /// searches subdirectories.
      /// </summary>
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
      IImmutableSet<String> EnumerateFileSystemEntries(String path, String searchPattern, SearchOption searchOption);

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
      IImmutableSet<String> GetDirectories(String path);

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
      IImmutableSet<String> GetDirectories(String path, String searchPattern);

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
      IImmutableSet<String> GetDirectories(String path, String searchPattern, SearchOption searchOption);

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
      IImmutableSet<String> GetFiles(String path);

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
      IImmutableSet<String> GetFiles(String path, String searchPattern);

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
      IImmutableSet<String> GetFiles(String path, String searchPattern, SearchOption searchOption);

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
      IImmutableSet<String> GetFileSystemEntries(String path);

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
      IImmutableSet<String> GetFileSystemEntries(String path, String searchPattern);

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
      IImmutableSet<String> GetFileSystemEntries(String path, String searchPattern, SearchOption searchOption);
   }
}
