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
   [DebuggerDisplay("Container = ({_container.Uid}: {_container.Name}),\r\nFromType = {RegisteredType},\r\nName = {RegisteredName}, IsDefault = {IsDefaultRegistration}")]
   internal struct ContainerRegistrationKeyTypeNameTrio :
      IContainerRegistrationKey,
      IComparable,
      IComparable<ContainerRegistrationKeyTypeNameTrio>,
      IEquatable<ContainerRegistrationKeyTypeNameTrio>
   {
      /// <summary>
      /// Gets the empty instance.
      /// </summary>
      public static readonly ContainerRegistrationKeyTypeNameTrio Empty = new ContainerRegistrationKeyTypeNameTrio();

      private readonly RegistrationKeyTypeNamePair _wrappedPair;
      private readonly IIocContainerMetaIdentity _container;

      /// <inheritdoc/>
      public ContainerRegistrationKeyTypeNameTrio(IIocContainerMetaIdentity container, Type registeredType) : this(container, registeredType, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of <see cref="ContainerRegistrationKeyTypeNameTrio"/>.
      /// </summary>
      /// <param name="container">
      /// The container that owns the registration.
      /// </param>
      /// <param name="registeredType">
      /// An abstract type or interface.
      /// </param>
      /// <param name="registeredName">
      /// A name for associated with the registration.
      /// </param>
      public ContainerRegistrationKeyTypeNameTrio(IIocContainerMetaIdentity container, Type registeredType, String registeredName)
      {
         container.ArgumentNotNull(nameof(container));
         registeredType.ArgumentNotNull(nameof(registeredType));

         if (!(registeredType.IsAbstract || registeredType.IsInterface))
         {
            throw new FromTypeMustBeInterfaceOrAbstractTypeArgumentException(registeredType, nameof(registeredType));
         }

         _container = container;
         _wrappedPair = new RegistrationKeyTypeNamePair(registeredType, registeredName);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerRegistrationKeyTypeNameTrio"/> struct.
      /// </summary>
      /// <param name="other">
      /// The value to clone.
      /// </param>
      public ContainerRegistrationKeyTypeNameTrio(ContainerRegistrationKeyTypeNameTrio other)
      {
         _container = other.Container;
         _wrappedPair = new RegistrationKeyTypeNamePair(other.RegisteredType, other.RegisteredName);
      }

      /// <inheritdoc/>
      public Object Clone()
      {
         return new ContainerRegistrationKeyTypeNameTrio(this);
      }

      /// <inheritdoc/>
      public Boolean IsDefaultRegistration => _wrappedPair.IsDefaultRegistration;

      /// <inheritdoc/>
      public String RegisteredName => _wrappedPair.RegisteredName;

      /// <inheritdoc/>
      public Type RegisteredType => _wrappedPair.RegisteredType;

      /// <inheritdoc/>
      public IIocContainerMetaIdentity Container => _container;

      /// <inheritdoc/>
      public Boolean IsEmpty => _wrappedPair.IsEmpty && _container == null;

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

         if (obj is ContainerRegistrationKeyTypeNameTrio containerRegistrationKeyTypeNameTrio)
         {
            return CompareTo(containerRegistrationKeyTypeNameTrio);
         }

         throw new ArgumentException($"'{nameof(obj)}' must be of type {GetType().FullName}.", nameof(obj));
      }

      /// <inheritdoc/>
      public Int32 CompareTo(ContainerRegistrationKeyTypeNameTrio other)
      {
         var thisContainerFullName = Container == null ? String.Empty : Container.GetType().FullName;
         var otherContainerFullName = other.Container == null ? String.Empty : other.Container.GetType().FullName;
         var rv = String.Compare(thisContainerFullName, otherContainerFullName, StringComparison.Ordinal);
         {
            if (0 == rv)
            {
               var thisRegisteredTypeFullName = RegisteredType == null ? String.Empty : RegisteredType.FullName;
               var otherRegisteredTypeFullName = other.RegisteredType == null ? String.Empty : other.RegisteredType.FullName;
               rv = String.Compare(thisRegisteredTypeFullName, otherRegisteredTypeFullName, StringComparison.Ordinal);
               if (0 == rv)
               {
                  var thisRegisteredName = RegisteredName ?? String.Empty;
                  var otherRegisteredName = other.RegisteredName ?? String.Empty;
                  rv = String.Compare(thisRegisteredName, otherRegisteredName, StringComparison.Ordinal);
               }
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      public Int32 CompareTo(IContainerRegistrationKey other)
      {
         if (other == null)
         {
            // this instance is greater than null.
            return 1;
         }

         var thisContainerFullName = Container == null ? String.Empty : Container.GetType().FullName;
         var otherContainerFullName = other.Container == null ? String.Empty : other.Container.GetType().FullName;
         var rv = String.Compare(thisContainerFullName, otherContainerFullName, StringComparison.Ordinal);
         {
            if (0 == rv)
            {
               var thisRegisteredTypeFullName = RegisteredType == null ? String.Empty : RegisteredType.FullName;
               var otherRegisteredTypeFullName = other.RegisteredType == null ? String.Empty : other.RegisteredType.FullName;
               rv = String.Compare(thisRegisteredTypeFullName, otherRegisteredTypeFullName, StringComparison.Ordinal);
               if (0 == rv)
               {
                  var thisRegisteredName = RegisteredName ?? String.Empty;
                  var otherRegisteredName = other.RegisteredName ?? String.Empty;
                  rv = String.Compare(thisRegisteredName, otherRegisteredName, StringComparison.Ordinal);
               }
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         return Equals(obj as IContainerRegistrationKey);
      }

      /// <inheritdoc/>
      public Boolean Equals(ContainerRegistrationKeyTypeNameTrio other)
      {
         return RegisteredType == other.RegisteredType &&
                String.Equals(RegisteredName, other.RegisteredName, StringComparison.Ordinal);
      }

      /// <inheritdoc/>
      public Boolean Equals(IContainerRegistrationKey other)
      {
         return other != null && RegisteredType == other.RegisteredType && String.Equals(RegisteredName, other.RegisteredName, StringComparison.Ordinal);
      }

      /// <inheritdoc/>
      [SuppressMessage("Microsoft.Globalization", "CA1307: Specify StringComparison", Justification = "Not available in .Net Standard or .Net Framework")]
      public override Int32 GetHashCode()
      {
         unchecked
         {
            // In .Net Standard 2.2 GetHashCode does not have an overload that takes a StringComparison value.
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
      public static Boolean operator ==(ContainerRegistrationKeyTypeNameTrio left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator ==(ContainerRegistrationKeyTypeNameTrio left, IContainerRegistrationKey right)
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
      public static Boolean operator ==(IContainerRegistrationKey left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator !=(ContainerRegistrationKeyTypeNameTrio left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator !=(ContainerRegistrationKeyTypeNameTrio left, IContainerRegistrationKey right)
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
      public static Boolean operator !=(IContainerRegistrationKey left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator <(ContainerRegistrationKeyTypeNameTrio left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator <(ContainerRegistrationKeyTypeNameTrio left, IContainerRegistrationKey right)
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
      public static Boolean operator <(IContainerRegistrationKey left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator <=(ContainerRegistrationKeyTypeNameTrio left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator <=(ContainerRegistrationKeyTypeNameTrio left, IContainerRegistrationKey right)
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
      public static Boolean operator <=(IContainerRegistrationKey left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator >(ContainerRegistrationKeyTypeNameTrio left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator >(ContainerRegistrationKeyTypeNameTrio left, IContainerRegistrationKey right)
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
      public static Boolean operator >(IContainerRegistrationKey left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator >=(ContainerRegistrationKeyTypeNameTrio left, ContainerRegistrationKeyTypeNameTrio right)
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
      public static Boolean operator >=(ContainerRegistrationKeyTypeNameTrio left, IContainerRegistrationKey right)
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
      public static Boolean operator >=(IContainerRegistrationKey left, ContainerRegistrationKeyTypeNameTrio right)
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
