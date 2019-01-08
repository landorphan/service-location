namespace Landorphan.Common.Threading
{
   using System;
   using System.Globalization;
   using System.Threading;

   /// <summary>
   /// An interlocked <see cref="bool"/>.
   /// </summary>
   public struct InterlockedBoolean
      : IComparable<Boolean>, IComparable<InterlockedBoolean>, IComparable, IEquatable<Boolean>, IEquatable<InterlockedBoolean>
   {
      private Int32 _value;

      /// <summary>
      /// Initializes a new instance of the <see cref="InterlockedBoolean"/> structure.
      /// </summary>
      /// <param name="value">
      /// The initial value of the instance.
      /// </param>
      public InterlockedBoolean(Boolean value)
      {
         _value = value ? 1 : 0;
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
      public static Boolean operator ==(Boolean left, InterlockedBoolean right)
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
      public static Boolean operator ==(InterlockedBoolean left, Boolean right)
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
      public static Boolean operator ==(InterlockedBoolean left, InterlockedBoolean right)
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
      public static Boolean operator >(Boolean left, InterlockedBoolean right)
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
      public static Boolean operator >(InterlockedBoolean left, Boolean right)
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
      public static Boolean operator >(InterlockedBoolean left, InterlockedBoolean right)
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
      public static Boolean operator >=(Boolean left, InterlockedBoolean right)
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
      public static Boolean operator >=(InterlockedBoolean left, Boolean right)
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
      public static Boolean operator >=(InterlockedBoolean left, InterlockedBoolean right)
      {
         return left.CompareTo(right) >= 0;
      }

      /// <summary>
      /// Performs an implicit conversion from <see cref="InterlockedBoolean"/> to <see cref="Boolean"/>.
      /// </summary>
      /// <param name="value">
      /// The value to convert.
      /// </param>
      /// <returns>
      /// The <see cref="Boolean"/> representation of the current instance.
      /// </returns>
      public static implicit operator Boolean(InterlockedBoolean value)
      {
         return value.GetValue();
      }

      /// <summary>
      /// Converts the given <paramref name="value"/> instance to a <see cref="Boolean"/>.
      /// </summary>
      /// <param name="value">
      /// The value to convert.
      /// </param>
      /// <returns>
      /// The <see cref="Boolean"/> representation of <paramref name="value"/>.
      /// </returns>
      public static Boolean ToBoolean(InterlockedBoolean value)
      {
         return value.GetValue();
      }

      /// <summary>
      /// Performs an implicit conversion from <see cref="Boolean"/> to <see cref="InterlockedBoolean"/>.
      /// </summary>
      /// <param name="value">
      /// The value to convert.
      /// </param>
      /// <returns>
      /// The <see cref="InterlockedBoolean"/> representation of <paramref name="value"/>.
      /// </returns>
      public static implicit operator InterlockedBoolean(Boolean value)
      {
         return new InterlockedBoolean(value);
      }

      /// <summary>
      /// Converts the given <paramref name="value"/> instance to a <see cref="Boolean"/>.
      /// </summary>
      /// <param name="value">
      /// The value to convert.
      /// </param>
      /// <returns>
      /// The <see cref="Boolean"/> representation of this instance.
      /// </returns>
      public static InterlockedBoolean ToInterlockedBoolean(Boolean value)
      {
         return new InterlockedBoolean(value);
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
      /// <c> true </c> when the instances are not equal, otherwise <c> false </c>.
      /// </returns>
      public static Boolean operator !=(Boolean left, InterlockedBoolean right)
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
      /// <c> true </c> when the instances are not equal, otherwise <c> false </c>.
      /// </returns>
      public static Boolean operator !=(InterlockedBoolean left, Boolean right)
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
      /// <c> true </c> when the instances are not equal, otherwise <c> false </c>.
      /// </returns>
      public static Boolean operator !=(InterlockedBoolean left, InterlockedBoolean right)
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
      /// <c> true </c> when the <paramref name="left"/> is less-than <paramref name="right"/> , otherwise <c> false </c>.
      /// </returns>
      public static Boolean operator <(Boolean left, InterlockedBoolean right)
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
      /// <c> true </c> when the <paramref name="left"/> is less-than <paramref name="right"/> , otherwise <c> false </c>.
      /// </returns>
      public static Boolean operator <(InterlockedBoolean left, Boolean right)
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
      /// <c> true </c> when the <paramref name="left"/> is less-than <paramref name="right"/> , otherwise <c> false </c>.
      /// </returns>
      public static Boolean operator <(InterlockedBoolean left, InterlockedBoolean right)
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
      /// <c> true </c> when the <paramref name="left"/> is Less-than-or-equal-to <paramref name="right"/> , otherwise <c> false </c>.
      /// </returns>
      public static Boolean operator <=(Boolean left, InterlockedBoolean right)
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
      /// <c> true </c> when the <paramref name="left"/> is Less-than-or-equal-to <paramref name="right"/> , otherwise <c> false </c>.
      /// </returns>
      public static Boolean operator <=(InterlockedBoolean left, Boolean right)
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
      /// <c> true </c> when the <paramref name="left"/> is Less-than-or-equal-to <paramref name="right"/> , otherwise <c> false </c>.
      /// </returns>
      public static Boolean operator <=(InterlockedBoolean left, InterlockedBoolean right)
      {
         return left.CompareTo(right) <= 0;
      }

      /// <inheritdoc/>
      public Int32 CompareTo(Boolean other)
      {
         return GetValue().CompareTo(other);
      }

      /// <inheritdoc/>
      public Int32 CompareTo(InterlockedBoolean other)
      {
         return GetValue().CompareTo(other.GetValue());
      }

      /// <inheritdoc/>
      public Boolean Equals(Boolean other)
      {
         return other.Equals(GetValue());
      }

      /// <inheritdoc/>
      public Boolean Equals(InterlockedBoolean other)
      {
         return Equals(other.GetValue());
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         if (obj is InterlockedBoolean ibValue)
         {
            return Equals(ibValue);
         }

         if (obj is Boolean boolValue)
         {
            return Equals(boolValue);
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
      public Boolean ExchangeValue(Boolean value)
      {
         var workingValue = value ? 1 : 0;
         var was = Interlocked.Exchange(ref _value, workingValue);
         return was == 1;
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         return Thread.VolatileRead(ref _value);
      }

      /// <summary>
      /// Gets the value.
      /// </summary>
      /// <returns>
      /// The current value.
      /// </returns>
      public Boolean GetValue()
      {
         return 0 != Thread.VolatileRead(ref _value);
      }

      /// <summary>
      /// Sets the value.
      /// </summary>
      /// <param name="value">
      /// The value to which the instance is set.
      /// </param>
      public void SetValue(Boolean value)
      {
         ExchangeValue(value);
      }

      /// <summary>
      /// Converts the value of this instance to its equivalent string representation (either "True" or "False").
      /// </summary>
      /// <returns>
      /// "True" if the value of this instance is <c> true </c>, else "False" when the value of this instance is <c> false </c>.
      /// </returns>
      /// <remarks>
      /// This method returns the constants "True" or "False".  Note that XML is case-sensitive, and that the XML specification recognizes
      /// "true" and "false" as the valid set of Boolean values.  If the String Object returned by the ToString() method is to be written to an XML
      /// file, its <see cref="String.ToLower()"/> method should be called first to convert it to lowercase.
      /// </remarks>
      public override String ToString()
      {
         return GetValue().ToString(CultureInfo.InvariantCulture);
      }

      /// <inheritdoc cref="Boolean"/>
      public String ToString(IFormatProvider provider)
      {
         return GetValue().ToString(provider);
      }

      /// <inheritdoc/>
      Int32 IComparable.CompareTo(Object obj)
      {
         if (ReferenceEquals(obj, null))
         {
            return 1;
         }

         if (obj is InterlockedBoolean boolValue)
         {
            // implicit conversion does not avoid an argument exception.
            return GetValue().CompareTo(boolValue.GetValue());
         }

         if (obj is Boolean)
         {
            return GetValue().CompareTo(obj);
         }

         throw new ArgumentException(@"Object must be of type Boolean or of type InterlockedBoolean.", nameof(obj));
      }
   }
}