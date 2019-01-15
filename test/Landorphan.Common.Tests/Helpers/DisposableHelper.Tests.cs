namespace Landorphan.Common.Tests.Helpers
{
   using System;
   using System.Diagnostics;
   using System.Threading;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class DisposableHelper_Tests
   {
      [TestClass]
      public class When_I_call_DisposableHelp_SafeCreate : ArrangeActAssert
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DisposableHelp_SafeCreate_Func_and_the_ctor_throws()
         {
            try
            {
               DisposableHelper.SafeCreate(() => new TestTarget());
            }
            catch (InvalidOperationException ioe)
            {
               ioe.Message.Should().Be("Intentionally throwing from .ctor");
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void When_I_call_DisposableHelp_SafeCreate_generic_and_the_ctor_throws()
         {
            try
            {
               DisposableHelper.SafeCreate<TestTarget>();
            }
            catch (InvalidOperationException ioe)
            {
               ioe.Message.Should().Be("Intentionally throwing from .ctor");
            }
         }
      }

      private class TestTarget : IDisposable
      {
         private Mutex _disposable = new Mutex();

         public TestTarget()
         {
            throw new InvalidOperationException("Intentionally throwing from .ctor");
         }

         public void Dispose()
         {
            if (_disposable != null)
            {
               _disposable.Dispose();
               _disposable = null;
            }

            Trace.WriteLine("TestTarget.Dispose()");
         }
      }
   }
}
