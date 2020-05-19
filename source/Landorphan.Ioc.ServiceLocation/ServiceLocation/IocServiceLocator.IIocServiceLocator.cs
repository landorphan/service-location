namespace Landorphan.Ioc.ServiceLocation
{
    using System;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    public sealed partial class IocServiceLocator : IIocServiceLocator, IIocServiceLocatorManager
    {
        /// <inheritdoc/>
        object IServiceProvider.GetService(Type serviceType)
        {
            if (TryResolve(serviceType, out var instance))
            {
                return instance;
            }

            return null;
        }

        /// <inheritdoc/>
        TFrom IIocServiceLocator.Resolve<TFrom>()
        {
            return _ambientContainer.Resolver.Resolve<TFrom>();
        }

        /// <inheritdoc/>
        TFrom IIocServiceLocator.Resolve<TFrom>(string name)
        {
            return _ambientContainer.Resolver.Resolve<TFrom>(name);
        }

        /// <inheritdoc/>
        object IIocServiceLocator.Resolve(Type fromType)
        {
            return _ambientContainer.Resolver.Resolve(fromType);
        }

        /// <inheritdoc/>
        object IIocServiceLocator.Resolve(Type fromType, string name)
        {
            return _ambientContainer.Resolver.Resolve(fromType, name);
        }

        /// <inheritdoc/>
        bool IIocServiceLocator.TryResolve<TFrom>(out TFrom instance)
        {
            return _ambientContainer.Resolver.TryResolve(out instance);
        }

        /// <inheritdoc/>
        bool IIocServiceLocator.TryResolve<TFrom>(string name, out TFrom instance)
        {
            return _ambientContainer.Resolver.TryResolve(name, out instance);
        }

        /// <inheritdoc/>
        bool IIocServiceLocator.TryResolve(Type fromType, out object instance)
        {
            return _ambientContainer.Resolver.TryResolve(fromType, out instance);
        }

        /// <inheritdoc/>
        bool IIocServiceLocator.TryResolve(Type fromType, string name, out object instance)
        {
            return _ambientContainer.Resolver.TryResolve(fromType, name, out instance);
        }
    }
}
