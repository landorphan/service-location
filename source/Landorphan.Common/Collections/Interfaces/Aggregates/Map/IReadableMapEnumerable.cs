namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Provides the base interface for the abstraction of read operations on maps.
   /// </summary>
   /// <typeparam name="TKey">
   /// The type of keys in the enumerable.
   /// </typeparam>
   /// <typeparam name="TValue">
   /// The type of values in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IReadableMapEnumerable<TKey, TValue>
      : IEnumerable<KeyValuePair<TKey, TValue>>, ICountEnumerable, IIsEmptyEnumerable, IQueryThreadSafe
   {
      /// <summary>
      /// Gets the comparer used to determine (in)equality of keys.
      /// </summary>
      /// <value>
      /// The key comparer.
      /// </value>
      IEqualityComparer<TKey> KeyComparer { get; }

      /// <summary>
      /// Gets a collection containing the keys in the <see cref="IMap{TKey,TValue}"/> .
      /// </summary>
      /// <value>
      /// A <see cref="IReadableBagEnumerable{TKey}"/> containing the keys in the <see cref="IMap{TKey,TValue}"/> .
      /// </value>
      IReadableSetEnumerable<TKey> Keys { get; }

      /// <summary>
      /// Gets the comparer used to determine (in)equality of values.
      /// </summary>
      /// <value>
      /// The value comparer.
      /// </value>
      IEqualityComparer<TValue> ValueComparer { get; }

      /// <summary>
      /// Gets a collection containing the values in the <see cref="IMap{TKey,TValue}"/> .
      /// </summary>
      /// <value>
      /// A <see cref="IReadableBagEnumerable{TValue}"/> containing the values in the <see cref="IMap{TKey,TValue}"/> .
      /// </value>
      IReadableBagEnumerable<TValue> Values { get; }

      /// <summary>
      /// Determines whether the <see cref="IMap{TKey, TValue}"/> contains the specified key.
      /// </summary>
      /// <param name="key">
      /// The key to locate in the <see cref="IMap{TKey, TValue}"/> .
      /// </param>
      /// <returns>
      /// <c> true </c> if the <see cref="IMap{TKey, TValue}"/> contains an element with the specified key; otherwise, <c> false </c> .
      /// </returns>
      Boolean ContainsKey(TKey key);

      /// <summary>
      /// Determines whether the <see cref="IMap{TKey, TValue}"/> contains the specified value.
      /// </summary>
      /// <param name="value">
      /// The value to locate in the <see cref="IMap{TKey, TValue}"/> .
      /// </param>
      /// <returns>
      /// <c> true </c> if the <see cref="IMap{TKey, TValue}"/> contains an element with the specified value; otherwise, <c> false </c> .
      /// </returns>
      Boolean ContainsValue(TValue value);

      /// <summary>
      /// Determines whether or not the key hash has been corrupted.
      /// </summary>
      /// <returns>
      /// <c> true </c> when the key hash has been corrupted; otherwise <c> false </c>.
      /// </returns>
      /// <remarks>
      /// <p>
      /// The key hash can become corrupted when the keys are mutable.  Once two keys share the same value as determined by
      /// <see cref="KeyComparer"/>, <see cref="ContainsKey"/> no longer works correctly and cannot be trusted.  The map must be rebuilt,
      /// and navigation of the values of keys must be performed.
      /// </p>
      /// <p> To avoid this unpleasantness, use immutable keys. </p>
      /// </remarks>
      Boolean IsKeyHashCorrupted();

      /// <summary>
      /// Copies the elements of the <see cref="IMap{TKey, TValue}"/> to an <see cref="IDictionary{TKey, TValue}"/> .
      /// </summary>
      /// <returns>
      /// A dictionary copy of this instance.
      /// </returns>
      IDictionary<TKey, TValue> ToDictionary();

      /// <summary>
      /// Gets the value associated with the specified key.
      /// </summary>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="key"/>
      /// is null.
      /// </exception>
      /// <param name="key">
      /// The key of the value to get.
      /// </param>
      /// <param name="value">
      /// When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default
      /// value for the type of the
      /// <paramref name="value"/> parameter.
      /// </param>
      /// <returns>
      /// <c> true </c> if the <see cref="IMap{TKey, TValue}"/> contains an element with the specified key; otherwise, false.
      /// </returns>
      Boolean TryGetValue(TKey key, out TValue value);
   }
}