namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Provides the base interface for the abstraction of mutation operations on sets.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IMutableSetEnumerable<T>
      : IMutableBagEnumerable<T>,
         IExceptWithEnumerable<T>,
         IIntersectWithEnumerable<T>,
         ISymmetricExceptWithEnumerable<T>,
         IUnionWithEnumerable<T>,
         IQueryIgnoresNullValues,
         IQueryThreadSafe
   {
      /// <summary>
      /// Removes non-unique elements from the set.
      /// </summary>
      /// <returns>
      /// <c> true </c> if duplicates were removed; <c> false </c> if no duplicates were detected.
      /// </returns>
      /// <remarks>
      /// Uniqueness is enforced on <see cref="IAddEnumerable{T}.Add"/> and <see cref="IAddRangeEnumerable{T}.AddRange"/>.  However, if elements have mutable
      /// hash values, duplicates
      /// can be introduced via modifications to mutable elements.  Removes all but the last duplicate element for any given hash value.  As a
      /// side-effect, it fixes the bug in <see cref="IContainsEnumerable{T}.Contains"/> that occurs when elements are mutated into equivalents.
      /// (RCA: <see cref="HashSet{T}"/> implementation side-effect).
      /// </remarks>
      Boolean RemoveExistingDuplicates();
   }
}