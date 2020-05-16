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
      public static object NonDefaultValue(this Type type)
      {
         // NOTE:  this method should not be used outside of testing scenarios.

         if (type == typeof(byte))
         {
            byte rv = 1;
            return rv;
         }

         if (type == typeof(bool))
         {
            var rv = true;
            return rv;
         }

         if (type == typeof(bool?))
         {
            bool? rv = true;
            return rv;
         }

         if (type == typeof(byte?))
         {
            byte? rv = 2;
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

         if (type == typeof(IEnumerable<string>))
         {
            IEnumerable<string> rv = new[] {"abc", "def", "ghi"};
            return rv;
         }

         if (type == typeof(IEnumerable<Type>))
         {
            IEnumerable<Type> rv = new[] {typeof(int), typeof(string), typeof(Guid)};
            return rv;
         }

         if (type == typeof(short))
         {
            short rv = -1;
            return rv;
         }

         if (type == typeof(short?))
         {
            short? rv = -2;
            return rv;
         }

         if (type == typeof(int))
         {
            var rv = -3;
            return rv;
         }

         if (type == typeof(int?))
         {
            int? rv = 4;
            return rv;
         }

         if (type == typeof(long))
         {
            long rv = -5;
            return rv;
         }

         if (type == typeof(long?))
         {
            long? rv = 6;
            return rv;
         }

         if (type == typeof(sbyte))
         {
            sbyte rv = -7;
            return rv;
         }

         if (type == typeof(sbyte?))
         {
            sbyte? rv = 8;
            return rv;
         }

         if (type == typeof(string))
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

         if (type == typeof(ushort))
         {
            ushort rv = 11;
            return rv;
         }

         if (type == typeof(ushort?))
         {
            ushort? rv = 12;
            return rv;
         }

         if (type == typeof(uint))
         {
            uint rv = 13;
            return rv;
         }

         if (type == typeof(uint?))
         {
            uint? rv = 14;
            return rv;
         }

         if (type == typeof(ulong))
         {
            ulong rv = 15;
            return rv;
         }

         if (type == typeof(ulong?))
         {
            ulong? rv = 16;
            return rv;
         }

         var tc = TypeDescriptor.GetConverter(type);
         // ReSharper disable once ConditionIsAlwaysTrueOrFalse
         return tc != null && tc.CanConvertFrom(typeof(string)) ? tc.ConvertFromInvariantString("1") : Activator.CreateInstance(type);
      }
   }
}
