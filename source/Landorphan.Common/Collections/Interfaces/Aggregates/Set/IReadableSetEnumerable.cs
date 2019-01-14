namespace Landorphan.Common.Collections
{
   using System;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Provides the base interface for the abstraction of read operations on sets.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the set.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IReadableSetEnumerable<T>
      : IReadableBagEnumerable<T>,
         IIsProperSubsetOfEnumerable<T>,
         IIsProperSupersetOfEnumerable<T>,
         IIsSubsetOfEnumerable<T>,
         IIsSupersetOfEnumerable<T>,
         IOverlapsEnumerable<T>,
         ISetEqualsEnumerable<T>,
         IQueryIgnoresNullValues,
         IQueryThreadSafe
   {
      /// <summary>
      /// Checks for the existence of duplicate elements.
      /// </summary>
      /// <returns>
      /// <c> true </c> if duplicates were detected; otherwise <c> false </c>.
      /// </returns>
      /// <remarks>
      /// Uniqueness is enforced on <see cref="IAddEnumerable{T}.Add"/> and <see cref="IAddRangeEnumerable{T}.AddRange"/>.  However, if elements have mutable
      /// hash values, duplicates
      /// can be introduced via modifications to mutable elements.  This method determines whether or not such has occurred.  To repair this
      /// state using the default algorithm, call <see cref="IMutableSetEnumerable{T}.RemoveExistingDuplicates"/>
      /// <p> To avoid this unpleasantness, use immutable values. </p>
      /// </remarks>
      Boolean CheckForExistingDuplicates();
   }
}
