namespace Landorphan.Common.Decorators
{
   using System;

   /// <summary>
   /// An <see cref="int"/> rank decorator pattern implementation with value semantics.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of item decorated.
   /// </typeparam>
   public sealed class Int32RankDecorator<TEntity> : RankDecorator<TEntity, Int32> where TEntity : class // exclude value types
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Int32RankDecorator{TEntity}"/> class.
      /// </summary>
      /// <param name="value">
      /// The instance to decorate.
      /// </param>
      public Int32RankDecorator(TEntity value) : base(value, 0)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Int32RankDecorator{TEntity}"/> class.
      /// </summary>
      /// <param name="value">
      /// The instance to decorate.
      /// </param>
      /// <param name="rank">
      /// The rank value.
      /// </param>
      public Int32RankDecorator(TEntity value, Int32 rank) : base(value, rank)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Int32RankDecorator{TEntity}"/> class.
      /// </summary>
      /// <param name="other">
      /// The instance to clone.
      /// </param>
      public Int32RankDecorator(Int32RankDecorator<TEntity> other) : base(other)
      {
      }

      /// <inheritdoc/>
      public override Object Clone()
      {
         var rv = new Int32RankDecorator<TEntity>(this);
         return rv;
      }
   }
}