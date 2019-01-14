namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Linq;
   using Landorphan.Common;

   /// <summary>
   /// Extension methods for IoC querying whether a type/name is registered in the given container, or the given container and its chain of parents.
   /// </summary>
   public static class IsPrecludedExtensions
   {
      /// <summary>
      /// Determines whether or not a given type is precluded from service location.
      /// </summary>
      /// <remarks>
      /// Checks the types precluded by this container, as well as any types precluded on the parent chain.
      /// </remarks>
      /// <typeparam name="TPrecluded">
      /// The type to check.
      /// </typeparam>
      /// <param name="manager">
      /// The container manager.
      /// </param>
      /// <returns>
      /// <c> true </c> if this instance is precluded from registration for service location; otherwise <c> false </c>.
      /// </returns>
      public static Boolean IsPrecluded<TPrecluded>(this IIocContainerManager manager)
      {
         manager.ArgumentNotNull(nameof(manager));

         var type = typeof(TPrecluded);
         var rv = manager.PrecludedTypes.Contains(type);
         return rv;
      }

      /// <summary>
      /// Determines whether or not a given type is precluded from service location.
      /// </summary>
      /// <remarks>
      /// Checks the types precluded by this container, as well as any types precluded on the parent chain.
      /// </remarks>
      /// <param name="manager">
      /// The container manager.
      /// </param>
      /// <param name="fromType">
      /// The type to check.
      /// </param>
      /// <returns>
      /// <c> true </c> if this instance is precluded from registration for service location; otherwise <c> false </c>.
      /// </returns>
      public static Boolean IsPrecluded(this IIocContainerManager manager, Type fromType)
      {
         manager.ArgumentNotNull(nameof(manager));
         var rv = manager.PrecludedTypes.Contains(fromType);
         return rv;
      }
   }
}
