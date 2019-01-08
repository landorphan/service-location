namespace Landorphan.Common.Validation
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Default implementation of <see cref="IValidationMessage"/> and <see cref="IValidationMessageWriter"/>.
   /// </summary>
   [SuppressMessage("SonarLint.CodeSmell", "S1939: Inheritance list should not be redundant", Justification = "Being explicit (MWP)")]
   public sealed class ValidationMessage : IValidationMessage, IValidationMessageWriter
   {
      private readonly IEqualityComparer<String> _stringComparer;
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper;

      private Boolean _isError;
      private String _message = String.Empty;
      private String _messageType = String.Empty;

      /// <summary>
      /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
      /// </summary>
      public ValidationMessage() : this((IEqualityComparer<String>) null)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
      /// </summary>
      /// <param name="stringComparer"> The String comparer or <c> null </c> to use the default comparer. </param>
      public ValidationMessage(IEqualityComparer<String> stringComparer)
      {
         // EqualityComparer<String>.Default does not throw on null values unlike StringComparer implementations for GetHashCode.
         _stringComparer = stringComparer ?? EqualityComparer<String>.Default;
         _supportsReadOnlyHelper = new SupportsReadOnlyHelper();
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
      /// </summary>
      /// <param name="other">
      /// The instance to clone.
      /// </param>
      public ValidationMessage(IValidationMessage other)
      {
         other.ArgumentNotNull(nameof(other));

         _stringComparer = other.GetStringComparer();
         _supportsReadOnlyHelper = new SupportsReadOnlyHelper();
         IsError = other.IsError;
         Message = other.Message;
         MessageType = other.MessageType;
      }

      /// <inheritdoc/>
      public Object Clone()
      {
         return new ValidationMessage(this);
      }

      /// <inheritdoc/>
      public Boolean IsError
      {
         get => _isError;
         set => SetIsError(value);
      }

      /// <inheritdoc/>
      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      /// <inheritdoc/>
      public String Message
      {
         get => _message;
         set => SetMessage(value);
      }

      /// <inheritdoc/>
      public String MessageType
      {
         get => _messageType;
         set => SetMessageType(value);
      }

      /// <inheritdoc/>
      public Boolean Equals(IValidationMessage other)
      {
         if (ReferenceEquals(null, other))
         {
            return false;
         }

         return _isError.Equals(other.IsError) &&
                _stringComparer.Equals(_message, other.Message) &&
                _stringComparer.Equals(_messageType, other.MessageType);
      }

      /// <inheritdoc/>
      IEqualityComparer<String> IValidationMessage.GetStringComparer()
      {
         return _stringComparer;
      }

      /// <inheritdoc/>
      public void MakeReadOnly()
      {
         _supportsReadOnlyHelper.MakeReadOnly();
      }

      /// <inheritdoc/>
      public void SetIsError(Boolean isError)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         _isError = isError;
      }

      /// <inheritdoc/>
      public void SetMessage(String message)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         _message = message.TrimNullToEmpty();
      }

      /// <inheritdoc/>
      public void SetMessageType(String messageType)
      {
         _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();

         _messageType = messageType.TrimNullToEmpty();
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         return Equals(obj as ValidationMessage);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            var rv = _isError.GetHashCode();
            rv = (rv * 397) ^ _stringComparer.GetHashCode(_message);
            rv = (rv * 397) ^ _stringComparer.GetHashCode(_messageType);
            return rv;
         }
      }
   }
}