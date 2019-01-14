namespace Landorphan.Common.Decorators
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Represents a <see cref="RankDecorator{TEntity, TRank}"/> equality comparison that evaluates only the <see cref="RankDecorator{TEntity, TRank}.Rank"/>
   /// values.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of item decorated.
   /// </typeparam>
   /// <typeparam name="TRank">
   /// The type of rank.
   /// </typeparam>
   [Serializable]
   public sealed class RankDecoratorShallowEqualityComparer<TEntity, TRank> : IEqualityComparer<RankDecorator<TEntity, TRank>>
      where TEntity : class
      where TRank : IComparable<TRank>, IEquatable<TRank>, new()
   {
      /// <inheritdoc/>
      public Boolean Equals(RankDecorator<TEntity, TRank> x, RankDecorator<TEntity, TRank> y)
      {
         if (x.IsNull())
         {
            return y.IsNull();
         }

         if (y.IsNull())
         {
            return false;
         }

         var ranksEqual = EqualityComparer<TRank>.Default.Equals(x.Rank, y.Rank);
         return ranksEqual;
      }

      /// <inheritdoc/>
      public Int32 GetHashCode(RankDecorator<TEntity, TRank> obj)
      {
         if (obj.IsNull())
         {
            return 0;
         }

         return obj.GetHashCode();
      }
   }
}
