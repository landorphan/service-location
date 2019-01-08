namespace Landorphan.Common.Validation
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;

   /// <summary>
   /// Base implementation of validation rules for a domain entity.
   /// </summary>
   /// <typeparam name="TEntity">
   /// The type of the entity to which this validation rule applies.
   /// </typeparam>
   [SuppressMessage("SonarLint.CodeSmell", "S1939: Inheritance list should not be redundant", Justification = "Being explicit (MWP)")]
   public abstract class ValidationRuleBase<TEntity> : IValidationRule<TEntity>, IValidationRuleWriter<TEntity>
   {
      private readonly IEqualityComparer<String> _stringComparer;
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper;

      private String _description;
      private String _name;
      private ImmutableHashSet<String> _propertyNames;

      /// <summary>
      /// Initializes a new instance of the <see cref="ValidationRuleBase{TEntity}"/> class.
      /// </summary>
      protected ValidationRuleBase() : this((IEqualityComparer<String>) null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ValidationRuleBase{TEntity}"/> class.
      /// </summary>
      /// <param name="stringComparer"> The String comparer or <c> null </c> to use the default comparer. </param>
      protected ValidationRuleBase(IEqualityComparer<String> stringComparer)
      {
         _stringComparer = stringComparer ?? EqualityComparer<String>.Default;
         _supportsReadOnlyHelper = new SupportsReadOnlyHelper();

         _description = String.Empty;
         _name = String.Empty;

         var builder = ImmutableHashSet<String>.Empty.ToBuilder();
         builder.KeyComparer = _stringComparer;
         _propertyNames = builder.ToImmutable();
      }

      /// <summary> Initializes a new instance of the <see cref="ValidationRuleBase{TEntity}"/> class. </summary>
      /// <param name="other">
      /// The instance to clone.
      /// </param>
      protected ValidationRuleBase(IValidationRule<TEntity> other) : this()
      {
         other.ArgumentNotNull(nameof(other));

         _stringComparer = other.GetStringComparer();
         _description = other.Description;
         _name = other.Name;

         var builder = ImmutableHashSet<String>.Empty.ToBuilder();
         builder.KeyComparer = _stringComparer;
         foreach (var p in other.PropertyNames)
         {
            builder.Add(p);
         }

         _propertyNames = builder.ToImmutable();
      }

      /// <inheritdoc/>
      public abstract Object Clone();

      /// <inheritdoc/>
      public String Description
      {
         get => _description;
         set => SetDescription(value);
      }

      /// <inheritdoc/>
      public Type EntityType => typeof(TEntity);

      /// <inheritdoc/>
      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public String Name
      {
         get => _name;
         set => SetName(value);
      }

      /// <inheritdoc/>
      public IEnumerable<String> PropertyNames => _propertyNames;

      /// <summary>  
      /// Gets string comparer. 
      /// </summary>
      protected IEqualityComparer<String> StringComparer => ((IValidationRule<TEntity>) this).GetStringComparer();

      /// <inheritdoc/>
      public virtual Boolean Equals(IValidationRule<TEntity> other)
      {
         if (ReferenceEquals(other, null))
         {
            return false;
         }

         // transform the other's property names to the convention employed by this implementation.
         var otherPropertyNames = new List<String>();
         if (other.PropertyNames != null)
         {
            otherPropertyNames = (from pn in other.PropertyNames select pn.TrimNullToEmpty()).ToList();
         }

         return _stringComparer.Equals(_description, other.Description.TrimNullToEmpty()) &&
                _propertyNames.SetEquals(otherPropertyNames) &&
                _stringComparer.Equals(_name, other.Name.TrimNullToEmpty());
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S4039:Interface methods should be callable by derived types", Justification = "Reviewed (MWP)")]
      [SuppressMessage("Microsoft.Design" ,"CA1033: Interface methods should be callable by child types", Justification = "Exposed via the StringCompararer property")]
      IEqualityComparer<String> IValidationRule<TEntity>.GetStringComparer()
      {
         return _stringComparer;
      }

      /// <inheritdoc/>
      public abstract IValidationRuleResult Validate(TEntity entity);

      /// <inheritdoc/>
      public Boolean AddPropertyName(String propertyName)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         if (ReferenceEquals(propertyName, null))
         {
            return false;
         }

         var cleanedPropertyName = propertyName.TrimNullToEmpty();
         var was = _propertyNames;
         _propertyNames = _propertyNames.Add(cleanedPropertyName);
         var rv = !ReferenceEquals(was, _propertyNames);
         return rv;
      }

      /// <inheritdoc/>
      public void ClearPropertyNames()
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         _propertyNames = _propertyNames.Clear();
      }

      /// <inheritdoc/>
      public void MakeReadOnly()
      {
         _supportsReadOnlyHelper.MakeReadOnly();
      }

      /// <inheritdoc/>
      public Boolean RemovePropertyName(String propertyName)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         if (ReferenceEquals(propertyName, null))
         {
            return false;
         }

         var cleanedPropertyName = propertyName.TrimNullToEmpty();
         var was = _propertyNames;
         _propertyNames = _propertyNames.Remove(cleanedPropertyName);
         var rv = !ReferenceEquals(was, _propertyNames);
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
         return Equals(obj as IValidationRule<TEntity>);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var rv = EntityType.GetHashCode();
            rv = (rv * 397) ^ _stringComparer.GetHashCode(_name);
            return rv;
         }
      }

      /// <summary>
      /// Builds validation rule result with property names taken from this instance as well as the rule name and description.
      /// </summary>
      /// <returns>
      /// An initialized <see cref="IValidationRuleResultWriter"/>.
      /// </returns>
      protected IValidationRuleResultWriter BuildValidationResult(TEntity evaluatedEntity)
      {
         var rv = new ValidationRuleResult
            {ValidationRuleDescription = Description, EvaluatedEntity = evaluatedEntity, ValidationRuleName = Name};

         return rv;
      }
   }
}