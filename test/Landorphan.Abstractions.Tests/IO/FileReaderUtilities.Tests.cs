namespace Landorphan.Abstractions.Tests.IO
{
   using System;
   using System.Collections.Immutable;
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

      public static class DirectoryReaderUtilities_Tests
      {
         private static readonly IFileUtilities _fileUtilities = IocServiceLocator.Resolve<IFileUtilities>();
         private static readonly IFileWriterUtilities _fileWriterUtilities = IocServiceLocator.Resolve<IFileWriterUtilities>();
         private static readonly IFileReaderUtilities _target = IocServiceLocator.Resolve<IFileReaderUtilities>();

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
                  var expected = new Byte[] {0x00, 0x01, 0x02, 0x03}.ToImmutableList();
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
               const String expected = "This is a test,\nThis is only a test.  If this had been a real life,\n you would have been given...";
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
}
