namespace Landorphan.Common.Specification
{
   using System;

   /// <summary>
   /// Represents a set of criteria.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this specification applies.
   /// </typeparam>
   public interface ISpecification<in TEntity>
   {
      /// <summary>
      /// Gets the entity type.
      /// </summary>
      /// <returns>
      /// The type of <typeparamref name="TEntity"/>.
      /// </returns>
      Type GetEntityType();

      /// <summary>
      /// Determines whether or not the <paramref name="entity"/> satisfies these criteria.
      /// </summary>
      /// <param name="entity">
      /// The entity to inspect.
      /// </param>
      /// <returns>
      /// <c> true </c> when <paramref name="entity"/> satisfies these criteria; otherwise, <c> false </c>.
      /// </returns>
      /// <remarks>
      /// Treatment of <c> null </c> is implementation defined.
      /// </remarks>
      Boolean IsSatisfiedBy(TEntity entity);
   }
}