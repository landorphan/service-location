// ReSharper disable once CheckNamespace
namespace Landorphan.TestUtilities
{
   using System;
   using System.IO;
   using System.Runtime.InteropServices;
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

      /// <summary>
      /// Initializes a new instance of the <see cref="TestBase" /> class.
      /// </summary>
      protected TestBase()
      {
         // ReSharper disable once AssignNullToNotNullAttribute
         if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            var codeBase = Path.GetDirectoryName(GetType().Assembly.GetName().CodeBase);
            var builder = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(builder.Path);
            _originalCurrentDirectory = path;
         }
         else
         {
            var uri = new Uri(Path.GetDirectoryName(GetType().Assembly.GetName().CodeBase));
            _originalCurrentDirectory = uri.LocalPath;
         }
      }

      /// <summary>
      /// Gets or sets the test context which provides information about and functionality for the current test run.
      /// </summary>
      public TestContext TestContext { get; protected set; }

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
      }

      /// <summary>
      /// Called once after each test method invocation.
      /// </summary>
      protected virtual void TeardownTestMethod()
      {
         // nothing to dispose
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
         // currently nothing injected by TestClassInitialize()
      }
   }
}
