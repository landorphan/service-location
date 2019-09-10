using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Ioc.Example.ConsoleApp
{
   public interface IConsole
   {
      void Write(string message);
      void WriteLine();
      void WriteLine(string message);
   }
}
