namespace Landorphan.Common.Validation
{
   using System;

   /// <summary>
   /// Represents an editable line-item entry in a <see cref="IValidationRuleResult"/> value.
   /// </summary>
   public interface IValidationMessageWriter : IValidationMessage, IConvertsToReadOnly
   {
      /// <summary>  
      /// Sets a value indicating whether or not this message represents a validation error
      /// </summary>
      /// <param name="isError"> 
      /// <code>true</code> when this instance represents a validation error; otherwise <code>false</code>. 
      /// </param>
      void SetIsError(Boolean isError);

      /// <summary>  
      /// Sets the message text.
      /// </summary>
      /// <param name="message">
      /// The message text.
      /// </param>
      void SetMessage(String message);

      /// <summary>  
      /// Sets the message type.
      /// </summary>
      /// <param name="messageType">
      /// The message type.
      /// </param>
      void SetMessageType(String messageType);
   }
}
