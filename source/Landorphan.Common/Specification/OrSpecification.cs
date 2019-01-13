namespace Landorphan.Common.Specification
{
   using System;

   /// <summary>
   /// Disjunctive composite <see cref="ISpecification{TEntity}"/>.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this specification applies.
   /// </typeparam>
   public sealed class OrSpecification<TEntity> : ISpecification<TEntity>
   {
      public OrSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
      {
         left.ArgumentNotNull(nameof(left));
         right.ArgumentNotNull(nameof(right));

         Left = left;
         Right = right;
      }

      public ISpecification<TEntity> Left { get; }

      public ISpecification<TEntity> Right { get; }

      /// <inheritdoc/>
      public Type GetEntityType()
      {
         return typeof(TEntity);
      }

      /// <inheritdoc/>
      public Boolean IsSatisfiedBy(TEntity entity)
      {
         return Left.IsSatisfiedBy(entity) || Right.IsSatisfiedBy(entity);
      }
   }
}