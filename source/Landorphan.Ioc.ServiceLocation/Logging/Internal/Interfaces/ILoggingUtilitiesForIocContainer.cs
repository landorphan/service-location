namespace Landorphan.Ioc.Logging.Internal
{
    using System;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    /// <summary>
   /// Represents logging utilities specific to <see cref="Landorphan.Ioc.ServiceLocation.Internal.IocContainer"/> instances.
   /// </summary>
   /// <remarks>
   /// Translates app domain objects into Microsoft.Extensions.Logging interface implementations.
   /// </remarks>
   internal interface ILoggingUtilitiesForIocContainer
   {
       void GetMessageChildContainerAdded(IIocContainerMetaIdentity parentContainer, IIocContainerMetaIdentity childContainer, out string message);
       void GetMessageChildContainerRemoved(IIocContainerMetaIdentity parentContainer, IIocContainerMetaIdentity childContainer, out string message);
       void GetMessageConfigurationChanged(IIocContainerConfiguration configuration, out string message);
       void GetMessagePrecludedTypeAdded(IIocContainerMetaIdentity container, Type precludedType, out string message);
       void GetMessagePrecludedTypeRemoved(IIocContainerMetaIdentity container, Type precludedType, out string message);
       void GetMessageRegistrationAdded(IIocContainerMetaIdentity container, IRegistrationKey registrationKey, Type toType, object instance, out string message);
       void GetMessageRegistrationRemoved(IIocContainerMetaIdentity container, IRegistrationKey registrationKey, out string message);
   }
}
