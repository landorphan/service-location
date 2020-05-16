namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
    using System;
    using Landorphan.Common.Interfaces;

    /// <summary>
   /// Represents a specified container and <see cref="IRegistrationKey"/>.
   /// </summary>
   public interface IContainerRegistrationKey :
      ICloneable,
      IQueryReadOnly,
      IComparable,
      IComparable<IContainerRegistrationKey>,
      IEquatable<IContainerRegistrationKey>
   {
       /// <summary>
      /// Gets the container.
      /// </summary>
      IIocContainerMetaIdentity Container { get; }

       /// <summary>
      /// Gets a value indicating whether or not this registration represents the default registration for its container.
      /// </summary>
      bool IsDefaultRegistration { get; }

       /// <summary>
      /// Gets a value indicating whether or not this instance is empty.
      /// </summary>
      /// <value>
      /// <c>true</c> when this instance is empty; otherwise <c>false</c>.
      /// </value>
      bool IsEmpty { get; }

       /// <summary>
      /// Gets the abstract type or interface registered.
      /// </summary>
      /// <value>
      /// The abstract type or interface registered.
      /// </value>
      string RegisteredName { get; }

       /// <summary>
      /// Gets the name associated with the registration.
      /// </summary>
      /// <value>
      /// The name associated with the registration.
      /// </value>
      Type RegisteredType { get; }
   }
}
