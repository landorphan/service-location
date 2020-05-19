namespace Landorphan.Ioc.Logging
{
    /// <summary>
    /// Factory interface used to create a logger instance when needed at runtime.
    /// </summary>
    public interface IIocLoggerFactory
    {
        /// <summary>
        /// Creates a new <see cref="IIocLogger{TClass}"/> instance.
        /// </summary>
        /// <typeparam name="TClass">The type to log.  May not be necessary for all logging systems.</typeparam>
        /// <returns>The <see cref="IIocLogger{TClass}"/>.</returns>
        IIocLogger<TClass> CreateLogger<TClass>();
    }
}
