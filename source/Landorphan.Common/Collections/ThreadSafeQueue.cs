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
   /// Represents a variable size first-in-first-out (FIFO) collection of instances of the same arbitrary type.
   /// </summary>
   /// <typeparam name="T">
   /// The type of the elements.
   /// </typeparam>
   /// <threadsafety static="true" instance="true"/>
   /// <seealso cref="DisposableObject"/>
   /// <seealso cref="IQueue{T}"/>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Naming", "CA1711: Identifiers should not have incorrect suffix")]
   [DebuggerDisplay("IsEmpty = {IsEmpty}")]
   public sealed class ThreadSafeQueue<T> : IQueue<T>
   {
      private readonly IEqualityComparer<T> _equalityComparer;
      private readonly INonRecursiveLock _lock;
      private ImmutableQueue<T> _queue;

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeQueue{T}"/> class.
      /// </summary>
      public ThreadSafeQueue() : this(null, true, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeQueue{T}"/> class.
      /// </summary>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or null to use the default
      /// <see cref="IEqualityComparer{T}"/> implementation for the type.
      /// </param>
      /// <param name="ignoreNullItems">
      /// When set to <c> true </c>, <c> null </c> values are ignored.
      /// </param>
      /// <param name="lockImplementation">
      /// The lock implementation, or null for the default implementation.
      /// </param>
      public ThreadSafeQueue(IEqualityComparer<T> equalityComparer, Boolean ignoreNullItems, INonRecursiveLock lockImplementation)
      {
         _lock = lockImplementation ?? new NonRecursiveLock();
         IgnoresNullValues = ignoreNullItems;
         _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
         _queue = ImmutableQueue<T>.Empty;
      }

      /// <inheritdoc/>
      public IEqualityComparer<T> EqualityComparer => _equalityComparer;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues { get; }

      /// <inheritdoc/>
      public Boolean IsEmpty => _queue.IsEmpty;

      /// <inheritdoc/>
      public Boolean IsReadOnly => false;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => true;

      /// <inheritdoc/>
      Int32 ICountEnumerable.Count => _queue.Count();

      /// <inheritdoc/>
      public void Clear()
      {
         using (_lock.EnterWriteLock())
         {
            _queue = _queue.Clear();
         }
      }

      /// <inheritdoc/>
      Boolean IContainsEnumerable<T>.Contains(T item)
      {
         return _queue.Any(contained => _equalityComparer.Equals(contained, item));
      }

      /// <inheritdoc/>
      public T Dequeue()
      {
         T item;
         _queue = _queue.Dequeue(out item);
         return item;
      }

      /// <inheritdoc/>
      public void Enqueue(T item)
      {
         if (IgnoresNullValues && ReferenceEquals(null, item))
         {
            return;
         }

         _queue = _queue.Enqueue(item);
      }

      /// <inheritdoc/>
      public IEnumerator<T> GetEnumerator()
      {
         return ((IEnumerable<T>) _queue).GetEnumerator();
      }

      /// <inheritdoc/>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
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