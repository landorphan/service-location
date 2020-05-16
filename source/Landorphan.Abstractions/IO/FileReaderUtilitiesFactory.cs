namespace Landorphan.Abstractions.IO
{
    using Landorphan.Abstractions.IO.Interfaces;

    /// <summary>
   /// Factory for instantiating <see cref="IFileUtilities"/> instances.
   /// </summary>
   public sealed class FileReaderUtilitiesFactory : IFileReaderUtilitiesFactory
   {
       /// <inheritdoc/>
      public IFileReaderUtilities Create()
      {
         return new FileReaderUtilities();
      }
   }
}
