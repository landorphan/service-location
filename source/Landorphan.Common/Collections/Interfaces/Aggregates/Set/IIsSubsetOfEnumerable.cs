namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="IsSubsetOf"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IIsSubsetOfEnumerable<T> : IEnumerable<T>
   {
      /// <summary>
      /// Determines whether a set is a subset of a specified collection.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="other"/>
      /// is null.
      /// </exception>
      /// <param name="other">
      /// The collection to compare to the current instance.
      /// </param>
      /// <returns>
      /// <code>true</code> if the current instance is a subset of <paramref name="other"/> ; otherwise, <code>false</code> .
      /// </returns>
      Boolean IsSubsetOf(IEnumerable<T> other);
   }
}
