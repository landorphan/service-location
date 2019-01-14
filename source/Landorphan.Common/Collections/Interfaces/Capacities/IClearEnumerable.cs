namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Clear"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IClearEnumerable<out T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Removes all items from the <see cref="IEnumerable{T}"/> .
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void Clear();
   }
}
