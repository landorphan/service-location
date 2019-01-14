namespace Landorphan.Common.Threading
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Threading;

   /// <summary>
   /// An interlocked <see cref="DateTime"/>.
   /// </summary>
   public struct InterlockedDateTime
      : IComparable<DateTime>, IComparable<InterlockedDateTime>, IComparable, IEquatable<DateTime>, IEquatable<InterlockedDateTime>
   {
      private Int64 _value;

      /// <summary>
      /// Initializes a new instance of the <see cref="InterlockedDateTime"/> structure.
      /// </summary>
      /// <param name="ticks">
      /// The ticks.
      /// </param>
      public InterlockedDateTime(Int64 ticks)
      {
         _value = ticks;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InterlockedDateTime"/> structure.
      /// </summary>
      /// <param name="value">
      /// The initial value of the instance.
      /// </param>
      public InterlockedDateTime(DateTime value)
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
      public static Boolean operator ==(DateTime left, InterlockedDateTime right)
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
      public static Boolean operator ==(InterlockedDateTime left, DateTime right)
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
      public static Boolean operator ==(InterlockedDateTime left, InterlockedDateTime right)
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
      public static Boolean operator >(DateTime left, InterlockedDateTime right)
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
      public static Boolean operator >(InterlockedDateTime left, DateTime right)
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
      public static Boolean operator >(InterlockedDateTime left, InterlockedDateTime right)
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
      public static Boolean operator >=(DateTime left, InterlockedDateTime right)
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
      public static Boolean operator >=(InterlockedDateTime left, DateTime right)
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
      public static Boolean operator >=(InterlockedDateTime left, InterlockedDateTime right)
      {
         return left.CompareTo(right) >= 0;
      }

      /// <summary>
      /// Performs an implicit conversion from <see cref="InterlockedDateTime"/> to <see cref="System.DateTime"/>.
      /// </summary>
      /// <param name="value">
      /// The value.
      /// </param>
      /// <returns>
      /// The <see cref="DateTime"/> representation of the given <paramref name="value"/>.
      /// </returns>
      public static implicit operator DateTime(InterlockedDateTime value)
      {
         return value.GetValue();
      }

      /// <summary>
      /// Converts to the given <paramref name="value"/> to a <see cref="DateTime"/>.
      /// </summary>
      /// <param name="value">
      /// The value to convert.
      /// </param>
      /// <returns>
      /// The <see cref="DateTime"/> representation of the given <paramref name="value"/>.
      /// </returns>
      public static DateTime ToDateTime(InterlockedDateTime value)
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
      public static Boolean operator !=(DateTime left, InterlockedDateTime right)
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
      public static Boolean operator !=(InterlockedDateTime left, DateTime right)
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
      public static Boolean operator !=(InterlockedDateTime left, InterlockedDateTime right)
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
      public static Boolean operator <(DateTime left, InterlockedDateTime right)
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
      public static Boolean operator <(InterlockedDateTime left, DateTime right)
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
      public static Boolean operator <(InterlockedDateTime left, InterlockedDateTime right)
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
      public static Boolean operator <=(DateTime left, InterlockedDateTime right)
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
      public static Boolean operator <=(InterlockedDateTime left, DateTime right)
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
      public static Boolean operator <=(InterlockedDateTime left, InterlockedDateTime right)
      {
         return left.CompareTo(right) <= 0;
      }

      /// <inheritdoc/>
      public Int32 CompareTo(DateTime other)
      {
         return GetValue().ToUtc().CompareTo(other.ToUtc());
      }

      /// <inheritdoc/>
      public Int32 CompareTo(InterlockedDateTime other)
      {
         return GetValue().ToUtc().CompareTo(other.GetValue().ToUtc());
      }

      /// <inheritdoc/>
      public Boolean Equals(DateTime other)
      {
         return other.ToUtc().Equals(GetValue().ToUtc());
      }

      /// <inheritdoc/>
      public Boolean Equals(InterlockedDateTime other)
      {
         return Equals(other.GetValue());
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         if (obj is InterlockedDateTime idt)
         {
            return Equals(idt);
         }

         if (obj is DateTime datetime)
         {
            return Equals(datetime);
         }

         return false;
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
      public DateTime ExchangeValue(DateTime value)
      {
         var workingValue = value.ToUtc().Ticks;
         var was = Interlocked.Exchange(ref _value, workingValue);
         return new DateTime(was, DateTimeKind.Utc);
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
      public DateTime GetValue()
      {
         var raw = Thread.VolatileRead(ref _value);
         return new DateTime(raw, DateTimeKind.Utc);
      }

      /// <summary>
      /// Sets the value.
      /// </summary>
      /// <param name="value">
      /// The value to which the instance is set.
      /// </param>
      public void SetValue(DateTime value)
      {
         ExchangeValue(value.ToUtc());
      }

      /// <inheritdoc cref="DateTime.ToString()"/>
      public override String ToString()
      {
         return GetValue().ToRoundtripString();
      }

      /// <inheritdoc cref="DateTime.ToString(String)"/>
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

      /// <inheritdoc cref="DateTime.ToString(IFormatProvider)"/>
      public String ToString(IFormatProvider provider)
      {
         return GetValue().ToString(provider);
      }

      /// <inheritdoc cref="DateTime.ToString(String, IFormatProvider)"/>
      public String ToString(String format, IFormatProvider provider)
      {
         return GetValue().ToString(format, provider);
      }

      /// <inheritdoc/>
      Int32 IComparable.CompareTo(Object obj)
      {
         if (ReferenceEquals(obj, null))
         {
            return 1;
         }

         if (obj is InterlockedDateTime idt)
         {
            // implicit conversion does not avoid an argument exception.
            return GetValue().CompareTo(idt.GetValue());
         }

         if (obj is DateTime dateTime)
         {
            var value = dateTime.ToUtc();
            return GetValue().CompareTo(value);
         }

         throw new ArgumentException(@"Object must be of type DateTime or of type InterlockedDateTime.", nameof(obj));
      }
   }
}
