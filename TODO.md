NOTE:  static operator overloads are not reordered when R# file cleanup with reordering is called.  Still True ? 2018.12.29

NOTE:  lost GhostDoc rule templates.  The Default templates do not put opening xml comment tags and closing xml comment tags on their 
own line.  Edit the rules to recreate this behavior.

To Enable code analysis in .Net Core:
   install Nuget Microsoft.NetCore.Analyzers
Create a ruleset (ape the existing implementation).
   Create a file named "ProjectName.RuleSet" next to the csproj file
   Include it in the project
   Set the build action to C# analyzer additional file
   Edit the CSPROJ file adding "<CodeAnalysisRuleSet>Project.Name.ruleset</CodeAnalysisRuleSet>"
   In .Net Core projects, there is no need to add <RunCodeAnalysis>true</RunCodeAnalysis>, it has no effect.



Something is stripping empty lines from line just before EOF.



GOTCHA:  takeaway use /// <inheritdoc/> above any [SuppressMessage] attributes

SYNTAX UNFLAGGED:
      /// <inheritdoc/>
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S4018: Generic methods should provide type parameters",
         Justification = "This generic method delegates implementation to the non-generic version.  I want one implementation (MWP)")]

SYNTAX FLAGGED:
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S4018: Generic methods should provide type parameters",
         Justification = "This generic method delegates implementation to the non-generic version.  I want one implementation (MWP)")]
      /// <inheritdoc/>


TODO:  IocServiceLocator and IocContainer:  too much duplication interacting with _registrations.



      /// <summary>
      /// Determines whether [is valid starting standard text] [the specified holdem starting standards cards text].
      /// </summary>
      /// <param name="holdemStartingStandardsCardsText">The holdem starting standards cards text.</param>
      /// <returns>Boolean.</returns>
      Boolean IsValidStartingStandardText(String holdemStartingStandardsCardsText);

Write reusable test looking for duplicate Rule Id= values