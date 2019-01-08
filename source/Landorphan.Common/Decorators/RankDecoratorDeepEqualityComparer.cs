namespace Landorphan.Common.Decorators
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Represents a <see cref="RankDecorator{TEntity, TRank}"/> equality comparison that evaluates both the <see cref="RankDecorator{TEntity, TRank}.Rank"/>
   /// values, and the
   /// decorated values as needed.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of item decorated.
   /// </typeparam>
   /// <typeparam name="TRank">
   /// The type of rank.
   /// </typeparam>
   [Serializable]
   public sealed class RankDecoratorDeepEqualityComparer<TEntity, TRank> : IEqualityComparer<RankDecorator<TEntity, TRank>>
      where TEntity : class, IEquatable<TEntity>
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
         if (!ranksEqual)
         {
            return false;
         }

         if (x.Value.IsNull())
         {
            return y.Value.IsNull();
         }

         if (y.Value.IsNull())
         {
            return false;
         }

         return x.Value.Equals(y.Value);
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