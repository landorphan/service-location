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

   public static class DirectoryReaderUtilitiesFactory_Tests
   {
      [TestClass]
      public class When_I_call_DirectoryReaderUtilitiesFactory_Create : ArrangeActAssert
      {
         private readonly DirectoryReaderUtilitiesFactory target = new DirectoryReaderUtilitiesFactory();
         private IDirectoryReaderUtilities actual;

         protected override void ActMethod()
         {
            actual = target.Create();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_an_IDirectoryReaderUtilities_instance()
         {
            actual.Should().BeAssignableTo<IDirectoryReaderUtilities>();
         }
      }

      [TestClass]
      public class When_I_call_DirectoryReaderUtilitiesFactory_Create_multiple_times : ArrangeActAssert
      {
         private readonly DirectoryReaderUtilitiesFactory target = new DirectoryReaderUtilitiesFactory();
         private HashSet<IDirectoryReaderUtilities> actuals;

         protected override void ArrangeMethod()
         {
            actuals = new HashSet<IDirectoryReaderUtilities>(new ReferenceEqualityComparer<IDirectoryReaderUtilities>());
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
      public class When_I_service_locate_IDirectoryReaderUtilitiesFactory : ArrangeActAssert
      {
         private IDirectoryReaderUtilitiesFactory actual;

         protected override void ActMethod()
         {
            actual = IocServiceLocator.Resolve<IDirectoryReaderUtilitiesFactory>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_give_me_a_DirectoryReaderUtilitiesFactory()
         {
            actual.Should().BeOfType<DirectoryReaderUtilitiesFactory>();
         }
      }
   }
}
