namespace Landorphan.Abstractions.Tests.Console
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
   [SuppressMessage("SonarLint.CodeSmell", "S2187: TestCases should contain tests")]
   [TestClass]
   public class ConsoleTestBase : TestBase
   {
      private TextWriter _originalErr;
      private TextReader _originalIn;
      private TextWriter _originalOut;

      protected StringWriter ErrorWriter { get; private set; }

      protected StreamReader InReader { get; private set; }

      protected StringWriter OutWriter { get; private set; }

      [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
      protected override void InitializeTestMethod()
      {
         _originalErr = Console.Error;
         _originalIn = Console.In;
         _originalOut = Console.Out;

         ErrorWriter = new StringWriter();
         InReader = new StreamReader(new MemoryStream());
         OutWriter = new StringWriter();

         Console.SetError(ErrorWriter);
         Console.SetIn(InReader);
         Console.SetOut(OutWriter);

         base.InitializeTestMethod();
      }

      protected override void TeardownTestMethod()
      {
         Console.SetError(_originalErr);
         Console.SetIn(_originalIn);
         Console.SetOut(_originalOut);

         ErrorWriter.Dispose();
         ErrorWriter = null;

         InReader.Dispose();
         InReader = null;

         OutWriter.Dispose();
         OutWriter = null;

         base.TeardownTestMethod();
      }
   }
}
