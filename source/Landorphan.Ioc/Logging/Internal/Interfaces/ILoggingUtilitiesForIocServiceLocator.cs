namespace Landorphan.Ioc.Logging.Internal
{
   using System;
   using System.Collections.Generic;
   using System.Reflection;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Represents logging utilities specific to the<see cref="IocServiceLocator"/> singleton instance.
   /// </summary>
   /// <remarks>
   /// Translates app domain objects into Microsoft.Extensions.Logging interface implementations.
   /// </remarks>
   internal interface ILoggingUtilitiesForIocServiceLocator
   {
      void GetMessageAmbientContainerChanged(IIocContainerMetaIdentity newContainer, out String message);
      void GetMessageContainerAssemblyCollectionSelfRegistrationInvokedAfter(IIocContainerMetaIdentity container, IEnumerable<Assembly> assemblies, out String message);
      void GetMessageContainerAssemblyCollectionSelfRegistrationInvokedBefore(IIocContainerMetaIdentity container, IEnumerable<Assembly> assemblies, out String message);
      void GetMessageContainerSingleAssemblySelfRegistrationInvokedAfter(IIocContainerMetaIdentity container, IAssemblySelfRegistration assemblySelfRegistration, out String message);
      void GetMessageContainerSingleAssemblySelfRegistrationInvokedBefore(IIocContainerMetaIdentity container, IAssemblySelfRegistration assemblySelfRegistration, out String message);
   }
}
