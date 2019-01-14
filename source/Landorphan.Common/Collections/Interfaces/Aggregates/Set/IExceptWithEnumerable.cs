namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="ExceptWith"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IExceptWithEnumerable<T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Removes all elements from the current instance that are found within <paramref name="other"/> .
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="other"/>
      /// is null.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      /// <param name="other">
      /// The collection of items to remove from the current instance.
      /// </param>
      void ExceptWith(IEnumerable<T> other);
   }
}
