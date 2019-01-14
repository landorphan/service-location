namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Globalization;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common;
   using Landorphan.Ioc.Resources;

   /// <summary>
   /// Exception thrown when service location finds an abstract implementation of <see cref="IAssemblySelfRegistration"/>.
   /// </summary>
   /// <seealso cref="LandorphanIocServiceLocationException"/>
   public sealed class AbstractAssemblyRegistrarException : LandorphanIocServiceLocationException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="AbstractAssemblyRegistrarException"/> class.
      /// </summary>
      public AbstractAssemblyRegistrarException() : this(null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AbstractAssemblyRegistrarException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message that describes the error.
      /// </param>
      public AbstractAssemblyRegistrarException(String message) : this(null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AbstractAssemblyRegistrarException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public AbstractAssemblyRegistrarException(String message, Exception innerException) : this(null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AbstractAssemblyRegistrarException"/> class.
      /// </summary>
      /// <param name="abstractType">
      /// The abstract type that gave rise to this exception.
      /// </param>
      public AbstractAssemblyRegistrarException(Type abstractType) : this(abstractType, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AbstractAssemblyRegistrarException"/> class.
      /// </summary>
      /// <param name="abstractType">
      /// The abstract type that gave rise to this exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public AbstractAssemblyRegistrarException(Type abstractType, Exception innerException) : this(abstractType, null, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AbstractAssemblyRegistrarException"/> class.
      /// </summary>
      /// <param name="abstractType">
      /// The abstract type that gave rise to this exception.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public AbstractAssemblyRegistrarException(Type abstractType, String message, Exception innerException)
         : base(NullToDefaultMessage(abstractType, message), innerException)
      {
         AbstractType = abstractType;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AbstractAssemblyRegistrarException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      private AbstractAssemblyRegistrarException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         AbstractType = (Type)info.GetValue("abstractType", typeof(Type));
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull("info");
         info.AddValue("abstractType", AbstractType);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the abstract type that gave rise to this exception.
      /// </summary>
      /// <value>
      /// The type of the abstract.
      /// </value>
      public Type AbstractType { get; }

      private static String NullToDefaultMessage(Type abstractType, String message)
      {
         var rv = message ??
                  String.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.AbstractAssemblyRegistrarExceptionDefaltMessageFmt,
                     abstractType.IsNull() ? StringResources.NullReplacementValue : abstractType.FullName);
         return rv;
      }
   }
}
