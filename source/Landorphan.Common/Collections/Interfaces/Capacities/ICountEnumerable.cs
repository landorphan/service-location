namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="Count"/> .
   /// </summary>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Design", "CA1010: Collections should implement generic interface", Justification = "This represents a capacity, not a collection.")]
   public interface ICountEnumerable : IEnumerable
   {
      /// <summary>
      /// Gets the number of elements contained in the <see cref="IEnumerable"/> .
      /// </summary>
      /// <value>
      /// The count.
      /// </value>
      Int32 Count { get; }
   }
}