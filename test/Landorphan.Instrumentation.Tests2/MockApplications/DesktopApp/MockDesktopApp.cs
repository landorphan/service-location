using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.MockApplications.DesktopApp
{
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.Instrumentation.Tests.HelperClasses;

   [LogMethod]
   public class MyFormBase
   {
      public void OnCreateControl()
      {

      }

      public void OnClosing(EventArgs args)
      {

      }
   }

   [LogMethod]
   public class MyForm : MyFormBase
   {

   }

   [LogMethod]
   public class MyForm1 : MyFormBase
   {

   }

   [LogMethod]
   public class MyForm2 : MyFormBase
   {

   }

   [LogMethod]
   public class MockDesktopApp
   {
      public static Dictionary<string, Type> FormsClasses = new Dictionary<string, Type>()
      {
         { typeof(MyForm).Name, typeof(MyForm) },
         { typeof(MyForm1).Name, typeof(MyForm1) },
         { typeof(MyForm2).Name, typeof(MyForm2) }
      };

      public object[] Args { get; set; }

      public MockDesktopApp(object[] args)
      {
         this.Args = args;
      }

      public void Run()
      {

      }

      #pragma warning disable CS0028
      public static MockDesktopApp Main(object[] args)
      {
         var retval = new MockDesktopApp(args);
         retval.Run();
         return retval;
      }
      #pragma warning restore CS0028
   }
}
