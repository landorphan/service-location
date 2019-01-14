namespace Landorphan.Common.Specification
{
   using System;

   /// <summary>
   /// Exclusive disjunction composite <see cref="ISpecification{TEntity}"/>.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this specification applies.
   /// </typeparam>
   public sealed class XorSpecification<TEntity> : ISpecification<TEntity>
   {
      internal XorSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
      {
         left.ArgumentNotNull(nameof(left));
         right.ArgumentNotNull(nameof(right));

         Left = left;
         Right = right;
      }

      internal ISpecification<TEntity> Left { get; }

      internal ISpecification<TEntity> Right { get; }

      /// <inheritdoc/>
      public Type GetEntityType()
      {
         return typeof(TEntity);
      }

      /// <inheritdoc/>
      public Boolean IsSatisfiedBy(TEntity entity)
      {
         return Left.IsSatisfiedBy(entity) ^ Right.IsSatisfiedBy(entity);
      }
   }
}
