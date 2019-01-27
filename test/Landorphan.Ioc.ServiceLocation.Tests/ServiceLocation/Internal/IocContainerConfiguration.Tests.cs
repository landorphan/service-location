namespace Landorphan.Ioc.Tests.ServiceLocation.Internal
{
   // ReSharper disable InconsistentNaming
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using System.Reflection;
   using FluentAssertions;
   using Landorphan.Ioc.ServiceLocation;
   using Landorphan.Ioc.ServiceLocation.Interfaces;
   using Landorphan.Ioc.ServiceLocation.Internal;
   using Landorphan.Ioc.Tests.Copy.Landorphan.TestUtilities;
   using Landorphan.Ioc.Tests.Copy.Landorphan.TestUtilities.ReusableTestImplementations;
   using Landorphan.Ioc.Tests.Mocks;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   public static class IocContainerConfiguration_Tests
   {
      [TestClass]
      public class When_I_call_IocContainerConfiguration_Clone : CloneableArrangeActAssert<IIocContainerConfiguration>
      {
         private readonly String containerName = "My test container";
         private readonly Guid containerUid = Guid.NewGuid();
         private Object actualObject;
         private IIocContainerMetaIdentity container;

         protected override IIocContainerConfiguration Target { get; set; }

         protected override void ArrangeMethod()
         {
            container = new MockContainerImplementingIIocContainerMetaIdentity(containerName, containerUid);
            var obj = new IocContainerConfiguration(container);
            Target = obj;
            Target.AllowNamedImplementations = true;
            Target.AllowPreclusionOfTypes = true;
            Target.ThrowOnRegistrationCollision = true;
         }

         protected override void ActMethod()
         {
            actualObject = Target.Clone();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_Should_Clone_Correctly()
         {
            It_Should_Clone_Correctly_Implementation();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_create_an_equivalent_untyped_clone_and_set_IsReadOnly_to_false()
         {
            actualObject.Should().BeOfType<IocContainerConfiguration>();

            ((IIocContainerConfiguration)actualObject).Should().BeOfType<IocContainerConfiguration>();
            ((IIocContainerConfiguration)actualObject).Equals(Target).Should().BeTrue();
            ((IIocContainerConfiguration)actualObject).AllowNamedImplementations.Should().Be(Target.AllowNamedImplementations);
            ((IIocContainerConfiguration)actualObject).AllowPreclusionOfTypes.Should().Be(Target.AllowPreclusionOfTypes);
            ((IIocContainerConfiguration)actualObject).Container.Uid.Should().Be(Target.Container.Uid);
            ((IIocContainerConfiguration)actualObject).ThrowOnRegistrationCollision.Should().Be(Target.ThrowOnRegistrationCollision);
            ((IIocContainerConfiguration)actualObject).IsReadOnly.Should().BeFalse();
            ((IIocContainerConfiguration)actualObject).GetHashCode().Should().Be(Target.GetHashCode());
         }
      }

      [TestClass]
      public class When_I_call_IocContainerConfiguration_MakeReadOnly : ArrangeActAssert
      {
         private const String containerName = "Mock container for IocContainerConfiguration Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IList<PropertyInfo> _propertyInfos;
         private IIocContainerMetaIdentity container;
         private IocContainerConfiguration target;

         protected override void ArrangeMethod()
         {
            container = new MockContainerImplementingIIocContainerMetaIdentity(containerName, containerUid);
            target = new IocContainerConfiguration(container);
         }

         /// <inheritdoc/>
         protected override void ActMethod()
         {
            target.MakeReadOnly();
            var type = typeof(IocContainerConfiguration);
            _propertyInfos =
               type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                  .ToImmutableList();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_inform_that_it_IsReadOnly()
         {
            target.IsReadOnly.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage(
            "Microsoft.Globalization",
            "CA1305: Specify IFormatProvide",
            Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
         public void It_should_throw_on_all_property_setters()
         {
            var unprotectedSetters = new List<String>();
            foreach (var pi in _propertyInfos)
            {
               var propertyInfo = pi;
               if (propertyInfo.CanWrite)
               {
                  Action throwingAction = () => propertyInfo.SetValue(target, GetDefaultValue(propertyInfo));
                  try
                  {
                     throwingAction.Should().Throw<TargetInvocationException>().WithInnerException<NotSupportedException>();
                  }
                  catch (AssertFailedException e)
                  {
                     unprotectedSetters.Add(
                        $"{target.GetType().FullName}.{propertyInfo.Name} should throw TargetInvocationException(NotSupportedException) when read only and the property setter is invoked, " +
                        $"but the following was thrown by the test:\r\n{e}.");
                  }
                  catch (Exception e)
                  {
                     unprotectedSetters.Add(
                        $"{target.GetType().FullName}.{propertyInfo.Name} should throw TargetInvocationException(NotSupportedException) when read only and the property setter is invoked, " +
                        $"but it did throw:\r\n{e}.");
                  }
               }
            }

            if (unprotectedSetters.Any())
            {
               foreach (var unprotected in unprotectedSetters)
               {
                  Trace.WriteLine(unprotected);
               }
            }

            unprotectedSetters.Should().BeEmpty();
         }

         private Object GetDefaultValue(PropertyInfo pi)
         {
            return pi.PropertyType.IsValueType ? Activator.CreateInstance(pi.PropertyType) : null;
         }
      }

      [TestClass]
      public class When_I_change_the_state_of_a_IocContainerConfiguration : ArrangeActAssert
      {
         private const String containerName = "Mock container for IocContainerConfiguration change state tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IIocContainerMetaIdentity container;
         private IocContainerConfiguration target;

         protected override void ArrangeMethod()
         {
            container = new MockContainerImplementingIIocContainerMetaIdentity(containerName, containerUid);

            target = new IocContainerConfiguration(container);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_ConfigurationChanged_when_I_update_AllowNamedImplementations()
         {
            Object actualSender = null;
            EventArgs actualEventArgs = null;

            var eh = new EventHandler<EventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ConfigurationChanged += eh;

            // perform an update update
            target.AllowNamedImplementations = !target.AllowNamedImplementations;

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);
            actualEventArgs.Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_ConfigurationChanged_when_I_update_AllowPreclusionOfTypes()
         {
            Object actualSender = null;
            EventArgs actualEventArgs = null;

            var eh = new EventHandler<EventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ConfigurationChanged += eh;

            // perform an update update
            target.AllowPreclusionOfTypes = !target.AllowPreclusionOfTypes;

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);
            actualEventArgs.Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_fire_ConfigurationChanged_when_I_update_ThrowOnRegistrationCollision()
         {
            Object actualSender = null;
            EventArgs actualEventArgs = null;

            var eh = new EventHandler<EventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ConfigurationChanged += eh;

            // perform an update update
            target.ThrowOnRegistrationCollision = !target.ThrowOnRegistrationCollision;

            actualSender.Should().NotBeNull();
            actualSender.Should().Be(target);
            actualEventArgs.Should().NotBeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_fire_ConfigurationChanged_when_I_call_MakeReadOnly()
         {
            Object actualSender = null;
            EventArgs actualEventArgs = null;

            var eh = new EventHandler<EventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ConfigurationChanged += eh;

            target.MakeReadOnly();

            actualSender.Should().BeNull();
            actualEventArgs.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S1656: Variables should not be self-assigned", Justification = "By design")]
         public void It_should_not_fire_ConfigurationChanged_when_perform_an_non_update_update_to_AllowNamedImplementations()
         {
            Object actualSender = null;
            EventArgs actualEventArgs = null;

            var eh = new EventHandler<EventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ConfigurationChanged += eh;

            // perform an non-update update
            var was = target.AllowNamedImplementations;
            target.AllowNamedImplementations = was;

            actualSender.Should().BeNull();
            actualEventArgs.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S1656: Variables should not be self-assigned", Justification = "By design")]
         public void It_should_not_fire_ConfigurationChanged_when_perform_an_non_update_update_to_AllowPreclusionOfTypes()
         {
            Object actualSender = null;
            EventArgs actualEventArgs = null;

            var eh = new EventHandler<EventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ConfigurationChanged += eh;

            // perform an non-update update
            var was = target.AllowPreclusionOfTypes;
            target.AllowPreclusionOfTypes = was;

            actualSender.Should().BeNull();
            actualEventArgs.Should().BeNull();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         [SuppressMessage("SonarLint.CodeSmell", "S1656: Variables should not be self-assigned", Justification = "By design")]
         public void It_should_not_fire_ConfigurationChanged_when_perform_an_non_update_update_to_ThrowOnRegistrationCollision()
         {
            Object actualSender = null;
            EventArgs actualEventArgs = null;

            var eh = new EventHandler<EventArgs>(
               (o, e) =>
               {
                  actualSender = o;
                  actualEventArgs = e;
               });

            target.ConfigurationChanged += eh;

            // perform an non-update update
            var was = target.ThrowOnRegistrationCollision;
            target.ThrowOnRegistrationCollision = was;

            actualSender.Should().BeNull();
            actualEventArgs.Should().BeNull();
         }
      }

      [TestClass]
      public class When_I_create_an_IocContainerConfiguration_using_the_container_constructor : ArrangeActAssert
      {
         private const String containerName = "Mock container for IocContainerConfiguration Tests";
         private readonly Guid containerUid = Guid.NewGuid();
         private IIocContainerMetaIdentity container;
         private IIocContainerConfiguration target;

         protected override void ArrangeMethod()
         {
            container = new MockContainerImplementingIIocContainerMetaIdentity(containerName, containerUid);

            target = new IocContainerConfiguration(container);
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_AllowNamedImplementations_set_to_true()
         {
            target.AllowNamedImplementations.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_AllowPreclusionOfTypes_set_to_true()
         {
            target.AllowPreclusionOfTypes.Should().BeTrue();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_have_ThrowOnRegistrationCollision_set_to_false()
         {
            target.ThrowOnRegistrationCollision.Should().BeFalse();
         }

         [TestMethod]
         [TestCategory(TestTiming.CheckIn)]
         public void It_should_not_be_ReadOnly()
         {
            target.IsReadOnly.Should().BeFalse();
         }
      }
   }
}
