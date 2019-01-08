namespace Landorphan.Common
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Reflection;
   using Landorphan.Common.Threading;

   /// <summary>
   /// Base implementation for classes implementing <see cref="IQueryDisposable"/>.
   /// </summary>
   /// <remarks>
   /// Avoid deriving from this class if binary serialization is needed.
   /// </remarks>
   [SuppressMessage("Microsoft.", "CA1063: Implement IDisposable Correctly", Justification = "Reviewed (MWP)")]
   public abstract class DisposableObject : INotifyingQueryDisposable
   {
      private readonly SourceWeakEventHandlerSet<EventArgs> _listenersDisposing = new SourceWeakEventHandlerSet<EventArgs>();
      private InterlockedBoolean _isDisposed = new InterlockedBoolean(false);
      private InterlockedBoolean _isDisposing = new InterlockedBoolean(false);

      /// <inheritdoc/>
      public void Dispose()
      {
         if (_isDisposed)
         {
            return;
         }

         if (_isDisposing.ExchangeValue(true))
         {
            return;
         }

         Dispose(true);
         _isDisposing = false;
         _isDisposed = true;

         // Use SuppressFinalize in case a subclass implements a finalizer.
         GC.SuppressFinalize(this);
      }

      /// <summary>
      /// Releases the unmanaged resources used by the <see cref="DisposableObject"/> and optionally releases the managed resources.
      /// </summary>
      /// <param name="disposing">
      /// <c> true </c> to release both managed and unmanaged resources; <c> false </c> to release only unmanaged resources.
      /// </param>
      protected virtual void Dispose(Boolean disposing)
      {
         if (disposing)
         {
            // notify all listeners
            OnDisposing();

            // Clean up managed resources if disposing
            ReleaseManagedResources();
         }

         // Clean up native resources always
         ReleaseUnmanagedResources();
      }

      /// <summary>
      /// Throws an <see cref="ObjectDisposedException"/> if this instance has been disposed.
      /// </summary>
      protected void ThrowIfDisposed()
      {
         if (_isDisposed)
         {
            throw new ObjectDisposedException(GetType().Name);
         }
      }

      /// <summary>
      /// Finds and releases all managed resources.
      /// </summary>
      [SuppressMessage("SonarLint.CodeSmell", "S1696:NullReferenceException should not be caught", Justification = "Eats the exception in a race condition (MWP)")]
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S3776:Cognitive Complexity of methods should not be too high",
         Justification = "This method addresses the general problem of disposing, reviewed as acceptable (MWP)")]
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S4056:Overloads with a 'CultureInfo' or an 'IFormatProvider' parameter should be used",
         Justification = "I see no value in applying a culture to a null value (MWP).")]
      protected virtual void ReleaseManagedResources()
      {
         var derivedType = GetType();
         while (derivedType != typeof(DisposableObject))
         {
            // TODO: (MWP) double check the auto-property fields are grabbed since port to this platform .
            var fields = derivedType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
               var value = field.GetValue(this);
               if (value == null)
               {
                  continue;
               }

               var fieldType = value.GetType();
               if (fieldType.IsValueType)
               {
                  continue;
               }

               if (ReferenceEquals(field.GetCustomAttribute<DoNotDisposeAttribute>(false), null))
               {
                  // collections of IDisposable.
                  if (value is IEnumerable<IDisposable> disposables)
                  {
                     foreach (var disposable in disposables)
                     {
                        disposable?.Dispose();
                     }
                  }

                  // TODO: (mwp) Add special treatment for IEnumerable<KeyValuePair<TKey, TValue>> because it is a common data structure.

                  // IDisposable
                  if (value is IDisposable asDisposable)
                  {
                     try
                     {
                        asDisposable.Dispose();
                     }
                     catch (NullReferenceException)
                     {
                        // eat the exception
                        // (this is sometimes seen when there are chains of disposables, ownership is not a well defined concept in .Net.
                     }
                  }

                  field.SetValue(this, null);
               }
            }

            derivedType = derivedType.BaseType;
         }
      }

      /// <summary>
      /// Releases the unmanaged resources.
      /// </summary>
      protected virtual void ReleaseUnmanagedResources()
      {
      }

      /// <summary>
      /// Ensures that resources are freed and other cleanup operations are performed when the garbage collector reclaims the
      /// <see cref="DisposableObject"/>.
      /// </summary>
      ~DisposableObject()
      {
         Dispose(false);
      }

      /// <inheritdoc/>
      public event EventHandler<EventArgs> Disposing
      {
         add => _listenersDisposing.Add(value);
         remove => _listenersDisposing.Remove(value);
      }

      /// <inheritdoc/>
      public Boolean IsDisposed => _isDisposed;

      /// <inheritdoc/>
      public Boolean IsDisposing => _isDisposing;

      /// <summary>
      /// Notifies all listeners that this instance is being disposed.
      /// </summary>
      protected virtual void OnDisposing()
      {
         _listenersDisposing.Invoke(this, EventArgs.Empty);
      }
   }
}