namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;

   /// <summary>
   /// Represents a set of instances of the same arbitrary type.
   /// </summary>
   /// <typeparam name="T">
   /// The type of the elements.
   /// </typeparam>
   /// <remarks>
   /// Silently ignores <c> null </c> values.  Exceptions are not thrown, but null values cannot be added as elements.  A null collection is treated
   /// as an empty set.
   /// </remarks>
   /// <threadsafety static="true" instance="false"/>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public sealed class SimpleSet<T> : ISet<T>, ISet2<T>, IConvertsToReadOnly
   {
      private readonly HashSet<T> _set;
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper;

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleSet{T}"/> class.
      /// </summary>
      public SimpleSet() : this(null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleSet{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new list.
      /// </param>
      public SimpleSet(IEnumerable<T> collection) : this(collection, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleSet{T}"/> class.
      /// </summary>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{T}"/> implementation for the type.
      /// </param>
      public SimpleSet(IEqualityComparer<T> equalityComparer) : this(null, equalityComparer)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleSet{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new collection.
      /// </param>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{T}"/> implementation for the type.
      /// </param>
      public SimpleSet(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer)
      {
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

         _set = new HashSet<T>(workingCollection, equalityComparer);
      }

      /// <inheritdoc/>
      public Int32 Count => _set.Count;

      /// <inheritdoc/>
      public IEqualityComparer<T> EqualityComparer => _set.Comparer;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues => true;

      /// <inheritdoc/>
      public Boolean IsEmpty => _set.Count == 0;

      /// <inheritdoc/>
      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => IsReadOnly;

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

         return _set.Add(item);
      }

      /// <summary>
      /// Adds an item to the <see cref="IEnumerable{T}"/> .
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      /// <param name="item">
      /// The Object to add to the <see cref="IEnumerable{T}"/> .
      /// </param>
      void ICollection<T>.Add(T item)
      {
         Add(item);
      }

      /// <inheritdoc/>
      public void Clear()
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         _set.Clear();
      }

      /// <inheritdoc/>
      public Boolean Contains(T item)
      {
         return _set.Contains(item);
      }

      /// <inheritdoc/>
      void ICollection<T>.CopyTo(T[] array, Int32 arrayIndex)
      {
         _set.CopyTo(array, arrayIndex);
      }

      /// <inheritdoc/>
      public void ExceptWith(IEnumerable<T> other)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);
         _set.ExceptWith(cleaned);
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
         _set.IntersectWith(cleaned);
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

         return _set.Remove(item);
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
         _set.SymmetricExceptWith(cleaned);
      }

      /// <inheritdoc/>
      public void UnionWith(IEnumerable<T> other)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         var cleaned = ReferenceEquals(other, null) ? new List<T>() : CleanNullsFromNonNullEnumerableOfT(other);
         _set.UnionWith(cleaned);
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

         var deepCheck = false;
         var hashValues = new SimpleSet<Int32>();
         foreach (var item in _set)
         {
            // some comparer GetHashCode implementations throw on null, avoid the exceptions
            var hashValue = ReferenceEquals(item, null) ? 0 : _set.Comparer.GetHashCode(item);
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
            var mySet = new SimpleSet<T>(_set.Comparer);
            mySet.AddRange(_set);
            rv = mySet.Count != _set.Count;
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
            var currentValues = _set.ToList();
            _set.Clear();
            foreach (var v in currentValues)
            {
               _set.Add(v);
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var hashcode = (Int32) _set.LongCount();
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