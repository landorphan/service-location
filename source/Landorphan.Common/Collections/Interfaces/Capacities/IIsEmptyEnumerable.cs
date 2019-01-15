namespace Landorphan.Common.Collections
{
   using System;
   using System.Collections;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes an enumerable that supports <see cref="IsEmpty"/> .
   /// </summary>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Design", "CA1010: Collections should implement generic interface", Justification = "This represents a capacity, not a collection.")]
   public interface IIsEmptyEnumerable : IEnumerable
   {
      /// <summary>
      /// Gets a value indicating whether the <see cref="IEnumerable"/> is empty.
      /// </summary>
      /// <value>
      /// true if this Object is empty, false if not.
      /// </value>
      Boolean IsEmpty { get; }
   }
}
