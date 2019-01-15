namespace Landorphan.Common
{
   using System;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Extension methods for working with <see cref="Object"/> instances.
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
      [SuppressMessage("SonarLint.CodeSmell" ,"S4225: Extension methods should not extend object")]
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
      [SuppressMessage("SonarLint.CodeSmell" ,"S4225: Extension methods should not extend object")]
      public static Boolean IsNull([ValidatedNotNull] this Object value)
      {
         return ReferenceEquals(value, null);
      }
   }
}
