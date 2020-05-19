namespace Landorphan.InstrumentationManagement.Tests.Aspects
{
    using System.Reflection;
    using Landorphan.Common;
    using Landorphan.InstrumentationManagement.Interfaces;
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
            method.ArgumentNotNull(nameof(method));
            MethodName = method.Name;
            TypeName = method.DeclaringType.Name;
            TypeFullName = method.DeclaringType.FullName;
            var type = method.DeclaringType;
            AssemblyQualifiedName = type.AssemblyQualifiedName;
            var typeInfo = type.GetTypeInfo();
            var assembly = typeInfo.Assembly;
            AssemblyFullName = assembly.FullName;
            AssemblyVersion = assembly.GetName().Version.ToString();
            if (method is MethodInfo asInfo)
            {
                ReturnTypeFullName = asInfo.ReturnType.FullName;
            }
        }

        /// <summary>
        /// Gets the full name of the declaring assembly.
        /// </summary>
        public string AssemblyFullName { get; private set; }

        /// <summary>
        /// Gets the assembly qualified name of the declaring type.
        /// </summary>
        public string AssemblyQualifiedName { get; private set; }

        /// <summary>
        /// Gets the version of the declaring assembly.
        /// </summary>
        public string AssemblyVersion { get; private set; }

        /// <summary>
        /// Gets the Name of the Method.
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public string ReturnTypeFullName { get; private set; }

        /// <summary>
        /// Gets the full name of the declaring type.
        /// </summary>
        public string TypeFullName { get; private set; }

        /// <summary>
        /// Gets the name  of  the declaring type.
        /// </summary>
        public string TypeName { get; private set; }
    }
}
