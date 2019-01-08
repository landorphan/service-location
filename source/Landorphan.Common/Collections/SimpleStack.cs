namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;

   /// <summary>
   /// Represents a variable-sized last-in-first-out (LIFO) collection of instances of the same arbitrary type.
   /// </summary>
   /// <typeparam name="T">
   /// The type of the elements.
   /// </typeparam>
   /// <threadsafety static="true" instance="false"/>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Naming", "CA1711: Identifiers should not have incorrect suffix")]
   [DebuggerDisplay("Count = {Count}")]
   public sealed class SimpleStack<T> : IStack<T>
   {
      private readonly IEqualityComparer<T> _equalityComparer;
      private readonly Stack<T> _stack;

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleStack{T}"/> class.
      /// </summary>
      public SimpleStack() : this(null, true)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SimpleStack{T}"/> class.
      /// </summary>
      /// <param name="equalityComparer">
      /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values, or <c> null </c> to use the
      /// default <see cref="IEqualityComparer{T}"/>  implementation for the type.
      /// </param>
      /// <param name="ignoreNullItems">
      /// When set to <c> true </c>, <c> null </c> values are ignored.
      /// </param>
      public SimpleStack(IEqualityComparer<T> equalityComparer, Boolean ignoreNullItems)
      {
         IgnoresNullValues = ignoreNullItems;
         _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
         _stack = new Stack<T>();
      }

      /// <inheritdoc/>
      public Int32 Count => _stack.Count;

      /// <inheritdoc/>
      public IEqualityComparer<T> EqualityComparer => _equalityComparer;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues { get; }

      /// <inheritdoc/>
      public Boolean IsEmpty => _stack.Count == 0;

      /// <inheritdoc/>
      public Boolean IsReadOnly => false;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => IsReadOnly;

      /// <inheritdoc/>
      public void Clear()
      {
         _stack.Clear();
      }

      /// <inheritdoc/>
      Boolean IContainsEnumerable<T>.Contains(T item)
      {
         return _stack.Any(contained => _equalityComparer.Equals(contained, item));
      }

      /// <inheritdoc/>
      IEnumerator<T> IEnumerable<T>.GetEnumerator()
      {
         return _stack.GetEnumerator();
      }

      /// <inheritdoc/>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return _stack.GetEnumerator();
      }

      /// <inheritdoc/>
      public T Peek()
      {
         return _stack.Peek();
      }

      /// <inheritdoc/>
      public T Pop()
      {
         return _stack.Pop();
      }

      /// <inheritdoc/>
      public void Push(T item)
      {
         if (IgnoresNullValues && ReferenceEquals(null, item))
         {
            return;
         }

         _stack.Push(item);
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