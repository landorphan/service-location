namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Represents registration services for an <see cref="IIocContainer"/> container.
   /// </summary>
   /// <remarks>
   /// <para>
   /// Writes registrations and container-specific 'overrides' to the container and provides notification of the same.
   /// </para>
   /// <para>
   /// Either the implementation type or the implementation type is required to register the registration type.
   /// </para>
   /// <para>
   /// Removal of the registration type requires only the registration type, and optionally the name of the registration.
   /// </para>
   /// <para>
   /// <see cref="IIocContainer"/> works in concert with a <see cref="IIocContainerManager"/>, a <see cref="IIocContainerRegistrar"/>,  and a <see cref="IIocContainerResolver"/>.
   /// </para>
   ///  </remarks>
   [SuppressMessage("SonarLint.CodeSmell", "S1939: Inheritance list should not be redundant", Justification = "Being explicit (MWP)")]
   public interface IIocContainerRegistrar : IIocContainerMetaSharedCapacities, IIocContainerRegistrationRepository
   {
      /// <summary>
      /// Occurs when a type is registered with this container.
      /// </summary>
      event EventHandler<ContainerTypeRegistrationEventArgs> ContainerRegistrationAdded;

      /// <summary>
      /// Occurs when a type is unregistered with this container.
      /// </summary>
      event EventHandler<ContainerTypeRegistrationEventArgs> ContainerRegistrationRemoved;

      /// <summary>
      /// Registers a type and implementation with this container as the default implementation.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The interface or abstract type to register.
      /// </typeparam>
      /// <typeparam name="TTo">
      /// The implementation type.
      /// </typeparam>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered instance of the given <typeparamref name="TTo"/> type.
      /// </para>
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <typeparamref name = "TFrom"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a default (un-named) interface or abstract type of the given <typeparmref name = "TFrom"/> has already been registered with this container registrar.
      /// </exception>
      void RegisterImplementation<TFrom, TTo>() where TFrom : class where TTo : class, TFrom, new();

      /// <summary>
      /// Registers a type and implementation with this container as the default implementation.
      /// </summary>
      /// <param name="fromType">
      /// The type of interface or abstract class to register.
      /// </param>
      /// <param name="toType">
      /// The implementation type.  (Requires a public default constructor; lazily loaded).
      /// </param>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered instance of the given <paramref name="fromType"/> type.
      /// </para>
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <paramref name = "fromType"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a default (un-named) interface or abstract type of the given <parmref name = "fromType"/> has already been registered with this container registrar.
      /// </exception>
      void RegisterImplementation(Type fromType, Type toType);

      /// <summary>
      /// Registers a type and implementation with this container as a named implementation.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The interface or abstract type to register.
      /// </typeparam>
      /// <param name="name">
      /// Name to use for registration, use <c> null </c> or <see cref="string.Empty"/> to specify the default instance.
      /// </param>
      /// <typeparam name="TTo">
      /// The implementation type.  (Requires a public default constructor; lazily loaded).
      /// </typeparam>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered instance of the given <typeparamref name="TTo"/> type.
      /// </para>
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <typeparamref name = "TFrom"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a named interface or abstract type of the given <typeparmref name = "TFrom"/> and the given name <paramref name = "name"/>
      /// has already been registered with this container registrar.
      /// </exception>
      /// <exception cref="ContainerConfigurationNamedImplementationsDisabledException">
      /// Thrown when the container is configured to disallow named registrations and <paramref name="name"/> is neither null nor empty.
      /// </exception>
      void RegisterImplementation<TFrom, TTo>(String name) where TFrom : class where TTo : class, TFrom, new();

      /// <summary>
      /// Registers a type and implementation with this container as a named implementation.
      /// </summary>
      /// <param name="fromType">
      /// The type of interface or abstract class to register.
      /// </param>
      /// <param name="name">
      /// Name to use for registration, use <c> null </c> or <see cref="string.Empty"/> to specify the default instance.
      /// </param>
      /// <param name="toType">
      /// The type of implementation.
      /// </param>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered instance of the given <paramref name="fromType"/> type.
      /// </para>
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <paramref name = "fromType"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a named interface or abstract type of the given <parmref name = "fromType"/> and the given name <paramref name = "name"/>
      /// has already been registered with this container registrar.
      /// </exception>
      /// <exception cref="ContainerConfigurationNamedImplementationsDisabledException">
      /// Thrown when the container is configured to disallow named registrations and <paramref name="name"/> is neither null nor empty.
      /// </exception>
      void RegisterImplementation(Type fromType, String name, Type toType);

      /// <summary>
      /// Registers a type and instance with this container as the default instance.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The type of instance to register.
      /// </typeparam>
      /// <param name="instance">
      /// The implementation instance to be returned on subsequent resolutions of the interface.
      /// </param>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered implementations of the of the same implementation type as <paramref name="instance"/>.
      /// </para>
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <typeparamref name = "TFrom"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a default (un-named) interface or abstract type of the given <typeparmref name = "TFrom"/> has already been registered with this container registrar.
      /// </exception>
      void RegisterInstance<TFrom>(TFrom instance) where TFrom : class;

      /// <summary>
      /// Registers a type and instance with this container as the default instance.
      /// </summary>
      /// <param name="fromType">
      /// The type of instance to register.
      /// </param>
      /// <param name="instance">
      /// The implementation instance to be returned on subsequent resolutions of the interface.
      /// </param>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <paramref name = "fromType"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a default (un-named) interface or abstract type of the given <parmref name = "fromType"/> has already been registered with this container registrar.
      /// </exception>
      void RegisterInstance(Type fromType, Object instance);

      /// <summary>
      /// Registers a type and instance with this container as a named instance.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The type of instance to register.
      /// </typeparam>
      /// <param name="name">
      /// The identifying name to use for registration, use <c> null </c> or <see cref="string.Empty"/> to specify the default instance.
      /// </param>
      /// <param name="instance">
      /// The implementation instance to be returned on subsequent resolutions of the interface.
      /// </param>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <typeparamref name = "TFrom"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a named interface or abstract type of the given <parmref name = "fromType"/> and given <paramref name = "name"/> has already been registered with this container registrar.
      /// </exception>
      /// <exception cref="ContainerConfigurationNamedImplementationsDisabledException">
      /// Thrown when the container is configured to disallow named registrations and <paramref name="name"/> is neither null nor empty.
      /// </exception>
      void RegisterInstance<TFrom>(String name, TFrom instance) where TFrom : class;

      /// <summary>
      /// Registers a type and instance with this container as a named instance.
      /// </summary>
      /// <param name="fromType">
      /// The type of instance to register.
      /// </param>
      /// <param name="name">
      /// The identifying name to use for registration, use <c> null </c> or <see cref="string.Empty"/> to specify the default instance.
      /// </param>
      /// <param name="instance">
      /// The implementation instance to be returned on subsequent resolutions of the interface.
      /// </param>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <paramref name = "fromType"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a named interface or abstract type of the given <parmref name = "fromType"/> and the given name <paramref name= "name"/> has already registered with this container registrar.
      /// </exception>
      /// <exception cref="ContainerConfigurationNamedImplementationsDisabledException">
      /// Thrown when the container is configured to disallow named registrations and <paramref name="name"/> is neither null nor empty.
      /// </exception>
      void RegisterInstance(Type fromType, String name, Object instance);

      /// <summary>
      /// Attempts to register a type and implementation with this container as the default implementation.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The interface or abstract type to register.
      /// </typeparam>
      /// <typeparam name="TTo">
      /// The implementation type.
      /// </typeparam>
      /// <returns>
      /// <c>true</c>when the registration is successful; otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered instance of the given <typeparamref name="TTo"/> type.
      /// </para>
      /// </remarks>
      Boolean TryRegisterImplementation<TFrom, TTo>() where TFrom : class where TTo : class, TFrom, new();

      /// <summary>
      /// Attempts to register a type and implementation with this container as the default implementation.
      /// </summary>
      /// <param name="fromType">
      /// The type of interface or abstract class to register.
      /// </param>
      /// <param name="toType">
      /// The implementation type.  (Requires a public default constructor; lazily loaded).
      /// </param>
      /// <returns>
      /// <c>true</c>when the registration is successful; otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered instance of the given <paramref name="fromType"/> type.
      /// </para>
      /// </remarks>
      Boolean TryRegisterImplementation(Type fromType, Type toType);

      /// <summary>
      /// Attempts to register a type and implementation with this container as a named implementation.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The interface or abstract type to register.
      /// </typeparam>
      /// <param name="name">
      /// Name to use for registration, use <c> null </c> or <see cref="string.Empty"/> to specify the default instance.
      /// </param>
      /// <typeparam name="TTo">
      /// The implementation type.  (Requires a public default constructor; lazily loaded).
      /// </typeparam>
      /// <returns>
      /// <c>true</c>when the registration is successful; otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered instance of the given <typeparamref name="TTo"/> type.
      /// </para>
      /// </remarks>
      Boolean TryRegisterImplementation<TFrom, TTo>(String name) where TFrom : class where TTo : class, TFrom, new();

      /// <summary>
      /// Attempts to register a type and implementation with this container as a named implementation.
      /// </summary>
      /// <param name="fromType">
      /// The type of interface or abstract class to register.
      /// </param>
      /// <param name="name">
      /// Name to use for registration, use <c> null </c> or <see cref="string.Empty"/> to specify the default instance.
      /// </param>
      /// <param name="toType">
      /// The type of implementation.
      /// </param>
      /// <returns>
      /// <c>true</c>when the registration is successful; otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered instance of the given <paramref name="fromType"/> type.
      /// </para>
      /// </remarks>
      Boolean TryRegisterImplementation(Type fromType, String name, Type toType);

      /// <summary>
      /// Attempts to register a type and instance with this container as the default instance.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The type of instance to register.
      /// </typeparam>
      /// <param name="instance">
      /// The implementation instance to be returned on subsequent resolutions of the interface.
      /// </param>
      /// <returns>
      /// <c>true</c>when the registration is successful; otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Subject to collisions with registered implementations of the of the same implementation type as <paramref name="instance"/>.
      /// </para>
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <typeparamref name = "TFrom"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a default (un-named) interface or abstract type of the given <typeparmref name = "TFrom"/> has already been registered with this container registrar.
      /// </exception>
      Boolean TryRegisterInstance<TFrom>(TFrom instance) where TFrom : class;

      /// <summary>
      /// Attempts to register a type and instance with this container as the default instance.
      /// </summary>
      /// <param name="fromType">
      /// The type of instance to register.
      /// </param>
      /// <param name="instance">
      /// The implementation instance to be returned on subsequent resolutions of the interface.
      /// </param>
      /// <returns>
      /// <c>true</c>when the registration is successful; otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <paramref name = "fromType"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a default (un-named) interface or abstract type of the given <parmref name = "fromType"/> has already been registered with this container registrar.
      /// </exception>
      Boolean TryRegisterInstance(Type fromType, Object instance);

      /// <summary>
      /// Attempts to register a type and instance with this container as a named instance.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The type of instance to register.
      /// </typeparam>
      /// <param name="name">
      /// The identifying name to use for registration, use <c> null </c> or <see cref="string.Empty"/> to specify the default instance.
      /// </param>
      /// <param name="instance">
      /// The implementation instance to be returned on subsequent resolutions of the interface.
      /// </param>
      /// <returns>
      /// <c>true</c>when the registration is successful; otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <typeparamref name = "TFrom"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a named interface or abstract type of the given <parmref name = "fromType"/> and given <paramref name = "name"/> has already been registered with this container registrar.
      /// </exception>
      Boolean TryRegisterInstance<TFrom>(String name, TFrom instance) where TFrom : class;

      /// <summary>
      /// Attempts to register a type and instance with this container as a named instance.
      /// </summary>
      /// <param name="fromType">
      /// The type of instance to register.
      /// </param>
      /// <param name="name">
      /// The identifying name to use for registration, use <c> null </c> or <see cref="string.Empty"/> to specify the default instance.
      /// </param>
      /// <param name="instance">
      /// The implementation instance to be returned on subsequent resolutions of the interface.
      /// </param>
      /// <returns>
      /// <c>true</c>when the registration is successful; otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// The chain of parent(s) and children is not evaluated.
      /// </remarks>
      /// <exception cref = "ContainerFromTypePrecludedArgumentException">
      /// Thrown when the type <paramref name = "fromType"/> has been precluded from registration.
      /// </exception>
      /// <exception cref="ContainerFromTypeNameAlreadyRegisteredArgumentException">
      /// Thrown when a named interface or abstract type of the given <parmref name = "fromType"/> and the given name <paramref name= "name"/> has already registered with this container registrar.
      /// </exception>
      Boolean TryRegisterInstance(Type fromType, String name, Object instance);

      /// <summary>
      /// Unregisters the default (un-named) type from this registration.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The type of interface or abstract class to evaluate.
      /// </typeparam>
      /// <returns>
      /// <c>true</c> when the registration was found and removed; otherwise <c>false</c> (no such registration was found).
      /// </returns>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Does not throw if no such registration found.
      /// </para>
      /// </remarks>
      Boolean Unregister<TFrom>() where TFrom : class;

      /// <summary>
      /// Unregisters a default (un-named) type from this registration.
      /// </summary>
      /// <param name="fromType">
      /// The type of interface or abstract class to evaluate.
      /// </param>
      /// <returns>
      /// <c>true</c> when the registration was found and removed; otherwise <c>false</c> (no such registration was found).
      /// </returns>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Does not throw if no such registration found.
      /// </para>
      /// </remarks>
      Boolean Unregister(Type fromType);

      /// <summary>
      /// Unregisters a named type registration from this container.
      /// </summary>
      /// <typeparam name="TFrom">
      /// The type of interface or abstract class to evaluate.
      /// </typeparam>
      /// <param name="name">
      /// The name to evaluate.
      /// </param>
      /// <returns>
      /// <c>true</c> when the registration was found and removed; otherwise <c>false</c> (no such registration was found).
      /// </returns>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Does not throw if no such registration found.
      /// </para>
      /// </remarks>
      Boolean Unregister<TFrom>(String name) where TFrom : class;

      /// <summary>
      /// Unregisters a named type registration from this container.
      /// </summary>
      /// <param name="fromType">
      /// The type of interface or abstract class to evaluate.
      /// </param>
      /// <param name="name">
      /// The name to evaluate.
      /// </param>
      /// <returns>
      /// <c>true</c> when the registration was found and removed; otherwise <c>false</c> (no such registration was found).
      /// </returns>
      /// <remarks>
      /// <para>
      /// The chain of parent(s) and children is not evaluated.
      /// </para>
      /// <para>
      /// Does not throw if no such registration found.
      /// </para>
      /// </remarks>
      Boolean Unregister(Type fromType, String name);
   }
}
