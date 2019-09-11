using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Ioc.Logging.Internal
{
   using Landorphan.Ioc.Logging.Internal.Interfaces;
   using Landorphan.Ioc.ServiceLocation;

   internal class IocLoggerManager : IIocLoggerManager
   {
      private readonly Dictionary<Type, object> loggers = new Dictionary<Type, object>();

      public IIocLogger<TClass> GetLogger<TClass>()
      {
         if (!loggers.TryGetValue(typeof(TClass), out object logger) && IocServiceLocator.Instance.TryResolve<IIocLoggerFactory>(out var factory))
         {
            logger = factory.CreateLogger<TClass>();
            loggers.Add(typeof(TClass), logger);
         }

         return (IIocLogger<TClass>) logger;
      }
   }
}
