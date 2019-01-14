namespace Landorphan.Common.Tests.Validation.ExampleRules
{
   using System;
   using System.Collections.Generic;
   using Landorphan.Common.Validation;

   internal class MyTestEntityValuePositiveRule : ValidationRuleBase<MyTestEntity>
   {
      public MyTestEntityValuePositiveRule() : this((IEqualityComparer<String>)null)
      {
      }

      public MyTestEntityValuePositiveRule(IEqualityComparer<String> stringComparer) : base(stringComparer)
      {
         Name = "Value must be positive";
         Description = "Requires the Value property be greater than zero.";

         AddPropertyName("Value");

         MakeReadOnly();
      }

      public MyTestEntityValuePositiveRule(MyTestEntityValuePositiveRule other) : base(other)
      {
         // nothing else needed (no state declared in this class).
      }

      public override Object Clone()
      {
         return new MyTestEntityValuePositiveRule(this);
      }

      public override IValidationRuleResult Validate(MyTestEntity entity)
      {
         var writer = BuildValidationResult(entity);
         var messageFactory = new ValidationMessageFactory();

         if (entity != null)
         {
            if (entity.Value > 0)
            {
               var msg = messageFactory.CreateErrorMessage("Passed:  Value is greater than zero.");
               writer.AddMessage(msg);
            }
            else
            {
               var msg = messageFactory.CreateErrorMessage("Failed:  Value must be greater than zero.");
               writer.AddMessage(msg);
            }
         }
         else
         {
            // entity is null
            var msg = messageFactory.CreateErrorMessage("Entity must not be null.");
            writer.AddMessage(msg);
         }

         writer.MakeReadOnly();
         return writer;
      }
   }
}
