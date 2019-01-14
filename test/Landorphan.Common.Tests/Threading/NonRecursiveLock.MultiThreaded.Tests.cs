namespace Landorphan.Common.Tests.Threading
{
   using System;
   using System.Collections.Concurrent;
   using System.Diagnostics.CodeAnalysis;
   using System.Threading;
   using FluentAssertions;
   using Landorphan.Common.Threading;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming
   // ReSharper disable ImpureMethodCallOnReadonlyValueField

   [TestClass]
   public abstract class Multi_Threaded_Non_Recursive_Lock_Context : DisposableArrangeActAssert
   {
      private readonly InterlockedBoolean _releaseReadLock = new InterlockedBoolean(false);
      private readonly InterlockedBoolean _releaseUpgradeableLock = new InterlockedBoolean(false);
      private readonly InterlockedBoolean _releaseWriteLock = new InterlockedBoolean(false);
      private NonRecursiveLock _target;
      private volatile Boolean _teardownThread;
      private ConcurrentBag<Thread> _threads;

      protected IDiagnosticNonRecursiveLock TargetDiagnostic => _target;

      [SuppressMessage("SonarLint", "S3776:Cognitive Complexity of methods should not be too high")]
      [SuppressMessage("SonarLint", "S134: Control flow statements if, switch, for, foreach, while, do  and try should not be nested too deeply")]
      protected override void TeardownTestMethod()
      {
         ReleaseWriteLock();
         ReleaseUpgradeableLock();
         ReleaseReadLock();

         _teardownThread = true;

         Thread.Sleep(TimeSpan.FromSeconds(1));

         if (_threads != null)
         {
            foreach (var t in _threads)
            {
               t?.Join();
            }

            _threads = null;
         }

         if (_target.IsNotNull())
         {
            _target.Dispose();
            _target = null;
         }
      }

      protected override void ArrangeMethod()
      {
         _threads = new ConcurrentBag<Thread>();
         _target = new NonRecursiveLock();
      }

      protected void AddThread(Thread thread)
      {
         _threads.Add(thread);
      }

      protected void ReleaseReadLock()
      {
         _releaseReadLock.SetValue(true);
      }

      protected void ReleaseUpgradeableLock()
      {
         _releaseUpgradeableLock.SetValue(true);
      }

      protected void ReleaseWriteLock()
      {
         _releaseWriteLock.SetValue(true);
      }

      protected void SeekReadLock()
      {
         using (_target.EnterReadLock())
         {
            while (!_releaseReadLock)
            {
               if (_teardownThread)
               {
                  return;
               }

               Thread.Sleep(0);
            }
         }
      }

      protected void SeekUpgradeableLock()
      {
         using (_target.EnterUpgradeableReadLock())
         {
            while (!_releaseUpgradeableLock)
            {
               if (_teardownThread)
               {
                  return;
               }

               Thread.Sleep(0);
            }
         }
      }

      protected void SeekWriteLock()
      {
         using (_target.EnterWriteLock())
         {
            while (!_releaseWriteLock)
            {
               if (_teardownThread)
               {
                  return;
               }

               Thread.Sleep(0);
            }
         }
      }
   }

   [TestClass]
   public class When_a_thread_seeks_a_read_lock_while_another_holds_a_read_lock : Multi_Threaded_Non_Recursive_Lock_Context
   {
      protected override void ActMethod()
      {
         var t0 = new Thread(SeekReadLock);
         AddThread(t0);
         t0.Start();

         Thread.Sleep(500);

         var t1 = new Thread(SeekReadLock);
         AddThread(t1);
         t1.Start();

         Thread.Sleep(500);
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckInNonIde)]
      public void It_should_grant_read_locks_to_both()
      {
         TargetDiagnostic.CurrentReadCount.Should().Be(2);
         TargetDiagnostic.WaitingReadCount.Should().Be(0);
         TargetDiagnostic.WaitingUpgradeableCount.Should().Be(0);
         TargetDiagnostic.WaitingWriteCount.Should().Be(0);
      }
   }

   [TestClass]
   public class When_a_thread_seeks_a_read_lock_while_another_holds_a_upgradeable_lock : Multi_Threaded_Non_Recursive_Lock_Context
   {
      protected override void ActMethod()
      {
         var t0 = new Thread(SeekUpgradeableLock);
         AddThread(t0);
         t0.Start();

         Thread.Sleep(500);

         var t1 = new Thread(SeekReadLock);
         AddThread(t1);
         t1.Start();

         Thread.Sleep(500);
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckInNonIde)]
      public void It_should_grant_the_read_lock()
      {
         TargetDiagnostic.CurrentReadCount.Should().Be(1);
         TargetDiagnostic.WaitingReadCount.Should().Be(0);
         TargetDiagnostic.WaitingUpgradeableCount.Should().Be(0);
         TargetDiagnostic.WaitingWriteCount.Should().Be(0);
      }
   }

   [TestClass]
   public class When_a_thread_seeks_a_read_lock_while_another_holds_a_write_lock : Multi_Threaded_Non_Recursive_Lock_Context
   {
      protected override void ActMethod()
      {
         var t0 = new Thread(SeekWriteLock);
         AddThread(t0);
         t0.Start();

         Thread.Sleep(500);

         var t1 = new Thread(SeekReadLock);
         AddThread(t1);
         t1.Start();

         Thread.Sleep(500);
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckInNonIde)]
      public void It_should_delay_the_read_lock()
      {
         TargetDiagnostic.CurrentReadCount.Should().Be(0);
         TargetDiagnostic.WaitingReadCount.Should().Be(1);
         TargetDiagnostic.WaitingUpgradeableCount.Should().Be(0);
         TargetDiagnostic.WaitingWriteCount.Should().Be(0);
      }
   }

   [TestClass]
   public class When_a_thread_seeks_an_upgradeable_lock_while_another_holds_a_read_lock : Multi_Threaded_Non_Recursive_Lock_Context
   {
      protected override void ActMethod()
      {
         var t0 = new Thread(SeekReadLock);
         AddThread(t0);
         t0.Start();

         Thread.Sleep(500);

         var t1 = new Thread(SeekUpgradeableLock);
         AddThread(t1);
         t1.Start();

         Thread.Sleep(500);
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckInNonIde)]
      public void It_should_grant_the_upgradeable_lock()
      {
         TargetDiagnostic.CurrentReadCount.Should().Be(1);
         TargetDiagnostic.WaitingReadCount.Should().Be(0);
         TargetDiagnostic.WaitingUpgradeableCount.Should().Be(0);
         TargetDiagnostic.WaitingWriteCount.Should().Be(0);
      }
   }

   [TestClass]
   public class When_a_thread_seeks_an_upgradeable_lock_while_another_holds_a_upgradeable_lock : Multi_Threaded_Non_Recursive_Lock_Context
   {
      protected override void ActMethod()
      {
         var t0 = new Thread(SeekUpgradeableLock);
         AddThread(t0);
         t0.Start();

         Thread.Sleep(500);

         var t1 = new Thread(SeekUpgradeableLock);
         AddThread(t1);
         t1.Start();

         Thread.Sleep(500);
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckInNonIde)]
      public void It_delay_the_second_upgradeable_lock()
      {
         TargetDiagnostic.CurrentReadCount.Should().Be(0);
         TargetDiagnostic.WaitingReadCount.Should().Be(0);
         TargetDiagnostic.WaitingUpgradeableCount.Should().Be(1);
         TargetDiagnostic.WaitingWriteCount.Should().Be(0);
      }
   }

   [TestClass]
   public class When_a_thread_seeks_an_upgradeable_lock_while_another_holds_a_write_lock : Multi_Threaded_Non_Recursive_Lock_Context
   {
      protected override void ActMethod()
      {
         var t0 = new Thread(SeekWriteLock);
         AddThread(t0);
         t0.Start();

         Thread.Sleep(500);

         var t1 = new Thread(SeekUpgradeableLock);
         AddThread(t1);
         t1.Start();

         Thread.Sleep(500);
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckInNonIde)]
      public void It_should_delay_the_upgradeable_lock()
      {
         TargetDiagnostic.CurrentReadCount.Should().Be(0);
         TargetDiagnostic.WaitingReadCount.Should().Be(0);
         TargetDiagnostic.WaitingUpgradeableCount.Should().Be(1);
         TargetDiagnostic.WaitingWriteCount.Should().Be(0);
      }
   }

   [TestClass]
   public class When_a_thread_seeks_a_write_lock_while_another_holds_a_read_lock : Multi_Threaded_Non_Recursive_Lock_Context
   {
      protected override void ActMethod()
      {
         var t0 = new Thread(SeekReadLock);
         AddThread(t0);
         t0.Start();

         Thread.Sleep(500);

         var t1 = new Thread(SeekWriteLock);
         AddThread(t1);
         t1.Start();

         Thread.Sleep(500);
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckInNonIde)]
      public void It_should_delay_the_write_lock()
      {
         TargetDiagnostic.CurrentReadCount.Should().Be(1);
         TargetDiagnostic.WaitingReadCount.Should().Be(0);
         TargetDiagnostic.WaitingUpgradeableCount.Should().Be(0);
         TargetDiagnostic.WaitingWriteCount.Should().Be(1);
      }
   }

   [TestClass]
   public class When_a_thread_seeks_a_write_lock_while_another_holds_a_upgradeable_lock : Multi_Threaded_Non_Recursive_Lock_Context
   {
      protected override void ActMethod()
      {
         var t0 = new Thread(SeekUpgradeableLock);
         AddThread(t0);
         t0.Start();

         Thread.Sleep(500);

         var t1 = new Thread(SeekWriteLock);
         AddThread(t1);
         t1.Start();

         Thread.Sleep(500);
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckInNonIde)]
      public void It_should_delay_the_write_lock()
      {
         TargetDiagnostic.CurrentReadCount.Should().Be(0);
         TargetDiagnostic.WaitingReadCount.Should().Be(0);
         TargetDiagnostic.WaitingUpgradeableCount.Should().Be(0);
         TargetDiagnostic.WaitingWriteCount.Should().Be(1);
      }
   }

   [TestClass]
   public class When_a_thread_seeks_a_write_lock_while_another_holds_a_write_lock : Multi_Threaded_Non_Recursive_Lock_Context
   {
      protected override void ActMethod()
      {
         var t0 = new Thread(SeekWriteLock);
         AddThread(t0);
         t0.Start();

         Thread.Sleep(500);

         var t1 = new Thread(SeekWriteLock);
         AddThread(t1);
         t1.Start();

         Thread.Sleep(500);
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckInNonIde)]
      public void It_should_delay_the_write_lock()
      {
         TargetDiagnostic.CurrentReadCount.Should().Be(0);
         TargetDiagnostic.WaitingReadCount.Should().Be(0);
         TargetDiagnostic.WaitingUpgradeableCount.Should().Be(0);
         TargetDiagnostic.WaitingWriteCount.Should().Be(1);
      }
   }
}
