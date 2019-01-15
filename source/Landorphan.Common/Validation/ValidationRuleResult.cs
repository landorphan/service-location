namespace Landorphan.Common.Validation
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Linq;
   using System.Runtime.Serialization;
   using Landorphan.Common.Threading;

   // ReSharper disable AssignNullToNotNullAttribute

   /// <summary>
   /// Contains a single result from a call to <see cref="IValidationRule{TEntity}.Validate"/>; a mash-up of a human-readable rule name, a
   /// human-readable rule description, and a set of line item messages.
   /// </summary>
   /// <remarks>
   /// Default implementation of <see cref="IValidationRuleResult"/> and <see cref="IValidationRuleResultWriter"/>.
   /// </remarks>
   [SuppressMessage("SonarLint.CodeSmell", "S1939: Inheritance list should not be redundant", Justification = "Being explicit (MWP)")]
   public sealed class ValidationRuleResult : DisposableObject, IValidationRuleResult, IValidationRuleResultWriter
   {
      private readonly NonRecursiveLock _lock;
      private readonly IEqualityComparer<String> _stringComparer;
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper;

      [NonSerialized]
      private Object _evaluatedEntity;

      [NonSerialized]
      private ImmutableHashSet<IValidationMessage> _messages;

      private IValidationMessage[] _serializedMessages;
      private String _validationRuleDescription;
      private String _validationRuleName;

      /// <summary>
      /// Initializes a new instance of the <see cref="ValidationRuleResult"/> class.
      /// </summary>
      public ValidationRuleResult() : this((IEqualityComparer<String>)null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ValidationRuleResult"/> class.
      /// </summary>
      /// <param name="stringComparer"> The String comparer; <c> null </c> to use the default comparer. </param>
      public ValidationRuleResult(IEqualityComparer<String> stringComparer)
      {
         // EqualityComparer<String>.Default does not throw on null values unlike StringComparer implementations for GetHashCode.
         _stringComparer = stringComparer ?? EqualityComparer<String>.Default;

         _lock = new NonRecursiveLock();
         _supportsReadOnlyHelper = new SupportsReadOnlyHelper();
         _messages = ImmutableHashSet<IValidationMessage>.Empty;
         _validationRuleDescription = String.Empty;
         _validationRuleName = String.Empty;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ValidationRuleResult"/> class.
      /// </summary>
      /// <param name="other">
      /// The instance to clone.
      /// </param>
      public ValidationRuleResult(IValidationRuleResult other) : this()
      {
         other.ArgumentNotNull(nameof(other));

         // clone the messages.
         foreach (var msg in other.Messages)
         {
            _messages = _messages.Add(new ValidationMessage(msg));
         }

         _evaluatedEntity = other.EvaluatedEntity is ICloneable cloneable ? cloneable.Clone() : other.EvaluatedEntity;

         _validationRuleDescription = other.ValidationRuleDescription;
         _validationRuleName = other.ValidationRuleName;
      }

      /// <inheritdoc/>
      public Object Clone()
      {
         return new ValidationRuleResult(this);
      }

      /// <inheritdoc/>
      public Object EvaluatedEntity
      {
         get => _evaluatedEntity;
         set => SetEvaluatedEntity(value);
      }

      /// <inheritdoc/>
      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public IEnumerable<IValidationMessage> Messages => _messages;

      /// <inheritdoc/>
      public String ValidationRuleDescription
      {
         get => _validationRuleDescription;
         set => SetValidationRuleDescription(value);
      }

      /// <inheritdoc/>
      public String ValidationRuleName
      {
         get => _validationRuleName;
         set => SetValidationRuleName(value);
      }

      /// <inheritdoc/>
      public Boolean Equals(IValidationRuleResult other)
      {
         if (ReferenceEquals(other, null))
         {
            return false;
         }

         using (_lock.EnterReadLock())
         {
            return _stringComparer.Equals(_validationRuleDescription, other.ValidationRuleDescription.TrimNullToEmpty()) &&
                   _stringComparer.Equals(_validationRuleName, other.ValidationRuleName.TrimNullToEmpty()) &&
                   _messages.SetEquals(other.Messages);
         }
      }

      /// <inheritdoc/>
      public Boolean GetHasError()
      {
         using (_lock.EnterReadLock())
         {
            var rv = (from m in _messages where m.IsError select m).Any();
            return rv;
         }
      }

      /// <inheritdoc/>
      public Boolean AddMessage(IValidationMessage message)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         if (message == null)
         {
            return false;
         }

         var was = _messages;
         _messages = _messages.Add(message);
         var rv = !ReferenceEquals(was, _messages);
         return rv;
      }

      /// <inheritdoc/>
      public void ClearMessages()
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         _messages = _messages.Clear();
      }

      /// <inheritdoc/>
      public void MakeReadOnly()
      {
         _supportsReadOnlyHelper.MakeReadOnly();
         using (_lock.EnterWriteLock())
         {
            foreach (var m in _messages)
            {
               var convertible = m as IConvertsToReadOnly;
               convertible?.MakeReadOnly();
            }
         }
      }

      /// <inheritdoc/>
      public Boolean RemoveMessage(IValidationMessage message)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
         if (message == null)
         {
            return false;
         }

         var was = _messages;
         _messages = _messages.Remove(message);
         var rv = !ReferenceEquals(was, _messages);
         return rv;
      }

      /// <inheritdoc/>
      public void SetEvaluatedEntity(Object evaluatedEntity)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         _evaluatedEntity = evaluatedEntity;
      }

      /// <inheritdoc/>
      public void SetValidationRuleDescription(String validationRuleDescription)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         _validationRuleDescription = validationRuleDescription.TrimNullToEmpty();
      }

      /// <inheritdoc/>
      public void SetValidationRuleName(String validationRuleName)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         _validationRuleName = validationRuleName.TrimNullToEmpty();
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         return Equals(obj as IValidationRuleResult);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var rv = _evaluatedEntity == null ? 0 : _evaluatedEntity.GetHashCode();
            rv = (rv * 397) ^ _stringComparer.GetHashCode(_validationRuleName);
            return rv;
         }
      }

      [OnDeserialized]
      private void OnDeserialized(StreamingContext context)
      {
         var builder = ImmutableHashSet<IValidationMessage>.Empty.ToBuilder();
         foreach (var msg in _serializedMessages)
         {
            builder.Add(msg);
         }

         _messages = builder.ToImmutable();
         _serializedMessages = null;
      }

      [OnSerializing]
      private void OnSerializing(StreamingContext context)
      {
         using (_lock.EnterReadLock())
         {
            _serializedMessages = _messages.ToArray();
         }
      }
   }
}
