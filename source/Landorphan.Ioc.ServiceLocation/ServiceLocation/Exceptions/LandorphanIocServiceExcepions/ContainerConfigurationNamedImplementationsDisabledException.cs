namespace Landorphan.Ioc.ServiceLocation.Exceptions
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security;
    using Landorphan.Common;
    using Landorphan.Ioc.Resources;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    /// <summary>
    /// Exception thrown when a container is configured to disable named implementations, but an attempt is made to register a named implementation or instance.
    /// </summary>
    /// <seealso cref="LandorphanIocServiceLocationException"/>
    [Serializable]
    public sealed class ContainerConfigurationNamedImplementationsDisabledException : LandorphanIocServiceLocationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerConfigurationNamedImplementationsDisabledException"/> class.
        /// </summary>
        public ContainerConfigurationNamedImplementationsDisabledException() : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerConfigurationNamedImplementationsDisabledException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public ContainerConfigurationNamedImplementationsDisabledException(string message) : this(null, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerConfigurationNamedImplementationsDisabledException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
        /// </param>
        public ContainerConfigurationNamedImplementationsDisabledException(string message, Exception innerException) : this(null, message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerConfigurationNamedImplementationsDisabledException" /> class.
        /// </summary>
        /// <param name = "container">
        /// The container that issued this exception.
        /// </param>
        /// <param name="message">
        /// The error message that explains the reason for this exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
        /// </param>
        // ReSharper disable ExpressionIsAlwaysNull
        public ContainerConfigurationNamedImplementationsDisabledException(
            IIocContainerMetaIdentity container,
            string message,
            Exception innerException)
            : base(NullToDefaultMessage(container, message), innerException)
        {
            Container = container;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerConfigurationNamedImplementationsDisabledException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext" /> that contains contextual information about the source or destination.
        /// </param>
        // ReSharper disable once UnusedMember.Local
        private ContainerConfigurationNamedImplementationsDisabledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Container = (IIocContainerMetaIdentity)info.GetValue("container", typeof(IIocContainerMetaIdentity));
        }

        /// <inheritdoc/>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.ArgumentNotNull("info");
            info.AddValue("container", Container);
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Gets the container that gave rise to this exception.
        /// </summary>
        public IIocContainerMetaIdentity Container { get; }

        private static string NullToDefaultMessage(IIocContainerMetaIdentity container, string message)
        {
            var cleanedContainerUid = StringResources.NullReplacementValue;
            var cleanedContainerName = StringResources.NullReplacementValue;
            if (container != null)
            {
                cleanedContainerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
                cleanedContainerName = container.Name.TrimNullToEmpty();
            }

            var rv = message ??
                     string.Format(
                         CultureInfo.InvariantCulture,
                         StringResources.ContainerConfigurationNamedImplementationsDisabledExceptionDefaultMessageFmt,
                         cleanedContainerUid,
                         cleanedContainerName);
            return rv;
        }
    }
}
