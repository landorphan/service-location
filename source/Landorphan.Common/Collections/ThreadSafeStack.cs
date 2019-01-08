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
   /// Represents a variable-sized last-in-first-out (LIFO) collection of instances of the same arbitrary type.
   /// </summary>
   /// <typeparam name="T">
   /// The type of the elements.
   /// </typeparam>
   /// <threadsafety static="true" instance="true"/>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Naming", "CA1711: Identifiers should not have incorrect suffix")]
   public sealed class ThreadSafeStack<T> : IStack<T>
   {
      private readonly IEqualityComparer<T> _equalityComparer;
      private readonly INonRecursiveLock _lock;
      private ImmutableStack<T> _stack;

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeStack{T}"/> class.
      /// </summary>
      public ThreadSafeStack() : this(null, true, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeStack{T}"/> class.
      /// </summary>
      /// <param name="equalityComparer"> The equality comparer. </param>
      /// <param name="ignoreNullItems"> if set to <c> true </c> [ignore null items]. </param>
      /// <param name="lockImplementation"> The lock implementation. </param>
      public ThreadSafeStack(IEqualityComparer<T> equalityComparer, Boolean ignoreNullItems, INonRecursiveLock lockImplementation)
      {
         _lock = lockImplementation ?? new NonRecursiveLock();
         IgnoresNullValues = ignoreNullItems;
         _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
         _stack = ImmutableStack<T>.Empty;
      }

      /// <inheritdoc/>
      public IEqualityComparer<T> EqualityComparer => _equalityComparer;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues { get; }

      /// <inheritdoc/>
      public Boolean IsEmpty => _stack.IsEmpty;

      /// <inheritdoc/>
      public Boolean IsReadOnly => false;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => true;

      /// <inheritdoc/>
      Int32 ICountEnumerable.Count => _stack.Count();

      /// <inheritdoc/>
      public void Clear()
      {
         using (_lock.EnterWriteLock())
         {
            _stack = _stack.Clear();
         }
      }

      /// <inheritdoc/>
      Boolean IContainsEnumerable<T>.Contains(T item)
      {
         return _stack.Any(contained => _equalityComparer.Equals(contained, item));
      }

      /// <inheritdoc/>
      public IEnumerator<T> GetEnumerator()
      {
         return ((IEnumerable<T>) _stack).GetEnumerator();
      }

      /// <inheritdoc/>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <inheritdoc/>
      public T Peek()
      {
         return _stack.Peek();
      }

      /// <inheritdoc/>
      public T Pop()
      {
         T item;
         _stack = _stack.Pop(out item);
         return item;
      }

      /// <inheritdoc/>
      public void Push(T item)
      {
         if (IgnoresNullValues && ReferenceEquals(null, item))
         {
            return;
         }

         _stack = _stack.Push(item);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var hashcode = (Int32) _stack.LongCount();
            return hashcode;
         }
      }
   }
}