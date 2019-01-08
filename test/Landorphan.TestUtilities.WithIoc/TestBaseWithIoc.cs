namespace Landorphan.TestUtilities.WithIoc
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.Ioc.ServiceLocation;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   /// Base class for tests that use TestUtilities.
   /// </summary>
   /// <remarks>
   /// MSTest attribute inheritance and interaction is tricky.  Ensure your test classes are executing in the order you
   /// expect!!!
   /// In particular, TestInitialize attributed members of base classes fire before ClassInitialize attributed members of
   /// descendant classes.
   /// </remarks>
   [CLSCompliant(false)]
   [TestClass]
   public abstract class TestBaseWithIoc : TestBase
   {
      /// <summary>
      /// Gets the test mocking service.
      /// </summary>
      /// <value>
      /// The test mocking service.
      /// </value>
      [SuppressMessage(
         "SonarLint:CodeSmell",
         "S2325: Methods and properties that don't access instance data should be static",
         Justification = "I prefer this 'interface', the performance advantage is negligible (MWP)")]
      protected ITestMockingService TestMockingService => IocServiceLocator.Resolve<ITestMockingService>();

      /// <summary>
      /// Called once before each test method invocation.
      /// </summary>
      protected override void InitializeTestMethod()
      {
         base.InitializeTestMethod();

         // Set the default mocking strategy.
         var tms = IocServiceLocator.Resolve<ITestMockingService>();
         tms.ApplyTestInstanceMockingOnTopOfTestRunMocking();
      }

      /// <summary>
      /// Called once after each test method invocation.
      /// </summary>
      protected override void TeardownTestMethod()
      {
         var tms = IocServiceLocator.Resolve<ITestMockingService>();

         // Cleanup any test specific mocks.
         tms.ResetIndividualTestContainers();

         base.TeardownTestMethod();
      }
   }
}