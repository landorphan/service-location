namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   /// <summary>
   /// Provides data for the <see cref="IIocContainerRegistrar.ContainerRegistrationAdded"/> event and <see cref="IIocContainerRegistrar.ContainerRegistrationRemoved"/> event.
   /// </summary>
   /// <seealso cref="EventArgs"/>
   public class ContainerTypeRegistrationEventArgs : EventArgs
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerTypeRegistrationEventArgs"/> class.
      /// </summary>
      public ContainerTypeRegistrationEventArgs() : this(null, null, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerTypeRegistrationEventArgs"/> class.
      /// </summary>
      /// <param name="container">
      /// The inversion of control container in which the type was registered.
      /// </param>
      /// <param name="fromType">
      /// The registered type.
      /// </param>
      /// <param name="name">
      /// The registered name.
      /// </param>
      /// <param name="toType">
      /// The implementation type.
      /// </param>
      /// <param name="instance">
      /// The resolved instance, or a null reference if it has yet to be resolved.
      /// </param>
      public ContainerTypeRegistrationEventArgs(IIocContainerMetaIdentity container, Type fromType, String name, Type toType, Object instance)
      {
         Container = container;
         FromType = fromType;
         Name = name.TrimNullToEmpty();
         ToType = toType;
         Instance = instance;
      }

      /// <summary>
      /// Gets the inversion of control container in which type was registered.
      /// </summary>
      public IIocContainerMetaIdentity Container { get; }

      /// <summary>
      /// Gets the type registered.
      /// </summary>
      public Type FromType { get; }

      /// <summary>
      /// Gets the instance registered.
      /// </summary>
      /// <remarks>
      /// May be a null reference.
      /// </remarks>
      public Object Instance { get; }

      /// <summary>
      /// Gets the registered name.
      /// </summary>
      /// <remarks>
      /// May be a null reference.
      /// </remarks>
      public String Name { get; }

      /// <summary>
      /// Gets the registered implementation type.
      /// </summary>
      /// <remarks>
      /// May be a null reference.
      /// </remarks>
      public Type ToType { get; }
   }
}
