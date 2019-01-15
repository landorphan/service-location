namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Collections.ObjectModel;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;

   /// <summary>
   /// An unordered collection of values.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the collection.
   /// </typeparam>
   /// <threadsafety static="true" instance="false"/>
   // ReSharper disable once UseNameofExpression
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [DebuggerDisplay("Count = {Count}")]
   public sealed class SimpleBag<T> : IBag<T>, ICollection<T>, IConvertsToReadOnly
   {
      private readonly Collection<T> _collection;
      private readonly IEqualityComparer<T> _equalityComparer;
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper;

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleBag{T}"/> class.
      /// </summary>
      public SimpleBag() : this(Array.Empty<T>())
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleBag{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new bag.
      /// </param>
      public SimpleBag(IEnumerable<T> collection) : this(collection, null, true)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleBag{T}"/> class.
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
      public SimpleBag(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer, Boolean ignoreNullValues)
      {
         IgnoresNullValues = ignoreNullValues;
         _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
         _supportsReadOnlyHelper = new SupportsReadOnlyHelper();
         _collection = new Collection<T>();
         if (collection.IsNotNull())
         {
            AddRange(collection);
         }
      }

      /// <inheritdoc cref="ICollection{T}.Count"/>
      public Int32 Count => _collection.Count;

      /// <inheritdoc/>
      public IEqualityComparer<T> EqualityComparer => _equalityComparer;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues { get; }

      /// <inheritdoc/>
      public Boolean IsEmpty => _collection.Count == 0;

      /// <inheritdoc cref="IQueryReadOnly.IsReadOnly" />
      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public Boolean Add(T item)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         if (IgnoresNullValues && item.IsNull())
         {
            return false;
         }

         _collection.Add(item);
         return true;
      }

      /// <inheritdoc/>
      public void AddRange(IEnumerable<T> source)
      {
         source.ArgumentNotNull("source");

         foreach (var item in source)
         {
            Add(item);
         }
      }

      /// <inheritdoc cref="ICollection{T}.Clear"/>
      public void Clear()
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         _collection.Clear();
      }

      /// <inheritdoc cref="IContainsEnumerable{T}.Contains"/>
      public Boolean Contains(T item)
      {
         return _collection.Any(contained => _equalityComparer.Equals(contained, item));
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

      /// <inheritdoc cref="IRemoveEnumerable{T}.Remove"/>.
      public Boolean Remove(T item)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         var rv = false;
         for (var i = 0; i < _collection.Count; ++i)
         {
            if (_equalityComparer.Equals(item, _collection[i]))
            {
               rv = true;
               _collection.RemoveAt(i);
               break;
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
      public void MakeReadOnly()
      {
         _supportsReadOnlyHelper.MakeReadOnly();
      }
   }
}
