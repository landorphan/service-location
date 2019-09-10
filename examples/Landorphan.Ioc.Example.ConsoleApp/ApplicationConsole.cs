using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Ioc.Example.ConsoleApp
{
   public class ApplicationConsole : IConsole
   {
      public void Write(string message)
      {
         Console.Write(message);
      }

      public void WriteLine()
      {
         Console.WriteLine();
      }

      public void WriteLine(string message)
      {
         Console.WriteLine(message);
      }
   }
}
