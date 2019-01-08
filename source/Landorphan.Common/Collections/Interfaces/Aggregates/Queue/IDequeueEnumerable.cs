namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Dequeue"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IDequeueEnumerable<out T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Removes and returns the Object at the beginning of the <see cref="IEnumerable{T}"/> .
      /// </summary>
      /// <exception cref="InvalidOperationException">
      /// The
      /// <see cref="IEnumerable{T}"/>
      /// is empty.
      /// </exception>
      /// <returns>
      /// The Object that is removed from the beginning of the <see cref="IEnumerable{T}"/> .
      /// </returns>
      T Dequeue();
   }
}