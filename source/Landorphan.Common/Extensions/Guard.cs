namespace Landorphan.Common
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Globalization;
   using System.Linq;
   using Landorphan.Common.Resources;

   /// <summary>
   /// Argument validation extension methods.
   /// </summary>
   public static class Guard
   {
      /// <summary>
      /// Throws <see cref="ArgumentOutOfRangeException"/> when <paramref name="value"/> is not greater than <paramref name="comparand"/>.
      /// </summary>
      /// <exception cref="ArgumentOutOfRangeException">
      /// Thrown if <paramref name="value"/> is not greater than <paramref name="comparand"/>.
      /// </exception>
      /// <typeparam name="T">
      /// The type of the values to compare.
      /// </typeparam>
      /// <param name="value">
      /// The value to inspect.
      /// </param>
      /// <param name="comparand">
      /// The comparand.
      /// </param>
      /// <param name="paramName">
      /// The parameter name.
      /// </param>
      public static void ArgumentGreaterThan<T>(this T value, T comparand, String paramName) where T : struct, IComparable<T>
      {
         if (value.CompareTo(comparand) > 0)
         {
            return;
         }

         throw new ArgumentOutOfRangeException(
            paramName,
            value,
            String.Format(CultureInfo.InvariantCulture, StringResources.ValueMustBeGreaterThanFmt, comparand, value));
      }

      /// <summary>
      /// Throws <see cref="ArgumentOutOfRangeException"/> when <paramref name="value"/> is not greater than or equal to
      /// <paramref name="comparand"/>.
      /// </summary>
      /// <exception cref="ArgumentOutOfRangeException">
      /// Thrown if <paramref name="value"/> is not greater than or equal to <paramref name="comparand"/>.
      /// </exception>
      /// <typeparam name="T">
      /// The type of the values to compare.
      /// </typeparam>
      /// <param name="value">
      /// The value to inspect.
      /// </param>
      /// <param name="comparand">
      /// The comparand.
      /// </param>
      /// <param name="paramName">
      /// The parameter name.
      /// </param>
      public static void ArgumentGreaterThanOrEqualTo<T>(this T value, T comparand, String paramName) where T : struct, IComparable<T>
      {
         if (value.CompareTo(comparand) >= 0)
         {
            return;
         }

         throw new ArgumentOutOfRangeException(
            paramName,
            value,
            String.Format(CultureInfo.InvariantCulture, StringResources.ValueMustBeGreaterThanOrEqualToFmt, comparand, value));
      }

      /// <summary>
      /// Throws <see cref="ArgumentOutOfRangeException"/> when <paramref name="value"/> is not less than <paramref name="comparand"/>.
      /// </summary>
      /// <exception cref="ArgumentOutOfRangeException">
      /// Thrown if <paramref name="value"/> is not less than <paramref name="comparand"/>.
      /// </exception>
      /// <typeparam name="T">
      /// The type of the values to compare.
      /// </typeparam>
      /// <param name="value">
      /// The value to inspect.
      /// </param>
      /// <param name="comparand">
      /// The comparand.
      /// </param>
      /// <param name="paramName">
      /// The parameter name.
      /// </param>
      public static void ArgumentLessThan<T>(this T value, T comparand, String paramName) where T : struct, IComparable<T>
      {
         if (value.CompareTo(comparand) < 0)
         {
            return;
         }

         throw new ArgumentOutOfRangeException(
            paramName,
            value,
            String.Format(CultureInfo.InvariantCulture, StringResources.ValueMustBeLessThanFmt, comparand, value));
      }

      /// <summary>
      /// Throws <see cref="ArgumentOutOfRangeException"/> when <paramref name="value"/> is not less than or equal to
      /// <paramref name="comparand"/>.
      /// </summary>
      /// <exception cref="ArgumentOutOfRangeException">
      /// Thrown if <paramref name="value"/> is not less than or equal to <paramref name="comparand"/>.
      /// </exception>
      /// <typeparam name="T">
      /// The type of the values to compare.
      /// </typeparam>
      /// <param name="value">
      /// The value to inspect.
      /// </param>
      /// <param name="comparand">
      /// The comparand.
      /// </param>
      /// <param name="paramName">
      /// The parameter name.
      /// </param>
      public static void ArgumentLessThanOrEqualTo<T>(this T value, T comparand, String paramName) where T : struct, IComparable<T>
      {
         if (value.CompareTo(comparand) <= 0)
         {
            return;
         }

         throw new ArgumentOutOfRangeException(
            paramName,
            value,
            String.Format(CultureInfo.InvariantCulture, StringResources.ValueMustBeLessThanOrEqualToFmt, comparand, value));
      }

      /// <summary>
      /// Throws <see cref="ArgumentEmptyException"/> when the given string is empty, and
      /// <see cref="ArgumentEmptyException"/>  when the given string is entirely composed of white-space.
      /// </summary>
      /// <param name="value">
      /// The string to inspect.
      /// </param>
      /// <param name="paramName">
      /// The parameter name.
      /// </param>
      /// <remarks>
      /// Allows <c> null </c> values.
      /// </remarks>
      public static void ArgumentNotEmptyNorWhiteSpace(this String value, String paramName)
      {
         if (ReferenceEquals(value, null))
         {
            return;
         }

         if (value.Length == 0)
         {
            throw new ArgumentEmptyException(paramName ?? String.Empty, null, null);
         }

         if (value.Trim().Length == 0)
         {
            throw new ArgumentWhiteSpaceException(paramName ?? String.Empty, null, null);
         }
      }

      /// <summary>
      /// Throws <see cref="ArgumentNullException"/> when <paramref name="value"/> is null.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// Thrown if the argument is null.
      /// </exception>
      /// <param name="value">
      /// The value to inspect.
      /// </param>
      /// <param name="argumentName">
      /// The argumentName.
      /// </param>
      [DebuggerStepThrough]
      public static void ArgumentNotNull([ValidatedNotNull] this Object value, String argumentName)
      {
         if (ReferenceEquals(value, null))
         {
            throw new ArgumentNullException(argumentName ?? String.Empty);
         }
      }

      /// <summary>
      /// Throws <see cref="ArgumentNullException"/> when the given collection is null, and
      /// <see cref="ArgumentContainsNullException"/> when the given collection contains a null reference.
      /// </summary>
      /// <exception cref="ArgumentContainsNullException">
      /// Thrown if the argument contains a null.
      /// </exception>
      /// <typeparam name="T">
      /// The type of the values to compare.
      /// </typeparam>
      /// <param name="value">
      /// The collection to inspect.
      /// </param>
      /// <param name="paramName">
      /// The parameter name.
      /// </param>
      public static void ArgumentNotNullNorContainsNull<T>([ValidatedNotNull] this IEnumerable<T> value, String paramName) where T : class
      {
         value.ArgumentNotNull(paramName);

         if ((from v in value where ReferenceEquals(v, null) select v).Any())
         {
            throw new ArgumentContainsNullException(paramName ?? String.Empty, null, null);
         }
      }

      /// <summary>
      /// Throws <see cref="ArgumentNullException"/> when the given collection is null, and
      /// <see cref="ArgumentEmptyException"/> when the given collection is empty.
      /// </summary>
      /// <exception cref="ArgumentEmptyException">
      /// Thrown if the value is empty.
      /// </exception>
      /// <param name="value">
      /// The collection to inspect.
      /// </param>
      /// <param name="paramName">
      /// The parameter name.
      /// </param>
      public static void ArgumentNotNullNorEmpty([ValidatedNotNull] this IEnumerable value, String paramName)
      {
         value.ArgumentNotNull(paramName);
         if (!value.Cast<Object>().Any())
         {
            throw new ArgumentEmptyException(paramName ?? String.Empty, null, null);
         }
      }

      /// <summary>
      /// Throws <see cref="ArgumentNullException"/> when the given string is null, and
      /// <see cref="ArgumentEmptyException"/> when the given string is empty, and
      /// <see cref="ArgumentWhiteSpaceException"/>  when the given string is entirely composed of white-space.
      /// </summary>
      /// <param name="value">
      /// The string to inspect.
      /// </param>
      /// <param name="paramName">
      /// The parameter name.
      /// </param>
      public static void ArgumentNotNullNorEmptyNorWhiteSpace([ValidatedNotNull] this String value, String paramName)
      {
         value.ArgumentNotNullNorEmpty(paramName);
         if (value.Trim().Length == 0)
         {
            throw new ArgumentWhiteSpaceException(paramName ?? String.Empty, null, null);
         }
      }
   }
}