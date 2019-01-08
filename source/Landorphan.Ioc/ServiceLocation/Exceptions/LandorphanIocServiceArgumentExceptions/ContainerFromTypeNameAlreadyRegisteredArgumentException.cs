namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common;
   using Landorphan.Ioc.Resources;

   /// <summary>
   /// Exception thrown when a container already has the given abstract or interface type registered with the same name.
   /// </summary>
   /// <remarks>
   /// This is a key violations.  There can only be one type per name.  String.Empty, null reference, and whitespace all represent the default registration.
   /// </remarks>
   [SuppressMessage("Microsoft.Maintainability", "CA1501: Avoid excessive inheritance", Justification = "Reviewed")]
   public sealed class ContainerFromTypeNameAlreadyRegisteredArgumentException : LandorphanIocServiceLocationArgumentException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypeNameAlreadyRegisteredArgumentException"/> class.
      /// </summary>
      public ContainerFromTypeNameAlreadyRegisteredArgumentException() : this(null, null, null, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypeNameAlreadyRegisteredArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      public ContainerFromTypeNameAlreadyRegisteredArgumentException(String message) : this(null, null, null, null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypeNameAlreadyRegisteredArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ContainerFromTypeNameAlreadyRegisteredArgumentException(String message, Exception innerException) : this(null, null, null, null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypeNameAlreadyRegisteredArgumentException" /> class.
      /// </summary>
      /// <param name="paramName">
      /// The registeredName of the parameter that caused the exception.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ContainerFromTypeNameAlreadyRegisteredArgumentException(String paramName, String message, Exception innerException) : this(null, null, null, paramName, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypeNameAlreadyRegisteredArgumentException" /> class.
      /// </summary>
      /// <param name = "container">
      /// The container that issued this exception.
      /// </param>
      /// <param name="registeredType">
      /// The registered type that gave rise to this exception.
      /// </param>
      /// <param name="registeredName">
      /// The registered name that gave rise to this exception.
      /// </param>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception.
      /// </param>
      /// <remarks>
      /// The pair:  <paramref name="registeredType"/> and <paramref name="registeredName"/> represent the key to the registrations database, per container.
      /// </remarks>
      public ContainerFromTypeNameAlreadyRegisteredArgumentException(IIocContainerMetaIdentity container, Type registeredType, String registeredName, String paramName) : this(
         container,
         registeredType,
         registeredName,
         paramName,
         null,
         null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypeNameAlreadyRegisteredArgumentException" /> class.
      /// </summary>
      /// <param name = "container">
      /// The container that issued this exception.
      /// </param>
      /// <param name="registeredType">
      /// The registered type that gave rise to this exception.
      /// </param>
      /// <param name="registeredName">
      /// The registered name that gave rise to this exception.
      /// </param>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for this exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      // ReSharper disable ExpressionIsAlwaysNull
      public ContainerFromTypeNameAlreadyRegisteredArgumentException(
         IIocContainerMetaIdentity container,
         Type registeredType,
         String registeredName,
         String paramName,
         String message,
         Exception innerException)
         : base(paramName, NullToDefaultMessage(container, registeredType, registeredName, paramName, message), innerException)
      {
         Container = container;
         RegisteredName = registeredName;
         RegisteredType = registeredType;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypeNameAlreadyRegisteredArgumentException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext" /> that contains contextual information about the source or destination.
      /// </param>
      // ReSharper disable once UnusedMember.Local
      private ContainerFromTypeNameAlreadyRegisteredArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         Container = (IIocContainerMetaIdentity) info.GetValue("container", typeof(IIocContainerMetaIdentity));
         RegisteredName = (String) info.GetValue("registeredName", typeof(String));
         RegisteredType = (Type) info.GetValue("registeredType", typeof(Type));
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull("info");
         info.AddValue("container", Container);
         info.AddValue("registeredName", RegisteredName);
         info.AddValue("registeredType", RegisteredType);

         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the container that gave rise to this exception.
      /// </summary>
      public IIocContainerMetaIdentity Container { get; }

      /// <summary>
      /// Gets the registered name that gave rise to this exception.
      /// </summary>
      public String RegisteredName { get; }

      /// <summary>
      /// Gets the registered type that gave rise to this exception.
      /// </summary>
      public Type RegisteredType { get; }

      private static String NullToDefaultMessage(IIocContainerMetaIdentity container, Type registeredType, String registeredName, String paramName, String message)
      {
         var cleanedContainerUid = StringResources.NullReplacementValue;
         var cleanedContainerName = StringResources.NullReplacementValue;
         if (container != null)
         {
            cleanedContainerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
            cleanedContainerName = container.Name.TrimNullToEmpty();
         }

         var cleanedRegisteredType = null == registeredType ? StringResources.NullReplacementValue : registeredType.FullName;
         var cleanedRegisteredName = registeredName.TrimNullToEmpty();
         var cleanedParamName = paramName.TrimNullToEmpty();

         var paramNameSuffix = cleanedParamName.Length == 0
            ? StringResources.ArgumentExceptionNoParamNameSuffix
            : String.Format(CultureInfo.InvariantCulture, StringResources.ArgumentExceptionWithParamNameSuffixFmt, paramName);

         var rv = message ??
                  String.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.ContainerTypeNameAlreadyRegisteredArgumentExceptionFmt,
                     cleanedContainerUid,
                     cleanedContainerName,
                     cleanedRegisteredType,
                     cleanedRegisteredName,
                     paramNameSuffix);
         return rv;
      }
   }
}