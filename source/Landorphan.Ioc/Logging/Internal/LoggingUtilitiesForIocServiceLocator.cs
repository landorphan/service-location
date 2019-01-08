namespace Landorphan.Ioc.Logging.Internal
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Globalization;
   using System.Linq;
   using System.Reflection;
   using System.Text;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Logging utilities specific to the<see cref="IocServiceLocator"/> singleton instance.   
   /// </summary>
   /// <remarks>
   /// Translates app domain objects into Microsoft.Extensions.Logging interface implementations.
   /// </remarks>
   internal sealed class LoggingUtilitiesForIocServiceLocator : ILoggingUtilitiesForIocServiceLocator
   {
      private readonly IIocLoggingUtilitiesService _parent;

      internal LoggingUtilitiesForIocServiceLocator(IIocLoggingUtilitiesService parent)
      {
         parent.ArgumentNotNull(nameof(parent));
         _parent = parent;
      }

      public void GetMessageAmbientContainerChanged(IIocContainerMetaIdentity newContainer, out String message)
      {
         newContainer.ArgumentNotNull(nameof(newContainer));

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var containerUid = newContainer.Uid.ToString("D", CultureInfo.InvariantCulture);
         var containerName = newContainer.Name;

         message = $"Service Locator: Ambient container changed\t{timestamp}\t{threadId}\tContainerUid:{containerUid}\tContainerName:{containerName}";
      }

      public void GetMessageContainerAssemblyCollectionSelfRegistrationInvokedAfter(IIocContainerMetaIdentity container, IEnumerable<Assembly> assemblies, out String message)
      {
         container.ArgumentNotNull(nameof(container));
         // ReSharper disable once PossibleMultipleEnumeration
         assemblies.ArgumentNotNullNorContainsNull(nameof(assemblies));
         // ReSharper disable once PossibleMultipleEnumeration
         var workingAssemblies = assemblies.ToList();

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var containerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
         var containerName = container.Name;
         var assemblyNames = (from a in workingAssemblies select a.FullName).ToImmutableSortedSet();
         var builder = new StringBuilder();
         builder.AppendLine(
            "Service Locator: After assemblies collection IAssemblySelfRegistration instances invoked" +
            $"\t{timestamp}\t{threadId}\tContainerUid:{containerUid}\tContainerName:{containerName}");
         foreach (var fullName in assemblyNames)
         {
            builder.AppendLine($"\tAssemblyName:{fullName}");
         }

         message = builder.ToString();
      }

      public void GetMessageContainerAssemblyCollectionSelfRegistrationInvokedBefore(IIocContainerMetaIdentity container, IEnumerable<Assembly> assemblies, out String message)
      {
         container.ArgumentNotNull(nameof(container));
         // ReSharper disable once PossibleMultipleEnumeration
         assemblies.ArgumentNotNullNorContainsNull(nameof(assemblies));
         // ReSharper disable once PossibleMultipleEnumeration
         var workingAssemblies = assemblies.ToList();

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var containerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
         var containerName = container.Name;
         var assemblyNames = (from a in workingAssemblies select a.FullName).ToImmutableSortedSet();
         var builder = new StringBuilder();
         builder.AppendLine(
            "Service Locator: Before assemblies collection IAssemblySelfRegistration instances invoked" +
            $"\t{timestamp}\t{threadId}\tContainerUid:{containerUid}\tContainerName:{containerName}");
         foreach (var fullName in assemblyNames)
         {
            builder.AppendLine($"\tAssemblyName:{fullName}");
         }

         message = builder.ToString();
      }

      public void GetMessageContainerSingleAssemblySelfRegistrationInvokedAfter(
         IIocContainerMetaIdentity container,
         IAssemblySelfRegistration assemblySelfRegistration,
         out String message)
      {
         container.ArgumentNotNull(nameof(container));
         assemblySelfRegistration.ArgumentNotNull(nameof(assemblySelfRegistration));

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var containerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
         var containerName = container.Name;
         var registrarName = assemblySelfRegistration.GetType().FullName;

         message = $"Service Locator: After single IAssemblySelfRegistration instance Invoked\t{timestamp}\t{threadId}\tContainerUid:{containerUid}\tContainerName:{containerName}\t" +
                   $"IAssemblySelfRegistration:{registrarName}";
      }

      public void GetMessageContainerSingleAssemblySelfRegistrationInvokedBefore(
         IIocContainerMetaIdentity container,
         IAssemblySelfRegistration assemblySelfRegistration,
         out String message)
      {
         container.ArgumentNotNull(nameof(container));
         assemblySelfRegistration.ArgumentNotNull(nameof(assemblySelfRegistration));

         var timestamp = _parent.GetTimestamp();
         var threadId = _parent.GetThreadId();
         var containerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
         var containerName = container.Name;
         var registrarName = assemblySelfRegistration.GetType().FullName;

         message = $"Service Locator: Before single IAssemblySelfRegistration instance Invoked\t{timestamp}\t{threadId}\tContainerUid:{containerUid}\tContainerName:{containerName}\t" +
                   $"IAssemblySelfRegistration:{registrarName}";
      }
   }
}