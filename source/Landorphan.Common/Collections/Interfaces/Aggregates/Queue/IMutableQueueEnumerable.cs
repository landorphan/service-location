namespace Landorphan.Common.Collections
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Provides the base interface for the abstraction of read operations on queues.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the enumerable.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface IMutableQueueEnumerable<T> : IClearEnumerable<T>,
      IDequeueEnumerable<T>,
      IEnqueueEnumerable<T>,
      IQueryIgnoresNullValues,
      IQueryThreadSafe
   {
   }
}