using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Ioc.Logging.Internal.Interfaces
{
   internal interface IIocLoggerManager
   {
      IIocLogger<TClass> GetLogger<TClass>();
   }
}
