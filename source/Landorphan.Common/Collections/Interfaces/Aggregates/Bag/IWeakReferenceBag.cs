namespace Landorphan.Common.Collections
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Represents a thread-safe collection of weak references that ignores duplicates and nulls.
   /// </summary>
   /// <typeparam name="T">
   /// The type of the elements.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IWeakReferenceBag<T> : IReadableBagEnumerable<T>, IMutableBagEnumerable<T> where T : class
   {
   }
}
