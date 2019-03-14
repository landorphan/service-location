using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   using System.Linq;
   using System.Xml.Schema;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   internal class WindowsPathTokenizer : PathTokenizer
   {


      public WindowsPathTokenizer(string path) : base(PreParsePath(path))
      {

      }

      internal static string PreParsePath(string path)
      {
         if (path == null)
         {
            return null;
         }

         if (path.StartsWith(@"\\?\UNC\"))
         {
            // Converts the (\\?\UNC\server\...) pattern into (UNC:server\...)
            path = "UNC:" + path.Substring(8);
         }
         else if (path.StartsWith(@"\\?\", StringComparison.Ordinal))
         {
            // Converts the (\\?\C:...) pattern into (C:...)
            path = path.Substring(4);
         }
         else if (path.StartsWith(@"\\"))
         {
            // Converts the (\\server\...) pattern into (UNC:server\...)
            path = "UNC:" + path.Substring(2);
         }
         return path.Replace('\\', '/');
      }
      //private static readonly ParsedCharacter End = new ParsedCharacter(CharacterType.End, WindowsPathChars.Null);
      //private static readonly ParsedCharacter Null = new ParsedCharacter(CharacterType.Null, WindowsPathChars.Null);
      //private static readonly ParsedCharacter Period = new ParsedCharacter(CharacterType.Period, WindowsPathChars.Period);
      //private static readonly ParsedCharacter Separator = new ParsedCharacter(CharacterType.PathSeparator, WindowsPathChars.BackSlash);
      //private static readonly ParsedCharacter Colon = new ParsedCharacter(CharacterType.Colon, WindowsPathChars.Colon);
      //private static readonly ParsedCharacter QuestionMark = new ParsedCharacter(CharacterType.QuestionMark, WindowsPathChars.QuestionMark);

      //private bool questionMarkLegal = false;
      //private bool colonLegal = false;
      //private char[] path;
      //private int loc = 0;
      //private WindowsParseState state = WindowsParseState.Start;
      //private Queue<ISegment> segments = new Queue<ISegment>();
      //private ISegment currentSegment;

      //private ISegment NextSegment<T>(char character = default) where T : ISegment
      //{
      //   switch (typeof(T).Name)
      //   {
      //      case nameof(NullSegment):
      //         currentSegment = Segment.NullSegment;
      //         segments.Enqueue(currentSegment);
      //         break;
      //      case nameof(EmptySegment):
      //         currentSegment = Segment.EmptySegment;
      //         segments.Enqueue(currentSegment);
      //         break;
      //      case nameof(Segment):
      //         currentSegment = new Segment(character);
      //         segments.Enqueue(currentSegment);
      //         break;
      //   }
      //   return currentSegment;
      //}

      //public static readonly Func<char, bool> IsPathSeparator = c => c == WindowsPathChars.BackSlash || c == WindowsPathChars.ForwardSlash;
      //public static readonly Func<char, bool> IsColon = c => c == WindowsPathChars.Colon;
      //public static readonly Func<char, bool> IsPeriod = c => c == WindowsPathChars.Period;
      //public static readonly Func<char, bool> IsQuestionMark = c => c == WindowsPathChars.QuestionMark;
      //public bool IsIllegalCharacter(char character)
      //{
      //   if (IsQuestionMark(character) && !questionMarkLegal)
      //   {
      //      return true;
      //   }
      //   if (IsColon(character) && !colonLegal)
      //   {
      //      return true;
      //   }
      //   return WindowsPathChars.IllegalCharacters.Contains(character);
      //}
      //public bool IsLegalCharacter(char character)
      //{
      //   return !IsIllegalCharacter(character);
      //}

      //public WindowsPathTokenizer(string pathString)
      //{
      //   path = pathString.ToCharArray();
      //}

      //public IPath Parse()
      //{
      //   Path retval = new Path();
      //   ParsedCharacter parsed;

      //   if (path.Length == 0)
      //   {
      //      return Path.EmptyPath;
      //   }

      //   while ((parsed = Consume()).CharacterType != CharacterType.End)
      //   {
      //      switch (state)
      //      {
      //         case WindowsParseState.Start:
      //            //if (parsed.CharacterType == CharacterType.PathSeparator)
      //            //{
      //            //   if (Peek().CharacterType == CharacterType.PathSeparator)
      //            //   {

      //            //   }
      //            //   state = WindowsParseState.DoubleOrRoot;
      //            //}
      //            //if (parsed.CharacterType == CharacterType.LegalCharacter)
      //            //{
      //            //   NextSegment<Segment>(parsed.Character);
      //            //   questionMarkLegal = false;
      //            //   state = WindowsParseState.RelativeOrVolume;
      //            //}

      //            break;
      //         default:
      //            break;
      //      }
      //   }

      //   return retval;
      //}

      //public ParsedCharacter Consume()
      //{
      //   ParsedCharacter retval = Peek();
      //   loc++;
      //   return retval;
      //}

      //private ParsedCharacter Peek()
      //{
      //   if (this.loc >= this.path.Length)
      //   {
      //      return End;
      //   }

      //   var character = this.path[loc];
      //   if (character == null)
      //   {
      //      return Null;
      //   }

      //   if (IsPathSeparator(character))
      //   {
      //      return Separator;
      //   }

      //   if (IsPeriod(character))
      //   {
      //      return Period;
      //   }

      //   if (IsIllegalCharacter(character))
      //   {
      //      return new ParsedCharacter(CharacterType.IllegalCharacter, character);
      //   }

      //   if (IsColon(character))
      //   {
      //      return Colon;
      //   }

      //   if (IsQuestionMark(character))
      //   {
      //      return QuestionMark;
      //   }

      //   return new ParsedCharacter(CharacterType.LegalCharacter, character);
      //}
   }
}
