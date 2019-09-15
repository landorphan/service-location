using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation
{
   using System.Reflection;
   using PostSharp.Serialization;

   /// <summary>
   /// Provides data for a method being tracked by the Instrumentation system.
   /// </summary>
   [PSerializable]
   public class MethodCompilationData : IMethodCompilationData
   {
      /// <summary>
      /// Initializes a new instance of the MethodCompilationData class.
      /// </summary>
      /// <param name="method">
      /// The MethodBase for the method being Instrumented.
      /// </param>
      public MethodCompilationData(MethodBase method)
      {
         this.MethodName = method.Name;
         this.TypeName = method.DeclaringType.Name;
         this.TypeFullName = method.DeclaringType.FullName;
         var type = method.DeclaringType;
         this.AssemblyQualifiedName = type.AssemblyQualifiedName;
         var typeInfo = type.GetTypeInfo();
         var assembly = typeInfo.Assembly;
         this.AssemblyFullName = assembly.FullName;
         this.AssemblyVersion = assembly.GetName().Version.ToString();
         if (method is MethodInfo asInfo)
         {
            this.ReturnTypeFullName = asInfo.ReturnType.FullName;
         }
      }
      /// <summary>
      /// Gets the Name of the Method.
      /// </summary>
      public string MethodName { get; private set; }

      /// <summary>
      /// Gets the name  of  the declaring type.
      /// </summary>
      public string TypeName { get; private set; }

      /// <summary>
      /// Gets the full name of the declaring type.
      /// </summary>
      public string TypeFullName { get; private set; }

      /// <summary>
      /// Gets the assembly qualified name of the declaring type.
      /// </summary>
      public string AssemblyQualifiedName { get; private set; }

      /// <summary>
      /// Gets the full name of the declaring assembly.
      /// </summary>
      public string AssemblyFullName { get; private set; }

      /// <summary>
      /// Gets the version of the declaring assembly.
      /// </summary>
      public string AssemblyVersion { get; private set; }

      /// <summary>
      /// Gets the return type of the method.
      /// </summary>
      public string ReturnTypeFullName { get; private set; }
   }
}
