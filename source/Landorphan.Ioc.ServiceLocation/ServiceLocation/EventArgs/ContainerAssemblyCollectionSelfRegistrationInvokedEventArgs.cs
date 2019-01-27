namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Reflection;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   /// <summary>
   /// Provides data for the <see cref="IIocServiceLocatorManager.BeforeContainerAssemblyCollectionSelfRegistrationInvoked"/> event and the
   /// <see cref="IIocServiceLocatorManager.AfterContainerAssemblyCollectionSelfRegistrationInvoked"/> event.
   /// </summary>
   /// <seealso cref="EventArgs"/>
   public sealed class ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs : EventArgs
   {
      private readonly IImmutableSet<Assembly> _assemblySet;

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs"/> class.
      /// </summary>
      public ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs() : this(null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs"/> class using the specified
      /// <see cref="Container"/> and <see cref="Assemblies"/> collection.
      /// </summary>
      /// <param name="container">
      /// The container in which the assemblies registrars will be/were invoked.
      /// </param>
      /// <param name="assemblies">
      /// A collection of assemblies whose <see cref="IAssemblySelfRegistration"/> instances will be/were invoked.
      /// </param>
      public ContainerAssemblyCollectionSelfRegistrationInvokedEventArgs(IIocContainerMetaIdentity container, IEnumerable<Assembly> assemblies)
      {
         // container intentionally not validated.
         Container = container;

         // assemblies accepts null coalescing to empty
         // eliminates all null elements.
         if (assemblies == null)
         {
            _assemblySet = ImmutableHashSet<Assembly>.Empty;
         }
         else
         {
            var builder = ImmutableHashSet<Assembly>.Empty.ToBuilder();
            foreach (var asm in assemblies)
            {
               if (asm != null)
               {
                  builder.Add(asm);
               }
            }

            _assemblySet = builder.ToImmutable();
         }
      }

      /// <summary>
      /// Gets a collection of <see cref="Assembly"></see> that represents the assemblies <see cref="IAssemblySelfRegistration"/> instances were invoked to register services.
      /// </summary>
      /// <returns>
      /// A non-null collection of <see cref="Assembly"></see> of zero or more non null <see cref="Assembly"/> instances representing the collection of assemblies in which
      /// the <see cref="IAssemblySelfRegistration"/> instances were invoked to register services.
      /// </returns>
      /// <remarks>
      /// Never null, nor contains null.
      /// </remarks>
      public IEnumerable<Assembly> Assemblies => _assemblySet;

      /// <summary>
      /// Gets the <see cref="IIocContainerMetaIdentity"></see> in which the <see cref="Assemblies"/>"/> collection of <see cref="IAssemblySelfRegistration"/> instances were invoked
      /// to register services.
      /// </summary>
      /// <returns>
      /// The <see cref="IIocContainerMetaIdentity"></see> in which the <see cref="Assemblies"/>"/> collection of <see cref="IAssemblySelfRegistration"/> instances were invoked to register services.
      /// </returns>
      /// <remarks>
      /// May be a null-reference.
      /// </remarks>
      public IIocContainerMetaIdentity Container { get; }
   }
}
