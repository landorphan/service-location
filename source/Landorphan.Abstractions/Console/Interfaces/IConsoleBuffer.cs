namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Security;

   /// <summary>
   /// Interface for console buffer.
   /// </summary>
   public interface IConsoleBuffer
   {
      /// <summary>
      /// Gets or sets the height of the buffer area.
      /// </summary>
      /// <value>
      /// The current height, in rows, of the buffer area.
      /// </value>
      /// <remarks>
      /// This property defines the number of rows (or lines) stored in the buffer that is accessed by a console mode window.  Input contrast, the
      /// <see cref="IConsoleAppearance.WindowHeight"/> property defines the number of rows that are actually displayed in the console window at any
      /// particular time. If the number of rows actually written to the buffer exceeds the number of rows defined by the
      /// <see cref="IConsoleAppearance.WindowHeight"/> property, the window can be scrolled vertically so that it displays a contiguous number of  rows that
      /// are equal to the <see cref="IConsoleAppearance.WindowHeight"/> property and are located anywhere in the buffer. If a set operation decreases the
      /// value of the <see cref="BufferHeight"/> property, the uppermost lines are removed.  For example, if the number of lines is reduced from 300 to 250,
      /// lines 0 through 49 are removed, and the existing lines 50 through 299 become lines 0 through 249.
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value in a set operation is less than or equal to zero.
      /// -or-
      /// The value in a set operation is greater than or equal to <see cref="Int16.MaxValue"/>.
      /// -or-
      /// The value in a set operation is less than <see cref="IConsoleAppearance.WindowTop"/> + <see cref="IConsoleAppearance.WindowHeight"/>.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Int32 BufferHeight { get; set; }

      /// <summary>
      /// Gets or sets the width of the buffer area.
      /// </summary>
      /// <value>
      /// The current width, in columns, of the buffer area.
      /// </value>
      /// <remarks>
      /// If a set operation decreases the value of the <see cref="BufferWidth"/> property, the rightmost columns are removed.  For example, if the number of
      /// columns is reduced from 80 to 60, columns 60 through 79 of each row are removed.
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value in a set operation is less than or equal to zero.
      /// -or-
      /// The value in a set operation is greater than or equal to <see cref="Int16.MaxValue"/>.
      /// -or-
      /// The value in a set operation is less than <see cref="IConsoleAppearance.WindowLeft"/> + <see cref="IConsoleAppearance.WindowWidth"/>.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Int32 BufferWidth { get; set; }

      /// <summary>
      /// Copies a specified source area of the screen buffer to a specified destination area.
      /// </summary>
      /// <param name="sourceLeft">
      /// The leftmost column of the source area.
      /// </param>
      /// <param name="sourceTop">
      /// The topmost row of the source area.
      /// </param>
      /// <param name="sourceWidth">
      /// The number of columns in the source area.
      /// </param>
      /// <param name="sourceHeight">
      /// The number of rows in the source area.
      /// </param>
      /// <param name="targetLeft">
      /// The leftmost column of the destination area.
      /// </param>
      /// <param name="targetTop">
      /// The topmost row of the destination area.
      /// </param>
      /// <remarks>
      /// If the destination and source parameters specify a position located outside the boundaries of the current screen buffer, only the portion of the
      /// source area that fits within the destination area is copied.  That is, the source area is clipped to fit the current screen buffer. The
      /// <see cref="MoveBufferArea(Int32,Int32,Int32,Int32,Int32,Int32)"/> method copies the source area to the destination area.  If the destination area
      /// does not
      /// intersect the source area, the source area is filled with blanks using the current foreground and background colors.  Otherwise, the intersected
      /// portion of the source area is not filled.
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// One or more of the parameters is less than zero.
      /// -or-
      /// sourceLeft or targetLeft is greater than or equal to BufferWidth.
      /// -or-
      /// sourceTop or targetTop is greater than or equal to BufferHeight.
      /// -or-
      /// sourceTop + sourceHeight is greater than or equal to BufferHeight.
      /// -or-
      /// sourceLeft + sourceWidth is greater than or equal to BufferWidth.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      void MoveBufferArea(Int32 sourceLeft, Int32 sourceTop, Int32 sourceWidth, Int32 sourceHeight, Int32 targetLeft, Int32 targetTop);

      /// <summary>
      /// Copies a specified source area of the screen buffer to a specified destination area.
      /// </summary>
      /// <param name="sourceLeft">
      /// The leftmost column of the source area.
      /// </param>
      /// <param name="sourceTop">
      /// The topmost row of the source area.
      /// </param>
      /// <param name="sourceWidth">
      /// The number of columns in the source area.
      /// </param>
      /// <param name="sourceHeight">
      /// The number of rows in the source area.
      /// </param>
      /// <param name="targetLeft">
      /// The leftmost column of the destination area.
      /// </param>
      /// <param name="targetTop">
      /// The topmost row of the destination area.
      /// </param>
      /// <param name="sourceChar">
      /// The character used to fill the source area.
      /// </param>
      /// <param name="sourceForeColor">
      /// The foreground color used to fill the source area.
      /// </param>
      /// <param name="sourceBackColor">
      /// The background color used to fill the source area.
      /// </param>
      /// <remarks>
      /// If the destination and source parameters specify a position located beyond the boundaries of the current screen buffer, only the portion of the
      /// source area that fits within the destination area is copied.  That is, the source area is clipped to fit the current screen buffer.  The
      /// <see cref="MoveBufferArea(Int32,Int32,Int32,Int32,Int32,Int32,Char,System.ConsoleColor,System.ConsoleColor)"/> method copies the source area to the
      /// destination
      /// area.  If the destination area does not intersect the source area, the source area is filled with the character specified by
      /// <paramref name="sourceChar"/>, using the colors specified by <paramref name="sourceForeColor"/> and <paramref name="sourceBackColor"/>.   Otherwise,
      /// the intersected portion of the source area is not filled.  The
      /// <see cref="MoveBufferArea(Int32,Int32,Int32,Int32,Int32,Int32,Char,System.ConsoleColor,System.ConsoleColor)"/> method performs no operation if
      /// <paramref name="sourceWidth"/> or <paramref name="sourceHeight"/> is zero.
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// One or more of the parameters is less than zero.
      /// -or-
      /// sourceLeft or targetLeft is greater than or equal to BufferWidth.
      /// -or-
      /// sourceTop or targetTop is greater than or equal to BufferHeight.
      /// -or-
      /// sourceTop + sourceHeight is greater than or equal to BufferHeight.
      /// -or-
      /// sourceLeft + sourceWidth is greater than or equal to BufferWidth.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// One or both of the color parameters is not a member of the <see cref="ConsoleColor"/> enumeration.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "char")]
      [SuppressMessage("SonarLint.CodeSmell", "S107: Methods should not have too many parameters")]
      void MoveBufferArea(
         Int32 sourceLeft,
         Int32 sourceTop,
         Int32 sourceWidth,
         Int32 sourceHeight,
         Int32 targetLeft,
         Int32 targetTop,
         Char sourceChar,
         ConsoleColor sourceForeColor,
         ConsoleColor sourceBackColor);

      /// <summary>
      /// Sets the height and width of the screen buffer area to the specified values.
      /// </summary>
      /// <param name="width">
      /// The width of the buffer area measured in columns.
      /// </param>
      /// <param name="height">
      /// The height of the buffer area measured in rows.
      /// </param>
      void SetBufferSize(Int32 width, Int32 height);
   }
}
