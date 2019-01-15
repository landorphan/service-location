namespace Landorphan.Common.Specification
{
   /// <summary>
   /// Extension methods for <see cref="ISpecification{T}"/>.
   /// </summary>
   public static class SpecificationExtensions
   {
      /// <summary>
      /// Instantiates a <see cref="ISpecification{T}"/> that performs logical conjunction of the two <see cref="ISpecification{T}"/> instances.
      /// </summary>
      /// <typeparam name="TEntity">
      /// The specific type of <see cref="ISpecification{T}"/>.
      /// </typeparam>
      /// <param name="left">
      /// The left <see cref="ISpecification{T}"/>.
      /// </param>
      /// <param name="right">
      /// The right <see cref="ISpecification{T}"/>.
      /// </param>
      /// <returns>
      /// A new <see cref="ISpecification{T}"/> instance that is satisfied when both given specifications are satisfied.
      /// </returns>
      public static ISpecification<TEntity> And<TEntity>(this ISpecification<TEntity> left, ISpecification<TEntity> right)
      {
         return new AndSpecification<TEntity>(left, right);
      }

      /// <summary>
      /// Performs logical negation of an <see cref="ISpecification{T}"/> instance.
      /// </summary>
      /// <typeparam name="TEntity">
      /// The specific type of <see cref="ISpecification{T}"/>.
      /// </typeparam>
      /// <param name="specification">
      /// The specification.
      /// </param>
      /// <returns>
      /// A new <see cref="ISpecification{T}"/> instance that is satisfied when the given specifications is not satisfied.
      /// </returns>
      public static ISpecification<TEntity> Not<TEntity>(this ISpecification<TEntity> specification)
      {
         return new NotSpecification<TEntity>(specification);
      }

      /// <summary>
      /// Instantiates a <see cref="ISpecification{T}"/> that performs logical disjunction of the two <see cref="ISpecification{T}"/> instances.
      /// </summary>
      /// <typeparam name="TEntity">
      /// The specific type of <see cref="ISpecification{T}"/>.
      /// </typeparam>
      /// <param name="left">
      /// The left <see cref="ISpecification{T}"/>.
      /// </param>
      /// <param name="right">
      /// The right <see cref="ISpecification{T}"/>.
      /// </param>
      /// <returns>
      /// A new <see cref="ISpecification{T}"/> instance that is satisfied when either or both given specifications are satisfied.
      /// </returns>
      public static ISpecification<TEntity> Or<TEntity>(this ISpecification<TEntity> left, ISpecification<TEntity> right)
      {
         return new OrSpecification<TEntity>(left, right);
      }

      /// <summary>
      /// Instantiates a <see cref="ISpecification{T}"/> that performs logical exclusive disjunction of the two <see cref="ISpecification{T}"/> instances.
      /// </summary>
      /// <typeparam name="TEntity">
      /// The type of the entity.
      /// </typeparam>
      /// <param name="left">
      /// The left <see cref="ISpecification{T}"/>.
      /// </param>
      /// <param name="right">
      /// The right <see cref="ISpecification{T}"/>.
      /// </param>
      /// <returns>
      /// A new <see cref="ISpecification{T}"/> instance that is satisfied when either but not both given specifications are satisfied.
      /// </returns>
      public static ISpecification<TEntity> Xor<TEntity>(this ISpecification<TEntity> left, ISpecification<TEntity> right)
      {
         return new XorSpecification<TEntity>(left, right);
      }
   }
}
