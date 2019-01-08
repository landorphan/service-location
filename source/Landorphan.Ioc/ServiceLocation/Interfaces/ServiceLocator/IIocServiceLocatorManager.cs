namespace Landorphan.Ioc.ServiceLocation
{
   using System;

   /// <summary>
   /// Represents the management interface of the service locator.
   /// </summary>
   public interface IIocServiceLocatorManager : IIocServiceLocatorMetaSharedCapacities
   {
      /// <summary>
      /// Event queue for all listeners interested the completion of a collection of assemblies having their <see cref="IAssemblySelfRegistration"/> instances invoked.
      /// </summary>
      /// <remarks>
      /// Occurs just after the completion of a collection of assemblies <see cref="IAssemblySelfRegistration"/> instances  are invoked.
      /// </remarks>
      event EventHandler<ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs> AfterContainerAssemblyCollectionSelfRegistrationInvoked;

      /// <summary>
      /// Event queue for all listeners interested in the completion of an individual <see cref="IAssemblySelfRegistration"/> instances invocation.
      /// </summary>
      /// <remarks>
      /// Occurs just after an individual <see cref="IAssemblySelfRegistration"/> instance is invoked.
      /// </remarks>
      event EventHandler<ContainerIndividualAssemblyRegistrarInvokedEventArgs> AfterContainerAssemblySingleSelfRegistrationInvoked;

      /// <summary>
      /// Event queue for all listeners interested in changes to the ambient container.
      /// </summary>
      /// <remarks>
      /// Occurs when ambient dependency injection container changes.
      /// </remarks>
      event EventHandler<EventArgs> AmbientContainerChanged;

      /// <summary>
      /// Event queue for all listeners interested the initiation of a collection of assemblies having their <see cref="IAssemblySelfRegistration"/> instances invoked.
      /// </summary>
      /// <remarks>
      /// Occurs just before the a collection of assemblies <see cref="IAssemblySelfRegistration"/> instances are invoked.
      /// </remarks>
      event EventHandler<ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs> BeforeContainerAssemblyCollectionSelfRegistrationInvoked;

      /// <summary>
      /// Event queue for all listeners interested in the initiation of an individual <see cref="IAssemblySelfRegistration"/> instances invocation.
      /// </summary>
      /// <remarks>
      /// Occurs just before an individual <see cref="IAssemblySelfRegistration"/> instance is invoked.
      /// </remarks>
      event EventHandler<ContainerIndividualAssemblyRegistrarInvokedEventArgs> BeforeContainerAssemblySingleSelfRegistrationInvoked;

      /// <summary>
      /// Gets the ambient container.
      /// </summary>
      /// <value>
      /// The ambient container.
      /// </value>
      IIocContainer AmbientContainer { get; }

      /// <summary>
      /// Gets the root container.
      /// </summary>
      /// <value>
      /// The root container.
      /// </value>
      IIocContainer RootContainer { get; }

      /// <summary>
      /// Sets the ambient container.
      /// </summary>
      /// <param name="container">
      /// The container.
      /// </param>
      void SetAmbientContainer(IIocContainer container);
   }
}