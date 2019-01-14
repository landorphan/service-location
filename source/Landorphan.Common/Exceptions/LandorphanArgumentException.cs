namespace Landorphan.Common
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Runtime.Serialization;

   /// <summary>
   /// Base class for thrown <see cref="ArgumentException"/> instances specific to the <see cref="Landorphan.Common"/> implementations.
   /// </summary>
   /// <remarks>
   /// <para>
   /// <see cref="Landorphan.Common"/> throws BCL exceptions as appropriate; for example <see cref="ArgumentNullException"/>.
   /// </para>
   /// <para>
   /// This class changes the order of (paramName, message, innerException) arguments to be consistent all other exceptions.
   /// This is in contrast with <see cref="ArgumentException"/> and its derivatives in the BCL which violate the pattern.
   /// </para>
   /// </remarks>
   /// <seealso cref="LandorphanException"/>
   [SuppressMessage("SonarLint.CodeSmell", "S4027: Exceptions should provide standard constructors", Justification = "False positive (MWP)")]
   public abstract class LandorphanArgumentException : ArgumentException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanArgumentException"/> class.
      /// </summary>
      protected LandorphanArgumentException()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message that explains the reason for the exception.
      /// </param>
      protected LandorphanArgumentException(String message) : base(message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanArgumentException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      protected LandorphanArgumentException(String message, Exception innerException) : this(null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanArgumentException"/> class.
      /// </summary>
      /// <param name="paramName">
      /// The name of the parameter that caused the current exception.
      /// </param>
      /// <param name="message">
      /// The message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      protected LandorphanArgumentException(String paramName, String message, Exception innerException)
         : base(message, paramName, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanArgumentException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized Object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      protected LandorphanArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }
   }
}
