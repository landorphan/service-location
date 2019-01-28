namespace Landorphan.Ioc.ServiceLocation.Internal
{
   // ReSharper disable ConvertToAutoProperty
   using System;
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation.Exceptions;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   /// <summary>
   /// Represents a registration value.
   /// </summary>
   /// <seealso cref="IRegistrationValue"/>
   /// <seealso cref="IComparable"/>
   /// <seealso cref="IComparable{T}"/>
   /// <seealso cref="IEquatable{RegistrationValueTypeInstancePair}"/>
   // Note:  cannot override default constructor.
   internal struct RegistrationValueTypeInstancePair :
      IRegistrationValue,
      IComparable,
      IComparable<RegistrationValueTypeInstancePair>,
      IEquatable<RegistrationValueTypeInstancePair>
   {
      /// <summary>
      /// Gets the empty instance.
      /// </summary>
      public static readonly RegistrationValueTypeInstancePair Empty = new RegistrationValueTypeInstancePair();

      private readonly Object _implementationInstance;
      private readonly Type _implementationType;

      /// <summary>
      /// Initializes a new instance of the <see cref="RegistrationValueTypeInstancePair"/> struct.
      /// </summary>
      /// <param name="implementationType">
      /// The implementation type.
      /// </param>
      public RegistrationValueTypeInstancePair(Type implementationType)
      {
         implementationType.ArgumentNotNull(nameof(implementationType));

         if (implementationType.IsInterface || implementationType.IsAbstract)
         {
            throw new ToTypeMustNotBeInterfaceNorAbstractArgumentException(implementationType, nameof(implementationType));
         }

         _implementationInstance = null;
         _implementationType = implementationType;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="RegistrationValueTypeInstancePair"/> struct.
      /// </summary>
      /// <param name="implementationInstance">
      /// The implementation instance.
      /// </param>
      public RegistrationValueTypeInstancePair(Object implementationInstance)
      {
         implementationInstance.ArgumentNotNull(nameof(implementationInstance));

         _implementationInstance = implementationInstance;
         _implementationType = null;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="RegistrationValueTypeInstancePair"/> struct.
      /// </summary>
      /// <param name="implementationType">
      /// Type of the implementation.
      /// </param>
      /// <param name="implementationInstance">
      /// The implementation instance.
      /// </param>
      [SuppressMessage("SonarLint.CodeSmell", "S2219: Runtime type checking should be simplified")]
      public RegistrationValueTypeInstancePair(Type implementationType, Object implementationInstance)
      {
         implementationType.ArgumentNotNull(nameof(implementationType));
         implementationInstance.ArgumentNotNull(nameof(implementationInstance));

         if (implementationType.IsInterface || implementationType.IsAbstract)
         {
            throw new ToTypeMustNotBeInterfaceNorAbstractArgumentException(implementationType, nameof(implementationType));
         }

         if (!implementationType.IsInstanceOfType(implementationInstance))
         {
            throw new InstanceMustImplementTypeArgumentException(implementationType, implementationInstance, nameof(implementationInstance));
         }

         _implementationType = implementationType;
         _implementationInstance = implementationInstance;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="RegistrationKeyTypeNamePair"/> struct.
      /// </summary>
      /// <param name="other">
      /// The value to clone.
      /// </param>
      public RegistrationValueTypeInstancePair(RegistrationValueTypeInstancePair other)
      {
         _implementationType = other.ImplementationType;
         _implementationInstance = other.ImplementationInstance;
      }

      /// <inheritdoc/>
      public Object Clone()
      {
         return new RegistrationValueTypeInstancePair(this);
      }

      /// <inheritdoc/>
      public Boolean IsReadOnly => true;

      /// <inheritdoc/>
      public Object ImplementationInstance => _implementationInstance;

      /// <inheritdoc/>
      public Boolean IsEmpty => ImplementationType == null && ImplementationInstance == null;

      /// <inheritdoc/>
      public Type ImplementationType => _implementationType;

      /// <inheritdoc/>
      Int32 IComparable.CompareTo(Object obj)
      {
         if (ReferenceEquals(obj, null))
         {
            // this instance is greater than null.
            return 1;
         }

         if (obj is RegistrationValueTypeInstancePair registrationTypeInstancePair)
         {
            return CompareTo(registrationTypeInstancePair);
         }

         throw new ArgumentException($"'{nameof(obj)}' must be of type {GetType().FullName}.", nameof(obj));
      }

      /// <inheritdoc/>
      public Int32 CompareTo(RegistrationValueTypeInstancePair other)
      {
         var thisTypeFullName = ImplementationType == null ? String.Empty : ImplementationType.FullName;
         var otherTypeFullName = other.ImplementationType == null ? String.Empty : other.ImplementationType.FullName;
         var rv = String.Compare(thisTypeFullName, otherTypeFullName, StringComparison.Ordinal);
         if (rv == 0)
         {
            var thisInstanceHash = ImplementationInstance?.GetHashCode() ?? 0;
            var otherInstanceHash = other.ImplementationInstance?.GetHashCode() ?? 0;
            rv = thisInstanceHash.CompareTo(otherInstanceHash);
         }

         return rv;
      }

      /// <inheritdoc/>
      public Int32 CompareTo(IRegistrationValue other)
      {
         if (other == null)
         {
            // this instance is greater than null.
            return 1;
         }

         var thisTypeFullName = ImplementationType == null ? String.Empty : ImplementationType.FullName;
         var otherTypeFullName = other.ImplementationType == null ? String.Empty : other.ImplementationType.FullName;
         var rv = String.Compare(thisTypeFullName, otherTypeFullName, StringComparison.Ordinal);
         if (rv == 0)
         {
            var thisInstanceHash = ImplementationInstance?.GetHashCode() ?? 0;
            var otherInstanceHash = other.ImplementationInstance?.GetHashCode() ?? 0;
            rv = thisInstanceHash.CompareTo(otherInstanceHash);
         }

         return rv;
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         return Equals(obj as IRegistrationValue);
      }

      /// <inheritdoc/>
      public Boolean Equals(RegistrationValueTypeInstancePair other)
      {
         return ImplementationType == other.ImplementationType && ImplementationInstance == other.ImplementationInstance;
      }

      /// <inheritdoc/>
      public Boolean Equals(IRegistrationValue other)
      {
         if (other == null)
         {
            return false;
         }

         return ImplementationType == other.ImplementationType && ImplementationInstance == other.ImplementationInstance;
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            return ((ImplementationType != null ? ImplementationType.GetHashCode() : 0) * 397) ^ (ImplementationInstance != null ? ImplementationInstance.GetHashCode() : 0);
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
      public static Boolean operator ==(RegistrationValueTypeInstancePair left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator ==(RegistrationValueTypeInstancePair left, IRegistrationValue right)
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
      public static Boolean operator ==(IRegistrationValue left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator !=(RegistrationValueTypeInstancePair left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator !=(RegistrationValueTypeInstancePair left, IRegistrationValue right)
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
      public static Boolean operator !=(IRegistrationValue left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator <(RegistrationValueTypeInstancePair left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator <(RegistrationValueTypeInstancePair left, IRegistrationValue right)
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
      public static Boolean operator <(IRegistrationValue left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator <=(RegistrationValueTypeInstancePair left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator <=(RegistrationValueTypeInstancePair left, IRegistrationValue right)
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
      public static Boolean operator <=(IRegistrationValue left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator >(RegistrationValueTypeInstancePair left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator >(RegistrationValueTypeInstancePair left, IRegistrationValue right)
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
      public static Boolean operator >(IRegistrationValue left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator >=(RegistrationValueTypeInstancePair left, RegistrationValueTypeInstancePair right)
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
      public static Boolean operator >=(RegistrationValueTypeInstancePair left, IRegistrationValue right)
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
      public static Boolean operator >=(IRegistrationValue left, RegistrationValueTypeInstancePair right)
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
