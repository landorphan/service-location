namespace Landorphan.Ioc.Tests.ServiceLocation.Extensions
{
   using System;
   using System.Collections.Generic;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.TestUtilities;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   // ReSharper disable InconsistentNaming

   public static class IsRegistered_Tests
   {
      public abstract class IsolatedContainerChainBase : DisposableArrangeActAssert
      {
         protected readonly String nonUniqueName = "Name Shared in multiple containers";
         private readonly String childContainerName = "Isolated Test Container (Child): IIocContainerResolver.IsRegistered* Tests";
         private readonly String grandParentContainerName = "Isolated Test Container (Grand Parent): IIocContainerResolver.IsRegistered* Tests";

         private readonly Guid grandParentContainerUid = Guid.NewGuid();
         private readonly String parentContainerName = "Isolated Test Container (Parent): IIocContainerResolver.IsRegistered* Tests";
         private readonly String siblingContainerName = "Isolated Test Container (Sibling): IIocContainerResolver.IsRegistered* Tests";
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

            parentContainer = (IOwnedIocContainer)grandParentContainer.Manager.CreateChildContainer(parentContainerName);
            parentContainerUid = parentContainer.Uid;
            parentContainer.Registrar.RegisterImplementation<IRegisteredInParent, ClassImplementingIRegisteredInParent>();

            childContainer = (IOwnedIocContainer)parentContainer.Manager.CreateChildContainer(childContainerName);
            childContainerUid = childContainer.Uid;
            childContainer.Registrar.RegisterImplementation<IRegisteredInChild, ClassImplementingIRegisteredInChild>();
            childContainer.Registrar.RegisterImplementation<IRegisteredInChild, AnotherClassIRegisteredInChild>(nonUniqueName);
            childContainer.Registrar.RegisterImplementation<IRegisteredInGrandParentAndChild, ClassImplementingIRegisteredInGrandParentAndChild>();
            childContainer.Registrar.RegisterImplementation<IRegisteredInGrandParentAndChild, AnotherClassImplementingIRegisteredInGrandParentAndChild>(nonUniqueName);
            childContainer.Registrar.RegisterImplementation<IRegisteredInChildAndSibling, ClassImplementingIRegisteredInChildAndSibling>();
            childContainer.Registrar.RegisterImplementation<IRegisteredInChildAndSibling, AnotherClassImplementingIRegisteredInChildAndSibling>(nonUniqueName);

            siblingContainer = (IOwnedIocContainer)parentContainer.Manager.CreateChildContainer(siblingContainerName);
            siblingContainerUid = siblingContainer.Uid;
            siblingContainer.Registrar.RegisterImplementation<IRegisterInSibling, ClassImplementingIRegisterInSibling>();
            siblingContainer.Registrar.RegisterImplementation<IRegisterInSiblingNamed, ClassImplementingIRegisterInSiblingNamed>(nonUniqueName);
            siblingContainer.Registrar.RegisterImplementation<IRegisteredInChildAndSibling, ClassImplementingIRegisteredInChildAndSibling>();
            siblingContainer.Registrar.RegisterImplementation<IRegisteredInChildAndSibling, AnotherClassImplementingIRegisteredInChildAndSibling>(nonUniqueName);
         }
      }

      [TestClass]
      public class When_I_have_an_Isolated_Container_chain_and_call_IsRegistered : IsolatedContainerChainBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_I_check_a_concrete_type()
         {
            var actual = ChildContainer.Registrar.IsRegistered<ClassImplementingIRegisteredInChildAndSibling>();
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegistered<ClassImplementingIRegisteredInChildAndSibling>(null);
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegistered(typeof(ClassImplementingIRegisteredInChildAndSibling));
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegistered(typeof(ClassImplementingIRegisteredInChildAndSibling), null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_I_check_an_open_generic()
         {
            // blocked by type system.  ChildContainer.Registrar.IsRegisteredChain<IList<>>();
            // blocked by type system.  ChildContainer.Registrar.IsRegisteredChain<IList<>>(name);
            var actual = ChildContainer.Registrar.IsRegistered(typeof(IList<>));
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegistered(typeof(IList<>), null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_I_check_null()
         {
            // blocked by type system.  ChildContainer.Registrar.IsRegistered<null>();
            // blocked by type system.  ChildContainer.Registrar.IsRegistered<null>(name);
            var actual = ChildContainer.Registrar.IsRegistered(null);
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegistered(null, null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_the_type_has_a_default_registration_only_in_an_ancestor_container()
         {
            var actual = ChildContainer.Registrar.IsRegistered<IRegisteredInGrandParent>();
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegistered<IRegisteredInGrandParent>(null);
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegistered(typeof(IRegisteredInGrandParent));
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegistered(typeof(IRegisteredInGrandParent), null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_the_type_has_a_named_registration_only_in_an_ancestor_container()
         {
            var actual = SiblingContainer.Registrar.IsRegistered<IRegisteredInGrandParentAndChild>(nonUniqueName);
            actual.Should().BeFalse();

            actual = SiblingContainer.Registrar.IsRegistered(typeof(IRegisteredInGrandParentAndChild), nonUniqueName);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_the_type_has_a_default_registration_in_the_current_container()
         {
            var actual = ChildContainer.Registrar.IsRegistered<IRegisteredInChild>();
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegistered<IRegisteredInChild>(null);
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegistered(typeof(IRegisteredInChild));
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegistered(typeof(IRegisteredInChild), null);
            actual.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_the_type_has_a_named_registration_in_the_current_container()
         {
            var actual = ChildContainer.Registrar.IsRegistered<IRegisteredInChildAndSibling>(nonUniqueName);
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegistered(typeof(IRegisteredInChild), nonUniqueName);
            actual.Should().BeTrue();
         }
      }

      [TestClass]
      public class When_I_have_an_Isolated_Container_chain_and_call_IsRegisteredChain : IsolatedContainerChainBase
      {
         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_I_an_open_generic()
         {
            // blocked by type system.  ChildContainer.Registrar.IsRegisteredChain<IList<>>();
            // blocked by type system.  ChildContainer.Registrar.IsRegisteredChain<IList<>>(name);
            var actual = ChildContainer.Registrar.IsRegisteredChain(typeof(IList<>));
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegisteredChain(typeof(IList<>), null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_I_check_a_concrete_type()
         {
            var actual = ChildContainer.Registrar.IsRegisteredChain<ClassImplementingIRegisteredInChildAndSibling>();
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegisteredChain<ClassImplementingIRegisteredInChildAndSibling>(null);
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegisteredChain(typeof(ClassImplementingIRegisteredInChildAndSibling));
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegisteredChain(typeof(ClassImplementingIRegisteredInChildAndSibling), null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_false_when_I_check_null()
         {
            // blocked by type system.  ChildContainer.Registrar.IsRegisteredChain<null>();
            // blocked by type system.  ChildContainer.Registrar.IsRegisteredChain<null>(name);
            var actual = ChildContainer.Registrar.IsRegisteredChain(null);
            actual.Should().BeFalse();

            actual = ChildContainer.Registrar.IsRegisteredChain(null, null);
            actual.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_the_type_has_a_default_registration_in_the_current_container()
         {
            var actual = ChildContainer.Registrar.IsRegisteredChain<IRegisteredInChild>();
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegisteredChain<IRegisteredInChild>(null);
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegisteredChain(typeof(IRegisteredInChild));
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegisteredChain(typeof(IRegisteredInChild), null);
            actual.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_the_type_has_a_default_registration_only_in_an_ancestor_container()
         {
            var actual = ChildContainer.Registrar.IsRegisteredChain<IRegisteredInGrandParent>();
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegisteredChain<IRegisteredInGrandParent>(null);
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegisteredChain(typeof(IRegisteredInGrandParent));
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegisteredChain(typeof(IRegisteredInGrandParent), null);
            actual.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_the_type_has_a_named_registration_in_the_current_container()
         {
            var actual = ChildContainer.Registrar.IsRegisteredChain<IRegisteredInChildAndSibling>(nonUniqueName);
            actual.Should().BeTrue();

            actual = ChildContainer.Registrar.IsRegisteredChain(typeof(IRegisteredInChild), nonUniqueName);
            actual.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_return_true_when_the_type_has_a_named_registration_only_in_an_ancestor_container()
         {
            var actual = SiblingContainer.Registrar.IsRegisteredChain<IRegisteredInGrandParentAndChild>(nonUniqueName);
            actual.Should().BeTrue();

            actual = SiblingContainer.Registrar.IsRegisteredChain(typeof(IRegisteredInGrandParentAndChild), nonUniqueName);
            actual.Should().BeTrue();
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
