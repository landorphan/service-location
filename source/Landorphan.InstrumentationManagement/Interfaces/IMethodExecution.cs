namespace Landorphan.InstrumentationManagement.Interfaces
{
    using System;

    /// <summary>
   /// Used to track the execution of a method.
   /// </summary>
   public interface IMethodExecution : IDisposable
   {
       /// <summary>
      /// Gets or sets the value returned from the method.
      /// </summary>
      object ReturnValue { get; set; }

       /// <summary>
      /// Manages the occurrence of an exception within a method
      /// execution.
      /// </summary>
      /// <param name="exception">
      /// The exception.
      /// </param>
      void HandleException(Exception exception);
   }
}
