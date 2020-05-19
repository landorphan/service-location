namespace Landorphan.Abstractions.Tests.IO.Factories
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Landorphan.Abstractions.IO;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Common;
    using Landorphan.Ioc.ServiceLocation;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

    public static class FileWriterUtilitiesFactory_Tests
    {
        [TestClass]
        public class When_I_call_FileWriterUtilitiesFactory_Create : ArrangeActAssert
        {
            private readonly FileWriterUtilitiesFactory target = new FileWriterUtilitiesFactory();
            private IFileWriterUtilities actual;

            protected override void ActMethod()
            {
                actual = target.Create();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_create_an_IFileWriterUtilities_instance()
            {
                actual.Should().BeAssignableTo<IFileWriterUtilities>();
            }
        }

        [TestClass]
        public class When_I_call_FileWriterUtilitiesFactory_Create_multiple_times : ArrangeActAssert
        {
            private readonly FileWriterUtilitiesFactory target = new FileWriterUtilitiesFactory();
            private HashSet<IFileWriterUtilities> actuals;

            protected override void ArrangeMethod()
            {
                actuals = new HashSet<IFileWriterUtilities>(new ReferenceEqualityComparer<IFileWriterUtilities>());
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
        public class When_I_service_locate_IFileWriterUtilitiesFactory : ArrangeActAssert
        {
            private IFileWriterUtilitiesFactory actual;

            protected override void ActMethod()
            {
                actual = IocServiceLocator.Resolve<IFileWriterUtilitiesFactory>();
            }

            [TestMethod]
            [TestCategory(TestTiming.CheckIn)]
            public void It_should_give_me_a_FileWriterUtilitiesFactory()
            {
                actual.Should().BeOfType<FileWriterUtilitiesFactory>();
            }
        }
    }
}
