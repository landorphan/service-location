namespace Landorphan.Common.Decorators
{
   using System;

   /// <summary>
   /// An <see cref="long"/> rank decorator pattern implementation with value semantics.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of item decorated.
   /// </typeparam>
   public sealed class Int64RankDecorator<TEntity> : RankDecorator<TEntity, Int64> where TEntity : class
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Int64RankDecorator{TEntity}"/> class.
      /// </summary>
      /// <param name="value">
      /// The instance to decorate.
      /// </param>
      public Int64RankDecorator(TEntity value) : base(value, 0)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Int64RankDecorator{TEntity}"/> class.
      /// </summary>
      /// <param name="value">
      /// The instance to decorate.
      /// </param>
      /// <param name="rank">
      /// The rank value.
      /// </param>
      public Int64RankDecorator(TEntity value, Int64 rank) : base(value, rank)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Int64RankDecorator{TEntity}"/> class.
      /// </summary>
      /// <param name="other">
      /// The instance to clone.
      /// </param>
      public Int64RankDecorator(Int64RankDecorator<TEntity> other) : base(other)
      {
      }

      /// <inheritdoc/>
      public override Object Clone()
      {
         var rv = new Int64RankDecorator<TEntity>(this);
         return rv;
      }
   }
}