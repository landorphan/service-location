namespace Landorphan.Common.Collections
{
   using System.Collections.Specialized;
   using System.ComponentModel;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Denotes the Object is a set that notifies listeners of dynamic change.
   /// </summary>
   /// <typeparam name="T">
   /// The type of elements in the set.
   /// </typeparam>
   [SuppressMessage("Microsoft.Naming", "CA1710: IdentifiersShouldHaveCorrectSuffix")]
   public interface INotifyingSet<T> : ISet2<T>, INotifyCollectionChanged, INotifyPropertyChanged
   {
   }
}