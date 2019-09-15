namespace Landorphan.InstrumentationManagement.Tests.Aspects
{
   using System.Reflection;
   using PostSharp.Serialization;

   [PSerializable]
   public class AspectMethodCompilationData : MethodCompilationData
   {
      public AspectMethodCompilationData(MethodBase method) : base(method)
      {
      }
   }
}
