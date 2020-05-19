namespace Landorphan.Abstractions.IO.Interfaces
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    /// <summary>
    /// Represents methods for writing to directories.
    /// </summary>
    public interface IDirectoryWriterUtilities
    {
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
    }
}
