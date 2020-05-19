namespace Landorphan.Ioc.Logging.Internal
{
    internal interface IIocLoggerManager
    {
        IIocLogger<TClass> GetLogger<TClass>();
    }
}
