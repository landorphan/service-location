There are currently (11) PowerShell scripts.

## Static Code Analysis support scripts:

| Script                                             | Description                                                                                                         |
|:-------------------------------------------------- |:------------------------------------------------------------------------------------------------------------------- |
| Copy-NetFx-DefaultProductionRuleset-ToProjects.ps1 | ​Copies the default production rule set, for .Net Framework projects, to each known .Net Framework source directory. |
| ​Copy-NetFx-DefaultTestRuleset-ToProjects.ps1       | Copies the default test rule set, for .Net Framework projects, to each known .Net Framework test directory.         |
| Copy-Std-DefaultProductRuleset-ToProjects.ps1      | ​Copies the default product rule set, for .Net Standard projects, to each known .Net standard source directory.      |
|                                                    |                                                                                                                     |
| Copy-FxCop-Rulesets.ps1                            | Executes all of the above as a batch.                                                                               |


###Suggestion:  Do not edit project *.ruleset files.  Pick the scope of impact and edit the *.ruleset files located in \build\BuildFiles and then execute Copy-FxCop-Rulesets.ps1
​	
Merging rulesets was possible, I have no idea if it is now.  I suggest this pattern until we find a rule Action specific to an individual project.

## Build scripts:

| Script                 | Description                                                                                        |
|:---------------------- |:-------------------------------------------------------------------------------------------------- |
| Build-clean.ps1		 | performs a dotnet clean, then follows up with an attempt to delete all .\bin and .\obj directories |
| Build-Debug.ps1        | performs a Build-Clean, then builds the debug version of PokerPro.Std.sln                          |
| Build-Release.ps1      | performs a Build-Clean, then builds the release version of PokerPro.Std.sln                        |

#TODO:  command lines builds are failing:  The type or namespace name 'VisualStudio' does not exist in the namespace 'Microsoft', this behavior is new.
Builds in VS do not exhibit this behavior.


###TODO:
Test run scripts, WBN:  Choice of Runners, DEV Workstation versus Build Server
Not implemented b/c more information needed.
