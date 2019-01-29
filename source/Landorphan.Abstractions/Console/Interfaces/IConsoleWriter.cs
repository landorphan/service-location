namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;

   /// <summary>
   /// Interface for console writer.
   /// </summary>
   [CLSCompliant(false)]
   public interface IConsoleWriter : IConsoleWrite, IConsoleWriteLine
   {
   }
}
