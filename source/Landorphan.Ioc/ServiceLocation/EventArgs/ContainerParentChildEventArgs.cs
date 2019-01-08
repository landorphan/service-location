namespace Landorphan.Ioc.ServiceLocation
{
   using System;

   /// <summary>
   /// Provides data for events that provide container and type information.
   /// </summary>
   /// <seealso cref="EventArgs"/>
   // ReSharper disable once InheritdocConsiderUsage
   public class ContainerParentChildEventArgs : EventArgs
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerTypeEventArgs"/> class.
      /// </summary>
      public ContainerParentChildEventArgs() : this(null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerTypeEventArgs"/> class.
      /// </summary>
      /// <param name="parent">
      /// The parent container.
      /// </param>
      /// <param name="child">
      /// The child container.
      /// </param>
      public ContainerParentChildEventArgs(IIocContainerMetaIdentity parent, IIocContainerMetaIdentity child)
      {
         Parent = parent;
         Child = child;
      }

      /// <summary>
      /// Gets the child container.
      /// </summary>
      public IIocContainerMetaIdentity Child { get; }

      /// <summary>
      /// Gets the parent container.
      /// </summary>
      public IIocContainerMetaIdentity Parent { get; }
   }
}