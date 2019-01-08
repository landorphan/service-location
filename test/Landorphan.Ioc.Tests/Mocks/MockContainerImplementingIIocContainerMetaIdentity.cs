namespace Landorphan.Ioc.Tests.Mocks
{
   using System;
   using Landorphan.Ioc.ServiceLocation;

   public sealed class MockContainerImplementingIIocContainerMetaIdentity : IIocContainerMetaIdentity
   {
      public MockContainerImplementingIIocContainerMetaIdentity(String name, Guid uid)
      {
         Name = name;
         Uid = uid;
      }

      public String Name { get; }

      public Guid Uid { get; }
   }
}