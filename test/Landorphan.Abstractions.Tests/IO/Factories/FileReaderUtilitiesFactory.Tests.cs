namespace Landorphan.Abstractions.Tests.IO.Factories
{
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using FluentAssertions;
   using Landorphan.Abstractions.IO;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class FileReaderUtilitiesFactory_Tests
   {
      [TestClass]
      public class When_I_call_FileReaderUtilitiesFactory_Create : ArrangeActAssert
      {
         private readonly FileReaderUtilitiesFactory target = new FileReaderUtilitiesFactory();
         private IFileReaderUtilities actual;

         protected override void ActMethod()
         {
            actual = target.Create();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_an_IFileReaderUtilities_instance()
         {
            actual.Should().BeAssignableTo<IFileReaderUtilities>();
         }
      }

      [TestClass]
      public class When_I_call_FileReaderUtilitiesFactory_Create_multiple_times : ArrangeActAssert
      {
         private readonly FileReaderUtilitiesFactory target = new FileReaderUtilitiesFactory();
         private HashSet<IFileReaderUtilities> actuals;

         protected override void ArrangeMethod()
         {
            actuals = new HashSet<IFileReaderUtilities>(new ReferenceEqualityComparer<IFileReaderUtilities>());
         }

         protected override void ActMethod()
         {
            actuals.Add(target.Create());
            actuals.Add(target.Create());
            actuals.Add(target.Create());
         }

         [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "anew")]
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_a_new_instance_each_time()
         {
            actuals.Count.Should().Be(3);
         }
      }

      [TestClass]
      public class When_I_service_locate_IFileReaderUtilitiesFactory : ArrangeActAssert
      {
         private IFileReaderUtilitiesFactory actual;

         protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IFileReaderUtilitiesFactory>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_FileReaderUtilitiesFactory()
         {
            actual.Should().BeOfType<FileReaderUtilitiesFactory>();
         }
      }
   }
}
