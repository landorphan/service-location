namespace Landorphan.Ioc.Logging.Internal
{
   using System;
   using System.Globalization;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Logging utilities specific to the<see cref="IocServiceLocator"/> singleton instance.   
   /// </summary>
   /// <remarks>
   /// Translates app domain objects into Microsoft.Extensions.Logging interface implementations.
   /// </remarks>
   internal sealed class LoggingUtilitiesForIocContainer : ILoggingUtilitiesForIocContainer
   {
      private readonly IIocLoggingUtilitiesService _parent;

      internal LoggingUtilitiesForIocContainer(IIocLoggingUtilitiesService parent)
      {
         parent.ArgumentNotNull(nameof(parent));
         _parent = parent;
      }

      public void GetMessageChildContainerAdded(IIocContainerMetaIdentity parentContainer, IIocContainerMetaIdentity childContainer, out String message)
      {
         parentContainer.ArgumentNotNull(nameof(parentContainer));
         childContainer.ArgumentNotNull(nameof(childContainer));

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var parentContainerUid = parentContainer.Uid.ToString("D", CultureInfo.InvariantCulture);
         var parentContainerName = parentContainer.Name;
         var childContainerUid = childContainer.Uid.ToString("D", CultureInfo.InvariantCulture);
         var childContainerName = childContainer.Name;

         message = $"Container: Child container added\t{timestamp}\t{threadId}" +
                   $"\tParentContainerUid:{parentContainerUid}\tParentContainerName:{parentContainerName}" +
                   $"\tChildContainerUid:{childContainerUid}\tChildContainerName:{childContainerName}";
      }

      public void GetMessageChildContainerRemoved(IIocContainerMetaIdentity parentContainer, IIocContainerMetaIdentity childContainer, out String message)
      {
         parentContainer.ArgumentNotNull(nameof(parentContainer));
         childContainer.ArgumentNotNull(nameof(childContainer));

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var parentContainerUid = parentContainer.Uid.ToString("D", CultureInfo.InvariantCulture);
         var parentContainerName = parentContainer.Name;
         var childContainerUid = childContainer.Uid.ToString("D", CultureInfo.InvariantCulture);
         var childContainerName = childContainer.Name;

         message = $"Container: Child container removed\t{timestamp}\t{threadId}" +
                   $"\tParentContainerUid:{parentContainerUid}\tParentContainerName:{parentContainerName}" +
                   $"\tChildContainerUid:{childContainerUid}\tChildContainerName:{childContainerName}";
      }

      public void GetMessageConfigurationChanged(IIocContainerConfiguration configuration, out String message)
      {
         configuration.ArgumentNotNull(nameof(configuration));

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();

         message = $"Container: Configuration changed\t{timestamp}\t{threadId}" +
                   $"\tContainerUid:{configuration.Container.Uid}\tContainerName:{configuration.Container.Name}" +
                   $"\tAllowNamedImplementations:{configuration.AllowNamedImplementations}" +
                   $"\tAllowPreclusionOfTypes:{configuration.AllowPreclusionOfTypes}" +
                   $"\tThrowOnRegistrationCollision:{configuration.ThrowOnRegistrationCollision}" +
                   $"\tIsReadOnly:{configuration.IsReadOnly}";
      }

      public void GetMessagePrecludedTypeAdded(IIocContainerMetaIdentity container, Type precludedType, out String message)
      {
         container.ArgumentNotNull(nameof(container));
         precludedType.ArgumentNotNull(nameof(precludedType));

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var containerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
         var containerName = container.Name;
         var precludedTypeFullName = precludedType.FullName;

         message = $"Container: Precluded type added\t{timestamp}\t{threadId}" +
                   $"\tContainerUid:{containerUid}\tContainerName:{containerName}" +
                   $"\tPrecludedType:{precludedTypeFullName}";
      }

      public void GetMessagePrecludedTypeRemoved(IIocContainerMetaIdentity container, Type precludedType, out String message)
      {
         container.ArgumentNotNull(nameof(container));
         precludedType.ArgumentNotNull(nameof(precludedType));

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var containerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
         var containerName = container.Name;
         var precludedTypeFullName = precludedType.FullName;

         message = $"Container: Precluded type removed\t{timestamp}\t{threadId}" +
                   $"\tContainerUid:{containerUid}\tContainerName:{containerName}" +
                   $"\tPrecludedType:{precludedTypeFullName}";
      }

      public void GetMessageRegistrationAdded(IIocContainerMetaIdentity container, IRegistrationKey registrationKey, Type toType, Object instance, out String message)
      {
         container.ArgumentNotNull(nameof(container));
         registrationKey.ArgumentNotNull(nameof(registrationKey));

         // toType and instance may be null

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var containerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
         var containerName = container.Name;

         if (toType != null)
         {
            message = $"Container: Registration added (implementation type)\t{timestamp}\t{threadId}" +
                      $"\tContainerUid:{containerUid}\tContainerName:{containerName}" +
                      $"\tRegisteredType:{registrationKey.RegisteredType.FullName}\tRegisteredName:{registrationKey.RegisteredName}" +
                      $"\tImplementationType:{toType.FullName}";
         }
         else
         {
            message = instance != null
               ? $"Container: Registration added (instance)\t{timestamp}\t{threadId}" +
                 $"\tContainerUid:{containerUid}\tContainerName:{containerName}" +
                 $"\tRegisteredType:{registrationKey.RegisteredType.FullName}\tRegisteredName:{registrationKey.RegisteredName}" +
                 $"\tInstanceType:{instance.GetType().FullName}"
               : $"Container: Registration added (unknown implementation)\t{timestamp}\t{threadId}" +
                 $"\tContainerUid:{containerUid}\tContainerName:{containerName}" +
                 $"\tRegisteredType:{registrationKey.RegisteredType.FullName}\tRegisteredName:{registrationKey.RegisteredName}";
         }
      }

      public void GetMessageRegistrationRemoved(IIocContainerMetaIdentity container, IRegistrationKey registrationKey, out String message)
      {
         container.ArgumentNotNull(nameof(container));
         registrationKey.ArgumentNotNull(nameof(registrationKey));

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var containerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
         var containerName = container.Name;

         message = $"Container: Registration removed\t{timestamp}\t{threadId}" +
                   $"\tContainerUid:{containerUid}\tContainerName:{containerName}" +
                   $"\tRegisteredType:{registrationKey.RegisteredType.FullName}\tRegisteredName:{registrationKey.RegisteredName}";
      }
   }
}
