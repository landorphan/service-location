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
    using NUnit.Framework;

    /// <summary>
    /// Base class for tests that use TestUtilities.
    /// </summary>
    /// <remarks>
    /// MSTest attribute inheritance and interaction is tricky.  Ensure your test classes are executing in the order you
    /// expect!!!
    /// In particular, TestInitialize attributed members of base classes fire before ClassInitialize attributed members of
    /// descendant classes.
    /// </remarks>
    [TestFixture]
    public abstract class TestBase
    {
        private readonly string _originalCurrentDirectory;
        private Lazy<EventMonitor> _eventMonitor = new Lazy<EventMonitor>(() => new EventMonitor());

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBase" /> class.
        /// </summary>
        //      [SuppressMessage("SonarLint.CodeSmell", "S4005: Call the overload that takes a 'System.Uri' as an argument instead.",
        //         Justification = "Needed to work around Unix/Linux base parsing and source is considered to be safe.")]
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
        /// Gets the monitored events.
        /// </summary>
        /// <value>
        /// The monitored events.
        /// </value>
        protected EventMonitor MonitoredEvents => _eventMonitor.Value;

        /// <summary>
        /// Code that executes after all of the test methods in the test class are executed.
        /// </summary>
        [OneTimeTearDown]
        public static void TestClassCleanup()
        {
            var tms = IocServiceLocator.Resolve<ITestMockingService>();

            // Cleanup any test specific mocks.
            tms.ResetIndividualTestContainers();
        }

        /// <summary>
        /// Code that executes before any of the tests methods in the test class are executed.
        /// </summary>
        /// <remarks>
        /// Note:  ClassInitializeAttribute is not inheritable.
        /// </remarks>
        [OneTimeSetUp]
        public static void TestClassInitialize()
        {
            // currently empty
        }

        /// <summary>
        /// Steps that are run after each test.
        /// </summary>
        [TearDown]
        public void TestCleanup()
        {
            TeardownTestMethod();
        }

        /// <summary>
        /// Steps that are run before each test.
        /// </summary>
        [SetUp]
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

        private void ApplyTestFilters()
        {
            var methodInfo = GetType().GetMethod(TestContext.CurrentContext.Test.Name);
            if (methodInfo != null)
            {
                var testFilters = (from a in methodInfo.GetCustomAttributes()
                                   let tf = a as TestFilterAttribute
                                   where tf != null
                                   select tf).ToList();

                var suppress = testFilters.Any(tf => tf.ReturnInconclusiveTestResult());
                if (suppress)
                {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    Assert.Inconclusive("This test has been suppressed by test filters evaluated against the runtime environment.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                }
            }
        }
    }
}
