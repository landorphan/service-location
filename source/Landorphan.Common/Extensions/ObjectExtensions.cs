namespace Landorphan.Common
{
   using System;

   /// <summary>
   /// Extension methods for <see cref="object"/> instances.
   /// </summary>
   public static class ObjectExtensions
   {
      /// <summary>
      /// Determines whether the specified Object is not null.
      /// </summary>
      /// <param name="value">
      /// The Object.
      /// </param>
      /// <returns>
      /// <c> true </c> if the specified Object is not null; otherwise, <c> false </c> .
      /// </returns>
      public static Boolean IsNotNull([ValidatedNotNull] this Object value)
      {
         return !ReferenceEquals(value, null);
      }

      /// <summary>
      /// Determines whether the specified Object is null.
      /// </summary>
      /// <param name="value">
      /// The Object.
      /// </param>
      /// <returns>
      /// <c> true </c> if the specified Object is null; otherwise, <c> false </c> .
      /// </returns>
      public static Boolean IsNull([ValidatedNotNull] this Object value)
      {
         return ReferenceEquals(value, null);
      }
   }
}