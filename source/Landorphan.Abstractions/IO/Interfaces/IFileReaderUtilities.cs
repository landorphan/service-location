namespace Landorphan.Abstractions.IO.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Security;
    using System.Text;
    using Landorphan.Common.Exceptions;

    /// <summary>
    /// Represents methods for reading from files.
    /// </summary>
    public interface IFileReaderUtilities
    {
        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path with read/write access with no sharing.
        /// </summary>
        /// <param name="path">
        /// The file to open.
        /// </param>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.
        /// </param>>
        /// <returns>
        /// A <see cref="FileMode"/> opened in the specified mode and path, with read/write access and not shared.
        /// </returns>
        /// <exception cref="ArgumentException">      
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.
        /// </exception>
        /// <exception cref="ArgumentNullException">      
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified <paramref name="path"/>, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified <paramref name="path"/> is invalid, (for example, it is on an unmapped drive). 
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurred while opening the file. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// -or-
        /// mode is <see cref="FileMode.Create"/> and the specified file is a hidden file.
        /// </exception>
        /// <exception cref="ExtendedInvalidEnumArgumentException">
        /// <paramref name="mode"/> specified an invalid value.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The file specified in path was not found.
        /// </exception>   
        /// <exception cref="NotSupportedException">
        /// <paramref name="path"/> is in an invalid format.
        /// </exception>      
        FileStream Open(string path, FileMode mode);

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path, with the specified mode and access with no sharing.
        /// </summary>
        /// <param name="path">
        /// The file to open.
        /// </param>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.
        /// </param>>
        /// <param name="access">
        /// A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file
        /// </param>
        /// <returns>
        /// An unshared <see cref="FileStream"/> that provides access to the specified file, with the specified mode and access.
        /// </returns>
        /// <exception cref="ArgumentException">      
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.
        /// </exception>
        /// <exception cref="ArgumentNullException">      
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified <paramref name="path"/>, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified <paramref name="path"/> is invalid, (for example, it is on an unmapped drive). 
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurred while opening the file. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// -or-
        /// mode is <see cref="FileMode.Create"/> and the specified file is a hidden file.
        /// </exception>
        /// <exception cref="ExtendedInvalidEnumArgumentException">
        /// <paramref name="mode"/> specified an invalid value.
        /// -or-
        /// <paramref name="access"/> specified an invalid value.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The file specified in path was not found.
        /// </exception>   
        /// <exception cref="NotSupportedException">
        /// <paramref name="path"/> is in an invalid format.
        /// </exception>   
        FileStream Open(string path, FileMode mode, FileAccess access);

        /// <summary>
        /// Opens a <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// </summary>
        /// <param name="path">
        /// The file to open.
        /// </param>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.
        /// </param>>
        /// <param name="access">
        /// A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file
        /// </param>
        /// <param name="share">
        /// A <see cref="FileShare"/> value specifying the type of access other threads have to the file.
        /// </param>
        /// <returns>
        /// A <see cref="FileStream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// </returns>
        /// <exception cref="ArgumentException">      
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.
        /// </exception>
        /// <exception cref="ArgumentNullException">      
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified <paramref name="path"/>, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified <paramref name="path"/> is invalid, (for example, it is on an unmapped drive). 
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurred while opening the file. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// -or-
        /// mode is <see cref="FileMode.Create"/> and the specified file is a hidden file.
        /// </exception>
        /// <exception cref="ExtendedInvalidEnumArgumentException">
        /// <paramref name="mode"/> specified an invalid value.
        /// -or-
        /// <paramref name="access"/> specified an invalid value.
        /// -or-
        /// <paramref name="share"/> specified an invalid value.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The file specified in path was not found.
        /// </exception>   
        /// <exception cref="NotSupportedException">
        /// <paramref name="path"/> is in an invalid format.
        /// </exception>   
        FileStream Open(string path, FileMode mode, FileAccess access, FileShare share);

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        /// <param name="path">
        /// The file to be opened for reading.
        /// </param>
        /// <returns>
        /// A read-only <see cref="FileStream"/> on the specified path.
        /// </returns>
        /// <exception cref="ArgumentException">      
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.
        /// </exception>
        /// <exception cref="ArgumentNullException">      
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified <paramref name="path"/>, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified <paramref name="path"/> is invalid, (for example, it is on an unmapped drive). 
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurred while opening the file. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// -or-
        /// mode is <see cref="FileMode.Create"/> and the specified file is a hidden file.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The file specified in path was not found.
        /// </exception>   
        /// <exception cref="NotSupportedException">
        /// <paramref name="path"/> is in an invalid format.
        /// </exception>   
        FileStream OpenRead(string path);

        /// <summary>
        /// Opens an existing UTF-8 encoded text file for reading.
        /// </summary>
        /// <param name="path">
        /// The file to be opened for reading.
        /// </param>
        /// <returns>
        /// A <see cref="StreamReader"/> on the specified path.
        /// </returns>
        /// <exception cref="ArgumentException">      
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.
        /// </exception>
        /// <exception cref="ArgumentNullException">      
        /// <paramref name="path"/> is null.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified <paramref name="path"/>, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified <paramref name="path"/> is invalid, (for example, it is on an unmapped drive). 
        /// </exception>
        /// <exception cref="IOException">
        /// An I/O error occurred while opening the file. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// -or-
        /// mode is <see cref="FileMode.Create"/> and the specified file is a hidden file.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The file specified in path was not found.
        /// </exception>   
        /// <exception cref="NotSupportedException">
        /// <paramref name="path"/> is in an invalid format.
        /// </exception>  
        StreamReader OpenText(string path);

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
        IImmutableList<byte> ReadAllBytes(string path);

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
        IImmutableList<string> ReadAllLines(string path, Encoding encoding);

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
        string ReadAllText(string path, Encoding encoding);

        /// <summary>
        /// Opens a text file with the specified <paramref name="encoding"/>, reads all lines of the file into a string array, and then closes the file.
        /// </summary>
        /// <param name="path">
        /// The file to open for reading.
        /// </param>
        /// <param name="encoding">
        /// The encoding applied to the contents of the file.
        /// </param>
        /// <returns>
        /// A non-null collection of containing all lines of the file specified by <paramref name="path"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
        /// more invalid characters as defined by
        /// <see cref="IPathUtilities.GetInvalidPathCharacters"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="encoding"/> is null.
        /// -or-
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
        IEnumerable<string> ReadLines(string path, Encoding encoding);
    }
}
