namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="AddRange"/> .
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IAddRangeEnumerable<T> : IQueryReadOnly, IEnumerable<T>
   {
      /// <summary>
      /// Attempts to add all elements in source to the current instance.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="source"/>
      /// is null.
      /// </exception>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      /// <param name="source">
      /// The source enumerable which is copied.
      /// </param>
      void AddRange(IEnumerable<T> source);
   }
}
