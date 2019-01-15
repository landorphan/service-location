namespace Landorphan.Common.Threading
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Runtime.Serialization;
   using System.Threading;
   using Landorphan.Common.Resources;

   /// <summary>
   /// Represents a lock that is used to manage access to a resource, allowing multiple threads for reading or exclusive access for writing.
   /// </summary>
   /// <remarks>
   /// This implementation guards against writer starvation but upgradeable lock requests may be starved.
   /// </remarks>
   public sealed class NonRecursiveLock : INonRecursiveLock, IDiagnosticNonRecursiveLock
   {
      /// <summary>
      /// A timeout value representing "never".
      /// </summary>
      public static readonly TimeSpan TimeoutNever = TimeSpan.FromMilliseconds(-1);

      private InterlockedBoolean _isDisposed = new InterlockedBoolean(false);
      private InterlockedBoolean _isDisposing = new InterlockedBoolean(false);

      [NonSerialized]
      private ReaderWriterLockSlim _wrappedLock;

      /// <summary>
      /// Initializes a new instance of the <see cref="NonRecursiveLock"/> class.
      /// </summary>
      public NonRecursiveLock()
      {
         _wrappedLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
      }

      /// <inheritdoc/>
      public void Dispose()
      {
         if (_isDisposed)
         {
            // already disposed
            return;
         }

         if (_isDisposing.ExchangeValue(true))
         {
            // already disposing
            return;
         }

         Dispose(true);
         _isDisposing = false;
         _isDisposed = true;
      }

      [SuppressMessage("SonarLint.CodeSmell", "S1066: Collapsible 'if' statements should be merged", Justification = "Let go my ears! (MWP)")]
      private void Dispose(Boolean disposing)
      {
         if (disposing)
         {
            // Clean up managed resources if disposing
            if (_wrappedLock != null)
            {
               _wrappedLock.Dispose();
               _wrappedLock = null;
            }
         }

         // Clean up native resources always
         // (none)
      }

      private void ThrowIfDisposed()
      {
         if (_isDisposed)
         {
            throw new ObjectDisposedException(GetType().Name);
         }
      }

      /// <inheritdoc/>
      public Boolean AllowsRecursion => _wrappedLock.RecursionPolicy != LockRecursionPolicy.NoRecursion;

      /// <inheritdoc/>
      public Boolean IsDisposed => _isDisposed;

      /// <inheritdoc/>
      public Boolean IsDisposing => _isDisposing;

      /// <inheritdoc/>
      Int32 IDiagnosticNonRecursiveLock.CurrentReadCount
      {
         get
         {
            ThrowIfDisposed();
            return _wrappedLock.CurrentReadCount;
         }
      }

      /// <inheritdoc/>
      Boolean IDiagnosticNonRecursiveLock.IsReadLockHeld
      {
         get
         {
            ThrowIfDisposed();
            return _wrappedLock.IsReadLockHeld;
         }
      }

      /// <inheritdoc/>
      Boolean IDiagnosticNonRecursiveLock.IsUpgradeableLockHeld
      {
         get
         {
            ThrowIfDisposed();
            return _wrappedLock.IsUpgradeableReadLockHeld;
         }
      }

      /// <inheritdoc/>
      Boolean IDiagnosticNonRecursiveLock.IsWriteLockHeld
      {
         get
         {
            ThrowIfDisposed();
            return _wrappedLock.IsWriteLockHeld;
         }
      }

      /// <inheritdoc/>
      TimeSpan INonRecursiveLock.TimeoutNever => TimeoutNever;

      /// <inheritdoc/>
      Int32 IDiagnosticNonRecursiveLock.WaitingReadCount
      {
         get
         {
            ThrowIfDisposed();
            return _wrappedLock.WaitingReadCount;
         }
      }

      /// <inheritdoc/>
      Int32 IDiagnosticNonRecursiveLock.WaitingUpgradeableCount
      {
         get
         {
            ThrowIfDisposed();
            return _wrappedLock.WaitingUpgradeCount;
         }
      }

      /// <inheritdoc/>
      Int32 IDiagnosticNonRecursiveLock.WaitingWriteCount
      {
         get
         {
            ThrowIfDisposed();
            return _wrappedLock.WaitingWriteCount;
         }
      }

      /// <inheritdoc/>
      public IDisposable EnterReadLock()
      {
         return EnterReadLock(TimeoutNever);
      }

      /// <inheritdoc/>
      public IDisposable EnterReadLock(TimeSpan timeout)
      {
         var rv = TryEnterReadLock(timeout, out var obtainedLock);
         if (!obtainedLock)
         {
            throw new TimeoutElapsedBeforeLockObtainedException(timeout);
         }

         return rv;
      }

      /// <inheritdoc/>
      public IDisposable EnterUpgradeableReadLock()
      {
         return EnterUpgradeableReadLock(TimeoutNever);
      }

      /// <inheritdoc/>
      public IDisposable EnterUpgradeableReadLock(TimeSpan timeout)
      {
         var rv = TryEnterUpgradeableReadLock(timeout, out var obtainedLock);
         if (!obtainedLock)
         {
            throw new TimeoutElapsedBeforeLockObtainedException(timeout);
         }

         return rv;
      }

      /// <inheritdoc/>
      public IDisposable EnterWriteLock()
      {
         return EnterWriteLock(TimeoutNever);
      }

      /// <inheritdoc/>
      public IDisposable EnterWriteLock(TimeSpan timeout)
      {
         var rv = TryEnterWriteLock(timeout, out var obtainedLock);
         if (!obtainedLock)
         {
            throw new TimeoutElapsedBeforeLockObtainedException(timeout);
         }

         return rv;
      }

      /// <inheritdoc/>
      public Boolean IsValidTimeout(TimeSpan timeSpan)
      {
         var rv = true;
         var totalMilliseconds = (Int64)timeSpan.TotalMilliseconds;
         if (totalMilliseconds < -1 || totalMilliseconds > Int32.MaxValue)
         {
            rv = false;
         }

         return rv;
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S3874: 'out' and 'ref' parameters should not be used", Justification = "Appropriate use (MWP)")]
      public IDisposable TryEnterReadLock(TimeSpan timeout, out Boolean obtainedLock)
      {
         ThrowIfDisposed();
         ValidateTimeout(timeout, "timeout");

         // ReaderWriterLockSlim prevents read locks from being obtained by a thread that already has a read lock.
         // ReaderWriterLockSlim prevents read locks from being obtained by a thread that already has a write lock.
         // ReaderWriterLockSlim DOES NOT prevent read locks from being obtained by a thread that already has an upgradeable lock.
         // For the sake of consistency, prevent read locks from being obtained by a thread that already has an upgradeable lock.
         var diagnostics = this as IDiagnosticNonRecursiveLock;
         if (diagnostics.IsUpgradeableLockHeld)
         {
            throw new LockRecursionException(StringResources.LockRecursionExceptionReadAfterUpgradeNotAllowed);
         }

         obtainedLock = _wrappedLock.TryEnterReadLock(timeout);
         var rv = obtainedLock ? new ExitLock(_wrappedLock.ExitReadLock) : null;
         return rv;
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S3874: 'out' and 'ref' parameters should not be used", Justification = "Appropriate use (MWP)")]
      public IDisposable TryEnterUpgradeableReadLock(TimeSpan timeout, out Boolean obtainedLock)
      {
         ThrowIfDisposed();
         ValidateTimeout(timeout, "timeout");

         obtainedLock = _wrappedLock.TryEnterUpgradeableReadLock(timeout);
         var rv = obtainedLock ? new ExitLock(_wrappedLock.ExitUpgradeableReadLock) : null;
         return rv;
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S3874: 'out' and 'ref' parameters should not be used", Justification = "Appropriate use (MWP)")]
      public IDisposable TryEnterWriteLock(TimeSpan timeout, out Boolean obtainedLock)
      {
         ThrowIfDisposed();
         ValidateTimeout(timeout, "timeout");

         obtainedLock = _wrappedLock.TryEnterWriteLock(timeout);
         var rv = obtainedLock ? new ExitLock(_wrappedLock.ExitWriteLock) : null;
         return rv;
      }

      [OnDeserializing]
      private void OnDeserializing(StreamingContext context)
      {
         _wrappedLock = new ReaderWriterLockSlim();
      }

      private void ValidateTimeout(TimeSpan timeout, String argumentName)
      {
         if (!IsValidTimeout(timeout))
         {
            throw new InvalidLockTimeoutArgumentException(argumentName, timeout, null, null);
         }
      }

      [SuppressMessage("SonarLint.CodeSmell", "S2933: Fields that are only assigned in the constructor should be 'readonly'", Justification = "False positive (MWP)")]
      private sealed class ExitLock : IDisposable
      {
         private InterlockedBoolean _disposed;
         private Action _releaseLockAction;

         internal ExitLock(Action releaseLockAction)
         {
            releaseLockAction.ArgumentNotNull(nameof(releaseLockAction));

            _releaseLockAction = releaseLockAction;
            _disposed = new InterlockedBoolean(false);
         }

         void IDisposable.Dispose()
         {
            if (_disposed.ExchangeValue(true))
            {
               // already disposed or currently disposing
               return;
            }

            _releaseLockAction.Invoke();
            _releaseLockAction = null;
         }
      }
   }
}
