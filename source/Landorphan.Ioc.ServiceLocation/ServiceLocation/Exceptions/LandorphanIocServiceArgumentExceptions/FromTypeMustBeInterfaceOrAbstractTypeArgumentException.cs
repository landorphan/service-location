namespace Landorphan.Ioc.ServiceLocation.Exceptions
{
    // ReSharper disable InheritdocConsiderUsage
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security;
    using Landorphan.Common;
    using Landorphan.Ioc.Resources;

    /// <summary>
    /// Exception thrown when attempting to register an interface or abstract type that is neither an interface nor an abstract type.
    /// </summary>
    /// <remarks>
    /// Concrete types and open generics are not acceptable for service location keys.
    /// </remarks>
    [SuppressMessage("Microsoft.Maintainability", "CA1501: Avoid excessive inheritance", Justification = "Reviewed")]
    public sealed class FromTypeMustBeInterfaceOrAbstractTypeArgumentException : LandorphanIocServiceLocationArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FromTypeMustBeInterfaceOrAbstractTypeArgumentException"/> class.
        /// </summary>
        public FromTypeMustBeInterfaceOrAbstractTypeArgumentException() : this(null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FromTypeMustBeInterfaceOrAbstractTypeArgumentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public FromTypeMustBeInterfaceOrAbstractTypeArgumentException(string message) : this(null, null, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FromTypeMustBeInterfaceOrAbstractTypeArgumentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException"> The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
        /// </param>
        public FromTypeMustBeInterfaceOrAbstractTypeArgumentException(string message, Exception innerException) : this(null, null, message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FromTypeMustBeInterfaceOrAbstractTypeArgumentException"/> class.
        /// </summary>
        /// <param name="actualType">
        /// The type that gave rise to this exception.
        /// </param>
        /// <param name="paramName">
        /// The name of the parameter that caused the exception.
        /// </param>
        public FromTypeMustBeInterfaceOrAbstractTypeArgumentException(Type actualType, string paramName) : this(actualType, paramName, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FromTypeMustBeInterfaceOrAbstractTypeArgumentException"/> class.
        /// </summary>
        /// <param name="actualType">
        /// The type that gave rise to this exception.
        /// </param>
        /// <param name="paramName">
        /// The name of the parameter that caused the exception.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
        /// </param>
        public FromTypeMustBeInterfaceOrAbstractTypeArgumentException(Type actualType, string paramName, string message, Exception innerException)
            : base(paramName, NullToDefaultMessage(actualType, paramName, message), innerException)
        {
            ActualType = actualType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FromTypeMustBeInterfaceOrAbstractTypeArgumentException"/> class.
        /// </summary>
        /// <param name="info"> The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context"> The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </param>
        // ReSharper disable once UnusedMember.Local
        private FromTypeMustBeInterfaceOrAbstractTypeArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ActualType = (Type)info.GetValue("toType", typeof(Type));
        }

        /// <inheritdoc/>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.ArgumentNotNull("info");
            info.AddValue("toType", ActualType);
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Gets the implementation type.
        /// </summary>
        public Type ActualType { get; }

        private static string NullToDefaultMessage(Type actualType, string paramName, string message)
        {
            var cleanedRegisteredType = null == actualType ? StringResources.NullReplacementValue : actualType.FullName;
            var cleanedParamName = paramName.TrimNullToEmpty();
            var paramNameSuffix = cleanedParamName.Length == 0
                ? StringResources.ArgumentExceptionNoParamNameSuffix
                : string.Format(CultureInfo.InvariantCulture, StringResources.ArgumentExceptionWithParamNameSuffixFmt, paramName);
            var rv = message ??
                     string.Format(
                         CultureInfo.InvariantCulture,
                         StringResources.FromTypeMustBeInterfaceOrAbstractTypeArgumentExceptionNameTypeFmt,
                         cleanedRegisteredType,
                         paramNameSuffix);
            return rv;
        }
    }
}
