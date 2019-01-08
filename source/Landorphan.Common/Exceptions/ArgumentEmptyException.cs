namespace Landorphan.Common
{
   using System;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common.Resources;

   /// <summary>
   /// The exception that is thrown when a empty reference is passed to a method that does not accept it as a valid argument.
   /// </summary>
   public sealed class ArgumentEmptyException : LandorphanArgumentException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ArgumentEmptyException"/> class.
      /// </summary>
      public ArgumentEmptyException() : this(null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ArgumentEmptyException"/> class with the name of the parameter that caused this
      /// exception.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for this exception.
      /// </param>
      public ArgumentEmptyException(String message) : this(null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ArgumentEmptyException"/> class with the inner exception that gave rise to
      /// this exception.
      /// </summary>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ArgumentEmptyException(Exception innerException) : this(null, null, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ArgumentEmptyException"/> class with a specified error message and the exception that
      /// is the cause of this exception.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for this exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ArgumentEmptyException(String message, Exception innerException) : this(null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes an instance of the <see cref="ArgumentEmptyException"/> class with a specified error message and the name of the
      /// parameter that causes this exception.
      /// </summary>
      /// <param name="paramName">
      /// The name of the parameter that caused the exception.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for this exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ArgumentEmptyException(String paramName, String message, Exception innerException)
         : base(paramName, message ?? StringResources.ArgumentEmptyExceptionDefaultMessage, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ArgumentEmptyException"/> class with serialized data.
      /// </summary>
      /// <param name="info"> The Object that holds the serialized Object data. </param>
      /// <param name="context"> An Object that describes the source or destination of the serialized data. </param>
      [SecurityCritical]
      private ArgumentEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }
   }
}