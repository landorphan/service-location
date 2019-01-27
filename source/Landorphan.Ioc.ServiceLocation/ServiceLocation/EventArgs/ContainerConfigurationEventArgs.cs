namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   /// <summary>
   /// Provides data for the <see cref="IIocContainerManager.ContainerConfigurationChanged"/> event.
   /// </summary>
   public class ContainerConfigurationEventArgs : EventArgs
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationEventArgs"/> class.
      /// </summary>
      public ContainerConfigurationEventArgs() : this(null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationEventArgs"/> class.
      /// </summary>
      /// <param name="configuration">
      /// The inversion of control container configuration.
      /// </param>
      public ContainerConfigurationEventArgs(IIocContainerConfiguration configuration)
      {
         Configuration = configuration;
      }

      /// <summary>
      /// Gets the inversion of control container configuration.
      /// </summary>
      public IIocContainerConfiguration Configuration { get; }
   }
}
