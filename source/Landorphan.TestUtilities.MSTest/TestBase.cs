namespace Landorphan.TestUtilities
{
   using System;
   using System.IO;
   using System.Linq;
   using System.Reflection;
   using System.Runtime.InteropServices;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Testability;
   using Landorphan.TestUtilities.TestFilters;
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
   [TestClass]
   public abstract class TestBase
   {
      private readonly String _originalCurrentDirectory;
      private Lazy<EventMonitor> _eventMonitor = new Lazy<EventMonitor>(() => new EventMonitor());

      /// <summary>
      /// Initializes a new instance of the <see cref="TestBase" /> class.
      /// </summary>
//      [SuppressMessage("SonarLint.CodeSmell", "S4005: Call the overload that takes a 'System.Uri' as an argument instead.",
//         Justification = "Needed to work around Unix/Linux base parsing and source is consitered to be safe.")]
      protected TestBase()
      {
         // ReSharper disable once AssignNullToNotNullAttribute
         if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            var codebase = GetType().Assembly.GetName().CodeBase;
            var builder = new UriBuilder
            {
               Scheme = Uri.UriSchemeFile,
               Host = string.Empty,
               Path = codebase.Replace("file://", string.Empty)
            };
            var path = Uri.UnescapeDataString(builder.Path);
            _originalCurrentDirectory = Path.GetDirectoryName(path);
         }
         else
         {
            var uri = new Uri(Path.GetDirectoryName(GetType().Assembly.GetName().CodeBase));
            _originalCurrentDirectory = uri.LocalPath;
         }

         // Set the default mocking strategy.
         var tms = IocServiceLocator.Resolve<ITestMockingService>();
         tms.ApplyTestInstanceMockingOnTopOfTestRunMocking();
      }

      /// <summary>
      /// Allows for a static OnTestCleanup method to be supplied that will be called
      /// after every test instance execution.
      /// </summary>
      public static Action<TestBase> OnTestCleanup { get; set; }

      /// <summary>
      /// Allows for a static OnTestInitialize method to be supplied that will be
      /// called before every test instance execution.
      /// </summary>
      public static Action<TestBase> OnTestInitialize { get; set; }

      /// <summary>
      /// Gets or sets the test context which provides information about and functionality for the current test run.
      /// </summary>
      public TestContext TestContext { get; protected set; }

      /// <summary>
      /// Gets the monitored events.
      /// </summary>
      /// <value>
      /// The monitored events.
      /// </value>
      protected EventMonitor MonitoredEvents => _eventMonitor.Value;

      /// <summary>
      /// Code that executes before any of the tests methods in the test class are executed.
      /// </summary>
      /// <remarks>
      /// Note:  ClassInitializeAttribute is not inheritable.
      /// </remarks>
      [ClassInitialize]
      public static void TestClassInitialize(TestContext context)
      {
         // Quieting intermittent code analysis warning
         TestHelp.DoNothing(context);
         // currently empty
      }

      /// <summary>
      /// Steps that are run before each test.
      /// </summary>
      [TestInitialize]
      public void TestInitialize()
      {
         InitializeTestMethod();
      }

      /// <summary>
      /// Called once before each test method invocation.
      /// </summary>
      protected virtual void InitializeTestMethod()
      {
         // ensure the current directory is restored to the original current directory
         var currentDirectory = Directory.GetCurrentDirectory();
         if (!_originalCurrentDirectory.Equals(currentDirectory, StringComparison.OrdinalIgnoreCase))
         {
            Directory.SetCurrentDirectory(_originalCurrentDirectory);
         }

         ApplyTestFilters();
         OnTestInitialize?.Invoke(this);
      }

      /// <summary>
      /// Called once after each test method invocation.
      /// </summary>
      protected virtual void TeardownTestMethod()
      {
         if (_eventMonitor.IsValueCreated)
         {
            _eventMonitor = new Lazy<EventMonitor>(() => new EventMonitor());
         }

         OnTestCleanup?.Invoke(this);
      }

      /// <summary>
      /// Steps that are run after each test.
      /// </summary>
      [TestCleanup]
      public void TestCleanup()
      {
         TeardownTestMethod();
      }

      /// <summary>
      /// Code that executes after all of the test methods in the test class are executed.
      /// </summary>
      [ClassCleanup]
      public static void TestClassCleanup()
      {
         var tms = IocServiceLocator.Resolve<ITestMockingService>();

         // Cleanup any test specific mocks.
         tms.ResetIndividualTestContainers();
      }

      private void ApplyTestFilters()
      {
         MethodInfo methodInfo = this.GetType().GetMethod(this.TestContext.TestName);
         if (methodInfo != null)
         {
            var testFilters = (
               from a in methodInfo.GetCustomAttributes()
               let tf = a as TestFilterAttribute
               where tf != null
               select tf);

            bool suppress = testFilters.Any(tf => tf.ReturnInconclusiveTestResult());
            if (suppress)
            {
               Assert.Inconclusive("This test has been suppressed by test filters evaluated against the runtime environment.");
            }
         }
      }
   }
}
