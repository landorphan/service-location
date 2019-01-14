namespace Landorphan.TestUtilities
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Diagnostics.CodeAnalysis;
   using System.Reflection;

   /// <summary>
   /// Extension methods for <see cref="object"/> instances.
   /// </summary>
   public static class TestObjectExtensions
   {
      /// <summary>
      /// Gets a non-default value for the given type.
      /// </summary>
      /// <param name="type">
      /// The type to evaluate.
      /// </param>
      /// <returns>
      /// The default value for the given type.
      /// </returns>
      /// <remarks>
      /// Only works on <see cref="Activator"/> instantiateable types (i.e., a public parameterless constructor is required).
      /// </remarks>
      [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.ComponentModel.TypeConverter.ConvertFromInvariantString(System.String)")]
      [SuppressMessage("Microsoft.Maintainability", "CA1502: Avoid excessive complexity")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      [SuppressMessage("Microsoft.Globlization", "CA1303: Do not pass literals as localized parameter")]
      public static Object NonDefaultValue(this Type type)
      {
         // NOTE:  this method should not be used outside of testing scenarios.

         if (type == typeof(Byte))
         {
            Byte rv = 1;
            return rv;
         }

         if (type == typeof(Boolean))
         {
            var rv = true;
            return rv;
         }

         if (type == typeof(Boolean?))
         {
            Boolean? rv = true;
            return rv;
         }

         if (type == typeof(Byte?))
         {
            Byte? rv = 2;
            return rv;
         }

         if (type == typeof(DateTime))
         {
            var rv = DateTime.UtcNow;
            return rv;
         }

         if (type == typeof(DateTime?))
         {
            DateTime? rv = DateTime.UtcNow;
            return rv;
         }

         if (type == typeof(DateTimeOffset))
         {
            var rv = DateTimeOffset.Now;
            return rv;
         }

         if (type == typeof(DateTimeOffset?))
         {
            DateTimeOffset? rv = DateTimeOffset.Now;
            return rv;
         }

         if (type == typeof(Exception))
         {
            Exception rv = new TargetInvocationException("Sample Test Exception", null);
            return rv;
         }

         if (type == typeof(Guid))
         {
            var rv = Guid.NewGuid();
            return rv;
         }

         if (type == typeof(Guid?))
         {
            Guid? rv = Guid.NewGuid();
            return rv;
         }

         if (type == typeof(IEnumerable<String>))
         {
            IEnumerable<String> rv = new[] {"abc", "def", "ghi"};
            return rv;
         }

         if (type == typeof(IEnumerable<Type>))
         {
            IEnumerable<Type> rv = new[] {typeof(Int32), typeof(String), typeof(Guid)};
            return rv;
         }

         if (type == typeof(Int16))
         {
            Int16 rv = -1;
            return rv;
         }

         if (type == typeof(Int16?))
         {
            Int16? rv = -2;
            return rv;
         }

         if (type == typeof(Int32))
         {
            var rv = -3;
            return rv;
         }

         if (type == typeof(Int32?))
         {
            Int32? rv = 4;
            return rv;
         }

         if (type == typeof(Int64))
         {
            Int64 rv = -5;
            return rv;
         }

         if (type == typeof(Int64?))
         {
            Int64? rv = 6;
            return rv;
         }

         if (type == typeof(SByte))
         {
            SByte rv = -7;
            return rv;
         }

         if (type == typeof(SByte?))
         {
            SByte? rv = 8;
            return rv;
         }

         if (type == typeof(String))
         {
            return "1";
         }

         if (type == typeof(TimeSpan))
         {
            var rv = TimeSpan.FromHours(9);
            return rv;
         }

         if (type == typeof(TimeSpan?))
         {
            TimeSpan? rv = TimeSpan.FromHours(10);
            return rv;
         }

         if (type == typeof(Type))
         {
            var rv = typeof(Guid);
            return rv;
         }

         if (type == typeof(UInt16))
         {
            UInt16 rv = 11;
            return rv;
         }

         if (type == typeof(UInt16?))
         {
            UInt16? rv = 12;
            return rv;
         }

         if (type == typeof(UInt32))
         {
            UInt32 rv = 13;
            return rv;
         }

         if (type == typeof(UInt32?))
         {
            UInt32? rv = 14;
            return rv;
         }

         if (type == typeof(UInt64))
         {
            UInt64 rv = 15;
            return rv;
         }

         if (type == typeof(UInt64?))
         {
            UInt64? rv = 16;
            return rv;
         }

         var tc = TypeDescriptor.GetConverter(type);
         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         return tc != null && tc.CanConvertFrom(typeof(String)) ? tc.ConvertFromInvariantString("1") : Activator.CreateInstance(type);
      }
   }
}
