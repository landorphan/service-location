namespace Landorphan.Abstractions.Internal
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Security;
   using System.Text;
   using Landorphan.Common.Interfaces;

   /// <summary>
   /// Represents the internal mapping from the static BCL <see cref="Console"/> class to an interface.
   /// </summary>
   internal interface IConsoleInternalMapping : IQueryDisposable
   {
      /// <summary>
      /// Occurs when the Control modifier key (Ctrl) and either the ConsoleKey.C console key (C) or the Break key are pressed simultaneously (Ctrl+C or
      /// Ctrl+Break).
      /// </summary>
      /// <remarks>
      /// <p>
      /// This event is used in conjunction with <see cref="EventHandler"/> and <see cref="ConsoleCancelEventArgs"/>.  The CancelKeyPress event
      /// enables a console application to intercept the Ctrl+C signal so the event handler can decide whether to continue executing or terminate.
      /// For more information about handling events,
      /// <see href="http://msdn.microsoft.com/en-us/library/edzehd2t(v=vs.110).aspx">
      /// Handling and Raising Events
      /// </see>
      /// </p>
      /// <p>
      /// When the user presses either Ctrl+C or Ctrl+Break, the CancelKeyPress event is fired and the application's
      /// <see cref="EventHandler{ConsoleCancelEventArgs}"/> event handler is executed.  The event handler is passed a
      /// <see cref="ConsoleCancelEventArgs"/> Object that has two useful properties:
      /// </p>
      /// • <see cref="ConsoleCancelEventArgs.SpecialKey"/>, which allows you to determine whether the handler was invoked as a result of the user
      /// pressing Ctrl+C (the property value is ConsoleSpecialKey.ControlC) or Ctrl+Break (the property value is ConsoleSpecialKey.ControlBreak).
      /// <p>
      /// • <see cref="ConsoleCancelEventArgs.Cancel"/>, which allows you to determine how to your application should respond to the user pressing
      /// Ctrl+C or Ctrl+Break.  By default, the Cancel property is false, which causes program execution to terminate when the event handler exits.
      /// Changing its property to true specifies that the application should continue to execute.
      /// </p>
      /// <p>
      /// The event handler for this event is executed on a thread pool thread.
      /// </p>
      /// </remarks>
      event EventHandler<ConsoleCancelEventArgs> CancelKeyPress;

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
      /// Gets or sets the height of the buffer area.
      /// </summary>
      /// <value>
      /// The current height, in rows, of the buffer area.
      /// </value>
      /// <remarks>
      /// This property defines the number of rows (or lines) stored in the buffer that is accessed by a console mode window.  Input contrast, the
      /// <see cref="WindowHeight"/> property defines the number of rows that are actually displayed in the console window at any particular time.
      /// If the number of rows actually written to the buffer exceeds the number of rows defined by the <see cref="WindowHeight"/> property, the
      /// window can be scrolled vertically so that it displays a contiguous number of rows that are equal to the <see cref="WindowHeight"/> property
      /// and are located anywhere in the buffer.
      /// If a set operation decreases the value of the <see cref="BufferHeight"/> property, the uppermost lines are removed.  For example, if the
      /// number of lines is reduced from 300 to 250, lines 0 through 49 are removed, and the existing lines 50 through 299 become lines 0
      /// through 249.
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value in a set operation is less than or equal to zero.
      /// -or-
      /// The value in a set operation is greater than or equal to <see cref="Int16.MaxValue"/>.
      /// -or-
      /// The value in a set operation is less than <see cref="WindowTop"/> + <see cref="WindowHeight"/>.
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
      /// If a set operation decreases the value of the <see cref="BufferWidth"/> property, the rightmost columns are removed.  For example, if the
      /// number of columns is reduced from 80 to 60, columns 60 through 79 of each row are removed.
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value in a set operation is less than or equal to zero.
      /// -or-
      /// The value in a set operation is greater than or equal to <see cref="Int16.MaxValue"/>.
      /// -or-
      /// The value in a set operation is less than <see cref="WindowLeft"/> + <see cref="WindowWidth"/>.
      /// </exception>
      /// <exception cref="SecurityException">
      /// The user does not have permission to perform this action.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Int32 BufferWidth { get; set; }

      /// <summary>
      /// Gets a value indicating whether the CAPS LOCK keyboard toggle is turned on or turned off.
      /// </summary>
      /// <value>
      /// <c> true </c> if CAPS LOCK is turned on; false if CAPS LOCK is turned <c> off </c>.
      /// </value>
      Boolean CapsLock { get; }

      /// <summary>
      /// Gets or sets the column position of the cursor within the buffer area.
      /// </summary>
      /// <value>
      /// The current position, in columns, of the cursor.
      /// </value>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value in a set operation is less than zero.
      /// -or-
      /// The value in a set operation is greater than or equal to <see cref="BufferWidth"/>.
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
      /// The value in a set operation is greater than or equal to <see cref="BufferHeight"/>.
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
      /// Gets the standard error output stream.
      /// </summary>
      /// <value>
      /// A <see cref="TextWriter"/> that represents the standard error output stream.
      /// </value>
      /// <remarks>
      /// This standard error stream is set to the console by default.  It can be set to another stream with the <see cref="SetError"/> method. After
      /// the standard error stream is redirected, it can be reacquired by calling the <see cref="OpenStandardError()"/> method.
      /// </remarks>
      TextWriter Error { get; }

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
      /// Gets the standard input stream.
      /// </summary>
      /// <value>
      /// A <see cref="TextWriter"/> that represents the standard input stream.
      /// </value>
      /// <remarks>
      /// This property is set to the standard input stream by default.  This property can be set to another stream with the <see cref="SetInput"/>
      /// method.  After the standard input stream is redirected, it can be reacquired by calling the <see cref="OpenStandardInput()"/> method.
      /// Read operations on the standard input stream execute synchronously.  That is, they block until the specified read operation has completed.
      /// This is true even if an asynchronous method, such as <see cref="TextReader.ReadLineAsync()"/>, is called on the <see cref="TextReader"/>
      /// Object returned by the <see cref="Input"/> property.
      /// </remarks>
      TextReader Input { get; }

      /// <summary>
      /// Gets or sets the encoding the console uses to read input.
      /// </summary>
      /// <value>
      /// The encoding used to read console input.
      /// </value>
      /// <remarks>
      /// The console uses the input encoding to translate keyboard input into a corresponding character.  The input encoding incorporates a code
      /// page that maps 256 keyboard character codes to individual characters.  Different code pages include different special characters,
      /// typically customized for a language or a group of languages.
      /// Starting with the .NET Framework 4, a property get operation may return a cached value instead of the console's current input encoding.
      /// This can occur if the value of the <see cref="InputEncoding"/> property is modified by some means other than an assignment to the
      /// <see cref="InputEncoding"/> property, such as calling the Windows <b> SetConsoleCP </b> function or using the <b> chcp </b> command from a
      /// PowerShell script.
      /// </remarks>
      /// <exception cref="ArgumentNullException">
      /// The property value in a set operation is <c> null </c>.
      /// </exception>
      /// <exception cref="SecurityException">
      /// Your application does not have permission to perform this operation.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Encoding InputEncoding { get; set; }

      /// <summary>
      /// Gets a value that indicates whether the error output stream has been redirected from the standard error stream.
      /// </summary>
      /// <value>
      /// <c> true </c> if error output is redirected; otherwise, <c> false </c>.
      /// </value>
      Boolean IsErrorRedirected { get; }

      /// <summary>
      /// Gets a value that indicates whether input has been redirected from the standard input stream.
      /// </summary>
      /// <value>
      /// <c> true </c> if input is redirected; otherwise, <c> false </c>.
      /// </value>
      Boolean IsInputRedirected { get; }

      /// <summary>
      /// Gets a value that indicates whether output has been redirected from the standard output stream.
      /// </summary>
      /// <value>
      /// <c> true </c> if output is redirected; otherwise, <c> false </c>.
      /// </value>
      Boolean IsOutputRedirected { get; }

      /// <summary>
      /// Gets a value indicating whether a key press is available in the input stream.
      /// </summary>
      /// <value>
      /// <c> true </c> if a key press is available; otherwise, <c> false </c>.
      /// </value>
      Boolean KeyAvailable { get; }

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
      /// Gets a value indicating whether the NUM LOCK keyboard toggle is turned on or turned off.
      /// </summary>
      /// <value>
      /// <c> true </c> if NUM LOCK is turned on; <c> false </c> if NUM LOCK is turned off.
      /// </value>
      Boolean NumberLock { get; }

      /// <summary>
      /// Gets the standard output stream.
      /// </summary>
      /// <value>
      /// A <see cref="TextWriter"/> that represents the standard output stream.
      /// </value>
      /// <remarks>
      /// This property is set to the standard output stream by default.  This property can be set to another stream with the <see cref="SetOutput"/>
      /// method.
      /// Note that calls to <b> Output.WriteLine </b> methods are equivalent to calls to the corresponding
      /// <see cref="TextWriter.WriteLine()"/> methods.
      /// </remarks>
      TextWriter Output { get; }

      /// <summary>
      /// Gets or sets the encoding the console uses to write output.
      /// </summary>
      /// <value>
      /// The encoding used to write console output.
      /// </value>
      /// <remarks>
      /// The console uses the output encoding to translate characters written by an application into corresponding console display characters.  The
      /// default code page that the console uses is determined by the system locale.
      /// Starting with the .NET Framework 4, a property get operation may return a cached value instead of the console's current output encoding.
      /// This can occur if the value of the <see cref="OutputEncoding"/> property is modified by some means other than an assignment to the
      /// <see cref="OutputEncoding"/> property, such as calling the Windows <b> SetConsoleOutputCP </b> function.
      /// <b> Notes to Callers </b>
      /// Of the Unicode encodings, <see cref="IConsoleInternalMapping"/> supports UTF-8 encoding with the <see cref="UTF8Encoding"/> class and,
      /// starting with the .NET Framework 4.5, it supports UTF-16 encoding with the <see cref="UnicodeEncoding"/> class.  UTF-32 encoding with the
      /// <see cref="UTF32Encoding"/> class is not supported.  Attempting to set the output encoding to UTF-32 throws an <see cref="IOException"/>.
      /// Note that successfully displaying Unicode characters to the console requires the following:
      /// • The console must use a TrueType font, such as Lucida Console or Consolas, to display characters.
      /// • A font used by the console must define the particular glyph or glyphs to be displayed. The console can take advantage of font linking to
      /// display glyphs from linked fonts if the base font does not contain a definition for that glyph.
      /// For more information about support for Unicode encoding by the console, see the "Unicode Support for the Console" section in the
      /// <see cref="Console"/> class.
      /// </remarks>
      /// <exception cref="ArgumentNullException">
      /// The property value in a set operation is <c> null </c>.
      /// </exception>
      /// <exception cref="SecurityException">
      /// Your application does not have permission to perform this operation.
      /// </exception>
      /// <exception cref="IOException">
      /// An I/O error occurred.
      /// </exception>
      Encoding OutputEncoding { get; set; }

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
      /// Gets or sets a value indicating whether the combination of the <see cref="ConsoleModifiers.Control"/> modifier key and C console key
      /// (Ctrl+C) is treated as ordinary input or as an interruption that is handled by the operating system.
      /// </summary>
      /// <value>
      /// <c> true </c> if Ctrl+C is treated as ordinary input; otherwise, <c> false </c>.
      /// </value>
      Boolean TreatControlCAsInput { get; set; }

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
      /// <see cref="BufferHeight"/> and <see cref="BufferWidth"/> properties.  The dimensions of the console area are defined by the
      /// <see cref="WindowHeight"/> and <see cref="WindowWidth"/> properties.  The <see cref="WindowLeft"/> property determines which column of the
      /// buffer area is displayed in the first column of the console window. The value of the WindowLeft property can range from 0 to
      /// <see cref="BufferWidth"/> - <see cref="WindowWidth"/>.  Attempting to set it to a value outside that range throws an
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
      /// As a result of the assignment, <see cref="WindowLeft"/> plus <see cref="WindowWidth"/> would exceed <see cref="BufferWidth"/>.
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
      /// <see cref="BufferHeight"/> and <see cref="BufferWidth"/> properties.  The dimensions of the console area are defined by the
      /// <see cref="WindowHeight"/> and <see cref="WindowWidth"/> properties.  The <see cref="WindowTop"/> property determines which row of the
      /// buffer area is displayed in the first column of the console window.  The value of the <see cref="WindowTop"/> property can range from 0 to
      /// <see cref="BufferHeight"/> - <see cref="WindowHeight"/>.  Attempting to set it to a value outside that range throws an
      /// <see cref="ArgumentOutOfRangeException"/>.
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// In a set operation, the value to be assigned is less than zero.
      /// -or-
      /// As a result of the assignment, <see cref="WindowTop"/> plus <see cref="WindowHeight"/> would exceed <see cref="BufferHeight"/>.
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
      /// Plays the sound of a beep through the console speaker.
      /// </summary>
      /// <remarks>
      /// <para>
      /// By default, the beep plays at a frequency of 800 hertz for a duration of 200 milliseconds.
      /// Whether Beep produces a sound on versions of Windows before Windows 7 depends on the
      /// presence of a 8254 programmable interval timer chip.  Starting with Windows 7, it depends on the default sound device.
      /// </para>
      /// <para>
      /// In .Net Framework, this would throw System.Security.HostProtectionException when executed on a server, such as MS SQL Server, that does not permit access to a user interface.
      /// TODO: find out what exception is thrown in .Net Standard 2.0
      /// </para>
      /// </remarks>
      void Beep();

      /// <summary>
      /// Plays the sound of a beep of a specified frequency and duration through the console speaker.
      /// </summary>
      /// <param name="frequency">
      /// The frequency of the beep, ranging from 37 to 32767 hertz.
      /// </param>
      /// <param name="duration">
      /// The duration of the beep measured in milliseconds.
      /// </param>
      /// <remarks>
      /// Beep wraps a call to the Windows Beep function.
      /// Whether Beep produces a sound on versions of Windows before Windows 7 depends on the
      /// presence of a 8254 programmable interval timer chip.  Starting with Windows 7, it depends on the default sound device.
      /// <para>
      /// In .Net Framework, this would throw System.Security.HostProtectionException when executed on a server, such as MS SQL Server, that does not permit access to a user interface.
      /// TODO: find out what exception is thrown in .Net Standard 2.0
      /// </para>
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// <i> frequency </i> is less than 37 or more than 32767 hertz.
      /// -or-
      /// <i> duration </i> is less than or equal to zero.
      /// </exception>
      void Beep(Int32 frequency, Int32 duration);

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
      /// If the destination and source parameters specify a position located outside the boundaries of the current screen buffer, only the portion
      /// of the source area that fits within the destination area is copied.  That is, the source area is clipped to fit the current screen buffer.
      /// The <see cref="MoveBufferArea(Int32,Int32,Int32,Int32,Int32,Int32)"/> method copies the source area to the destination area.  If the destination area
      /// does not
      /// intersect the
      /// source area, the source area is filled with blanks using the current foreground and background colors.  Otherwise, the intersected portion
      /// of the source area is not filled.
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
      /// If the destination and source parameters specify a position located beyond the boundaries of the current screen buffer, only the portion of
      /// the source area that fits within the destination area is copied.  That is, the source area is clipped to fit the current screen buffer.
      /// The <see cref="MoveBufferArea(Int32,Int32,Int32,Int32,Int32,Int32,Char,System.ConsoleColor,System.ConsoleColor)"/> method copies the source area to
      /// the
      /// destination area.  If the destination area does not intersect
      /// the source area, the source area is filled with the character specified by <paramref name="sourceChar"/>, using the colors specified by
      /// <paramref name="sourceForeColor"/> and <paramref name="sourceBackColor"/>.  Otherwise, the intersected portion of the source area is not
      /// filled.
      /// The <see cref="MoveBufferArea(Int32,Int32,Int32,Int32,Int32,Int32,Char,System.ConsoleColor,System.ConsoleColor)"/> method performs no operation if
      /// <paramref name="sourceWidth"/> or <paramref name="sourceHeight"/>
      /// is zero.
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
      /// Acquires the standard error stream.
      /// </summary>
      /// <remarks>
      /// This method can be used to reacquire the standard error stream after it has been changed by the <see cref="SetError"/> method.
      /// </remarks>
      /// <returns>
      /// The standard error stream.
      /// </returns>
      Stream OpenStandardError();

      /// <summary>
      /// Acquires the standard error stream.
      /// </summary>
      /// <param name="bufferSize">
      /// The internal stream buffer size.
      /// </param>
      /// <remarks>
      /// This method can be used to reacquire the standard error stream after it has been changed by the <see cref="SetError"/> method.
      /// </remarks>
      /// <returns>
      /// The standard error stream.
      /// </returns>
      Stream OpenStandardError(Int32 bufferSize);

      /// <summary>
      /// Acquires the standard input stream.
      /// </summary>
      /// <remarks>
      /// This method can be used to reacquire the standard error stream after it has been changed by the <see cref="SetInput"/> method.
      /// </remarks>
      /// <returns>
      /// The standard input stream.
      /// </returns>
      Stream OpenStandardInput();

      /// <summary>
      /// Acquires the standard input stream.
      /// </summary>
      /// <param name="bufferSize">
      /// The internal stream buffer size.
      /// </param>
      /// <remarks>
      /// This method can be used to reacquire the standard input stream after it has been changed by the <see cref="SetInput"/> method.
      /// </remarks>
      /// <returns>
      /// The standard input stream.
      /// </returns>
      Stream OpenStandardInput(Int32 bufferSize);

      /// <summary>
      /// Acquires the standard output stream.
      /// </summary>
      /// <remarks>
      /// This method can be used to reacquire the standard output stream after it has been changed by the <see cref="SetOutput"/> method.
      /// </remarks>
      /// <returns>
      /// The standard output stream.
      /// </returns>
      Stream OpenStandardOutput();

      /// <summary>
      /// Acquires the standard output stream.
      /// </summary>
      /// <param name="bufferSize">
      /// The internal stream buffer size.
      /// </param>
      /// <remarks>
      /// This method can be used to reacquire the standard output stream after it has been changed by the <see cref="SetOutput"/> method.
      /// </remarks>
      /// <returns>
      /// The standard output stream.
      /// </returns>
      Stream OpenStandardOutput(Int32 bufferSize);

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

      /// <summary>
      /// Sets the foreground and background console colors to their defaults.
      /// </summary>
      void ResetColor();

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

      /// <summary>
      /// Sets the <see cref="Error"/> property to the specified <see cref="TextWriter"/> Object.
      /// </summary>
      /// <param name="newError">
      /// A stream that is the new standard error output.
      /// </param>
      void SetError(TextWriter newError);

      /// <summary>
      /// Sets the <see cref="Input"/> property to the specified <see cref="TextReader"/> Object.
      /// </summary>
      /// <param name="newIn">
      /// A stream that is the new standard input.
      /// </param>
      void SetInput(TextReader newIn);

      /// <summary>
      /// Sets the <see cref="Output"/>  property to the specified <see cref="TextWriter"/> Object.
      /// </summary>
      /// <param name="newOut">
      /// The new out.
      /// </param>
      void SetOutput(TextWriter newOut);

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

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(Boolean value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(Char value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="buffer">
      /// The buffer to write.
      /// </param>
      void Write(Char[] buffer);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(Decimal value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(Double value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(Int32 value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(Int64 value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write or <c> null </c>.
      /// </param>
      void Write(Object value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(Single value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(String value);

      /// <summary>
      /// Writes the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(UInt32 value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void Write(UInt64 value);

      /// <summary>
      /// Writes the text representation of the specified Object to the standard output stream using the specified format information.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="arg0">
      /// An argument to write using <paramref name="format"/>.
      /// </param>
      void Write(String format, Object arg0);

      /// <summary>
      /// Writes the text representation of the specified values to the standard output stream using the specified format information.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="arg">
      /// An array of objects to write using <paramref name="format"/>.
      /// </param>
      void Write(String format, params Object[] arg);

      /// <summary>
      /// Writes the text representation of the specified Object to the standard output stream.
      /// </summary>
      /// <param name="buffer">
      /// An array of Unicode characters.
      /// </param>
      /// <param name="index">
      /// The starting position in <paramref name="buffer"/>.
      /// </param>
      /// <param name="count">
      /// The number of characters to write.
      /// </param>
      void Write(Char[] buffer, Int32 index, Int32 count);

      /// <summary>
      /// Writes the text representation of the specified objects to the standard output stream.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="arg0">
      /// The first Object to write using <paramref name="format"/>.
      /// </param>
      /// <param name="arg1">
      /// The second Object to write using <paramref name="format"/>.
      /// </param>
      void Write(String format, Object arg0, Object arg1);

      /// <summary>
      /// Writes the text representation of the specified objects to the standard output stream.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="arg0">
      /// The first Object to write using <paramref name="format"/>.
      /// </param>
      /// <param name="arg1">
      /// The second Object to write using <paramref name="format"/>.
      /// </param>
      /// <param name="arg2">
      /// The third Object to write using <paramref name="format"/>.
      /// </param>
      void Write(String format, Object arg0, Object arg1, Object arg2);

      /// <summary>
      /// Writes the current line terminator to the standard output stream.
      /// </summary>
      void WriteLine();

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(Boolean value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(Char value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="buffer">
      /// The buffer to write.
      /// </param>
      void WriteLine(Char[] buffer);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(Decimal value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(Double value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(Int32 value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(Int64 value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write or <c> null </c>.
      /// </param>
      void WriteLine(Object value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(Single value);

      /// <summary>
      /// Writes the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(String value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(UInt32 value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      void WriteLine(UInt64 value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="arg0">
      /// An argument to write using <paramref name="format"/>.
      /// </param>
      void WriteLine(String format, Object arg0);

      /// <summary>
      /// Writes the text representation of the specified values, followed by the current line terminator, to the standard output stream using the
      /// specified format information.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="arg">
      /// An array of objects to write using <paramref name="format"/>.
      /// </param>
      void WriteLine(String format, params Object[] arg);

      /// <summary>
      /// Writes the specified sub-array of Unicode characters, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="buffer">
      /// An array of Unicode characters.
      /// </param>
      /// <param name="index">
      /// The starting position in <paramref name="buffer"/>.
      /// </param>
      /// <param name="count">
      /// The number of characters to write.
      /// </param>
      void WriteLine(Char[] buffer, Int32 index, Int32 count);

      /// <summary>
      /// Writes the text representation of the specified values, followed by the current line terminator, to the standard output stream using the
      /// specified format information.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="arg0">
      /// An argument to write using <paramref name="format"/>.
      /// </param>
      /// <param name="arg1">
      /// The second Object to write using <paramref name="format"/>.
      /// </param>
      void WriteLine(String format, Object arg0, Object arg1);

      /// <summary>
      /// Writes the text representation of the specified values, followed by the current line terminator, to the standard output stream using the
      /// specified format information.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="arg0">
      /// An argument to write using <paramref name="format"/>.
      /// </param>
      /// <param name="arg1">
      /// The second Object to write using <paramref name="format"/>.
      /// </param>
      /// <param name="arg2">
      /// The third Object to write using <paramref name="format"/>.
      /// </param>
      void WriteLine(String format, Object arg0, Object arg1, Object arg2);
   }
}
