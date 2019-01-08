namespace Landorphan.Common.Collections
{
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Provides the base interface for the abstraction of read operations on bags.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IReadableBagEnumerable<T>
      : ICountEnumerable, IIsEmptyEnumerable, IContainsEnumerable<T>, IQueryIgnoresNullValues, IQueryThreadSafe
   {
      /// <summary>
      /// Gets the comparer used to determine (in)equality of values.
      /// </summary>
      /// <value>
      /// The equality comparer.
      /// </value>
      IEqualityComparer<T> EqualityComparer { get; }
   }
}