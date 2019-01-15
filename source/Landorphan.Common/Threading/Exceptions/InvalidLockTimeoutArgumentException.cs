namespace Landorphan.Common.Threading
{
   using System;
   using System.Globalization;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common.Resources;

   /// <summary>
   /// The exception thrown when a timeout value is invalid for a lock implementation.
   /// </summary>
   public sealed class InvalidLockTimeoutArgumentException : LandorphanArgumentException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="InvalidLockTimeoutArgumentException"/> class.
      /// </summary>
      public InvalidLockTimeoutArgumentException() : this(null, TimeSpan.MinValue, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InvalidLockTimeoutArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      public InvalidLockTimeoutArgumentException(String message) : this(null, TimeSpan.MinValue, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InvalidLockTimeoutArgumentException"/> class with the inner exception that gave rise to
      /// this exception.
      /// </summary>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public InvalidLockTimeoutArgumentException(Exception innerException) : this(null, null, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InvalidLockTimeoutArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public InvalidLockTimeoutArgumentException(String message, Exception innerException) : this(
         null,
         TimeSpan.MinValue,
         message,
         innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InvalidLockTimeoutArgumentException"/> class.
      /// </summary>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception, or an empty string if not supplied.
      /// </param>
      /// <param name="invalidValue">
      /// The invalid value that gave rise to this exception, or <see cref="TimeSpan.MinValue"/> if not supplied.
      /// </param>
      public InvalidLockTimeoutArgumentException(String paramName, TimeSpan invalidValue) : this(paramName, invalidValue, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InvalidLockTimeoutArgumentException"/> class.
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
      public InvalidLockTimeoutArgumentException(String paramName, String message, Exception innerException)
         : this(paramName, TimeSpan.MinValue, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InvalidLockTimeoutArgumentException"/> class.
      /// </summary>
      /// <param name="paramName">
      /// The name of the parameter that gave rise to this exception, or an empty string if not supplied.
      /// </param>
      /// <param name="invalidValue">
      /// The invalid value that gave rise to this exception, or <see cref="TimeSpan.MinValue"/> if not supplied.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public InvalidLockTimeoutArgumentException(String paramName, TimeSpan invalidValue, String message, Exception innerException)
         : base(paramName, NullToDefaultMessage(paramName, invalidValue, message), innerException)
      {
         InvalidValue = invalidValue;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InvalidLockTimeoutArgumentException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized Object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      private InvalidLockTimeoutArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         InvalidValue = TimeSpan.FromTicks(info.GetInt64("invalidValue"));
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull(nameof(info));

         info.AddValue("invalidValue", InvalidValue.Ticks);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the invalid value that gave rise to this exception.
      /// </summary>
      /// <value>
      /// The invalid value or <see cref="TimeSpan.MinValue"/> if not supplied.
      /// </value>
      public TimeSpan InvalidValue { get; }

      private static String NullToDefaultMessage(String paramName, TimeSpan invalidValue, String message)
      {
         // The implementation of ArgumentException will append "\r\nParameter name: {0}" if parameterName is supplied.
         // (not null, and not string.Empty)
         var rv = message ??
                  String.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.InvalidLockTimeoutArgumentExceptionMessageFmt,
                     paramName ?? String.Empty,
                     invalidValue.TotalMilliseconds.ToString("N0", CultureInfo.CurrentCulture),
                     Int32.MaxValue.ToString("N0", CultureInfo.CurrentCulture));

         return rv;
      }
   }
}
