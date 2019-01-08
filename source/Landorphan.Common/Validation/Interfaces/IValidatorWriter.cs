namespace Landorphan.Common.Validation
{
   using System;

   /// <summary>
   /// Represents an editable validation logic object for a domain entity.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this validator applies.
   /// </typeparam>
   public interface IValidatorWriter<TEntity> : IValidator<TEntity>, IConvertsToReadOnly
   {
      /// <summary>
      /// Adds a rule to this instance.
      /// </summary>
      /// <param name="rule"> The rule to be added. </param>
      /// <returns>
      /// <c> true </c> if the <paramref name="rule"/>  was added; otherwise <c> false </c> (e.g. duplicate rule).
      /// </returns>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      Boolean AddValidationRule(IValidationRule<TEntity> rule);

      /// <summary>
      /// Clears this instance of all rules.
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void ClearValidationRules();

      /// <summary>
      /// Removes a rule from this instance.
      /// </summary>
      /// <param name="rule"> The rule to be removed. </param>
      /// <returns>
      /// <c> true </c> if the <paramref name="rule"/>  was removed; otherwise <c> false </c> (e.g. no matching rule found).
      /// </returns>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      Boolean RemoveValidationRule(IValidationRule<TEntity> rule);

      /// <summary>
      /// Sets the description of this validator.
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void SetDescription(String description);

      /// <summary>
      /// Sets the name of this validator.
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void SetName(String name);
   }
}