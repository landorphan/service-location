namespace Landorphan.Abstractions.IO.Interfaces
{
   /// <summary>
   /// Factory for instantiating <see cref="IFileUtilities"/> instances.
   /// </summary>
   public interface IFileUtilitiesFactory
   {
       /// <summary>
      /// Creates a <see cref="IFileUtilities"/> instance.
      /// </summary>
      /// <returns>
      /// A non-null <see cref="IFileUtilities"/> instance.
      /// </returns>
      IFileUtilities Create();
   }
}
