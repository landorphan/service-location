namespace Landorphan.Common.Collections
{
   using System;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Represents a queue collection of instances of the same arbitrary type for which items are retrieved in priority order.
   /// </summary>
   /// <typeparam name="T">
   /// Specifies the type of elements in the queue.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Naming", "CA1711: Identifiers should not have incorrect suffix")]
   public interface IInlinePriorityQueue<T> : IQueue<T>, IAddRangeEnumerable<T>
   {
      /// <summary>
      /// The comparison used to determine priority.
      /// </summary>
      /// <remarks>
      /// Priority is inverse to comparison. Items that are less than other items have higher priority.
      /// </remarks>
      /// <value>
      /// The priority comparison.
      /// </value>
      Comparison<T> PriorityComparison { get; }

      /// <summary>
      /// Reschedules the priority of the given item.
      /// </summary>
      /// <remarks>
      /// Reference equality is used to find the item.
      /// </remarks>
      /// <param name="item">
      /// The item to reschedule.
      /// </param>
      void Reschedule(T item);

      /// <summary>
      /// Reschedules the priority of all items in the queue.
      /// </summary>
      void RescheduleAll();
   }
}
