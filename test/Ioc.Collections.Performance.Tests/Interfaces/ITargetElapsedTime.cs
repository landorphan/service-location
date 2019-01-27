namespace Ioc.Collections.Performance.Tests
{
   using System;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   public interface ITargetElapsedTime : IIocContainerConfiguration
   {
      void GetRegistrationOverwriteStats(out TimeSpan registrationOverwriteTime, out Int32 registrationOverwriteCount);
      void GetRegistrationTotalStats(out TimeSpan registrationTotalTime, out Int32 registrationTotalCount);
      void GetRegistrationValidationStats(out TimeSpan registrationValidationTime);
      
      void GetResolutionTotalStats(out TimeSpan resolutionTotalTime, out Int32 resolutionTotalCount);
      void GetResolutionOverwriteStats(out TimeSpan resolutionOverwriteTime, out Int32 resolutionNewInstancesCount);
      void GetResolutionValidationStats(out TimeSpan resolutionValidationTime);

      // ReSharper disable IdentifierTypo
      void GetUnregistrationStats(out TimeSpan unregistrationTotalTime, out Int32 unregistrationTotalCount);
   }
}
