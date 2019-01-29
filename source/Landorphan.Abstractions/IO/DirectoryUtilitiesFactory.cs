namespace Landorphan.Abstractions.IO
{
   using Landorphan.Abstractions.IO.Interfaces;

   /// <summary>
   /// Factory for instantiating <see cref="IDirectoryUtilities"/> instances.
   /// </summary>
   public sealed class DirectoryUtilitiesFactory : IDirectoryUtilitiesFactory
   {
      /// <inheritdoc/>
      public IDirectoryUtilities Create()
      {
         return new DirectoryUtilities();
      }
   }
}
