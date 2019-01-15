namespace Landorphan.Common.Collections
{
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Enqueue"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IEnqueueEnumerable<T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Adds an Object to the end of the <see cref="IEnumerable{T}"/> .
      /// </summary>
      /// <param name="item">
      /// The Object to add to the <see cref="IEnumerable{T}"/> . The value can be null for reference types.
      /// </param>
      void Enqueue(T item);
   }
}
