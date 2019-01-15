namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;

   /// <summary>
   /// A generic collection of key/value pairs.
   /// </summary>
   /// <typeparam name="TKey">
   /// The type of keys in the map.
   /// </typeparam>
   /// <typeparam name="TValue">
   /// The type of values in the map.
   /// </typeparam>
   /// <threadsafety static="true" instance="false"/>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [DebuggerDisplay("Count = {Count}")]
   public sealed class SimpleMap<TKey, TValue> : IMap<TKey, TValue>, IConvertsToReadOnly
   {
      private readonly IEqualityComparer<TKey> _keyComparer;
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper;
      private readonly Dictionary<TKey, TValue> _underlyingMap;
      private readonly IEqualityComparer<TValue> _valueComparer;

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleMap{TKey, TValue}"/> class that is empty and uses the default
      /// <see cref="IEqualityComparer{TKey}"/> and <see cref="IEqualityComparer{TValue}"/>.
      /// </summary>
      public SimpleMap() : this(null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleMap{TKey, TValue}"/> class that copies the given collection, and uses the default
      /// <see cref="IEqualityComparer{TKey}"/> and <see cref="IEqualityComparer{TValue}"/>.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new <see cref="SimpleMap{TKey,TValue}"/>.
      /// </param>
      public SimpleMap(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(collection, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleMap{TKey, TValue}"/> class that is empty and uses the specified
      /// <see cref="IEqualityComparer{TKey}"/> and <see cref="IEqualityComparer{TValue}"/>.
      /// </summary>
      /// <param name="keyComparer">
      /// The key comparer to use when comparing keys, or null to use the default <see cref="IEqualityComparer{TKey}"/>.
      /// </param>
      /// <param name="valueComparer">
      /// The value comparer to use when comparing values, or null to use the default <see cref="IEqualityComparer{TValue}"/>.
      /// </param>
      public SimpleMap(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) : this(
         null,
         keyComparer,
         valueComparer)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleMap{TKey, TValue}"/> class that copies the given collection and uses the specified
      /// <see cref="IEqualityComparer{TKey}"/> and <see cref="IEqualityComparer{TValue}"/>.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new <see cref="SimpleMap{TKey,TValue}"/>, or null to create an empty map.
      /// </param>
      /// <param name="keyComparer">
      /// The key comparer to use when comparing keys, or null to use the default <see cref="IEqualityComparer{TKey}"/>.
      /// </param>
      /// <param name="valueComparer">
      /// The value comparer to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{TValue}"/>.
      /// </param>
      public SimpleMap(
         IEnumerable<KeyValuePair<TKey, TValue>> collection,
         IEqualityComparer<TKey> keyComparer,
         IEqualityComparer<TValue> valueComparer)
      {
         _supportsReadOnlyHelper = new SupportsReadOnlyHelper();

         _keyComparer = ReferenceEquals(keyComparer, null) ? EqualityComparer<TKey>.Default : keyComparer;
         _valueComparer = ReferenceEquals(valueComparer, null) ? EqualityComparer<TValue>.Default : valueComparer;
         _underlyingMap = new Dictionary<TKey, TValue>(_keyComparer);
         if (collection != null)
         {
            foreach (var pair in collection)
            {
               Add(pair.Key, pair.Value);
            }
         }
      }

      /// <inheritdoc/>
      public Int32 Count => _underlyingMap.Count;

      /// <inheritdoc/>
      public Boolean IsEmpty => _underlyingMap.Count == 0;

      /// <inheritdoc/>
      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => IsReadOnly;

      /// <inheritdoc/>
      public IEqualityComparer<TKey> KeyComparer => _keyComparer;

      /// <inheritdoc/>
      public IReadableSetEnumerable<TKey> Keys
      {
         get
         {
            var snapshot = new SimpleSet<TKey>(_underlyingMap.Keys, _keyComparer);
            snapshot.MakeReadOnly();
            return snapshot;
         }
      }

      /// <inheritdoc/>
      public IEqualityComparer<TValue> ValueComparer => _valueComparer;

      /// <inheritdoc/>
      public IReadableBagEnumerable<TValue> Values
      {
         get
         {
            var snapshot = new SimpleBag<TValue>(_underlyingMap.Values, _valueComparer, false);
            snapshot.MakeReadOnly();
            return snapshot;
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

         set
         {
            _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
            AddOrUpdate(key, value);
         }
      }

      /// <inheritdoc/>
      public void MakeReadOnly()
      {
         _supportsReadOnlyHelper.MakeReadOnly();
      }

      /// <inheritdoc/>
      public Boolean Add(TKey key, TValue value)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         if (_underlyingMap.ContainsKey(key))
         {
            return false;
         }

         _underlyingMap.Add(key, value);
         return true;
      }

      /// <inheritdoc/>
      public void AddOrUpdate(TKey key, TValue value)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         if (_underlyingMap.ContainsKey(key))
         {
            _underlyingMap[key] = value;
         }
         else
         {
            _underlyingMap.Add(key, value);
         }
      }

      /// <inheritdoc/>
      public void Clear()
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         _underlyingMap.Clear();
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
         var q = from kvp in _underlyingMap select new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
         var snapshot = new SimpleBag<KeyValuePair<TKey, TValue>>(q);
         snapshot.MakeReadOnly();
         return snapshot.GetEnumerator();
      }

      /// <inheritdoc/>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
      }

      /// <inheritdoc/>
      public Boolean IsKeyHashCorrupted()
      {
         if (typeof(TKey).IsValueType || typeof(TKey) == typeof(String))
         {
            return false;
         }

         var rv = CheckForDuplicateKeys();
         return rv;
      }

      /// <inheritdoc/>
      public Boolean Remove(TKey key)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         var rv = _underlyingMap.Remove(key);
         return rv;
      }

      /// <inheritdoc/>
      public IDictionary<TKey, TValue> ToDictionary()
      {
         return this.ToDictionary(pair => pair.Key, pair => pair.Value);
      }

      /// <inheritdoc/>
      public Boolean TryGetValue(TKey key, out TValue value)
      {
         return _underlyingMap.TryGetValue(key, out value);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var hashcode = (Int32)_underlyingMap.LongCount();
            return hashcode;
         }
      }

      private Boolean CheckForDuplicateKeys()
      {
         var deepCheck = false;
         var hashValues = new SimpleSet<Int32>();
         foreach (var key in _underlyingMap.Keys)
         {
            // some comparer GetHashCode implementations throw on null, avoid the exceptions
            var hashValue = ReferenceEquals(key, null) ? 0 : _underlyingMap.Comparer.GetHashCode(key);
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
            var mySet = new SimpleSet<TKey>(_underlyingMap.Comparer);
            mySet.AddRange(_underlyingMap.Keys);
            rv = mySet.Count != _underlyingMap.Keys.Count;
         }

         return rv;
      }
   }
}
