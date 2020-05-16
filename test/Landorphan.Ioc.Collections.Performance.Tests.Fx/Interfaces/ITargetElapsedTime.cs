namespace Ioc.Collections.Performance.Tests
{
    using System;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    public interface ITargetElapsedTime : IIocContainerConfiguration
   {
       void GetRegistrationOverwriteStats(out TimeSpan registrationOverwriteTime, out int registrationOverwriteCount);
       void GetRegistrationTotalStats(out TimeSpan registrationTotalTime, out int registrationTotalCount);
       void GetRegistrationValidationStats(out TimeSpan registrationValidationTime);
       void GetResolutionOverwriteStats(out TimeSpan resolutionOverwriteTime, out int resolutionNewInstancesCount);

       void GetResolutionTotalStats(out TimeSpan resolutionTotalTime, out int resolutionTotalCount);
       void GetResolutionValidationStats(out TimeSpan resolutionValidationTime); // ReSharper disable IdentifierTypo
       void GetUnregistrationStats(out TimeSpan unregistrationTotalTime, out int unregistrationTotalCount);
   }
}
