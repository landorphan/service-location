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
   /// An unordered collection of values.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the collection.
   /// </typeparam>
   /// <threadsafety static="true" instance="true"/>
   /// <seealso cref="DisposableObject"/>
   /// <seealso cref="IBag{T}"/>
   /// <seealso cref="ICollection{T}"/>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [DebuggerDisplay("Count = {Count}")]
   public sealed class ThreadSafeBag<T> : IBag<T>, ICollection<T>
   {
      private readonly IEqualityComparer<T> _equalityComparer;
      private readonly INonRecursiveLock _lock;
      private ImmutableList<T> _collection;

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeBag{T}"/> class.
      /// </summary>
      public ThreadSafeBag() : this(Array.Empty<T>(), null, true, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeBag{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new bag.
      /// </param>
      public ThreadSafeBag(IEnumerable<T> collection) : this(collection, null, true, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeBag{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new bag.
      /// </param>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{T}"/>
      /// implementation for the type.
      /// </param>
      /// <param name="ignoreNullValues">
      /// When set to <c> true </c> null values are ignored.
      /// </param>
      public ThreadSafeBag(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer, Boolean ignoreNullValues)
         : this(collection, equalityComparer, ignoreNullValues, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeBag{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new bag.
      /// </param>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{T}"/>
      /// implementation for the type.
      /// </param>
      /// <param name="ignoreNullValues">
      /// When set to <c> true </c> null values are ignored.
      /// </param>
      /// <param name="lockImplementation">
      /// The lock implementation, or null for the default implementation.
      /// </param>
      public ThreadSafeBag(
         IEnumerable<T> collection,
         IEqualityComparer<T> equalityComparer,
         Boolean ignoreNullValues,
         INonRecursiveLock lockImplementation)
      {
         _lock = lockImplementation ?? new NonRecursiveLock();
         IgnoresNullValues = ignoreNullValues;

         _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
         _collection = ImmutableList<T>.Empty;

         if (collection.IsNotNull())
         {
            AddRange(collection);
         }
      }

      /// <inheritdoc/>
      public Int32 Count => _collection.Count;

      /// <inheritdoc/>
      public IEqualityComparer<T> EqualityComparer => _equalityComparer;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues { get; }

      /// <inheritdoc/>
      public Boolean IsEmpty => _collection.Count == 0;

      /// <inheritdoc/>
      public Boolean IsReadOnly => false;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => true;

      /// <inheritdoc/>
      public Boolean Add(T item)
      {
         if (IgnoresNullValues && !typeof(T).IsValueType && item.IsNull())
         {
            return false;
         }

         var rv = false;
         using (_lock.EnterWriteLock())
         {
            var original = _collection;
            var modified = _collection.Add(item);
            if (!ReferenceEquals(original, modified))
            {
               rv = true;
               _collection = modified;
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      public void AddRange(IEnumerable<T> source)
      {
         source.ArgumentNotNull("source");
         var cleaned = source.ToList();

         if (IgnoresNullValues && !typeof(T).IsValueType)
         {
            cleaned = (from item in cleaned where item.IsNotNull() select item).ToList();
         }

         using (_lock.EnterWriteLock())
         {
            _collection = _collection.AddRange(cleaned);
         }
      }

      /// <inheritdoc/>
      public void Clear()
      {
         using (_lock.EnterWriteLock())
         {
            _collection = _collection.Clear();
         }
      }

      /// <inheritdoc/>
      public Boolean Contains(T item)
      {
         var snapshot = TakeSnapshot();
         return snapshot.Contains(item, _equalityComparer);
      }

      /// <inheritdoc/>
      public IEnumerator<T> GetEnumerator()
      {
         return _collection.GetEnumerator();
      }

      /// <inheritdoc/>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <inheritdoc/>
      public Boolean Remove(T item)
      {
         if (IgnoresNullValues && !typeof(T).IsValueType && item.IsNull())
         {
            return false;
         }

         var rv = false;
         using (_lock.EnterWriteLock())
         {
            var original = _collection;
            var modified = _collection.Remove(item, _equalityComparer);
            if (!ReferenceEquals(original, modified))
            {
               rv = true;
               _collection = modified;
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      void ICollection<T>.Add(T item)
      {
         Add(item);
      }

      /// <inheritdoc/>
      void ICollection<T>.CopyTo(T[] array, Int32 arrayIndex)
      {
         _collection.CopyTo(array, arrayIndex);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var hashcode = (Int32) _collection.LongCount();
            return hashcode;
         }
      }

      private IImmutableList<T> TakeSnapshot()
      {
         using (_lock.EnterWriteLock())
         {
            var rv = _collection;
            return rv;
         }
      }
   }
}