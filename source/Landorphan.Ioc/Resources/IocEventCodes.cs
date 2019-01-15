namespace Landorphan.Ioc.Resources
{
   using System;

   internal static class IocEventIdCodes
   {
      internal static class IocContainer
      {
         internal const Int32 ChildContainerAdded = 6001;
         internal const Int32 ChildContainerRemoved = 6002;
         internal const Int32 ConfigurationChanged = 6007;
         internal const Int32 PrecludedTypeAdded = 6003;
         internal const Int32 PrecludedTypeRemoved = 6004;
         internal const Int32 RegistrationAdded = 6005;
         internal const Int32 RegistrationRemoved = 6006;
      }

      internal static class ServiceLocator
      {
         internal const Int32 AmbientContainerChanged = 5005;
         internal const Int32 ContainerAssemblyCollectionSelfRegistrationInvokedBefore = 5001;
         internal const Int32 ContainerAssemblyCollectionSelfRegistrationsInvokedAfter = 5004;
         internal const Int32 ContainerSingleAssemblySelfRegistrationInvokedAfter = 5002;
         internal const Int32 ContainerSingleAssemblySelfRegistrationsInvokedBefore = 5003;
      }
   }
}
