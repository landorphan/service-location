namespace Landorphan.Abstractions.Tests
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Runtime.InteropServices;
   using FluentAssertions;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class When_I_call_EnvironmentUtilitiesFactory_Create : ArrangeActAssert
   {
      private readonly EnvironmentUtilitiesFactory target = new EnvironmentUtilitiesFactory();
      private IEnvironmentUtilities actual;

      protected override void ActMethod()
      {
         actual = target.Create();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_create_an_IEnvironment_instance()
      {
         actual.Should().BeAssignableTo<IEnvironmentUtilities>();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void TODO_REMOVE()
      {
         var fileUtils = IocServiceLocator.Resolve<IFileUtilities>();

         var tempFile = fileUtils.CreateTemporaryFile();
         var lastGoodDt = DateTime.UtcNow;
         if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            lastGoodDt = new DateTime(1601, 1, 1, 0, 0, 0, 1);
         }

         if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
         {
            lastGoodDt = new DateTime(1970, 1, 1, 0, 0, 1, 0);
         }

         try
         {
            while (true)
            {
               try
               {
                  var adjustedDt = DateTime.UtcNow;
                  if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                  {
                     adjustedDt = lastGoodDt.AddTicks(-1);
                  }

                  if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                  {
                     // linux file precision is to the second
                     adjustedDt = lastGoodDt.AddTicks(-1 * TimeSpan.TicksPerSecond);
                  }

                  Directory.SetCreationTimeUtc(tempFile, adjustedDt);
                  var getDt = Directory.GetCreationTimeUtc(tempFile);
                  if (adjustedDt != getDt)
                  {
                     // supposed to throw but does not on Windows
                     break;
                  }

                  lastGoodDt = adjustedDt;
               }
               catch (ArgumentOutOfRangeException)
               {
                  break;
               }
            }
         }
         finally
         {
            fileUtils.DeleteFile(tempFile);
         }

         Trace.WriteLine($"lastGoodDt = {lastGoodDt}");
         Trace.WriteLine($"lastGoodDt.Ticks = {lastGoodDt.Ticks}");
         
         // We want to keep this test in strictly as a temporary method 
         // while evaluating the platform rules.  This avoids warnings that 
         // no assertions exist.
         Assert.IsNotNull(tempFile);
      }
   }

   [TestClass]
   public class When_I_call_EnvironmentUtilitiesFactory_Create_multiple_times : ArrangeActAssert
   {
      private readonly EnvironmentUtilitiesFactory target = new EnvironmentUtilitiesFactory();
      private HashSet<IEnvironmentUtilities> actuals;

      protected override void ArrangeMethod()
      {
         actuals = new HashSet<IEnvironmentUtilities>(new ReferenceEqualityComparer<IEnvironmentUtilities>());
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
   public class When_I_service_locate_IEnvironmentUtilitiesFactory : ArrangeActAssert
   {
      private IEnvironmentUtilitiesFactory actual;

      protected override void ActMethod()
      {
         actual = IocServiceLocator.Resolve<IEnvironmentUtilitiesFactory>();
      }

      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "mean")]
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_give_me_a_EnvironmentUtilitiesFactory()
      {
         actual.Should().BeOfType<EnvironmentUtilitiesFactory>();
      }
   }
}
