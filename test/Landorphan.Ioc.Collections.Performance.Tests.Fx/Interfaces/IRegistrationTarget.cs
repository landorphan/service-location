namespace Ioc.Collections.Performance.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    public interface IRegistrationTarget : IIocContainerMetaIdentity, ITargetElapsedTime
   {
       bool AddPrecludedType(Type precludedType);
       bool RegisterImplementationImplementation(Type fromType, string fromTypeParameterName, string name, Type toType, string toTypeParameterName, bool tryLogic);
       bool RegisterInstanceImplementation(Type fromType, string fromTypeParameterName, string name, object instance, string instanceParameterName, bool tryLogic);
       bool RemovePrecludedType(Type precludedType);

       // TODO: CA1007 is firing on the build server, despite this suppression, it does not fire locally.
       [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
      bool ResolveImplementation(Type fromType, string fromTypeParameterName, string name, bool tryLogic, out object instance);

      bool UnregisterImplementation(Type fromType, string name);
   }
}
