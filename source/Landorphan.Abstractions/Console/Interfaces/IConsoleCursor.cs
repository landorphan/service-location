namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;
   using System.IO;
   using System.Security;

   /// <summary>
   /// Interface for console mno.
   /// </summary>
   public interface IConsoleCursor
   {
      /// <summary>
      /// Gets or sets the column position of the cursor within the buffer area.
      /// </summary>
      /// <value>
      /// The current position, in columns, of the cursor.
      /// </value>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value in a set operation is less than zero.
      /// -or-
      /// The value in a set operation is greater than or equal to <see cref="IConsoleBuffer.BufferWidth"/>.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Int32 CursorLeft { get; set; }

      /// <summary>
      /// Gets or sets the height of the cursor within a character cell.
      /// </summary>
      /// <value>
      /// The size of the cursor expressed as a percentage of the height of a character cell.  The property value ranges from 1 to 100.
      /// </value>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value specified in a set operation is less than 1 or greater than 100.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Int32 CursorSize { get; set; }

      /// <summary>
      /// Gets or sets the row position of the cursor within the buffer area.
      /// </summary>
      /// <value>
      /// The current position, in rows, of the cursor.
      /// </value>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value in a set operation is less than zero.
      /// -or-
      /// The value in a set operation is greater than or equal to <see cref="IConsoleBuffer.BufferHeight"/>.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Int32 CursorTop { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether the cursor is visible.
      /// </summary>
      /// <value>
      /// <c> true </c> if the cursor is visible; otherwise, <c> false </c>.
      /// </value>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Boolean CursorVisible { get; set; }

      /// <summary>
      /// Sets the position of the cursor.
      /// </summary>
      /// <param name="left">
      /// The column position of the cursor. Columns are numbered from left to right starting at 0.
      /// </param>
      /// <param name="top">
      /// The row position of the cursor. Rows are numbered from top to bottom starting at 0.
      /// </param>
      void SetCursorPosition(Int32 left, Int32 top);
   }
}
