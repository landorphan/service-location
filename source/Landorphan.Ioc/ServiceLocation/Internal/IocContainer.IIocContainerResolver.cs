namespace Landorphan.Ioc.ServiceLocation.Internal
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using Landorphan.Common;

   public sealed partial class IocContainer : DisposableObject, IOwnedIocContainer, IIocContainerManager, IIocContainerRegistrar, IIocContainerResolver
   {
      /// <inheritdoc/>
      TFrom IIocContainerResolver.Resolve<TFrom>()
      {
         return ((IIocContainerResolver)this).Resolve<TFrom>(null);
      }

      /// <inheritdoc/>
      TFrom IIocContainerResolver.Resolve<TFrom>(String name)
      {
         ResolveImplementation(typeof(TFrom), nameof(TFrom), name, false, out var instance);
         return (TFrom)instance;
      }

      /// <inheritdoc/>
      Object IIocContainerResolver.Resolve(Type fromType)
      {
         return ((IIocContainerResolver)this).Resolve(fromType, null);
      }

      /// <inheritdoc/>
      Object IIocContainerResolver.Resolve(Type fromType, String name)
      {
         ResolveImplementation(fromType, nameof(fromType), name, false, out var instance);
         return instance;
      }

      /// <inheritdoc/>
      public Boolean TryResolve<TFrom>(out TFrom instance) where TFrom : class
      {
         return TryResolve(null, out instance);
      }

      /// <inheritdoc/>
      public Boolean TryResolve<TFrom>(String name, out TFrom instance) where TFrom : class
      {
         var rv = ResolveImplementation(typeof(TFrom), nameof(TFrom), name, true, out var obj);
         instance = (TFrom)obj;
         return rv;
      }

      /// <inheritdoc/>
      public Boolean TryResolve(Type fromType, out Object instance)
      {
         return TryResolve(fromType, null, out instance);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S1067:Expressions should not be too complex", Justification = "Extending the type system, it is complex")]
      public Boolean TryResolve(Type fromType, String name, out Object instance)
      {
         var rv = ResolveImplementation(fromType, nameof(fromType), name, true, out var obj);
         instance = obj;
         return rv;
      }

      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      private Boolean ResolveImplementation(Type fromType, String fromTypeParameterName, String name, Boolean tryLogic, out Object instance)
      {
         instance = null;

         // fromType: not null
         if (fromType == null)
         {
            if (tryLogic)
            {
               return false;
            }

            throw new ArgumentNullException(fromTypeParameterName);
         }

         // fromType: not open generic
         if (fromType.ContainsGenericParameters)
         {
            if (tryLogic)
            {
               return false;
            }

            throw new TypeMustNotBeAnOpenGenericArgumentException(fromType, fromTypeParameterName, null, null);
         }

         // fromType: is interface or abstract type
         if (!(fromType.IsInterface || fromType.IsAbstract))
         {
            if (tryLogic)
            {
               return false;
            }

            throw new FromTypeMustBeInterfaceOrAbstractTypeArgumentException(fromType, fromTypeParameterName, null, null);
         }

         // fromType: not precluded
         if (Manager.PrecludedTypes.Contains(fromType))
         {
            if (tryLogic)
            {
               return false;
            }

            throw new ContainerFromTypePrecludedArgumentException(Manager, fromType, fromTypeParameterName);
         }

         // name:  clean and names allowed
         var cleanedName = name.TrimNullToEmpty();
         if (cleanedName.Length > 0 && !_configuration.AllowNamedImplementations)
         {
            if (tryLogic)
            {
               return false;
            }

            throw new ContainerConfigurationNamedImplementationsDisabledException(this, null, null);
         }

         var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);
         CheckForNewRegistrations();
         if (_registrations.TryGetValue(key, out var value))
         {
            if (value.ImplementationInstance != null)
            {
               instance = value.ImplementationInstance;
               return true;
            }

            // lazily construct registered instance using default ctor
            var newInstance = Activator.CreateInstance(value.ImplementationType);
            var replacementValue = new RegistrationValueTypeInstancePair(value.ImplementationType, newInstance);
            _registrations = _registrations.SetItem(key, replacementValue);
            instance = newInstance;
            return true;
         }

         // no matching entry here, check ancestors...
         var parent = this._parent;
         if (parent != null)
         {
            return parent.ResolveImplementation(fromType, fromTypeParameterName, name, tryLogic, out instance);
         }

         // no matching entry here nor in ancestor chain, throw...
         if (tryLogic)
         {
            return false;
         }

         throw new ResolutionException(fromType, cleanedName);
      }
   }
}