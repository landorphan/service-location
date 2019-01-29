namespace Landorphan.Abstractions.Tests.IO
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;
   using System.Text;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Landorphan.TestUtilities.TestFacilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class FileWriterUtilities_Tests
   {
      [TestClass]
      public class Given_I_have_an_IFileWriterUtilities : TestBase
      {
         // b/c this is such a thin wrapper over tested implementation, negative testing is not implemented.

         private static readonly IEnvironmentUtilities _environmentUtilities = IocServiceLocator.Resolve<IEnvironmentUtilities>();
         private static readonly IFileReaderUtilities _fileReaderUtilities = IocServiceLocator.Resolve<IFileReaderUtilities>();
         private static readonly IFileUtilities _fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
         private static readonly IPathUtilities _pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         private static readonly IFileWriterUtilities _target = IocServiceLocator.Resolve<IFileWriterUtilities>();
         private static readonly String _tempPath = _environmentUtilities.GetTemporaryDirectoryPath();

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_AppendAllLines_It_should_append_all_lines()
         {
            var contents = new[] {"one", "two", "three"};
            var path = _fileUtilities.CreateTemporaryFile();
            try
            {
               var enc = new UTF8Encoding(false, true);
               _target.AppendAllLines(path, contents, enc);
               var actualLines = _fileReaderUtilities.ReadAllLines(path, enc);
               actualLines.Should().Contain(contents);
            }
            finally
            {
               _fileUtilities.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_AppendAllText_It_should_append_all_text()
         {
            const String contents = "one\r\ntwo\r\nthree";
            var path = _fileUtilities.CreateTemporaryFile();
            try
            {
               var enc = new UTF8Encoding(false, true);
               _target.AppendAllText(path, contents, enc);
               var actual = _fileReaderUtilities.ReadAllText(path, enc);
               actual.Should().Contain(contents + _environmentUtilities.NewLine);
            }
            finally
            {
               _fileUtilities.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_CopyNoOverwrite_It_should_copy_the_file()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _fileUtilities.CreateTemporaryFile();
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + ".tmp");
            try
            {
               _target.CopyNoOverwrite(sourceFileName, destFileName);
               _fileUtilities.FileExists(destFileName).Should().BeTrue();
            }
            finally
            {
               _fileUtilities.DeleteFile(sourceFileName);
               _fileUtilities.DeleteFile(destFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_CopyWithOverwrite_It_should_copy_the_file()
         {
            // HAPPY PATH TEST:
            var sourceFileName = _fileUtilities.CreateTemporaryFile();
            var destFileName = _fileUtilities.CreateTemporaryFile();
            try
            {
               _target.CopyWithOverwrite(sourceFileName, destFileName);
            }
            finally
            {
               _fileUtilities.DeleteFile(sourceFileName);
               _fileUtilities.DeleteFile(destFileName);
            }

            TestUtilitiesHardCodes.NoExceptionWasThrown.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_Move_It_should_move_the_file()
         {
            var sourceFileName = _fileUtilities.CreateTemporaryFile();
            var destFileName = _pathUtilities.Combine(_tempPath, Guid.NewGuid() + "When_I_call_FileWriterUtilities_Move.txt");
            try
            {
               _target.Move(sourceFileName, destFileName);

               _fileUtilities.FileExists(sourceFileName).Should().BeFalse();
               _fileUtilities.FileExists(destFileName).Should().BeTrue();
            }
            finally
            {
               _fileUtilities.DeleteFile(sourceFileName);
               _fileUtilities.DeleteFile(destFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_ReplaceContentsNoBackup_It_should_replace_the_contents()
         {
            var encoding = Encoding.UTF8;
            var sourceContents = new[] {"one", "two", "three"}.ToImmutableList();

            var sourceFileName = _fileUtilities.CreateTemporaryFile();
            var destinationFileName = _fileUtilities.CreateTemporaryFile();
            try
            {
               _target.AppendAllLines(sourceFileName, sourceContents, encoding);

               _target.ReplaceContentsNoBackup(sourceFileName, destinationFileName);

               _fileUtilities.FileExists(sourceFileName).Should().BeFalse();
               _fileUtilities.FileExists(destinationFileName).Should().BeTrue();
               var contentsOfDestinationAfter = _fileReaderUtilities.ReadAllLines(destinationFileName, encoding);
               contentsOfDestinationAfter.Should().BeEquivalentTo(sourceContents);
            }
            finally
            {
               _fileUtilities.DeleteFile(sourceFileName);
               _fileUtilities.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_ReplaceContentsNoBackupIgnoringMetadataErrors_It_should_replace_the_contents()
         {
            var encoding = Encoding.UTF8;
            var sourceContents = new[] {"one", "two", "three"}.ToImmutableList();

            var sourceFileName = _fileUtilities.CreateTemporaryFile();
            var destinationFileName = _fileUtilities.CreateTemporaryFile();
            try
            {
               _target.AppendAllLines(sourceFileName, sourceContents, encoding);

               _target.ReplaceContentsNoBackupIgnoringMetadataErrors(sourceFileName, destinationFileName);

               _fileUtilities.FileExists(sourceFileName).Should().BeFalse();
               _fileUtilities.FileExists(destinationFileName).Should().BeTrue();
               var contentsOfDestinationAfter = _fileReaderUtilities.ReadAllLines(destinationFileName, encoding);
               contentsOfDestinationAfter.Should().BeEquivalentTo(sourceContents);
            }
            finally
            {
               _fileUtilities.DeleteFile(sourceFileName);
               _fileUtilities.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_ReplaceContentsWithBackup_It_should_replace_the_contents_and_back_up_destination()
         {
            var encoding = Encoding.UTF8;
            var sourceContents = new[] {"one", "two", "three"}.ToImmutableList();
            var destinationContents = new[] {"4", "5", "6"}.ToImmutableList();

            var sourceFileName = _fileUtilities.CreateTemporaryFile();
            var destinationFileName = _fileUtilities.CreateTemporaryFile();
            var backupFileName = _fileUtilities.CreateTemporaryFile();
            try
            {
               _target.AppendAllLines(sourceFileName, sourceContents, encoding);
               _target.AppendAllLines(destinationFileName, destinationContents, encoding);

               _target.ReplaceContentsWithBackup(sourceFileName, destinationFileName, backupFileName);

               _fileUtilities.FileExists(sourceFileName).Should().BeFalse();
               _fileUtilities.FileExists(destinationFileName).Should().BeTrue();
               _fileUtilities.FileExists(backupFileName).Should().BeTrue();
               var contentsOfDestinationAfter = _fileReaderUtilities.ReadAllLines(destinationFileName, encoding);
               contentsOfDestinationAfter.Should().BeEquivalentTo(sourceContents);
               var contentsOfBackupAfter = _fileReaderUtilities.ReadAllLines(backupFileName, encoding);
               contentsOfBackupAfter.Should().BeEquivalentTo(destinationContents);
            }
            finally
            {
               _fileUtilities.DeleteFile(sourceFileName);
               _fileUtilities.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void
            When_I_call_FileWriterUtilities_ReplaceContentsWithBackupIgnoringMetadataErrors_It_should_replace_the_contents_and_back_up_destination()
         {
            var encoding = Encoding.UTF8;
            var sourceContents = new[] {"one", "two", "three"}.ToImmutableList();
            var destinationContents = new[] {"4", "5", "6"}.ToImmutableList();

            var sourceFileName = _fileUtilities.CreateTemporaryFile();
            var destinationFileName = _fileUtilities.CreateTemporaryFile();
            var backupFileName = _fileUtilities.CreateTemporaryFile();
            try
            {
               _target.AppendAllLines(sourceFileName, sourceContents, encoding);
               _target.AppendAllLines(destinationFileName, destinationContents, encoding);

               _target.ReplaceContentsWithBackupIgnoringMetadataErrors(sourceFileName, destinationFileName, backupFileName);

               _fileUtilities.FileExists(sourceFileName).Should().BeFalse();
               _fileUtilities.FileExists(destinationFileName).Should().BeTrue();
               _fileUtilities.FileExists(backupFileName).Should().BeTrue();
               var contentsOfDestinationAfter = _fileReaderUtilities.ReadAllLines(destinationFileName, encoding);
               contentsOfDestinationAfter.Should().BeEquivalentTo(sourceContents);
               var contentsOfBackupAfter = _fileReaderUtilities.ReadAllLines(backupFileName, encoding);
               contentsOfBackupAfter.Should().BeEquivalentTo(destinationContents);
            }
            finally
            {
               _fileUtilities.DeleteFile(sourceFileName);
               _fileUtilities.DeleteFile(destinationFileName);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_WriteAllBytes_It_should_write_the_bytes()
         {
            var expected = new Byte[] {0x01, 0x01, 0x02, 0x03}.ToImmutableList();

            var path = _fileUtilities.CreateTemporaryFile();
            try
            {
               _target.WriteAllBytes(path, expected);
               _fileReaderUtilities.ReadAllBytes(path).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _fileUtilities.DeleteFile(path);
            }

            path = _fileUtilities.CreateTemporaryFile();
            try
            {
               _target.WriteAllBytes(path, expected.ToArray());
               _fileReaderUtilities.ReadAllBytes(path).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _fileUtilities.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_WriteAllLines_It_should_write_the_contents()
         {
            var expected = new[] {"zero", "one", "two", "three"}.ToImmutableList();

            var path = _fileUtilities.CreateTemporaryFile();
            try
            {
               _target.WriteAllLines(path, expected.ToArray(), Encoding.UTF8);
               _fileReaderUtilities.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);

               _target.WriteAllLines(path, (IEnumerable<String>)expected, Encoding.UTF8);
               _fileReaderUtilities.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);

               _target.WriteAllLines(path, expected, Encoding.UTF8);
               _fileReaderUtilities.ReadAllLines(path, Encoding.UTF8).Should().BeEquivalentTo(expected);
            }
            finally
            {
               _fileUtilities.DeleteFile(path);
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_FileWriterUtilities_WriteAllText_It_should_write_the_contents()
         {
            const String expected = "zero\r\none\r\ntwo\r\nthree";

            var path = _fileUtilities.CreateTemporaryFile();
            try
            {
               _target.WriteAllText(path, expected, Encoding.UTF8);
               _fileReaderUtilities.ReadAllText(path, Encoding.UTF8).Should().Be(expected);
            }
            finally
            {
               _fileUtilities.DeleteFile(path);
            }
         }
      }

      [TestClass]
      public class When_I_service_locate_IFileWriterUtilities : ArrangeActAssert
      {
         private IFileWriterUtilities actual;

         protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IFileWriterUtilities>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_FileReaderUtilitiesFactory()
         {
            actual.Should().BeOfType<FileWriterUtilities>();
         }
      }
   }
}
