namespace Landorphan.Abstractions.Tests.IO
{
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Text;
    using FluentAssertions;
    using Landorphan.Abstractions.IO;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

    public static class FileReaderUtilities_Tests
    {
        // b/c this is such a thin wrapper over tested implementation, negative testing is not implemented.

        private static readonly IFileUtilities _fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
        private static readonly IFileWriterUtilities _fileWriterUtilities = IocServiceLocator.Resolve<IFileWriterUtilities>();
        private static readonly IFileReaderUtilities _target = IocServiceLocator.Resolve<IFileReaderUtilities>();

        [TestClass]
        public class When_I_call_FileReaderUtilities_Open : TestBase
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_open_the_file()
            {
                var path = _fileUtilities.CreateTemporaryFile();
                try
                {
                    using (var stream = _target.Open(path, FileMode.Open))
                    {
                        stream.Should().NotBeNull();
                    }

                    using (var stream = _target.Open(path, FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Should().NotBeNull();
                    }

                    using (var stream = _target.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        stream.Should().NotBeNull();
                    }
                }
                finally
                {
                    _fileUtilities.DeleteFile(path);
                }
            }
        }

        [TestClass]
        public class When_I_call_FileReaderUtilities_OpenRead : TestBase
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_open_the_file()
            {
                var path = _fileUtilities.CreateTemporaryFile();
                try
                {
                    using (var stream = _target.OpenRead(path))
                    {
                        stream.Should().NotBeNull();
                    }
                }
                finally
                {
                    _fileUtilities.DeleteFile(path);
                }
            }
        }

        [TestClass]
        public class When_I_call_FileReaderUtilities_OpenText : TestBase
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_open_the_file()
            {
                var path = _fileUtilities.CreateTemporaryFile();
                try
                {
                    using (var sr = _target.OpenText(path))
                    {
                        sr.Should().NotBeNull();
                    }
                }
                finally
                {
                    _fileUtilities.DeleteFile(path);
                }
            }
        }

        [TestClass]
        public class When_I_call_FileReaderUtilities_ReadAllBytes : TestBase
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_read_all_bytes()
            {
                var path = _fileUtilities.CreateTemporaryFile();
                try
                {
                    var expected = new byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
                    _fileWriterUtilities.WriteAllBytes(path, expected);
                    _target.ReadAllBytes(path).Should().BeEquivalentTo(expected);
                }
                finally
                {
                    _fileUtilities.DeleteFile(path);
                }
            }
        }

        [TestClass]
        public class When_I_call_FileReaderUtilities_ReadAllLines : TestBase
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_read_all_lines()
            {
                var path = _fileUtilities.CreateTemporaryFile();
                try
                {
                    var expected = new[] {"zero", "one", "two", "three"}.ToImmutableList();
                    _fileWriterUtilities.WriteAllLines(path, expected, Encoding.UTF8);
                    _target.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);
                }
                finally
                {
                    _fileUtilities.DeleteFile(path);
                }
            }
        }

        [TestClass]
        public class When_I_call_FileReaderUtilities_ReadAllText : TestBase
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_read_all_text()
            {
                const string expected = "This is a test,\nThis is only a test.  If this had been a real life,\n you would have been given...";
                var path = _fileUtilities.CreateTemporaryFile();
                try
                {
                    _fileWriterUtilities.WriteAllText(path, expected, Encoding.UTF8);
                    _target.ReadAllText(path, Encoding.UTF8).Should().Be(expected);
                }
                finally
                {
                    _fileUtilities.DeleteFile(path);
                }
            }
        }

        [TestClass]
        public class When_I_call_FileReaderUtilities_ReadLines : TestBase
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_read_the_lines()
            {
                var path = _fileUtilities.CreateTemporaryFile();
                try
                {
                    var expected = new[] {"Line 0", "Line 1", "Line2", "Line 3"}.ToImmutableList();
                    var encoding = Encoding.UTF8;
                    _fileWriterUtilities.WriteAllLines(path, expected, encoding);

                    var lines = _target.ReadLines(path, encoding);
                    lines.SequenceEqual(expected).Should().BeTrue();
                }
                finally
                {
                    _fileUtilities.DeleteFile(path);
                }
            }
        }

        [TestClass]
        public class When_I_service_locate_IFileReaderUtilities : ArrangeActAssert
        {
            private IFileReaderUtilities actual;

            protected override void ActMethod()
            {
                actual = IocServiceLocator.Resolve<IFileReaderUtilities>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_give_me_a_FileReaderUtilities()
            {
                actual.Should().BeOfType<FileReaderUtilities>();
            }
        }
    }
}
