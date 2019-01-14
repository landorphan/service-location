namespace Landorphan.Common.Validation
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Represents an readable validation logic object for a domain entity.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this validator applies.
   /// </typeparam>
   /// <remarks>
   /// Applies zero to many rules against a single instance of <typeparamref name="TEntity"/>.
   /// </remarks>
   public interface IValidator<TEntity> : IEquatable<IValidator<TEntity>>, IQueryReadOnly, ICloneable
   {
      /// <summary>
      /// Gets the description of this validator.
      /// </summary>
      /// <value>
      /// The description of this validator.
      /// </value>
      String Description { get; }

      /// <summary>
      /// Gets the name of this validator.
      /// </summary>
      /// <value>
      /// The name of this validator.
      /// </value>
      String Name { get; }

      /// <summary>
      /// Gets the validation rules.
      /// </summary>
      /// <value>
      /// The validation rules.
      /// </value>
      IEnumerable<IValidationRule<TEntity>> Rules { get; }

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
      /// Evaluates the given entity against this defined rules.
      /// </summary>
      /// <param name="entity">
      /// The <typeparamref name="TEntity"/> instance to validate.
      /// </param>
      /// <returns>
      /// A non-null instance of <see cref="ValidationRuleResult"/>.
      /// </returns>
      IValidationRuleResult Validate(TEntity entity);
   }
}
