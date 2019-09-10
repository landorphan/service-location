using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Ioc.Example.Tests.Simulators
{
   using Landorphan.Ioc.Example.ConsoleApp;

   class ConsoleSimulator : IConsole
   {
      private StringBuilder builder = new StringBuilder();

      public void Clear()
      {
         this.builder = new StringBuilder();
      }

      public string GetConsoleOutput()
      {
         return this.builder.ToString();
      }

      public void Write(string message)
      {
         this.builder.Append(message);
      }

      public void WriteLine()
      {
         this.builder.AppendLine();
      }

      public void WriteLine(string message)
      {
         this.builder.AppendLine(message);
      }
   }
}
