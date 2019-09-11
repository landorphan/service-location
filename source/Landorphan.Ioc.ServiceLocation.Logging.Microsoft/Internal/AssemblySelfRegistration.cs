using Landorphan.Ioc.ServiceLocation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Ioc.ServiceLocation.Logging.Microsoft.Internal
{
   using global::Microsoft.Extensions.Logging;
   using Landorphan.Ioc.Logging;

   internal class AssemblySelfRegistration : IAssemblySelfRegistration
   {
      public void RegisterServiceInstances(IIocContainerRegistrar registrar)
      {
         registrar.RegisterImplementation<ILoggerFactory, LoggerFactory>();
         registrar.RegisterImplementation<IIocLoggerFactory, IocLoggerFactory>();
      }
   }
}
