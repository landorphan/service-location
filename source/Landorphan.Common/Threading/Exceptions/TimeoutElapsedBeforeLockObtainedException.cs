namespace Landorphan.Common.Threading
{
   using System;
   using System.Globalization;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common.Resources;

   /// <summary>
   /// Represents a failure to obtain a lock.
   /// </summary>
   public sealed class TimeoutElapsedBeforeLockObtainedException : LandorphanException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="TimeoutElapsedBeforeLockObtainedException"/> class.
      /// </summary>
      public TimeoutElapsedBeforeLockObtainedException() : this(TimeSpan.MinValue)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TimeoutElapsedBeforeLockObtainedException"/> class.
      /// </summary>
      /// <param name="timeout">
      /// The timeout.
      /// </param>
      public TimeoutElapsedBeforeLockObtainedException(TimeSpan timeout) : this(timeout, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TimeoutElapsedBeforeLockObtainedException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message.
      /// </param>
      public TimeoutElapsedBeforeLockObtainedException(String message) : this(TimeSpan.MinValue, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TimeoutElapsedBeforeLockObtainedException"/> class with the inner exception that gave rise to
      /// this exception.
      /// </summary>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public TimeoutElapsedBeforeLockObtainedException(Exception innerException) : this(TimeSpan.MinValue, null, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TimeoutElapsedBeforeLockObtainedException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message.
      /// </param>
      /// <param name="innerException">
      /// The inner exception.
      /// </param>
      public TimeoutElapsedBeforeLockObtainedException(String message, Exception innerException) : this(
         TimeSpan.MinValue,
         message,
         innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TimeoutElapsedBeforeLockObtainedException"/> class.
      /// </summary>
      /// <param name="timeout">
      /// The timeout.
      /// </param>
      /// <param name="message">
      /// The message.
      /// </param>
      /// <param name="innerException">
      /// The inner exception.
      /// </param>
      public TimeoutElapsedBeforeLockObtainedException(TimeSpan timeout, String message, Exception innerException)
         : base(NullToDefaultMessage(message, timeout), innerException)
      {
         Timeout = timeout;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TimeoutElapsedBeforeLockObtainedException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized Object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      /// <exception cref="ArgumentNullException">
      /// The
      /// <paramref name="info"/>
      /// parameter is null.
      /// </exception>
      /// <exception cref="SerializationException">
      /// The class name is null or
      /// <see cref="Exception.HResult"/>
      /// is zero (0).
      /// </exception>
      private TimeoutElapsedBeforeLockObtainedException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         Timeout = (TimeSpan) info.GetValue("timeout", typeof(TimeSpan));
      }

      /// <summary>
      /// When overridden in a derived class, sets the <see cref="SerializationInfo"/> with information about the exception.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized Object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull(nameof(info));

         info.AddValue("timeout", Timeout);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the timeout.
      /// </summary>
      /// <value>
      /// The timeout.
      /// </value>
      public TimeSpan Timeout { get; }

      private static String NullToDefaultMessage(String message, TimeSpan timeout)
      {
         if (message != null)
         {
            return message;
         }

         var rv = String.Format(
            CultureInfo.CurrentCulture,
            StringResources.TimeoutElapsedBeforeLockObtainedExceptionDefaultMessageFmt,
            timeout);
         return rv;
      }
   }
}