namespace Landorphan.Ioc.ServiceLocation.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security;
    using Landorphan.Common;
    using Landorphan.Ioc.Resources;

    /// <summary>
   /// Exception thrown when a type argument is required to be concrete but is abstract.
   /// </summary>
   [SuppressMessage("Microsoft.Maintainability", "CA1501: Avoid excessive inheritance", Justification = "Reviewed")]
   public sealed class ToTypeMustNotBeInterfaceNorAbstractArgumentException : LandorphanIocServiceLocationArgumentException
   {
       /// <summary>
      /// Initializes a new instance of the <see cref="ToTypeMustNotBeInterfaceNorAbstractArgumentException"/> class.
      /// </summary>
      public ToTypeMustNotBeInterfaceNorAbstractArgumentException() : this(null, null, null, null)
      {
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="ToTypeMustNotBeInterfaceNorAbstractArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      public ToTypeMustNotBeInterfaceNorAbstractArgumentException(string message) : this(null, null, message, null)
      {
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="ToTypeMustNotBeInterfaceNorAbstractArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference
      /// if no inner exception is specified.
      /// </param>
      public ToTypeMustNotBeInterfaceNorAbstractArgumentException(string message, Exception innerException) : this(null, null, message, innerException)
      {
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="ToTypeMustNotBeInterfaceNorAbstractArgumentException"/> class.
      /// </summary>
      /// <param name="toType">
      /// The type that gave rise to this exception.
      /// </param>
      /// <param name="paramName">
      /// The name of the parameter that caused the exception.
      /// </param>
      public ToTypeMustNotBeInterfaceNorAbstractArgumentException(Type toType, string paramName) : this(toType, paramName, null, null)
      {
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="ToTypeMustNotBeInterfaceNorAbstractArgumentException"/> class.
      /// </summary>
      /// <param name="toType">
      /// The type that gave rise to this exception. </param>
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
      public ToTypeMustNotBeInterfaceNorAbstractArgumentException(Type toType, string paramName, string message, Exception innerException)
         : base(paramName, NullToDefaultMessage(toType, paramName, message), innerException)
      {
         ToType = toType;
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="ToTypeMustNotBeInterfaceNorAbstractArgumentException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      // ReSharper disable once UnusedMember.Local
      private ToTypeMustNotBeInterfaceNorAbstractArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         ToType = (Type)info.GetValue("fromType", typeof(Type));
      }

       /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull("info");
         info.AddValue("toType", ToType);
         base.GetObjectData(info, context);
      }

       /// <summary>
      /// Gets the type that gave rise to this exception.
      /// </summary>
      public Type ToType { get; }

       private static string NullToDefaultMessage(Type toType, string paramName, string message)
      {
         var cleanedToType = null == toType ? StringResources.NullReplacementValue : toType.FullName;
         var cleanedParamName = paramName.TrimNullToEmpty();
         var paramNameSuffix = cleanedParamName.Length == 0
            ? StringResources.ArgumentExceptionNoParamNameSuffix
            : string.Format(CultureInfo.InvariantCulture, StringResources.ArgumentExceptionWithParamNameSuffixFmt, paramName);
         var rv = message ??
                  string.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.ToTypeMustNotBeAbstractOrInterfaceArgumentExceptionDefaultMessageFmt,
                     cleanedToType,
                     paramNameSuffix);
         return rv;
      }
   }
}
