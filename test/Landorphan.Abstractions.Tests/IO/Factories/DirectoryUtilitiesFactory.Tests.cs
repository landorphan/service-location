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

   [TestClass]
   public class When_I_call_DirectoryUtilitiesFactory_Create : ArrangeActAssert
   {
      private readonly DirectoryUtilitiesFactory target = new DirectoryUtilitiesFactory();
      private IDirectoryUtilities actual;

      protected override void ActMethod()
      {
         actual = target.Create();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_create_an_IDirectory_instance()
      {
         actual.Should().BeAssignableTo<IDirectoryUtilities>();
      }
   }

   [TestClass]
   public class When_I_call_DirectoryUtilitiesFactory_Create_multiple_times : ArrangeActAssert
   {
      private readonly DirectoryUtilitiesFactory target = new DirectoryUtilitiesFactory();
      private HashSet<IDirectoryUtilities> actuals;

      protected override void ArrangeMethod()
      {
         actuals = new HashSet<IDirectoryUtilities>(new ReferenceEqualityComparer<IDirectoryUtilities>());
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
   public class When_I_service_locate_IDirectoryUtilitiesFactory : ArrangeActAssert
   {
      private IDirectoryUtilitiesFactory actual;

      protected override void ActMethod()
      {
         actual = IocServiceLocator.Resolve<IDirectoryUtilitiesFactory>();
      }

      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "mean")]
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_give_me_a_DirectoryUtilitiesFactory()
      {
         actual.Should().BeOfType<DirectoryUtilitiesFactory>();
      }
   }
}
