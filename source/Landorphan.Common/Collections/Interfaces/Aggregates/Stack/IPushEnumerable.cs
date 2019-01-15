namespace Landorphan.Common.Collections
{
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Push"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IPushEnumerable<T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Adds an Object to the top of the <see cref="IStack{T}"/> .
      /// </summary>
      /// <param name="item">
      /// The Object to add to the <see cref="IStack{T}"/> . The value can be null for reference types.
      /// </param>
      void Push(T item);
   }
}
