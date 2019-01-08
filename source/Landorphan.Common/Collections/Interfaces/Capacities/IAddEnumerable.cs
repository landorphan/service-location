namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Add"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IAddEnumerable<T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Adds an item to the <see cref="IEnumerable{T}"/> .
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      /// <param name="item">
      /// The Object to add to the <see cref="IEnumerable{T}"/> .
      /// </param>
      /// <returns>
      /// <code>true</code> if <paramref name="item"/> was successfully added to the <see cref="IEnumerable{T}"/> ; otherwise, <code>false</code> .
      /// </returns>
      Boolean Add(T item);
   }
}