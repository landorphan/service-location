namespace Landorphan.Common.Decorators
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.Common.Threading;

   /// <summary>
   /// A rank decorator pattern implementation with value semantics.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of item decorated.
   /// </typeparam>
   /// <typeparam name="TRank">
   /// The type of rank.
   /// </typeparam>
   [SuppressMessage("SonarLint.CodeSmell", "S1939: Inheritance list should not be redundant", Justification = "Being explicit (MWP)")]
   [SuppressMessage("SonarLint.CodeSmell", "S4035: Classes implementing IEquatable<T> should be sealed", Justification = "This class is responsible for checking it's fields")]
   public class RankDecorator<TEntity, TRank>
      : DisposableObject, ICloneable, IConvertsToReadOnly, IQueryReadOnly, IQueryThreadSafe, IEquatable<RankDecorator<TEntity, TRank>>
      where TEntity : class // exclude value types.
      where TRank : IComparable<TRank>, IEquatable<TRank>, new()
   {
      private readonly NonRecursiveLock _lock;
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper;
      private TRank _rank;

      /// <summary>
      /// Initializes a new instance of the <see cref="RankDecorator{TEntity, TRank}"/> class.
      /// </summary>
      /// <param name="value">
      /// The instance to decorate.
      /// </param>
      public RankDecorator(TEntity value) : this(value, new TRank())
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="RankDecorator{TEntity, TRank}"/> class.
      /// </summary>
      /// <param name="value">
      /// The instance to decorate.
      /// </param>
      /// <param name="rank">
      /// The rank value.
      /// </param>
      public RankDecorator(TEntity value, TRank rank) : this()
      {
         _rank = rank;
         Value = value;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="RankDecorator{TEntity, TRank}"/> class.
      /// </summary>
      /// <param name="other">
      /// The instance to clone.
      /// </param>
      public RankDecorator(RankDecorator<TEntity, TRank> other) : this()
      {
         other.ArgumentNotNull(nameof(other));

         _rank = !(other._rank is ICloneable rankAsCloneable) ? other._rank : (TRank)rankAsCloneable.Clone();

         // shallow- or deep-copy based on implementation of ICloneable.
         Value = !(other.Value is ICloneable valueAsCloneable) ? other.Value : (TEntity)valueAsCloneable.Clone();
      }

      /// <summary>
      /// Prevents a default instance of the <see cref="RankDecorator{TEntity, TRank}"/> class from being created.
      /// </summary>
      private RankDecorator()
      {
         _lock = new NonRecursiveLock();
         _supportsReadOnlyHelper = new SupportsReadOnlyHelper();
      }

      /// <inheritdoc/>
      public virtual Object Clone()
      {
         // .ctor enters a read lock
         var rv = new RankDecorator<TEntity, TRank>(this);
         return rv;
      }

      /// <inheritdoc/>
      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public Boolean IsThreadSafe => true;

      /// <summary>
      /// Gets or sets the rank.
      /// </summary>
      /// <value>
      /// The rank.
      /// </value>
      public TRank Rank
      {
         get
         {
            using (_lock.EnterReadLock())
            {
               return _rank;
            }
         }
         set
         {
            using (_lock.EnterWriteLock())
            {
               _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
               _rank = value;
            }
         }
      }

      /// <summary>
      /// Gets the decorated value.
      /// </summary>
      /// <value>
      /// The value.
      /// </value>
      public TEntity Value { get; }

      /// <inheritdoc/>
      public void MakeReadOnly()
      {
         _supportsReadOnlyHelper.MakeReadOnly();

         var valueAsIConvertsToReadonly = Value as IConvertsToReadOnly;
         if (!valueAsIConvertsToReadonly.IsNull())
         {
            // ReSharper disable once PossibleNullReferenceException
            valueAsIConvertsToReadonly.MakeReadOnly();
         }
      }

      /// <inheritdoc/>
      public Boolean Equals(RankDecorator<TEntity, TRank> other)
      {
         if (other.IsNull())
         {
            return false;
         }

         using (_lock.EnterReadLock())
         {
            var ranksEqual = EqualityComparer<TRank>.Default.Equals(_rank, other._rank);
            if (!ranksEqual)
            {
               return false;
            }

            Boolean rv;
            if (Value == null)
            {
               rv = other.Value == null;
            }
            else
            {
               rv = !(Value is IEquatable<TEntity> valueAsIEquatable) ? Value.Equals(other.Value) : valueAsIEquatable.Equals(other.Value);
            }

            return rv;
         }
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         return Equals(obj as RankDecorator<TEntity, TRank>);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         return Rank.GetHashCode();
      }
   }
}
