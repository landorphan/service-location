namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Contains"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IContainsEnumerable<T> : IEnumerable<T>
   {
      /// <summary>
      /// Determines whether the current instance contains a specific value.
      /// </summary>
      /// <param name="item">
      /// The Object to locate in the current instance.
      /// </param>
      /// <returns>
      /// <code>true</code> if <paramref name="item"/> is found in the current instance; otherwise, <code>false</code> .
      /// </returns>
      Boolean Contains(T item);
   }
}
