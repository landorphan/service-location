namespace Landorphan.InstrumentationManagement.PlugIns
{
    using System;
    using System.Collections.Generic;
    using Landorphan.InstrumentationManagement.Interfaces;

    /// <summary>
    /// Used to log activities that occur to the underlying data storage.
    /// </summary>
    public interface IInstrumentationPluginLog
    {
        /// <summary>
        /// Called to log an action to the underlying system.
        /// </summary>
        /// <param name="name">
        ///    The name of the action to log.
        /// </param>
        /// <param name="context">
        ///    The context of the Instrumentation state to provide further data into the logs.
        /// </param>
        /// <param name="tags">
        ///    Action specific tags to log along with the action.
        /// </param>
        void LogAction(string name, IInstrumentationContext context, KeyValuePair<string, string>[] tags);

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
    }
}
