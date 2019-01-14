namespace Landorphan.Common.Validation
{
   using System;
   using System.Globalization;
   using Landorphan.Common.Resources;

   // ReSharper disable AssignNullToNotNullAttribute

   /// <summary>
   /// Factory for instantiating <see cref="ValidationMessage"/>.
   /// </summary>
   public class ValidationMessageFactory : IValidationMessageFactory
   {
      /// <inheritdoc/>
      public IValidationMessage CreateErrorMessage(String text)
      {
         var rv = new ValidationMessage {IsError = true, MessageType = StringResources.MessageTypeError, Message = text};
         rv.MakeReadOnly();
         return rv;
      }

      /// <inheritdoc/>
      public IValidationMessage CreateErrorMessage(String format, params Object[] args)
      {
         return CreateErrorMessage(String.Format(CultureInfo.CurrentCulture, format, args));
      }

      /// <inheritdoc/>
      public IValidationMessage CreateInformationMessage(String text)
      {
         var rv = new ValidationMessage {IsError = false, MessageType = StringResources.MessageTypeInformation, Message = text};
         rv.MakeReadOnly();
         return rv;
      }

      /// <inheritdoc/>
      public IValidationMessage CreateInformationMessage(String format, params Object[] args)
      {
         return CreateInformationMessage(String.Format(CultureInfo.CurrentCulture, format, args));
      }

      /// <inheritdoc/>
      public IValidationMessage CreateVerboseMessage(String text)
      {
         var rv = new ValidationMessage {IsError = false, MessageType = StringResources.MessageTypeVerbose, Message = text};
         rv.MakeReadOnly();
         return rv;
      }

      /// <inheritdoc/>
      public IValidationMessage CreateVerboseMessage(String format, params Object[] args)
      {
         return CreateVerboseMessage(String.Format(CultureInfo.CurrentCulture, format, args));
      }

      /// <inheritdoc/>
      public IValidationMessage CreateWarningMessage(String text)
      {
         var rv = new ValidationMessage {IsError = false, MessageType = StringResources.MessageTypeWarning, Message = text};
         rv.MakeReadOnly();
         return rv;
      }

      /// <inheritdoc/>
      public IValidationMessage CreateWarningMessage(String format, params Object[] args)
      {
         return CreateWarningMessage(String.Format(CultureInfo.CurrentCulture, format, args));
      }
   }
}
