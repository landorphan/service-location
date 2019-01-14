namespace Landorphan.Common.Specification
{
   using System;
   using System.Linq.Expressions;

   /// <summary>
   /// A implementation of <see cref="ISpecification{TEntity}"/> that uses an injected expression or delegate to evaluate entities.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this specification applies.
   /// </typeparam>
   public class DelegateSpecification<TEntity> : ISpecification<TEntity>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="DelegateSpecification{TEntity}"/> class.
      /// </summary>
      /// <param name="predicate"> The predicate. </param>
      public DelegateSpecification(Func<TEntity, Boolean> predicate)
      {
         predicate.ArgumentNotNull(nameof(predicate));
         Delegate = predicate;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="DelegateSpecification{TEntity}"/> class.
      /// </summary>
      /// <param name="predicate"> The predicate. </param>
      /// <remarks> Support for expression trees. </remarks>
      public DelegateSpecification(Expression<Func<TEntity, Boolean>> predicate)
      {
         predicate.ArgumentNotNull(nameof(predicate));
         Delegate = predicate.Compile();
      }

      internal Func<TEntity, Boolean> Delegate { get; }

      /// <inheritdoc/>
      public Type GetEntityType()
      {
         return typeof(TEntity);
      }

      /// <inheritdoc/>
      public Boolean IsSatisfiedBy(TEntity entity)
      {
         return Delegate.Invoke(entity);
      }
   }
}
