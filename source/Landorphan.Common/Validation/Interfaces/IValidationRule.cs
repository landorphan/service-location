namespace Landorphan.Common.Validation
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Represents a single readable validation rule for a domain entity.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this validation rule applies.
   /// </typeparam>
   public interface IValidationRule<TEntity> : IEquatable<IValidationRule<TEntity>>, ICloneable, IQueryReadOnly
   {
      /// <summary>
      /// Gets the description of this validation rule.
      /// </summary>
      /// <value>
      /// The description of this validation rule.
      /// </value>
      String Description { get; }

      /// <summary>
      /// Gets the entity type.
      /// </summary>
      /// <value>
      /// The entity type.
      /// </value>
      /// <remarks>
      /// Convenience property.
      /// </remarks>
      Type EntityType { get; }

      /// <summary>
      /// Gets the name of this validation rule.
      /// </summary>
      /// <value>
      /// The name of this validation rule.
      /// </value>
      String Name { get; }

      /// <summary>
      /// Gets the names of the properties this validation rule evaluates.
      /// </summary>
      /// <value>
      /// The names of the properties this validation rule evaluates.
      /// </value>
      IEnumerable<String> PropertyNames { get; }

      /// <summary>  
      /// Gets string comparer. 
      /// </summary>
      /// <returns>
      /// The string comparer. 
      /// </returns>
      /// <remarks>
      /// This is used primarily for cloning.
      /// </remarks>
      IEqualityComparer<String> GetStringComparer();

      /// <summary>
      /// Evaluates the given entity against this validation rule.
      /// </summary>
      /// <param name="entity">
      /// The <typeparamref name="TEntity"/> instance to validate.
      /// </param>
      /// <returns>
      /// A non-null instance of <see cref="IValidationRuleResult"/>.
      /// </returns>
      IValidationRuleResult Validate(TEntity entity);
   }
}
