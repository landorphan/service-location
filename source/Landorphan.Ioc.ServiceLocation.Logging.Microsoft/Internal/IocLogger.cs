namespace Landorphan.Ioc.ServiceLocation.Logging.Microsoft.Internal
{
    using System.Diagnostics.CodeAnalysis;
    using global::Microsoft.Extensions.Logging;
    using Landorphan.Ioc.Logging;

    internal class IocLogger<T> : IIocLogger<T>
    {
        private readonly ILogger<T> internalLoger;

        public IocLogger(ILogger<T> extensionsLogger)
        {
            internalLoger = extensionsLogger;
        }
        
        [SuppressMessage("Microsoft.Usage", "CA2254: Template should be a static expression", Justification = "New rule, old code (MWP).")]
        [SuppressMessage("Microsoft.Performance", "CA1848: Use the LoggerMessage delegates", Justification = "New rule, old code (MWP).")]
        public void LogInformation(int eventId, string message, params object[] args)
        {
            internalLoger.LogInformation(eventId, message, args);
        }
    }
}
