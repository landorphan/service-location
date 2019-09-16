namespace Landorphan.InstrumentationManagement.Tests.MockApplications
{
   using System;
   using System.Collections.Generic;
   using Landorphan.InstrumentationManagement.Tests.Aspects;

   [LogMethod]
   public class MyFormBase
   {
      public void OnCreateControl()
      {
         // Method intentionally left empty as this strictly simulates a form
         // but doesn't actually crate one or leverage the windows subsystem.
      }

      public void OnClosing(EventArgs args)
      {
         // Method intentionally left empty as this strictly simulates a form
         // but doesn't actually crate one or leverage the windows subsystem.
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
      public static Dictionary<string, Type> FormsClasses = new Dictionary<string, Type>
      {
         { typeof(MyForm).Name, typeof(MyForm) },
         { typeof(MyForm1).Name, typeof(MyForm1) },
         { typeof(MyForm2).Name, typeof(MyForm2) }
      };

      private object[] Args { get; set; }

      public MockDesktopApp(object[] args)
      {
         this.Args = args;
      }

      public void Run()
      {
         // Method intentionally left empty as this strictly simulates an application
         // but doesn't actually crate one or leverage the windows subsystem.
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
