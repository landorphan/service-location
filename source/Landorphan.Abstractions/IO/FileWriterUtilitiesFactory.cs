namespace Landorphan.Abstractions.IO
{
   using Landorphan.Abstractions.IO.Interfaces;

   /// <summary>
   /// Factory for instantiating <see cref="IFileWriterUtilities"/> instances.
   /// </summary>
   public sealed class FileWriterUtilitiesFactory : IFileWriterUtilitiesFactory
   {
      /// <inheritdoc/>
      public IFileWriterUtilities Create()
      {
         return new FileWriterUtilities();
      }
   }
}
