namespace Landorphan.Ioc.Logging.Internal
{
   using System;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Represents logging utilities specific to <see cref="Landorphan.Ioc.ServiceLocation.Internal.IocContainer"/> instances.
   /// </summary>
   /// <remarks>
   /// Translates app domain objects into Microsoft.Extensions.Logging interface implementations.
   /// </remarks>
   internal interface ILoggingUtilitiesForIocContainer
   {
      void GetMessageChildContainerAdded(IIocContainerMetaIdentity parentContainer, IIocContainerMetaIdentity childContainer, out String message);
      void GetMessageChildContainerRemoved(IIocContainerMetaIdentity parentContainer, IIocContainerMetaIdentity childContainer, out String message);
      void GetMessageConfigurationChanged(IIocContainerConfiguration configuration, out String message);
      void GetMessagePrecludedTypeAdded(IIocContainerMetaIdentity container, Type precludedType, out String message);
      void GetMessagePrecludedTypeRemoved(IIocContainerMetaIdentity container, Type precludedType, out String message);
      void GetMessageRegistrationAdded(IIocContainerMetaIdentity container, IRegistrationKey registrationKey, Type toType, Object instance, out String message);
      void GetMessageRegistrationRemoved(IIocContainerMetaIdentity container, IRegistrationKey registrationKey, out String message);
   }
}
