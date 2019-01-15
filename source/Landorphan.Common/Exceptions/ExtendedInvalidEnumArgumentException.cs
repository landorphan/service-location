namespace Landorphan.Common
{
   using System;
   using System.ComponentModel;
   using System.Globalization;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common.Resources;

   /// <summary>
   /// The exception thrown when using invalid arguments that are enumerators.
   /// </summary>
   /// <remarks>
   /// Redo of <see cref="InvalidEnumArgumentException"/> which only provides 32 bits of storage for the invalid value and does not expose it.
   /// </remarks>
   public sealed class ExtendedInvalidEnumArgumentException : LandorphanArgumentException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ExtendedInvalidEnumArgumentException"/> class.
      /// </summary>
      public ExtendedInvalidEnumArgumentException() : this(null, 0, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ExtendedInvalidEnumArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      public ExtendedInvalidEnumArgumentException(String message) : this(null, 0, null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ExtendedInvalidEnumArgumentException"/> class with the inner exception that gave rise to
      /// this exception.
      /// </summary>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ExtendedInvalidEnumArgumentException(Exception innerException) : this(null, null, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ExtendedInvalidEnumArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ExtendedInvalidEnumArgumentException(String message, Exception innerException) : this(null, 0, null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ExtendedInvalidEnumArgumentException"/> class.
      /// </summary>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception, or an empty string if not supplied.
      /// </param>
      /// <param name="invalidValue">
      /// The invalid value that gave rise to this exception, or zero if not supplied.
      /// </param>
      /// <param name="enumType">
      /// The enumeration type, or <see cref="System.Object"/> type if not supplied.
      /// </param>
      public ExtendedInvalidEnumArgumentException(String paramName, Int64 invalidValue, Type enumType)
         : this(paramName, invalidValue, enumType, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ExtendedInvalidEnumArgumentException"/> class.
      /// </summary>
      /// <param name="invalidValue">
      /// The invalid value that gave rise to this exception, or zero if not supplied.
      /// </param>
      /// <param name="enumType">
      /// The enumeration type, or <see cref="System.Object"/> type if not supplied.
      /// </param>
      public ExtendedInvalidEnumArgumentException(Int64 invalidValue, Type enumType) : this(null, invalidValue, enumType, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ExtendedInvalidEnumArgumentException"/> class.
      /// </summary>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception, or an empty string if not supplied.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ExtendedInvalidEnumArgumentException(String paramName, String message, Exception innerException)
         : this(paramName, 0, null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ExtendedInvalidEnumArgumentException"/> class.
      /// </summary>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception, or an empty string if not supplied.
      /// </param>
      /// <param name="invalidValue">
      /// The invalid value that gave rise to this exception, or zero if not supplied.
      /// </param>
      /// <param name="enumType">
      /// The enumeration type, or <see cref="System.Object"/> type if not supplied.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ExtendedInvalidEnumArgumentException(
         String paramName,
         Int64 invalidValue,
         Type enumType,
         String message,
         Exception innerException)
         : base(paramName, NullToDefaultMessage(paramName, invalidValue, enumType, message), innerException)
      {
         InvalidValue = invalidValue;
         EnumType = enumType ?? typeof(Object);
         UnderlyingType = GetEnumUnderlyingType(enumType);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ExtendedInvalidEnumArgumentException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized Object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      private ExtendedInvalidEnumArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         InvalidValue = info.GetInt64("invalidValue");
         EnumType = (Type)info.GetValue("enumType", typeof(Type));
         UnderlyingType = (Type)info.GetValue("underlyingType", typeof(Type));
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull(nameof(info));

         info.AddValue("invalidValue", InvalidValue);
         info.AddValue("enumType", EnumType);
         info.AddValue("underlyingType", UnderlyingType);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the type of the enumeration.
      /// </summary>
      /// <value>
      /// The type of the enumeration, or <see cref="System.Object"/> type if not supplied.
      /// </value>
      public Type EnumType { get; }

      /// <summary>
      /// Gets the invalid value that gave rise to this exception.
      /// </summary>
      /// <value>
      /// The invalid value.
      /// </value>
      public Int64 InvalidValue { get; }

      /// <summary>
      /// Gets the underlying type.
      /// </summary>
      /// <value>
      /// The underlying type, or <see cref="System.Object"/> type <see cref="EnumType"/> was not supplied or was is not an enum type.
      /// </value>
      public Type UnderlyingType { get; }

      private static Type GetEnumUnderlyingType(Type enumType)
      {
         if (ReferenceEquals(null, enumType) || !enumType.IsEnum)
         {
            return typeof(Object);
         }

         return Enum.GetUnderlyingType(enumType);
      }

      private static String NullToDefaultMessage(String paramName, Int64 invalidValue, Type enumType, String message)
      {
         // The implementation of ArgumentException will append "\r\nParameter name: {0}" if parameterName is supplied.
         // (not null, and not string.Empty)
         var rv = message ??
                  String.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.ExtendedInvalidEnumArgumentExceptionMessageFmt,
                     paramName ?? String.Empty,
                     invalidValue.ToString("N0", CultureInfo.CurrentCulture),
                     enumType?.Name ?? "Object");
         return rv;
      }
   }
}
