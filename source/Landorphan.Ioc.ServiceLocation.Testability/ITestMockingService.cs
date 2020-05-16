namespace Landorphan.Ioc.ServiceLocation.Testability
{
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    /// <summary>
   /// Represents the mocking capabilities of the test subsystem.
   /// </summary>
   /// <remarks>
   /// |  Grandparent Container |  Parent Container | Leaf Container                       |
   /// | :--------------------- | :---------------- | :----------------------------------- |
   /// | RootContainer          | TestRunContainer  | TestInstanceOnTestRunOnRootContainer |
   /// | RootContainer          | -                 | TestInstanceOnRootContainer          |
   /// <para>
   /// The default implementation of <see cref="ITestMockingService"/> adds 3 containers to the root container configured as above.
   /// </para>
   /// <para>
   /// Resolution evaluates the current ambient container, then the chain of parent containers up to the
   /// root (production) container.
   /// </para>
   /// <para>
   /// No containers are added for single test methods.  Experience has shown they are not needed.  If you feel differently, you can achieve the result in one of two ways:
   /// 1. Call <see cref="ResetIndividualTestContainers"/> in your test method, this resets both <see cref="TestInstanceOnTestRunOnRootContainer"/>
   /// and <see cref="TestInstanceOnRootContainer"/> and arrange your test; or
   /// 2. Call add your own containers, for example, you could call ITestMockingService.TestInstanceOnRootContainer.Manager.CreateChildContainer("some name")
   /// </para>
   /// </remarks>
   // docfx 2.40.8.0 does not seem to handle <list type="table"> correctly, but MD tables work just fine as long as the table is the first element in remarks :)
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
      void ResetIndividualTestContainers();
   }
}
