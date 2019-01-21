namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Represents resolving services for an <see cref="IIocContainer"/> container.
   /// </summary>
   /// <remarks>
   /// <para>
   /// Reads registrations and overrides from the container.
   /// </para>
   /// <para>
   /// <see cref="IIocContainer"/> works in concert with a <see cref="IIocContainerManager"/>, a <see cref="IIocContainerRegistrar"/>,  and a <see cref="IIocContainerResolver"/>.
   /// </para>
   /// </remarks>
   [SuppressMessage("SonarLint.CodeSmell", "S1939: Inheritance list should not be redundant", Justification = "Being explicit (MWP)")]
   public interface IIocContainerResolver : IIocContainerMetaSharedCapacities, IIocContainerRegistrationRepository
   {
      /// <summary>
      /// Resolves the default instance of the requested type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The requested type.
      /// </typeparam>
      /// <returns>
      /// The retrieved instance.
      /// </returns>
      /// <exception cref="ContainerFromTypePrecludedArgumentException">
      /// Thrown when an the type has been precluded from service location.
      /// </exception>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <typeparamref name="TFrom"/> service instance.
      /// </exception>
      TFrom Resolve<TFrom>() where TFrom : class;

      /// <summary>
      /// Resolves a named instance of the requested type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The requested type.
      /// </typeparam>
      /// <param name="name">
      /// The name of the instance to retrieve, null for the default instance.
      /// </param>
      /// <returns>
      /// The retrieved instance.
      /// </returns>
      /// <exception cref="ContainerFromTypePrecludedArgumentException">
      /// Thrown when an the type has been precluded from service location.
      /// </exception>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <typeparamref name="TFrom"/> service instance.
      /// </exception>
      TFrom Resolve<TFrom>(String name) where TFrom : class;

      /// <summary>
      /// Resolves the default instance of the requested type.
      /// </summary>
      /// <param name="fromType">
      /// The requested type.
      /// </param>
      /// <returns>
      /// The retrieved instance.
      /// </returns>
      /// <exception cref="ContainerFromTypePrecludedArgumentException">
      /// Thrown when an the type has been precluded from service location.
      /// </exception>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <paramref name="fromType"/> service instance.
      /// </exception>
      Object Resolve(Type fromType);

      /// <summary>
      /// Resolves a named instance of the requested type.
      /// </summary>
      /// <param name="fromType">
      /// The requested type.
      /// </param>
      /// <param name="name">
      /// The name of the instance to retrieve, null for the default instance.
      /// </param>
      /// <returns>
      /// The retrieved instance.
      /// </returns>
      /// <exception cref="ContainerFromTypePrecludedArgumentException">
      /// Thrown when an the type has been precluded from service location.
      /// </exception>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <paramref name="fromType"/> service instance.
      /// </exception>
      Object Resolve(Type fromType, String name);

      /// <summary>
      /// Attempts to resolve the default instance of the requested type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The requested service interface type.
      /// </typeparam>
      /// <param name="instance">
      /// The default registered instance when found; otherwise a null reference.
      /// </param>
      /// <returns>
      /// <c>true</c> when a default registered instance was found; otherwise <c>false</c>.
      /// </returns>
      Boolean TryResolve<TFrom>(out TFrom instance) where TFrom : class;

      /// <summary>
      /// Resolves a named instance of the requested type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The requested service interface type.
      /// </typeparam>
      /// <param name="name">
      /// The name of the instance to retrieve, specify null or <see cref="String.Empty"/> to retrieve the default instance.
      /// </param>
      /// <param name="instance">
      /// The named registered instance when found; otherwise a null reference.
      /// </param>
      /// <returns>
      /// <c>true</c> when the named registered instance was found; otherwise <c>false</c>.
      /// </returns>
      Boolean TryResolve<TFrom>(String name, out TFrom instance) where TFrom : class;

      /// <summary>
      /// Attempts to resolve the default instance of the requested type.
      /// </summary>
      /// <param name="fromType">
      /// The requested service interface type.
      /// </param>
      /// <param name="instance">
      /// The default registered instance when found; otherwise a null reference.
      /// </param>
      /// <returns>
      /// <c>true</c> when a default registered instance was found; otherwise <c>false</c>.
      /// </returns>
      Boolean TryResolve(Type fromType, out Object instance);

      /// <summary>
      /// Resolves a named instance of the requested type.
      /// </summary>
      /// <param name="fromType">
      /// The requested service interface type.
      /// </param>
      /// <param name="name">
      /// The name of the instance to retrieve, specify null or <see cref="String.Empty"/> to retrieve the default instance.
      /// </param>
      /// <param name="instance">
      /// The named registered instance when found; otherwise a null reference.
      /// </param>
      /// <returns>
      /// <c>true</c> when the named registered instance was found; otherwise <c>false</c>.
      /// </returns>
      Boolean TryResolve(Type fromType, String name, out Object instance);
   }
}
