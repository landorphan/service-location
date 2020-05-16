namespace Landorphan.Ioc.ServiceLocation.Logging.Microsoft.Internal
{
    using global::Microsoft.Extensions.Logging;
    using Landorphan.Ioc.Logging;

    internal class IocLoggerFactory : IIocLoggerFactory
   {
       public IIocLogger<TClass> CreateLogger<TClass>()
      {
         var factory = IocServiceLocator.Resolve<ILoggerFactory>();
         return new IocLogger<TClass>(factory.CreateLogger<TClass>());
      }
   }
}
