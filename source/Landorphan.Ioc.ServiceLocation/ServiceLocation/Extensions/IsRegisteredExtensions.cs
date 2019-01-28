namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using Landorphan.Ioc.ServiceLocation.Internal;

   /// <summary>
   /// Extension methods for IoC.
   /// </summary>
   public static class IsRegisteredExtensions
   {
      /// <summary>
      /// Determines whether or not the given <paramref name="container"/> container, contains a default registration for the given <typeparamref name="TFrom"/> type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The abstract or interface type to check.
      /// </typeparam>
      /// <param name="container">
      /// The container (registrar or resolver) to inspect.
      /// </param>
      /// <returns>
      /// <c>true</c> if the given <paramref name="container"/> container, contains a default registration for the given <typeparamref name="TFrom"/> type;
      /// otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      public static Boolean IsRegistered<TFrom>(this IIocContainerRegistrationRepository container) where TFrom : class
      {
         container.ArgumentNotNull(nameof(container));

         return IsRegistered<TFrom>(container, String.Empty);
      }

      /// <summary>
      /// Determines whether or not the given <paramref name="container"/> container, contains a named registration for the given <typeparamref name="TFrom"/> type.
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
      /// <c>true</c> if the given <paramref name="container"/> container, contains a named registration for the given <typeparamref name="TFrom"/> type that matches <paramref name="name"/>;
      /// otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      public static Boolean IsRegistered<TFrom>(this IIocContainerRegistrationRepository container, String name)
      {
         container.ArgumentNotNull(nameof(container));
         var immutableDictionary = container.Registrations;

         var fromType = typeof(TFrom);
         if (!(fromType.IsInterface || fromType.IsAbstract))
         {
            return false;
         }

         var cleanedName = name.TrimNullToEmpty();
         var key = new RegistrationKeyTypeNamePair(typeof(TFrom), cleanedName);
         var rv = immutableDictionary.ContainsKey(key);
         return rv;
      }

      /// <summary>
      /// Determines whether or not the given <paramref name="container"/> container, contains a default registration for the given <paramref name="fromType"/> type.
      /// </summary>
      /// <param name="container">
      /// The container (registrar or resolver) to inspect.
      /// </param>
      /// <param name="fromType">
      /// The abstract or interface type to check.
      /// </param>
      /// <returns>
      /// <c>true</c> if the given <paramref name="container"/> container, contains a default registration for the given <paramref name="fromType"/> type;
      /// otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      public static Boolean IsRegistered(this IIocContainerRegistrationRepository container, Type fromType)
      {
         container.ArgumentNotNull(nameof(container));

         return IsRegistered(container, fromType, String.Empty);
      }

      /// <summary>
      /// Determines whether or not the given container identified by the <see cref="IIocContainerRegistrationRepository"/>, contains a named <paramref name="name"/> registration for the
      /// given <paramref name="fromType"/> type.
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
      /// <c>true</c> if the given <paramref name="container"/> container, contains a named registration for the given <paramref name="fromType"/> type that matches <paramref name="name"/>;
      /// otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      public static Boolean IsRegistered(this IIocContainerRegistrationRepository container, Type fromType, String name)
      {
         container.ArgumentNotNull(nameof(container));
         var immutableDictionary = container.Registrations;

         if (fromType == null || !(fromType.IsInterface || fromType.IsAbstract) || fromType.ContainsGenericParameters)
         {
            return false;
         }

         var cleanedName = name.TrimNullToEmpty();
         var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);
         var rv = immutableDictionary.ContainsKey(key);
         return rv;
      }

      /// <summary>
      /// Determines whether or not the given <paramref name="container"/> container, or its chain of parents, contains a default registration for the given <typeparamref name="TFrom"/> type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The abstract or interface type to check.
      /// </typeparam>
      /// <param name="container">
      /// The container (registrar or resolver) to inspect.
      /// </param>
      /// <returns>
      /// <c>true</c> if the given <paramref name="container"/> container, or its chain of parents, contains a default registration for the given <typeparamref name="TFrom"/> type;
      /// otherwise <c>false</c>.
      /// </returns>
      public static Boolean IsRegisteredChain<TFrom>(this IIocContainerRegistrationRepository container) where TFrom : class
      {
         container.ArgumentNotNull(nameof(container));

         return IsRegisteredChain<TFrom>(container, String.Empty);
      }

      /// <summary>
      /// Determines whether or not the given <paramref name="container"/> container, or its chain of parents, contains a default registration for the given <typeparamref name="TFrom"/> type.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The abstract or interface type to check.
      /// </typeparam>
      /// <param name="container">
      /// The container (registrar or resolver) to inspect.
      /// </param>
      /// <param name="name">
      /// The name to check, use <c> null </c> or <see cref="string.Empty"/> or whitespace to check for the default registration of the given type.
      /// </param> <returns>
      /// <c>true</c> if the given <paramref name="container"/> container, contains a named registration for the given <typeparamref name="TFrom"/> type that matches <paramref name="name"/>;
      /// otherwise <c>false</c>.
      /// </returns>
      public static Boolean IsRegisteredChain<TFrom>(this IIocContainerRegistrationRepository container, String name) where TFrom : class
      {
         container.ArgumentNotNull(nameof(container));

         var fromType = typeof(TFrom);
         if (!(fromType.IsInterface || fromType.IsAbstract))
         {
            return false;
         }

         var cleanedName = name.TrimNullToEmpty();
         var key = new RegistrationKeyTypeNamePair(typeof(TFrom), cleanedName);

         var thisRepository = container;
         var immutableDictionary = thisRepository.Registrations;
         while (thisRepository != null)
         {
            if (immutableDictionary.ContainsKey(key))
            {
               return true;
            }

            // loop maintenance
            var parent = thisRepository.Container.Parent;
            thisRepository = parent?.Resolver;
            immutableDictionary = thisRepository?.Registrations;
         }

         return false;
      }

      /// <summary>
      /// Determines whether or not the given <paramref name="container"/> container, or its chain of parents, contains a default registration for the given <paramref name="fromType"/> type.
      /// </summary>
      /// <param name="container">
      /// The container (registrar or resolver) to inspect.
      /// </param>
      /// <param name="fromType">
      /// The abstract or interface type to check.
      /// </param>
      /// <remarks>
      /// <c>true</c> if the given <paramref name="container"/> container, or its chain of parents, contains a default registration for the given <paramref name="fromType"/> type;
      /// otherwise <c>false</c>.
      /// </remarks>
      public static Boolean IsRegisteredChain(this IIocContainerRegistrationRepository container, Type fromType)
      {
         container.ArgumentNotNull(nameof(container));

         return IsRegisteredChain(container, fromType, String.Empty);
      }

      /// <summary>
      /// Determines whether or not the given <paramref name="container"/> container, or its chain of parents, contains a named registration for the given <paramref name="fromType"/> type.
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
      /// <c>true</c> if the given the identified container <see cref="IIocContainerRegistrationRepository"/>, contains a named registration with the given name <paramref name="name"/> for the
      /// given <paramref name="fromType"/> type; otherwise <c>false</c>.
      /// </returns>
      public static Boolean IsRegisteredChain(this IIocContainerRegistrationRepository container, Type fromType, String name)
      {
         container.ArgumentNotNull(nameof(container));

         if (fromType == null || !(fromType.IsInterface || fromType.IsAbstract) || fromType.ContainsGenericParameters)
         {
            return false;
         }

         var cleanedName = name.TrimNullToEmpty();
         var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);

         var thisRepository = container;
         var immutableDictionary = thisRepository.Registrations;
         while (thisRepository != null)
         {
            if (immutableDictionary.ContainsKey(key))
            {
               return true;
            }

            // loop maintenance
            var parent = thisRepository.Container.Parent;
            thisRepository = parent?.Resolver;
            immutableDictionary = thisRepository?.Registrations;
         }

         return false;
      }
   }
}
