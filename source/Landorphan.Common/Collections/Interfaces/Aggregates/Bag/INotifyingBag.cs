namespace Landorphan.Common.Collections
{
   using System.Collections.Specialized;
   using System.ComponentModel;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes the Object is an unordered collection that notifies listeners of dynamic change.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the collection.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface INotifyingBag<T> : IBag<T>, INotifyCollectionChanged, INotifyPropertyChanged
   {
   }
}
