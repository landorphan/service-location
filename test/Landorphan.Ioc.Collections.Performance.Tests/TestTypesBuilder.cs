namespace Ioc.Collections.Performance.Tests
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.Reflection;
   using System.Reflection.Emit;
   using System.Text;
   using Landorphan.Common;

   // ReSharper disable AssignNullToNotNullAttribute

   [SuppressMessage("SonarLint.CodeSmell", "S4017: Method signatures should not contain nested generic types")]
   public sealed class TestTypesBuilder : DisposableObject
   {
      public void BuildTypePairs(Int32 count, out AssemblyName assemblyName, out IList<KeyValuePair<Type, Type>> list)
      {
         var currentAppDomain = AppDomain.CurrentDomain;
         var randomName = BuildRandomIdentifierName();
         var asmName = new AssemblyName("DynamicAssemblyForPerformanceTests" + randomName);
         var asmBuilder = currentAppDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
         var moduleBuilder = asmBuilder.DefineDynamicModule(asmName.Name);

         var rv = new List<KeyValuePair<Type, Type>>();
         count.ArgumentGreaterThanOrEqualTo(0, nameof(count));

         for (var idx = 0; idx < count; idx++)
         {
            var item = BuildTestKeyValuePair(moduleBuilder);
            rv.Add(item);
         }

         assemblyName = asmName;
         list = rv;
         Dispose(true);
      }

      private String BuildRandomIdentifierName()
      {
         // build a random string based on a GUID.
         var sb = new StringBuilder();
         sb.Append(Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));
         // remove the leading and trailing brace characters
         sb.Remove(0, 1);
         sb.Remove(sb.Length - 1 -1, 1);
         // remove the embedded dashes
         sb.Replace("-", null);
         // prepend a T in case the GUID started with a numeric character
         sb.Insert(0, "T");
         return sb.ToString();
      }

      private KeyValuePair<Type, Type> BuildTestKeyValuePair(ModuleBuilder moduleBuilder)
      {
         var implementationTypeName = BuildRandomIdentifierName();
         var interfaceTypeName = "I" + implementationTypeName;

         var interfaceBuilder = moduleBuilder.DefineType(interfaceTypeName, TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Interface);
         var interfaceType = interfaceBuilder.CreateType();

         var implementationBuilder = moduleBuilder.DefineType(implementationTypeName, TypeAttributes.Public | TypeAttributes.Class);
         implementationBuilder.AddInterfaceImplementation(interfaceType);
         var implementationType = implementationBuilder.CreateType();

         var rv = new KeyValuePair<Type, Type>(interfaceType, implementationType);
         return rv;
      }
   }
}
