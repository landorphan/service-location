namespace Landorphan.Abstractions.IO.Interfaces
{
   /// <summary>
   /// Factory for instantiating <see cref="IFileWriterUtilities"/> instances.
   /// </summary>
   public interface IFileWriterUtilitiesFactory
   {
      /// <summary>
      /// Creates a <see cref="IFileWriterUtilities"/> instance.
      /// </summary>
      /// <returns>
      /// A non-null <see cref="IFileWriterUtilities"/> instance.
      /// </returns>
      IFileWriterUtilities Create();
   }
}
