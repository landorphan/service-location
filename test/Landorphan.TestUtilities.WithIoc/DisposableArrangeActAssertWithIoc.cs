namespace Landorphan.TestUtilities.WithIoc
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Reflection;
   using System.Threading;
   using Landorphan.Common;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   ///    Provides common services for BDD-style (context/specification) unit tests.  Serves as an adapter between the MSTest
   ///    and  BDD-style tests.
   /// </summary>
   /// <remarks>
   ///    Used when <see cref="IDisposable" /> fields are present.
   /// </remarks>
   [SuppressMessage("SonarLint.CodeSmell", "S3881:IDisposable should be implemented correctly", Justification = "Reviewed (MWP)")]
   [CLSCompliant(false)]
   [TestClass]
   public abstract class DisposableArrangeActAssertWithIoc : ArrangeActAssertWithIoc, INotifyingQueryDisposable
   {
      private Int32 _isDisposed;
      private Int32 _isDisposing;

      /// <inheritdoc />
      [SuppressMessage("Microsoft.Design", "CA1063: Implement IDisposable Correctly", Justification = "Reviewed (MWP)")]
      public void Dispose()
      {
         if (0 != Interlocked.Exchange(ref _isDisposed, 1))
         {
            // already disposed
            return;
         }

         if (0 != Interlocked.Exchange(ref _isDisposing, 1))
         {
            // already disposing
            return;
         }

         Dispose(true);

         // Use SuppressFinalize in case a subclass implements a finalizer.
         GC.SuppressFinalize(this);
      }

      /// <summary>
      ///    Releases the unmanaged resources used by the <see cref="DisposableArrangeActAssert" /> and optionally releases the
      ///    managed resources.
      /// </summary>
      /// <param name="disposing">
      ///    true to release both managed and unmanaged resources; false to release only unmanaged
      ///    resources.
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
      ///    Throws an <see cref="ObjectDisposedException" /> if this instance has been disposed.
      /// </summary>
      protected void ThrowIfDisposed()
      {
         if (IsDisposed)
         {
            throw new ObjectDisposedException(GetType().Name);
         }
      }

      /// <summary>
      ///    Finds and releases all managed resources.
      /// </summary>
      [SuppressMessage("SonarLint.CodeSmell", "S134: Control flow statements if, switch, for, foreach, while, do  and try should not be nested too deeply")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      [SuppressMessage("SonarLint.CodeSmell", "S4056: Overloads with a CultureInfo or an IFormatProvider parameter should be used")]
      protected virtual void ReleaseManagedResources()
      {
         var derivedType = GetType();

         while (derivedType != typeof(DisposableArrangeActAssert))
         {
            // ReSharper disable once PossibleNullReferenceException
            var fields = derivedType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
               var value = field.GetValue(this);
               if (value != null)
               {
                  var fieldType = value.GetType();
                  if (fieldType.IsValueType)
                  {
                     continue;
                  }

                  // collections of IDisposable.
                  if (value is IEnumerable<IDisposable> disposables)
                  {
                     foreach (var disposable in disposables)
                     {
                        disposable?.Dispose();
                     }
                  }

                  // Todo: (mwp) Add special treatment for IEnumerable<KeyValuePair<TKey, TValue>> because it is a common data structure.

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

                  // else ... this disposable field is not owned by this instance.
               }
            }

            derivedType = derivedType.BaseType;
         }
      }

      /// <summary>
      ///    Releases the unmanaged resources.
      /// </summary>
      protected virtual void ReleaseUnmanagedResources()
      {
      }

      /// <summary>
      ///    Ensures that resources are freed and other cleanup operations are performed when the garbage collector reclaims the
      ///    <see cref="DisposableArrangeActAssert" />.
      /// </summary>
      ~DisposableArrangeActAssertWithIoc()
      {
         Dispose(false);
      }

      /// <inheritdoc />
      public event EventHandler<EventArgs> Disposing;

      /// <inheritdoc />
      public Boolean IsDisposed => _isDisposed != 0;

      /// <inheritdoc />
      public Boolean IsDisposing => _isDisposing != 0;

      /// <summary>
      ///    Notifies all listeners that this instance is being disposed.
      /// </summary>
      protected virtual void OnDisposing()
      {
         var handler = Disposing;
         handler?.Invoke(this, EventArgs.Empty);
      }
   }
}