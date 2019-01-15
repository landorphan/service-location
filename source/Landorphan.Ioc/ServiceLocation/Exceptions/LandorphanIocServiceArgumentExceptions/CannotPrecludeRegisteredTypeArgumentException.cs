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
   /// Exception thrown when an attempt is made to precluded a type that has already been registered.
   /// </summary>
   [SuppressMessage("Microsoft.Maintainability", "CA1501: Avoid excessive inheritance", Justification = "Reviewed")]
   public sealed class CannotPrecludeRegisteredTypeArgumentException : LandorphanIocServiceLocationArgumentException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CannotPrecludeRegisteredTypeArgumentException"/> class.
      /// </summary>
      public CannotPrecludeRegisteredTypeArgumentException() : this(null, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CannotPrecludeRegisteredTypeArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      public CannotPrecludeRegisteredTypeArgumentException(String message) : this(null, null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CannotPrecludeRegisteredTypeArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public CannotPrecludeRegisteredTypeArgumentException(String message, Exception innerException) : this(null, null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CannotPrecludeRegisteredTypeArgumentException"/> class.
      /// </summary>
      /// <param name="registeredType">
      /// The registered type that gave rise to this exception.
      /// </param>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception.
      /// </param>
      public CannotPrecludeRegisteredTypeArgumentException(Type registeredType, String paramName) : this(registeredType, paramName, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CannotPrecludeRegisteredTypeArgumentException"/> class.
      /// </summary>
      /// <param name="registeredType">
      /// The registered type that gave rise to this exception.
      /// </param>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public CannotPrecludeRegisteredTypeArgumentException(Type registeredType, String paramName, String message, Exception innerException)
         : base(paramName, NullToDefaultMessage(registeredType, paramName, message), innerException)
      {
         RegisteredType = registeredType;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CannotPrecludeRegisteredTypeArgumentException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      // ReSharper disable once UnusedMember.Local
      private CannotPrecludeRegisteredTypeArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         RegisteredType = (Type)info.GetValue("registeredType", typeof(Type));
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull("info");
         info.AddValue("registeredType", RegisteredType);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the registered type.
      /// </summary>
      public Type RegisteredType { get; }

      private static String NullToDefaultMessage(Type registeredType, String paramName, String message)
      {
         var cleanedRegisteredType = null == registeredType ? StringResources.NullReplacementValue : registeredType.FullName;
         var cleanedParamName = paramName.TrimNullToEmpty();
         var paramNameSuffix = cleanedParamName.Length == 0
            ? StringResources.ArgumentExceptionNoParamNameSuffix
            : String.Format(CultureInfo.InvariantCulture, StringResources.ArgumentExceptionWithParamNameSuffixFmt, paramName);
         var rv = message ??
                  String.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.CannotPrecludeRegisteredTypeArgumentExceptionDefaultMessageFmt,
                     cleanedRegisteredType,
                     paramNameSuffix);
         return rv;
      }
   }
}
