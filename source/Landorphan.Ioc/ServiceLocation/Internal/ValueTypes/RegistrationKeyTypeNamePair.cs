namespace Landorphan.Ioc.ServiceLocation.Internal
{
   // ReSharper disable ConvertToAutoProperty
   using System;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.Common;

   /// <summary>
   /// Represents a registration key.
   /// </summary>
   // Note:  cannot override default constructor.
   [DebuggerDisplay("FromType = {RegisteredType}, Name = {RegisteredName}, IsDefault = {IsDefaultRegistration}")]
   internal struct RegistrationKeyTypeNamePair :
      IRegistrationKey,
      IComparable,
      IComparable<RegistrationKeyTypeNamePair>,
      IEquatable<RegistrationKeyTypeNamePair>
   {
      /// <summary>
      /// Gets the empty instance.
      /// </summary>
      public static readonly RegistrationKeyTypeNamePair Empty = new RegistrationKeyTypeNamePair();

      private readonly Boolean _isDefaultRegistration;
      private readonly String _registeredName;
      private readonly Type _registeredType;

      /// <inheritdoc/>
      public RegistrationKeyTypeNamePair(Type registeredType) : this(registeredType, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of <see cref="RegistrationKeyTypeNamePair"/>.
      /// </summary>
      /// <param name="registeredType">
      /// An abstract type or interface.
      /// </param>
      /// <param name="registeredName">
      /// A name for associated with the registration.
      /// </param>
      public RegistrationKeyTypeNamePair(Type registeredType, String registeredName)
      {
         registeredType.ArgumentNotNull(nameof(registeredType));
         if (registeredType.ContainsGenericParameters)
         {
            throw new TypeMustNotBeAnOpenGenericArgumentException(registeredType, nameof(registeredType), null, null);
         }

         if (!(registeredType.IsAbstract || registeredType.IsInterface))
         {
            throw new FromTypeMustBeInterfaceOrAbstractTypeArgumentException(registeredType, nameof(registeredType), null, null);
         }

         _registeredType = registeredType;
         _registeredName = registeredName.TrimNullToEmpty();
         _isDefaultRegistration = _registeredName.Length == 0;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="RegistrationKeyTypeNamePair"/> struct.
      /// </summary>
      /// <param name="other">
      /// The value to clone.
      /// </param>
      public RegistrationKeyTypeNamePair(RegistrationKeyTypeNamePair other)
      {
         _registeredType = other.RegisteredType;
         _registeredName = other.RegisteredName;
         _isDefaultRegistration = other.IsDefaultRegistration;
      }

      /// <inheritdoc/>
      public Object Clone()
      {
         return new RegistrationKeyTypeNamePair(this);
      }

      /// <inheritdoc/>
      public Boolean IsDefaultRegistration => _isDefaultRegistration;

      /// <inheritdoc/>
      public String RegisteredName => _registeredName;

      /// <inheritdoc/>

      public Type RegisteredType => _registeredType;

      /// <inheritdoc/>
      public Boolean IsEmpty => RegisteredType == null && RegisteredName.TrimNullToEmpty().Length == 0;

      /// <inheritdoc/>
      public Boolean IsReadOnly => true;

      /// <inheritdoc/>
      Int32 IComparable.CompareTo(Object obj)
      {
         if (ReferenceEquals(obj, null))
         {
            // this instance is greater than null.
            return 1;
         }

         if (obj is RegistrationKeyTypeNamePair registrationTypeNamePair)
         {
            return CompareTo(registrationTypeNamePair);
         }

         throw new ArgumentException($"'{nameof(obj)}' must be of type {GetType().FullName}.", nameof(obj));
      }

      /// <inheritdoc/>
      public Int32 CompareTo(RegistrationKeyTypeNamePair other)
      {
         var thisRegisteredTypeFullName = RegisteredType == null ? String.Empty : RegisteredType.FullName;
         var otherRegisteredTypeFullName = other.RegisteredType == null ? String.Empty : other.RegisteredType.FullName;
         var rv = String.Compare(thisRegisteredTypeFullName, otherRegisteredTypeFullName, StringComparison.Ordinal);
         if (0 == rv)
         {
            var thisRegisteredName = RegisteredName ?? String.Empty;
            var otherRegisteredName = other.RegisteredName ?? String.Empty;
            rv = String.Compare(thisRegisteredName, otherRegisteredName, StringComparison.Ordinal);
         }

         return rv;
      }

      /// <inheritdoc/>
      public Int32 CompareTo(IRegistrationKey other)
      {
         if (other == null)
         {
            // this instance is greater than null.
            return 1;
         }

         var thisRegisteredTypeFullName = RegisteredType == null ? String.Empty : RegisteredType.FullName;
         var otherRegisteredTypeFullName = other.RegisteredType == null ? String.Empty : other.RegisteredType.FullName;
         var rv = String.Compare(thisRegisteredTypeFullName, otherRegisteredTypeFullName, StringComparison.Ordinal);
         if (0 == rv)
         {
            var thisRegisteredName = RegisteredName ?? String.Empty;
            var otherRegisteredName = other.RegisteredName ?? String.Empty;
            rv = String.Compare(thisRegisteredName, otherRegisteredName, StringComparison.Ordinal);
         }

         return rv;
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         return Equals(obj as IRegistrationKey);
      }

      /// <inheritdoc/>
      public Boolean Equals(RegistrationKeyTypeNamePair other)
      {
         return RegisteredType == other.RegisteredType &&
                String.Equals(RegisteredName, other.RegisteredName, StringComparison.Ordinal);
      }

      /// <inheritdoc/>
      public Boolean Equals(IRegistrationKey other)
      {
         return other != null && RegisteredType == other.RegisteredType && String.Equals(RegisteredName, other.RegisteredName, StringComparison.Ordinal);
      }

      /// <inheritdoc/>
      [SuppressMessage("Microsoft.Globalization", "CA1307: Specify StringComparison", Justification = "Not available in .Net Standard or .Net Framework")]
      public override Int32 GetHashCode()
      {
         unchecked
         {
            //Note: In .Net Standard, you cannot pass a StringComparison value into GetHashCode
            return (RegisteredName.GetHashCode() * 397) ^ RegisteredType.GetHashCode();
         }
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
      /// <c>true</c> when the instances are equal, otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator ==(RegistrationKeyTypeNamePair left, RegistrationKeyTypeNamePair right)
      {
         return left.Equals(right);
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
      /// <c>true</c> when the instances are not equal, otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator ==(RegistrationKeyTypeNamePair left, IRegistrationKey right)
      {
         return left.Equals(right);
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
      /// <c>true</c> when the instances are not equal, otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator ==(IRegistrationKey left, RegistrationKeyTypeNamePair right)
      {
         if (left == null)
         {
            // value types cannot be null
            return false;
         }

         return left.Equals(right);
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
      /// <c>true</c> when the instances are not equal, otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator !=(RegistrationKeyTypeNamePair left, RegistrationKeyTypeNamePair right)
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
      /// <c>true</c> when the instances are not equal, otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator !=(RegistrationKeyTypeNamePair left, IRegistrationKey right)
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
      /// <c>true</c> when the instances are not equal, otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator !=(IRegistrationKey left, RegistrationKeyTypeNamePair right)
      {
         if (left == null)
         {
            // value types cannot be null
            return true;
         }

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
      /// <c>true</c> when the <paramref name="left"/> is less-than <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator <(RegistrationKeyTypeNamePair left, RegistrationKeyTypeNamePair right)
      {
         return left.CompareTo(right) < 0;
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
      /// <c>true</c> when the <paramref name="left"/> is less-than <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator <(RegistrationKeyTypeNamePair left, IRegistrationKey right)
      {
         return left.CompareTo(right) < 0;
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
      /// <c>true</c> when the <paramref name="left"/> is less-than <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator <(IRegistrationKey left, RegistrationKeyTypeNamePair right)
      {
         if (left == null)
         {
            // value types cannot be null.
            return true;
         }

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
      /// <c>true</c> when the <paramref name="left"/> is Less-than-or-equal-to <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator <=(RegistrationKeyTypeNamePair left, RegistrationKeyTypeNamePair right)
      {
         return left.CompareTo(right) <= 0;
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
      /// <c>true</c> when the <paramref name="left"/> is Less-than-or-equal-to <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator <=(RegistrationKeyTypeNamePair left, IRegistrationKey right)
      {
         return left.CompareTo(right) <= 0;
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
      /// <c>true</c> when the <paramref name="left"/> is Less-than-or-equal-to <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator <=(IRegistrationKey left, RegistrationKeyTypeNamePair right)
      {
         if (left == null)
         {
            // value types cannot be null.
            return true;
         }

         return left.CompareTo(right) <= 0;
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
      /// <c>true</c> when the <paramref name="left"/> is greater-than <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator >(RegistrationKeyTypeNamePair left, RegistrationKeyTypeNamePair right)
      {
         return left.CompareTo(right) > 0;
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
      /// <c>true</c> when the <paramref name="left"/> is greater-than <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator >(RegistrationKeyTypeNamePair left, IRegistrationKey right)
      {
         return left.CompareTo(right) > 0;
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
      /// <c>true</c> when the <paramref name="left"/> is greater-than <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator >(IRegistrationKey left, RegistrationKeyTypeNamePair right)
      {
         if (left == null)
         {
            // value types cannot be null.
            return false;
         }

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
      /// <c>true</c> when the <paramref name="left"/> is Greater-than-or-equal-to <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator >=(RegistrationKeyTypeNamePair left, RegistrationKeyTypeNamePair right)
      {
         return left.CompareTo(right) >= 0;
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
      /// <c>true</c> when the <paramref name="left"/> is Greater-than-or-equal-to <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator >=(RegistrationKeyTypeNamePair left, IRegistrationKey right)
      {
         return left.CompareTo(right) >= 0;
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
      /// <c>true</c> when the <paramref name="left"/> is Greater-than-or-equal-to <paramref name="right"/> , otherwise <c>false</c>.
      /// </returns>
      public static Boolean operator >=(IRegistrationKey left, RegistrationKeyTypeNamePair right)
      {
         if (left == null)
         {
            // value types cannot be null.
            return true;
         }

         return left.CompareTo(right) >= 0;
      }
   }
}
