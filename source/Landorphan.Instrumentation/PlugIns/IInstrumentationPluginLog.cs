using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.PlugIns
{
   using Landorphan.Instrumentation.Interfaces;

   /// <summary>
   /// Used to log activities that occur to the underlying data storage.
   /// </summary>
   public interface IInstrumentationPluginLog
   {
      /// <summary>
      /// Logs the entry of a a method.
      /// </summary>
      /// <param name="methodData">
      /// The method data.
      /// </param>
      /// <param name="context">
      /// the current instrumentation context.
      /// </param>
      /// <param name="arguments">
      /// The method arguments.
      /// </param>
      void LogMethodEntry(IMethodCompilationData methodData, IInstrumentationContext context, ArgumentData[] arguments);

      /// <summary>
      /// Logs the exit of a method.
      /// </summary>
      /// <param name="methodData">
      /// The method data.
      /// </param>
      /// <param name="context">
      /// the current instrumentation context.
      /// </param>
      /// <param name="arguments">
      /// The method arguments.
      /// </param>
      /// <param name="returnValue">
      /// The return value from the method.
      /// </param>
      void LogMethodExit(IMethodCompilationData methodData, IInstrumentationContext context, ArgumentData[] arguments, object returnValue);

      /// <summary>
      /// Logs an exception within a method.
      /// </summary>
      /// <param name="methodData">
      /// The method data.
      /// </param>
      /// <param name="context">
      /// the current instrumentation context.
      /// </param>
      /// <param name="arguments">
      /// The method arguments.
      /// </param>
      /// <param name="exception">
      /// The exception that return from the method execution.
      /// </param>
      void LogMethodException(IMethodCompilationData methodData, IInstrumentationContext context, ArgumentData[] arguments, Exception exception);
   }
}
