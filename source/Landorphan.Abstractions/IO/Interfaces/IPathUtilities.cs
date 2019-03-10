namespace Landorphan.Abstractions.IO.Interfaces
{
   using System;
   using System.Collections.Immutable;
   using System.Globalization;
   using System.IO;
   using System.Security;

   /// <summary>
   /// Performs operations on <see cref="string"/> instances that contain file or directory path information. These operations
   /// are performed in a cross-platform manner.
   /// </summary>
   public interface IPathUtilities
   {
      /// <summary>
      /// Provides a platform-specific alternate character used to separate directory levels in a path string that reflects a hierarchical
      /// file system organization.
      /// </summary>
      /// <value>
      /// The alternate directory separator character.
      /// </value>
      Char AltDirectorySeparatorCharacter { get; }

      /// <summary>
      /// Provides the platform-specific alternate directory separator as a string to simplify casting
      /// issues.
      /// </summary>
      /// <value>
      /// The alternate directory separator as a string.
      /// </value>
      string AltDirectorySeparatorString { get; }

      /// <summary>
      /// Provides a platform-specific character used to separate directory levels in a path string that reflects a hierarchical file system
      /// organization.
      /// </summary>
      /// <value>
      /// The directory separator character.
      /// </value>
      Char DirectorySeparatorCharacter { get; }

      /// <summary>
      /// Provides the platform-specific directory separator as a string to simplify casting
      /// issues.
      /// </summary>
      /// <value>
      /// The directory separator as a string.
      /// </value>
      string DirectorySeparatorString { get; }

      /// <summary>
      /// A platform-specific separator character used to separate path strings in environment variables.
      /// </summary>
      /// <value>
      /// The path separator.
      /// </value>
      Char PathSeparatorCharacter { get; }

      /// <summary>
      /// Provides a platform-specific volume separator character.
      /// </summary>
      /// <value>
      /// The volume separator character.
      /// </value>
      Char VolumeSeparatorCharacter { get; }

      /// <summary>
      /// Changes the extension of a path string.
      /// </summary>
      /// <param name="path">
      /// The path information to modify. The path cannot contain any of the characters defined in
      /// <see cref="GetInvalidPathCharacters"/>.
      /// </param>
      /// <param name="extension">
      /// The new extension (with or without a leading period). Specify <c>null</c> to remove an existing extension from <paramref name="path"/>.
      /// </param>
      /// <returns>
      /// The modified path information.
      /// On Windows-based desktop platforms, if <paramref name="path"/> is <c>null</c> or an empty string (""), the path information is returned unmodified.
      /// If <paramref name="extension"/> is <c>null</c>, the returned string contains the specified path with its extension removed.
      /// If <paramref name="path"/> has no extension, and <paramref name="extension"/> is not <c>null</c>,
      /// the returned path string contains <paramref name="extension"/> appended to the end of <paramref name="path"/>.
      /// </returns>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> contains one or more of the invalid characters defined in
      /// <see cref="GetInvalidPathCharacters"/>.
      /// </exception>
      String ChangeExtension(String path, String extension);

      /// <summary>
      /// Combines an array of strings into a path.
      /// </summary>
      /// <param name="paths">
      /// An array of parts of the path.
      /// </param>
      /// <returns>
      /// The combined paths as an minimally validated string.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// One of the strings in the array is <c>null</c>.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// One of the strings in the array contains one or more of the invalid characters defined in
      /// <see cref="GetInvalidPathCharacters"/>.
      /// </exception>
      String Combine(params String[] paths);

      /// <summary>
      /// Returns the extension of the specified path string.
      /// </summary>
      /// <param name="path">
      /// The path string from which to get the extension.
      /// </param>
      /// <returns>
      /// The extension of the specified path (including the period "."), or <c>null</c>, or <see cref="String.Empty"/>. If
      /// <paramref name="path"/> is <c>null</c>, <see cref="IPathUtilities.GetExtension(String)"/> returns <c>null</c>. If <paramref name="path"/> does
      /// not have extension information,
      /// <see cref="IPathUtilities.GetExtension(String)"/> returns <see cref="String.Empty"/>.
      /// </returns>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> contains one or more of the invalid characters defined in
      /// <see cref="GetInvalidPathCharacters"/>.
      /// </exception>
      String GetExtension(String path);

      /// <summary>
      /// Returns the file name and extension of the specified path string.
      /// </summary>
      /// <param name="path">
      /// The path string from which to obtain the file name and extension.
      /// </param>
      /// <returns>
      /// The characters after the last directory character in <paramref name="path"/>. If the last character of
      /// <paramref name="path"/> is a directory or volume separator character, this method returns
      /// <see cref="String.Empty"/>.
      /// If <paramref name="path"/> is <c>null</c>, this method returns <c>null</c>.
      /// </returns>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> contains one or more of the invalid characters defined in
      /// <see cref="GetInvalidPathCharacters"/>.
      /// </exception>
      String GetFileName(String path);

      /// <summary>
      /// Returns the file name of the specified path string without the extension.
      /// </summary>
      /// <param name="path">
      /// The path of the file.
      /// </param>
      /// <returns>
      /// The string returned by <see cref="Path.GetFileName(String)"/>, minus the last period (.) and all characters
      /// following it.
      /// </returns>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> contains one or more of the invalid characters defined in
      /// <see cref="GetInvalidPathCharacters"/>.
      /// </exception>
      String GetFileNameWithoutExtension(String path);

      /// <summary>
      /// Returns the absolute path for the specified path string.
      /// </summary>
      /// <paramref name="path"/> contains a colon (":") that is not part of a volume identifier (for
      /// example, "c:\").
      /// <param name="path">
      /// The file or directory for which to obtain absolute path information.
      /// </param>
      /// <returns>
      /// The fully qualified location of <paramref name="path"/>, such as "C:\MyFile.txt".
      /// </returns>
      /// <remarks>
      /// Always returns a value using <see cref="DirectorySeparatorCharacter"/> and never <see cref="AltDirectorySeparatorCharacter"/>.
      /// </remarks>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is <c>null</c>.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
      /// more of the invalid characters defined in <see cref="GetInvalidPathCharacters"/>.
      /// -or-
      /// The system could not retrieve the absolute path.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permissions.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// </exception>
      String GetFullPath(String path);

      /// <summary>
      /// Gets an array containing the characters that are not allowed in file names.
      /// </summary>
      /// <returns>
      /// A set of characters that are not allowed in file names.
      /// </returns>
      IImmutableSet<Char> GetInvalidFileNameCharacters();

      /// <summary>
      /// Gets an array containing the characters that are not allowed in path names.
      /// </summary>
      /// <returns>
      /// A set of characters that are not allowed in path names.
      /// </returns>
      IImmutableSet<Char> GetInvalidPathCharacters();

      /// <summary>
      /// Returns the parent path for the specified path string.
      /// </summary>
      /// <param name="path">
      /// The path of a file or directory or share.
      /// </param>
      /// <returns>
      /// The parent directory path for <paramref name="path"/>;
      /// or <c>null</c> when <paramref name="path"/> denotes a root directory, 
      /// or <see cref="DirectorySeparatorCharacter"/> when <paramref name="path"/> is a relative root.
      /// or <see cref="String.Empty"/> when <paramref name="path"/> does not contain path information.
      /// </returns>
      /// <remarks>
      /// |  Path                   |  Path.GetDirectoryName | IPathUtilities.GetParentPath |
      /// | :---------------------- | :--------------------- | :--------------------------- |
      /// | \\share                 | (null)                 | (null)                       |
      /// | \\share\file.txt        | (null)                 | \\share          ***         |
      /// | \\share\folder\file.txt | \\share\folder         | \\share\folder               |
      /// | c:\                     | (null)                 | (null)                       |
      /// | c:\file.txt             | c:\                    | c:\                          |
      /// | c:\folder\file.txt      | c:\folder              | c:\folder                    |
      /// | \folder                 | \                      | \                            |
      /// | \folder\file.txt        | \folder                | \folder                      |
      ///
      /// </remarks>
      /// <exception cref="ArgumentException">
      /// The <paramref name="path"/> parameter contains invalid characters, is empty, or contains only white spaces.
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The <paramref name="path"/> parameter is longer than the system-defined maximum length.
      /// </exception>
      String GetParentPath(String path);

      /// <summary>
      /// Gets the root directory information of the specified path.
      /// </summary>
      /// <param name="path">
      /// The path from which to obtain root directory information.
      /// </param>
      /// <returns>
      /// <para>
      /// The root directory of <paramref name="path"/>, such as "C:\", or <c>null</c> if <paramref name="path"/> is <c>null</c>, or an empty string if <paramref name="path"/> does not contain
      /// root directory information.
      /// </para>
      /// <para>
      /// By design, return <see cref="DirectorySeparatorCharacter"/> or <see cref="AltDirectorySeparatorCharacter"/> if <paramref name="path"/> is a relative path.
      /// </para>
      /// </returns>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> contains one or more of the invalid characters defined in
      /// <see cref="GetInvalidPathCharacters"/>.
      /// -or-
      /// <see cref="String.Empty"/> was passed to <paramref name="path"/>.
      /// </exception>
      String GetRootPath(String path);

      /// <summary>
      /// Determines whether a path includes a file name extension.
      /// </summary>
      /// <param name="path">
      /// The path to search for an extension.
      /// </param>
      /// <returns>
      /// true if the characters that follow the last directory separator (\\ or /) or volume separator (:) in the path include a period (.)
      /// followed by one or more characters; otherwise, false.
      /// </returns>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> contains one or more of the invalid characters defined in
      /// <see cref="GetInvalidPathCharacters"/>.
      /// </exception>
      Boolean HasExtension(String path);

      /// <summary>
      /// Gets a value indicating whether the specified path string is a relative path.  
      /// </summary>
      /// <param name="path">
      /// The path to test.
      /// </param>
      /// <returns>
      /// true if <paramref name="path"/> is a relative path; otherwise, false (UNC and rooted paths).
      /// </returns>
      /// <exception cref="ArgumentException">
      /// <paramref name="path"/> contains one or more of the invalid characters defined in
      /// <see cref="GetInvalidPathCharacters"/>.
      /// </exception>
      Boolean IsPathRelative(String path);
   }
}
