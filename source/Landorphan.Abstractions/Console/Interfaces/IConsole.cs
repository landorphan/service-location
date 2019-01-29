namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;

   /// <summary>
   /// Interface for the console.
   /// </summary>
   [CLSCompliant(false)]
   public interface IConsole : IConsoleAppearance, IConsoleBuffer, IConsoleCursor, IConsoleStreams, IConsoleReader, IConsoleWriter, IConsoleMisc
   {
   }
}
