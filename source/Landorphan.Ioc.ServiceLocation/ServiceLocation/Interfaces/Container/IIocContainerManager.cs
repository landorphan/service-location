namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
   using System;
   using System.Collections.Generic;
   using Landorphan.Ioc.ServiceLocation.EventArguments;
   using Landorphan.Ioc.ServiceLocation.Exceptions;

   /// <summary>
   /// Represents the management capabilities of a dependency injection container.
   /// </summary>
   /// <remarks>
   /// <para>
   /// <see cref="IIocContainer"/> works in concert with a <see cref="IIocContainerManager"/>, a <see cref="IIocContainerRegistrar"/>,  and a <see cref="IIocContainerResolver"/>.
   /// </para>
   /// <para>
   /// There is not implementation for removing a child container.  If you must remove a child container, call its <see cref="IDisposable.Dispose"/> method.
   /// </para>
   /// </remarks>
   public interface IIocContainerManager : IIocContainerMetaSharedCapacities
   {
      /// <summary>
      /// Occurs when a child container is added to this container.
      /// </summary>
      event EventHandler<ContainerParentChildEventArgs> ContainerChildAdded;

      /// <summary>
      /// Occurs when a child container is removed from this container.
      /// </summary>
      event EventHandler<ContainerParentChildEventArgs> ContainerChildRemoved;

      /// <summary>
      /// Occurs when the configuration of this container is changed.
      /// </summary>
      event EventHandler<ContainerConfigurationEventArgs> ContainerConfigurationChanged;

      /// <summary>
      /// Occurs when a type is precluded with this container.
      /// </summary>
      event EventHandler<ContainerTypeEventArgs> ContainerPrecludedTypeAdded;

      /// <summary>
      /// Occurs when a precluded type is removed from this container.
      /// </summary>
      event EventHandler<ContainerTypeEventArgs> ContainerPrecludedTypeRemoved;

      /// <summary>
      /// Gets the configuration settings for this container.
      /// </summary>
      /// <value>
      /// The configuration settings for this container.
      /// </value>
      IIocContainerConfiguration Configuration { get; }

      /// <summary>
      /// Gets a value indicating whether or not the configuration of this instance is locked.
      /// </summary>
      /// <value>
      /// The value indicating whether or not the configuration of this instance is locked.
      /// </value>
      Boolean IsConfigurationLocked { get; }

      /// <summary>
      /// Gets the collection of types precluded from registration by this container.
      /// </summary>
      /// <remarks>
      /// Does not include types precluded by parent containers.
      /// </remarks>
      /// <value>
      /// A collection of types precluded from registration.
      /// </value>
      IReadOnlyCollection<Type> PrecludedTypes { get; }

      /// <summary>
      /// Adds a precluded type.
      /// </summary>
      /// <typeparam name="TPrecluded">
      /// The type to be precluded from registration.
      /// </typeparam>
      /// <returns>
      /// <c> true </c> if successfully added; otherwise <c> false </c>.
      /// </returns>
      /// <remarks>
      /// Remarks precluded types are barred from service location registration.
      /// </remarks>
      /// <exception cref="FromTypeMustBeInterfaceOrAbstractTypeArgumentException">
      /// Thrown when <typeparamref name="TPrecluded"/> is neither an interface, nor an abstract class.
      /// </exception>
      /// <exception cref="CannotPrecludeRegisteredTypeArgumentException">
      /// Thrown when <typeparamref name="TPrecluded"/> is currently registered.  To resolve, remove the registration before precluding the type.
      /// </exception>
      Boolean AddPrecludedType<TPrecluded>();

      /// <summary>
      /// Adds a precluded type.
      /// </summary>
      /// <param name="precludedType">
      /// The type to be precluded from registration.
      /// </param>
      /// <returns>
      /// <c> true </c> if successfully added; otherwise <c> false </c> (already precluded).
      /// </returns>
      /// <remarks>
      /// Remarks precluded types are barred from service location registration.
      /// </remarks>
      /// <exception cref="FromTypeMustBeInterfaceOrAbstractTypeArgumentException">
      /// Thrown when <paramref name="precludedType"/> is neither an interface, nor an abstract class.
      /// </exception>
      /// <exception cref="CannotPrecludeRegisteredTypeArgumentException">
      /// Thrown when <paramref name="precludedType"/> is already registered.  Remove the registration before precluding the type.
      /// </exception>
      Boolean AddPrecludedType(Type precludedType);

      /// <summary>
      /// Adds a new child container with the given name.
      /// </summary>
      /// <param name="name">
      /// The name of the child container.  Name values are trimmed of leading and trailing whitespace, null values are converted to <see cref="String.Empty"/>.
      /// </param>
      /// <returns>
      /// A <see cref="IIocContainer"/> referencing the child container (can be null).
      /// </returns>
      /// <remarks>
      /// This instance becomes the owner of the newly created child instance.
      /// </remarks>
      IIocContainer CreateChildContainer(String name);

      /// <summary>
      /// Locks the configuration of this instance.
      /// </summary>
      /// <returns>
      /// The previous lock state.
      /// </returns>
      /// <remarks>
      /// <para>
      /// Locking an unlocked configuration makes any further attempts to change the configuration of this
      /// <see cref="IIocContainer" /> throw;  Locking a locked configuration has no effect.
      /// </para>
      /// <para>
      /// Once locked, the container configuration cannot be unlocked.
      /// </para>
      /// </remarks>
      Boolean LockConfiguration();

      /// <summary>
      /// Removes a precluded type.
      /// </summary>
      /// <typeparam name="TPrecluded">
      /// The type to be removed from preclusion.
      /// </typeparam>
      /// <returns>
      /// <c> true </c> if successfully removed; otherwise <c> false </c>.
      /// </returns>
      /// <remarks>
      /// The primary intent of this method is to enable the removal of a precluded type after a test or battery of tests.
      /// </remarks>
      Boolean RemovePrecludedType<TPrecluded>();

      /// <summary>
      /// Removes a precluded type.
      /// </summary>
      /// <param name="precludedType">
      /// The type to be removed from preclusion.
      /// </param>
      /// <returns>
      /// <c> true </c> if successfully removed; otherwise <c> false </c> (not precluded).
      /// </returns>
      /// <exception cref="CannotPrecludeRegisteredTypeArgumentException">
      /// Thrown when <paramref name="precludedType"/> is already registered.  Remove the registration before precluding the type.
      /// </exception>
      Boolean RemovePrecludedType(Type precludedType);
   }
}
