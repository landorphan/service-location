namespace Landorphan.Ioc.ServiceLocation
{
   using System.Collections.Immutable;

   /// <summary>
   /// Represents the collection of registrations in a <see cref="IIocContainer"/>.
   /// </summary>
   /// <remarks>
   /// Implemented by types that consume or manipulate registrations such as implementations of <see cref="IIocContainerResolver"/> and <see cref="IIocContainerRegistrar"/>.
   /// </remarks>
   /// <seealso cref="IIocContainerMetaIdentity"/>
   public interface IIocContainerRegistrationRepository : IIocContainerMetaSharedCapacities
   {
      /// <summary>
      /// Gets the service location registrations for the container.
      /// </summary>
      /// <value>
      /// The graph of registrations in this container.
      /// </value>
      /// <remarks>
      /// Inheritance chains are not considered.  Registrations are explicitly for this container only.
      /// </remarks>
      IImmutableDictionary<IRegistrationKey, IRegistrationValue> Registrations { get; }
   }
}
