namespace Landorphan.Ioc.Tests.Mocks
{
    using System;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    public sealed class MockContainerImplementingIIocContainerMetaIdentity : IIocContainerMetaIdentity
    {
        public MockContainerImplementingIIocContainerMetaIdentity(string name, Guid uid)
        {
            Name = name;
            Uid = uid;
        }

        public string Name { get; }

        public Guid Uid { get; }
    }
}
