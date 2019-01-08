namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Peek"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IPeekEnumerable<out T> : IEnumerable<T>
   {
      /// <summary>
      /// Returns the Object at the top of the <see cref="IStack{T}"/> without removing it.
      /// </summary>
      /// <exception cref="InvalidOperationException">
      /// The
      /// <see cref="IStack{T}"/>
      /// is empty.
      /// </exception>
      /// <returns>
      /// The Object at the top of the <see cref="IStack{T}"/> .
      /// </returns>
      T Peek();
   }
}