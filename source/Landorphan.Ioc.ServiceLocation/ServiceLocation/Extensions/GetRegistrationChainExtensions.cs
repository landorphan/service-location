namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using Landorphan.Ioc.ServiceLocation.Internal;

   /// <summary>
   /// Extension methods for IoC querying the type/name is registration key in the given container, and its chain of parents.
   /// </summary>
   public static class GetRegistrationChainExtensions
   {
      /// <summary>
      /// Gets all default registration entries for the given container, and its chain of parents, for the given <typeparamref name="TFrom"/> type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The abstract or interface type to check.
      /// </typeparam>
      /// <param name="container">
      /// The container (registrar or resolver) to inspect.
      /// </param>
      /// <returns>
      /// A non-null collection of zero or more matching entries.
      /// </returns>
      public static IReadOnlyDictionary<IContainerRegistrationKey, IRegistrationValue> GetRegistrationChain<TFrom>(this IIocContainerRegistrationRepository container) where TFrom : class
      {
         container.ArgumentNotNull(nameof(container));
         return GetRegistrationChain<TFrom>(container, String.Empty);
      }

      /// <summary>
      /// Gets all named registration entries for the given container, and its chain of parents, for the given <typeparamref name="TFrom"/> type and <paramref name="name"/> name.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The abstract or interface type to check.
      /// </typeparam>
      /// <param name="container">
      /// The container (registrar or resolver) to inspect.
      /// </param>
      /// <param name="name">
      /// The name to check, use <c> null </c> or <see cref="string.Empty"/> or whitespace to check for the default registration of the given type.
      /// </param>
      /// <returns>
      /// A non-null collection of zero or more matching entries.
      /// </returns>
      public static IReadOnlyDictionary<IContainerRegistrationKey, IRegistrationValue> GetRegistrationChain<TFrom>(this IIocContainerRegistrationRepository container, String name) where TFrom : class
      {
         container.ArgumentNotNull(nameof(container));

         var fromType = typeof(TFrom);
         if (!(fromType.IsInterface || fromType.IsAbstract))
         {
            return ImmutableDictionary<IContainerRegistrationKey, IRegistrationValue>.Empty;
         }

         var cleanedName = name.TrimNullToEmpty();
         var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);

         var builder = ImmutableDictionary<IContainerRegistrationKey, IRegistrationValue>.Empty.ToBuilder();

         var thisRepository = container;
         while (thisRepository != null)
         {
            var immutableDictionary = thisRepository.Registrations;
            var matching = from kvp in immutableDictionary where kvp.Key == key select kvp;
            foreach (var kvp in matching)
            {
               var returnKey = new ContainerRegistrationKeyTypeNameTrio(thisRepository, kvp.Key.RegisteredType, kvp.Key.RegisteredName);
               builder.Add(returnKey, kvp.Value);
            }

            // loop maintenance
            var parent = thisRepository.Container.Parent;
            thisRepository = parent?.Resolver;
         }

         return builder.ToImmutable();
      }

      /// <summary>
      /// Gets all default registration entries for the given container, and its chain of parents, for the given <paramref name="fromType"/> type.
      /// </summary>
      /// <param name="container">
      /// The container (registrar or resolver) to inspect.
      /// </param>
      /// <param name="fromType">
      /// The abstract or interface type to check.
      /// </param>
      /// <returns>
      /// A non-null collection of zero or more matching entries.
      /// </returns>
      public static IReadOnlyDictionary<IContainerRegistrationKey, IRegistrationValue> GetRegistrationChain(this IIocContainerRegistrationRepository container, Type fromType)
      {
         container.ArgumentNotNull(nameof(container));

         return GetRegistrationChain(container, fromType, String.Empty);
      }

      /// <summary>
      /// Gets all named registration entries for the given container, and its chain of parents, for the given <paramref name="fromType"/> type and <paramref name="name"/> name.
      /// </summary>
      /// <param name="container">
      /// The container (registrar or resolver) to inspect.
      /// </param>
      /// <param name="fromType">
      /// The abstract or interface type to check.
      /// </param>
      /// <param name="name">
      /// The name to check, use <c> null </c> or <see cref="string.Empty"/> or whitespace to check for the default registration of the given type.
      /// </param>
      /// <returns>
      /// A non-null collection of zero or more matching entries.
      /// </returns>
      public static IReadOnlyDictionary<IContainerRegistrationKey, IRegistrationValue> GetRegistrationChain(this IIocContainerRegistrationRepository container, Type fromType, String name)
      {
         container.ArgumentNotNull(nameof(container));

         if (fromType == null || !(fromType.IsInterface || fromType.IsAbstract) || fromType.ContainsGenericParameters)
         {
            return ImmutableDictionary<IContainerRegistrationKey, IRegistrationValue>.Empty;
         }

         var cleanedName = name.TrimNullToEmpty();
         var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);

         var builder = ImmutableDictionary<IContainerRegistrationKey, IRegistrationValue>.Empty.ToBuilder();

         var thisRepository = container;
         while (thisRepository != null)
         {
            var immutableDictionary = thisRepository.Registrations;
            var matching = from kvp in immutableDictionary where kvp.Key == key select kvp;
            foreach (var kvp in matching)
            {
               var returnKey = new ContainerRegistrationKeyTypeNameTrio(thisRepository, kvp.Key.RegisteredType, kvp.Key.RegisteredName);
               builder.Add(returnKey, kvp.Value);
            }

            // loop maintenance
            var parent = thisRepository.Container.Parent;
            thisRepository = parent?.Resolver;
         }

         return builder.ToImmutable();
      }
   }
}
