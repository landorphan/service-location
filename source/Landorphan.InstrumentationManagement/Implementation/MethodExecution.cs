namespace Landorphan.InstrumentationManagement.Implementation
{
    using System;
    using Landorphan.Common;
    using Landorphan.InstrumentationManagement.Interfaces;

    /// <summary>
    /// Manages the execution of a method.
    /// </summary>
    public class MethodExecution : DisposableObject, IMethodExecution
    {
        private readonly ArgumentData[] argumentData;
        private readonly InstrumentationBootstrapData bootstrapData;
        private readonly IInstrumentationContext context;
        private readonly IMethodCompilationData methodData;

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
        internal MethodExecution(
            IMethodCompilationData methodData,
            InstrumentationBootstrapData bootstrapData,
            IInstrumentationContext context,
            ArgumentData[] arguments)
        {
            this.methodData = methodData;
            argumentData = arguments;
            this.bootstrapData = bootstrapData;
            this.context = context;
            this.bootstrapData.Logger.LogMethodEntry(methodData, context, arguments);
        }

        /// <inheritdoc />
        protected override void ReleaseManagedResources()
        {
            bootstrapData.Logger.LogMethodExit(methodData, context, argumentData, ReturnValue);
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
