namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using Landorphan.Common.Resources;

   /// <summary>
   /// Represents a queue collection of instances of the same arbitrary type for which items are retrieved in priority order.
   /// </summary>
   /// <typeparam name="T">
   /// Specifies the type of elements in the queue.
   /// </typeparam>
   /// <threadsafety static="true" instance="false"/>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Naming", "CA1711: Identifiers should not have incorrect suffix")]
   public sealed class InlinePriorityQueue<T> : IInlinePriorityQueue<T>, ICollection<T>
   {
      // TODO: consider re-implementing with IComparer<T>.
      private readonly Comparison<T> _comparison;
      private readonly List<T> _heap;

      /// <summary>
      /// Initializes a new instance of the <see cref="InlinePriorityQueue{T}"/> class.
      /// </summary>
      public InlinePriorityQueue() : this(null, null, true)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InlinePriorityQueue{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new queue.
      /// </param>
      public InlinePriorityQueue(IEnumerable<T> collection) : this(collection, null, true)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InlinePriorityQueue{T}"/> class.
      /// </summary>
      /// <param name="comparison">
      /// The comparison to use to prioritize the queue, or null to use the default
      /// <see cref="Comparer{T}.Compare"/>
      /// implementation.
      /// </param>
      public InlinePriorityQueue(Comparison<T> comparison) : this(Array.Empty<T>(), comparison, true)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InlinePriorityQueue{T}"/> class.
      /// </summary>
      /// <param name="collection">
      /// The collection whose elements are copied to the new queue.
      /// </param>
      /// <param name="comparison">
      /// The comparison to use to prioritize the queue, or null to use the default
      /// <see cref="Comparer{T}.Compare"/>
      /// implementation.
      /// </param>
      /// <param name="ignoreNullItems">
      /// When set to <c> true </c> null values are ignored.
      /// </param>
      public InlinePriorityQueue(IEnumerable<T> collection, Comparison<T> comparison, Boolean ignoreNullItems)
      {
         IgnoresNullValues = ignoreNullItems;
         _comparison = comparison ?? Comparer<T>.Default.Compare;

         _heap = new List<T>();
         if (collection.IsNotNull())
         {
            AddRange(collection);
            RescheduleAll();
         }
      }

      /// <inheritdoc cref="ICollection{T}.Count" />
      public Int32 Count => _heap.Count;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues { get; }

      /// <inheritdoc/>
      public Boolean IsEmpty => _heap.Count == 0;

      /// <inheritdoc cref="IQueryReadOnly.IsReadOnly" />
      public Boolean IsReadOnly => false;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => IsReadOnly;

      /// <inheritdoc/>
      public Comparison<T> PriorityComparison => _comparison;

      /// <inheritdoc/>
      IEqualityComparer<T> IReadableBagEnumerable<T>.EqualityComparer => null;

      /// <inheritdoc/>
      void ICollection<T>.Add(T item)
      {
         Enqueue(item);
      }

      /// <inheritdoc/>
      Boolean ICollection<T>.Contains(T item)
      {
         return _heap.Contains(item);
      }

      /// <inheritdoc/>
      void ICollection<T>.CopyTo(T[] array, Int32 arrayIndex)
      {
         _heap.CopyTo(array, arrayIndex);
      }

      /// <inheritdoc/>
      Boolean ICollection<T>.Remove(T item)
      {
         throw new NotSupportedException();
      }

      /// <inheritdoc/>
      public void AddRange(IEnumerable<T> source)
      {
         source.ArgumentNotNull("source");

         foreach (var item in source)
         {
            Enqueue(item);
         }
      }

      /// <inheritdoc cref="ICollection{T}.Clear" />
      public void Clear()
      {
         _heap.Clear();
      }

      /// <inheritdoc/>
      Boolean IContainsEnumerable<T>.Contains(T item)
      {
         return _heap.Any(contained => _comparison(item, contained) == 0);
      }

      /// <inheritdoc/>
      public T Dequeue()
      {
         if (_heap.Count == 0)
         {
            throw new InvalidOperationException(StringResources.EmptyPriorityQueue);
         }

         var p = 0;
         var result = _heap[0];
         _heap[0] = _heap[_heap.Count - 1];
         _heap.RemoveAt(_heap.Count - 1);
         while (true)
         {
            var pn = p;
            var p1 = 2 * p + 1;
            var p2 = 2 * p + 2;

            if (_heap.Count > p1 && Compare(p, p1) > 0)
            {
               p = p1;
            }

            if (_heap.Count > p2 && Compare(p, p2) > 0)
            {
               p = p2;
            }

            if (p == pn)
            {
               break;
            }

            SwapByIndex(p, pn);
         }

         return result;
      }

      /// <inheritdoc/>
      public void Enqueue(T item)
      {
         if (item.IsNull() && IgnoresNullValues)
         {
            return;
         }

         var p = _heap.Count;
         _heap.Add(item);
         while (true)
         {
            if (p == 0)
            {
               break;
            }

            var p2 = (p - 1) / 2;
            if (Compare(p, p2) < 0)
            {
               SwapByIndex(p, p2);
               p = p2;
            }
            else
            {
               break;
            }
         }
      }

      /// <inheritdoc/>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return _heap.GetEnumerator();
      }

      /// <inheritdoc/>
      IEnumerator<T> IEnumerable<T>.GetEnumerator()
      {
         return _heap.GetEnumerator();
      }

      /// <inheritdoc/>
      public T Peek()
      {
         if (_heap.Count == 0)
         {
            throw new InvalidOperationException(StringResources.EmptyPriorityQueue);
         }

         return _heap[0];
      }

      /// <inheritdoc/>
      public void Reschedule(T item)
      {
         RescheduleImpl(_heap.IndexOf(item));
      }

      /// <inheritdoc/>
      public void RescheduleAll()
      {
         _heap.Sort(_comparison);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var hashcode = (Int32)_heap.LongCount();
            return hashcode;
         }
      }

      private Int32 Compare(Int32 i, Int32 j)
      {
         return _comparison(_heap[i], _heap[j]);
      }

      private void RescheduleImpl(Int32 index)
      {
         var p = index;
         Int32 p2;
         while (true)
         {
            if (p == 0)
            {
               break;
            }

            p2 = (p - 1) / 2;
            if (Compare(p, p2) < 0)
            {
               SwapByIndex(p, p2);
               p = p2;
            }
            else
            {
               break;
            }
         }

         if (p < index)
         {
            return;
         }

         while (true)
         {
            var pn = p;
            var p1 = 2 * p + 1;
            p2 = 2 * p + 2;

            if (_heap.Count > p1 && Compare(p, p1) > 0)
            {
               p = p1;
            }

            if (_heap.Count > p2 && Compare(p, p2) > 0)
            {
               p = p2;
            }

            if (p == pn)
            {
               break;
            }

            SwapByIndex(p, pn);
         }
      }

      private void SwapByIndex(Int32 i, Int32 j)
      {
         var temp = _heap[i];
         _heap[i] = _heap[j];
         _heap[j] = temp;
      }
   }
}
