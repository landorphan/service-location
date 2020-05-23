namespace Landorphan.Ioc.Logging.Internal
{
    using System;
    using System.Collections.Generic;
    using Landorphan.Ioc.ServiceLocation;

    internal class IocLoggerManager : IIocLoggerManager
    {
        private readonly Dictionary<Type, object> loggers = new Dictionary<Type, object>();

        public IIocLogger<TClass> GetLogger<TClass>()
        {
            if (!loggers.TryGetValue(typeof(TClass), out var logger) && IocServiceLocator.Instance.TryResolve<IIocLoggerFactory>(out var factory))
            {
                logger = factory.CreateLogger<TClass>();
                loggers.Add(typeof(TClass), logger);
            }

            return (IIocLogger<TClass>)logger;
        }
    }
}
