namespace Landorphan.Ioc.ServiceLocation.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Landorphan.Common;
    using Landorphan.Ioc.ServiceLocation.Exceptions;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    // ReSharper disable ConvertToAutoProperty
   // ReSharper disable InheritdocConsiderUsage
   // ReSharper disable RedundantExtendsListEntry

   internal sealed partial class IocContainer : DisposableObject, IOwnedIocContainer, IIocContainerManager, IIocContainerRegistrar, IIocContainerResolver
   {
       /// <inheritdoc/>
      TFrom IIocContainerResolver.Resolve<TFrom>()
      {
         return ((IIocContainerResolver)this).Resolve<TFrom>(null);
      }

       /// <inheritdoc/>
      TFrom IIocContainerResolver.Resolve<TFrom>(string name)
      {
         ResolveImplementation(typeof(TFrom), nameof(TFrom), name, false, out var instance);
         return (TFrom)instance;
      }

       /// <inheritdoc/>
      object IIocContainerResolver.Resolve(Type fromType)
      {
         return ((IIocContainerResolver)this).Resolve(fromType, null);
      }

       /// <inheritdoc/>
      object IIocContainerResolver.Resolve(Type fromType, string name)
      {
         ResolveImplementation(fromType, nameof(fromType), name, false, out var instance);
         return instance;
      }

       /// <inheritdoc/>
      public bool TryResolve<TFrom>(out TFrom instance) where TFrom : class
      {
         return TryResolve(null, out instance);
      }

       /// <inheritdoc/>
      public bool TryResolve<TFrom>(string name, out TFrom instance) where TFrom : class
      {
         var rv = ResolveImplementation(typeof(TFrom), nameof(TFrom), name, true, out var obj);
         instance = (TFrom)obj;
         return rv;
      }

       /// <inheritdoc/>
      public bool TryResolve(Type fromType, out object instance)
      {
         return TryResolve(fromType, null, out instance);
      }

       /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S1067:Expressions should not be too complex", Justification = "Extending the type system, it is complex")]
      public bool TryResolve(Type fromType, string name, out object instance)
      {
         var rv = ResolveImplementation(fromType, nameof(fromType), name, true, out var obj);
         instance = obj;
         return rv;
      }

       [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      private bool ResolveImplementation(Type fromType, string fromTypeParameterName, string name, bool tryLogic, out object instance)
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
         if (_precludedTypes.Contains(fromType))
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

         CheckForNewRegistrations();
         var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);
         RegistrationValueTypeInstancePair value;
         bool found;
         using (_registrationsLock.EnterReadLock())
         {
            found = _registrations.TryGetValue(key, out value);
         }

         if (found)
         {
            if (value.ImplementationInstance != null)
            {
               instance = value.ImplementationInstance;
               return true;
            }

            // lazily construct registered instance using default ctor
            // ReSharper disable once AssignNullToNotNullAttribute
            var newInstance = Activator.CreateInstance(value.ImplementationType);
            var replacementValue = new RegistrationValueTypeInstancePair(value.ImplementationType, newInstance);
            using (_registrationsLock.EnterWriteLock())
            {
               _registrations = _registrations.SetItem(key, replacementValue);
            }

            instance = newInstance;
            return true;
         }

         // no matching entry here, check ancestors...
         var parent = _parent;
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
