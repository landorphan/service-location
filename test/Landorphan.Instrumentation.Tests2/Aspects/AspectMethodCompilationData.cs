using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.Aspects
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
