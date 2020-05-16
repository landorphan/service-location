namespace Landorphan.TestUtilities.NUnit.Tests.Architecture
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using global::NUnit.Framework;
    using Landorphan.Common.Exceptions;
    using Landorphan.TestUtilities.ReusableTestImplementations;

    // ReSharper disable InconsistentNaming

   [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
   [TestFixture]
   public class Exception_Tests : ExceptionValidityRequirements
   {
       [Test]
      [Category(TestTiming.CheckIn)]
      public void Exceptions_In_DotNet_Core_Should_Not_Be_Marked_As_Serializable()
      {
         Exceptions_In_DotNet_Core_Should_Not_Be_Marked_As_Serializable_Implementation();
      }

      [Test]
      [Category(TestTiming.CheckIn)]
      public void Exceptions_Should_Be_Abstract_Or_Sealed()
      {
         Exceptions_Should_Be_Abstract_Or_Sealed_Implementation();
      }

      [Test]
      [Category(TestTiming.CheckIn)]
      public void Exceptions_Should_Be_Public()
      {
         Exceptions_Should_Be_Public_Implementation();
      }

      [Test]
      [Category(TestTiming.CheckIn)]
      public void Exceptions_Should_Descend_From_An_Acceptable_Base()
      {
         Exceptions_Should_Descend_From_An_Acceptable_Base_Implementation();
      }

      [Test]
      [Category(TestTiming.CheckIn)]
      public void Exceptions_Should_Have_A_Default_Constructor()
      {
         Exceptions_Should_Have_A_Default_Constructor_Implementation();
      }

      [Test]
      [Category(TestTiming.CheckIn)]
      public void Exceptions_Should_Have_A_Message_And_Inner_Exception_Constructor()
      {
         Exceptions_Should_Have_A_Message_And_Inner_Exception_Constructor_Implementation();
      }

      [Test]
      [Category(TestTiming.CheckIn)]
      public void Exceptions_Should_Have_A_Message_Constructor()
      {
         Exceptions_Should_Have_A_Message_Constructor_Implementation();
      }

      [Test]
      [Category(TestTiming.CheckIn)]
      public void Exceptions_Should_Have_A_Serialization_Constructor()
      {
         Exceptions_Should_Have_A_Serialization_Constructor_Implementation();
      }

      [Test]
      [Category(TestTiming.CheckIn)]
      public void Exceptions_Should_Have_Valid_Other_Public_Constructors_When_Present()
      {
         Exceptions_Should_Have_Valid_Other_Public_Constructors_When_Present_Implementation();
      }

      protected override IImmutableSet<Type> GetAcceptableBaseExceptionTypes()
      {
         var builder = ImmutableHashSet<Type>.Empty.ToBuilder();
         builder.Add(typeof(LandorphanArgumentException));
         builder.Add(typeof(LandorphanException));
         return builder.ToImmutable();
      }

      protected override IImmutableSet<Assembly> GetAssembliesUnderTest()
      {
         return TestAssemblyInitializeCleanup.AssembliesUnderTest;
      }
   }
}
