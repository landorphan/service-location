## Static Code Analysis support scripts:

| Script                                                 | Description                                                                                                              |
|:------------------------------------------------------ |:------------------------------------------------------------------------------------------------------------------------ |
| Copy-DefaultProductionRuleset-NetCore-ToProjects.ps1   | ​Copies the default production rule set, for .Net Core projects, to each known .Net Core source project folder.          |
| Copy-DefaultProductionRuleset-NetFx-ToProjects.ps1     | Copies the default production rule set, for .Net Framework projects, to each known .Net Framework source project folder. |
| Copy-DefaultProductionRuleset-NetStd-ToProjects.ps1    | ​Copies the default production rule set, for .Net Standard projects, to each known .Net Standard source project folder.  |
| Copy-DefaultTestRuleset-NetCore-ToProjects.ps1         | ​Copies the default test rule set, for .Net Core projects, to each known .Net Core test project folder.                  |
| Copy-DefaultTestRuleset-NetFx-ToProjects.ps1           | ​Copies the default test rule set, for .Net Framework projects, to each known .Net Framework test project folder.        |
|                                                        | (there is no such thing as a .Net Standard MSTest project)                                                               |
|                                                        |                                                                                                                          |
| Copy-FxCop-Rulesets.ps1                                | Executes all of the above as a batch.                                                                                    |


### Suggestion:
Do not edit project *.ruleset files.  Pick the scope of impact and edit the *.ruleset files located in *build* directory, then execute Copy-FxCop-Rulesets.ps1


Note: Merging rulesets was possible, I have no idea if it is now.  I suggest this pattern until we find a rule Action specific to an individual project.

#### CustomDictionary.xml
This is a custom dictionary for .Net Framework projects
Add it as a link to .Net Framework projects and set the BuildAction=CodeAnalysisDictionary
