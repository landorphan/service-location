namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using Landorphan.Common.Threading;

   /// <summary>
   /// Represents a set of instances of the same arbitrary type.
   /// </summary>
   /// <typeparam name="T">
   /// The type of the elements.
   /// </typeparam>
   /// <threadsafety static="true" instance="true"/>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public sealed class ThreadSafeSet<T> : ISet<T>, ISet2<T>, IConvertsToReadOnly
   {
      private readonly INonRecursiveLock _lock;
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper;
      private ImmutableHashSet<T> _set;

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeSet{T}"/> class.
      /// </summary>
      public ThreadSafeSet() : this(null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeSet{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new list.
      /// </param>
      public ThreadSafeSet(IEnumerable<T> collection) : this(collection, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeSet{T}"/> class.
      /// </summary>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{T}"/> implementation for the type.
      /// </param>
      public ThreadSafeSet(IEqualityComparer<T> equalityComparer) : this(null, equalityComparer, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeSet{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new list.
      /// </param>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{T}"/> implementation for the type.
      /// </param>
      public ThreadSafeSet(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer) : this(collection, equalityComparer, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeSet{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new collection.
      /// </param>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{T}"/> implementation for the type.
      /// </param>
      /// <param name="lockImplementation">
      /// The lock implementation, or null for the default implementation.
      /// </param>
      public ThreadSafeSet(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer, INonRecursiveLock lockImplementation)
      {
         _lock = lockImplementation ?? new NonRecursiveLock();
         _supportsReadOnlyHelper = new SupportsReadOnlyHelper();

         var workingCollection = new List<T>();
         if (collection.IsNotNull())
         {
            if (!typeof(T).IsValueType)
            {
               workingCollection.AddRange(from item in collection where item.IsNotNull() select item);
            }
            else
            {
               workingCollection.AddRange(collection);
            }
         }

         _set = ImmutableHashSet<T>.Empty.WithComparer(equalityComparer ?? EqualityComparer<T>.Default);
         if (workingCollection.Count > 0)
         {
            _set = _set.Union(workingCollection);
         }
      }

      /// <inheritdoc/>
      public Int32 Count => _set.Count;

      /// <inheritdoc/>
      public IEqualityComparer<T> EqualityComparer => _set.KeyComparer;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues => true;

      /// <inheritdoc/>
      public Boolean IsEmpty => _set.Count == 0;

      /// <inheritdoc/>
      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => true;

      /// <inheritdoc/>
      public void MakeReadOnly()
      {
         if (_supportsReadOnlyHelper.IsReadOnly)
         {
            return;
         }

         _supportsReadOnlyHelper.MakeReadOnly();
         RemoveExistingDuplicates();
      }

      /// <inheritdoc/>
      public Boolean Add(T item)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         if (item.IsNull())
         {
            return false;
         }

         var rv = false;
         using (_lock.EnterWriteLock())
         {
            var original = _set;
            var modified = _set.Add(item);
            if (!ReferenceEquals(original, modified))
            {
               rv = true;
               _set = modified;
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
      public void Clear()
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         using (_lock.EnterWriteLock())
         {
            _set = _set.Clear();
         }
      }

      /// <inheritdoc/>
      public Boolean Contains(T item)
      {
         if (item.IsNull())
         {
            return false;
         }

         return _set.Contains(item);
      }

      /// <inheritdoc/>
      void ICollection<T>.CopyTo(T[] array, Int32 arrayIndex)
      {
         using (_lock.EnterReadLock())
         {
            ((ICollection<T>)_set).CopyTo(array, arrayIndex);
         }
      }

      /// <inheritdoc/>
      public void ExceptWith(IEnumerable<T> other)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);

         using (_lock.EnterWriteLock())
         {
            var builder = _set.ToBuilder();
            builder.ExceptWith(cleaned);
            _set = builder.ToImmutable();
         }
      }

      /// <inheritdoc/>
      public IEnumerator<T> GetEnumerator()
      {
         return _set.GetEnumerator();
      }

      /// <inheritdoc/>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <inheritdoc/>
      public void IntersectWith(IEnumerable<T> other)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);

         using (_lock.EnterWriteLock())
         {
            var builder = _set.ToBuilder();
            builder.IntersectWith(cleaned);
            _set = builder.ToImmutable();
         }
      }

      /// <inheritdoc/>
      public Boolean IsProperSubsetOf(IEnumerable<T> other)
      {
         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);
         return _set.IsProperSubsetOf(cleaned);
      }

      /// <inheritdoc/>
      public Boolean IsProperSupersetOf(IEnumerable<T> other)
      {
         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);
         return _set.IsProperSupersetOf(cleaned);
      }

      /// <inheritdoc/>
      public Boolean IsSubsetOf(IEnumerable<T> other)
      {
         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);
         return _set.IsSubsetOf(cleaned);
      }

      /// <inheritdoc/>
      public Boolean IsSupersetOf(IEnumerable<T> other)
      {
         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);
         return _set.IsSupersetOf(cleaned);
      }

      /// <inheritdoc/>
      public Boolean Overlaps(IEnumerable<T> other)
      {
         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);
         return _set.Overlaps(cleaned);
      }

      /// <inheritdoc/>
      public Boolean Remove(T item)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         if (item.IsNull())
         {
            return false;
         }

         var rv = false;
         using (_lock.EnterWriteLock())
         {
            var original = _set;
            var modified = _set.Remove(item);
            if (!ReferenceEquals(original, modified))
            {
               rv = true;
               _set = modified;
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      public Boolean SetEquals(IEnumerable<T> other)
      {
         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);
         return _set.SetEquals(cleaned);
      }

      /// <inheritdoc/>
      public void SymmetricExceptWith(IEnumerable<T> other)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);
         using (_lock.EnterWriteLock())
         {
            var builder = _set.ToBuilder();
            builder.SymmetricExceptWith(cleaned);
            _set = builder.ToImmutable();
         }
      }

      /// <inheritdoc/>
      public void UnionWith(IEnumerable<T> other)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);

         using (_lock.EnterWriteLock())
         {
            var builder = _set.ToBuilder();
            builder.UnionWith(cleaned);
            _set = builder.ToImmutable();
         }
      }

      /// <inheritdoc/>
      public void AddRange(IEnumerable<T> source)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         UnionWith(source);
      }

      /// <inheritdoc/>
      public Boolean CheckForExistingDuplicates()
      {
         if (typeof(T).IsValueType)
         {
            return false;
         }

         Boolean rv;

         using (_lock.EnterReadLock())
         {
            var deepCheck = false;
            var hashValues = new HashSet<Int32>();
            foreach (var item in _set)
            {
               // some comparer GetHashCode implementations throw on null, avoid the exceptions
               var hashValue = ReferenceEquals(item, null) ? 0 : _set.KeyComparer.GetHashCode(item);
               var isUnique = hashValues.Add(hashValue);
               if (!isUnique)
               {
                  deepCheck = true;
                  break;
               }
            }

            rv = deepCheck;
            if (deepCheck)
            {
               // the were duplicate hash values so perform deep check using full equality evaluation.
               var mySet = new SimpleSet<T>(_set.KeyComparer);
               mySet.AddRange(_set);
               rv = mySet.Count != _set.Count;
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      public Boolean RemoveExistingDuplicates()
      {
         // do not enforce read-only (this is called from MakeReadOnly).
         // do not call other methods that enforce read-only from this implementation.
         var rv = CheckForExistingDuplicates();
         if (rv)
         {
            using (_lock.EnterWriteLock())
            {
               var currentValues = _set.ToList();
               _set = _set.Clear();
               _set = _set.Union(currentValues);
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var hashcode = (Int32)_set.LongCount();
            return hashcode;
         }
      }

      private List<T> CleanNullsFromNonNullEnumerableOfT(IEnumerable<T> other)
      {
         other.ArgumentNotNull("other");

         List<T> rv;
         if (typeof(T).IsValueType)
         {
            rv = other.ToList();
         }
         else
         {
            rv = (from item in other where item.IsNotNull() select item).ToList();
         }

         return rv;
      }
   }
}
