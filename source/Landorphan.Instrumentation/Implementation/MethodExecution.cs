using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Implementation
{
   using Landorphan.Instrumentation.Interfaces;

   /// <summary>
   /// Manages the execution of a method.
   /// </summary>
   public class MethodExecution : IMethodExecution
   {
      private readonly IMethodCompilationData methodData;
      private readonly ArgumentData[] argumentData;
      private readonly InstrumentationBootstrapData bootstrapData;
      private readonly IInstrumentationContext context;

      /// <summary>
      /// Creates a new instance of the MethodExecution class.
      /// </summary>
      /// <param name="methodData">
      /// The compilation data that represents the method.
      /// </param>
      /// <param name="bootstrapData">
      /// The instrumentation bootstrap data.
      /// </param>
      /// <param name="context">
      /// the instrumentation context.
      /// </param>
      /// <param name="arguments">
      /// the method argument data.
      /// </param>
      internal MethodExecution(IMethodCompilationData methodData,
                               InstrumentationBootstrapData bootstrapData,
                               IInstrumentationContext context,
                               ArgumentData[] arguments)
      {
         this.methodData = methodData;
         this.argumentData = arguments;
         this.bootstrapData = bootstrapData;
         this.context = context;
         this.bootstrapData.Logger.LogMethodEntry(methodData, context, arguments);
      }

      /// <inheritdoc />
      public void Dispose()
      {
         bootstrapData.Logger.LogMethodExit(methodData, context, argumentData, this.ReturnValue);
      }

      /// <inheritdoc />
      public object ReturnValue { get; set; }

      /// <inheritdoc />
      public void HandleException(Exception exception)
      {
         ReturnValue = exception;
         bootstrapData.Logger.LogMethodException(methodData, context, argumentData, exception);
      }
   }
}

