namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Collections.Immutable;
   using System.Linq;
   using Landorphan.Common;
   using Landorphan.Ioc.Logging.Internal;
   using Landorphan.Ioc.ServiceLocation.Internal;

   public sealed partial class IocServiceLocator : IIocServiceLocator, IIocServiceLocatorManager
   {
      private static readonly IocServiceLocator t_singletonInstance;

      // the static ctor is in *.Shared.cs

      /// <summary>
      /// Gets the ambient container.
      /// </summary>
      /// <value>
      /// The ambient container.
      /// </value>
      public static IIocContainer AmbientContainer => t_singletonInstance._ambientContainer;

      /// <summary>
      /// Gets the singleton instance.
      /// </summary>
      public static IIocServiceLocator Instance => t_singletonInstance;

      /// <summary>
      /// Gets the root container.
      /// </summary>
      /// <value>
      /// The root container.
      /// </value>
      // ReSharper disable once ConvertToAutoProperty
      public static IIocContainer RootContainer => IocContainer.RootContainer;

      /// <summary>
      /// Gets the singleton instance the the class type, instead of the interface type.
      /// </summary>
      /// <remarks>
      /// This is used by <see cref="IocContainer"/> to address the bootstrapping issue where resolve can be called by clients before initialization of <see cref="IocServiceLocator"/>
      /// and <see cref="IocContainer"/> is completed.  Yes, that goes against the claims of the CLR documentation.
      /// </remarks>
      internal static IocServiceLocator InternalInstance => t_singletonInstance;

      /// <summary>
      /// Resolves the default instance of the requested type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The requested type.
      /// </typeparam>
      /// <returns>
      /// The retrieved instance.
      /// </returns>
      /// <remarks>
      /// Enables syntax simplification:
      /// <para>
      /// <code>IocServiceLocator.Instance.Resolve</code> can be written as <code>IocServiceLocator.Resolve</code>
      /// </para>
      /// </remarks>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <typerefparam name="TFrom" /> service instance.
      /// </exception>
      public static TFrom Resolve<TFrom>() where TFrom : class
      {
         return ((IIocServiceLocator)t_singletonInstance).Resolve<TFrom>();
      }

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
      /// <remarks>
      /// Enables syntax simplification:
      /// <para>
      /// <code>IocServiceLocator.Instance.Resolve</code> can be written as <code>IocServiceLocator.Resolve</code>
      /// </para>
      /// </remarks>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <typerefparam name="TFrom" /> service instance.
      /// </exception>
      public static TFrom Resolve<TFrom>(String name) where TFrom : class
      {
         return ((IIocServiceLocator)t_singletonInstance).Resolve<TFrom>(name);
      }

      /// <summary>
      /// Resolves the default instance of the requested type.
      /// </summary>
      /// <param name="fromType">
      /// The requested type.
      /// </param>
      /// <returns>
      /// The retrieved instance.
      /// </returns>
      /// <remarks>
      /// Enables syntax simplification:
      /// <para>
      /// <code>IocServiceLocator.Instance.Resolve</code> can be written as <code>IocServiceLocator.Resolve</code>
      /// </para>
      /// </remarks>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <paramref name="fromType" /> service instance.
      /// </exception>
      public static Object Resolve(Type fromType)
      {
         return ((IIocServiceLocator)t_singletonInstance).Resolve(fromType);
      }

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
      /// <remarks>
      /// Enables syntax simplification:
      /// <para>
      /// <code>IocServiceLocator.Instance.Resolve</code> can be written as <code>IocServiceLocator.Resolve</code>
      /// </para>
      /// </remarks>
      /// <exception cref="ResolutionException">
      /// Thrown when an error occurs resolving the <paramref name="fromType" /> service instance.
      /// </exception>
      public static Object Resolve(Type fromType, String name)
      {
         return ((IIocServiceLocator)t_singletonInstance).Resolve(fromType, name);
      }

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
      public static Boolean TryResolve<TFrom>(out TFrom instance) where TFrom : class
      {
         var rv = ((IIocServiceLocator)t_singletonInstance).TryResolve<TFrom>(out var working);
         instance = working;
         return rv;
      }

      /// <summary>
      /// Resolves a named instance of the requested type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The requested service interface type.
      /// </typeparam>
      /// <param name="name">
      /// The name of the instance to retrieve, specify null or <see cref="String.Empty" /> to retrieve the default instance.
      /// </param>
      /// <param name="instance">
      /// The named registered instance when found; otherwise a null reference.
      /// </param>
      /// <returns>
      /// <c>true</c> when the named registered instance was found; otherwise <c>false</c>.
      /// </returns>
      public static Boolean TryResolve<TFrom>(String name, out TFrom instance) where TFrom : class
      {
         var rv = ((IIocServiceLocator)t_singletonInstance).TryResolve<TFrom>(name, out var working);
         instance = working;
         return rv;
      }

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
      public static Boolean TryResolve(Type fromType, out Object instance)
      {
         var rv = ((IIocServiceLocator)t_singletonInstance).TryResolve(fromType, out var working);
         instance = working;
         return rv;
      }

      /// <summary>
      /// Resolves a named instance of the requested type.
      /// </summary>
      /// <param name="fromType">
      /// The requested service interface type.
      /// </param>
      /// <param name="name">
      /// The name of the instance to retrieve, specify null or <see cref="String.Empty" /> to retrieve the default instance.
      /// </param>
      /// <param name="instance">
      /// The named registered instance when found; otherwise a null reference.
      /// </param>
      /// <returns>
      /// <c>true</c> when the named registered instance was found; otherwise <c>false</c>.
      /// </returns>
      public static Boolean TryResolve(Type fromType, String name, out Object instance)
      {
         var rv = ((IIocServiceLocator)t_singletonInstance).TryResolve(fromType, name, out var working);
         instance = working;
         return rv;
      }

      private static IImmutableSet<Type> GetIocInterfacesAndAbstractTypesExceptLoggingInterfaces()
      {
         // must review if a service locatable interface is added to this assembly (e.g., logging interfaces, see below).
         // note:  ILoggerFactory is defined externally.
         var asm = typeof(IIocServiceLocator).Assembly;
         var types = asm.SafeGetTypes();
         var rv = (from t in types where t.IsInterface || t.IsAbstract select t).ToImmutableHashSet();
         rv = rv.Remove(typeof(IIocLoggingUtilitiesService));

         return rv;
      }
   }
}
