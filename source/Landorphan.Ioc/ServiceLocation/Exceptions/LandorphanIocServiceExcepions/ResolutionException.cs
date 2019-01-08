namespace Landorphan.Ioc.ServiceLocation
{
   using System;
   using System.Globalization;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common;
   using Landorphan.Ioc.Resources;

   /// <summary>
   /// Exception thrown when a requested type could not be resolved to an implementation.
   /// </summary>
   /// <seealso cref="LandorphanIocServiceLocationException"/>
   public sealed class ResolutionException : LandorphanIocServiceLocationException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ResolutionException"/> class.
      /// </summary>
      public ResolutionException() : this(null, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ResolutionException"/> class.
      /// </summary>
      /// <param name="message">
      /// The message that describes the error.
      /// </param>
      public ResolutionException(String message) : this(null, null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ResolutionException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for the exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference
      /// if no inner exception is specified.
      /// </param>
      public ResolutionException(String message, Exception innerException) : this(null, null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ResolutionException"/> class.
      /// </summary>
      /// <param name="requestedType">
      /// The requested <see cref="Type"/> that could not be resolved.
      /// </param>
      public ResolutionException(Type requestedType) : this(requestedType, null, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ResolutionException"/> class.
      /// </summary>
      /// <param name="requestedType">
      /// The requested <see cref="Type"/> that could not be resolved.
      /// </param>
      /// <param name="name">
      /// The name.
      /// </param>
      public ResolutionException(Type requestedType, String name) : this(requestedType, name, null, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ResolutionException"/> class.
      /// </summary>
      /// <param name="requestedType">
      /// The requested <see cref="Type"/> that could not be resolved.
      /// </param>
      /// <param name="name">
      /// The requested name that could not be resolved.
      /// </param>
      /// <param name="message">
      /// The message.
      /// </param>
      /// <param name="innerException">
      /// The inner exception.
      /// </param>
      public ResolutionException(Type requestedType, String name, String message, Exception innerException)
         : base(NullToDefaultMessage(message, requestedType, name), innerException)
      {
         Name = name ?? StringResources.NullReplacementValue;
         RequestedType = requestedType;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ResolutionException"/> class.
      /// </summary>
      /// <param name="info"> The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param>
      /// <param name="context"> The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </param>
      private ResolutionException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         RequestedType = (Type) info.GetValue("requestedType", typeof(Type));
         Name = info.GetString("name");
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull("info");
         info.AddValue("requestedType", RequestedType);
         info.AddValue("name", Name);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the requested name that could not be resolved.
      /// </summary>
      public String Name { get; }

      /// <summary>
      /// Gets the requested type that could not be resolved.
      /// </summary>
      public Type RequestedType { get; }

      private static String NullToDefaultMessage(String message, Type requestedType, String name)
      {
         var rv = message ??
                  string.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.ResolutionIocExceptionDefaultMessageFmt,
                     requestedType == null ? StringResources.NullReplacementValue : requestedType.FullName,
                     name ?? string.Empty);

         return rv;
      }
   }
}