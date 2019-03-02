namespace Ioc.Collections.Performance.Tests
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   public interface IRegistrationTarget : IIocContainerMetaIdentity, ITargetElapsedTime
   {
      Boolean AddPrecludedType(Type precludedType);
      Boolean RegisterImplementationImplementation(Type fromType, String fromTypeParameterName, String name, Type toType, String toTypeParameterName, Boolean tryLogic);
      Boolean RegisterInstanceImplementation(Type fromType, String fromTypeParameterName, String name, Object instance, String instanceParameterName, Boolean tryLogic);
      Boolean RemovePrecludedType(Type precludedType);

      // TODO: CA1007 is firing on the build server, despite this suppression, it does not fire locally.
      [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
      Boolean ResolveImplementation(Type fromType, String fromTypeParameterName, String name, Boolean tryLogic, out Object instance);

      Boolean UnregisterImplementation(Type fromType, String name);
   }
}
