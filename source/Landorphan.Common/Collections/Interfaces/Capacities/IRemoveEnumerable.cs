namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Remove"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IRemoveEnumerable<T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Removes the first occurrence of a matching Object from the <see cref="IEnumerable{T}"/> .
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      /// <param name="item">
      /// The Object to remove from the <see cref="IEnumerable{T}"/> .
      /// </param>
      /// <returns>
      /// <code>true</code> if <paramref name="item"/> was successfully removed from the <see cref="IEnumerable{T}"/> ; otherwise, <code>false</code> . This
      /// method also returns false if
      /// <paramref name="item"/> is not found in the original <see cref="IEnumerable{T}"/>.
      /// </returns>
      Boolean Remove(T item);
   }
}