namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Provides the base interface for the abstraction of mutation operations on maps.
   /// </summary>
   /// <typeparam name="TKey">
   /// Type of the key.
   /// </typeparam>
   /// <typeparam name="TValue">
   /// Type of the value.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IMutableMapEnumerable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IQueryThreadSafe
   {
      /// <summary>
      /// Attempts to add the specified key and value to the <see cref="IMap{TKey, TValue}"/> .
      /// </summary>
      /// <param name="key">
      /// The key of the element to add.
      /// </param>
      /// <param name="value">
      /// The value of the element to add. The value can be a null reference for reference types.
      /// </param>
      /// <returns>
      /// <c> true </c> if the key/value pair was added to the <see cref="IMap{TKey,TValue}"/> successfully. When the key already exists, this
      /// method returns false. When the value is null, this method returns false.
      /// </returns>
      Boolean Add(TKey key, TValue value);

      /// <summary>
      /// Adds a key/value pair to the <see cref="IMap{TKey,TValue}"/> when the key does not already exist, or updates the value when the key
      /// already exists.
      /// </summary>
      /// <param name="key">
      /// The key to add or locate in the <see cref="IMap{TKey, TValue}"/> .
      /// </param>
      /// <param name="value">
      /// The value to add or update in the <see cref="IMap{TKey, TValue}"/> .
      /// </param>
      void AddOrUpdate(TKey key, TValue value);

      /// <summary>
      /// Removes all items from the <see cref="IMutableMapEnumerable{TKey, TValue}"/> .
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void Clear();

      /// <summary>
      /// Removes the value with the specified key from the <see cref="IMap{TKey, TValue}"/> .
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="key"/>
      /// is null.
      /// </exception>
      /// <param name="key">
      /// The key of the element to remove.
      /// </param>
      /// <returns>
      /// <c> true </c> if the element is successfully found and removed; otherwise, <c> false </c> . This method returns <c> false </c> if
      /// <paramref name="key"/> is not found.
      /// </returns>
      Boolean Remove(TKey key);
   }
}
