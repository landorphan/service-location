namespace Ioc.Collections.Performance.Tests
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Landorphan.Common;
    using Landorphan.Common.Threading;
    using Landorphan.Ioc.ServiceLocation.Exceptions;
    using Landorphan.Ioc.ServiceLocation.Interfaces;
    using Landorphan.Ioc.ServiceLocation.Internal;

    // ReSharper disable IdentifierTypo

   [SuppressMessage("SonarLint.CodeSmell", "S3459: Unassigned members should be removed")]
   [SuppressMessage("SonarLint.CodeSmell", "S3052: Members should not be initialized to default values")]
   [SuppressMessage("SonarLint.CodeSmell", "S1200: Classes should not be coupled to too many other classes (Single Responsibility Principle)")]
   public sealed class ImmutableDictionaryTarget : DisposableObject, IRegistrationTarget
   {
       // this class mimics the implementation of IocContainer so as to allow for performance testing.
       private readonly IocContainerConfiguration _configuration;
       private readonly ImmutableDictionaryTarget _parent = null;
       private readonly NonRecursiveLock _registrationsLock = new NonRecursiveLock();
       private readonly Stopwatch _swPrecludedTypeAdd;
       private readonly Stopwatch _swPrecludedTypeRemove;
       private readonly Stopwatch _swRegister;
       private readonly Stopwatch _swRegisterValidation;
       private readonly Stopwatch _swRegistrationOverwrite;
       private readonly Stopwatch _swResolve;
       private readonly Stopwatch _swResolveOverwrite;
       private readonly Stopwatch _swResolveValidation;
       private readonly Stopwatch _swUnregister;
       private IImmutableSet<Type> _precludedTypes = ImmutableHashSet<Type>.Empty;
       private int _registrationOverwriteCount;
       private IImmutableDictionary<RegistrationKeyTypeNamePair, RegistrationValueTypeInstancePair> _registrations =
         ImmutableDictionary<RegistrationKeyTypeNamePair, RegistrationValueTypeInstancePair>.Empty;
       private int _registrationTotalCount;
       private int _resolutionNewInstancesCount;
       private int _resolutionTotalCount;
       private int _unregistrationTotalCount;

       public ImmutableDictionaryTarget(bool allowNamedImplementations, bool allowPreclusionOfTypes, bool throwOnRegistrationCollision)
      {
         var configuration = new IocContainerConfiguration((IIocContainerMetaIdentity)this)
         {
            AllowNamedImplementations = allowNamedImplementations,
            AllowPreclusionOfTypes = allowPreclusionOfTypes,
            ThrowOnRegistrationCollision = throwOnRegistrationCollision
         };
         configuration.MakeReadOnly();
         _configuration = configuration;

         _swRegisterValidation = new Stopwatch();
         _swRegister = new Stopwatch();
         _swRegistrationOverwrite = new Stopwatch();
         _swResolveValidation = new Stopwatch();
         _swResolve = new Stopwatch();
         _swResolveOverwrite = new Stopwatch();
         _swPrecludedTypeAdd = new Stopwatch();
         _swPrecludedTypeRemove = new Stopwatch();
         _swUnregister = new Stopwatch();
      }

       public object Clone()
      {
         throw new NotSupportedException();
      }

       public event EventHandler<EventArgs> ConfigurationChanged
      {
         add => throw new NotSupportedException();
         remove => throw new NotSupportedException();
      }

       public bool AllowNamedImplementations
      {
         get => _configuration.AllowNamedImplementations;
         set => throw new NotSupportedException();
      }

       public bool AllowPreclusionOfTypes
      {
         get => _configuration.AllowPreclusionOfTypes;
         set => throw new NotSupportedException();
      }

       public IIocContainerMetaIdentity Container => this;

       public bool IsReadOnly => false;

       public string Name => "Performance Test: ImmutableDictionary<RegistrationKeyTypeNamePair, RegistrationValueTypeInstancePair>";

       public bool ThrowOnRegistrationCollision
      {
         get => _configuration.ThrowOnRegistrationCollision;
         set => throw new NotSupportedException();
      }

       public Guid Uid { get; } = Guid.NewGuid();

       public bool AddPrecludedType(Type precludedType)
      {
         try
         {
            _swPrecludedTypeAdd.Start();
            if (!_configuration.AllowPreclusionOfTypes)
            {
               throw new ContainerConfigurationPrecludedTypesDisabledException(this, null, null);
            }

            var was = _precludedTypes;

            if (precludedType != null && (precludedType.IsInterface || precludedType.IsAbstract))
            {
               _precludedTypes = _precludedTypes.Add(precludedType);
            }

            var rv = !ReferenceEquals(was, _precludedTypes);

            return rv;
         }
         finally
         {
            _swPrecludedTypeAdd.Stop();
         }
      }

       [SuppressMessage("SonarLint.CodeSmell", "S3877: Exceptions should not be thrown from unexpected methods")]
      [SuppressMessage("Microsoft.Design", "CA1065: Do not raise exceptions in unexpected locations", Justification = "Reviewed (MWP)")]
      public bool Equals(IIocContainerConfiguration other)
      {
         throw new NotSupportedException();
      }

      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "swRegistrationOverwrite")]
      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsRunning")]
      public void GetRegistrationOverwriteStats(out TimeSpan registrationOverwriteTime, out int registrationOverwriteCount)
      {
         if (_swRegistrationOverwrite.IsRunning)
         {
            throw new InvalidOperationException("_swRegistrationOverwrite.IsRunning");
         }

         registrationOverwriteTime = _swRegistrationOverwrite.Elapsed;
         registrationOverwriteCount = _registrationOverwriteCount;
      }

      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "swRegister")]
      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsRunning")]
      public void GetRegistrationTotalStats(out TimeSpan registrationTotalTime, out int registrationTotalCount)
      {
         if (_swRegister.IsRunning)
         {
            throw new InvalidOperationException("_swRegister.IsRunning");
         }

         registrationTotalTime = _swRegister.Elapsed;
         registrationTotalCount = _registrationTotalCount;
      }

      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "swRegisterValidation")]
      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsRunning")]
      public void GetRegistrationValidationStats(out TimeSpan registrationValidationTime)
      {
         if (_swRegisterValidation.IsRunning)
         {
            throw new InvalidOperationException("_swRegisterValidation.IsRunning");
         }

         registrationValidationTime = _swRegisterValidation.Elapsed;
      }

      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "swResolveOverwrite")]
      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsRunning")]
      public void GetResolutionOverwriteStats(out TimeSpan resolutionOverwriteTime, out int resolutionNewInstancesCount)
      {
         if (_swResolveOverwrite.IsRunning)
         {
            throw new InvalidOperationException("_swResolveOverwrite.IsRunning");
         }

         resolutionOverwriteTime = _swResolveOverwrite.Elapsed;
         resolutionNewInstancesCount = _resolutionNewInstancesCount;
      }

      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "swResolve")]
      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsRunning")]
      public void GetResolutionTotalStats(out TimeSpan resolutionTotalTime, out int resolutionTotalCount)
      {
         if (_swResolve.IsRunning)
         {
            throw new InvalidOperationException("_swResolve.IsRunning");
         }

         resolutionTotalTime = _swResolve.Elapsed;
         resolutionTotalCount = _resolutionTotalCount;
      }

      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "swResolveValidation")]
      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsRunning")]
      public void GetResolutionValidationStats(out TimeSpan resolutionValidationTime)
      {
         if (_swResolveValidation.IsRunning)
         {
            throw new InvalidOperationException("_swResolveValidation.IsRunning");
         }

         resolutionValidationTime = _swResolveValidation.Elapsed;
      }

      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "swUnregister")]
      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsRunning")]
      public void GetUnregistrationStats(out TimeSpan unregistrationTotalTime, out int unregistrationTotalCount)
      {
         if (_swUnregister.IsRunning)
         {
            throw new InvalidOperationException("_swUnregister.IsRunning");
         }

         unregistrationTotalTime = _swUnregister.Elapsed;
         unregistrationTotalCount = _unregistrationTotalCount;
      }

      [SuppressMessage("Microsoft.Maintainability", "CA1502: Avoid excessive complexity")]
      [SuppressMessage("SonarLint.CodeSmell", "S138: Functions should not have too many lines of code")]
      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      public bool RegisterImplementationImplementation(Type fromType, string fromTypeParameterName, string name, Type toType, string toTypeParameterName, bool tryLogic)
      {
         string cleanedName;
         try
         {
            _registrationTotalCount++;
            _swRegisterValidation.Start();
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

               throw new ContainerFromTypePrecludedArgumentException(null, fromType, fromTypeParameterName);
            }

            // name:  clean and names allowed
            cleanedName = name.TrimNullToEmpty();
            if (cleanedName.Length > 0 && !_configuration.AllowNamedImplementations)
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new ContainerConfigurationNamedImplementationsDisabledException(this, null, null);
            }

            // toType: not null
            if (toType == null)
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new ArgumentNullException(toTypeParameterName);
            }

            // toType: not open generic
            if (toType.ContainsGenericParameters)
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new TypeMustNotBeAnOpenGenericArgumentException(toType, toTypeParameterName, null, null);
            }

            // toType: not interface and not abstract
            if (toType.IsInterface || toType.IsAbstract)
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new ToTypeMustNotBeInterfaceNorAbstractArgumentException(toType, toTypeParameterName);
            }

            // toType: implements fromType
            if (!fromType.IsAssignableFrom(toType))
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new ToTypeMustImplementTypeArgumentException(fromType, toType, toTypeParameterName, null, null);
            }

            // toType: has public default ctor
            var defaultPublicCtor = toType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
            if (defaultPublicCtor == null)
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new ToTypeMustHavePublicDefaultConstructorArgumentException(toType, toTypeParameterName);
            }
         }
         finally
         {
            _swRegisterValidation.Stop();
         }

         var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);
         var value = new RegistrationValueTypeInstancePair(toType);
         try
         {
            _swRegister.Start();
            try
            {
               using (_registrationsLock.EnterWriteLock())
               {
                  var was = _registrations;
                  _registrations = _registrations.Add(key, value);
                  if (ReferenceEquals(was, _registrations) && _configuration.ThrowOnRegistrationCollision)
                  {
                     // duplicate key, duplicate value: no change.
                     if (tryLogic)
                     {
                        return false;
                     }

                     throw new ContainerFromTypeNameAlreadyRegisteredArgumentException(this, fromType, cleanedName, nameof(fromType));
                  }
               }

               return true;
            }
            catch (ArgumentException ae)
            {
               // duplicate key, different value.
               if (_configuration.ThrowOnRegistrationCollision)
               {
                  if (tryLogic)
                  {
                     return false;
                  }

                  throw new ContainerFromTypeNameAlreadyRegisteredArgumentException(this, fromType, cleanedName, nameof(fromType), null, ae);
               }

               // last updater wins: update in place
               try
               {
                  _registrationOverwriteCount++;
                  _swRegistrationOverwrite.Start();
                  using (_registrationsLock.EnterWriteLock())
                  {
                     var was = _registrations;
                     _registrations = _registrations.SetItem(key, value);
                     if (ReferenceEquals(was, _registrations))
                     {
                        return false;
                     }
                  }

                  return true;
               }
               finally
               {
                  _swRegistrationOverwrite.Stop();
               }
            }
         }
         finally
         {
            _swRegister.Stop();
         }
      }

      [SuppressMessage("SonarLint.CodeSmell", "S138: Functions should not have too many lines of code")]
      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      public bool RegisterInstanceImplementation(Type fromType, string fromTypeParameterName, string name, object instance, string instanceParameterName, bool tryLogic)
      {
         string cleanedName;
         try
         {
            _registrationTotalCount++;
            _swRegisterValidation.Start();
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

               throw new ContainerFromTypePrecludedArgumentException(null, fromType, fromTypeParameterName);
            }

            // name:  clean and names allowed
            cleanedName = name.TrimNullToEmpty();
            if (cleanedName.Length > 0 && !_configuration.AllowNamedImplementations)
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new ContainerConfigurationNamedImplementationsDisabledException(this, null, null);
            }

            // instance: not null
            if (instance == null)
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new ArgumentNullException(instanceParameterName);
            }

            // instance: implements fromType
            if (!fromType.IsInstanceOfType(instance))
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new InstanceMustImplementTypeArgumentException(fromType, instance, instanceParameterName);
            }
         }
         finally
         {
            _swRegisterValidation.Stop();
         }

         var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);
         var value = new RegistrationValueTypeInstancePair(instance);
         try
         {
            _swRegister.Start();
            try
            {
               using (_registrationsLock.EnterWriteLock())
               {
                  var was = _registrations;
                  _registrations = _registrations.Add(key, value);
                  if (ReferenceEquals(was, _registrations) && _configuration.ThrowOnRegistrationCollision)
                  {
                     // duplicate key, duplicate value: no change.
                     if (tryLogic)
                     {
                        return false;
                     }

                     throw new ContainerFromTypeNameAlreadyRegisteredArgumentException(this, fromType, cleanedName, nameof(fromType));
                  }
               }

               return true;
            }
            catch (ArgumentException ae)
            {
               // duplicate key, different value.
               if (_configuration.ThrowOnRegistrationCollision)
               {
                  if (tryLogic)
                  {
                     return false;
                  }

                  throw new ContainerFromTypeNameAlreadyRegisteredArgumentException(this, fromType, cleanedName, nameof(fromType), null, ae);
               }

               // last updater wins: update in place
               try
               {
                  _registrationOverwriteCount++;
                  _swRegistrationOverwrite.Start();
                  using (_registrationsLock.EnterWriteLock())
                  {
                     var was = _registrations;
                     _registrations = _registrations.SetItem(key, value);
                     if (ReferenceEquals(was, _registrations))
                     {
                        return false;
                     }
                  }

                  return true;
               }
               finally
               {
                  _swRegistrationOverwrite.Stop();
               }
            }
         }
         finally
         {
            _swRegister.Stop();
         }
      }

      public bool RemovePrecludedType(Type precludedType)
      {
         try
         {
            _swPrecludedTypeRemove.Start();
            // does not check whether preclusion of types configuration is disabled by design.
            var was = _precludedTypes;
            _precludedTypes = _precludedTypes.Remove(precludedType);
            var rv = !ReferenceEquals(was, _precludedTypes);

            return rv;
         }
         finally
         {
            _swPrecludedTypeRemove.Stop();
         }
      }

      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      [SuppressMessage("SonarLint.CodeSmell", "S138: Functions should not have too many lines of code")]
      public bool ResolveImplementation(Type fromType, string fromTypeParameterName, string name, bool tryLogic, out object instance)
      {
         instance = null;
         string cleanedName;
         try
         {
            _resolutionTotalCount++;
            _swResolveValidation.Start();
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

               throw new ContainerFromTypePrecludedArgumentException(null, fromType, fromTypeParameterName);
            }

            // name:  clean and names allowed
            cleanedName = name.TrimNullToEmpty();
            if (cleanedName.Length > 0 && !_configuration.AllowNamedImplementations)
            {
               if (tryLogic)
               {
                  return false;
               }

               throw new ContainerConfigurationNamedImplementationsDisabledException(this, null, null);
            }
         }
         finally
         {
            _swResolveValidation.Stop();
         }

         var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);
         // ReSharper disable once TooWideLocalVariableScope
         RegistrationValueTypeInstancePair value;
         // ReSharper disable once TooWideLocalVariableScope
         // ReSharper disable once RedundantAssignment
         var found = false;
         try
         {
            _swResolve.Start();
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
               try
               {
                  _swResolveOverwrite.Start();
                  _resolutionNewInstancesCount++;
                  var newInstance = Activator.CreateInstance(value.ImplementationType);
                  var replacementValue = new RegistrationValueTypeInstancePair(value.ImplementationType, newInstance);
                  using (_registrationsLock.EnterWriteLock())
                  {
                     _registrations = _registrations.SetItem(key, replacementValue);
                  }

                  instance = newInstance;
                  return true;
               }
               finally
               {
                  _swResolveOverwrite.Stop();
               }
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
         finally
         {
            _swResolve.Stop();
         }
      }

      public bool UnregisterImplementation(Type fromType, string name)
      {
         try
         {
            _unregistrationTotalCount++;
            _swUnregister.Start();
            if (fromType == null || !(fromType.IsAbstract || fromType.IsInterface) || fromType.ContainsGenericParameters)
            {
               return false;
            }

            var cleanedName = name.TrimNullToEmpty();
            if (cleanedName.Length > 0 && !_configuration.AllowNamedImplementations)
            {
               return false;
            }

            bool rv;
            var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);
            using (_registrationsLock.EnterWriteLock())
            {
               var was = _registrations;
               _registrations = _registrations.Remove(key);
               if (ReferenceEquals(was, _registrations))
               {
                  // no change
                  // the desired state exists; the fromType is not registered.  Do not throw. 
                  rv = false;
               }
               else
               {
                  // removed
                  rv = true;
               }
            }

            return rv;
         }
         finally
         {
            _swUnregister.Stop();
         }
      }
   }
}
