namespace Landorphan.Common.Tests
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class DisposableObjectTests : ArrangeActAssert
   {
      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_dispose_of_all_disposable_fields_and_contained_disposables()
      {
         var target = DisposableHelper.SafeCreate<DisposableEnumerable>();
         var contained = DisposableHelper.SafeCreate(() => new DisposableItem());
         target.AddDisposable(contained);

         target.Dispose();

         contained.IsDisposed.Should().BeTrue();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_handle_contained_nulls()
      {
         var target = DisposableHelper.SafeCreate<DisposableEnumerable>();
         target.AddDisposable(null);

         target.Dispose();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_handle_inheritance()
      {
         var target = DisposableHelper.SafeCreate<DisposableDescendant>();
         var item0 = DisposableHelper.SafeCreate(() => new DisposableItem());
         target.SetFlatField(item0);

         var item1 = DisposableHelper.SafeCreate(() => new DisposableItem());
         target.SetAnotherField(item1);

         target.Dispose();

         item1.IsDisposed.Should().BeTrue();
         item0.IsDisposed.Should().BeTrue();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_handle_null_fields()
      {
         var target = DisposableHelper.SafeCreate<DisposableEnumerable>();
         target.SetFlatField(null);

         target.Dispose();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_throw_when_disposed()
      {
         var target = DisposableHelper.SafeCreate<DisposableItem>();
         target.Dispose();

         // ReSharper disable once AccessToDisposedClosure
         Action throwingAction = target.Method;
         throwingAction.Should().Throw<ObjectDisposedException>();
      }

      private class DisposableDescendant : DisposableEnumerable
      {
         // ReSharper disable once NotAccessedField.Local
         private IDisposable anotherField;

         public void SetAnotherField(IDisposable value)
         {
            anotherField = value;
         }
      }

      private class DisposableEnumerable : DisposableObject, IEnumerable<IDisposable>
      {
         private readonly List<IDisposable> listOfDisposables = new List<IDisposable>();

         // ReSharper disable once NotAccessedField.Local
         private IDisposable flatField;

         public IEnumerator<IDisposable> GetEnumerator()
         {
            return listOfDisposables.GetEnumerator();
         }

         IEnumerator IEnumerable.GetEnumerator()
         {
            return GetEnumerator();
         }

         public void AddDisposable(IDisposable item)
         {
            ThrowIfDisposed();

            listOfDisposables.Add(item);
         }

         public void SetFlatField(IDisposable value)
         {
            flatField = value;
         }
      }

      private class DisposableItem : DisposableObject
      {
         public void Method()
         {
            ThrowIfDisposed();
         }
      }
   }
}
