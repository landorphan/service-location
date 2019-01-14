namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using Landorphan.Common;

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
      Boolean IsDefaultRegistration { get; }

      /// <summary>
      /// Gets a value indicating whether or not this instance is empty.
      /// </summary>
      /// <value>
      /// <c>true</c> when this instance is empty; otherwise <c>false</c>.
      /// </value>
      Boolean IsEmpty { get; }

      /// <summary>
      /// Gets the abstract type or interface registered.
      /// </summary>
      /// <value>
      /// The abstract type or interface registered.
      /// </value>
      String RegisteredName { get; }

      /// <summary>
      /// Gets the name associated with the registration.
      /// </summary>
      /// <value>
      /// The name associated with the registration.
      /// </value>
      Type RegisteredType { get; }
   }
}
