namespace Landorphan.Common.Threading
{
   using System;
   using System.Threading;

   /// <summary>
   /// Represents a lock that is used to manage access to a resource, allowing multiple threads for reading or exclusive access for writing.
   /// </summary>
   /// <remarks>
   /// Use INonRecursiveLock instances to protect a resource that is read by multiple threads and written to by one thread at a time.
   /// INonRecursiveLock instances allows multiple threads to be in read mode, allows one thread to be in write mode with exclusive ownership of
   /// the lock, and allows one thread that has read access to be in upgradeable read mode, from which the thread can upgrade to write mode without
   /// having to relinquish its read access to the resource.
   /// </remarks>
   public interface INonRecursiveLock : IQueryDisposable
   {
      /// <summary>
      /// A timeout value representing "never".
      /// </summary>
      /// <value>
      /// The "never" timeout value.
      /// </value>
      TimeSpan TimeoutNever { get; }

      /// <summary>
      /// Tries to enter the lock in read mode.
      /// </summary>
      /// <returns>
      /// An <see cref="IDisposable"/> that exits the lock when disposed.
      /// </returns>
      /// <remarks>
      /// <p>
      /// This method blocks until the calling thread enters the lock, and therefore might never return.  Use either the
      /// <see cref="EnterReadLock(TimeSpan)"/> or the <see cref="TryEnterReadLock"/> method to block for a specified interval.
      /// </p>
      /// <p> Multiple threads can enter read mode at the same time. </p>
      /// <p>
      /// If one or more threads are waiting to enter write mode, a thread that calls the EnterReadLock method blocks until those threads have
      /// either timed out or entered write mode and then exited from it.
      /// </p>
      /// </remarks>
      /// <exception cref="LockRecursionException">
      /// The current thread has already entered read mode.
      /// -or-
      /// The current thread has already entered upgradeable read mode.
      /// -or-
      /// The current thread has already entered write mode.
      /// </exception>
      /// <exception cref="ObjectDisposedException">
      /// This instance has already been disposed.
      /// </exception>
      IDisposable EnterReadLock();

      /// <summary>
      /// Tries to enter the lock in read mode, with an optional time-out.
      /// </summary>
      /// <param name="timeout">
      /// The interval to wait, or -1 milliseconds to wait indefinitely.
      /// </param>
      /// <returns>
      /// An <see cref="IDisposable"/> that exits the lock when disposed.
      /// </returns>
      /// <remarks>
      /// <p> Multiple threads can enter read mode at the same time. </p>
      /// <p>
      /// If one or more threads are waiting to enter write mode, a thread that calls the EnterReadLock method blocks until those threads have
      /// either timed out or entered write mode and then exited from it.
      /// </p>
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value of timeout is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.
      /// -or-
      /// The value of timeout is greater than <see cref="Int32.MaxValue"/> milliseconds.
      /// </exception>
      /// <exception cref="LockRecursionException">
      /// The current thread has already entered read mode.
      /// -or-
      /// The current thread has already entered upgradeable read mode.
      /// -or-
      /// The current thread has already entered write mode.
      /// </exception>
      /// <exception cref="ObjectDisposedException">
      /// This instance has already been disposed.
      /// </exception>
      /// <exception cref="TimeoutElapsedBeforeLockObtainedException">
      /// The timeout elapsed before a lock was obtained.
      /// </exception>
      IDisposable EnterReadLock(TimeSpan timeout);

      /// <summary>
      /// Tries to enter the lock in upgradeable read mode.
      /// </summary>
      /// <returns>
      /// An <see cref="IDisposable"/> that exits the lock when disposed.
      /// </returns>
      /// <remarks>
      /// <p>
      /// This method blocks until the calling thread enters the lock, and therefore might never return.  Use either the
      /// <see cref="EnterUpgradeableReadLock(TimeSpan)"/> or the <see cref="TryEnterUpgradeableReadLock"/> method to block for a specified interval.
      /// </p>
      /// <p>
      /// Only one thread can enter upgradeable mode at any given time.  If a thread is in upgradeable mode, and there are no threads waiting to
      /// enter write mode, any number of other threads can enter read mode, even if there are threads waiting to enter upgradeable mode.
      /// (Upgradeable starvation).
      /// </p>
      /// <p>
      /// If one or more threads are waiting to enter write mode, a thread that calls the EnterUpgradeableReadLock method blocks until those threads
      /// have either timed out or entered write mode and then exited from it.
      /// </p>
      /// </remarks>
      /// <exception cref="LockRecursionException">
      /// The current thread has already entered read mode.
      /// -or-
      /// The current thread has already entered upgradeable read mode.
      /// -or-
      /// The current thread has already entered write mode.
      /// </exception>
      /// <exception cref="ObjectDisposedException">
      /// This instance has already been disposed.
      /// </exception>
      IDisposable EnterUpgradeableReadLock();

      /// <summary>
      /// Tries to enter the lock in upgradeable read mode, with an optional time-out.
      /// </summary>
      /// <param name="timeout">
      /// The interval to wait, or -1 milliseconds to wait indefinitely.
      /// </param>
      /// <returns>
      /// An <see cref="IDisposable"/> that exits the lock when disposed.
      /// </returns>
      /// <remarks>
      /// <p>
      /// Only one thread can enter upgradeable mode at any given time.  If a thread is in upgradeable mode, and there are no threads waiting to
      /// enter write mode, any number of other threads can enter read mode, even if there are threads waiting to enter upgradeable mode.
      /// (Upgradeable starvation).
      /// </p>
      /// <p>
      /// If one or more threads are waiting to enter write mode, a thread that calls the EnterUpgradeableReadLock method blocks until those threads
      /// have either timed out or entered write mode and then exited from it.
      /// </p>
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value of timeout is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.
      /// -or-
      /// The value of timeout is greater than <see cref="Int32.MaxValue"/> milliseconds.
      /// </exception>
      /// <exception cref="LockRecursionException">
      /// The current thread has already entered read mode.
      /// -or-
      /// The current thread has already entered upgradeable read mode.
      /// -or-
      /// The current thread has already entered write mode.
      /// </exception>
      /// <exception cref="ObjectDisposedException">
      /// This instance has already been disposed.
      /// </exception>
      /// <exception cref="TimeoutElapsedBeforeLockObtainedException">
      /// The timeout elapsed before a lock was obtained.
      /// </exception>
      IDisposable EnterUpgradeableReadLock(TimeSpan timeout);

      /// <summary>
      /// Tries to enter the lock in write mode.
      /// </summary>
      /// <returns>
      /// An <see cref="IDisposable"/> that exits the lock when disposed.
      /// </returns>
      /// <remarks>
      /// <p>
      /// This method blocks until the calling thread enters the lock, and therefore might never return.  Use either the
      /// <see cref="EnterWriteLock(TimeSpan)"/> or the <see cref="TryEnterWriteLock"/> method to block for a specified interval.
      /// </p>
      /// <p>
      /// If other threads have entered the lock in read mode, a thread that calls to enter write mode blocks until those
      /// threads have exited read mode.  When there are threads waiting to enter write mode, additional threads that try to enter read mode or
      /// upgradeable mode block until all the threads waiting to enter write mode have either timed out or entered write mode and then exited
      /// from it.
      /// </p>
      /// </remarks>
      /// <exception cref="LockRecursionException">
      /// The current thread has already entered read mode.
      /// -or-
      /// The current thread has already entered upgradeable read mode.
      /// -or-
      /// The current thread has already entered write mode.
      /// </exception>
      /// <exception cref="ObjectDisposedException">
      /// This instance has already been disposed.
      /// </exception>
      IDisposable EnterWriteLock();

      /// <summary>
      /// Tries to enter the lock in write mode, with an optional time-out.
      /// </summary>
      /// <param name="timeout">
      /// The interval to wait, or -1 milliseconds to wait indefinitely.
      /// </param>
      /// <returns>
      /// An <see cref="IDisposable"/> that exits the lock when disposed.
      /// </returns>
      /// <remarks>
      /// <p>
      /// This method blocks until the calling thread enters the lock, and therefore might never return.  Use either the
      /// <see cref="EnterWriteLock(TimeSpan)"/> or the <see cref="TryEnterWriteLock"/> method to block for a specified interval.
      /// </p>
      /// <p>
      /// If other threads have entered the lock in read mode, a thread that calls to enter write mode blocks until those
      /// threads have exited read mode.  When there are threads waiting to enter write mode, additional threads that try to enter read mode or
      /// upgradeable mode block until all the threads waiting to enter write mode have either timed out or entered write mode and then exited
      /// from it.
      /// </p>
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value of timeout is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.
      /// -or-
      /// The value of timeout is greater than <see cref="Int32.MaxValue"/> milliseconds.
      /// </exception>
      /// <exception cref="LockRecursionException">
      /// The current thread has already entered read mode.
      /// -or-
      /// The current thread has already entered upgradeable read mode.
      /// -or-
      /// The current thread has already entered write mode.
      /// </exception>
      /// <exception cref="ObjectDisposedException">
      /// This instance has already been disposed.
      /// </exception>
      /// <exception cref="TimeoutElapsedBeforeLockObtainedException">
      /// The timeout elapsed before a lock was obtained.
      /// </exception>
      IDisposable EnterWriteLock(TimeSpan timeout);

      /// <summary>
      /// Determines if the given value is valid for a timeout.
      /// </summary>
      /// <param name="timeSpan">
      /// The value to inspect.
      /// </param>
      /// <returns>
      /// <c> true </c>if the given value is valid for a timeout; otherwise, <c> false </c>.
      /// </returns>
      Boolean IsValidTimeout(TimeSpan timeSpan);

      /// <summary>
      /// Tries to enter the lock in read mode, with an optional time-out.
      /// </summary>
      /// <param name="timeout">
      /// The interval to wait, or -1 milliseconds to wait indefinitely.
      /// </param>
      /// <param name="obtainedLock">
      /// Set to <c> true </c> if a lock was obtained; otherwise it is set to <c> false </c>.
      /// </param>
      /// <returns>
      /// An <see cref="IDisposable"/> that exits the lock when disposed.
      /// </returns>
      /// <remarks>
      /// <p> Multiple threads can enter read mode at the same time. </p>
      /// <p>
      /// If one or more threads are waiting to enter write mode, a thread that calls the EnterReadLock method blocks until those threads have
      /// either timed out or entered write mode and then exited from it.
      /// </p>
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value of timeout is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.
      /// -or-
      /// The value of timeout is greater than <see cref="Int32.MaxValue"/> milliseconds.
      /// </exception>
      /// <exception cref="LockRecursionException">
      /// The current thread has already entered read mode.
      /// -or-
      /// The current thread has already entered upgradeable read mode.
      /// -or-
      /// The current thread has already entered write mode.
      /// </exception>
      /// <exception cref="ObjectDisposedException">
      /// This instance has already been disposed.
      /// </exception>
      IDisposable TryEnterReadLock(TimeSpan timeout, out Boolean obtainedLock);

      /// <summary>
      /// Tries to enter the lock in upgradeable read mode, with an optional time-out.
      /// </summary>
      /// <param name="timeout">
      /// The interval to wait, or -1 milliseconds to wait indefinitely.
      /// </param>
      /// <param name="obtainedLock">
      /// Set to <c> true </c> if a lock was obtained; otherwise it is set to <c> false </c>.
      /// </param>
      /// <returns>
      /// An <see cref="IDisposable"/> that exits the lock when disposed.
      /// </returns>
      /// <remarks>
      /// <p>
      /// Only one thread can enter upgradeable mode at any given time.  If a thread is in upgradeable mode, and there are no threads waiting to
      /// enter write mode, any number of other threads can enter read mode, even if there are threads waiting to enter upgradeable mode.
      /// (Upgradeable starvation).
      /// </p>
      /// <p>
      /// If one or more threads are waiting to enter write mode, a thread that calls the EnterUpgradeableReadLock method blocks until those threads
      /// have either timed out or entered write mode and then exited from it.
      /// </p>
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value of timeout is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.
      /// -or-
      /// The value of timeout is greater than <see cref="Int32.MaxValue"/> milliseconds.
      /// </exception>
      /// <exception cref="LockRecursionException">
      /// The current thread has already entered read mode.
      /// -or-
      /// The current thread has already entered upgradeable read mode.
      /// -or-
      /// The current thread has already entered write mode.
      /// </exception>
      /// <exception cref="ObjectDisposedException">
      /// This instance has already been disposed.
      /// </exception>
      IDisposable TryEnterUpgradeableReadLock(TimeSpan timeout, out Boolean obtainedLock);

      /// <summary>
      /// Tries to enter the lock in write mode, with an optional time-out.
      /// </summary>
      /// <param name="timeout">
      /// The interval to wait, or -1 milliseconds to wait indefinitely.
      /// </param>
      /// <param name="obtainedLock">
      /// Set to <c> true </c> if a lock was obtained; otherwise it is set to <c> false </c>.
      /// </param>
      /// <returns>
      /// An <see cref="IDisposable"/> that exits the lock when disposed.
      /// </returns>
      /// <remarks>
      /// <p>
      /// If other threads have entered the lock in read mode, a thread that calls the <see cref="EnterWriteLock()"/> method blocks until those
      /// threads have exited read mode.  When there are threads waiting to enter write mode, additional threads that try to enter read mode or
      /// upgradeable mode block until all the threads waiting to enter write mode have either timed out or entered write mode and then exited
      /// from it.
      /// </p>
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// The value of timeout is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.
      /// -or-
      /// The value of timeout is greater than <see cref="Int32.MaxValue"/> milliseconds.
      /// </exception>
      /// <exception cref="LockRecursionException">
      /// The current thread has already entered read mode.
      /// -or-
      /// The current thread has already entered upgradeable read mode.
      /// -or-
      /// The current thread has already entered write mode.
      /// </exception>
      /// <exception cref="ObjectDisposedException">
      /// This instance has already been disposed.
      /// </exception>
      IDisposable TryEnterWriteLock(TimeSpan timeout, out Boolean obtainedLock);
   }
}