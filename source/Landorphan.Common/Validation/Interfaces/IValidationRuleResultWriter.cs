namespace Landorphan.Common.Validation
{
   using System;

   /// <summary>
   /// Represents a single writable result from a a call to <see cref="IValidationRule{TEntity}.Validate"/> in it's entirety; 
   /// a mash-up of a human-readable rule, and the resultant human readable messages.
   /// </summary>
   public interface IValidationRuleResultWriter : IValidationRuleResult, IConvertsToReadOnly
   {
      /// <summary>
      /// Adds a message to the <see cref="IValidationRuleResult.Messages"/> collection.
      /// </summary>
      /// <param name="message">
      /// The message to be added to the <see cref="IValidationRuleResult.Messages"/> collection.
      /// </param>
      /// <returns>
      /// <code>true</code> if <paramref name="message"/> was successfully added to the 
      /// <see cref="IValidationRuleResult.Messages"/> collection ; otherwise, <code>false</code>.
      /// </returns>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      Boolean AddMessage(IValidationMessage message);

      /// <summary>
      /// Removes all messages from the <see cref="IValidationRuleResult.Messages"/> collection.
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void ClearMessages();

      /// <summary>
      /// Removes a message from the <see cref="IValidationRuleResult.Messages"/> collection.
      /// </summary>
      /// <param name="message">
      /// The message to remove from the <see cref="IValidationRuleResult.Messages"/> collection.
      /// </param>
      /// <returns>
      /// <code>true</code> if <paramref name="message"/> was successfully removed from the 
      /// <see cref="IValidationRuleResult.Messages"/> collection ; otherwise, <code>false</code>.
      /// </returns>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      Boolean RemoveMessage(IValidationMessage message);

      /// <summary>
      /// Gets the evaluated entity that gave rise to this instance.
      /// </summary>
      /// <value>
      /// The evaluated entity.
      /// </value>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void SetEvaluatedEntity(Object evaluatedEntity);

      /// <summary>  
      /// Sets the description of the validation rule applied that gave rise to this instance.
      /// </summary>
      /// <value> 
      /// The description of validation rule applied.
      /// </value>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void SetValidationRuleDescription(String validationRuleDescription);

      /// <summary>  
      /// Sets the name of the validation rule applied that gave rise to this instance.
      /// </summary>
      /// <value> 
      /// The name of validation rule applied. 
      /// </value>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void SetValidationRuleName(String validationRuleName);
   }
}