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
   /// Exception thrown when a type argument is a precluded type.
   /// </summary>
   [SuppressMessage("Microsoft.Maintainability", "CA1501: Avoid excessive inheritance", Justification = "Reviewed")]
   public sealed class ContainerFromTypePrecludedArgumentException : LandorphanIocServiceLocationArgumentException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypePrecludedArgumentException"/> class.
      /// </summary>
      public ContainerFromTypePrecludedArgumentException() : this(null, null, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypePrecludedArgumentException"/> class.
      /// </summary>
      /// <param name="message"> The error message that explains the reason for the exception. </param>
      public ContainerFromTypePrecludedArgumentException(String message) : this(null, null, null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypePrecludedArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ContainerFromTypePrecludedArgumentException(String message, Exception innerException) : this(null, null, null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypePrecludedArgumentException"/> class.
      /// </summary>
      /// <param name = "container">
      /// The container that issued this exception.
      /// </param>
      /// <param name="precludedType">
      /// The type that gave rise to this exception.
      /// </param>
      /// <param name="paramName">
      /// The name of the parameter that caused the exception.
      /// </param>
      public ContainerFromTypePrecludedArgumentException(IIocContainerMetaIdentity container, Type precludedType, String paramName) : this(container, precludedType, paramName, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypePrecludedArgumentException"/> class.
      /// </summary>
      /// <param name = "container">
      /// The container that issued this exception.
      /// </param>
      /// <param name="precludedType">
      /// The type that gave rise to this exception.
      /// </param>
      /// <param name="paramName">
      /// The name of the parameter that caused the exception.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference
      /// if no inner exception is specified.
      /// </param>
      // ReSharper disable ExpressionIsAlwaysNull
      public ContainerFromTypePrecludedArgumentException(IIocContainerMetaIdentity container, Type precludedType, String paramName, String message, Exception innerException)
         : base(paramName, NullToDefaultMessage(container, precludedType, paramName, message), innerException)
      {
         Container = container;
         PrecludedType = precludedType;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerFromTypePrecludedArgumentException"/> class.
      /// </summary>
      /// <param name="info"> The <see cref="SerializationInfo"/>
      /// that holds the serialized object data about the exception being thrown.
      /// </param>
      /// <param name="context"> The <see cref="StreamingContext"/>
      /// that contains contextual information about the source or destination.
      /// </param>
      // ReSharper disable once UnusedMember.Local
      private ContainerFromTypePrecludedArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         Container = (IIocContainer) info.GetValue("container", typeof(IIocContainerMetaIdentity));
         PrecludedType = (Type) info.GetValue("precludedType", typeof(Type));
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull("info");
         info.AddValue("container", Container);
         info.AddValue("precludedType", PrecludedType);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the actual type.
      /// </summary>
      public Type PrecludedType { get; }

      /// <summary>
      /// Gets the container that gave rise to this exception.
      /// </summary>
      public IIocContainerMetaIdentity Container { get; }

      private static String NullToDefaultMessage(IIocContainerMetaIdentity container, Type actualType, String paramName, String message)
      {
         var cleanedContainerUid = StringResources.NullReplacementValue;
         var cleanedContainerName = StringResources.NullReplacementValue;
         if (container != null)
         {
            cleanedContainerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
            cleanedContainerName = container.Name.TrimNullToEmpty();
         }

         var cleanedActualTypeFullName = null == actualType ? StringResources.NullReplacementValue : actualType.FullName;
         var cleanedParamName = paramName.TrimNullToEmpty();
         var paramNameSuffix = cleanedParamName.Length == 0
            ? StringResources.ArgumentExceptionNoParamNameSuffix
            : String.Format(CultureInfo.InvariantCulture, StringResources.ArgumentExceptionWithParamNameSuffixFmt, paramName);
         var rv = message ??
                  String.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.ContainerTypePrecludedArgumentExceptionFmt,
                     cleanedContainerUid,
                     cleanedContainerName,
                     cleanedActualTypeFullName,
                     paramNameSuffix);
         return rv;
      }
   }
}