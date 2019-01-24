namespace Landorphan.Ioc.ServiceLocation.Testability
{
   using System;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// Represents the mocking capabilities of the test subsystem.
   /// </summary>
   /// <remarks>
   /// Container Structure:
   ///   (root)   -> (TestRunContainerName) -> (TestInstanceOnTestRunOnRootContainerName)
   ///            |
   ///            -> (TestInstanceOnRootContainerName)
   /// Resolution evaluates the current ambient container, then the chain of parent containers.
   /// </remarks>
   public sealed class TestMockingService : DisposableObject, ITestMockingService
   {
      private const String TestInstanceOnRootContainerName = "IoC Test Instance Container on IoC Root Container";
      private const String TestInstanceOnTestRunOnRootContainerName = "IoC Test Instance Container on IoC Full Test Run Container on IoC Root Container";
      private const String TestRunContainerName = "IoC Full Test Run Container";

      private readonly Object _syncRoot = new Object();

      private readonly IOwnedIocContainer _testRunContainer;
      private IOwnedIocContainer _testInstanceOnRootContainer;
      private IOwnedIocContainer _testInstanceOnTestRunOnRootContainer;

      internal TestMockingService()
      {
         RootContainer = IocServiceLocator.RootContainer;
         _testRunContainer = (IOwnedIocContainer)RootContainer.Manager.CreateChildContainer(TestRunContainerName);
         _testInstanceOnTestRunOnRootContainer = (IOwnedIocContainer)TestRunContainer.Manager.CreateChildContainer(TestInstanceOnTestRunOnRootContainerName);
         _testInstanceOnRootContainer = (IOwnedIocContainer)RootContainer.Manager.CreateChildContainer(TestInstanceOnRootContainerName);
      }

      /// <inheritdoc/>
      public IIocContainer RootContainer { get; }

      /// <inheritdoc/>
      public IIocContainer TestInstanceOnRootContainer => _testInstanceOnRootContainer;

      /// <inheritdoc/>
      public IIocContainer TestInstanceOnTestRunOnRootContainer => _testInstanceOnTestRunOnRootContainer;

      /// <inheritdoc/>
      public IIocContainer TestRunContainer => _testRunContainer;

      /// <inheritdoc/>
      public void ApplyNoMocking()
      {
         IocServiceLocator.Instance.Manager.SetAmbientContainer(RootContainer);
      }

      /// <inheritdoc/>
      public void ApplyTestInstanceMockingOnly()
      {
         IocServiceLocator.Instance.Manager.SetAmbientContainer(TestInstanceOnRootContainer);
      }

      /// <inheritdoc/>
      public void ApplyTestInstanceMockingOnTopOfTestRunMocking()
      {
         IocServiceLocator.Instance.Manager.SetAmbientContainer(TestInstanceOnTestRunOnRootContainer);
      }

      /// <inheritdoc/>
      public void ApplyTestRunMockingOnly()
      {
         IocServiceLocator.Instance.Manager.SetAmbientContainer(TestRunContainer);
      }

      /// <inheritdoc/>
      public void ResetIndividualTestContainers()
      {
         lock (_syncRoot)
         {
            _testInstanceOnTestRunOnRootContainer.Dispose();
            _testInstanceOnTestRunOnRootContainer = (IOwnedIocContainer)((IIocContainerManager)TestRunContainer).CreateChildContainer(TestInstanceOnTestRunOnRootContainerName);

            _testInstanceOnRootContainer.Dispose();
            _testInstanceOnRootContainer = (IOwnedIocContainer)((IIocContainerManager)RootContainer).CreateChildContainer(TestInstanceOnRootContainerName);
         }
      }
   }
}
