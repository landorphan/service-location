namespace Landorphan.Common.Collections
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Provides the base interface for the abstraction of mutation operations on stacks.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IMutableStackEnumerable<T> : IClearEnumerable<T>,
      IPopEnumerable<T>,
      IPushEnumerable<T>,
      IQueryIgnoresNullValues,
      IQueryThreadSafe
   {
   }
}