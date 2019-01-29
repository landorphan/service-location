namespace Landorphan.Abstractions.IO.Interfaces
{
   using System;
   using System.Collections.Immutable;
   using System.IO;
   using System.Security;
   using System.Text;

   /// <summary>
   /// Represents methods for reading from files.
   /// </summary>
   public interface IFileReaderUtilities
   {
      /// <summary>
      /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
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
      /// <exception cref="DirectoryNotFoundException">
      /// The specified path is invalid (for example, it is on an unmapped drive).
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred while opening the file.
      /// </exception>
      /// <exception cref="UnauthorizedAccessException">
      /// This operation is not supported on the current platform.-or- <paramref name="path"/>
      /// specified a directory.-or- The caller does not have the required permission.
      /// </exception>
      /// <exception cref="FileNotFoundException">
      /// The file specified in <paramref name="path"/> was not found.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permission.
      /// </exception>
      /// <param name="path">
      /// The file to open for reading.
      /// </param>
      /// <returns>
      /// A list of byte containing the contents of the file.
      /// </returns>
      IImmutableList<Byte> ReadAllBytes(String path);

      /// <summary>
      /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
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
      /// <exception cref="DirectoryNotFoundException">
      /// The specified path is invalid (for example, it is on an unmapped drive).
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred while opening the file.
      /// </exception>
      /// <exception cref="UnauthorizedAccessException">
      /// <paramref name="path"/> specified a file that is read-only.-or- This operation is not
      /// supported on the current platform.-or-
      /// <paramref name="path"/> specified a directory.-or- The caller does not have the
      /// required permission.
      /// </exception>
      /// <exception cref="FileNotFoundException">
      /// The file specified in <paramref name="path"/> was not found.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permission.
      /// </exception>
      /// <param name="path">
      /// The file to open for reading.
      /// </param>
      /// <param name="encoding">
      /// The encoding applied to the contents of the file.
      /// </param>
      /// <returns>
      /// A list containing all lines of the file.
      /// </returns>
      IImmutableList<String> ReadAllLines(String path, Encoding encoding);

      /// <summary>
      /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
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
      /// <exception cref="DirectoryNotFoundException">
      /// The specified path is invalid (for example, it is on an unmapped drive).
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred while opening the file.
      /// </exception>
      /// <exception cref="UnauthorizedAccessException">
      /// <paramref name="path"/> specified a file that is read-only.-or- This operation is not
      /// supported on the current platform.-or-
      /// <paramref name="path"/> specified a directory.-or- The caller does not have the
      /// required permission.
      /// </exception>
      /// <exception cref="FileNotFoundException">
      /// The file specified in <paramref name="path"/> was not found.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="path"/> is in an invalid format.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permission.
      /// </exception>
      /// <param name="path">
      /// The file to open for reading.
      /// </param>
      /// <param name="encoding">
      /// The encoding applied to the contents of the file.
      /// </param>
      /// <returns>
      /// A string containing all lines of the file.
      /// </returns>
      String ReadAllText(String path, Encoding encoding);
   }
}
