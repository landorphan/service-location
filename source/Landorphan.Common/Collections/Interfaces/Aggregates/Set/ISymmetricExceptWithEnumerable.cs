namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="SymmetricExceptWith"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface ISymmetricExceptWithEnumerable<T> : IEnumerable<T>
   {
      /// <summary>
      /// Modifies the current instance so that it contains only elements that are present either in the current instance or within
      /// <paramref name="other"/> , but not both.
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
      void SymmetricExceptWith(IEnumerable<T> other);
   }
}
