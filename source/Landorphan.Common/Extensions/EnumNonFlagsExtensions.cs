namespace Landorphan.Common
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;

   /// <summary>
   /// Enumeration extensions for flags.
   /// </summary>
   /// <remarks>
   /// Generic constraints do not support 'enum', so restricting to struct, IComparable, IFormattable, IConvertible.  However, this still leaves the
   /// extension available to too many commonly used types, so placing these extensions in an isolated namespace.
   /// </remarks>
   public static class NonFlagsEnumExtensions
   {
      /// <summary>
      /// Validates a non-[Flags] enumeration argument value.
      /// </summary>
      /// <typeparam name="T">
      /// The enumeration type.
      /// </typeparam>
      /// <param name="value">
      /// The value to be validated.
      /// </param>
      /// <param name="parameterName">
      /// The name of the parameter.
      /// </param>
      /// <exception cref="ExtendedInvalidEnumArgumentException">
      /// Thrown when the value is not valid.
      /// </exception>
      /// <exception cref="InvalidOperationException">
      /// Thrown when <typeparamref name="T"/> is not an enumeration,
      /// -or-
      /// when <typeparamref name="T"/> is attributed with [Flags].
      /// </exception>
      // Constraint type 'System.IConvertible' is not CLS-compliant
      [CLSCompliant(false)]
      public static void ArgumentMustBeValidEnumValue<T>(this T value, String parameterName)
         where T : struct, IComparable, IFormattable, IConvertible
      {
         if (IsValidEnumValue(value))
         {
            return;
         }

         throw new ExtendedInvalidEnumArgumentException(parameterName, Convert.ToInt64(value, CultureInfo.InvariantCulture), typeof(T));
      }

      /// <summary>
      /// Determines whether a the given value is a valid combination of flag values.
      /// </summary>
      /// <typeparam name="T">
      /// The flags enumeration type.
      /// </typeparam>
      /// <param name="value">
      /// The value to be validated.
      /// </param>
      /// <returns>
      /// <c> true </c> when <paramref name="value"/> is a valid combination of flag values; otherwise, <c> false </c>.
      /// </returns>
      /// <exception cref="InvalidOperationException">
      /// Thrown when <typeparamref name="T"/> is not an enumeration,
      /// -or-
      /// when <typeparamref name="T"/> is not attributed with [Flags].
      /// </exception>
      // Constraint type 'System.IConvertible' is not CLS-compliant
      [CLSCompliant(false)]
      [SuppressMessage("SonarLint.CodeSmell", "S126: 'if ... else if' constructs should end with 'else' clauses", Justification = "The chain is exhaustive (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      public static Boolean IsValidEnumValue<T>(this T value) where T : struct, IComparable, IFormattable, IConvertible
      {
         var t = typeof(T);

         TypeMustBeNonFlagsEnumeration(t);

         unchecked
         {
            // Convert.To* was compiled with overflow checking turned on, must use casting in unchecked block to handle unsigned values.

            // BOXING/UNBOXING ISSUE here:  must first unbox the value as its base type, then cast it.
            var underlyingType = Enum.GetUnderlyingType(t);

            var allValues = new HashSet<Int64>();

            foreach (var v in Enum.GetValues(t))
            {
               Int64 convertedValue = 0;
               if (underlyingType == typeof(SByte))
               {
                  convertedValue = (SByte) v;
               }
               else if (underlyingType == typeof(Byte))
               {
                  convertedValue = (Byte) v;
               }
               else if (underlyingType == typeof(Int16))
               {
                  convertedValue = (Int16) v;
               }
               else if (underlyingType == typeof(UInt16))
               {
                  convertedValue = (UInt16) v;
               }
               else if (underlyingType == typeof(Int32))
               {
                  convertedValue = (Int32) v;
               }
               else if (underlyingType == typeof(UInt32))
               {
                  convertedValue = (UInt32) v;
               }
               else if (underlyingType == typeof(Int64))
               {
                  convertedValue = (Int64) v;
               }
               else if (underlyingType == typeof(UInt64))
               {
                  convertedValue = (Int64) (UInt64) v;
               }

               allValues.Add(convertedValue);
            }

            Int64 castValue = 0;

            if (underlyingType == typeof(SByte))
            {
               castValue = (SByte) (Object) value;
            }
            else if (underlyingType == typeof(Byte))
            {
               castValue = (Byte) (Object) value;
            }
            else if (underlyingType == typeof(Int16))
            {
               castValue = (Int16) (Object) value;
            }
            else if (underlyingType == typeof(UInt16))
            {
               castValue = (UInt16) (Object) value;
            }
            else if (underlyingType == typeof(Int32))
            {
               castValue = (Int32) (Object) value;
            }
            else if (underlyingType == typeof(UInt32))
            {
               castValue = (UInt32) (Object) value;
            }
            else if (underlyingType == typeof(Int64))
            {
               castValue = (Int64) (Object) value;
            }
            else if (underlyingType == typeof(UInt64))
            {
               castValue = (Int64) (UInt64) (Object) value;
            }

            return allValues.Contains(castValue);
         }
      }

      private static void TypeMustBeNonFlagsEnumeration(Type t)
      {
         t.ArgumentNotNull(nameof(t));

         if (!t.IsEnum)
         {
            throw new InvalidOperationException(t.Name + " is not an enumeration type.");
         }

         if (t.IsDefined(typeof(FlagsAttribute), false))
         {
            throw new InvalidOperationException(t.Name + " is attributed with [Flags].");
         }
      }
   }
}