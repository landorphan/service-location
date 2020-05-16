namespace Landorphan.Abstractions.Tests.Internal.Environment
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using FluentAssertions;
    using Landorphan.Abstractions.Interfaces;
    using Landorphan.Abstractions.Internal;
    using Landorphan.Common.Exceptions;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable StringLiteralTypo
   // ReSharper disable InconsistentNaming

   public static class EnvironmentInternalMapping_Tests
   {
       private static readonly EnvironmentInternalMapping _target = new EnvironmentInternalMapping();

       [TestClass]
      public class When_I_call_EnvironmentMapper_ExpandEnvironmentVariables : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_expand_multiple_variables()
         {
            const string format = "My system drive is '{0}' and my system root is '{1}'.";
            var query = string.Format(CultureInfo.InvariantCulture, format, "%SystemDrive%", "%SystemRoot%");
            var actual = _target.ExpandEnvironmentVariables(query);
            actual.Should().Be(Environment.ExpandEnvironmentVariables(query));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_original_string_when_no_variables_are_present()
         {
            const string query = "The quick brown fox.";
            var actual = _target.ExpandEnvironmentVariables(query);
            actual.Should().Be(query);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.ExpandEnvironmentVariables(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("name");
         }
      }

      [TestClass]
      public class When_I_call_EnvironmentMapper_GetCommandLineArgs : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_CommandLineArguments()
         {
            _target.GetCommandLineArgs().Should().BeEquivalentTo(Environment.GetCommandLineArgs());
         }
      }

      [TestClass]
      public class When_I_call_EnvironmentMapper_GetEnvironmentVariable : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_target_is_not_recognized_It_should_throw_ExtendedInvalidEnumArgumentException()
         {
            Action throwingAction = () => _target.GetEnvironmentVariable("windir", (EnvironmentVariableTarget)int.MinValue);
            var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.ParamName.Should().Be("target");
            e.And.Message.Should().Contain("The value of argument '");
            e.And.Message.Should().Contain("is invalid for Enum type '");
            e.And.Message.Should().Contain("EnvironmentVariableTarget");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_variable_has_leading_whitespace_It_should_be_recognized_and_returned()
         {
            // windir changed to PATH so the environment variable would be x-plat
            const string variableName = "   PATH";

            var actual = _target.GetEnvironmentVariable(variableName);
            actual.Should().NotBeNull();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               // PER: https://docs.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable?view=netframework-4.7.2
               // .NET Core on macOS and Linux does not support per-machine or per-user environment variables.
               actual = _target.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);
               actual.Should().NotBeNull();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_variable_has_trailing_whitespace_It_should_be_recognized_and_returned()
         {
            // windir changed to PATH so the environment variable would be x-plat
            const string variableName = "PATH   ";

            var actual = _target.GetEnvironmentVariable(variableName);
            actual.Should().NotBeNull();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               // PER: https://docs.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable?view=netframework-4.7.2
               // .NET Core on macOS and Linux does not support per-machine or per-user environment variables.
               actual = _target.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);
               actual.Should().NotBeNull();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_variable_is_not_recognized_in_the_specified_target_It_should_return_null()
         {
            // ReSharper disable once CommentTypo
            // windir is available in the machine target.
            var actual = _target.GetEnvironmentVariable("windir", EnvironmentVariableTarget.User);
            actual.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_variable_is_not_recognized_It_should_return_null()
         {
            var actual = _target.GetEnvironmentVariable(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
            actual.Should().BeNull();

            actual = _target.GetEnvironmentVariable(Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture), EnvironmentVariableTarget.User);
            actual.Should().BeNull();
         }

         [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_case_sensitive()
         {
            const string variableName = "WinDir";

            var actualMixed0 = _target.GetEnvironmentVariable(variableName);
            var actualMixed1 = _target.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);

            var actualUpper0 = _target.GetEnvironmentVariable(variableName.ToUpperInvariant());
            var actualUpper1 = _target.GetEnvironmentVariable(variableName.ToUpperInvariant(), EnvironmentVariableTarget.Machine);

            var actualLower0 = _target.GetEnvironmentVariable(variableName.ToLowerInvariant());
            var actualLower1 = _target.GetEnvironmentVariable(variableName.ToLowerInvariant(), EnvironmentVariableTarget.Machine);

            actualMixed0.Should().Be(actualMixed1);
            actualMixed1.Should().Be(actualUpper0);
            actualUpper0.Should().Be(actualUpper1);
            actualUpper1.Should().Be(actualLower0);
            actualLower0.Should().Be(actualLower1);
            actualLower1.Should().Be(actualMixed0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_requested_environment_variable()
         {
            _target.GetEnvironmentVariable("windir").Should().Be(Environment.GetEnvironmentVariable("windir"));

            _target.GetEnvironmentVariable("windir", EnvironmentVariableTarget.Machine)
               .Should()
               .Be(Environment.GetEnvironmentVariable("windir", EnvironmentVariableTarget.Machine));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.GetEnvironmentVariable(null);
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("variable");

            throwingAction = () => _target.GetEnvironmentVariable(null, EnvironmentVariableTarget.Machine);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("variable");
         }
      }

      [TestClass]
      public class When_I_call_EnvironmentMapper_GetEnvironmentVariables : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_target_is_not_recognized_It_should_throw_ExtendedInvalidEnumArgumentException()
         {
            Action throwingAction = () => _target.GetEnvironmentVariables((EnvironmentVariableTarget)int.MinValue);
            var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.ParamName.Should().Be("target");
            e.And.Message.Should().Contain("The value of argument '");
            e.And.Message.Should().Contain("is invalid for Enum type '");
            e.And.Message.Should().Contain("EnvironmentVariableTarget");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Microsoft.PlatformCompatibility", "DE0006: API is deprecated", Justification = "Referencing BCL(MWP)")]
         public void It_should_return_the_environment_variables()
         {
            var expected = new HashSet<IEnvironmentVariable>();
            var dictionary = Environment.GetEnvironmentVariables().OfType<DictionaryEntry>();
            foreach (var entry in dictionary)
            {
               expected.Add(new EnvironmentVariable(entry.Key.ToString(), entry.Value.ToString()));
            }

            var actual = _target.GetEnvironmentVariables();
            actual.SetEquals(expected).Should().BeTrue();

            actual = _target.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
            actual.IsSubsetOf(expected).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_call_EnvironmentMapper_GetLogicalDrives : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_non_null_array_of_non_null_elements()
         {
            var actual = _target.GetLogicalDrives();
            actual.Should().NotBeNull();
            actual.Length.Should().BeGreaterThan(0);
            actual.Should().NotContain((string)null);
         }
      }

      [TestClass]
      public class When_I_call_EnvironmentMapper_GetSpecialDirectoryPath : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_option_is_not_recognized_It_should_throw_ExtendedInvalidEnumArgumentException()
         {
            Action throwingAction = () => _target.GetSpecialFolderPath(Environment.SpecialFolder.AdminTools, (Environment.SpecialFolderOption)int.MinValue);
            var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.ParamName.Should().Be("option");
            e.And.Message.Should().Contain("The value of argument '");
            e.And.Message.Should().Contain("is invalid for Enum type '");
            e.And.Message.Should().Contain("SpecialFolderOption");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_specialDirectory_is_not_recognized_It_should_throw_ExtendedInvalidEnumArgumentException()
         {
            Action throwingAction = () => _target.GetSpecialFolderPath((Environment.SpecialFolder)int.MinValue);
            var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.ParamName.Should().Be("specialFolder");
            e.And.Message.Should().Contain("The value of argument '");
            e.And.Message.Should().Contain("is invalid for Enum type '");
            e.And.Message.Should().Contain("SpecialFolder");

            throwingAction = () => _target.GetSpecialFolderPath((Environment.SpecialFolder)int.MinValue, Environment.SpecialFolderOption.None);
            e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
            e.And.ParamName.Should().Be("specialFolder");
            e.And.Message.Should().Contain("The value of argument '");
            e.And.Message.Should().Contain("is invalid for Enum type '");
            e.And.Message.Should().Contain("SpecialFolder");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_directory_path()
         {
            _target.GetSpecialFolderPath(Environment.SpecialFolder.AdminTools)
               .Should()
               .Be(Environment.GetFolderPath(Environment.SpecialFolder.AdminTools));

            _target.GetSpecialFolderPath(Environment.SpecialFolder.AdminTools, Environment.SpecialFolderOption.None)
               .Should()
               .Be(Environment.GetFolderPath(Environment.SpecialFolder.AdminTools, Environment.SpecialFolderOption.None));
         }
      }

      [TestClass]
      public class When_I_call_EnvironmentMapper_SetEnvironmentVariable : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_value_is_null_It_should_not_throw()
         {
            // Notes:  
            //    not all environment variables can be updated, when they are not updated, no exception is thrown.

            const string variableName = "DXROOT";
            var was = _target.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);

            try
            {
               _target.SetEnvironmentVariable(variableName, string.Empty);
               _target.GetEnvironmentVariable(variableName).Should().BeNull();
               _target.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User).Should().BeNull();
               _target.SetEnvironmentVariable(variableName, was);

               _target.SetEnvironmentVariable(variableName, string.Empty, EnvironmentVariableTarget.User);
               _target.GetEnvironmentVariable(variableName).Should().BeNull();
               _target.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User).Should().BeNull();
            }
            finally
            {
               _target.SetEnvironmentVariable(variableName, was);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_variable_is_null_It_should_throw_ArgumentNullException()
         {
            Action throwingAction = () => _target.SetEnvironmentVariable(null, "a value");
            var e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("variable");

            throwingAction = () => _target.SetEnvironmentVariable(null, "a value", EnvironmentVariableTarget.Process);
            e = throwingAction.Should().Throw<ArgumentNullException>();
            e.And.ParamName.Should().Be("variable");
         }
      }

      [TestClass]
      public class When_I_service_locate_IEnvironmentUtilities : ArrangeActAssert
      {
          private IEnvironmentUtilities actual;

          protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IEnvironmentUtilities>();
         }

          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_an_EnvironmentMapper()
         {
            actual.Should().BeOfType<EnvironmentInternalMapping>();
         }
      }

      [TestClass]
      public class When_I_use_EnvironmentMapper_Properties : TestBase
      {
          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_get_and_set_the_ExitCode()
         {
            var was = _target.ExitCode;
            try
            {
               const int expected = 415;
               _target.ExitCode = expected;
               _target.ExitCode.Should().Be(expected);
               Environment.ExitCode.Should().Be(_target.ExitCode);
            }
            finally
            {
               _target.ExitCode = was;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_ClrVersion()
         {
            _target.ClrVersion.Should().Be(Environment.Version);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_CommandLine()
         {
            _target.CommandLine.Should().Be(Environment.CommandLine);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_CurrentManagedThreadId()
         {
            _target.CurrentManagedThreadId.Should().Be(Environment.CurrentManagedThreadId);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_HasShutdownStarted()
         {
            _target.HasShutdownStarted.Should().Be(Environment.HasShutdownStarted);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_Is64BitOperatingSystem()
         {
            _target.Is64BitOperatingSystem.Should().Be(Environment.Is64BitOperatingSystem);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_Is64BitProcess()
         {
            _target.Is64BitProcess.Should().Be(Environment.Is64BitProcess);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_MachineName()
         {
            _target.MachineName.Should().Be(Environment.MachineName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_NewLine()
         {
            _target.NewLine.Should().Be(Environment.NewLine);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("Microsoft.PlatformCompatibility", "DE0009: API is deprecated", Justification = "Referencing BCL(MWP)")]
         public void It_should_return_the_OSVersion()
         {
            _target.OSVersion.Should().Be(Environment.OSVersion);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_ProcessorCount()
         {
            _target.ProcessorCount.Should().Be(Environment.ProcessorCount);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_StackTrace()
         {
            _target.StackTrace.Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_SystemDirectory()
         {
            _target.SystemDirectory.Should().Be(Environment.SystemDirectory);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_SystemPageSizeBytes()
         {
            _target.SystemPageSizeBytes.Should().Be(Environment.SystemPageSize);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_UserDomainName()
         {
            _target.UserDomainName.Should().Be(Environment.UserDomainName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_UserInteractive()
         {
            _target.UserInteractive.Should().Be(Environment.UserInteractive);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_WorkingSetBytes()
         {
            double actual = _target.WorkingSetBytes;
            actual.Should().BeGreaterThan(0);
         }
      }
   }
}
