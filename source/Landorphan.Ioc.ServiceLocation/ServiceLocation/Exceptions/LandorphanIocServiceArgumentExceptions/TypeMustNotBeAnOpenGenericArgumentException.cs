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
   /// Exception thrown when a type argument is an open generic type but required not to be an open generic type.
   /// </summary>
   [SuppressMessage("Microsoft.Maintainability", "CA1501: Avoid excessive inheritance", Justification = "Reviewed")]
   public sealed class TypeMustNotBeAnOpenGenericArgumentException : LandorphanIocServiceLocationArgumentException
   {
       /// <summary>
      /// Initializes a new instance of the <see cref="TypeMustNotBeAnOpenGenericArgumentException"/> class.
      /// </summary>
      public TypeMustNotBeAnOpenGenericArgumentException() : this(null, null, null, null)
      {
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="TypeMustNotBeAnOpenGenericArgumentException"/> class.
      /// </summary>
      /// <param name="message">The error message that explains the reason for the exception.</param>
      public TypeMustNotBeAnOpenGenericArgumentException(string message) : this(null, null, message, null)
      {
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="TypeMustNotBeAnOpenGenericArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference
      /// if no inner exception is specified.
      /// </param>
      public TypeMustNotBeAnOpenGenericArgumentException(string message, Exception innerException) : this(null, null, message, innerException)
      {
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="TypeMustNotBeAnOpenGenericArgumentException" /> class.
      /// </summary>
      /// <param name="actualType">
      /// The type that gave rise to this exception.
      /// </param>
      /// <param name="paramName">
      /// The name of the parameter that caused the exception.
      /// </param>
      public TypeMustNotBeAnOpenGenericArgumentException(Type actualType, string paramName) : this(actualType, paramName, null, null)
      {
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="TypeMustNotBeAnOpenGenericArgumentException" /> class.
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
      /// The exception that is the cause of the current exception, or a null reference
      /// if no inner exception is specified.
      /// </param>
      public TypeMustNotBeAnOpenGenericArgumentException(Type actualType, string paramName, string message, Exception innerException)
         : base(paramName, NullToDefaultMessage(actualType, paramName, message), innerException)
      {
         ActualType = actualType;
      }

       /// <summary>
      /// Initializes a new instance of the <see cref="TypeMustNotBeAnOpenGenericArgumentException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
      // ReSharper disable once UnusedMember.Local
      private TypeMustNotBeAnOpenGenericArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
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
      /// Gets the actual type that gave rise to this exception.
      /// </summary>
      public Type ActualType { get; }

       private static string NullToDefaultMessage(Type actualType, string paramName, string message)
      {
         var cleanedActualType = actualType == null ? StringResources.NullReplacementValue : actualType.FullName;
         var cleanedParamName = paramName.TrimNullToEmpty();
         var paramNameSuffix = cleanedParamName.Length == 0
            ? StringResources.ArgumentExceptionNoParamNameSuffix
            : string.Format(CultureInfo.InvariantCulture, StringResources.ArgumentExceptionWithParamNameSuffixFmt, paramName);
         var rv = message ??
                  string.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.TypeMustNotBeOpenGenericDefaultMessageFmt,
                     cleanedActualType,
                     paramNameSuffix);
         return rv;
      }
   }
}
