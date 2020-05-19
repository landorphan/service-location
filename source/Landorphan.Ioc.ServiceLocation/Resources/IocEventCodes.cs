namespace Landorphan.Ioc.Resources
{
    internal static class IocEventIdCodes
    {
        internal static class IocContainer
        {
            internal const int ChildContainerAdded = 6001;
            internal const int ChildContainerRemoved = 6002;
            internal const int ConfigurationChanged = 6007;
            internal const int PrecludedTypeAdded = 6003;
            internal const int PrecludedTypeRemoved = 6004;
            internal const int RegistrationAdded = 6005;
            internal const int RegistrationRemoved = 6006;
        }

        internal static class ServiceLocator
        {
            internal const int AmbientContainerChanged = 5005;
            internal const int ContainerAssemblyCollectionSelfRegistrationInvokedBefore = 5001;
            internal const int ContainerAssemblyCollectionSelfRegistrationsInvokedAfter = 5004;
            internal const int ContainerSingleAssemblySelfRegistrationInvokedAfter = 5002;
            internal const int ContainerSingleAssemblySelfRegistrationsInvokedBefore = 5003;
        }
    }
}
