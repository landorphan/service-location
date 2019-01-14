namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Pop"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IPopEnumerable<out T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Removes and returns the Object at the top of the <see cref="IStack{T}"/> .
      /// </summary>
      /// <exception cref="InvalidOperationException">
      /// The
      /// <see cref="IStack{T}"/>
      /// is empty.
      /// </exception>
      /// <returns>
      /// The Object that is removed from the top of the <see cref="IStack{T}"/> .
      /// </returns>
      T Pop();
   }
}
