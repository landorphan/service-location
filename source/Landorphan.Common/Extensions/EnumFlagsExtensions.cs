namespace Landorphan.Common
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;

   /// <summary>
   /// Enumeration extensions for flags.
   /// </summary>
   /// <remarks>
   /// Generic constraints do not support 'enum', so restricting to struct, IComparable, IFormattable, IConvertible.  However, this still
   /// leaves the extension available to too many commonly used types, so placing these extensions in an isolated namespace.
   /// </remarks>
   public static class FlagsEnumExtensions
   {
      /// <summary>
      /// Validates a [Flags] enumeration argument value.
      /// </summary>
      /// <typeparam name="T">
      /// The flags enumeration type.
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
      /// when <typeparamref name="T"/> is not attributed with [Flags].
      /// </exception>
      // Constraint type 'System.IConvertible' is not CLS-compliant
      [CLSCompliant(false)]
      public static void ArgumentMustBeValidFlagsEnumValue<T>(this T value, String parameterName)
         where T : struct, IComparable, IFormattable, IConvertible
      {
         if (IsValidFlagsValue(value))
         {
            return;
         }

         // if reach here, T is an enum with [Flags] so it can be converted to a long value.
         throw new ExtendedInvalidEnumArgumentException(parameterName, Convert.ToInt64(value, CultureInfo.InvariantCulture), typeof(T));
      }

      /// <summary>
      /// Determines whether the given flag is set in the specified value.
      /// </summary>
      /// <typeparam name="T">
      /// The flags enumeration type.
      /// </typeparam>
      /// <param name="value">
      /// The value to inspect.
      /// </param>
      /// <param name="flag">
      /// The flag.
      /// </param>
      /// <returns>
      /// <c> true </c> when the flag is set; otherwise, <c> false </c>.
      /// </returns>
      [CLSCompliant(false)]
      public static Boolean IsFlagSet<T>(this T value, T flag) where T : struct, IComparable, IFormattable, IConvertible
      {
         var t = typeof(T);
         TypeMustBeFlagsEnumeration(t);

         var v = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
         var f = Convert.ToUInt64(flag, CultureInfo.InvariantCulture);

         return (v & f) == f;
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
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      [SuppressMessage("SonarLint.CodeSmell", "S138: Functions should not have too many lines of code")]
      [SuppressMessage("SonarLint.CodeSmell", "S126: 'if ... else if' constructs should end with 'else' clauses", Justification = "The chain is exhaustive (MWP)")]
      public static Boolean IsValidFlagsValue<T>(this T value) where T : struct, IComparable, IFormattable, IConvertible
      {
         var t = typeof(T);

         TypeMustBeFlagsEnumeration(t);

         unchecked
         {
            // Convert.To* was compiled with overflow checking turned on, must use casting in unchecked block to handle unsigned values.

            // BOXING/UNBOXING ISSUE here:  must first unbox the value as its base type, then cast it.
            var underlyingType = Enum.GetUnderlyingType(t);

            Int64 allValues = 0;
            foreach (var flag in Enum.GetValues(t))
            {
               Int64 convertedFlag = 0;
               if (underlyingType == typeof(SByte))
               {
                  convertedFlag = (SByte) flag;
               }
               else if (underlyingType == typeof(Byte))
               {
                  convertedFlag = (Byte) flag;
               }
               else if (underlyingType == typeof(Int16))
               {
                  convertedFlag = (Int16) flag;
               }
               else if (underlyingType == typeof(UInt16))
               {
                  convertedFlag = (UInt16) flag;
               }
               else if (underlyingType == typeof(Int32))
               {
                  convertedFlag = (Int32) flag;
               }
               else if (underlyingType == typeof(UInt32))
               {
                  convertedFlag = (UInt32) flag;
               }
               else if (underlyingType == typeof(Int64))
               {
                  convertedFlag = (Int64) flag;
               }
               else if (underlyingType == typeof(UInt64))
               {
                  convertedFlag = (Int64) (UInt64) flag;
               }

               allValues |= convertedFlag;
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

            // determine if any invalid bits are on.
            return (~allValues & castValue) == 0;
         }
      }

      private static void TypeMustBeFlagsEnumeration(Type t)
      {
         t.ArgumentNotNull(nameof(t));

         if (!t.IsEnum)
         {
            throw new InvalidOperationException(t.Name + " is not an enumeration type.");
         }

         if (!t.IsDefined(typeof(FlagsAttribute), false))
         {
            throw new InvalidOperationException(t.Name + " is not an attributed with [Flags].");
         }
      }
   }
}