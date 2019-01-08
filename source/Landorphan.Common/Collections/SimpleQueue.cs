namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;

   /// <summary>
   /// Represents a variable size first-in-first-out (FIFO) collection of instances of the same arbitrary type.
   /// </summary>
   /// <typeparam name="T">
   /// The type of the elements.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Naming", "CA1711: Identifiers should not have incorrect suffix")]
   [DebuggerDisplay("IsEmpty = {IsEmpty}")]
   public sealed class SimpleQueue<T> : IQueue<T>
   {
      private readonly IEqualityComparer<T> _equalityComparer;
      private readonly Queue<T> _queue;

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleQueue{T}"/> class.
      /// </summary>
      public SimpleQueue() : this(null, true)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleQueue{T}"/> class.
      /// </summary>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{T}"/> implementation for the type.
      /// </param>
      /// <param name="ignoreNullItems">
      /// When set to <c> true </c>, <c> null </c> values are ignored.
      /// </param>
      public SimpleQueue(IEqualityComparer<T> equalityComparer, Boolean ignoreNullItems)
      {
         IgnoresNullValues = ignoreNullItems;
         _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
         _queue = new Queue<T>();
      }

      /// <inheritdoc/>
      public IEqualityComparer<T> EqualityComparer => _equalityComparer;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues { get; }

      /// <inheritdoc/>
      public Boolean IsEmpty => _queue.Count == 0;

      /// <inheritdoc/>
      public Boolean IsReadOnly => false;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => IsReadOnly;

      /// <inheritdoc/>
      Int32 ICountEnumerable.Count => _queue.Count;

      /// <inheritdoc/>
      public void Clear()
      {
         _queue.Clear();
      }

      /// <inheritdoc/>
      Boolean IContainsEnumerable<T>.Contains(T item)
      {
         return _queue.Any(contained => _equalityComparer.Equals(contained, item));
      }

      /// <inheritdoc/>
      public T Dequeue()
      {
         return _queue.Dequeue();
      }

      /// <inheritdoc/>
      public void Enqueue(T item)
      {
         if (IgnoresNullValues && ReferenceEquals(null, item))
         {
            return;
         }

         _queue.Enqueue(item);
      }

      /// <inheritdoc/>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return _queue.GetEnumerator();
      }

      /// <inheritdoc/>
      IEnumerator<T> IEnumerable<T>.GetEnumerator()
      {
         return _queue.GetEnumerator();
      }

      /// <inheritdoc/>
      public T Peek()
      {
         return _queue.Peek();
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var hashcode = (Int32) _queue.LongCount();
            return hashcode;
         }
      }
   }
}