using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using System.Linq;
   using System.Reflection;
   using Landorphan.Instrumentation.Interfaces;
   using Landorphan.Instrumentation.Resources;
   using Landorphan.Instrumentation.Tests.Aspects;
   using PostSharp.Aspects;
   using PostSharp.Serialization;

   [PSerializable]
   public class LogMethodAttribute : OnMethodBoundaryAspect
   {
      private class ExectionData
      {
         public ArgumentData[] Arguments { get; set; }
         public IMethodExecution Execution { get; set; }
      }
      private IMethodCompilationData methodCompilationData;
      private IParameterData[] parameterData;

      public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
      {
         methodCompilationData = new AspectMethodCompilationData(method);
         parameterData = (from p in method.GetParameters()
                        select new ParameterData() { ParameterName = p.Name,
                                                     ParameterTypeFullName = p.ParameterType.FullName }).ToArray();
         base.CompileTimeInitialize(method, aspectInfo);
      }

      public override void OnEntry(MethodExecutionArgs args)
      {
         var executionData = new ExectionData();
         executionData.Arguments = new ArgumentData[args.Arguments.Count];
         for (int i = 0; i < args.Arguments.Count; i++)
         {
            executionData.Arguments[i] = new ArgumentData() { ParameterData = parameterData[i], ArgumentValue = args.Arguments[i] };
         }
         
         args.MethodExecutionTag = executionData;
         if (Instrumentation.IsBootstrapped)
         {
            executionData.Execution = Instrumentation.Current.EnterMethod(methodCompilationData, executionData.Arguments);
         }
         base.OnEntry(args);
      }

      public override void OnExit(MethodExecutionArgs args)
      {
         if (args.MethodExecutionTag is ExectionData executionData && Instrumentation.IsBootstrapped)
         {
            executionData.Execution.ReturnValue = args.ReturnValue;
            executionData.Execution.Dispose();
         }
      }

      public override void OnException(MethodExecutionArgs args)
      {
         if (args.MethodExecutionTag is ExectionData executionData && Instrumentation.IsBootstrapped)
         {
            executionData.Execution.HandleException(args.Exception);
         }
      }
   }
}
