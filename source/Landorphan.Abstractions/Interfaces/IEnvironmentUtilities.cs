namespace Landorphan.Abstractions.Interfaces
{
   using System;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Security;
   using System.Security.Permissions;
   using Landorphan.Common.Exceptions;

   /// <summary>
   /// Provides information about, and means to manipulate, the current environment and platform.
   /// </summary>
   public interface IEnvironmentUtilities
   {
      /// <summary>
      /// Gets a <see cref="Version"/> Object that describes the major, minor, build, and revision numbers of the common language
      /// runtime.
      /// </summary>
      /// <value>
      /// An Object that displays the version of the common language runtime.
      /// </value>
      Version ClrVersion { get; }

      /// <summary>
      /// Gets the command line for this process.
      /// </summary>
      /// <value>
      /// A String containing command-line arguments.
      /// </value>
      String CommandLine { get; }

      /// <summary>
      /// Gets a unique identifier for the current managed thread.
      /// </summary>
      /// <value>
      /// An integer that represents a unique identifier for this managed thread.
      /// </value>
      Int32 CurrentManagedThreadId { get; }

      /// <summary>
      /// Gets or sets the exit code of the process.
      /// </summary>
      /// <value>
      /// A 32-bit signed integer containing the exit code. The default value is zero.
      /// </value>
      Int32 ExitCode { get; set; }

      /// <summary>
      /// Gets a value indicating whether the common language runtime (CLR) is shutting down.
      /// </summary>
      /// <value>
      /// <c> true </c> if the CLR is shutting down; otherwise, <c> false </c>.
      /// </value>
      Boolean HasShutdownStarted { get; }

      /// <summary>
      /// Determines whether the current operating system is a 64-bit operating system.
      /// </summary>
      /// <value>
      /// <c>
      ///    <c> true </c>
      /// </c>
      /// if the operating system is 64-bit; otherwise, <c> false </c>.
      /// </value>
      Boolean Is64BitOperatingSystem { get; }

      /// <summary>
      /// Determines whether the current process is a 64-bit process.
      /// </summary>
      /// <value>
      /// <c> true </c> if the process is 64-bit; otherwise, <c> false </c>.
      /// </value>
      Boolean Is64BitProcess { get; }

      /// <summary>
      /// Gets the NetBIOS name of this local computer.
      /// </summary>
      /// <exception cref="InvalidOperationException">
      /// The name of this computer cannot be obtained.
      /// </exception>
      /// <value>
      /// A String containing the name of this computer.
      /// </value>
      String MachineName { get; }

      /// <summary>
      /// Gets the newline String defined for this environment.
      /// </summary>
      /// <value>
      /// A String containing "\r\n" for non-Unix platforms, or a String containing "\n" for Unix platforms.
      /// </value>
      String NewLine { get; }

      /// <summary>
      /// Gets an <see cref="OperatingSystem"/> Object that contains the current platform identifier and version number.
      /// </summary>
      /// <exception cref="InvalidOperationException">
      /// This property was unable to obtain the system version.
      /// -or-
      /// The obtained platform identifier is not a member of <see cref="PlatformID"/>
      /// </exception>
      /// <value>
      /// An Object that contains the platform identifier and version number.
      /// </value>
      [SuppressMessage("SonarLint.CodeSmell", "S100: Methods and properties should be named in PascalCase")]
      OperatingSystem OSVersion { get; }

      /// <summary>
      /// Gets the number of processors on the current machine.
      /// </summary>
      /// <value>
      /// The 32-bit signed integer that specifies the number of processors on the current machine. There is no default. If the current machine
      /// contains multiple processor groups, this property returns the number of logical processors that are available for use by the common
      /// language runtime (CLR).
      /// </value>
      Int32 ProcessorCount { get; }

      /// <summary>
      /// Gets current stack trace information.
      /// </summary>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The requested stack trace information is out of range.
      /// </exception>
      /// <value>
      /// A String containing stack trace information. This value can be <see cref="String.Empty"/>.
      /// </value>
      String StackTrace { get; }

      /// <summary>
      /// Gets the fully qualified path of the system directory.
      /// </summary>
      /// <value>
      /// A String containing a directory path.
      /// </value>
      /// <remarks>
      /// Usually C:\WINDOWS\system32
      /// </remarks>
      String SystemDirectory { get; }

      /// <summary>
      /// Gets the amount of memory for an operating system's page file.
      /// </summary>
      /// <value>
      /// The number of bytes in a system page file.
      /// </value>
      Int32 SystemPageSizeBytes { get; }

      /// <summary>
      /// Gets the network domain name associated with the current user.
      /// </summary>
      /// <exception cref="PlatformNotSupportedException">
      /// The operating system does not support retrieving the network domain name.
      /// </exception>
      /// <exception cref="InvalidOperationException">
      /// The network domain name cannot be retrieved.
      /// </exception>
      /// <value>
      /// The network domain name associated with the current user.
      /// </value>
      String UserDomainName { get; }

      /// <summary>
      /// Gets a value indicating whether the current process is running in user interactive mode.
      /// </summary>
      /// <value>
      /// <c> true </c> if the current process is running in user interactive mode; otherwise, <c> false </c>.
      /// </value>
      Boolean UserInteractive { get; }

      /// <summary>
      /// Gets the user name of the person who is currently logged on to the Windows operating system.
      /// </summary>
      /// <value>
      /// The user name of the person who is logged on to Windows.
      /// </value>
      String UserName { get; }

      /// <summary>
      /// Gets the amount of physical memory mapped to the process context.
      /// </summary>
      /// <value>
      /// A 64-bit signed integer containing the number of bytes of physical memory mapped to the process context.
      /// </value>
      Int64 WorkingSetBytes { get; }

      /// <summary>
      /// Terminates this process and gives the underlying operating system the specified exit code.
      /// </summary>
      /// <exception cref="SecurityException">
      /// The caller does not have sufficient security permission to perform this function.
      /// </exception>
      /// <param name="exitCode">
      /// Exit code to be given to the operating system.
      /// </param>
      [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
      [SuppressMessage(
         "Microsoft.Naming",
         "CA1716:IdentifiersShouldNotMatchKeywords",
         MessageId = "Exit",
         Justification = "Matching underlying implementation(MWP)")]
      void Exit(Int32 exitCode);

      /// <summary>
      /// Replaces the name of each environment variable embedded in the specified String with the String equivalent of the value of the
      /// variable, then returns the resulting String.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="name"/> is null.
      /// </exception>
      /// <param name="name">
      /// A String containing the names of zero or more environment variables. Each environment variable is quoted with the percent
      /// sign character (%).
      /// </param>
      /// <returns>
      /// A String with each environment variable replaced by its value.
      /// </returns>
      String ExpandEnvironmentVariables(String name);

      /// <summary>
      /// Immediately terminates a process after writing a message to the Windows Application event log, and then includes the message in
      /// error reporting to Microsoft.
      /// </summary>
      /// <param name="message">
      /// A message that explains why the process was terminated, or null if no explanation is provided.
      /// </param>
      void FailFast(String message);

      /// <summary>
      /// Immediately terminates a process after writing a message to the Windows Application event log, and then includes the message and
      /// exception information in error reporting to Microsoft.
      /// </summary>
      /// <param name="message">
      /// A message that explains why the process was terminated, or null if no explanation is provided.
      /// </param>
      /// <param name="exception">
      /// An exception that represents the error that caused the termination. This is typically the exception in a catch block.
      /// </param>
      void FailFast(String message, Exception exception);

      /// <summary>
      /// Returns a String array containing the command-line arguments for the current process.
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The system does not support command-line arguments.
      /// </exception>
      /// <returns>
      /// An array of String where each element contains a command-line argument. The first element is the executable file name, and the
      /// following zero or more elements contain the remaining command-line arguments.
      /// </returns>
      String[] GetCommandLineArgs();

      /// <summary>
      /// Retrieves the value of an environment variable from the current process.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="variable"/> is null.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permission to perform this operation.
      /// </exception>
      /// <param name="variable">
      /// The name of the environment variable.
      /// </param>
      /// <returns>
      /// The value of the environment variable specified by <paramref name="variable"/>, or null if the environment variable is not found.
      /// </returns>
      String GetEnvironmentVariable(String variable);

      /// <summary>
      /// Retrieves the value of an environment variable from the current process or from the Windows operating system registry key for the
      /// current user or local machine.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="variable"/> is null.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="target"/> is <see cref="EnvironmentVariableTarget.User"/> or
      /// <see cref="EnvironmentVariableTarget.Machine"/> and the current operating system is Windows
      /// 95, Windows 98, or Windows Me.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="target"/> is not a valid <see cref="EnvironmentVariableTarget"/> value.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permission to perform this operation.
      /// </exception>
      /// <param name="variable">
      /// The name of an environment variable.
      /// </param>
      /// <param name="target">
      /// One of the <see cref="EnvironmentVariableTarget"/> values.
      /// </param>
      /// <returns>
      /// The value of the environment variable specified by the <paramref name="variable"/> and <paramref name="target"/> parameters, or null
      /// if the environment variable is not found.
      /// </returns>
      String GetEnvironmentVariable(String variable, EnvironmentVariableTarget target);

      /// <summary>
      /// Retrieves all environment variable names and their values from the current process.
      /// </summary>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permission to perform this operation.
      /// </exception>
      /// <exception cref="OutOfMemoryException">
      /// The buffer is out of memory.
      /// </exception>
      /// <returns>
      /// A dictionary that contains all environment variable names and their values; otherwise, an empty dictionary if no environment
      /// variables are found.
      /// </returns>
      IImmutableSet<IEnvironmentVariable> GetEnvironmentVariables();

      /// <summary>
      /// Retrieves all environment variable names and their values from the current process, or from the Windows operating system registry
      /// key for the current user or local machine.
      /// </summary>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permission to perform this operation for the specified value of
      /// <paramref name="target"/>.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// This method cannot be used on Windows 95 or Windows 98 platforms.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="target"/> contains an illegal value.
      /// </exception>
      /// <param name="target">
      /// One of the <see cref="EnvironmentVariableTarget"/> values.
      /// </param>
      /// <returns>
      /// A dictionary that contains all environment variable names and their values from the source specified by the
      /// <paramref name="target"/> parameter; otherwise, an empty dictionary if no environment variables are found.
      /// </returns>
      IImmutableSet<IEnvironmentVariable> GetEnvironmentVariables(EnvironmentVariableTarget target);

      /// <summary>
      /// Returns an array of String containing the names of the logical drives on the current computer.
      /// </summary>
      /// <exception cref="IOException">
      /// An I/O error occurs.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permissions.
      /// </exception>
      /// <returns>
      /// An array of strings where each element contains the name of a logical drive. For example, if the computer's hard drive is the first
      /// logical drive, the first element returned is "C:\".
      /// </returns>
      String[] GetLogicalDrives();

      /// <summary>
      /// Gets the path to the system special directory that is identified by the specified enumeration.
      /// </summary>
      /// <exception cref="ArgumentException">
      /// <paramref name="specialFolder"/> is not a member of <see cref="Environment.SpecialFolder"/>.
      /// </exception>
      /// <exception cref="PlatformNotSupportedException">
      /// The current platform is not supported.
      /// </exception>
      /// <param name="specialFolder">
      /// An enumerated constant that identifies a system special directory.
      /// </param>
      /// <returns>
      /// The path to the specified system special directory, if that directory physically exists on your computer; otherwise, an empty String ("").
      /// A directory will not physically exist if the operating system did not create it, the existing directory was deleted, or the directory is a
      /// virtual directory, such as My Computer, which does not correspond to a physical path.
      /// </returns>
      String GetSpecialFolderPath(Environment.SpecialFolder specialFolder);

      /// <summary>
      /// Gets the path to the system special directory that is identified by the specified enumeration, and uses a specified option for
      /// accessing special folders.
      /// </summary>
      /// <exception cref="PlatformNotSupportedException">
      /// </exception>
      /// <param name="specialFolder">
      /// An enumerated constant that identifies a system special directory.
      /// </param>
      /// <param name="option">
      /// Specifies options to use for accessing a special directory.
      /// </param>
      /// <returns>
      /// The path to the specified system special directory, if that directory physically exists on your computer; otherwise, an empty String ("").
      /// A directory will not physically exist if the operating system did not create it, the existing directory was deleted, or the directory is a
      /// virtual directory, such as My Computer, which does not correspond to a physical path.
      /// </returns>
      /// <exception cref="ExtendedInvalidEnumArgumentException">
      /// <paramref name="specialFolder"/> is not a member of <see cref="Environment.SpecialFolder"/>
      /// -or-
      /// <paramref name="option"/> is not a member of <see cref="Environment.SpecialFolderOption"/>
      /// </exception>
      [SuppressMessage(
         "Microsoft.Naming",
         "CA1716:IdentifiersShouldNotMatchKeywords",
         MessageId = "Option",
         Justification = "Matching underlying implementation(MWP)")]
      String GetSpecialFolderPath(Environment.SpecialFolder specialFolder, Environment.SpecialFolderOption option);

      /// <summary>
      /// Returns the path of the current user's temporary directory.
      /// </summary>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permissions.
      /// </exception>
      /// <returns>
      /// The path to the temporary directory, ending with a backslash.
      /// </returns>
      [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      String GetTemporaryDirectoryPath();

      /// <summary>
      /// Creates, modifies, or deletes an environment variable stored in the current process.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="variable"/> is null.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="variable"/> contains a zero-length String, an initial hexadecimal zero character (0x00),
      /// or an equal sign ("=").
      /// -or-
      /// The length of <paramref name="variable"/> or <paramref name="value"/> is greater than or equal to 32, 767
      /// characters.
      /// -or-
      /// An error occurred during the execution of this operation.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permission to perform this operation.
      /// </exception>
      /// <param name="variable">
      /// The name of an environment variable.
      /// </param>
      /// <param name="value">
      /// A value to assign to <paramref name="variable"/>.
      /// </param>
      void SetEnvironmentVariable(String variable, String value);

      /// <summary>
      /// Creates, modifies, or deletes an environment variable stored in the current process or in the Windows operating system registry key
      /// reserved for the current user or local machine.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="variable"/> is null.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// <paramref name="variable"/> contains a zero-length String, an initial hexadecimal zero character (0x00),
      /// or an equal sign ("=").
      /// -or-
      /// The length of <paramref name="variable"/> is greater than or equal to 32,767 characters.
      /// -or-
      /// <paramref name="target"/> is not a member of the <see cref="EnvironmentVariableTarget"/>
      /// enumeration.
      /// -or-<paramref name="target"/> is <see cref="EnvironmentVariableTarget.Machine"/> or
      /// <see cref="EnvironmentVariableTarget.User"/> and the length of <paramref name="variable"/> is
      /// greater than or equal to 255.
      /// -or-
      /// <paramref name="target"/> is <see cref="EnvironmentVariableTarget.Process"/> and the length of
      /// <paramref name="value"/> is greater than or equal to 32,767 characters. -or-An error occurred during the
      /// execution of this operation.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// <paramref name="target"/> is <see cref="EnvironmentVariableTarget.User"/> or
      /// <see cref="EnvironmentVariableTarget.Machine"/> and the current operating system is Windows
      /// 95, Windows 98, or Windows Me.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The caller does not have the required permission to perform this operation.
      /// </exception>
      /// <param name="variable">
      /// The name of an environment variable.
      /// </param>
      /// <param name="value">
      /// A value to assign to <paramref name="variable"/>.
      /// </param>
      /// <param name="target">
      /// One of the <see cref="EnvironmentVariableTarget"/> values.
      /// </param>
      void SetEnvironmentVariable(String variable, String value, EnvironmentVariableTarget target);
   }
}
