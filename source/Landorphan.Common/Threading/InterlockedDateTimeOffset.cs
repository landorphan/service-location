namespace Landorphan.Common.Threading
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.Threading;

   /// <summary>
   /// An interlocked <see cref="DateTimeOffset"/>.
   /// </summary>
   public struct InterlockedDateTimeOffset
      : IComparable,
         IComparable<DateTimeOffset>,
         IComparable<InterlockedDateTimeOffset>,
         IEquatable<DateTimeOffset>,
         IEquatable<InterlockedDateTimeOffset>
   {
      private Int64 _value;

      /// <summary>
      /// Initializes a new instance of the <see cref="InterlockedDateTimeOffset"/> structure.
      /// </summary>
      /// <param name="value">
      /// The initial value of the instance.
      /// </param>
      public InterlockedDateTimeOffset(DateTime value)
      {
         _value = value.ToUtc().Ticks;
      }

      /// <summary>
      /// Equality operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the instances are equal, otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator ==(DateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.Equals(right.GetValue());
      }

      /// <summary>
      /// Equality operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the instances are equal, otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator ==(InterlockedDateTimeOffset left, DateTimeOffset right)
      {
         return left.Equals(right);
      }

      /// <summary>
      /// Equality operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the instances are equal, otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator ==(InterlockedDateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.Equals(right);
      }

      /// <summary>
      /// Greater-than operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is greater-than <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator >(DateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.CompareTo(right.GetValue()) > 0;
      }

      /// <summary>
      /// Greater-than operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is greater-than <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator >(InterlockedDateTimeOffset left, DateTimeOffset right)
      {
         return left.GetValue().CompareTo(right) > 0;
      }

      /// <summary>
      /// Greater-than operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is greater-than <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator >(InterlockedDateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.CompareTo(right) > 0;
      }

      /// <summary>
      /// Greater-than-or-equal-to operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is Greater-than-or-equal-to <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator >=(DateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.CompareTo(right.GetValue()) >= 0;
      }

      /// <summary>
      /// Greater-than-or-equal-to operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is Greater-than-or-equal-to <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator >=(InterlockedDateTimeOffset left, DateTimeOffset right)
      {
         return left.GetValue().CompareTo(right) >= 0;
      }

      /// <summary>
      /// Greater-than-or-equal-to operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is Greater-than-or-equal-to <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator >=(InterlockedDateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.CompareTo(right) >= 0;
      }

      /// <summary>
      /// Performs an implicit conversion from <see cref="InterlockedDateTimeOffset"/> to <see cref="DateTimeOffset"/>.
      /// </summary>
      /// <param name="value">
      /// The value to convert.
      /// </param>
      /// <returns>
      /// The <see cref="DateTimeOffset"/> representation of the given <paramref name="value"/>.
      /// </returns>
      public static implicit operator DateTimeOffset(InterlockedDateTimeOffset value)
      {
         return value.GetValue();
      }

      /// <summary>
      /// Converts to <paramref name="value"/> to a <see cref="DateTimeOffset"/> representation.
      /// </summary>
      /// <param name="value">
      /// The value to convert.
      /// </param>
      /// <returns>
      /// The <see cref="DateTimeOffset"/> representation of the given <paramref name="value"/>.
      /// </returns>
      public static DateTimeOffset ToDateTimeOffset(InterlockedDateTimeOffset value)
      {
         return value.GetValue();
      }

      /// <summary>
      /// Inequality operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the instances are not equal, otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator !=(DateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return !left.Equals(right.GetValue());
      }

      /// <summary>
      /// Inequality operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the instances are not equal, otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator !=(InterlockedDateTimeOffset left, DateTimeOffset right)
      {
         return !left.Equals(right);
      }

      /// <summary>
      /// Inequality operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the instances are not equal, otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator !=(InterlockedDateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return !left.Equals(right);
      }

      /// <summary>
      /// Less-than operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is less-than <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator <(DateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.CompareTo(right.GetValue()) < 0;
      }

      /// <summary>
      /// Less-than operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is less-than <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator <(InterlockedDateTimeOffset left, DateTimeOffset right)
      {
         return left.GetValue().CompareTo(right) < 0;
      }

      /// <summary>
      /// Less-than operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is less-than <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator <(InterlockedDateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.CompareTo(right) < 0;
      }

      /// <summary>
      /// Less-than-or-equal-to operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is Less-than-or-equal-to <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator <=(DateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.CompareTo(right.GetValue()) <= 0;
      }

      /// <summary>
      /// Less-than-or-equal-to operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is Less-than-or-equal-to <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator <=(InterlockedDateTimeOffset left, DateTimeOffset right)
      {
         return left.GetValue().CompareTo(right) <= 0;
      }

      /// <summary>
      /// Less-than-or-equal-to operator overload.
      /// </summary>
      /// <param name="left">
      /// The left value.
      /// </param>
      /// <param name="right">
      /// The right value.
      /// </param>
      /// <returns>
      /// <c> true </c> when the <paramref name="left"/> is Less-than-or-equal-to <paramref name="right"/> , otherwise <c> false </c> .
      /// </returns>
      public static Boolean operator <=(InterlockedDateTimeOffset left, InterlockedDateTimeOffset right)
      {
         return left.CompareTo(right) <= 0;
      }

      /// <inheritdoc/>
      public Int32 CompareTo(DateTimeOffset other)
      {
         return GetValue().CompareTo(other);
      }

      /// <inheritdoc/>
      public Int32 CompareTo(InterlockedDateTimeOffset other)
      {
         return GetValue().CompareTo(other.GetValue());
      }

      /// <inheritdoc/>
      public Boolean Equals(DateTimeOffset other)
      {
         return other.Equals(GetValue());
      }

      /// <inheritdoc/>
      public Boolean Equals(InterlockedDateTimeOffset other)
      {
         return Equals(other.GetValue());
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         return obj is InterlockedDateTimeOffset idt ? Equals(idt) : obj is DateTimeOffset dto && Equals(dto);
      }

      /// <summary>
      /// Exchanges the value.
      /// </summary>
      /// <param name="value">
      /// The value to which the instance is set.
      /// </param>
      /// <returns>
      /// The original value.
      /// </returns>
      public DateTimeOffset ExchangeValue(DateTimeOffset value)
      {
         var workingValue = value.ToUniversalTime().Ticks;
         var was = Interlocked.Exchange(ref _value, workingValue);
         return new DateTimeOffset(new DateTime(was, DateTimeKind.Utc));
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         var raw = Thread.VolatileRead(ref _value);
         return raw.GetHashCode();
      }

      /// <summary>
      /// Gets the value.
      /// </summary>
      /// <returns>
      /// The current value.
      /// </returns>
      public DateTimeOffset GetValue()
      {
         var raw = Thread.VolatileRead(ref _value);
         return new DateTimeOffset(new DateTime(raw, DateTimeKind.Utc));
      }

      /// <summary>
      /// Sets the value.
      /// </summary>
      /// <param name="value">
      /// The value to which the instance is set.
      /// </param>
      public void SetValue(DateTimeOffset value)
      {
         ExchangeValue(value);
      }

      /// <inheritdoc cref="DateTimeOffset.ToString()"/>
      [SuppressMessage(
         "Microsoft.Globalization",
         "CA1305:SpecifyIFormatProvider",
         Justification = "Matching expected pattern, code analysis will inform consumers of the better course (MWP)")]
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S4056:Overloads with a 'CultureInfo' or an 'IFormatProvider' parameter should be used",
         Justification = "Matching expected pattern, code analysis will inform consumers of the better course (MWP)")]
      public override String ToString()
      {
         return GetValue().ToString(CultureInfo.InvariantCulture);
      }

      /// <inheritdoc cref="DateTimeOffset.ToString(String)"/>
      [SuppressMessage(
         "Microsoft.Globalization",
         "CA1305:SpecifyIFormatProvider",
         Justification = "Matching expected pattern, code analysis will inform consumers of the better course (MWP)")]
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S4056:Overloads with a 'CultureInfo' or an 'IFormatProvider' parameter should be used",
         Justification = "Matching expected pattern, code analysis will inform consumers of the better course (MWP)")]
      public String ToString(String format)
      {
         return GetValue().ToString(format);
      }

      /// <inheritdoc cref="DateTimeOffset.ToString(IFormatProvider)"/>
      public String ToString(IFormatProvider formatProvider)
      {
         return GetValue().ToString(formatProvider);
      }

      /// <inheritdoc cref="DateTimeOffset.ToString(String, IFormatProvider)"/>
      public String ToString(String format, IFormatProvider formatProvider)
      {
         return GetValue().ToString(format, formatProvider);
      }

      /// <inheritdoc/>
      Int32 IComparable.CompareTo(Object obj)
      {
         if (ReferenceEquals(obj, null))
         {
            return 1;
         }

         if (obj is InterlockedDateTimeOffset dto)
         {
            // implicit conversion does not avoid an argument exception.
            return GetValue().CompareTo(dto.GetValue());
         }

         if (obj is DateTimeOffset)
         {
            return (GetValue() as IComparable).CompareTo(obj);
         }

         throw new ArgumentException(@"Object must be of type DateTimeOffset or of type InterlockedDateTimeOffset.", nameof(obj));
      }
   }
}
