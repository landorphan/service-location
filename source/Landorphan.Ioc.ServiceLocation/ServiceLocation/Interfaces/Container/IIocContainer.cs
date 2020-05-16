namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
   /// Represents a dependency injection container.
   /// </summary>
   /// <remarks>
   /// <see cref="IIocContainer"/> works in concert with a <see cref="IIocContainerManager"/>, a <see cref="IIocContainerRegistrar"/>,  and a <see cref="IIocContainerResolver"/>.
   /// </remarks>
   public interface IIocContainer : IIocContainerMetaSharedCapacities
   {
       /// <summary>
      /// Gets all of the immediate children of this container.
      /// </summary>
      /// <value>
      /// The immediate children of this container.
      /// </value>
      /// <remarks>
      /// Children are owned, the reference to the parent is not.  When a container is disposed; all of its children are also disposed.  The collection of children does not expose the "ownership" in
      /// its interface, but does enforce it in its implementation.
      /// </remarks>
      IReadOnlyCollection<IIocContainer> Children { get; }

       /// <summary>
      /// Gets a value indicating whether this instance is a root container.
      /// </summary>
      /// <value>
      /// <c> true </c> if this instance is a root container; otherwise <c> false </c>.
      /// </value>
      bool IsRoot { get; }

       /// <summary>
      /// Gets the parent of this container.
      /// </summary>
      /// <value>
      /// The parent container if this container is a child container; otherwise <c> null </c>.
      /// </value>
      IIocContainer Parent { get; }
   }
}
