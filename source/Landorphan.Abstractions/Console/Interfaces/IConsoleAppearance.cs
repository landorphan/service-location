namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;
   using System.IO;
   using System.Security;

   /// <summary>
   /// Interface for console definition.
   /// </summary>
   public interface IConsoleAppearance
   {
      /// <summary>
      /// Gets or sets the background color of the console.
      /// </summary>
      /// <value>
      /// A value that specifies the background color of the console; that is, the color that appears behind each character. The default is black.
      /// </value>
      /// <remarks>
      /// A change to the BackgroundColor property affects only output that is written to individual character cells after the background color is
      /// changed.  To change the background color of the console window as a whole, set the BackgroundColor property and call the
      /// <see cref="Clear"/> method.
      /// </remarks>
      /// <exception cref="ArgumentException">
      /// The color specified in a set operation is not a valid member of <see cref="ConsoleColor"/>.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      ConsoleColor BackgroundColor { get; set; }

      /// <summary>
      /// Gets or sets the foreground color of the console.
      /// </summary>
      /// <value>
      /// A <see cref="ConsoleColor"/> that specifies the foreground color of the console; that is, the color of each character that is displayed.
      /// The default is gray.
      /// </value>
      /// <remarks>
      /// A get operation for a Windows-based application, in which a console does not exist, returns <see cref="ConsoleColor.Gray"/>.
      /// </remarks>
      ConsoleColor ForegroundColor { get; set; }

      /// <summary>
      /// Gets the largest possible number of console window rows, based on the current font and screen resolution.
      /// </summary>
      /// <value>
      /// The height of the largest possible console window measured in rows.
      /// </value>
      Int32 LargestWindowHeight { get; }

      /// <summary>
      /// Gets the largest possible number of console window columns, based on the current font and screen resolution.
      /// </summary>
      /// <value>
      /// The width of the largest possible console window measured in columns.
      /// </value>
      Int32 LargestWindowWidth { get; }

      /// <summary>
      /// Gets or sets the title to display in the console title bar.
      /// </summary>
      /// <value>
      /// The String to be displayed in the title bar of the console. The maximum length of the title String is 24500 characters.
      /// </value>
      /// <exception cref="InvalidOperationException">
      /// In a get operation, the retrieved title is longer than 24500 characters.
      /// </exception>
      /// <exception cref="ArgumentOutOfRangeException">
      /// In a set operation, the specified title is longer than 24500 characters.
      /// </exception>
      /// <exception cref="ArgumentNullException">
      /// In a set operation, the specified title is <c> null </c>.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      String Title { get; set; }

      /// <summary>
      /// Gets or sets the height of the console window area.
      /// </summary>
      /// <value>
      /// The height of the console window measured in rows.
      /// </value>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value of the <see cref="WindowWidth"/> property or the value of the <see cref="WindowHeight"/> property is less than or equal to 0.
      /// -or-
      /// The value of the <see cref="WindowHeight"/> property plus the value of the <see cref="WindowTop"/> property is greater than or equal to
      /// <see cref="Int16.MaxValue"/>.
      /// -or-
      /// The value of the <see cref="WindowWidth"/> property or the value of the <see cref="WindowHeight"/> property is greater than the largest
      /// possible window width or height for the current screen resolution and console font.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Int32 WindowHeight { get; set; }

      /// <summary>
      /// Gets or sets the leftmost position of the console window area relative to the screen buffer.
      /// </summary>
      /// <value>
      /// The leftmost console window position measured in columns.
      /// </value>
      /// <remarks>
      /// The console represents a rectangular window into a larger rectangular buffer area.  Both the window and the buffer are measured vertically
      /// by their number of rows and horizontally by their number of columns.  The dimensions of the buffer area are defined by the
      /// <see cref="IConsoleBuffer.BufferHeight"/> and <see cref="IConsoleBuffer.BufferWidth"/> properties.  The dimensions of the console area are defined by
      /// the
      /// <see cref="WindowHeight"/> and <see cref="WindowWidth"/> properties.  The <see cref="WindowLeft"/> property determines which column of the
      /// buffer area is displayed in the first column of the console window. The value of the WindowLeft property can range from 0 to
      /// <see cref="IConsoleBuffer.BufferWidth"/> - <see cref="WindowWidth"/>.  Attempting to set it to a value outside that range throws an
      /// <see cref="ArgumentOutOfRangeException"/>.
      /// When a console window first opens, the default value of the <see cref="WindowLeft"/> property is zero, which indicates that the first
      /// column shown by the console corresponds to the first column (the column at position zero) in the buffer area.  The default width of both
      /// the console window and the buffer area is 80 columns.  This means that the <see cref="WindowLeft"/> property can be modified only if the
      /// console window is made narrower or the buffer area is made wider.
      /// Note that if the width of the buffer area exceeds the width of the console window, the value of the <see cref="WindowLeft"/> property is
      /// automatically adjusted when the user uses the horizontal scroll bar to define the window's relationship to the buffer area.
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// In a set operation, the value to be assigned is less than zero.
      /// -or-
      /// As a result of the assignment, <see cref="WindowLeft"/> plus <see cref="WindowWidth"/> would exceed <see cref="IConsoleBuffer.BufferWidth"/>.
      /// </exception>
      /// <exception cref="IOException">
      /// Error reading or writing information.
      /// </exception>
      Int32 WindowLeft { get; set; }

      /// <summary>
      /// Gets or sets the top position of the console window area relative to the screen buffer.
      /// </summary>
      /// <value>
      /// The uppermost console window position measured in rows.
      /// </value>
      /// <remarks>
      /// The console represents a rectangular window into a larger rectangular buffer area.  Both the window and the buffer are measured vertically
      /// by their number of rows and horizontally by their number of columns.  The dimensions of the buffer area are defined by the
      /// <see cref="IConsoleBuffer.BufferHeight"/> and <see cref="IConsoleBuffer.BufferWidth"/> properties.  The dimensions of the console area are defined by
      /// the
      /// <see cref="WindowHeight"/> and <see cref="WindowWidth"/> properties.  The <see cref="WindowTop"/> property determines which row of the
      /// buffer area is displayed in the first column of the console window.  The value of the <see cref="WindowTop"/> property can range from 0 to
      /// <see cref="IConsoleBuffer.BufferHeight"/> - <see cref="WindowHeight"/>.  Attempting to set it to a value outside that range throws an
      /// <see cref="ArgumentOutOfRangeException"/>.
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// In a set operation, the value to be assigned is less than zero.
      /// -or-
      /// As a result of the assignment, <see cref="WindowTop"/> plus <see cref="WindowHeight"/> would exceed <see cref="IConsoleBuffer.BufferHeight"/>.
      /// </exception>
      /// <exception cref="IOException">
      /// Error reading or writing information.
      /// </exception>
      Int32 WindowTop { get; set; }

      /// <summary>
      /// Gets or sets the width of the console window.
      /// </summary>
      /// <value>
      /// The width of the console window measured in columns.
      /// </value>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value of the <see cref="WindowWidth"/> property or the value of the <see cref="WindowHeight"/> property is less than or equal to 0.
      /// -or-
      /// The value of the <see cref="WindowHeight"/> property plus the value of the <see cref="WindowTop"/> property is greater than or equal to
      /// <see cref="Int16.MaxValue"/>.
      /// -or-
      /// The value of the <see cref="WindowWidth"/> property or the value of the <see cref="WindowHeight"/> property is greater than the largest
      /// possible window width or height for the current screen resolution and console font.
      /// </exception>
      /// <exception cref="IOException">
      /// Error reading or writing information.
      /// </exception>
      Int32 WindowWidth { get; set; }

      /// <summary>
      /// Clears the console buffer and corresponding console window of display information.
      /// </summary>
      /// <remarks>
      /// Using the <see cref="Clear"/> method is equivalent invoking the MS-DOS cls command in the command prompt window.  When the
      /// <see cref="Clear"/> method is called, the cursor automatically scrolls to the top-left corner of the window and the contents of the screen
      /// buffer are set to blanks using the current foreground and background colors.
      /// </remarks>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      void Clear();

      /// <summary>
      /// Sets the foreground and background console colors to their defaults.
      /// </summary>
      void ResetColor();

      /// <summary>
      /// Sets the position of the console window relative to the screen buffer.
      /// </summary>
      /// <param name="left">
      /// The column position of the upper left corner of the console window.
      /// </param>
      /// <param name="top">
      /// The row position of the upper left corner of the console window.
      /// </param>
      void SetWindowPosition(Int32 left, Int32 top);

      /// <summary>
      /// Sets the height and width of the console window to the specified values.
      /// </summary>
      /// <param name="width">
      /// The width of the console window measured in columns.
      /// </param>
      /// <param name="height">
      /// The height of the console window measured in rows.
      /// </param>
      void SetWindowSize(Int32 width, Int32 height);
   }
}
