namespace Landorphan.Common.Threading
{
   using System;

   /// <summary>
   /// Represents a diagnostic services for a non-recursive lock.
   /// </summary>
   /// <remarks>
   /// These features should only be used for diagnostic purposes.  They should not be used for control of flow in production scenarios.
   /// </remarks>
   public interface IDiagnosticNonRecursiveLock
   {
      /// <summary>
      /// Gets the total number of unique threads that have entered the lock in read mode.
      /// </summary>
      /// <value>
      /// The number of unique threads that have entered the lock in read mode.  (Does not include any upgradeable read locks).
      /// </value>
      Int32 CurrentReadCount { get; }

      /// <summary>
      /// Gets a value that indicates whether the current thread has entered the lock in read mode.
      /// </summary>
      /// <value>
      /// <c> true </c> if the current thread has entered read mode; otherwise <c> false </c>.
      /// </value>
      Boolean IsReadLockHeld { get; }

      /// <summary>
      /// Gets a value that indicates whether the current thread has entered the lock in upgradeable mode.
      /// </summary>
      /// <value>
      /// <c> true </c> if the current thread has entered upgradeable mode; otherwise <c> false </c>.
      /// </value>
      Boolean IsUpgradeableLockHeld { get; }

      /// <summary>
      /// Gets a value that indicates whether the current thread has entered the lock in write mode.
      /// </summary>
      /// <value>
      /// <c> true </c> if the current thread has entered write mode; otherwise <c> false </c>.
      /// </value>
      Boolean IsWriteLockHeld { get; }

      /// <summary>
      /// Gets the total number of threads that are waiting to enter the lock in read mode.
      /// </summary>
      /// <value>
      /// The total number of threads that are waiting to enter read mode.
      /// </value>
      Int32 WaitingReadCount { get; }

      /// <summary>
      /// Gets the total number of threads that are waiting to enter the lock in upgradeable mode.
      /// </summary>
      /// <value>
      /// The total number of threads that are waiting to enter upgradeable mode.
      /// </value>
      Int32 WaitingUpgradeableCount { get; }

      /// <summary>
      /// Gets the total number of threads that are waiting to enter the lock in write mode.
      /// </summary>
      /// <value>
      /// The total number of threads that are waiting to enter write mode.
      /// </value>
      Int32 WaitingWriteCount { get; }
   }
}