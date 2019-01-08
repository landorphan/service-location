namespace Landorphan.Common.Validation
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Represents a single readable result from a a call to <see cref="IValidationRule{TEntity}.Validate"/> in it's entirety; 
   /// a mash-up of a human-readable rule, and the resultant human readable messages.
   /// </summary>
   public interface IValidationRuleResult : IEquatable<IValidationRuleResult>, ICloneable, IQueryReadOnly
   {
      /// <summary>
      /// Gets the evaluated entity.
      /// </summary>
      /// <value>
      /// The evaluated entity.
      /// </value>
      Object EvaluatedEntity { get; }

      /// <summary>  
      /// Gets all of the resultant validation messages. 
      /// </summary>
      /// <value>
      /// A non-null collection of <see cref="IValidationMessage"/> containing zero or more messages. 
      /// </value>
      IEnumerable<IValidationMessage> Messages { get; }

      /// <summary>  
      /// The description of the validation rule applied that gave rise to this instance.
      /// </summary>
      /// <value> 
      /// The description of validation rule applied.
      /// </value>
      String ValidationRuleDescription { get; }

      /// <summary>  
      /// The name of the validation rule applied that gave rise to this instance.
      /// </summary>
      /// <value> 
      /// The name of validation rule applied. 
      /// </value>
      String ValidationRuleName { get; }

      /// <summary>  
      /// Gets a value indicating whether this instance contains a message that represents an error condition.
      /// </summary>
      Boolean GetHasError();
   }
}