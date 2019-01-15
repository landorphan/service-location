namespace Landorphan.Common.Validation
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Represents a readable line-item entry in a <see cref="IValidationRuleResult"/> value.
   /// </summary>
   public interface IValidationMessage : IEquatable<IValidationMessage>, ICloneable, IQueryReadOnly
   {
      /// <summary>
      /// Gets a value indicating whether this instance represents an error.
      /// </summary>
      /// <value>
      /// <c> true </c> if this instance represents an error; otherwise, <c> false </c>.
      /// </value>
      Boolean IsError { get; }

      /// <summary>
      /// Gets the message.
      /// </summary>
      /// <value>
      /// The message.
      /// </value>
      String Message { get; }

      /// <summary>
      /// Gets the message type.
      /// </summary>
      /// <value>
      /// The message type.
      /// </value>
      String MessageType { get; }

      /// <summary>  
      /// Gets string comparer. 
      /// </summary>
      /// <returns>
      /// The string comparer. 
      /// </returns>
      /// <remarks>
      /// This is used primarily for cloning.
      /// </remarks>
      IEqualityComparer<String> GetStringComparer();
   }
}
