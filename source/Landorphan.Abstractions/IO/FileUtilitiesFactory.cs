namespace Landorphan.Abstractions.IO
{
   using Landorphan.Abstractions.IO.Interfaces;

   /// <summary>
   /// Factory for instantiating <see cref="IFileUtilities"/> instances.
   /// </summary>
   public sealed class FileUtilitiesFactory : IFileUtilitiesFactory
   {
      /// <inheritdoc/>
      public IFileUtilities Create()
      {
         return new FileUtilities();
      }
   }
}
