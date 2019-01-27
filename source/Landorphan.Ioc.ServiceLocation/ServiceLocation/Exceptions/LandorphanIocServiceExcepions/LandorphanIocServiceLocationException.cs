namespace Landorphan.Ioc.ServiceLocation.Exceptions
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Runtime.Serialization;
   using Landorphan.Common.Exceptions;

   /// <summary>
   /// Base class for thrown <see cref="Exception"/> instances specific to the <see cref="Landorphan.Ioc.ServiceLocation"/> implementations.
   /// </summary>
   /// <seealso cref="LandorphanArgumentException"/>
   [SuppressMessage("SonarLint.CodeSmell", "S4027: Exceptions should provide standard constructors", Justification = "False Positive(MWP)")]
   public abstract class LandorphanIocServiceLocationException : LandorphanException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationException"/> class.
      /// </summary>
      protected LandorphanIocServiceLocationException()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message that explains the reason for the exception.
      /// </param>
      protected LandorphanIocServiceLocationException(String message) : base(message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      protected LandorphanIocServiceLocationException(String message, Exception innerException) : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized Object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      protected LandorphanIocServiceLocationException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }
   }
}
