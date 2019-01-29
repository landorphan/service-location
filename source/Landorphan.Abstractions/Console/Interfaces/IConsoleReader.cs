namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;

   /// <summary>
   /// Interface for console reader.
   /// </summary>
   public interface IConsoleReader
   {
      /// <summary>
      /// Gets a value indicating whether a key press is available in the input stream.
      /// </summary>
      /// <value>
      /// <c> true </c> if a key press is available; otherwise, <c> false </c>.
      /// </value>
      Boolean KeyAvailable { get; }

      /// <summary>
      /// Reads the next character from the standard input stream.
      /// </summary>
      /// <remarks>
      /// The <see cref="Read"/> method blocks its return while you type input characters; it terminates when you press the Enter key. Pressing Enter
      /// appends a platform-dependent line termination sequence to your input (for example, Windows appends a carriage return-linefeed sequence).
      /// Subsequent calls to the <see cref="Read"/> method retrieve your input one character at a time.  After the final character is retrieved,
      /// <see cref="Read"/> blocks its return again and the cycle repeats.
      /// </remarks>
      /// <returns>
      /// The next character from the input stream, or negative one (-1) if there are currently no more characters to be read.
      /// </returns>
      Int32 Read();

      /// <summary>
      /// Obtains the next character or function key pressed by the user.  The pressed key is displayed in the console window.
      /// </summary>
      /// <returns>
      /// A ConsoleKeyInfo Object that describes the ConsoleKey constant and Unicode character, if any, that correspond to the pressed console key.
      /// The ConsoleKeyInfo Object also describes, in a bitwise combination of ConsoleModifiers values, whether one or more Shift, Alt, or Ctrl
      /// modifier keys was pressed simultaneously with the console key.
      /// </returns>
      ConsoleKeyInfo ReadKey();

      /// <summary>
      /// Obtains the next character or function key pressed by the user.  The pressed key is optionally displayed in the console window.
      /// </summary>
      /// <param name="intercept">
      /// Determines whether to display the pressed key in the console window. <c> true </c> to not display the pressed key; otherwise, <c> false </c>.
      /// </param>
      /// <returns>
      /// A ConsoleKeyInfo Object that describes the ConsoleKey constant and Unicode character, if any, that correspond to the pressed console key.
      /// The ConsoleKeyInfo Object also describes, in a bitwise combination of ConsoleModifiers values, whether one or more Shift, Alt, or Ctrl modifier
      /// keys was pressed simultaneously with the console key.
      /// </returns>
      ConsoleKeyInfo ReadKey(Boolean intercept);

      /// <summary>
      /// Reads the next line of characters from the standard input stream.
      /// </summary>
      /// <returns>
      /// The next line of characters from the input stream, or <c> null </c> if no more lines are available.
      /// </returns>
      String ReadLine();
   }
}
