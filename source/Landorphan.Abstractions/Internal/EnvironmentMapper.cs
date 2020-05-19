namespace Landorphan.Abstractions.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Landorphan.Abstractions.Interfaces;
    using Landorphan.Common;
    using Landorphan.Common.Exceptions;

    /// <summary>
    /// Provides information about, and means to manipulate, the current environment and platform.
    /// </summary>
    /// <remarks>
    /// Provides a one-to-one mapping to <see cref="Environment"/> but in an Object instance (as opposed to static) to support testability.
    /// </remarks>
    internal sealed class EnvironmentInternalMapping : IEnvironmentUtilities
    {
        /// <inheritdoc/>
        public Version ClrVersion => Environment.Version;

        /// <inheritdoc/>
        public string CommandLine => Environment.CommandLine;

        /// <inheritdoc/>
        public int CurrentManagedThreadId => Environment.CurrentManagedThreadId;

        /// <inheritdoc/>
        public int ElapsedSinceSystemStartupMS => Environment.TickCount;

        /// <inheritdoc/>
        public int ExitCode
        {
            get => Environment.ExitCode;
            set => Environment.ExitCode = value;
        }

        /// <inheritdoc/>
        public bool HasShutdownStarted => Environment.HasShutdownStarted;

        /// <inheritdoc/>
        public bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;

        /// <inheritdoc/>
        public bool Is64BitProcess => Environment.Is64BitProcess;

        /// <inheritdoc/>
        public string MachineName => Environment.MachineName;

        /// <inheritdoc/>
        public string NewLine => Environment.NewLine;

        /// <inheritdoc/>
        public OperatingSystem OSVersion => Environment.OSVersion;

        /// <inheritdoc/>
        public int ProcessorCount => Environment.ProcessorCount;

        /// <inheritdoc/>
        public string StackTrace => Environment.StackTrace;

        /// <inheritdoc/>
        public string SystemDirectory => Environment.SystemDirectory;

        /// <inheritdoc/>
        public int SystemPageSizeBytes => Environment.SystemPageSize;

        /// <inheritdoc/>
        public string UserDomainName => Environment.UserDomainName;

        /// <inheritdoc/>
        public bool UserInteractive => Environment.UserInteractive;

        /// <inheritdoc/>
        public string UserName => Environment.UserName;

        /// <inheritdoc/>
        public long WorkingSetBytes => Environment.WorkingSet;

        /// <inheritdoc/>
        [SuppressMessage("SonarLint.CodeSmell", "S1147: Exit methods should not be called")]
        public void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        /// <inheritdoc/>
        public string ExpandEnvironmentVariables(string name)
        {
            return Environment.ExpandEnvironmentVariables(name);
        }

        /// <inheritdoc/>
        public void FailFast(string message)
        {
            Environment.FailFast(message);
        }

        /// <inheritdoc/>
        public void FailFast(string message, Exception exception)
        {
            Environment.FailFast(message, exception);
        }

        /// <inheritdoc/>
        public string[] GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }

        /// <inheritdoc/>
        public string GetEnvironmentVariable(string variable)
        {
            variable.ArgumentNotNull(nameof(variable));

            return Environment.GetEnvironmentVariable(variable.Trim());
        }

        /// <inheritdoc/>
        public string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            if (!Enum.IsDefined(typeof(EnvironmentVariableTarget), target))
            {
                throw new ExtendedInvalidEnumArgumentException(nameof(target), (long)target, typeof(EnvironmentVariableTarget));
            }

            variable.ArgumentNotNull(nameof(variable));

            return Environment.GetEnvironmentVariable(variable.Trim(), target);
        }

        /// <inheritdoc/>
        public IImmutableSet<IEnvironmentVariable> GetEnvironmentVariables()
        {
            var builder = ImmutableHashSet<IEnvironmentVariable>.Empty.ToBuilder();

            var orig = Environment.GetEnvironmentVariables().OfType<DictionaryEntry>();
            foreach (var entry in orig)
            {
                builder.Add(new EnvironmentVariable(entry.Key.ToString(), entry.Value?.ToString()));
            }

            return builder.ToImmutable();
        }

        /// <inheritdoc/>
        public IImmutableSet<IEnvironmentVariable> GetEnvironmentVariables(EnvironmentVariableTarget target)
        {
            if (!Enum.IsDefined(typeof(EnvironmentVariableTarget), target))
            {
                throw new ExtendedInvalidEnumArgumentException(nameof(target), (long)target, typeof(EnvironmentVariableTarget));
            }

            var builder = ImmutableHashSet<IEnvironmentVariable>.Empty.ToBuilder();

            var orig = Environment.GetEnvironmentVariables().OfType<DictionaryEntry>();
            foreach (var entry in orig)
            {
                builder.Add(new EnvironmentVariable(entry.Key.ToString(), entry.Value.ToString()));
            }

            return builder.ToImmutable();
        }

        /// <inheritdoc/>
        public string[] GetLogicalDrives()
        {
            return Environment.GetLogicalDrives();
        }

        /// <inheritdoc/>
        public string GetSpecialFolderPath(Environment.SpecialFolder specialFolder)
        {
            return GetSpecialFolderPath(specialFolder, Environment.SpecialFolderOption.None);
        }

        /// <inheritdoc/>
        public string GetSpecialFolderPath(Environment.SpecialFolder specialFolder, Environment.SpecialFolderOption option)
        {
            if (!Enum.IsDefined(typeof(Environment.SpecialFolder), specialFolder))
            {
                throw new ExtendedInvalidEnumArgumentException(nameof(specialFolder), (long)specialFolder, typeof(Environment.SpecialFolder));
            }

            if (!Enum.IsDefined(typeof(Environment.SpecialFolderOption), option))
            {
                throw new ExtendedInvalidEnumArgumentException(nameof(option), (long)option, typeof(Environment.SpecialFolderOption));
            }

            return Environment.GetFolderPath(specialFolder, option);
        }

        /// <inheritdoc/>
        public void SetEnvironmentVariable(string variable, string value)
        {
            variable.ArgumentNotNull(nameof(variable));
            variable = variable.Trim();

            Environment.SetEnvironmentVariable(variable, value);
        }

        /// <inheritdoc/>
        public void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            variable.ArgumentNotNull(nameof(variable));
            variable = variable.Trim();

            Environment.SetEnvironmentVariable(variable, value, target);
        }
    }
}
