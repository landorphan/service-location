namespace Landorphan.Common.Decorators
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Represents a <see cref="RankDecorator{TEntity, TRank}"/> comparison that evaluates only the <see cref="RankDecorator{TEntity, TRank}.Rank"/> values.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of item decorated.
   /// </typeparam>
   /// <typeparam name="TRank">
   /// The type of rank.
   /// </typeparam>
   [Serializable]
   public sealed class RankDecoratorShallowComparer<TEntity, TRank> : IComparer<RankDecorator<TEntity, TRank>>
      where TEntity : class // exclude value types
      where TRank : IComparable<TRank>, IEquatable<TRank>, new()
   {
      /// <summary>
      /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
      /// </summary>
      /// <param name="x">
      /// The first Object to compare.
      /// </param>
      /// <param name="y">
      /// The second Object to compare.
      /// </param>
      /// <returns>
      /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table:
      /// <list type="table">
      ///    <listheader>
      ///       <term> Value </term> <description> Meaning </description>
      ///    </listheader>
      ///    <item>
      ///       <term> Less than zero </term> <description> <paramref name="x"/> is less than <paramref name="y"/>. </description>
      ///    </item>
      ///    <item>
      ///       <term> Zero </term><description> <paramref name="x"/> equals <paramref name="y"/>. </description>
      ///    </item>
      ///    <item>
      ///       <term> Greater than zero </term> <description> <paramref name="x"/> is greater than <paramref name="y"/>. </description>
      ///    </item>
      /// </list>
      /// </returns>
      /// <remarks>
      /// Compares by <see cref="RankDecorator{TEntity, TRank}.Rank"/> values only.
      /// </remarks>
      public Int32 Compare(RankDecorator<TEntity, TRank> x, RankDecorator<TEntity, TRank> y)
      {
         if (x.IsNull())
         {
            return y.IsNull() ? 0 : -1;
         }

         if (y.IsNull())
         {
            return 1;
         }

         var rv = Comparer<TRank>.Default.Compare(x.Rank, y.Rank);
         return rv;
      }
   }
}