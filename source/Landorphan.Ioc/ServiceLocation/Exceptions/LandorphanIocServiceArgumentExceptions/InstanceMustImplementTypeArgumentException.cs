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
   /// Exception thrown when an instance argument does not implement a required interface or abstract type.
   /// </summary>
   [SuppressMessage("Microsoft.Maintainability", "CA1501: Avoid excessive inheritance", Justification = "Reviewed")]
   public sealed class InstanceMustImplementTypeArgumentException : LandorphanIocServiceLocationArgumentException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="InstanceMustImplementTypeArgumentException"/> class.
      /// </summary>
      public InstanceMustImplementTypeArgumentException() : this(null, null, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InstanceMustImplementTypeArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      public InstanceMustImplementTypeArgumentException(String message) : this(null, null, null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InstanceMustImplementTypeArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public InstanceMustImplementTypeArgumentException(String message, Exception innerException)
         : this(null, null, null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InstanceMustImplementTypeArgumentException"/> class.
      /// </summary>
      /// <param name="fromType">
      /// The required interface or abstract type.
      /// </param>
      /// <param name="instance">
      /// The value of the argument that gave rise to this exception.
      /// </param>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception.
      /// </param>
      public InstanceMustImplementTypeArgumentException(Type fromType, Object instance, String paramName)
         : this(fromType, instance, paramName, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InstanceMustImplementTypeArgumentException"/> class.
      /// </summary>
      /// <param name="fromType">
      /// The required interface or abstract type.
      /// </param>
      /// <param name="instance">
      /// The value of the argument that gave rise to this exception.
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
      public InstanceMustImplementTypeArgumentException(
         Type fromType,
         Object instance,
         String paramName,
         String message,
         Exception innerException) : base(paramName, NullToDefaultMessage(fromType, instance, paramName, message), innerException)
      {
         FromType = fromType;
         Instance = instance;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InstanceMustImplementTypeArgumentException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      // ReSharper disable once UnusedMember.Local
      private InstanceMustImplementTypeArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         Instance = info.GetValue("instance", typeof(Object));
         FromType = (Type)info.GetValue("fromType", typeof(Type));
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull("info");
         info.AddValue("instance", Instance);
         info.AddValue("fromType", FromType);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the required interface or abstract type.
      /// </summary>
      public Type FromType { get; }

      /// <summary>
      /// Gets the instance that gave rise to this exception.
      /// </summary>
      public Object Instance { get; }

      private static String NullToDefaultMessage(Type fromType, Object instance, String paramName, String message)
      {
         var cleanedFromType = fromType == null ? StringResources.NullReplacementValue : fromType.FullName;
         var cleanedInstanceType = instance == null ? StringResources.NullReplacementValue : instance.GetType().FullName;
         var cleanedParamName = paramName.TrimNullToEmpty();
         var paramNameSuffix = cleanedParamName.Length == 0
            ? StringResources.ArgumentExceptionNoParamNameSuffix
            : String.Format(CultureInfo.InvariantCulture, StringResources.ArgumentExceptionWithParamNameSuffixFmt, paramName);
         var rv = message ??
                  String.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.InstanceMustImplementTypeArgumentExceptionDefaultMessageFmt,
                     cleanedFromType,
                     cleanedInstanceType,
                     paramNameSuffix);
         return rv;
      }
   }
}
