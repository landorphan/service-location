namespace Landorphan.Ioc.Tests.ServiceLocation.Extensions
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class GetRegistrationChain_Tests
   {
      public abstract class GetRegistrationChainBase : DisposableArrangeActAssert
      {
         protected readonly String nonUniqueName = "Name Shared in multiple containers";
         private readonly String childContainerName = "Isolated Test Container (Child): IIocContainerResolver.GetRegistrationChain Tests";
         private readonly String grandParentContainerName = "Isolated Test Container (Grand Parent): IIocContainerResolver.GetRegistrationChain Tests";

         private readonly Guid grandParentContainerUid = Guid.NewGuid();
         private readonly String parentContainerName = "Isolated Test Container (Parent): IIocContainerResolver.GetRegistrationChain Tests";
         private readonly String siblingContainerName = "Isolated Test Container (Sibling): IIocContainerResolver.GetRegistrationChain Tests";
         private IOwnedIocContainer childContainer;
         private Guid childContainerUid;
         private IOwnedIocContainer grandParentContainer;
         private IOwnedIocContainer parentContainer;
         private Guid parentContainerUid;
         private IOwnedIocContainer siblingContainer;
         private Guid siblingContainerUid;

         protected IIocContainer ChildContainer => childContainer;

         protected IIocContainer GrandParentContainer => grandParentContainer;

         protected IIocContainer ParentContainer => parentContainer;

         protected IIocContainer SiblingContainer => siblingContainer;

         protected override void ArrangeMethod()
         {
            grandParentContainer = IocContainer.TestHookCreateIsolatedContainer(grandParentContainerUid, grandParentContainerName);
            grandParentContainer.Registrar.RegisterImplementation<IRegisteredInGrandParent, ClassImplementingIRegisteredInGrandParent>();
            grandParentContainer.Registrar.RegisterImplementation<IRegisteredInGrandParent, ClassImplementingIRegisteredInGrandParent>(nonUniqueName);
            grandParentContainer.Registrar.RegisterImplementation<IRegisteredInGrandParentAndChild, ClassImplementingIRegisteredInGrandParentAndChild>();
            grandParentContainer.Registrar.RegisterImplementation<IRegisteredInGrandParentAndChild, AnotherClassImplementingIRegisteredInGrandParentAndChild>(nonUniqueName);

            parentContainer = (IOwnedIocContainer) grandParentContainer.Manager.CreateChildContainer(parentContainerName);
            parentContainerUid = parentContainer.Uid;
            parentContainer.Registrar.RegisterImplementation<IRegisteredInParent, ClassImplementingIRegisteredInParent>();

            childContainer = (IOwnedIocContainer) parentContainer.Manager.CreateChildContainer(childContainerName);
            childContainerUid = childContainer.Uid;
            childContainer.Registrar.RegisterImplementation<IRegisteredInChild, ClassImplementingIRegisteredInChild>();
            childContainer.Registrar.RegisterImplementation<IRegisteredInChild, AnotherClassIRegisteredInChild>(nonUniqueName);
            childContainer.Registrar.RegisterImplementation<IRegisteredInGrandParentAndChild, ClassImplementingIRegisteredInGrandParentAndChild>();
            childContainer.Registrar.RegisterImplementation<IRegisteredInGrandParentAndChild, AnotherClassImplementingIRegisteredInGrandParentAndChild>(nonUniqueName);
            childContainer.Registrar.RegisterImplementation<IRegisteredInChildAndSibling, ClassImplementingIRegisteredInChildAndSibling>();
            childContainer.Registrar.RegisterImplementation<IRegisteredInChildAndSibling, AnotherClassImplementingIRegisteredInChildAndSibling>(nonUniqueName);

            siblingContainer = (IOwnedIocContainer) parentContainer.Manager.CreateChildContainer(siblingContainerName);
            siblingContainerUid = siblingContainer.Uid;
            siblingContainer.Registrar.RegisterImplementation<IRegisterInSibling, ClassImplementingIRegisterInSibling>();
            siblingContainer.Registrar.RegisterImplementation<IRegisterInSiblingNamed, ClassImplementingIRegisterInSiblingNamed>(nonUniqueName);
            siblingContainer.Registrar.RegisterImplementation<IRegisteredInChildAndSibling, ClassImplementingIRegisteredInChildAndSibling>();
            siblingContainer.Registrar.RegisterImplementation<IRegisteredInChildAndSibling, AnotherClassImplementingIRegisteredInChildAndSibling>(nonUniqueName);
         }
      }

      [TestClass]
      public class When_I_have_an_Isolated_Container_chain_and_call_GetRegistrationChain : GetRegistrationChainBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_an_empty_set_when_I_check_a_concrete_type()
         {
            var actual = ChildContainer.Registrar.GetRegistrationChain<ClassImplementingIRegisteredInChildAndSibling>();
            actual.Should().BeEmpty();

            actual = ChildContainer.Registrar.GetRegistrationChain<ClassImplementingIRegisteredInChildAndSibling>(null);
            actual.Should().BeEmpty();

            actual = ChildContainer.Registrar.GetRegistrationChain(typeof(ClassImplementingIRegisteredInChildAndSibling));
            actual.Should().BeEmpty();

            actual = ChildContainer.Registrar.GetRegistrationChain(typeof(ClassImplementingIRegisteredInChildAndSibling), null);
            actual.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_an_empty_set_when_I_check_an_open_generic()
         {
            // blocked by type system.  ChildContainer.Registrar.GetRegistrationChain<IList<>>();
            // blocked by type system.  ChildContainer.Registrar.GetRegistrationChain<IList<>>(name);
            var actual = ChildContainer.Registrar.GetRegistrationChain(typeof(IList<>));
            actual.Should().BeEmpty();

            actual = ChildContainer.Registrar.GetRegistrationChain(typeof(IList<>), null);
            actual.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_an_empty_set_when_I_check_null()
         {
            // blocked by type system.  ChildContainer.Registrar.GetRegistrationChain<null>();
            // blocked by type system.  ChildContainer.Registrar.GetRegistrationChain<null>(name);
            var actual = ChildContainer.Registrar.GetRegistrationChain(null);
            actual.Should().BeEmpty();

            actual = ChildContainer.Registrar.GetRegistrationChain(null, null);
            actual.Should().BeEmpty();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_expected_chain_when_the_type_has_a_named_registration_only_in_an_ancestor_container()
         {
            var builder = ImmutableDictionary<IContainerRegistrationKey, IRegistrationValue>.Empty.ToBuilder();

            var key = new ContainerRegistrationKeyTypeNameTrio(GrandParentContainer, typeof(IRegisteredInGrandParent), nonUniqueName);
            var value = new RegistrationValueTypeInstancePair(typeof(ClassImplementingIRegisteredInGrandParent));
            builder.Add(new KeyValuePair<IContainerRegistrationKey, IRegistrationValue>(key, value));

            var expected = builder.ToImmutable();

            var actual = ChildContainer.Registrar.GetRegistrationChain<IRegisteredInGrandParent>(nonUniqueName);

            actual.Should().OnlyContain(element => expected.Contains(element));

            actual = ChildContainer.Registrar.GetRegistrationChain(typeof(IRegisteredInGrandParent), nonUniqueName);
            actual.Should().OnlyContain(element => expected.Contains(element));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_expected_chain_when_the_type_has_a_default_registration_in_the_current_container()
         {
            var builder = ImmutableDictionary<IContainerRegistrationKey, IRegistrationValue>.Empty.ToBuilder();

            var key = new ContainerRegistrationKeyTypeNameTrio(ChildContainer, typeof(IRegisteredInChild), String.Empty);
            var value = new RegistrationValueTypeInstancePair(typeof(ClassImplementingIRegisteredInChild));
            builder.Add(new KeyValuePair<IContainerRegistrationKey, IRegistrationValue>(key, value));

            var expected = builder.ToImmutable();

            var actual = ChildContainer.Registrar.GetRegistrationChain<IRegisteredInChild>();
            actual.Should().OnlyContain(element => expected.Contains(element));

            actual = ChildContainer.Registrar.GetRegistrationChain<IRegisteredInChild>(null);
            actual.Should().OnlyContain(element => expected.Contains(element));

            actual = ChildContainer.Registrar.GetRegistrationChain(typeof(IRegisteredInChild));
            actual.Should().OnlyContain(element => expected.Contains(element));

            actual = ChildContainer.Registrar.GetRegistrationChain(typeof(IRegisteredInChild), null);
            actual.Should().OnlyContain(element => expected.Contains(element));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_expected_chain_when_the_type_has_a_default_registration_only_in_an_ancestor_container()
         {
            var builder = ImmutableDictionary<IContainerRegistrationKey, IRegistrationValue>.Empty.ToBuilder();

            var key = new ContainerRegistrationKeyTypeNameTrio(GrandParentContainer, typeof(IRegisteredInGrandParent), null);
            var value = new RegistrationValueTypeInstancePair(typeof(ClassImplementingIRegisteredInGrandParent));
            builder.Add(new KeyValuePair<IContainerRegistrationKey, IRegistrationValue>(key, value));

            var expected = builder.ToImmutable();

            var actual = ChildContainer.Registrar.GetRegistrationChain<IRegisteredInGrandParent>();
            actual.Should().OnlyContain(element => expected.Contains(element));

            actual = ChildContainer.Registrar.GetRegistrationChain<IRegisteredInGrandParent>(null);
            actual.Should().OnlyContain(element => expected.Contains(element));

            actual = ChildContainer.Registrar.GetRegistrationChain(typeof(IRegisteredInGrandParent));
            actual.Should().OnlyContain(element => expected.Contains(element));

            actual = ChildContainer.Registrar.GetRegistrationChain(typeof(IRegisteredInGrandParent), null);
            actual.Should().OnlyContain(element => expected.Contains(element));
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_the_expected_chain_when_the_type_has_a_named_registration_in_the_current_and_ancestor_container()
         {
            var builder = ImmutableDictionary<IContainerRegistrationKey, IRegistrationValue>.Empty.ToBuilder();

            var key = new ContainerRegistrationKeyTypeNameTrio(ChildContainer, typeof(IRegisteredInGrandParentAndChild), nonUniqueName);
            var value = new RegistrationValueTypeInstancePair(typeof(AnotherClassImplementingIRegisteredInGrandParentAndChild));
            builder.Add(new KeyValuePair<IContainerRegistrationKey, IRegistrationValue>(key, value));

            var expected = builder.ToImmutable();

            var actual = ChildContainer.Registrar.GetRegistrationChain<IRegisteredInGrandParentAndChild>(nonUniqueName);
            actual.Should().OnlyContain(element => expected.Contains(element));

            actual = ChildContainer.Registrar.GetRegistrationChain(typeof(IRegisteredInGrandParentAndChild), nonUniqueName);
            actual.Should().OnlyContain(element => expected.Contains(element));
         }
      }

      private class AnotherClassImplementingIRegisteredInChildAndSibling : IRegisteredInChildAndSibling
      {
      }

      private class AnotherClassImplementingIRegisteredInGrandParentAndChild : IRegisteredInGrandParentAndChild
      {
      }

      private class AnotherClassIRegisteredInChild : IRegisteredInChild
      {
      }

      private class ClassImplementingIRegisteredInChild : IRegisteredInChild
      {
      }

      private class ClassImplementingIRegisteredInChildAndSibling : IRegisteredInChildAndSibling
      {
      }

      private class ClassImplementingIRegisteredInGrandParent : IRegisteredInGrandParent
      {
      }

      private class ClassImplementingIRegisteredInGrandParentAndChild : IRegisteredInGrandParentAndChild
      {
      }

      private class ClassImplementingIRegisteredInParent : IRegisteredInParent
      {
      }

      private class ClassImplementingIRegisterInSibling : IRegisterInSibling
      {
      }

      private class ClassImplementingIRegisterInSiblingNamed : IRegisterInSiblingNamed
      {
      }

      private interface IRegisteredInChild
      {
      }

      private interface IRegisteredInChildAndSibling
      {
      }

      private interface IRegisteredInGrandParent
      {
      }

      private interface IRegisteredInGrandParentAndChild
      {
      }

      private interface IRegisteredInParent
      {
      }

      private interface IRegisterInSibling
      {
      }

      private interface IRegisterInSiblingNamed
      {
      }
   }
}