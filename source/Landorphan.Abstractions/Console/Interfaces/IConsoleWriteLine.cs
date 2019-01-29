namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;

   /// <summary>
   /// Interface for console stu.
   /// </summary>
   [CLSCompliant(false)]
   public interface IConsoleWriteLine
   {
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
      [CLSCompliant(false)]
      void WriteLine(UInt32 value);

      /// <summary>
      /// Writes the text representation of the specified value, followed by the current line terminator, to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      [CLSCompliant(false)]
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
      /// Writes the specified subarray of Unicode characters, followed by the current line terminator, to the standard output stream.
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
