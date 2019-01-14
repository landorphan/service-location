namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="IntersectWith"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IIntersectWithEnumerable<T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Removes all elements from the current instance that are not found within <paramref name="other"/> .
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="other"/>
      /// is null.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      /// <param name="other">
      /// The collection to compare to the current instance.
      /// </param>
      void IntersectWith(IEnumerable<T> other);
   }
}
