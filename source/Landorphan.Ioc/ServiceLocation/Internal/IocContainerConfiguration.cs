﻿namespace Landorphan.Ioc.ServiceLocation.Internal
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.Common;
   using Landorphan.Common.Threading;

   /// <summary>
   /// Default implementation of <see cref="IIocContainerConfiguration"/>.
   /// </summary>
   [SuppressMessage(
      "SonarLint.CodeSmell",
      "S2933: Fields that are only assigned in the constructor should be readonly",
      Justification = "False positive, these fields are set with impure method calls (MWP).")]
   public sealed class IocContainerConfiguration : IIocContainerConfiguration
   {
      private readonly IIocContainerMetaIdentity _container;
      private readonly SourceWeakEventHandlerSet<EventArgs> _listenersConfigurationChanged = new SourceWeakEventHandlerSet<EventArgs>();
      private InterlockedBoolean _allowNamedImplementations;
      private InterlockedBoolean _allowPreclusionOfTypes;
      private InterlockedBoolean _locked;
      private InterlockedBoolean _throwOnRegistrationCollision;

      /// <summary>
      /// Initializes a new instance of the <see cref="IocContainerConfiguration"/> class.
      /// </summary>
      /// <param name="container">
      /// The container whose configuration this instance represents.
      /// </param>
      public IocContainerConfiguration(IIocContainerMetaIdentity container)
      {
         container.ArgumentNotNull(nameof(container));

         _container = container;

         _allowNamedImplementations = true;
         _allowPreclusionOfTypes = true;
         _throwOnRegistrationCollision = false;
         _locked = false;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="IocContainerConfiguration"/> class.
      /// </summary>
      /// <param name="other">
      /// The instance to clone.
      /// </param>
      /// <remarks>
      /// Neither the event queue for <see cref="IIocContainerConfiguration.ConfigurationChanged"/> nor the value of <see cref="IIocContainerConfiguration.IsLocked"/> is copied.
      /// </remarks>
      internal IocContainerConfiguration(IIocContainerConfiguration other)
      {
         other.ArgumentNotNull(nameof(other));

         _container = other.Container;

         _allowNamedImplementations = other.AllowNamedImplementations;
         _allowPreclusionOfTypes = other.AllowPreclusionOfTypes;
         _throwOnRegistrationCollision = other.ThrowOnRegistrationCollision;

         // Clones are never locked.
         _locked = false;
      }

      /// <inheritdoc/>
      public Object Clone()
      {
         return new IocContainerConfiguration(this);
      }

      /// <summary>
      /// Event queue for all listeners interested changes to the configuration.
      /// </summary>
      /// <remarks>
      /// Occurs just a configuration value is changed.
      /// </remarks>
      public event EventHandler<EventArgs> ConfigurationChanged
      {
         add => _listenersConfigurationChanged.Add(value);
         remove => _listenersConfigurationChanged.Remove(value);
      }

      /// <inheritdoc/>
      public Boolean AllowNamedImplementations
      {
         get => _allowNamedImplementations;
         set
         {
            if (_locked)
            {
               throw new ContainerConfigurationLockedException(_container, null, null);
            }

            var was = _allowNamedImplementations.ExchangeValue(value);
            if (was != value)
            {
               OnConfigurationChanged();
            }
         }
      }

      /// <inheritdoc/>
      public Boolean AllowPreclusionOfTypes
      {
         get => _allowPreclusionOfTypes;
         set
         {
            if (_locked)
            {
               throw new ContainerConfigurationLockedException(_container, null, null);
            }

            var was = _allowPreclusionOfTypes.ExchangeValue(value);
            if (was != value)
            {
               OnConfigurationChanged();
            }
         }
      }

      /// <inheritdoc/>
      public IIocContainerMetaIdentity Container => _container;

      /// <inheritdoc/>
      public Boolean IsLocked => _locked.GetValue();

      /// <inheritdoc/>
      public Boolean ThrowOnRegistrationCollision
      {
         get => _throwOnRegistrationCollision;
         set
         {
            if (_locked)
            {
               throw new ContainerConfigurationLockedException(_container, null, null);
            }

            var was = _throwOnRegistrationCollision.ExchangeValue(value);
            if (was != value)
            {
               OnConfigurationChanged();
            }
         }
      }

      /// <inheritdoc/>
      public Boolean Equals(IIocContainerConfiguration other)
      {
         if (other == null)
         {
            return false;
         }

         var thisContainerUid = _container?.Uid ?? Guid.Empty;
         var otherContainerUid = other.Container?.Uid ?? Guid.Empty;

         return thisContainerUid == otherContainerUid &&
                _allowNamedImplementations.Equals(other.AllowNamedImplementations) &&
                _allowPreclusionOfTypes.Equals(other.AllowPreclusionOfTypes) &&
                _throwOnRegistrationCollision.Equals(other.ThrowOnRegistrationCollision);
      }

      /// <inheritdoc/>
      public Boolean LockConfiguration()
      {
         var was = _locked.ExchangeValue(true);
         if (!was)
         {
            OnConfigurationChanged();
         }

         return was;
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         if (ReferenceEquals(null, obj))
         {
            return false;
         }

         if (ReferenceEquals(this, obj))
         {
            return true;
         }

         return obj is IocContainerConfiguration other && Equals(other);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var thisContainerUid = _container?.Uid ?? Guid.Empty;
            var hashCode = thisContainerUid.GetHashCode();
            hashCode = (hashCode * 397) ^ _allowNamedImplementations.GetHashCode();
            hashCode = (hashCode * 397) ^ _allowPreclusionOfTypes.GetHashCode();
            hashCode = (hashCode * 397) ^ _throwOnRegistrationCollision.GetHashCode();
            return hashCode;
         }
      }

      private void OnConfigurationChanged()
      {
         var e = new EventArgs();
         _listenersConfigurationChanged?.Invoke(this, e);
      }
   }
}