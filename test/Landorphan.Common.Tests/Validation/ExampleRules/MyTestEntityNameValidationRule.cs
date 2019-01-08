namespace Landorphan.Common.Tests.Validation.ExampleRules
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Text.RegularExpressions;
   using Landorphan.Common.Validation;

   internal class MyTestEntityNameValidationRule : ValidationRuleBase<MyTestEntity>
   {
      public MyTestEntityNameValidationRule() : this((IEqualityComparer<String>) null)
      {
      }

      public MyTestEntityNameValidationRule(IEqualityComparer<String> stringComparer) : base(stringComparer)
      {
         Name = "Names must be human-readable";
         Description =
            "Requires the Name property not to be null, nor have leading or trailing white-space, and contain at least one alphabetical character.";

         AddPropertyName("Name");

         MakeReadOnly();
      }

      public MyTestEntityNameValidationRule(MyTestEntityNameValidationRule other) : base(other)
      {
         // nothing else needed (no state declared in this class).
      }

      public override Object Clone()
      {
         return new MyTestEntityNameValidationRule(this);
      }

      [SuppressMessage(
         "SonarLint:CodeSmell",
         "S134: Control flow statements if, switch, for, foreach, while, do and try should not be nested too deeply",
         Justification = "Acknowledged, but 3 is too tight (MWP)")]
      public override IValidationRuleResult Validate(MyTestEntity entity)
      {
         var writer = BuildValidationResult(entity);
         var messageFactory = new ValidationMessageFactory();

         if (entity != null)
         {
            if (entity.Name != null)
            {
               if (StringComparer.Equals(entity.Name, entity.Name.Trim()))
               {
                  // has a non-digit word character
                  var isValid = Regex.IsMatch(entity.Name, @"\w", RegexOptions.IgnoreCase) &&
                                Regex.IsMatch(entity.Name, @"\D", RegexOptions.IgnoreCase);
                  if (isValid)
                  {
                     messageFactory.CreateInformationMessage("Passed:  Name is well-formed.");
                  }
                  else
                  {
                     messageFactory.CreateErrorMessage("Failed:  Name is not well-formed.");
                  }
               }
               else
               {
                  messageFactory.CreateErrorMessage("Failed:  Name is has leading and/or trailing whitespace.");
               }
            }
            else
            {
               messageFactory.CreateErrorMessage("Failed:  Name is null.");
            }
         }

         writer.MakeReadOnly();
         return writer;
      }
   }
}