namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Overlaps"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IOverlapsEnumerable<T> : IEnumerable<T>
   {
      /// <summary>
      /// Determines whether the current instance overlaps with the specified collection.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="other"/>
      /// is null.
      /// </exception>
      /// <param name="other">
      /// The collection to compare to the current instance.
      /// </param>
      /// <returns>
      /// <code>true</code> if the current instance and <paramref name="other"/> share at least one common element; otherwise, <code>false</code> .
      /// </returns>
      Boolean Overlaps(IEnumerable<T> other);
   }
}
