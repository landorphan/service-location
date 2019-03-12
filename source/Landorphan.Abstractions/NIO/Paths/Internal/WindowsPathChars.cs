namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   internal class WindowsPathChars
   {
      public const char Null = (char) 0x0;
      public const char Period = '.';
      public const char BackSlash = '\\';
      public const char ForwardSlash = '/';
      public const char Colon = ':';
      public const char DoubleQuote = '"';
      public const char VerticalBar = '|';
      public const char QuestionMark = '?';
      public const char Asterisk = '*';
      public const char LessThanSign = '<';
      public const char GreaterThanSign = '>';
      public static readonly char[] IllegalCharacters = new[] {Null, DoubleQuote, VerticalBar, Asterisk, LessThanSign, GreaterThanSign};
   }
}