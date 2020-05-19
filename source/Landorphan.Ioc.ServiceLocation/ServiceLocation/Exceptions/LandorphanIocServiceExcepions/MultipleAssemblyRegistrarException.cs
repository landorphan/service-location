namespace Landorphan.Ioc.ServiceLocation.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Text;
    using Landorphan.Common;
    using Landorphan.Ioc.Resources;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    /// <summary>
    /// Exception thrown when service location finds an assembly with multiple implementations of <see cref="IAssemblySelfRegistration"/>.
    /// </summary>
    /// <seealso cref="LandorphanIocServiceLocationException"/>
    public sealed class MultipleAssemblyRegistrarException : LandorphanIocServiceLocationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssemblyRegistrarException"/> class.
        /// </summary>
        public MultipleAssemblyRegistrarException() : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssemblyRegistrarException"/> class.
        /// </summary>
        /// <param name="message"> The message that describes the error. </param>
        public MultipleAssemblyRegistrarException(string message) : this(null, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssemblyRegistrarException"/> class.
        /// </summary>
        /// <param name="message"> The error message that explains the reason for the exception. </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public MultipleAssemblyRegistrarException(string message, Exception innerException) : this(null, message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssemblyRegistrarException"/> class.
        /// </summary>
        /// <param name="implementationTypes"> The abstract type that gave rise to this exception. </param>
        public MultipleAssemblyRegistrarException(IEnumerable<Type> implementationTypes) : this(implementationTypes, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssemblyRegistrarException"/> class.
        /// </summary>
        /// <param name="implementationTypes"> The implementation types that gave rise to this exception. </param>
        /// <param name="message"> The error message that explains the reason for the exception. </param>
        public MultipleAssemblyRegistrarException(IEnumerable<Type> implementationTypes, string message) : this(
            implementationTypes,
            message,
            null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssemblyRegistrarException"/> class.
        /// </summary>
        /// <param name="implementationTypes"> The implementation types that gave rise to this exception. </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public MultipleAssemblyRegistrarException(IEnumerable<Type> implementationTypes, Exception innerException)
            : this(implementationTypes, null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssemblyRegistrarException"/> class.
        /// </summary>
        /// <param name="implementationTypes"> The abstract type that gave rise to this exception. </param>
        /// <param name="message"> The error message that explains the reason for the exception. </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public MultipleAssemblyRegistrarException(IEnumerable<Type> implementationTypes, string message, Exception innerException)
            : base(NullToDefaultMessage(implementationTypes, message), innerException)
        {
            var cleaned = new List<Type>();
            if (!ReferenceEquals(null, implementationTypes))
            {
                foreach (var t in implementationTypes)
                {
                    if (!ReferenceEquals(null, t))
                    {
                        cleaned.Add(t);
                    }
                }
            }

            ImplementationTypes = cleaned;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssemblyRegistrarException"/> class.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        private MultipleAssemblyRegistrarException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ImplementationTypes = (List<Type>)info.GetValue("implementationTypes", typeof(List<Type>));
        }

        /// <inheritdoc/>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.ArgumentNotNull("info");
            info.AddValue("implementationTypes", ImplementationTypes, typeof(List<Type>));
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Gets the implementation types that gave rise to this exception
        /// </summary>
        public IEnumerable<Type> ImplementationTypes { get; }

        private static string NullToDefaultMessage(IEnumerable<Type> implementationTypes, string message)
        {
            var cleaned = new List<Type>();
            if (!ReferenceEquals(null, implementationTypes))
            {
                foreach (var t in implementationTypes)
                {
                    if (!ReferenceEquals(null, t))
                    {
                        cleaned.Add(t);
                    }
                }
            }

            string assemblyName;
            string assemblyVersion;
            string typesCsv;
            if (cleaned.Count == 0)
            {
                assemblyName = StringResources.NullReplacementValue;
                assemblyVersion = StringResources.NullReplacementValue;
                typesCsv = StringResources.NullReplacementValue;
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var t in cleaned)
                {
                    sb.Append(t.FullName);
                    sb.Append(", ");
                }

                const int trailingAppendedSuffixLength = 2;
                var an = cleaned[0].Assembly.GetName();
                assemblyName = an.FullName;
                assemblyVersion = an.Version.ToString();
                typesCsv = sb.ToString().Substring(0, sb.Length - trailingAppendedSuffixLength);
            }

            var rv = message ??
                     string.Format(
                         CultureInfo.InvariantCulture,
                         StringResources.MultipleAssemblyRegistrarDetectedFmt,
                         assemblyName,
                         assemblyVersion,
                         typesCsv);
            return rv;
        }
    }
}
