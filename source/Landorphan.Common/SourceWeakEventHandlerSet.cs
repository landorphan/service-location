namespace Landorphan.Common
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.Linq;
   using System.Reflection;
   using System.Threading;
   using System.Threading.Tasks;

   /// <summary>
   /// A collection event listeners for an Object that sources events.
   /// </summary>
   /// <remarks>
   /// Used by event sources to hold weak references to event handlers.  Duplicate subscriptions are prevented, but do not generate
   /// exceptions.
   /// </remarks>
   /// <typeparam name="TEventArgs">
   /// Type of the event arguments.
   /// </typeparam>
   /// <threadsafety static="true" instance="true"/>
   /// <example>
   /// Example of an event provider implementation:
   /// <code>
   /// class EventSource
   /// {
   ///      private readonly SourceWeakEventHandlerSet&lt;EventArgs&gt; _listeners = new SourceWeakEventHandlerSet&lt;EventArgs&gt;();
   /// 
   ///      public event EventHandler&lt;EventArgs&gt; MyEvent
   ///      {
   ///         add { _listeners.Add(value); }
   ///         remove { _listeners.Remove(value); }
   ///      }
   /// 
   ///      private void OnMyEvent(EventArgs e)
   ///      {
   ///         _listeners.Invoke(this, e);
   ///      }
   /// }
   /// </code>
   /// </example>
   public sealed class SourceWeakEventHandlerSet<TEventArgs> where TEventArgs : EventArgs
   {
      private readonly Object _lockObject = new Object();

      // ImmutableHashSet throws on null elements.
      private ImmutableHashSet<EventItem> _listeners = ImmutableHashSet<EventItem>.Empty;

      /// <summary>
      /// Adds an event handler to collection of listeners.
      /// </summary>
      /// <param name="handler">
      /// The handler to add.
      /// </param>
      /// <returns>
      /// <c> true </c> if the handler is added; otherwise <c> false </c> (value is <c> null </c> or already subscribed).
      /// </returns>
      public Boolean Add(EventHandler<TEventArgs> handler)
      {
         if (handler == null)
         {
            return false;
         }

         Cleanup();
         // ReSharper disable once InconsistentlySynchronizedField
         var original = _listeners;
         lock (_lockObject)
         {
            var item = new EventItem(handler);
            _listeners = _listeners.Add(item);
         }

         // ReSharper disable once InconsistentlySynchronizedField
         return !ReferenceEquals(original, _listeners);
      }

      /// <summary>
      /// Clears all dead weak reference listeners from this instance.
      /// </summary>
      public void Cleanup()
      {
         var toBeRemoved = new List<EventItem>();

         // ReSharper disable once InconsistentlySynchronizedField
         var currentCollection = _listeners;
         foreach (var item in currentCollection)
         {
            if (item.NeedsCleanup())
            {
               toBeRemoved.Add(item);
            }
         }

         if (toBeRemoved.Any())
         {
            lock (_lockObject)
            {
               var builder = _listeners.ToBuilder();
               foreach (var item in toBeRemoved)
               {
                  builder.Remove(item);
               }

               _listeners = builder.ToImmutable();
            }
         }
      }

      /// <summary>
      /// Clears all event handlers from the collection of listeners.
      /// </summary>
      public void Clear()
      {
         lock (_lockObject)
         {
            _listeners = _listeners.Clear();
         }
      }

      /// <summary>
      /// Removes an event handler from collection of listeners.
      /// </summary>
      /// <param name="handler">
      /// The handler to remove.
      /// </param>
      /// <returns>
      /// <c> true </c> if the handler is removed; otherwise <c> false </c> (value is <c> null </c> or not subscribed).
      /// </returns>
      public Boolean Remove(EventHandler<TEventArgs> handler)
      {
         if (handler == null)
         {
            return false;
         }

         Cleanup();

         // ReSharper disable once InconsistentlySynchronizedField
         var original = _listeners;
         lock (_lockObject)
         {
            _listeners = _listeners.Remove(new EventItem(handler));
         }

         // ReSharper disable once InconsistentlySynchronizedField
         return !ReferenceEquals(original, _listeners);
      }

      /// <summary>
      /// Notifies all subscribers on a different thread.
      /// </summary>
      /// <param name="sender">
      /// Source of the event.
      /// </param>
      /// <param name="e">
      /// The event information.
      /// </param>
      public void Invoke(Object sender, TEventArgs e)
      {
         // ReSharper disable once InconsistentlySynchronizedField
         var currentCollection = _listeners;

         // NOTES:
         //    Cannot use Task.Run(() => Parallel.ForEach(currentCollection, item => item.Invoke(sender, e)));
         //       because sender and e are copied, which breaks reference identity which is critically important in events
         //
         //    Can use Parallel.ForEach(currentCollection, item => item.Invoke(sender, e));
         //       but it hangs applications when called from the "UI Thread
         var currentThread = Thread.CurrentThread;

         if (!(currentThread.GetApartmentState() == ApartmentState.STA && currentThread.ManagedThreadId == 1 && !currentThread.IsBackground && !currentThread.IsThreadPoolThread))
         {
            // not on a WPF or WinForm UI thread, assume blocking call "safe"
            Parallel.ForEach(currentCollection, item => item.Invoke(sender, e));
         }
         else
         {
            // on a WPF wor WinForm UI thread:  a blocking call is not safe.
            foreach (var item in currentCollection)
            {
               item.Invoke(sender, e);
            }
         }
      }

      /// <summary>
      /// An wrapper around an event handler.
      /// </summary>
      /// <remarks>
      /// The wrapper is responsible for deciding between a strong or weak reference implementation.
      /// </remarks>
      [SuppressMessage("SonarLint.CodeSmell", "S4056: Overloads with a CultureInfo or an IFormatProvider parameter should be used", Justification = "reflection (MWP)")]
      private class EventItem : IEquatable<EventItem>
      {
         private readonly EventHandler<TEventArgs> _strongEventHandler;
         private readonly IWeakEventHandler _weakEventHandler;

         /// <summary>
         /// Initializes a new instance of the <see cref="EventItem"/> class.
         /// </summary>
         /// <exception cref="InvalidOperationException">
         /// Thrown when the requested operation is invalid.
         /// </exception>
         /// <param name="handler">
         /// The handler.
         /// </param>
         public EventItem(EventHandler<TEventArgs> handler)
         {
            handler.ArgumentNotNull(nameof(handler));

            var d = (Delegate) handler;

            // will be null for static methods.
            var target = d.Target;
            if (ReferenceEquals(target, null))
            {
               // static method.  Types are not garbage collected so use a strong reference.
               _strongEventHandler = handler;
            }
            else
            {
               var wet = typeof(WeakEventHandler<>).MakeGenericType(typeof(TEventArgs), handler.Method.DeclaringType);
               var eht = typeof(EventHandler<TEventArgs>);

               var ctor = wet.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] {eht}, null);

               if (ctor == null)
               {
                  throw new InvalidOperationException(
                     String.Format(
                        CultureInfo.InvariantCulture,
                        "Could not find a constructor for type '{0}' taking one parameter of type '{1}'",
                        wet.Name,
                        eht.Name));
               }

               _weakEventHandler = (IWeakEventHandler) ctor.Invoke(new Object[] {handler});
            }
         }

         private Boolean IsWeak => _weakEventHandler != null;

         /// <inheritdoc/>
         public Boolean Equals(EventItem other)
         {
            if (ReferenceEquals(other, null))
            {
               return false;
            }

            if (IsWeak != other.IsWeak)
            {
               return false;
            }

            if (IsWeak)
            {
               return ReferenceEquals(_weakEventHandler.Target, other._weakEventHandler.Target) &&
                      ReferenceEquals(_weakEventHandler.Method, other._weakEventHandler.Method);
            }

            // neither are weak events
            return ReferenceEquals(_strongEventHandler.Target, other._strongEventHandler.Target) &&
                   ReferenceEquals(_strongEventHandler.Method, other._strongEventHandler.Method);
         }

         /// <inheritdoc/>
         public override Boolean Equals(Object obj)
         {
            return Equals(obj as EventItem);
         }

         /// <inheritdoc/>
         public override Int32 GetHashCode()
         {
            unchecked
            {
               var rv = 0;
               if (_strongEventHandler.IsNotNull())
               {
                  rv = _strongEventHandler.Method.GetHashCode();
               }
               else if (_weakEventHandler.IsNotNull())
               {
                  var target = _weakEventHandler.Target;
                  var method = _weakEventHandler.Method;
                  rv = ((target.IsNotNull() ? target.GetHashCode() : 0) * 397) ^ (method.IsNotNull() ? method.GetHashCode() : 0);
               }

               return rv;
            }
         }

         /// <summary>
         /// Determines if this instance can be removed from the collection.
         /// </summary>
         /// <returns>
         /// <c> true </c> if this instance is implemented with a weak reference and the target has been garbage collected; otherwise <c> false </c>.
         /// </returns>
         public Boolean NeedsCleanup()
         {
            return IsWeak && _weakEventHandler.Target.IsNull();
         }

         /// <summary>
         /// Executes the given operation on a different thread, and waits for the result.
         /// </summary>
         /// <param name="sender">
         /// Source of the event.
         /// </param>
         /// <param name="e">
         /// T event information.
         /// </param>
         public void Invoke(Object sender, TEventArgs e)
         {
            if (IsWeak)
            {
               _weakEventHandler.Invoke(sender, e);
            }
            else
            {
               _strongEventHandler.Invoke(sender, e);
            }
         }

         /// <summary>
         /// Equality operator.
         /// </summary>
         /// <param name="left">
         /// The left.
         /// </param>
         /// <param name="right">
         /// The right.
         /// </param>
         /// <returns>
         /// The result of the operation.
         /// </returns>
         public static Boolean operator ==(EventItem left, EventItem right)
         {
            return Equals(left, right);
         }

         /// <summary>
         /// Inequality operator.
         /// </summary>
         /// <param name="left">
         /// The left.
         /// </param>
         /// <param name="right">
         /// The right.
         /// </param>
         /// <returns>
         /// The result of the operation.
         /// </returns>
         public static Boolean operator !=(EventItem left, EventItem right)
         {
            return !Equals(left, right);
         }
      }

      /// <summary>
      /// Represents a weak event.
      /// </summary>
      /// <remarks>
      /// Used to reference a <see cref="WeakEventHandler{TTarget}"/> without referencing the type parameter TTarget.
      /// </remarks>
      private interface IWeakEventHandler
      {
         MethodInfo Method { get; }

         Object Target { get; }

         void Invoke(Object sender, TEventArgs e);
      }

      /// <summary>
      /// A weak-reference event handler.
      /// </summary>
      /// <typeparam name="TTarget">
      /// Type of the target.
      /// </typeparam>
      private class WeakEventHandler<TTarget> : IWeakEventHandler
      {
         private delegate void OpenEventHandler(TTarget target, Object sender, TEventArgs eventArgs);

         private readonly OpenEventHandler _openHandler;
         private readonly WeakReference _targetReference;

         /// <summary>
         /// Initializes a new instance of the <see cref="WeakEventHandler{TTarget}"/> class.
         /// </summary>
         /// <exception cref="EventHandlerMustNotHaveStaticMethodArgumentException">
         /// Thrown when <paramref name="handler"/> references a static method.
         /// </exception>
         /// <exception cref="EventHandlerMustNotHaveNullMethodArgumentException">
         /// Thrown when an Event Handler Must Not Have Null Method Argument error
         /// condition occurs.
         /// </exception>
         /// <param name="handler">
         /// The event handler to be converted.
         /// </param>
         [SuppressMessage("SonarLint.CodeSmell", "S1144:Unused private types or members should be removed", Justification = "False positive.  This is public ctor in a class library")]
         public WeakEventHandler(EventHandler<TEventArgs> handler)
         {
            handler.ArgumentNotNull(nameof(handler));
            if (handler.Method.IsStatic)
            {
               throw new EventHandlerMustNotHaveStaticMethodArgumentException(nameof(handler), null, null);
            }

            if (handler.Method.IsNull())
            {
               throw new EventHandlerMustNotHaveNullMethodArgumentException(nameof(handler), null, null);
            }

            _targetReference = new WeakReference(handler.Target);
            _openHandler = (OpenEventHandler) Delegate.CreateDelegate(typeof(OpenEventHandler), null, handler.Method);
         }

         /// <inheritdoc/>
         public MethodInfo Method => _openHandler.Method;

         /// <inheritdoc/>
         public Object Target
         {
            get
            {
               var rv = _targetReference.Target;
               return rv;
            }
         }

         /// <inheritdoc/>
         public void Invoke(Object sender, TEventArgs e)
         {
            var target = (TTarget) _targetReference.Target;

            if (ReferenceEquals(target, null))
            {
               return;
            }

            _openHandler.Invoke(target, sender, e);
         }
      }
   }
}
