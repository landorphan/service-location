namespace Landorphan.TestUtilities.WithIoc
{
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
   public interface ITestMockingService
   {
      /// <summary>
      /// Gets the root container.
      /// </summary>
      /// <value>
      /// The root container (no mocking).
      /// </value>
      IIocContainer RootContainer { get; }

      /// <summary>
      /// Gets the test instance container on top of the root container.
      /// </summary>
      /// <value>
      /// The test run instance on top of the root container.
      /// </value>
      IIocContainer TestInstanceOnRootContainer { get; }

      /// <summary>
      /// Gets the container on top of the test run container on top of the root container.
      /// </summary>
      /// <value>
      /// The container on top of the test run container on top of the root container.
      /// </value>
      IIocContainer TestInstanceOnTestRunOnRootContainer { get; }

      /// <summary>
      /// Gets the test run container on top of the root container.
      /// </summary>
      /// <value>
      /// The test run container on top of the root container.
      /// </value>
      IIocContainer TestRunContainer { get; }

      /// <summary>
      /// Removes all mocking.
      /// </summary>
      void ApplyNoMocking();

      /// <summary>
      /// Applies test instance mocking on top of the root container.
      /// </summary>
      void ApplyTestInstanceMockingOnly();

      /// <summary>
      /// Applies test instance mocking on top of test run mocking on top of the root container.
      /// </summary>
      void ApplyTestInstanceMockingOnTopOfTestRunMocking();

      /// <summary>
      /// Applies test run mocking on top of the root container.
      /// </summary>
      void ApplyTestRunMockingOnly();

      /// <summary>
      /// Resets the individual test containers.
      /// </summary>
      /// <remarks>
      /// Use when initializing a test method.  The <see cref="IIocContainer"/> instances dedicated to an individual test method are reset to new instances.
      /// </remarks>
      void ResetIndividualTestContainers();
   }
}