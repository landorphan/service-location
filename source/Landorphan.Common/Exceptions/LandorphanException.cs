namespace Landorphan.Common
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Runtime.Serialization;

   /// <summary>
   /// Base class for thrown <see cref="Exception"/> instances specific to the <see cref="Landorphan.Common"/> implementation
   /// </summary>
   /// <remarks>
   /// The framework throws BCL exceptions as appropriate; for example <see cref="OperationCanceledException"/>.
   /// </remarks>
   /// <seealso cref="LandorphanArgumentException"/>
   [SuppressMessage("SonarLint.CodeSmell", "S4027: Exceptions should provide standard constructors", Justification = "False Positive(MWP)")]
   public abstract class LandorphanException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanException"/> class.
      /// </summary>
      protected LandorphanException()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message that explains the reason for the exception.
      /// </param>
      protected LandorphanException(String message) : base(message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      protected LandorphanException(String message, Exception innerException) : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized Object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      protected LandorphanException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }
   }
}
