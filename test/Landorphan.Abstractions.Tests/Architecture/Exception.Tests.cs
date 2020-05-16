namespace Landorphan.Abstractions.Tests.Architecture
{
    using Landorphan.Abstractions.Tests.TestFacilities;
    using Landorphan.Ioc.ServiceLocation.Exceptions;
    using Landorphan.TestUtilities;
    using Landorphan.TestUtilities.ReusableTestImplementations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Immutable;
    using System.Reflection;

    // ReSharper disable InconsistentNaming

    [TestClass]
    public class Exception_Tests : ExceptionValidityRequirements
    {
        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void Exceptions_In_DotNet_Core_Should_Not_Be_Marked_As_Serializable()
        {
            Exceptions_In_DotNet_Core_Should_Not_Be_Marked_As_Serializable_Implementation();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void Exceptions_Should_Be_Abstract_Or_Sealed()
        {
            Exceptions_Should_Be_Abstract_Or_Sealed_Implementation();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void Exceptions_Should_Be_Public()
        {
            Exceptions_Should_Be_Public_Implementation();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void Exceptions_Should_Descend_From_An_Acceptable_Base()
        {
            Exceptions_Should_Descend_From_An_Acceptable_Base_Implementation();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void Exceptions_Should_Have_A_Default_Constructor()
        {
            Exceptions_Should_Have_A_Default_Constructor_Implementation();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void Exceptions_Should_Have_A_Message_And_Inner_Exception_Constructor()
        {
            Exceptions_Should_Have_A_Message_And_Inner_Exception_Constructor_Implementation();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void Exceptions_Should_Have_A_Message_Constructor()
        {
            Exceptions_Should_Have_A_Message_Constructor_Implementation();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void Exceptions_Should_Have_A_Serialization_Constructor()
        {
            Exceptions_Should_Have_A_Serialization_Constructor_Implementation();
        }

        [TestMethod]
        [TestCategory(TestTiming.CheckIn)]
        public void Exceptions_Should_Have_Valid_Other_Public_Constructors_When_Present()
        {
            Exceptions_Should_Have_Valid_Other_Public_Constructors_When_Present_Implementation();
        }

        protected override IImmutableSet<Type> GetAcceptableBaseExceptionTypes()
        {
            var builder = ImmutableHashSet<Type>.Empty.ToBuilder();
            builder.Add(typeof(LandorphanIocServiceLocationArgumentException));
            builder.Add(typeof(LandorphanIocServiceLocationException));
            return builder.ToImmutable();
        }

        protected override IImmutableSet<Assembly> GetAssembliesUnderTest()
        {
            return TestAssemblyInitializeCleanup.AssembliesUnderTest;
        }

        protected override bool GetDefaultValueForParameterType(Type parameterType, out object defaultValue)
        {
            defaultValue = new object();
            return false;
        }
    }
}
