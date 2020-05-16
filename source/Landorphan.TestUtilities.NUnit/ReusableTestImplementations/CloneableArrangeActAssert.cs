namespace Landorphan.TestUtilities.ReusableTestImplementations
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;

    // ReSharper disable  InconsistentNaming

   /// <summary>
   /// Implements test of <see cref="ICloneable.Clone"/>.
   /// </summary>
   /// <typeparam name="T">
   /// The type being tested.
   /// </typeparam>
   public abstract class CloneableArrangeActAssert<T> : ArrangeActAssert where T : ICloneable
   {
       /// <summary>
      /// Descendants should assign a value before calling the test implementation.
      /// </summary>
      /// <value>
      /// The target.
      /// </value>
      protected abstract T Target { get; set; }

       /// <summary>
      /// Implementation of the test of <see cref="ICloneable.Clone"/>.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void It_Should_Clone_Correctly_Implementation()
      {
         var actualObject = Target.Clone();
         actualObject.Should().NotBeNull();
         actualObject.Should().BeAssignableTo<T>();
         actualObject.Should().NotBeSameAs(Target);

         var actualAsIEquatable = actualObject as IEquatable<T>;
         actualAsIEquatable?.Equals(Target).Should().BeTrue();
         actualAsIEquatable?.Should().NotBeSameAs(Target);
      }
   }
}
