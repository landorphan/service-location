namespace Landorphan.Ioc.ServiceLocation
{
   /// <summary>
   /// Represents the meta capacities shared by
   /// <see cref="IIocContainer"/> in concert with
   /// a <see cref="IIocContainerManager"/>,
   /// a <see cref="IIocContainerRegistrar"/>,
   /// and a <see cref="IIocContainerResolver"/>.
   /// </summary>
   /// <remarks>
   /// These interfaces work in tandem in 1:1 relationships but with separate responsibilities:
   /// <see cref="IIocContainer"/> represents the existence, relationship to other containers, and the identity of a registration "database",
   /// whereas <see cref="IIocContainerRegistrar"/> represents the capacity to add, remove, and query registrations,
   /// and <see cref="IIocContainerManager"/> represents the additional management capacities,
   /// and <see cref="IIocContainerResolver"/> is the client-side interface for finding implementation instances for queried service location interfaces.
   /// </remarks>
   public interface IIocContainerMetaSharedCapacities : IIocContainerMetaIdentity
   {
      /// <summary>
      /// Gets the container.
      /// </summary>
      /// <value>
      /// The container, a simple 1:1 navigation reference.
      /// </value>
      IIocContainer Container { get; }

      /// <summary>
      /// Gets the container manager.
      /// </summary>
      /// <value>
      /// The container manager, a simple 1:1 navigation reference.
      /// </value>
      IIocContainerManager Manager { get; }

      /// <summary>
      /// Gets the container registrar.
      /// </summary>
      /// <value>
      /// The container registrar, a simple 1:1 navigation reference.
      /// </value>
      IIocContainerRegistrar Registrar { get; }

      /// <summary>
      /// Gets the container resolver.
      /// </summary>
      /// <value>
      /// The container resolver, a simple 1:1 navigation reference.
      /// </value>
      IIocContainerResolver Resolver { get; }
   }
}
