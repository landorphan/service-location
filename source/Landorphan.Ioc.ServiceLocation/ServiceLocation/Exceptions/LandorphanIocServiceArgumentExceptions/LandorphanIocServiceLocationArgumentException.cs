namespace Landorphan.Ioc.ServiceLocation.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using Landorphan.Common.Exceptions;

    /// <summary>
    /// Base class for thrown <see cref="ArgumentException"/> instances specific to the <see cref="Landorphan.Ioc.ServiceLocation"/> implementations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Landorphan.Ioc.ServiceLocation"/> throws BCL exceptions as appropriate; for example <see cref="ArgumentNullException"/>.
    /// </para>
    /// <para>
    /// This class changes the order of (paramName, message, innerException) arguments to be consistent all other exceptions.
    /// This is in contrast with <see cref="ArgumentException"/> and its derivatives in the BCL which violate the pattern.
    /// </para>
    /// </remarks>
    [SuppressMessage("Microsoft.Maintainability", "CA1501: Avoid excessive inheritance", Justification = "Reviewed")]
    [SuppressMessage("SonarLint.CodeSmell", "S4027: Exceptions should provide standard constructors", Justification = "False positive (MWP)")]
    public abstract class LandorphanIocServiceLocationArgumentException : LandorphanArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationArgumentException"/> class.
        /// </summary>
        protected LandorphanIocServiceLocationArgumentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationArgumentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that explains the reason for the exception.
        /// </param>
        protected LandorphanIocServiceLocationArgumentException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationArgumentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
        /// </param>
        protected LandorphanIocServiceLocationArgumentException(string message, Exception innerException) : this(null, message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationArgumentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that explains the reason for the exception.
        /// </param>
        /// <param name="paramName">
        /// The name of the parameter that caused the current exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
        /// </param>
        protected LandorphanIocServiceLocationArgumentException(string paramName, string message, Exception innerException)
            : base(paramName, message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationArgumentException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized Object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected LandorphanIocServiceLocationArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
