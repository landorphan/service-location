namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
    using System;

    /// <summary>
   /// Represents the meta identity of a <see cref="IIocContainer"/> in concert with
   /// a <see cref="IIocContainerManager"/>,
   /// a <see cref="IIocContainerRegistrar"/>,
   /// and a <see cref="IIocContainerResolver"/>.
   /// </summary>
   /// <remarks>
   /// <see cref="IIocContainerMetaIdentity.Uid"/> is authoritative; <see cref="IIocContainerMetaIdentity.Name"/> is suggestive and used only for human consumption.
   /// </remarks>
   public interface IIocContainerMetaIdentity
   {
       /// <summary>
      /// Gets the name of this container.
      /// </summary>
      /// <value>
      /// The name of this container.
      /// </value>
      /// <remarks>
      /// This is intended for convenience and application-specific practices.  It is not required to be unique, but probably should be in practice.  Leading and trailing white space is removed
      /// </remarks>
      string Name { get; }

       /// <summary>
      /// Gets the unique identifier of this container instance.
      /// </summary>
      /// <value>
      /// The unique identifier of this container instance.
      /// </value>
      Guid Uid { get; }
   }
}
