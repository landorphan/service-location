namespace Landorphan.Ioc.ServiceLocation.Exceptions
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.Runtime.Serialization;
   using System.Security;
   using Landorphan.Common;
   using Landorphan.Ioc.Resources;
   using Landorphan.Ioc.ServiceLocation.Interfaces;

   /// <summary>
   /// Thrown when attempting to change the configuration of a locked <see cref="IIocContainerConfiguration"/> instance.
   /// </summary>
   [SuppressMessage("SonarLint.CodeSmell", "S4027: Exceptions should provide standard constructors", Justification = "False Positive(MWP)")]
   public sealed class ContainerConfigurationLockedException : LandorphanIocServiceLocationException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationLockedException"/> class.
      /// </summary>
      public ContainerConfigurationLockedException()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationLockedException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for this exception.
      /// </param>
      public ContainerConfigurationLockedException(String message) : this(null, message, null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationLockedException"/> class.
      /// </summary>
      /// <param name="message">
      /// The error message that explains the reason for this exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ContainerConfigurationLockedException(String message, Exception innerException) : this(null, message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ContainerConfigurationLockedException"/> class.
      /// </summary>
      /// <param name="container">
      /// The container that gave rise to this exception.
      /// </param>
      /// <param name="message">
      /// The error message that explains the reason for this exception.
      /// </param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference if no inner exception is specified.
      /// </param>
      public ContainerConfigurationLockedException(IIocContainerMetaIdentity container, String message, Exception innerException) : base(NullToDefaultMessage(container, message), innerException)
      {
         Container = container;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LandorphanIocServiceLocationException"/> class.
      /// </summary>
      /// <param name="info">
      /// The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
      /// </param>
      /// <param name="context">
      /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      /// </param>
      // ReSharper disable once UnusedMember.Local
      private ContainerConfigurationLockedException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }

      /// <inheritdoc/>
      [SecurityCritical]
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.ArgumentNotNull("info");
         info.AddValue("container", Container);
         base.GetObjectData(info, context);
      }

      /// <summary>
      /// Gets the <see cref="IIocContainerMetaIdentity"></see> which gave rise to this exception.
      /// </summary>
      /// <returns>
      /// The <see cref="IIocContainerMetaIdentity"></see> which gave rise to this exception.
      /// </returns>
      /// <remarks>
      /// May be a null-reference.
      /// </remarks>
      public IIocContainerMetaIdentity Container { get; }

      private static String NullToDefaultMessage(IIocContainerMetaIdentity container, String message)
      {
         var cleanedContainerUid = StringResources.NullReplacementValue;
         var cleanedContainerName = StringResources.NullReplacementValue;
         if (container != null)
         {
            cleanedContainerUid = container.Uid.ToString("D", CultureInfo.InvariantCulture);
            cleanedContainerName = container.Name.TrimNullToEmpty();
         }

         var rv = message ??
                  String.Format(
                     CultureInfo.InvariantCulture,
                     StringResources.IocContainerConfigurationLockedExceptionDefaltMessageFmt,
                     container.IsNull() ? StringResources.NullReplacementValue : cleanedContainerUid,
                     cleanedContainerName
                  );
         return rv;
      }
   }
}
