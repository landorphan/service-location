namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
    using System;
    using Landorphan.Ioc.ServiceLocation.Exceptions;

    /// <summary>
   /// Represents the client interface for service location.
   /// </summary>
   /// <seealso cref="IServiceProvider"/>
   public interface IIocServiceLocator : IServiceProvider, IIocServiceLocatorMetaSharedCapacities
   {
       /// <summary>
      /// Resolves the default instance of the requested type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The requested service interface type
      /// </typeparam>
      /// <returns>
      /// The default implementation instance.
      /// </returns>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <typeparamref name="TFrom"/> service default instance.
      /// </exception>
      TFrom Resolve<TFrom>() where TFrom : class;

       /// <summary>
      /// Resolves a named instance of the requested type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The requested service interface type.
      /// </typeparam>
      /// <param name="name">
      /// The name of the instance to retrieve, null or <see cref="string.Empty"/> for the default instance.
      /// </param>
      /// <returns>
      /// The retrieved instance.
      /// </returns>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <typeparamref name="TFrom"/> service instance.
      /// </exception>
      TFrom Resolve<TFrom>(string name) where TFrom : class;

       /// <summary>
      /// Resolves the default instance of the requested type.
      /// </summary>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <paramref name="fromType"/> service instance.
      /// </exception>
      /// <param name="fromType">
      /// The requested type.
      /// </param>
      /// <returns>
      /// The retrieved instance.
      /// </returns>
      object Resolve(Type fromType);

       /// <summary>
      /// Resolves a named instance of the requested type.
      /// </summary>
      /// <param name="fromType">
      /// The requested service interface type.
      /// </param>
      /// <param name="name">
      /// The name of the instance to retrieve, specify null or <see cref="String.Empty"/> to retrieve the default instance.
      /// </param>
      /// <returns>
      /// The retrieved instance.
      /// </returns>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the named <paramref name="fromType"/> service instance.
      /// </exception>
      object Resolve(Type fromType, string name);

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
      bool TryResolve<TFrom>(out TFrom instance) where TFrom : class;

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
      bool TryResolve<TFrom>(string name, out TFrom instance) where TFrom : class;

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
      bool TryResolve(Type fromType, out object instance);

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
      bool TryResolve(Type fromType, string name, out object instance);
   }
}
