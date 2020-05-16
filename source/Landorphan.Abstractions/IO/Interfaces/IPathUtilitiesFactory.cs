namespace Landorphan.Abstractions.IO.Interfaces
{
   /// <summary>
   /// Factory for instantiating <see cref="IPathUtilities"/> instances.
   /// </summary>
   public interface IPathUtilitiesFactory
   {
       /// <summary>
      /// Creates a <see cref="IPathUtilities"/> instance.
      /// </summary>
      /// <returns>
      /// A non-null <see cref="IPathUtilities"/> instance.
      /// </returns>
      IPathUtilities Create();
   }
}
