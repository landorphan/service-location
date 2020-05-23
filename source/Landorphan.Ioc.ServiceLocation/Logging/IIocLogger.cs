namespace Landorphan.Ioc.Logging
{
    /// <summary>
    /// Provides an internal logging abstraction to be used by the Service Location system for logging activities.
    /// This is necessary because the IOC system can not depend on and external relationship for logging but expects
    /// any necessary logging infrastructure to be registered with the system.
    ///
    /// This creates a "chicken and the egg" scenario where IOC can't depend on a logging framework (because it doesn't
    /// know what framework will be used) but needs to communicate with a logging framework.
    ///
    /// The design pattern chosen was to make internal abstractions for logging frameworks and allow a separate NuGet
    /// package to provide an implementation.
    /// </summary>
    /// <typeparam name="TClass">The class to log.  May not be needed for all logging systems.</typeparam>
    public interface IIocLogger<TClass>
    {
        /// <summary>
        /// Logs an informational message to the registered logger.
        /// </summary>
        /// <param name="eventId">The event id of the message to log.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="args">Arguments to include in the log message.</param>
        void LogInformation(int eventId, string message, params object[] args);
    }
}
