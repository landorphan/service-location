namespace Landorphan.Common.Tests.Helpers
{
   using System;
   using FluentAssertions;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   [TestClass]
   public class When_I_Have_A_SupportsReadOnlyHelper_And_Do_Not_Call_MakeReadOnly : ArrangeActAssert
   {
      private SupportsReadOnlyHelper target;

      protected override void ArrangeMethod()
      {
         target = new SupportsReadOnlyHelper();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_Not_Be_Read_Only()
      {
         target.IsReadOnly.Should().BeFalse();
      }
   }

   [TestClass]
   public class When_I_Have_A_SupportsReadOnlyHelper_And_Call_MakeReadOnly : ArrangeActAssert
   {
      private SupportsReadOnlyHelper target;

      protected override void ArrangeMethod()
      {
         target = new SupportsReadOnlyHelper();
      }

      protected override void ActMethod()
      {
         target.MakeReadOnly();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_Be_Read_Only()
      {
         target.IsReadOnly.Should().BeTrue();
      }
   }

   [TestClass]
   public class When_I_Have_A_SupportsReadOnlyHelper_And_Call_MakeReadOnly_Multiple_Times : ArrangeActAssert
   {
      private SupportsReadOnlyHelper target;

      protected override void ArrangeMethod()
      {
         target = new SupportsReadOnlyHelper();
      }

      protected override void ActMethod()
      {
         target.MakeReadOnly();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_Be_Read_Only()
      {
         target.IsReadOnly.Should().BeTrue();
         target.MakeReadOnly();
         target.IsReadOnly.Should().BeTrue();
      }
   }

   [TestClass]
   public class When_I_Have_A_SupportsReadOnlyHelper_And_Call_ThrowIfReadOnlyInstance_And_It_Is_Not_Read_Only : ArrangeActAssert
   {
      private SupportsReadOnlyHelper target;

      protected override void ArrangeMethod()
      {
         target = new SupportsReadOnlyHelper();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_Not_Throw()
      {
         target.ThrowIfReadOnlyInstance();
      }
   }

   [TestClass]
   public class When_I_Have_A_SupportsReadOnlyHelper_And_Call_ThrowIfReadOnlyInstance_And_It_Is_Read_Only : ArrangeActAssert
   {
      private SupportsReadOnlyHelper target;

      protected override void ArrangeMethod()
      {
         target = new SupportsReadOnlyHelper();
      }

      protected override void ActMethod()
      {
         target.MakeReadOnly();
      }

      [TestMethod]
      [TestCategory(TestTiming.CheckIn)]
      public void It_should_throw()
      {
         Action throwingAction = () => target.ThrowIfReadOnlyInstance();
         throwingAction.Should().Throw<NotSupportedException>().WithMessage("*read-only*");
      }
   }
}