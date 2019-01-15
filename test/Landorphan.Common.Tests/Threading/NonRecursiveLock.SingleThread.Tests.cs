namespace Landorphan.Common.Tests.Threading
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Threading;
   using FluentAssertions;
   using Landorphan.Common.Threading;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class NonRecursiveLock_Tests
   {
      [TestClass]
      public class It_should_be_able_to_determine_the_validity_of_a_timeout_value : TestBase
      {
         private static INonRecursiveLock target;

         /// <summary>
         /// Code that executes before any of the tests methods in the test class are executed.
         /// </summary>
         [ClassInitialize]
         public static void ThisTestClassInitialize(TestContext context)
         {
            target = new NonRecursiveLock();
         }

         /// <summary>
         /// Code that executes after all of the test methods in the test class are executed.
         /// </summary>
         [ClassCleanup]
         public static void ThisTestClassCleanup()
         {
            if (target != null)
            {
               target.Dispose();
               target = null;
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_valid_values()
         {
            target.IsValidTimeout(NonRecursiveLock.TimeoutNever).Should().BeTrue();
            target.IsValidTimeout(TimeSpan.FromMilliseconds(0)).Should().BeTrue();
            target.IsValidTimeout(TimeSpan.FromMilliseconds(1)).Should().BeTrue();
            target.IsValidTimeout(TimeSpan.FromMilliseconds(Int32.MaxValue)).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_disallow_negative_values_other_than_timeout_never()
         {
            target.IsValidTimeout(TimeSpan.FromMilliseconds(-2)).Should().BeFalse();
            target.IsValidTimeout(NonRecursiveLock.TimeoutNever).Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_disallow_positive_values_that_are_too_large()
         {
            target.IsValidTimeout(TimeSpan.FromMilliseconds(Int32.MaxValue + 1L)).Should().BeFalse();
            target.IsValidTimeout(TimeSpan.MaxValue).Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_allow_recursion()
         {
            target.AllowsRecursion.Should().BeFalse();
         }
      }

      [TestClass]
      public abstract class NonRecursiveLockContext : DisposableArrangeActAssert
      {
         private NonRecursiveLock _target;

         protected INonRecursiveLock Target => _target;

         protected IDiagnosticNonRecursiveLock TargetDiagnostic => _target;

         protected override void TeardownTestMethod()
         {
            if (_target.IsNotNull())
            {
               _target.Dispose();
               _target = null;
            }
         }

         protected override void ActMethod()
         {
            _target = new NonRecursiveLock();
         }
      }

      [TestClass]
      public class When_a_thread_holds_a_read_lock : NonRecursiveLockContext
      {
         private IDisposable exitLock;

         protected override void TeardownTestMethod()
         {
            if (exitLock.IsNotNull())
            {
               exitLock.Dispose();
               exitLock = null;
            }

            base.TeardownTestMethod();
         }

         protected override void ActMethod()
         {
            base.ActMethod();
            exitLock = Target.EnterReadLock();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_disallow_additional_locks_from_the_same_thread()
         {
            // read lock attempts...
            Action throwingAction = () => Target.EnterReadLock();
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.EnterReadLock(NonRecursiveLock.TimeoutNever);
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.TryEnterReadLock(NonRecursiveLock.TimeoutNever, out _);
            throwingAction.Should().Throw<LockRecursionException>();

            // upgradeable read lock attempts...
            throwingAction = () => Target.EnterUpgradeableReadLock();
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.EnterUpgradeableReadLock(NonRecursiveLock.TimeoutNever);
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.TryEnterUpgradeableReadLock(NonRecursiveLock.TimeoutNever, out _);
            throwingAction.Should().Throw<LockRecursionException>();

            // write lock attempts...
            throwingAction = () => Target.EnterWriteLock();
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.EnterWriteLock(NonRecursiveLock.TimeoutNever);
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.TryEnterWriteLock(NonRecursiveLock.TimeoutNever, out _);
            throwingAction.Should().Throw<LockRecursionException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_indicate_a_read_lock_is_held_by_this_thread()
         {
            TargetDiagnostic.IsReadLockHeld.Should().BeTrue();
            TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
            TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();

            TargetDiagnostic.CurrentReadCount.Should().Be(1);
         }
      }

      [TestClass]
      public class When_a_thread_holds_a_write_lock : NonRecursiveLockContext
      {
         private IDisposable exitLock;

         protected override void TeardownTestMethod()
         {
            if (exitLock.IsNotNull())
            {
               exitLock.Dispose();
               exitLock = null;
            }

            base.TeardownTestMethod();
         }

         protected override void ActMethod()
         {
            base.ActMethod();
            exitLock = Target.EnterWriteLock();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_disallow_additional_locks_from_the_same_thread()
         {
            // read lock attempts...
            Action throwingAction = () => Target.EnterReadLock();
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.EnterReadLock(NonRecursiveLock.TimeoutNever);
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.TryEnterReadLock(NonRecursiveLock.TimeoutNever, out _);
            throwingAction.Should().Throw<LockRecursionException>();

            // upgradeable read lock attempts...
            throwingAction = () => Target.EnterUpgradeableReadLock();
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.EnterUpgradeableReadLock(NonRecursiveLock.TimeoutNever);
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.TryEnterUpgradeableReadLock(NonRecursiveLock.TimeoutNever, out _);
            throwingAction.Should().Throw<LockRecursionException>();

            // write lock attempts...
            throwingAction = () => Target.EnterWriteLock();
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.EnterWriteLock(NonRecursiveLock.TimeoutNever);
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.TryEnterWriteLock(NonRecursiveLock.TimeoutNever, out _);
            throwingAction.Should().Throw<LockRecursionException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_indicate_a_write_lock_is_held_by_this_thread()
         {
            TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
            TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
            TargetDiagnostic.IsWriteLockHeld.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_a_thread_holds_an_upgradeable_read_lock : NonRecursiveLockContext
      {
         private IDisposable exitLock;

         protected override void TeardownTestMethod()
         {
            if (exitLock.IsNotNull())
            {
               exitLock.Dispose();
               exitLock = null;
            }

            base.TeardownTestMethod();
         }

         protected override void ActMethod()
         {
            base.ActMethod();
            exitLock = Target.EnterUpgradeableReadLock();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_allow_an_upgrade_to_a_write_lock_on_the_same_thread()
         {
            using (Target.EnterWriteLock())
            {
               TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeTrue();
               TargetDiagnostic.IsWriteLockHeld.Should().BeTrue();
            }

            using (Target.TryEnterWriteLock(NonRecursiveLock.TimeoutNever, out _))
            {
               TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeTrue();
               TargetDiagnostic.IsWriteLockHeld.Should().BeTrue();
            }
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_disallow_additional_read_or_upgradeable_locks_from_the_same_thread()
         {
            // read lock attempts...
            Action throwingAction = () => Target.EnterReadLock();
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.EnterReadLock(NonRecursiveLock.TimeoutNever);
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.TryEnterReadLock(NonRecursiveLock.TimeoutNever, out _);
            throwingAction.Should().Throw<LockRecursionException>();

            // upgradeable read lock attempts...
            throwingAction = () => Target.EnterUpgradeableReadLock();
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.EnterUpgradeableReadLock(NonRecursiveLock.TimeoutNever);
            throwingAction.Should().Throw<LockRecursionException>();

            throwingAction = () => Target.TryEnterUpgradeableReadLock(NonRecursiveLock.TimeoutNever, out _);
            throwingAction.Should().Throw<LockRecursionException>();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_indicate_an_upgradeable_lock_is_held_by_this_thread()
         {
            TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
            TargetDiagnostic.IsUpgradeableLockHeld.Should().BeTrue();
            TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_a_thread_obtains_a_lock_as_the_first_requester : NonRecursiveLockContext
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_be_granted_a_lock()
         {
            // read locks
            using (Target.EnterReadLock())
            {
               TargetDiagnostic.IsReadLockHeld.Should().BeTrue();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
               TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
            }

            TargetDiagnostic.IsReadLockHeld.Should().BeFalse();

            using (Target.EnterReadLock(NonRecursiveLock.TimeoutNever))
            {
               TargetDiagnostic.IsReadLockHeld.Should().BeTrue();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
               TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
            }

            TargetDiagnostic.IsReadLockHeld.Should().BeFalse();

            using (Target.TryEnterReadLock(NonRecursiveLock.TimeoutNever, out var lockObtained))
            {
               lockObtained.Should().BeTrue();
               TargetDiagnostic.IsReadLockHeld.Should().BeTrue();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
               TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
            }

            TargetDiagnostic.IsReadLockHeld.Should().BeFalse();

            // upgradeable read locks
            using (Target.EnterUpgradeableReadLock())
            {
               TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeTrue();
               TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
            }

            TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();

            using (Target.EnterUpgradeableReadLock(NonRecursiveLock.TimeoutNever))
            {
               TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeTrue();
               TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
            }

            TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();

            using (Target.TryEnterUpgradeableReadLock(NonRecursiveLock.TimeoutNever, out var lockObtained))
            {
               lockObtained.Should().BeTrue();
               TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeTrue();
               TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
            }

            TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();

            // write locks
            using (Target.EnterWriteLock())
            {
               TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
               TargetDiagnostic.IsWriteLockHeld.Should().BeTrue();
            }

            TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();

            using (Target.EnterWriteLock(NonRecursiveLock.TimeoutNever))
            {
               TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
               TargetDiagnostic.IsWriteLockHeld.Should().BeTrue();
            }

            TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();

            using (Target.TryEnterWriteLock(NonRecursiveLock.TimeoutNever, out var lockObtained))
            {
               lockObtained.Should().BeTrue();
               TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
               TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
               TargetDiagnostic.IsWriteLockHeld.Should().BeTrue();
            }

            TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_a_thread_releases_a_read_lock : NonRecursiveLockContext
      {
         private IDisposable exitLock;

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_on_subsequent_calls_to_Dispose()
         {
            exitLock.Dispose();
         }

         protected override void TeardownTestMethod()
         {
            if (exitLock.IsNotNull())
            {
               exitLock.Dispose();
               exitLock = null;
            }

            base.TeardownTestMethod();
         }

         protected override void ActMethod()
         {
            base.ActMethod();
            exitLock = Target.EnterReadLock();
            exitLock.Dispose();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_release_the_lock()
         {
            TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_a_thread_releases_a_write_lock : NonRecursiveLockContext
      {
         private IDisposable exitLock;

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_on_subsequent_calls_to_Dispose()
         {
            exitLock.Dispose();
         }

         protected override void TeardownTestMethod()
         {
            if (exitLock.IsNotNull())
            {
               exitLock.Dispose();
               exitLock = null;
            }

            base.TeardownTestMethod();
         }

         protected override void ActMethod()
         {
            base.ActMethod();
            exitLock = Target.EnterWriteLock();
            exitLock.Dispose();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_release_the_lock()
         {
            TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
         }
      }

      [TestClass]
      public class When_a_thread_releases_an_upgradeable_read_lock : NonRecursiveLockContext
      {
         private IDisposable exitLock;

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_on_subsequent_calls_to_Dispose()
         {
            exitLock.Dispose();
         }

         protected override void TeardownTestMethod()
         {
            if (exitLock.IsNotNull())
            {
               exitLock.Dispose();
               exitLock = null;
            }

            base.TeardownTestMethod();
         }

         protected override void ActMethod()
         {
            base.ActMethod();
            exitLock = Target.EnterUpgradeableReadLock();
            exitLock.Dispose();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_release_the_lock()
         {
            TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
         }
      }

      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "aNon")]
      [TestClass]
      public class When_I_create_a_NonRecursiveLock : NonRecursiveLockContext
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_no_read_locks()
         {
            TargetDiagnostic.IsReadLockHeld.Should().BeFalse();
            TargetDiagnostic.CurrentReadCount.Should().Be(0);
            TargetDiagnostic.WaitingReadCount.Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_no_upgradeable_locks()
         {
            TargetDiagnostic.IsUpgradeableLockHeld.Should().BeFalse();
            TargetDiagnostic.WaitingUpgradeableCount.Should().Be(0);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_no_write_locks()
         {
            TargetDiagnostic.IsWriteLockHeld.Should().BeFalse();
            TargetDiagnostic.WaitingWriteCount.Should().Be(0);
         }
      }

      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "aNon")]
      [TestClass]
      public class When_I_dispose_a_NonRecursiveLock : NonRecursiveLockContext
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_throw_on_Dispose()
         {
            Target.Dispose();
         }

         protected override void ActMethod()
         {
            base.ActMethod();
            Target.Dispose();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_indicate_it_is_disposed()
         {
            Target.IsDisposed.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_CurrentReadCount()
         {
            Object ignored = null;

            Action throwingAction = () => ignored = TargetDiagnostic.CurrentReadCount;
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");

            TestHelp.DoNothing(ignored);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_EnterReadLock()
         {
            Action throwingAction = () => Target.EnterReadLock();
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_EnterUpgradeableReadLock()
         {
            Action throwingAction = () => Target.EnterUpgradeableReadLock();
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_EnterWriteLock()
         {
            Action throwingAction = () => Target.EnterWriteLock();
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_IsReadLockHeld()
         {
            Object ignored = null;

            Action throwingAction = () => ignored = TargetDiagnostic.IsReadLockHeld;
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");

            TestHelp.DoNothing(ignored);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_IsUpgradeableLockHeld()
         {
            Object ignored = null;

            Action throwingAction = () => ignored = TargetDiagnostic.IsUpgradeableLockHeld;
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");

            TestHelp.DoNothing(ignored);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_IsWriteLockHeld()
         {
            Object ignored = null;

            Action throwingAction = () => ignored = TargetDiagnostic.IsWriteLockHeld;
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");

            TestHelp.DoNothing(ignored);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_TryEnterReadLock()
         {
            Action throwingAction = () => Target.TryEnterReadLock(NonRecursiveLock.TimeoutNever, out _);
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_TryEnterUpgradeableReadLock()
         {
            Action throwingAction = () => Target.TryEnterUpgradeableReadLock(NonRecursiveLock.TimeoutNever, out _);
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_TryEnterWriteLock()
         {
            Action throwingAction = () => Target.TryEnterWriteLock(NonRecursiveLock.TimeoutNever, out _);
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_WaitingReadCount()
         {
            Object ignored = null;

            Action throwingAction = () => ignored = TargetDiagnostic.WaitingReadCount;
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");

            TestHelp.DoNothing(ignored);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_WaitingUpgradeableCount()
         {
            Object ignored = null;

            Action throwingAction = () => ignored = TargetDiagnostic.WaitingUpgradeableCount;
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");

            TestHelp.DoNothing(ignored);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_throw_on_WaitingWriteCount()
         {
            Object ignored = null;

            Action throwingAction = () => ignored = TargetDiagnostic.WaitingWriteCount;
            var e = throwingAction.Should().Throw<ObjectDisposedException>();
            e.WithMessage("*Cannot access a disposed object.*");
            e.WithMessage("*NonRecursiveLock*");

            TestHelp.DoNothing(ignored);
         }
      }
   }
}
