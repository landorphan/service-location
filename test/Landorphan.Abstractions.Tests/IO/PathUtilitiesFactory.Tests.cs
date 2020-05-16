namespace Landorphan.Abstractions.Tests.IO
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Landorphan.Abstractions.IO;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Common;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

    [TestClass]
    public class When_I_call_PathUtilitiesFactory_Create : ArrangeActAssert
    {
        private readonly PathUtilitiesFactory target = new PathUtilitiesFactory();
        private IPathUtilities actual;

        protected override void ActMethod()
        {
            actual = target.Create();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void It_should_create_an_IPathUtilities_instance()
        {
            actual.Should().BeAssignableTo<IPathUtilities>();
        }
    }

    [TestClass]
    public class When_I_call_PathUtilitiesFactory_Create_multiple_times : ArrangeActAssert
    {
        private readonly PathUtilitiesFactory target = new PathUtilitiesFactory();
        private HashSet<IPathUtilities> actuals;

        protected override void ArrangeMethod()
        {
            actuals = new HashSet<IPathUtilities>(new ReferenceEqualityComparer<IPathUtilities>());
        }

        protected override void ActMethod()
        {
            actuals.Add(target.Create());
            actuals.Add(target.Create());
            actuals.Add(target.Create());
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void It_should_return_a_new_instance_each_time()
        {
            actuals.Count.Should().Be(3);
        }
    }

    [TestClass]
    public class When_I_service_locate_IPathUtilitiesFactory : ArrangeActAssert
    {
        private IPathUtilitiesFactory actual;

        protected override void ActMethod()
        {
            actual = IocServiceLocator.Resolve<IPathUtilitiesFactory>();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void It_should_give_me_a_PathUtilitiesFactory()
        {
            actual.Should().BeOfType<PathUtilitiesFactory>();
        }
    }
}
