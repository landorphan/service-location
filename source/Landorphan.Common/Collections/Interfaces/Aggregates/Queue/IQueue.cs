namespace Landorphan.Common.Collections
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Represents a variable size first-in-first-out (FIFO) collection of instances of the same arbitrary type.
   /// </summary>
   /// <typeparam name="T">
   /// Specifies the type of elements in the queue.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Naming", "CA1711: Identifiers should not have incorrect suffix")]
   public interface IQueue<T> : IMutableQueueEnumerable<T>, IReadableQueueEnumerable<T>
   {
   }
}