namespace Landorphan.Ioc.ServiceLocation.EventArguments
{
    using System;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    /// <summary>
   /// Provides data for events that provide container and type information.
   /// </summary>
   /// <seealso cref="EventArgs"/>
   // ReSharper disable once InheritdocConsiderUsage
   public class ContainerTypeEventArgs : EventArgs
   {
       /// <summary>
      /// Initializes a new instance of the <see cref="ContainerTypeEventArgs"/> class.
      /// </summary>
      public ContainerTypeEventArgs() : this(null, null)
      {
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="ContainerTypeEventArgs"/> class.
      /// </summary>
      /// <param name="container">
      /// The inversion of control container in which the type was registered.
      /// </param>
      /// <param name="type">
      /// The registered type.
      /// </param>
      public ContainerTypeEventArgs(IIocContainerMetaIdentity container, Type type)
      {
         Container = container;
         Type = type;
      }

       /// <summary>
      /// Gets the inversion of control container in which type was registered.
      /// </summary>
      public IIocContainerMetaIdentity Container { get; }

       /// <summary>
      /// Gets the type registered.
      /// </summary>
      public Type Type { get; }
   }
}
