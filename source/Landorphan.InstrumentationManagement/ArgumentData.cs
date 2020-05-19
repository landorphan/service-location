namespace Landorphan.InstrumentationManagement
{
    /// <summary>
    /// Provides data on the arguments supplied to a method.
    /// </summary>
    public class ArgumentData
    {
        /// <summary>
        /// Gets or sets the value of the argument.
        /// </summary>
        public object ArgumentValue { get; set; }

        /// <summary>
        /// Gets or sets the Parameter Data for the method arguments.
        /// </summary>
        public IParameterData ParameterData { get; set; }
    }
}
