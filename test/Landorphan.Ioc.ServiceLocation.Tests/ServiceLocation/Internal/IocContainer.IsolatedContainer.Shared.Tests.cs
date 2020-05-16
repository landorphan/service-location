namespace Landorphan.Ioc.Tests.ServiceLocation.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using FluentAssertions;
    using Landorphan.Ioc.ServiceLocation.Interfaces;
    using Landorphan.Ioc.ServiceLocation.Internal;
    using Landorphan.TestUtilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming

   public static partial class IocContainer_IsolatedContainer_Tests
   {
       private const string Whitespace = " \t ";
       // contains Fields constants and nested types using by the partial classes.
       // Implements tests of  IIocContainerMetaSharedCapacities and IIocContainerMetaIdentity

       [TestClass]
      public class When_I_have_an_isolated_container : DisposableArrangeActAssert
      {
          private readonly string containerName = "Isolated Test Container: Meta Tests";
          private readonly Guid containerUid = Guid.NewGuid();
          private IOwnedIocContainer target;

          protected override void ArrangeMethod()
         {
            target = IocContainer.TestHookCreateIsolatedContainer(containerUid, Whitespace + containerName + Whitespace);
         }

          [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_Container_reference()
         {
            target.Container.Should().Be(target);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_Manager_reference()
         {
            target.Manager.Should().Be(target);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_Name()
         {
            // name should be trimmed of leading and trailing whitespace
            target.Name.Should().Be(containerName);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_Registrar_reference()
         {
            target.Registrar.Should().Be(target);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_Resolver_reference()
         {
            target.Resolver.Should().Be(target);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_the_expected_Uid()
         {
            target.Uid.Should().Be(containerUid);
         }
      }

      private abstract class AbstractRegisteredType : IRegisteredType
      {}

      private class AnotherRegisteredDescendingFromIRegisteredType : IRegisteredType
      {}

      private class ConcreteClass
      {}

      private interface IRegisteredType
      {}

      private class RegisteredTypeImplementingIRegisteredType : IRegisteredType
      {}

      private class RegisteredTypeWithoutDefaultPublicCtor : IRegisteredType
      {
          [SuppressMessage("SonarLint.CodeSmell", "S1144: Unused private types or members should be removed", Justification = "By design for tests (MWP)")]
         internal RegisteredTypeWithoutDefaultPublicCtor()
         {
            Name = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
         }

         // ReSharper disable once MemberCanBePrivate.Local
         // ReSharper disable once UnusedAutoPropertyAccessor.Local
         internal string Name { get; }
      }
   }
}
