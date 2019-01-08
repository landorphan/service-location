namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using Landorphan.Common.Threading;

   /// <summary>
   /// A generic collection of key/value pairs.
   /// </summary>
   /// <typeparam name="TKey">
   /// The type of keys in the map.
   /// </typeparam>
   /// <typeparam name="TValue">
   /// The type of values in the map.
   /// </typeparam>
   /// <threadsafety static="true" instance="true"/>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [DebuggerDisplay("Count = {Count}")]
   public sealed class ThreadSafeMap<TKey, TValue> : IMap<TKey, TValue>
   {
      private readonly INonRecursiveLock _lock;
      private ImmutableDictionary<TKey, TValue> _underlyingMap;

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeMap{TKey, TValue}"/> class that is empty and uses the default
      /// <see cref="IEqualityComparer{T}"/> and <see cref="IEqualityComparer{TValue}"/>.
      /// </summary>
      public ThreadSafeMap() : this(null, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeMap{TKey, TValue}"/> class that copies the given collection, and uses the default
      /// <see cref="IEqualityComparer{TKey}"/> and <see cref="IEqualityComparer{TValue}"/>.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new <see cref="ThreadSafeMap{TKey,TValue}"/>.
      /// </param>
      public ThreadSafeMap(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(collection, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeMap{TKey, TValue}"/> class that is empty and uses the specified
      /// <see cref="IEqualityComparer{TKey}"/> and <see cref="IEqualityComparer{TValue}"/>.
      /// </summary>
      /// <param name="keyComparer">
      /// The key comparer to use when comparing keys, or null to use the default <see cref="IEqualityComparer{TKey}"/>.
      /// </param>
      /// <param name="valueComparer">
      /// The value comparer to use when comparing values, or null to use the default <see cref="IEqualityComparer{TValue}"/>.
      /// </param>
      public ThreadSafeMap(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
         : this(null, keyComparer, valueComparer, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeMap{TKey, TValue}"/> class that copies the given collection and uses the specified
      /// <see cref="IEqualityComparer{TKey}"/> and <see cref="IEqualityComparer{TValue}"/>.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new <see cref="ThreadSafeMap{TKey,TValue}"/>, or null to create an empty map.
      /// </param>
      /// <param name="keyComparer">
      /// The key comparer to use when comparing keys, or null to use the default <see cref="IEqualityComparer{TKey}"/>.
      /// </param>
      /// <param name="valueComparer">
      /// The value comparer to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{TValue}"/>.
      /// </param>
      public ThreadSafeMap(
         IEnumerable<KeyValuePair<TKey, TValue>> collection,
         IEqualityComparer<TKey> keyComparer,
         IEqualityComparer<TValue> valueComparer) : this(collection, keyComparer, valueComparer, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeMap{TKey, TValue}"/> class that copies the given collection and uses the specified
      /// <see cref="IEqualityComparer{TKey}"/> and <see cref="IEqualityComparer{TValue}"/>.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new <see cref="ThreadSafeMap{TKey,TValue}"/>, or null to create an empty map.
      /// </param>
      /// <param name="keyComparer">
      /// The key comparer to use when comparing keys, or null to use the default <see cref="IEqualityComparer{TKey}"/>.
      /// </param>
      /// <param name="valueComparer">
      /// The value comparer to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{TValue}"/>.
      /// </param>
      /// <param name="lockImplementation">
      /// The lock implementation, or null for the default implementation.
      /// </param>
      public ThreadSafeMap(
         IEnumerable<KeyValuePair<TKey, TValue>> collection,
         IEqualityComparer<TKey> keyComparer,
         IEqualityComparer<TValue> valueComparer,
         INonRecursiveLock lockImplementation)
      {
         _lock = lockImplementation ?? new NonRecursiveLock();

         _underlyingMap = ImmutableDictionary<TKey, TValue>.Empty.WithComparers(
            keyComparer ?? EqualityComparer<TKey>.Default,
            valueComparer ?? EqualityComparer<TValue>.Default);

         if (collection.IsNotNull())
         {
            _underlyingMap = _underlyingMap.AddRange(collection);
         }
      }

      /// <inheritdoc/>
      public Int32 Count => _underlyingMap.Count;

      /// <inheritdoc/>
      public Boolean IsEmpty => _underlyingMap.Count == 0;

      /// <inheritdoc/>
      public Boolean IsReadOnly => false;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => true;

      /// <inheritdoc/>
      public IEqualityComparer<TKey> KeyComparer => _underlyingMap.KeyComparer;

      /// <inheritdoc/>
      public IReadableSetEnumerable<TKey> Keys
      {
         get
         {
            var snapshot = _underlyingMap;
            var rv = new ThreadSafeSet<TKey>(snapshot.Keys, snapshot.KeyComparer);
            return rv;
         }
      }

      /// <inheritdoc/>
      public IEqualityComparer<TValue> ValueComparer => _underlyingMap.ValueComparer;

      /// <inheritdoc/>
      public IReadableBagEnumerable<TValue> Values
      {
         get
         {
            var snapshot = _underlyingMap;
            var rv = new ThreadSafeBag<TValue>(snapshot.Values, snapshot.ValueComparer, false);
            return rv;
         }
      }

      /// <inheritdoc/>
      public TValue this[TKey key]
      {
         get
         {
            TValue value;
            var keyFound = _underlyingMap.TryGetValue(key, out value);

            if (!keyFound)
            {
               value = default;
            }

            return value;
         }

         set => AddOrUpdate(key, value);
      }

      /// <inheritdoc/>
      public Boolean Add(TKey key, TValue value)
      {
         using (_lock.EnterWriteLock())
         {
            if (_underlyingMap.ContainsKey(key))
            {
               return false;
            }

            _underlyingMap = _underlyingMap.Add(key, value);
            return true;
         }
      }

      /// <inheritdoc/>
      public void AddOrUpdate(TKey key, TValue value)
      {
         using (_lock.EnterWriteLock())
         {
            if (_underlyingMap.ContainsKey(key))
            {
               _underlyingMap = _underlyingMap.SetItem(key, value);
            }
            else
            {
               _underlyingMap = _underlyingMap.Add(key, value);
            }
         }
      }

      /// <inheritdoc/>
      public void Clear()
      {
         using (_lock.EnterWriteLock())
         {
            _underlyingMap = _underlyingMap.Clear();
         }
      }

      /// <inheritdoc/>
      public Boolean ContainsKey(TKey key)
      {
         IReadableBagEnumerable<TKey> snapshot = Keys;
         return snapshot.Contains(key);
      }

      /// <inheritdoc/>
      public Boolean ContainsValue(TValue value)
      {
         var snapshot = Values;
         return snapshot.Contains(value);
      }

      /// <inheritdoc/>
      IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
      {
         var snapshot = _underlyingMap;
         return snapshot.GetEnumerator();
      }

      /// <inheritdoc/>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return ((IEnumerable<KeyValuePair<TKey, TValue>>) this).GetEnumerator();
      }

      /// <inheritdoc/>
      public Boolean IsKeyHashCorrupted()
      {
         if (typeof(TKey).IsValueType)
         {
            return false;
         }

         var rv = CheckForDuplicateKeys();
         return rv;
      }

      /// <inheritdoc/>
      public Boolean Remove(TKey key)
      {
         var rv = false;
         using (_lock.EnterWriteLock())
         {
            var original = _underlyingMap;
            var modified = _underlyingMap.Remove(key);
            if (!ReferenceEquals(original, modified))
            {
               rv = true;
               _underlyingMap = modified;
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      public IDictionary<TKey, TValue> ToDictionary()
      {
         var snapshot = _underlyingMap;
         return snapshot.ToDictionary(pair => pair.Key, pair => pair.Value);
      }

      /// <inheritdoc/>
      public Boolean TryGetValue(TKey key, out TValue value)
      {
         using (_lock.EnterReadLock())
         {
            return _underlyingMap.TryGetValue(key, out value);
         }
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var hashcode = (Int32) _underlyingMap.LongCount();
            return hashcode;
         }
      }

      private Boolean CheckForDuplicateKeys()
      {
         using (_lock.EnterReadLock())
         {
            var deepCheck = false;
            var hashValues = new SimpleSet<Int32>();

            // note:  must enumerate key-value pairs not keys
            var kvps = new List<KeyValuePair<TKey, TValue>>();
            using (var kvpEnumerator = ((IEnumerable<KeyValuePair<TKey, TValue>>) _underlyingMap).GetEnumerator())
            {
               while (kvpEnumerator.MoveNext())
               {
                  kvps.Add(kvpEnumerator.Current);
               }
            }

            foreach (var kvp in kvps)
            {
               // some comparer GetHashCode implementations throw on null, avoid the exceptions
               var hashValue = ReferenceEquals(kvp.Key, null) ? 0 : _underlyingMap.KeyComparer.GetHashCode(kvp.Key);
               var isUnique = hashValues.Add(hashValue);
               if (!isUnique)
               {
                  deepCheck = true;
                  break;
               }
            }

            var rv = deepCheck;
            if (deepCheck)
            {
               // the were duplicate hash values so perform deep check using full equality evaluation.
               var mySet = new SimpleSet<TKey>(_underlyingMap.KeyComparer);
               mySet.AddRange(from kvp in kvps select kvp.Key);
               rv = mySet.Count != kvps.Count;
            }

            return rv;
         }
      }
   }
}