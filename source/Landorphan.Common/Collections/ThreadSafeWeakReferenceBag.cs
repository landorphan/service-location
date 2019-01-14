namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using Landorphan.Common.Threading;

   /// <summary>
   /// Represents a thread-safe collection of <see cref="WeakReference"/> instances that reference the same arbitrary type.
   /// </summary>
   /// <remarks>
   /// Both <c> null </c> and duplicate references are ignored.
   /// </remarks>
   /// <typeparam name="T">
   /// The type of the elements referenced.
   /// </typeparam>
   /// <threadsafety static="true" instance="true"/>
   /// <seealso cref="DisposableObject"/>
   /// <seealso cref="IWeakReferenceBag{T}"/>
   /// <remarks> Duplicate references are not supported, attempts to add a duplicate reference return <c> false </c>. </remarks>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [DebuggerDisplay("Count = {Count}")]
   public sealed class ThreadSafeWeakReferenceBag<T> : IWeakReferenceBag<T> where T : class
   {
      // observed 2014-04-11: there seems to be side-effects caused by the interactions of System.Collections.Immutable instances
      // and WeakReference.  Do not use these together until this is solved/understood.
      private readonly IEqualityComparer<T> _equalityComparer;
      private readonly INonRecursiveLock _lock;
      private readonly List<WeakReference> _lst;

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeWeakReferenceBag{T}"/> class.
      /// </summary>
      public ThreadSafeWeakReferenceBag() : this(null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ThreadSafeWeakReferenceBag{T}"/> class.
      /// </summary>
      /// <param name="lockImplementation">
      /// The lock implementation, or null for the default implementation.
      /// </param>
      public ThreadSafeWeakReferenceBag(INonRecursiveLock lockImplementation)
      {
         _lock = lockImplementation ?? new NonRecursiveLock();
         _lst = new List<WeakReference>();
         _equalityComparer = new ReferenceEqualityComparer<T>();
      }

      /// <inheritdoc/>
      public Int32 Count
      {
         get
         {
            var strongReferences = CopyStrongReferences();
            return strongReferences.Count;
         }
      }

      /// <inheritdoc/>
      public IEqualityComparer<T> EqualityComparer => _equalityComparer;

      /// <inheritdoc/>
      public Boolean IgnoresNullValues => true;

      /// <inheritdoc/>
      public Boolean IsEmpty => Count == 0;

      /// <inheritdoc/>
      public Boolean IsReadOnly => false;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => true;

      /// <inheritdoc/>
      public Boolean Add(T item)
      {
         if (ReferenceEquals(item, null))
         {
            return false;
         }

         var index = IndexOf(item);
         if (-1 == index)
         {
            using (_lock.EnterWriteLock())
            {
               _lst.Add(new WeakReference(item));
            }

            return true;
         }

         return false;
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

      /// <inheritdoc/>
      public void Clear()
      {
         using (_lock.EnterWriteLock())
         {
            _lst.Clear();
         }
      }

      /// <inheritdoc/>
      public Boolean Contains(T item)
      {
         var strongReferences = CopyStrongReferences();
         return strongReferences.Any(strongReference => _equalityComparer.Equals(strongReference, item));
      }

      /// <inheritdoc/>
      public IEnumerator<T> GetEnumerator()
      {
         // create a thread-safe enumerator.
         var strongReferences = CopyStrongReferences();
         return strongReferences.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <inheritdoc/>
      public Boolean Remove(T item)
      {
         var result = false;
         var index = IndexOf(item);
         if (-1 != index)
         {
            using (_lock.EnterWriteLock())
            {
               _lst.RemoveAt(index);
            }

            result = true;
         }

         return result;
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var hashcode = (Int32)_lst.LongCount();
            return hashcode;
         }
      }

      private void Cleanup()
      {
         var toBeRemoved = new List<WeakReference>();
         using (_lock.EnterWriteLock())
         {
            toBeRemoved.AddRange(_lst.Where(weak => !weak.IsAlive));

            foreach (var weak in toBeRemoved)
            {
               _lst.Remove(weak);
            }
         }
      }

      private ISet2<T> CopyStrongReferences()
      {
         var cleanup = false;
         var rv = new SimpleSet<T>();
         using (_lock.EnterReadLock())
         {
            foreach (var weak in _lst)
            {
               if (weak.IsAlive)
               {
                  var item = (T)weak.Target;
                  if (null != item)
                  {
                     rv.Add(item);
                  }
               }
               else
               {
                  cleanup = true;
               }
            }
         }

         if (cleanup)
         {
            Cleanup();
         }

         return rv;
      }

      private Int32 IndexOf(T item)
      {
         var cleanup = false;
         WeakReference found = null;
         using (_lock.EnterReadLock())
         {
            foreach (var weak in _lst)
            {
               if (weak.IsAlive)
               {
                  var target = (T)weak.Target;
                  if (null != target)
                  {
                     if (Equals(target, item))
                     {
                        found = weak;
                     }
                  }
               }
               else
               {
                  cleanup = true;
               }
            }
         }

         if (cleanup)
         {
            Cleanup();
         }

         if (null != found)
         {
            using (_lock.EnterWriteLock())
            {
               return _lst.IndexOf(found);
            }
         }

         return -1;
      }
   }
}
