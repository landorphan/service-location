namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
   using System;
   using Landorphan.Common;

   /// <summary>
   /// Represents a registration key.
   /// </summary>
   /// <remarks>
   /// Registration values are initialized the a type that has a public parameterless constructor, or an instance.
   /// </remarks>
   public interface IRegistrationValue :
      ICloneable,
      IQueryReadOnly,
      IComparable,
      IComparable<IRegistrationValue>,
      IEquatable<IRegistrationValue>
   {
      /// <summary>
      /// Gets the implementation instance.
      /// </summary>
      /// <value>
      /// The implementation instance.
      /// </value>
      Object ImplementationInstance { get; }

      /// <summary>
      /// Gets the implementation type.
      /// </summary>
      /// <value>
      /// The implementation type, if any.
      /// </value>
      Type ImplementationType { get; }

      /// <summary>
      /// Gets a value indicating whether or not this instance is empty.
      /// </summary>
      /// <value>
      /// <c>true</c> when this instance is empty; otherwise <c>false</c>.
      /// </value>
      Boolean IsEmpty { get; }
   }
}
