namespace Landorphan.InstrumentationManagement.Interfaces
{
   /// <summary>
   /// Provides data for a method being tracked by the Instrumentation system.
   /// </summary>
   public interface IMethodCompilationData
   {
       /// <summary>
      /// Gets the full name of the declaring assembly.
      /// </summary>
      string AssemblyFullName { get; }

       /// <summary>
      /// Gets the assembly qualified name of the declaring type.
      /// </summary>
      string AssemblyQualifiedName { get; }

       /// <summary>
      /// Gets the version of the declaring assembly.
      /// </summary>
      string AssemblyVersion { get; }

       /// <summary>
      /// Gets the Name of the Method.
      /// </summary>
      string MethodName { get; }

       /// <summary>
      /// Gets the return type of the method.
      /// </summary>
      string ReturnTypeFullName { get; }

       /// <summary>
      /// Gets the full name of the declaring type.
      /// </summary>
      string TypeFullName { get; }

       /// <summary>
      /// Gets the name  of  the declaring type.
      /// </summary>
      string TypeName { get; }
   }
}
