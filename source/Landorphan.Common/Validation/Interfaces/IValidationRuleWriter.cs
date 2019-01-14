namespace Landorphan.Common.Validation
{
   using System;

   /// <summary>
   /// Represents a single editable validation rule for a domain entity.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this validation rule applies.
   /// </typeparam>
   public interface IValidationRuleWriter<TEntity> : IValidationRule<TEntity>, IConvertsToReadOnly
   {
      /// <summary>
      /// Adds a property name to the <see cref="IValidationRule{TEntity}.PropertyNames"/> collection.
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      /// <param name="propertyName">
      /// The property name to add to the <see cref="IValidationRule{TEntity}.PropertyNames"/> collection.
      /// </param>
      /// <returns>
      /// <code>true</code> if <paramref name="propertyName"/> was successfully added to the 
      /// <see cref="IValidationRule{TEntity}.PropertyNames"/> collection ; otherwise, <code>false</code>.
      /// </returns>
      Boolean AddPropertyName(String propertyName);

      /// <summary>
      /// Removes all property names from the <see cref="IValidationRule{TEntity}.PropertyNames"/> collection.
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void ClearPropertyNames();

      /// <summary>
      /// Removes a property name from the <see cref="IValidationRule{TEntity}.PropertyNames"/> collection.
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      /// <param name="propertyName">
      /// The property name to remove from the <see cref="IValidationRule{TEntity}.PropertyNames"/> collection.
      /// </param>
      /// <returns>
      /// <code>true</code> if <paramref name="propertyName"/> was successfully removed from the 
      /// <see cref="IValidationRule{TEntity}.PropertyNames"/> collection; otherwise, <code>false</code>.
      /// </returns>
      Boolean RemovePropertyName(String propertyName);

      /// <summary>
      /// Sets the description of this validation rule.
      /// </summary>
      /// <param name="description">
      /// The description of the rule.
      /// </param>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void SetDescription(String description);

      /// <summary>
      /// Sets the name of this validation rule.
      /// </summary>
      /// <param name="name">
      /// The name of the rule.
      /// </param>
      /// <exception cref="NotSupportedException">
      /// The current instance is read-only.
      /// </exception>
      void SetName(String name);
   }
}
