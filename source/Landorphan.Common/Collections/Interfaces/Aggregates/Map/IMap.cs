namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Represents a generic collection of key/value pairs.
   /// </summary>
   /// <typeparam name="TKey">
   /// The type of keys in the map.
   /// </typeparam>
   /// <typeparam name="TValue">
   /// The type of values in the map.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IMap<TKey, TValue> : IReadableMapEnumerable<TKey, TValue>, IMutableMapEnumerable<TKey, TValue>, IQueryReadOnly
   {
      /// <summary>
      /// Gets or sets the value associated with the specified key.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="key"/>
      /// is null.
      /// </exception>
      /// <exception cref="KeyNotFoundException">
      /// The property is retrieved and key does not exist in the collection.
      /// </exception>
      /// <param name="key">
      /// The key of the value to get.
      /// </param>
      /// <returns>
      /// The value associated with the specified key.
      /// </returns>
      TValue this[TKey key] { get; set; }
   }
}