namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Globalization;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common;
   using Landorphan.Ioc.Resources;

   /// <summary>
   /// Exception thrown when service location finds an implementation of <see cref="IAssemblySelfRegistration"/> without a public parameterless constructor.
   /// </summary>
   /// <seealso cref="LandorphanIocServiceLocationException"/>
   public sealed class AssemblyRegistrarMustHavePublicDefaultConstructorException : LandorphanIocServiceLocationException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyRegistrarMustHavePublicDefaultConstructorException"/> class.
      /// </summary>
      public AssemblyRegistrarMustHavePublicDefaultConstructorException() : this(null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyRegistrarMustHavePublicDefaultConstructorException"/> class.
      /// </summary>
      /// <param name="message"> The message that describes the error. </param>
      public AssemblyRegistrarMustHavePublicDefaultConstructorException(String message) : this(null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyRegistrarMustHavePublicDefaultConstructorException"/> class.
      /// </summary>
      /// <param name="message"> The error message that explains the reason for the exception. </param>
      /// <param name="innerException"> The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
      public AssemblyRegistrarMustHavePublicDefaultConstructorException(String message, Exception innerException) : this(null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyRegistrarMustHavePublicDefaultConstructorException"/> class.
      /// </summary>
      /// <param name="implementationType"> The type implementing <see cref="IAssemblySelfRegistration"/>that gave rise to this exception. </param>
      public AssemblyRegistrarMustHavePublicDefaultConstructorException(Type implementationType) : this(implementationType, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyRegistrarMustHavePublicDefaultConstructorException"/> class.
      /// </summary>
      /// <param name="implementationType"> The type implementing <see cref="IAssemblySelfRegistration"/>that gave rise to this exception. </param>
      /// <param name="message"> The error message that explains the reason for the exception. </param>
      public AssemblyRegistrarMustHavePublicDefaultConstructorException(Type implementationType, String message) : this(implementationType, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyRegistrarMustHavePublicDefaultConstructorException"/> class.
      /// </summary>
      /// <param name="implementationType"> The type implementing <see cref="IAssemblySelfRegistration"/>that gave rise to this exception. </param>
      /// <param name="innerException"> The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
      public AssemblyRegistrarMustHavePublicDefaultConstructorException(Type implementationType, Exception innerException) : this(
         implementationType,
         null,
         innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyRegistrarMustHavePublicDefaultConstructorException"/> class.
      /// </summary>
      /// <param name="implementationType"> The type implementing <see cref="IAssemblySelfRegistration"/>that gave rise to this exception. </param>
      /// <param name="message"> The error message that explains the reason for the exception. </param>
      /// <param name="innerException"> The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
      public AssemblyRegistrarMustHavePublicDefaultConstructorException(Type implementationType, String message, Exception innerException)
         : base(NullToDefaultMessage(implementationType, message), innerException)
      {
         ImplementationType = implementationType;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyRegistrarMustHavePublicDefaultConstructorException"/> class.
      /// </summary>
      /// <param name="info"> The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param>
      /// <param name="context"> The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </param>
      private AssemblyRegistrarMustHavePublicDefaultConstructorException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         ImplementationType = (Type) info.GetValue("fromType", typeof(Type));
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull("info");
         info.AddValue("fromType", ImplementationType);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the implementation type that gave rise to this exception.
      /// </summary>
      public Type ImplementationType { get; }

      private static String NullToDefaultMessage(Type implementationType, String message)
      {
         var rv = message ??
                  string.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.AssemblyRegistrarMustHavePublicDefaultConstructorExceptionDefaultMessageFmt,
                     implementationType.IsNull() ? StringResources.NullReplacementValue : implementationType.FullName);
         return rv;
      }
   }
}