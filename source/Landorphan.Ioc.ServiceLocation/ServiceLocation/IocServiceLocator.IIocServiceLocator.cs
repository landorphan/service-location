namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Reflection;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   public sealed partial class IocServiceLocator : IIocServiceLocator, IIocServiceLocatorManager
   {
      public string _aproperty { get; set; }
      /// <inheritdoc/>
      Object IServiceProvider.GetService(Type serviceType)
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
      TFrom IIocServiceLocator.Resolve<TFrom>(String name)
      {
         return _ambientContainer.Resolver.Resolve<TFrom>(name);
      }

      /// <inheritdoc/>
      Object IIocServiceLocator.Resolve(Type fromType)
      {
         return _ambientContainer.Resolver.Resolve(fromType);
      }

      /// <inheritdoc/>
      Object IIocServiceLocator.Resolve(Type fromType, String name)
      {
         return _ambientContainer.Resolver.Resolve(fromType, name);
      }

      /// <inheritdoc/>
      Boolean IIocServiceLocator.TryResolve<TFrom>(out TFrom instance)
      {
         return _ambientContainer.Resolver.TryResolve(out instance);
      }

      /// <inheritdoc/>
      Boolean IIocServiceLocator.TryResolve<TFrom>(String name, out TFrom instance)
      {
         return _ambientContainer.Resolver.TryResolve(name, out instance);
      }

      /// <inheritdoc/>
      Boolean IIocServiceLocator.TryResolve(Type fromType, out Object instance)
      {
         return _ambientContainer.Resolver.TryResolve(fromType, out instance);
      }

      /// <inheritdoc/>
      Boolean IIocServiceLocator.TryResolve(Type fromType, String name, out Object instance)
      {
         return _ambientContainer.Resolver.TryResolve(fromType, name, out instance);
      }
   }
}
