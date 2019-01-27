namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Reflection;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   /// <summary>
   /// Provides data for the <see cref="IIocServiceLocatorManager.BeforeContainerAssemblySingleSelfRegistrationInvoked"/> event and the
   /// <see cref="IIocServiceLocatorManager.AfterContainerAssemblySingleSelfRegistrationInvoked"/> event.
   /// </summary>
   /// <seealso cref="EventArgs"/>
   public sealed class ContainerIndividualAssemblyRegistrarInvokedEventArgs : EventArgs
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerIndividualAssemblyRegistrarInvokedEventArgs"/> class using the specified
      /// <see cref="IAssemblySelfRegistration"/>
      /// </summary>
      public ContainerIndividualAssemblyRegistrarInvokedEventArgs() : this(null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerIndividualAssemblyRegistrarInvokedEventArgs"/> class using the specified
      /// <see cref="IAssemblySelfRegistration"/>
      /// </summary>
      /// <param name="container">
      /// The container in which the assembly registrars were invoked.
      /// </param>
      /// <param name="selfRegistration">
      /// An instance that represents the assembly registrar that was invoked.
      /// </param>
      public ContainerIndividualAssemblyRegistrarInvokedEventArgs(IIocContainerMetaIdentity container, IAssemblySelfRegistration selfRegistration)
      {
         Container = container;
         Assembly = container?.GetType().Assembly;
         SelfRegistration = selfRegistration;
      }

      /// <summary>
      /// Gets the <see cref="Assembly"/> that holds the implementation of the <see cref="IAssemblySelfRegistration"/>.
      /// </summary>
      /// <value>
      /// The <see cref="Assembly"/> that holds the implementation of the <see cref="IAssemblySelfRegistration"/>.
      /// </value>
      public Assembly Assembly { get; }

      /// <summary>
      /// Gets the inversion of control container in which <see cref="IAssemblySelfRegistration"/> will be registering/did register services.
      /// </summary>
      public IIocContainerMetaIdentity Container { get; }

      /// <summary>
      /// Gets the <see cref="IAssemblySelfRegistration"></see> that will be/was invoked to register services in the <see cref="Container"/>.
      /// </summary>
      /// <returns>
      /// An instance of <see cref="IAssemblySelfRegistration"></see> that that will be/was invoked to register services in the <see cref="Container"/>.
      /// </returns>
      public IAssemblySelfRegistration SelfRegistration { get; }
   }
}
