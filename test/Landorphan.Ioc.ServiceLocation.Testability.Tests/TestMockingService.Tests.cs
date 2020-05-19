namespace Landorphan.Ioc.ServiceLocation.Testability.Tests
{
    using FluentAssertions;
    using Landorphan.Common.Interfaces;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

    public static class TestMockingService_Tests
    {
        [TestClass]
        public class The_TestMockingService_should_be_automatically_registered_with_Ioc : TestBase
        {
            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void Verify()
            {
                IocServiceLocator.RootContainer.Registrar.IsRegistered<ITestMockingService>().Should().BeTrue();
                var actual = IocServiceLocator.Resolve<ITestMockingService>();
                actual.Should().BeOfType<TestMockingService>();
            }
        }

        [TestClass]
        public class When_I_have_a_TestMockingService : ArrangeActAssert
        {
            private ITestMockingService target;

            protected override void ArrangeMethod()
            {
                target = IocServiceLocator.Resolve<ITestMockingService>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_ApplyNoMocking_the_AmbientContainer_should_be_the_RootContainer()
            {
                target.ApplyNoMocking();
                IocServiceLocator.AmbientContainer.Should().BeSameAs(target.RootContainer);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_ApplyTestInstanceMockingOnly_the_AmbientContainer_should_be_the_TestInstanceOnRootContainer()
            {
                target.ApplyTestInstanceMockingOnly();
                IocServiceLocator.AmbientContainer.Should().BeSameAs(target.TestInstanceOnRootContainer);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_ApplyTestInstanceMockingOnTopOfTestRunMocking_the_AmbientContainer_should_be_the_TestInstanceOnTestRunOnRootContainer()
            {
                target.ApplyTestInstanceMockingOnTopOfTestRunMocking();
                IocServiceLocator.AmbientContainer.Should().BeSameAs(target.TestInstanceOnTestRunOnRootContainer);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_ApplyTestRunMockingOnly_the_AmbientContainer_should_be_the_TestRunContainer()
            {
                target.ApplyTestRunMockingOnly();
                IocServiceLocator.AmbientContainer.Should().BeSameAs(target.TestRunContainer);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void And_I_call_ResetIndividualTestContainers_it_should_reset_the_test_instance_containers()
            {
                var wasTestInstanceOnRoot = target.TestInstanceOnRootContainer;
                var wasTestInstanceOnTestRunOnRoot = target.TestInstanceOnTestRunOnRootContainer;

                target.ResetIndividualTestContainers();

                var updatedTestInstanceOnRoot = target.TestInstanceOnRootContainer;
                var updatedTestInstanceOnTestRunOnRoot = target.TestInstanceOnTestRunOnRootContainer;

                (wasTestInstanceOnRoot as IQueryDisposable)?.IsDisposed.Should().BeTrue();
                updatedTestInstanceOnRoot.Should().NotBeSameAs(wasTestInstanceOnRoot);

                (wasTestInstanceOnTestRunOnRoot as IQueryDisposable)?.IsDisposed.Should().BeTrue();
                updatedTestInstanceOnTestRunOnRoot.Should().NotBeSameAs(wasTestInstanceOnTestRunOnRoot);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_have_a_RootContainer()
            {
                target.RootContainer.Should().BeSameAs(IocServiceLocator.RootContainer);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_have_a_TestInstanceOnRootContainer()
            {
                var testOnRoot = target.TestInstanceOnRootContainer;
                testOnRoot.Should().NotBeNull();
                testOnRoot.Parent.Should().BeSameAs(target.RootContainer);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_have_a_TestInstanceOnTestRunOnRootContainer()
            {
                var testInstanceOnTestRunRunOnRoot = target.TestInstanceOnTestRunOnRootContainer;
                testInstanceOnTestRunRunOnRoot.Should().NotBeNull();
                testInstanceOnTestRunRunOnRoot.Parent.Should().BeSameAs(target.TestRunContainer);
                testInstanceOnTestRunRunOnRoot.Parent.Parent.Should().BeSameAs(target.RootContainer);
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_have_a_TestRunContainer()
            {
                var testRunOnRoot = target.TestRunContainer;
                testRunOnRoot.Should().NotBeNull();
                testRunOnRoot.Parent.Should().BeSameAs(target.RootContainer);
            }
        }
    }
}
