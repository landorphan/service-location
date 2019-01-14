namespace Landorphan.Common.Collections
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Represents a variable-sized last-in-first-out (LIFO) collection of instances of the same arbitrary type.
   /// </summary>
   /// <typeparam name="T">
   /// Specifies the type of elements in the stack.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   [SuppressMessage("Microsoft.Naming", "CA1711: Identifiers should not have incorrect suffix")]
   public interface IStack<T> : IReadableStackEnumerable<T>, IMutableStackEnumerable<T>
   {
   }
}
