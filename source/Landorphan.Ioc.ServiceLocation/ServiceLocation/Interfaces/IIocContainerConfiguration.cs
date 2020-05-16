﻿namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
    using System;
    using Landorphan.Common.Interfaces;

    /// <summary>
   /// Represents the configuration of an <see cref="IIocContainer" />.
   /// </summary>
   /// <remarks>
   /// <para>
   /// Containers are configured individually.
   /// </para>
   /// </remarks>
   public interface IIocContainerConfiguration : ICloneable, IQueryReadOnly, IEquatable<IIocContainerConfiguration>
   {
       /// <summary>
      /// Event queue for all listeners interested in state changes to this instance.
      /// </summary>
      event EventHandler<EventArgs> ConfigurationChanged;

       /// <summary>
      /// Gets or sets a value governing the behavior or named implementations.
      /// </summary>
      /// <value>
      /// When <c>true</c> registrations are distinguished by both registered "fromType" and name;
      /// when <c>false</c> only default registrations are allowed, any attempt to use named resolutions throws an exception
      /// by default.
      /// </value>
      /// <remarks>
      /// When the value changes from <c>true</c> to <c>false</c>; the collection of named registrations is emptied for this
      /// <see cref="IIocContainer" />.
      /// </remarks>
      bool AllowNamedImplementations { get; set; }

       /// <summary>
      /// Gets or sets a value governing the behavior of precluded types.
      /// </summary>
      /// <value>
      /// When <c>true</c>types can be precluded from registration and/or resolution;
      /// When <c>false</c>, attempting to preclude a type throws an exception.
      /// </value>
      /// <remarks>
      /// When the value changes from <c>true</c> to <c>false</c>; the collection of precluded types is emptied for this
      /// <see cref="IIocContainer" />.
      /// </remarks>
      bool AllowPreclusionOfTypes { get; set; }

       /// <summary>
      /// Gets the container to which this configuration applies.
      /// </summary>
      /// <value>
      /// The container to which this configuration applies.
      /// </value>
      IIocContainerMetaIdentity Container { get; }

       /// <summary>
      /// Gets or sets a value governing the behavior when a registration collision occurs.
      /// </summary>
      /// <value>
      /// When <c>true</c>, a registration type/name collection throws and exceptions;
      /// when <c>false</c>, a registration type/name collision lets the last updater win.
      /// </value>
      /// <remarks>
      /// This setting has no affect on the behavior of
      /// <see cref="IIocContainerRegistrar.TryRegisterImplementation(Type, Type)" /> and
      /// <see cref="IIocContainerRegistrar.TryRegisterInstance(Type, Object)" /> and their overloads.
      /// </remarks>
      bool ThrowOnRegistrationCollision { get; set; }
   }
}
