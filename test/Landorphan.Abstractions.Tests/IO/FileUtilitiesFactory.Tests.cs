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

    public static class FileUtilitiesFactory_Tests
    {
        [TestClass]
        public class When_I_call_FileUtilitiesFactory_Create : ArrangeActAssert
        {
            private readonly FileUtilitiesFactory target = new FileUtilitiesFactory();
            private IFileUtilities actual;

            protected override void ActMethod()
            {
                actual = target.Create();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_create_an_IFileUtilities_instance()
            {
                actual.Should().BeAssignableTo<IFileUtilities>();
            }
        }

        [TestClass]
        public class When_I_call_FileUtilitiesFactory_Create_multiple_times : ArrangeActAssert
        {
            private readonly FileUtilitiesFactory target = new FileUtilitiesFactory();
            private HashSet<IFileUtilities> actuals;

            protected override void ArrangeMethod()
            {
                actuals = new HashSet<IFileUtilities>(new ReferenceEqualityComparer<IFileUtilities>());
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
        public class When_I_service_locate_IFileUtilitiesFactory : ArrangeActAssert
        {
            private IFileUtilitiesFactory actual;

            protected override void ActMethod()
            {
                actual = IocServiceLocator.Resolve<IFileUtilitiesFactory>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_give_me_a_FileUtilitiesFactory()
            {
                actual.Should().BeOfType<FileUtilitiesFactory>();
            }
        }
    }
}
