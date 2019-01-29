namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Security;
   using System.Text;

   /// <summary>
   /// Interface for console streams.
   /// </summary>
   public interface IConsoleStreams
   {
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
      [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error")]
      TextWriter Error { get; }

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
      /// Of the Unicode encodings, <see cref="IConsole"/> supports UTF-8 encoding with the <see cref="UTF8Encoding"/> class and,
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
      /// Gets or sets a value indicating whether the combination of the <see cref="ConsoleModifiers.Control"/> modifier key and C console key
      /// (Ctrl+C) is treated as ordinary input or as an interruption that is handled by the operating system.
      /// </summary>
      /// <value>
      /// <c> true </c> if Ctrl+C is treated as ordinary input; otherwise, <c> false </c>.
      /// </value>
      Boolean TreatControlCAsInput { get; set; }

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
      /// Sets the <see cref="Error"/> property to the specified <see cref="TextWriter"/> Object.
      /// </summary>
      /// <param name="newError">
      /// A stream that is the new standard error output.
      /// </param>
      void SetError(TextWriter newError);

      /// <summary>
      /// Sets the <see cref="Input"/> property to the specified <see cref="TextReader"/> Object.
      /// </summary>
      /// <param name="newInput">
      /// A stream that is the new standard input.
      /// </param>
      void SetInput(TextReader newInput);

      /// <summary>
      /// Sets the <see cref="Output"/>  property to the specified <see cref="TextWriter"/> Object.
      /// </summary>
      /// <param name="newOutput">
      /// The new out.
      /// </param>
      void SetOutput(TextWriter newOutput);
   }
}
