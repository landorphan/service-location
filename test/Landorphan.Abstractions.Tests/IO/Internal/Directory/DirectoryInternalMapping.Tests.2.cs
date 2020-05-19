namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Abstractions.Tests.TestFacilities;
    using Landorphan.Common.Exceptions;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.TestUtilities;
    using Landorphan.TestUtilities.TestFilters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming   

    [SuppressMessage("SonarLint.CodeSmell", "S4058: Overloads with a StringComparison parameter should be used")]
    public static partial class DirectoryInternalMapping_Tests
    {
        [TestClass]
        public class When_I_call_DirectoryInternalMapping_EnumerateFiles : TestBase
        {
            // An empty search pattern does not throw, does it change behavior?
            // An white-space search pattern does not throw, does it change behavior?

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [RunTestOnlyOnWindows]
            public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
            {
                var path = TempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [RunTestOnlyOnWindows]
            public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
            {
                var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_does_not_exist_It_should_throw_DirectoryNotFoundException()
            {
                if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
                {
                    Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
                    return;
                }

                // usually c:\
                var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
                var path = _pathUtilities.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_has_leading_spaces_It_should_not_throw()
            {
                var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)));
                var expected = new List<string>
                {
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt")),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt")),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt"))
                };

                try
                {
                    _target.CreateDirectory(outerFullPath);
                    foreach (var filePath in expected)
                    {
                        _fileInternalMapping.CreateFile(filePath);
                    }

                    var actual = _target.EnumerateFiles(Spaces + outerFullPath);
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }

                    actual = _target.EnumerateFiles(Spaces + outerFullPath, "*");
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }

                    actual = _target.EnumerateFiles(Spaces + outerFullPath, "*", SearchOption.AllDirectories);
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }
                }
                finally
                {
                    _target.DeleteRecursively(outerFullPath);
                }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_has_trailing_spaces_It_should_not_throw()
            {
                var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)));
                var expected = new List<string>
                {
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt")),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt")),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ".txt"))
                };

                try
                {
                    _target.CreateDirectory(outerFullPath);
                    foreach (var filePath in expected)
                    {
                        _fileInternalMapping.CreateFile(filePath);
                    }

                    var actual = _target.EnumerateFiles(outerFullPath + Spaces);
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }

                    actual = _target.EnumerateFiles(outerFullPath + Spaces, "*");
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }

                    actual = _target.EnumerateFiles(outerFullPath + Spaces, "*", SearchOption.AllDirectories);
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }
                }
                finally
                {
                    _target.DeleteRecursively(outerFullPath);
                }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_empty_It_should_throw_ArgumentException()
            {
                var path = string.Empty;

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_null_It_should_throw_ArgumentNullException()
            {
                const string path = null;

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("path");

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("path");

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
            {
                if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
                {
                    Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
                    return;
                }

                var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
                _target.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
            {
                var path = TempPath + new string('A', TestHardCodes.PathAlwaysTooLong);

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<PathTooLongException>();
                e.And.Message.Should().StartWith("The path");
                e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<PathTooLongException>();
                e.And.Message.Should().StartWith("The path");
                e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<PathTooLongException>();
                e.And.Message.Should().StartWith("The path");
                e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_white_space_It_should_throw_ArgumentException()
            {
                const string path = " \t ";

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_matches_an_existing_file_It_should_throw_DirectoryNotFoundException()
            {
                var path = _fileInternalMapping.CreateTemporaryFile();

                try
                {
                    _fileInternalMapping.FileExists(path).Should().BeTrue();

                    Action throwingAction = () => _target.EnumerateFiles(path);
                    var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                    e.And.Message.Should().Contain("Could not find a part of the directory path '");
                    e.And.Message.Should().Contain(path);
                    e.And.Message.Should().Contain("Parameter name: path");

                    throwingAction = () => _target.EnumerateFiles(path, "*");
                    e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                    e.And.Message.Should().Contain("Could not find a part of the directory path '");
                    e.And.Message.Should().Contain(path);
                    e.And.Message.Should().Contain("Parameter name: path");

                    throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                    e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                    e.And.Message.Should().Contain("Could not find a part of the directory path '");
                    e.And.Message.Should().Contain(path);
                    e.And.Message.Should().Contain("Parameter name: path");
                }
                finally
                {
                    _fileInternalMapping.DeleteFile(path);
                }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [RunTestOnlyOnWindows]
            public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
            {
                const string path = ":";

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
            {
                var path = string.Format(
                    CultureInfo.InvariantCulture,
                    @"\\{0}\{1}",
                    Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
                    Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [RunTestOnlyOnWindows]
            public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
            {
                var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

                Action throwingAction = () => _target.EnumerateFiles(path);
                var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFiles(path, "*");
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_searchOption_is_unrecognized_It_should_throw_ExtendedInvalidEnumArgumentException()
            {
                const string path = @".\";
                const string SearchPattern = "*.";
                var searchOption = (SearchOption)(-5);

                Action throwingAction = () => _target.EnumerateFiles(path, SearchPattern, searchOption);
                var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
                e.And.ParamName.Should().Be("searchOption");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_searchPattern_is_null_It_should_throw_ArgumentNullException()
            {
                const string path = @".\";
                const string SearchPattern = null;

                Action throwingAction = () => _target.EnumerateFiles(path, SearchPattern);
                var e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("searchPattern");

                throwingAction = () => _target.EnumerateFiles(path, SearchPattern, SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("searchPattern");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_return_the_known_files()
            {
                var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_files)));
                var expected = new List<string>
                {
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_files) + ".txt")),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_files) + ".txt")),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_files) + ".txt"))
                };

                try
                {
                    _target.CreateDirectory(outerFullPath);
                    foreach (var filePath in expected)
                    {
                        _fileInternalMapping.CreateFile(filePath);
                    }

                    _target.EnumerateFiles(outerFullPath + Spaces).Should().Contain(expected);
                    _target.EnumerateFiles(outerFullPath + Spaces, "*").Should().Contain(expected);
                    _target.EnumerateFiles(outerFullPath + Spaces, "*", SearchOption.AllDirectories).Should().Contain(expected);
                }
                finally
                {
                    _target.DeleteRecursively(outerFullPath);
                }
            }
        }

        [TestClass]
        public class When_I_call_DirectoryInternalMapping_EnumerateFileSystemEntries : TestBase
        {
            // An empty search pattern does not throw, does it change behavior?
            // An white-space search pattern does not throw, does it change behavior?

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [RunTestOnlyOnWindows]
            public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_throw_ArgumentException()
            {
                var path = TempPath + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + ":" + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [RunTestOnlyOnWindows]
            public void And_the_path_contains_an_invalid_character_It_should_throw_ArgumentException()
            {
                var path = _pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)) + "|";

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (invalid characters).\r\nParameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_does_not_exist_It_should_throw_DirectoryNotFoundException()
            {
                if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
                {
                    Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
                    return;
                }

                // usually c:\
                var drive = TestHardCodes.WindowsLocalTestPaths.MappedDrive;
                var path = _pathUtilities.Combine(drive, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_has_leading_spaces_It_should_not_throw()
            {
                var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)));
                var expected = new List<string>
                {
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)))
                };

                try
                {
                    _target.CreateDirectory(outerFullPath);
                    foreach (var sd in expected)
                    {
                        _target.CreateDirectory(sd);
                    }

                    var actual = _target.EnumerateFileSystemEntries(Spaces + outerFullPath);
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }

                    actual = _target.EnumerateFileSystemEntries(Spaces + outerFullPath, "*");
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }

                    actual = _target.EnumerateFileSystemEntries(Spaces + outerFullPath, "*", SearchOption.AllDirectories);
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }
                }
                finally
                {
                    _target.DeleteRecursively(outerFullPath);
                }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_has_trailing_spaces_It_should_not_throw()
            {
                var outerFullPath = _pathUtilities.GetFullPath(_pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)));
                var expected = new List<string>
                {
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture))),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)))
                };

                try
                {
                    _target.CreateDirectory(outerFullPath);
                    foreach (var sd in expected)
                    {
                        _target.CreateDirectory(sd);
                    }

                    var actual = _target.EnumerateFileSystemEntries(outerFullPath + Spaces);
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }

                    actual = _target.EnumerateFileSystemEntries(outerFullPath + Spaces, "*");
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }

                    actual =
                        _target.EnumerateFileSystemEntries(outerFullPath + Spaces, "*", SearchOption.AllDirectories);
                    actual.Should().Contain(expected);
                    foreach (var path in actual)
                    {
                        // BCL will inject the "padding" if not handled by the wrapper.
                        path.Contains(Spaces).Should().BeFalse();
                    }
                }
                finally
                {
                    _target.DeleteRecursively(outerFullPath);
                }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_empty_It_should_throw_ArgumentException()
            {
                var path = string.Empty;

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_null_It_should_throw_ArgumentNullException()
            {
                const string path = null;

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_on_an_unmapped_drive_It_should_throw_DirectoryNotFoundException()
            {
                if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
                {
                    Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
                    return;
                }

                var path = TestHardCodes.WindowsLocalTestPaths.UnmappedDrive + Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
                _target.DirectoryExists(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive).Should().BeFalse();

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_too_long_It_should_throw_PathTooLongException()
            {
                var path = TempPath + new string('A', TestHardCodes.PathAlwaysTooLong);

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<PathTooLongException>();
                e.And.Message.Should().StartWith("The path");
                e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<PathTooLongException>();
                e.And.Message.Should().StartWith("The path");
                e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<PathTooLongException>();
                e.And.Message.Should().StartWith("The path");
                e.And.Message.Should().Contain("is too long, or a component of the specified path is too long");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_is_white_space_It_should_throw_ArgumentException()
            {
                const string path = " \t ";

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().ContainAll("The path is not well-formed (cannot be empty or all whitespace)", "Parameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_matches_an_existing_file_It_should_throw_DirectoryNotFoundException()
            {
                var path = _fileInternalMapping.CreateTemporaryFile();

                try
                {
                    _fileInternalMapping.FileExists(path).Should().BeTrue();

                    Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                    var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                    e.And.Message.Should().Contain("Could not find a part of the directory path '");
                    e.And.Message.Should().Contain(path);
                    e.And.Message.Should().Contain("Parameter name: path");

                    throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                    e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                    e.And.Message.Should().Contain("Could not find a part of the directory path '");
                    e.And.Message.Should().Contain(path);
                    e.And.Message.Should().Contain("Parameter name: path");

                    throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                    e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                    e.And.Message.Should().Contain("Could not find a part of the directory path '");
                    e.And.Message.Should().Contain(path);
                    e.And.Message.Should().Contain("Parameter name: path");
                }
                finally
                {
                    _fileInternalMapping.DeleteFile(path);
                }
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [RunTestOnlyOnWindows]
            public void And_the_path_starts_with_a_colon_It_should_throw_ArgumentException()
            {
                const string path = ":";

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("path");
                e.And.Message.Should().Be("The path is not well-formed (':' used outside the drive label).\r\nParameter name: path");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_path_uses_an_unknown_network_name_host_It_should_throw_DirectoryNotFoundException()
            {
                var path = string.Format(
                    CultureInfo.InvariantCulture,
                    @"\\{0}\{1}",
                    Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture),
                    Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            [RunTestOnlyOnWindows]
            public void And_the_path_uses_an_unknown_network_name_share_It_should_throw_DirectoryNotFoundException()
            {
                var path = _pathUtilities.Combine(@"\\localhost\", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path);
                var e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*");
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);

                throwingAction = () => _target.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<DirectoryNotFoundException>();
                e.And.Message.Should().Contain("Could not find a part of the directory path");
                e.And.Message.Should().Contain(path);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_searchOption_is_unrecognized_It_should_throw_ExtendedInvalidEnumArgumentException()
            {
                const string path = @".\";
                const string SearchPattern = "*.";
                var searchOption = (SearchOption)(-5);

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path, SearchPattern, searchOption);
                var e = throwingAction.Should().Throw<ExtendedInvalidEnumArgumentException>();
                e.And.ParamName.Should().Be("searchOption");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_searchPattern_contains_an_invalid_character_It_should_throw_ArgumentException()
            {
                const string path = @".\";

                var pathUtils = IocServiceLocator.Resolve<IPathUtilities>();
                var SearchPattern = "*." + pathUtils.GetInvalidFileNameCharacters().First();

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path, SearchPattern);
                var e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("searchPattern");
                e.And.Message.Should().Contain("The search pattern is not well-formed (contains invalid characters).");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, SearchPattern, SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentException>();
                e.And.ParamName.Should().Be("searchPattern");
                e.And.Message.Should().Contain("The search pattern is not well-formed (contains invalid characters).");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_the_searchPattern_is_null_It_should_throw_ArgumentNullException()
            {
                const string path = @".\";
                const string SearchPattern = null;

                Action throwingAction = () => _target.EnumerateFileSystemEntries(path, SearchPattern);
                var e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("searchPattern");

                throwingAction = () => _target.EnumerateFileSystemEntries(path, SearchPattern, SearchOption.AllDirectories);
                e = throwingAction.Should().Throw<ArgumentNullException>();
                e.And.ParamName.Should().Be("searchPattern");
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_return_the_known_FileSystemEntries()
            {
                var outerFullPath =
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(TempPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_FileSystemEntries)));
                var expected = new List<string>
                {
                    _pathUtilities.GetFullPath(
                        _pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_FileSystemEntries))),
                    _pathUtilities.GetFullPath(
                        _pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_FileSystemEntries))),
                    _pathUtilities.GetFullPath(_pathUtilities.Combine(outerFullPath, Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + nameof(It_should_return_the_known_FileSystemEntries)))
                };

                try
                {
                    _target.CreateDirectory(outerFullPath);
                    foreach (var sd in expected)
                    {
                        _target.CreateDirectory(sd);
                    }

                    _target.EnumerateFileSystemEntries(outerFullPath).Should().Contain(expected);
                    _target.EnumerateFileSystemEntries(outerFullPath, "*").Should().Contain(expected);
                    _target.EnumerateFileSystemEntries(outerFullPath, "*", SearchOption.AllDirectories).Should().Contain(expected);
                }
                finally
                {
                    _target.DeleteRecursively(outerFullPath);
                }
            }
        }
    }
}
