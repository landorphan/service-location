namespace Landorphan.Common.Specification
{
   using System;

   /// <summary>
   /// Negation composite <see cref="ISpecification{TEntity}"/>.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this specification applies.
   /// </typeparam>
   public sealed class NotSpecification<TEntity> : ISpecification<TEntity>
   {
      internal NotSpecification(ISpecification<TEntity> specification)
      {
         specification.ArgumentNotNull(nameof(specification));

         Negated = specification;
      }

      internal ISpecification<TEntity> Negated { get; }

      /// <inheritdoc/>
      public Type GetEntityType()
      {
         return typeof(TEntity);
      }

      /// <inheritdoc/>
      public Boolean IsSatisfiedBy(TEntity entity)
      {
         return !Negated.IsSatisfiedBy(entity);
      }
   }
}
