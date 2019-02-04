﻿namespace Landorphan.Abstractions.Tests.IO.Internal.Directory
{
   using System;
   using FluentAssertions;
   using Landorphan.Abstractions.IO.Internal;
   using Landorphan.Abstractions.Tests.TestFacilities;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming   

   public static partial class DirectoryInternalMapping_Tests
   {
      [TestClass]
      public class When_I_call_DirectoryInternalMapping_TestHookPathContainsUnmappedDrive : TestBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_contains_no_colon_characters_It_should_return_false()
         {
            DirectoryInternalMapping.TestHookPathContainsUnmappedDrive(@"abc").Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_contains_a_colon_character_that_is_not_part_of_the_drive_label_It_should_return_false()
         {
            DirectoryInternalMapping.TestHookPathContainsUnmappedDrive(@"abc:def").Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_empty_It_should_return_false()
         {
            DirectoryInternalMapping.TestHookPathContainsUnmappedDrive(String.Empty).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_null_It_should_return_false()
         {
            DirectoryInternalMapping.TestHookPathContainsUnmappedDrive(null).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_mapped_drive_It_should_return_false()
         {
            if (TestHardCodes.WindowsLocalTestPaths.MappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.MappedDrive)}");
               return;
            }

            // ReSharper disable once StringLiteralTypo
            var path = _pathUtilities.Combine(TestHardCodes.WindowsLocalTestPaths.MappedDrive, @"abc:defg\");
            DirectoryInternalMapping.TestHookPathContainsUnmappedDrive(path).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void And_the_path_is_on_an_unmapped_drive_It_should_return_true()
         {
            if (TestHardCodes.WindowsLocalTestPaths.UnmappedDrive == null)
            {
               Assert.Inconclusive($"Null path returned from {nameof(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive)}");
               return;
            }

            // ReSharper disable once StringLiteralTypo
            var path = _pathUtilities.Combine(TestHardCodes.WindowsLocalTestPaths.UnmappedDrive, @"abc:defg\");
            DirectoryInternalMapping.TestHookPathContainsUnmappedDrive(path).Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_service_locate_IDirectoryInternalMapping : ArrangeActAssert
      {
         private IDirectoryInternalMapping actual;

         protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IDirectoryInternalMapping>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_DirectoryInternalMapping()
         {
            actual.Should().BeOfType<DirectoryInternalMapping>();
         }
      }
   }
}
