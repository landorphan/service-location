using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Instrumentation.Interfaces
{
   /// <summary>
   /// Used to track a method execution within the system.
   /// </summary>
   public interface IInstrumentationRecordMethod
   {
      /// <summary>
      /// Called when a method is entered and disposed when it is complete.
      /// </summary>
      /// <returns>
      /// An object used to track the scope of the method execution.
      /// </returns>
      IMethodExecution EnterMethod(IMethodCompilationData compilationData, ArgumentData[] arguments);
   }
}
