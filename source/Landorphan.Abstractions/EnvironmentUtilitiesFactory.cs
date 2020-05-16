namespace Landorphan.Abstractions
{
    using Landorphan.Abstractions.Interfaces;
    using Landorphan.Abstractions.Internal;

    /// <summary>
   /// Factory for instantiating <see cref="IEnvironmentUtilities"/> instances.
   /// </summary>
   public sealed class EnvironmentUtilitiesFactory : IEnvironmentUtilitiesFactory
   {
       /// <inheritdoc/>
      public IEnvironmentUtilities Create()
      {
         return new EnvironmentInternalMapping();
      }
   }
}
