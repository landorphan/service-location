namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Globalization;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common;
   using Landorphan.Ioc.Resources;

   /// <summary>
   /// Exception thrown when an attempt is made to add a precluded type, but the configuration of the container disables preclusion of types.
   /// </summary>
   /// <seealso cref="LandorphanIocServiceLocationException"/>
   public sealed class ContainerConfigurationPrecludedTypesDisabledException : LandorphanIocServiceLocationException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationPrecludedTypesDisabledException"/> class.
      /// </summary>
      public ContainerConfigurationPrecludedTypesDisabledException() : this(null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationPrecludedTypesDisabledException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      public ContainerConfigurationPrecludedTypesDisabledException(String message) : this(null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationPrecludedTypesDisabledException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ContainerConfigurationPrecludedTypesDisabledException(String message, Exception innerException) : this(null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationPrecludedTypesDisabledException"/> class.
      /// </summary>
      /// <param name="container">
      /// The container that gave rise to this exception.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ContainerConfigurationPrecludedTypesDisabledException(IIocContainerMetaIdentity container, String message, Exception innerException)
         : base(NullToDefaultMessage(container, message), innerException)
      {
         Container = container;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationPrecludedTypesDisabledException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      // ReSharper disable once UnusedMember.Local
      private ContainerConfigurationPrecludedTypesDisabledException(SerializationInfo info, StreamingContext context) : base(info, context)
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

      private static String NullToDefaultMessage(IIocContainerMetaIdentity container, String message)
      {
         var cleanedContainerUid = StringResources.NullReplacementValue;
         var cleanedContainerName = StringResources.NullReplacementValue;
         if (container != null)
         {
            cleanedContainerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
            cleanedContainerName = container.Name.TrimNullToEmpty();
         }

         var rv = message ??
                  String.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.ContainerConfigurationPrecludedTypesDisabledExceptionDefaultMessageFmt,
                     cleanedContainerUid,
                     cleanedContainerName);
         return rv;
      }
   }
}
