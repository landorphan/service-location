namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;

   /// <summary>
   /// Interface for console vwz.
   /// </summary>
   [CLSCompliant(false)]
   public interface IConsoleWrite
   {
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
      [CLSCompliant(false)]
      void Write(UInt32 value);

      /// <summary>
      /// Writes the text representation of the specified value to the standard output stream.
      /// </summary>
      /// <param name="value">
      /// The value to write.
      /// </param>
      [CLSCompliant(false)]
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
   }
}
