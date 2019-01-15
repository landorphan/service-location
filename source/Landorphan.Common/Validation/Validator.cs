namespace Landorphan.Common.Validation
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using Landorphan.Common.Threading;

   // ReSharper disable AssignNullToNotNullAttribute

   /// <summary>
   /// Default implementation of <see cref="IValidator{TEntity}"/> and <see cref="IValidatorWriter{TEntity}"/>.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this validator applies.
   /// </typeparam>
   [SuppressMessage("SonarLint.CodeSmell", "S1939: Inheritance list should not be redundant", Justification = "Being explicit (MWP)")]
   public sealed class Validator<TEntity> : DisposableObject, IValidator<TEntity>, IValidatorWriter<TEntity> where TEntity : class
   {
      private readonly IEqualityComparer<String> _stringComparer;
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper;

      private String _description;
      private NonRecursiveLock _lock;
      private String _name;

      private ImmutableHashSet<IValidationRule<TEntity>> _rules;

      /// <summary>
      /// Initializes a new instance of the <see cref="Validator{TEntity}"/> class.
      /// </summary>
      public Validator() : this((IEqualityComparer<String>)null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Validator{TEntity}"/> class.
      /// </summary>
      /// <param name="stringComparer"> The String comparer; <c> null </c> to use the default comparer. </param>
      public Validator(IEqualityComparer<String> stringComparer)
      {
         // EqualityComparer<String>.Default does not throw on null values unlike StringComparer implementations for GetHashCode.
         _stringComparer = stringComparer ?? EqualityComparer<String>.Default;
         _lock = new NonRecursiveLock();
         _supportsReadOnlyHelper = new SupportsReadOnlyHelper();

         _description = String.Empty;
         _name = String.Empty;
         _rules = ImmutableHashSet<IValidationRule<TEntity>>.Empty;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Validator{TEntity}"/> class.
      /// </summary>
      /// <param name="other">
      /// The instance to be cloned.
      /// </param>
      public Validator(IValidator<TEntity> other) : this()
      {
         other.ArgumentNotNull(nameof(other));

         var builder = ImmutableHashSet<IValidationRule<TEntity>>.Empty.ToBuilder();
         foreach (var rule in other.Rules)
         {
            builder.Add((IValidationRule<TEntity>)rule.Clone());
         }

         _rules = builder.ToImmutable();
         _description = other.Description.TrimNullToEmpty();
         _name = other.Name.TrimNullToEmpty();
      }

      /// <inheritdoc/>
      public Object Clone()
      {
         return new Validator<TEntity>(this);
      }

      /// <inheritdoc/>
      public String Description
      {
         get => _description;
         set => SetDescription(value);
      }

      /// <inheritdoc/>
      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public String Name
      {
         get => _name;
         set => SetName(value);
      }

      /// <inheritdoc/>
      public IEnumerable<IValidationRule<TEntity>> Rules => _rules;

      /// <inheritdoc/>
      public Boolean Equals(IValidator<TEntity> other)
      {
         if (ReferenceEquals(null, other))
         {
            return false;
         }

         return _stringComparer.Equals(Name, other.Name) &&
                _stringComparer.Equals(Description, other.Description) &&
                _rules.SetEquals(other.Rules);
      }

      /// <inheritdoc/>
      IEqualityComparer<String> IValidator<TEntity>.GetStringComparer()
      {
         return _stringComparer;
      }

      /// <inheritdoc/>
      public IValidationRuleResult Validate(TEntity entity)
      {
         var rv = new ValidationRuleResult(_stringComparer);
         foreach (var r in _rules)
         {
            var vrr = r.Validate(entity);
            foreach (var m in vrr.Messages)
            {
               rv.AddMessage(m);
            }
         }

         rv.MakeReadOnly();

         return rv;
      }

      /// <inheritdoc/>
      public Boolean AddValidationRule(IValidationRule<TEntity> rule)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         if (rule == null)
         {
            return false;
         }

         var was = _rules;
         _rules = _rules.Add(rule);
         var rv = !ReferenceEquals(was, _rules);
         return rv;
      }

      /// <inheritdoc/>
      public void ClearValidationRules()
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         _rules = _rules.Clear();
      }

      /// <inheritdoc/>
      public void MakeReadOnly()
      {
         if (_supportsReadOnlyHelper.IsReadOnly)
         {
            return;
         }

         _supportsReadOnlyHelper.MakeReadOnly();
         using (_lock.EnterWriteLock())
         {
            foreach (var rule in _rules)
            {
               var asConvertsToReadOnly = rule as IConvertsToReadOnly;
               asConvertsToReadOnly?.MakeReadOnly();
            }
         }

         _lock = null;
      }

      /// <inheritdoc/>
      public Boolean RemoveValidationRule(IValidationRule<TEntity> rule)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         if (rule == null)
         {
            return false;
         }

         var was = _rules;
         _rules = _rules.Remove(rule);
         var rv = !ReferenceEquals(was, _rules);
         return rv;
      }

      /// <inheritdoc/>
      public void SetDescription(String description)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         _description = description.TrimNullToEmpty();
      }

      /// <inheritdoc/>
      public void SetName(String name)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         _name = name.TrimNullToEmpty();
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         return Equals(obj as IValidator<TEntity>);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var rv = (Int32)_rules.LongCount();
            rv = (rv * 397) ^ _stringComparer.GetHashCode(_description);
            rv = (rv * 397) ^ _stringComparer.GetHashCode(_name);
            return rv;
         }
      }
   }
}
