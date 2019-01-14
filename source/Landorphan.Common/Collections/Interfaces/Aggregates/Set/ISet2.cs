namespace Landorphan.Common.Collections
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Represents an unordered collection of unique items.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the set.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface ISet2<T> : IReadableSetEnumerable<T>, IMutableSetEnumerable<T>
   {
   }
}
