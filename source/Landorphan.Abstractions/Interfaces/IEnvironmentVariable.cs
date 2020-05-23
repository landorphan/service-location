namespace Landorphan.Abstractions.Interfaces
{
    using System;

    /// <summary>
    /// Represents an environment variable.
    /// </summary>
    public interface IEnvironmentVariable : IEquatable<IEnvironmentVariable>
    {
        /// <summary>
        /// Gets the name of the environment variable.
        /// </summary>
        /// <value>
        /// The name of the environment variable.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the value of the environment variable.
        /// </summary>
        /// <value>
        /// The value of the environment variable.
        /// </value>
        string Value { get; }
    }
}
