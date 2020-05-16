namespace Landorphan.TestUtilities.ReusableTestImplementations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using Landorphan.Common.Interfaces;
    using NUnit.Framework;

    // ReSharper disable  InconsistentNaming

   /// <summary>
   /// Implements test of <see cref="IConvertsToReadOnly"/> and <see cref="IQueryReadOnly"/>
   /// </summary>
   /// <typeparam name="T">
   /// The type being tested.
   /// </typeparam>
   public abstract class MakeReadOnlyArrangeActAssert<T> : ArrangeActAssert where T : IConvertsToReadOnly, IQueryReadOnly
   {
       private IList<PropertyInfo> _propertyInfos;

       /// <summary>
      /// Descendants should assign a value before calling the test implementation.
      /// </summary>
      /// <value>
      /// The target.
      /// </value>
      protected abstract T Target { get; set; }

       /// <inheritdoc/>
      protected override void ActMethod()
      {
         Target.MakeReadOnly();
         var typeOfT = typeof(T);
         _propertyInfos =
            typeOfT.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
               .ToImmutableList();
      }

       /// <summary>
      /// Asserts <see cref="IQueryReadOnly.IsReadOnly"/> is true.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void It_Should_Inform_That_It_Is_A_Read_Only_Implementation()
      {
         Target.IsReadOnly.Should().BeTrue();
      }

       /// <summary>
      /// Asserts all property setters throw <see cref="NotSupportedException"/>.
      /// </summary>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
      [SuppressMessage("SonarLint.CodeSmell", "S4056: Overloads with a CultureInfo or an IFormatProvider parameter should be used", Justification = "reflection")]
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void It_Should_Throw_On_All_Property_Setters_Implementation()
      {
         var unprotectedSetters = new List<string>();
         foreach (var pi in _propertyInfos)
         {
            var propertyInfo = pi;
            if (propertyInfo.CanWrite)
            {
               Action throwingAction = () => propertyInfo.SetValue(Target, GetDefaultValue(propertyInfo));
               try
               {
                  throwingAction.Should().Throw<TargetInvocationException>().WithInnerException<NotSupportedException>();
               }
               catch (AssertionException e)
               {
                  unprotectedSetters.Add(
                     $"{Target.GetType().FullName}.{propertyInfo.Name} should throw TargetInvocationException(NotSupportedException) when read only and the property setter is invoked, " +
                     $"but the following was thrown by the test:\r\n{e}.");
               }
               catch (Exception e)
               {
                  unprotectedSetters.Add(
                     $"{Target.GetType().FullName}.{propertyInfo.Name} should throw TargetInvocationException(NotSupportedException) when read only and the property setter is invoked, " +
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

       private object GetDefaultValue(PropertyInfo pi)
      {
         return pi.PropertyType.IsValueType ? Activator.CreateInstance(pi.PropertyType) : null;
      }
   }
}
