namespace Landorphan.Common.Specification
{
   using System;

   /// <summary>
   /// Conjunctive composite <see cref="ISpecification{TEntity}"/>.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this specification applies.
   /// </typeparam>
   public sealed class AndSpecification<TEntity> : ISpecification<TEntity>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="AndSpecification{TEntity}"/> class.
      /// </summary>
      /// <param name="left"> The left specification. </param>
      /// <param name="right"> The right specification. </param>
      public AndSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
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
         return Left.IsSatisfiedBy(entity) && Right.IsSatisfiedBy(entity);
      }
   }
}
