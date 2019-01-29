namespace Landorphan.Abstractions.IO
{
   using Landorphan.Abstractions.IO.Interfaces;

   /// <summary>
   /// Factory for instantiating <see cref="IDirectoryUtilities"/> instances.
   /// </summary>
   public sealed class DirectoryWriterUtilitiesFactory : IDirectoryWriterUtilitiesFactory
   {
      /// <inheritdoc/>
      public IDirectoryWriterUtilities Create()
      {
         return new DirectoryWriterUtilities();
      }
   }
}
