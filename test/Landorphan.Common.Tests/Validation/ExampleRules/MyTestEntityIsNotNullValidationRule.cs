namespace Landorphan.Common.Tests.Validation.ExampleRules
{
   using System;
   using System.Collections.Generic;
   using Landorphan.Common.Validation;

   internal sealed class MyTestEntityIsNotNullValidationRule : ValidationRuleBase<MyTestEntity>
   {
      public MyTestEntityIsNotNullValidationRule() : this((IEqualityComparer<String>)null)
      {
      }

      public MyTestEntityIsNotNullValidationRule(IEqualityComparer<String> stringComparer) : base(stringComparer)
      {
         Description = "Requires the instance not to be null.";
         Name = "MyEntity instance must not be null";
      }

      public MyTestEntityIsNotNullValidationRule(MyTestEntityIsNotNullValidationRule other) : base(other)
      {
         // nothing else needed (no state declared in this class).
      }

      public override Object Clone()
      {
         return new MyTestEntityIsNotNullValidationRule(this);
      }

      public override IValidationRuleResult Validate(MyTestEntity entity)
      {
         var writer = BuildValidationResult(entity);
         var messageFactory = new ValidationMessageFactory();

         writer.AddMessage(
            entity != null
               ? messageFactory.CreateInformationMessage("Passed:  instance is not null.")
               : messageFactory.CreateErrorMessage("Failed:  instance must not be null."));

         writer.MakeReadOnly();
         return writer;
      }
   }
}
