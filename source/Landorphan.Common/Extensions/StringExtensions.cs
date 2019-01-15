namespace Landorphan.Common
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Security;
   using System.Text;

   /// <summary>
   /// Extension methods for working with <see cref="String"/> instances.
   /// </summary>
   public static class StringExtensions
   {
      /// <summary>
      /// Determines if a given <see cref="String"/>  is neither null nor empty.
      /// </summary>
      /// <param name="value">
      /// The value to evaluate.
      /// </param>
      /// <returns>
      /// <c> true </c> when the given string is something other than <c> null </c> or <see cref="string.Empty"/>; otherwise, <c> false </c>.
      /// </returns>
      public static Boolean IsNotNullNorEmpty([ValidatedNotNull] this String value)
      {
         return !String.IsNullOrEmpty(value);
      }

      /// <summary>
      /// Determines if a given <see cref="String"/>  is neither null nor empty nor composed entirely of white-space.
      /// </summary>
      /// <param name="value">
      /// The value to evaluate.
      /// </param>
      /// <returns>
      /// <c> true </c> when the given string is something other than <c> null </c> or <see cref="string.Empty"/> or composed entirely of whitespace;
      /// otherwise, <c> false </c>.
      /// </returns>
      public static Boolean IsNotNullNorEmptyNorWhiteSpace([ValidatedNotNull] this String value)
      {
         return (value ?? String.Empty).Trim().Length > 0;
      }

      /// <summary>
      /// Determines if a given <see cref="String"/>  is null nor empty.
      /// </summary>
      /// <param name="value">
      /// The value to evaluate.
      /// </param>
      /// <returns>
      /// <c> true </c> when the given string is <c> null </c> or <see cref="string.Empty"/>; otherwise, <c> false </c>.
      /// </returns>
      public static Boolean IsNullOrEmpty([ValidatedNotNull] this String value)
      {
         return String.IsNullOrEmpty(value);
      }

      /// <summary>
      /// Determines if a given <see cref="String"/>  is null, or empty, or composed entirely of white-space.
      /// </summary>
      /// <param name="value">
      /// The value to evaluate.
      /// </param>
      /// <returns>
      /// <c> true </c> when the given string is <c> null </c>, or <see cref="string.Empty"/> or composed entirely of white-space;
      /// otherwise, <c> false </c>.
      /// </returns>
      public static Boolean IsNullOrEmptyOrWhiteSpace([ValidatedNotNull] this String value)
      {
         return (value ?? String.Empty).Trim().Length == 0;
      }

      /// <summary>
      /// Trims all leading white space from the given string.
      /// </summary>
      /// <param name="value"> The value to inspect. </param>
      /// <returns>
      /// The <paramref name="value"/>without any leading white space.
      /// </returns>
      public static String LeftTrim(this String value)
      {
         if (value == null)
         {
            return null;
         }

         if (value.Length == 0)
         {
            return value;
         }

         var sb = new StringBuilder(value);
         while (sb.Length > 0 && Char.IsWhiteSpace(sb[0]))
         {
            sb.Remove(0, 1);
         }

         return sb.ToString();
      }

      /// <summary>
      /// Trims all leading instances of the given character from the given string.
      /// </summary>
      /// <param name="value"> The value to inspect. </param>
      /// <param name="character"> The character to remove. </param>
      /// <returns>
      /// The <paramref name="value"/>without any leading occurrences of the given <paramref name="character"/>.
      /// </returns>
      public static String LeftTrim(this String value, Char character)
      {
         return value.LeftTrim(new[] {character});
      }

      /// <summary>
      /// Trims all leading instances of the given characters from the given string.
      /// </summary>
      /// <param name="value"> The value to inspect. </param>
      /// <param name="characters"> The characters to remove. </param>
      /// <returns>
      /// The <paramref name="value"/>without any leading occurrences of the given <paramref name="characters"/>.
      /// </returns>
      public static String LeftTrim(this String value, IEnumerable<Char> characters)
      {
         characters.ArgumentNotNull(nameof(characters));
         var pinned = new HashSet<Char>(characters);

         if (value == null)
         {
            return null;
         }

         if (value.Length == 0)
         {
            return value;
         }

         var sb = new StringBuilder(value);
         while (sb.Length > 0 && pinned.Contains(sb[0]))
         {
            sb.Remove(0, 1);
         }

         return sb.ToString();
      }

      /// <summary>
      /// Reverses the specified value.
      /// </summary>
      /// <param name="value">
      /// The value.
      /// </param>
      /// <returns>
      /// The reverse value of the given string.
      /// </returns>
      public static String ReverseString([ValidatedNotNull] this String value)
      {
         value.ArgumentNotNull(nameof(value));
         var rv = new String(value.Reverse().ToArray());
         return rv;
      }

      /// <summary>
      /// Trims all trailing white space from the given string.
      /// </summary>
      /// <param name="value"> The value to inspect. </param>
      /// <returns>
      /// The <paramref name="value"/>without any trailing white space.
      /// </returns>
      public static String RightTrim(this String value)
      {
         if (value == null)
         {
            return null;
         }

         if (value.Length == 0)
         {
            return value;
         }

         var sb = new StringBuilder(value);
         while (sb.Length > 0 && Char.IsWhiteSpace(sb[sb.Length - 1]))
         {
            sb.Length = sb.Length - 1;
         }

         return sb.ToString();
      }

      /// <summary>
      /// Trims all trailing instances of the given character from the given string.
      /// </summary>
      /// <param name="value"> The value to inspect. </param>
      /// <param name="character"> The character to remove. </param>
      /// <returns>
      /// The <paramref name="value"/>without any leading occurrences of the given <paramref name="character"/>.
      /// </returns>
      public static String RightTrim(this String value, Char character)
      {
         return value.RightTrim(new[] {character});
      }

      /// <summary>
      /// Trims all trailing instances of the given characters from the given string.
      /// </summary>
      /// <param name="value">
      /// The value to inspect.
      /// </param>
      /// <param name="characters">
      /// The characters to remove.
      /// </param>
      /// <returns>
      /// The <paramref name="value"/>without any trailing occurrences of the given <paramref name="characters"/>.
      /// </returns>
      public static String RightTrim(this String value, IEnumerable<Char> characters)
      {
         characters.ArgumentNotNull(nameof(characters));
         var pinned = new HashSet<Char>(characters);

         if (value == null)
         {
            return null;
         }

         if (value.Length == 0)
         {
            return value;
         }

         var sb = new StringBuilder(value);
         while (sb.Length > 0 && pinned.Contains(sb[sb.Length - 1]))
         {
            sb.Length = sb.Length - 1;
         }

         var rv = sb.ToString();
         return rv;
      }

      /// <summary>
      /// A string extension method that converts the <paramref name="value"/> to a <see cref="SecureString"/>.
      /// </summary>
      /// <param name="value">
      /// The this value.
      /// </param>
      /// <returns>
      /// The given <paramref name="value"/> to its <see cref="SecureString"/> equivalent value.
      /// </returns>
      public static SecureString ToSecureString(this String value)
      {
         if (value == null)
         {
            return null;
         }

         var rv = new SecureString();
         try
         {
            foreach (var c in value)
            {
               rv.AppendChar(c);
            }

            return rv;
         }
         catch (Exception)
         {
            rv.Dispose();
            throw;
         }
      }

      /// <summary>
      /// Returns a new string in which all leading and trailing occurrences of whitespace, or converts null to an empty string.
      /// </summary>
      /// <param name="value">
      /// The value.
      /// </param>
      /// <returns>
      /// A new string in which all leading and trailing occurrences of whitespace, or converts null to an empty string.
      /// </returns>
      public static String TrimNullToEmpty(this String value)
      {
         var rv = (value ?? String.Empty).Trim();
         return rv;
      }

      /// <summary>
      /// Returns a new string in which all leading and trailing occurrences of whitespace, or null if the value is null.
      /// </summary>
      /// <param name="value"> The value. </param>
      /// <returns>
      /// A new string in which all leading and trailing occurrences of whitespace, or null if the value is null.
      /// </returns>
      public static String TrimNullToNull(this String value)
      {
         String rv = null;
         if (value != null)
         {
            rv = value.Trim();
         }

         return rv;
      }

      /// <summary>
      /// Returns a new string in which all leading and trailing occurrences of whitespace, or converts null to the specified string.
      /// </summary>
      /// <param name="value">
      /// The value.
      /// </param>
      /// <param name="nullReplacementValue">
      /// The value used to replace nulls.
      /// </param>
      /// <returns>
      /// A new string in which all leading and trailing occurrences of whitespace, or converts null to an empty string.
      /// </returns>
      public static String TrimNullToValue(this String value, String nullReplacementValue)
      {
         var rv = (value ?? nullReplacementValue.TrimNullToEmpty()).Trim();
         return rv;
      }
   }
}
