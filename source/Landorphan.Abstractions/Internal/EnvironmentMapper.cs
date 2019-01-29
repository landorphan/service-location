namespace Landorphan.Abstractions.Internal
{
   using System;
   using System.Collections;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
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
      public String CommandLine => Environment.CommandLine;

      /// <inheritdoc/>
      public Int32 CurrentManagedThreadId => Environment.CurrentManagedThreadId;

      /// <inheritdoc/>
      public Int32 ElapsedMillisecondsSinceSystemStart => Environment.TickCount;

      /// <inheritdoc/>
      public Int32 ExitCode
      {
         get => Environment.ExitCode;
         set => Environment.ExitCode = value;
      }

      /// <inheritdoc/>
      public Boolean HasShutdownStarted => Environment.HasShutdownStarted;

      /// <inheritdoc/>
      public Boolean Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;

      /// <inheritdoc/>
      public Boolean Is64BitProcess => Environment.Is64BitProcess;

      /// <inheritdoc/>
      public String MachineName => Environment.MachineName;

      /// <inheritdoc/>
      public String NewLine => Environment.NewLine;

      /// <inheritdoc/>
      public OperatingSystem OSVersion => Environment.OSVersion;

      /// <inheritdoc/>
      public Int32 ProcessorCount => Environment.ProcessorCount;

      /// <inheritdoc/>
      public String StackTrace => Environment.StackTrace;

      /// <inheritdoc/>
      public String SystemDirectory => Environment.SystemDirectory;

      /// <inheritdoc/>
      public Int32 SystemPageSizeBytes => Environment.SystemPageSize;

      /// <inheritdoc/>
      public String UserDomainName => Environment.UserDomainName;

      /// <inheritdoc/>
      public Boolean UserInteractive => Environment.UserInteractive;

      /// <inheritdoc/>
      public String UserName => Environment.UserName;

      /// <inheritdoc/>
      public Int64 WorkingSetBytes => Environment.WorkingSet;

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S1147: Exit methods should not be called")]
      public void Exit(Int32 exitCode)
      {
         Environment.Exit(exitCode);
      }

      /// <inheritdoc/>
      public String ExpandEnvironmentVariables(String name)
      {
         return Environment.ExpandEnvironmentVariables(name);
      }

      /// <inheritdoc/>
      public void FailFast(String message)
      {
         Environment.FailFast(message);
      }

      /// <inheritdoc/>
      public void FailFast(String message, Exception exception)
      {
         Environment.FailFast(message, exception);
      }

      /// <inheritdoc/>
      public String[] GetCommandLineArgs()
      {
         return Environment.GetCommandLineArgs();
      }

      /// <inheritdoc/>
      public String GetEnvironmentVariable(String variable)
      {
         variable.ArgumentNotNull(nameof(variable));

         return Environment.GetEnvironmentVariable(variable.Trim());
      }

      /// <inheritdoc/>
      public String GetEnvironmentVariable(String variable, EnvironmentVariableTarget target)
      {
         if (!Enum.IsDefined(typeof(EnvironmentVariableTarget), target))
         {
            throw new ExtendedInvalidEnumArgumentException(nameof(target), (Int64)target, typeof(EnvironmentVariableTarget));
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
            throw new ExtendedInvalidEnumArgumentException(nameof(target), (Int64)target, typeof(EnvironmentVariableTarget));
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
      public String[] GetLogicalDrives()
      {
         return Environment.GetLogicalDrives();
      }

      /// <inheritdoc/>
      public String GetSpecialFolderPath(Environment.SpecialFolder specialFolder)
      {
         return GetSpecialFolderPath(specialFolder, Environment.SpecialFolderOption.None);
      }

      /// <inheritdoc/>
      public String GetSpecialFolderPath(Environment.SpecialFolder specialFolder, Environment.SpecialFolderOption option)
      {
         if (!Enum.IsDefined(typeof(Environment.SpecialFolder), specialFolder))
         {
            throw new ExtendedInvalidEnumArgumentException(nameof(specialFolder), (Int64)specialFolder, typeof(Environment.SpecialFolder));
         }

         if (!Enum.IsDefined(typeof(Environment.SpecialFolderOption), option))
         {
            throw new ExtendedInvalidEnumArgumentException(nameof(option), (Int64)option, typeof(Environment.SpecialFolderOption));
         }

         return Environment.GetFolderPath(specialFolder, option);
      }

      /// <inheritdoc/>
      public String GetTemporaryDirectoryPath()
      {
         return Path.GetTempPath();
      }

      /// <inheritdoc/>
      public void SetEnvironmentVariable(String variable, String value)
      {
         variable.ArgumentNotNull(nameof(variable));
         variable = variable.Trim();

         Environment.SetEnvironmentVariable(variable, value);
      }

      /// <inheritdoc/>
      public void SetEnvironmentVariable(String variable, String value, EnvironmentVariableTarget target)
      {
         variable.ArgumentNotNull(nameof(variable));
         variable = variable.Trim();

         Environment.SetEnvironmentVariable(variable, value, target);
      }
   }
}
