namespace Landorphan.Ioc.ServiceLocation.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Landorphan.Common;
    using Landorphan.Ioc.Resources;
    using Landorphan.Ioc.ServiceLocation.EventArguments;
    using Landorphan.Ioc.ServiceLocation.Exceptions;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    // ReSharper disable ConvertToAutoProperty
    // ReSharper disable InheritdocConsiderUsage
    // ReSharper disable RedundantExtendsListEntry

    internal sealed partial class IocContainer : DisposableObject, IOwnedIocContainer, IIocContainerManager, IIocContainerRegistrar, IIocContainerResolver
    {
        /// <inheritdoc cref="IIocContainer"/>
        event EventHandler<ContainerTypeRegistrationEventArgs> IIocContainerRegistrar.ContainerRegistrationAdded
        {
            add => _listenersContainerRegistrationAdded.Add(value);
            remove => _listenersContainerRegistrationAdded.Remove(value);
        }

        /// <inheritdoc cref="IIocContainer"/>
        event EventHandler<ContainerTypeRegistrationEventArgs> IIocContainerRegistrar.ContainerRegistrationRemoved
        {
            add => _listenersContainerRegistrationRemoved.Add(value);
            remove => _listenersContainerRegistrationRemoved.Remove(value);
        }

        /// <inheritdoc/>
        void IIocContainerRegistrar.RegisterImplementation<TFrom, TTo>()
        {
            ((IIocContainerRegistrar)this).RegisterImplementation<TFrom, TTo>(null);
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Usage", "CA2208: Instantiate argument exceptions correctly", Justification = "Using type parameters (MWP)")]
        void IIocContainerRegistrar.RegisterImplementation<TFrom, TTo>(string name)
        {
            RegisterImplementationImplementation(typeof(TFrom), nameof(TFrom), name, typeof(TTo), nameof(TTo), false);
        }

        /// <inheritdoc/>
        void IIocContainerRegistrar.RegisterImplementation(Type fromType, Type toType)
        {
            ((IIocContainerRegistrar)this).RegisterImplementation(fromType, null, toType);
        }

        /// <inheritdoc/>
        void IIocContainerRegistrar.RegisterImplementation(Type fromType, string name, Type toType)
        {
            RegisterImplementationImplementation(fromType, nameof(fromType), name, toType, nameof(toType), false);
        }

        /// <inheritdoc/>
        void IIocContainerRegistrar.RegisterInstance<TFrom>(TFrom instance)
        {
            ((IIocContainerRegistrar)this).RegisterInstance(null, instance);
        }

        /// <inheritdoc/>
        [SuppressMessage("Microsoft.Usage", "CA2208: Instantiate argument exceptions correctly", Justification = "Using type parameters (MWP)")]
        void IIocContainerRegistrar.RegisterInstance<TFrom>(string name, TFrom instance)
        {
            RegisterInstanceImplementation(typeof(TFrom), nameof(TFrom), name, instance, nameof(instance), false);
        }

        /// <inheritdoc/>
        void IIocContainerRegistrar.RegisterInstance(Type fromType, object instance)
        {
            ((IIocContainerRegistrar)this).RegisterInstance(fromType, null, instance);
        }

        /// <inheritdoc/>
        void IIocContainerRegistrar.RegisterInstance(Type fromType, string name, object instance)
        {
            RegisterInstanceImplementation(fromType, nameof(fromType), name, instance, nameof(instance), false);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.TryRegisterImplementation<TFrom, TTo>()
        {
            return ((IIocContainerRegistrar)this).TryRegisterImplementation<TFrom, TTo>(null);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.TryRegisterImplementation<TFrom, TTo>(string name)
        {
            return RegisterImplementationImplementation(typeof(TFrom), nameof(TFrom), name, typeof(TTo), nameof(TTo), true);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.TryRegisterImplementation(Type fromType, Type toType)
        {
            return ((IIocContainerRegistrar)this).TryRegisterImplementation(fromType, null, toType);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.TryRegisterImplementation(Type fromType, string name, Type toType)
        {
            return RegisterImplementationImplementation(fromType, nameof(fromType), name, toType, nameof(toType), true);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.TryRegisterInstance<TFrom>(TFrom instance)
        {
            return ((IIocContainerRegistrar)this).TryRegisterInstance(null, instance);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.TryRegisterInstance<TFrom>(string name, TFrom instance)
        {
            return RegisterInstanceImplementation(typeof(TFrom), nameof(TFrom), name, instance, nameof(instance), true);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.TryRegisterInstance(Type fromType, object instance)
        {
            return ((IIocContainerRegistrar)this).TryRegisterInstance(fromType, null, instance);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.TryRegisterInstance(Type fromType, string name, object instance)
        {
            return RegisterInstanceImplementation(fromType, nameof(fromType), name, instance, nameof(instance), true);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.Unregister<TFrom>()
        {
            return ((IIocContainerRegistrar)this).Unregister(typeof(TFrom), null);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.Unregister<TFrom>(string name)
        {
            return UnregisterImplementation(typeof(TFrom), name);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.Unregister(Type fromType)
        {
            return ((IIocContainerRegistrar)this).Unregister(fromType, null);
        }

        /// <inheritdoc/>
        bool IIocContainerRegistrar.Unregister(Type fromType, string name)
        {
            return UnregisterImplementation(fromType, name);
        }

        private void OnContainerRegistrationAdded(RegistrationKeyTypeNamePair typeNamePair, Type toType, object instance)
        {
            // fires the event
            TryLogRegistrationAddedOrRemoved(IocEventIdCodes.IocContainer.RegistrationAdded, typeNamePair, toType, instance);

            // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
            var e = new ContainerTypeRegistrationEventArgs(this, typeNamePair.RegisteredType, typeNamePair.RegisteredName, toType, instance);
            _listenersContainerRegistrationAdded?.Invoke(this, e);
        }

        private void OnContainerRegistrationRemoved(RegistrationKeyTypeNamePair typeNamePair)
        {
            // fires the event
            TryLogRegistrationAddedOrRemoved(IocEventIdCodes.IocContainer.RegistrationRemoved, typeNamePair, null, null);

            // need null propagation because the listeners field will be set to null when this instance is disposing, creating a race condition.
            var e = new ContainerTypeRegistrationEventArgs(this, typeNamePair.RegisteredType, typeNamePair.RegisteredName, null, null);
            _listenersContainerRegistrationRemoved?.Invoke(this, e);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502: Avoid excessive complexity")]
        [SuppressMessage("SonarLint.CodeSmell", "S138: Functions should not have too many lines of code")]
        [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
        [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
        private bool RegisterImplementationImplementation(Type fromType, string fromTypeParameterName, string name, Type toType, string toTypeParameterName, bool tryLogic)
        {
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

            var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);
            var value = new RegistrationValueTypeInstancePair(toType);
            CheckForNewRegistrations();
            try
            {
                using (_registrationsLock.EnterWriteLock())
                {
                    var was = _registrations;
                    _registrations = _registrations.Add(key, value);
                    if (ReferenceEquals(was, _registrations) && Manager.Configuration.ThrowOnRegistrationCollision)
                    {
                        // duplicate key, duplicate value: no change.
                        if (tryLogic)
                        {
                            return false;
                        }

                        throw new ContainerFromTypeNameAlreadyRegisteredArgumentException(this, fromType, cleanedName, nameof(fromType));
                    }
                }

                OnContainerRegistrationAdded(key, value.ImplementationType, value.ImplementationInstance);
                return true;
            }
            catch (ArgumentException ae)
            {
                // duplicate key, different value.
                if (Manager.Configuration.ThrowOnRegistrationCollision)
                {
                    if (tryLogic)
                    {
                        return false;
                    }

                    throw new ContainerFromTypeNameAlreadyRegisteredArgumentException(this, fromType, cleanedName, nameof(fromType), null, ae);
                }

                // last updater wins: update in place
                using (_registrationsLock.EnterWriteLock())
                {
                    var was = _registrations;
                    _registrations = _registrations.SetItem(key, value);
                    if (ReferenceEquals(was, _registrations))
                    {
                        return false;
                    }
                }

                OnContainerRegistrationRemoved(key);
                OnContainerRegistrationAdded(key, value.ImplementationType, value.ImplementationInstance);
                return true;
            }
        }

        [SuppressMessage("SonarLint.CodeSmell", "S138: Functions should not have too many lines of code")]
        [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
        [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
        private bool RegisterInstanceImplementation(Type fromType, string fromTypeParameterName, string name, object instance, string instanceParameterName, bool tryLogic)
        {
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

            var key = new RegistrationKeyTypeNamePair(fromType, cleanedName);
            var value = new RegistrationValueTypeInstancePair(instance);
            CheckForNewRegistrations();
            try
            {
                using (_registrationsLock.EnterWriteLock())
                {
                    var was = _registrations;
                    _registrations = _registrations.Add(key, value);
                    if (ReferenceEquals(was, _registrations) && Manager.Configuration.ThrowOnRegistrationCollision)
                    {
                        // duplicate key, duplicate value: no change.
                        if (tryLogic)
                        {
                            return false;
                        }

                        throw new ContainerFromTypeNameAlreadyRegisteredArgumentException(this, fromType, cleanedName, nameof(fromType));
                    }
                }

                OnContainerRegistrationAdded(key, value.ImplementationType, value.ImplementationInstance);
                return true;
            }
            catch (ArgumentException ae)
            {
                // duplicate key, different value.
                if (Manager.Configuration.ThrowOnRegistrationCollision)
                {
                    if (tryLogic)
                    {
                        return false;
                    }

                    throw new ContainerFromTypeNameAlreadyRegisteredArgumentException(this, fromType, cleanedName, nameof(fromType), null, ae);
                }

                // last updater wins: update in place
                using (_registrationsLock.EnterWriteLock())
                {
                    var was = _registrations;
                    _registrations = _registrations.SetItem(key, value);
                    if (ReferenceEquals(was, _registrations))
                    {
                        return false;
                    }
                }

                OnContainerRegistrationRemoved(key);
                OnContainerRegistrationAdded(key, value.ImplementationType, value.ImplementationInstance);
                return true;
            }
        }

        private bool UnregisterImplementation(Type fromType, string name)
        {
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

            if (rv)
            {
                OnContainerRegistrationRemoved(key);
            }

            return rv;
        }
    }
}
