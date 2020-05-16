namespace Landorphan.Ioc.ServiceLocation.Logging.Microsoft.Internal
{
    using global::Microsoft.Extensions.Logging;
    using Landorphan.Ioc.Logging;

    internal class IocLogger<T> : IIocLogger<T>
    {
        private readonly ILogger<T> internalLoger;

        public IocLogger(ILogger<T> extensionsLogger)
        {
            internalLoger = extensionsLogger;
        }

        public void LogInformation(int eventId, string message, params object[] args)
        {
            internalLoger.LogInformation(eventId, message, args);
        }
    }
}
