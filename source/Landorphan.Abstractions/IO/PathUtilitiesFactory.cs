namespace Landorphan.Abstractions.IO
{
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Abstractions.IO.Internal;

    /// <summary>
    /// Factory for instantiating <see cref="IPathUtilities"/> instances.
    /// </summary>
    public sealed class PathUtilitiesFactory : IPathUtilitiesFactory
    {
        /// <inheritdoc/>
        public IPathUtilities Create()
        {
            return new PathInternalMapping();
        }
    }
}
