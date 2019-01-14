namespace Landorphan.Common.Collections
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes the Object is an unordered collection.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the collection.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IBag<T> : IReadableBagEnumerable<T>, IMutableBagEnumerable<T>
   {
   }
}
