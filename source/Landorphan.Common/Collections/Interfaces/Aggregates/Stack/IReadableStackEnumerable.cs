namespace Landorphan.Common.Collections
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Provides the base interface for the abstraction of read operations on stacks.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IReadableStackEnumerable<T> : IReadableBagEnumerable<T>, IPeekEnumerable<T>, IQueryIgnoresNullValues, IQueryThreadSafe
   {
   }
}
