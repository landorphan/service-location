## Static Code Analysis support scripts:

| Script                                               | Description                                                                                                         |
|:---------------------------------------------------- |:------------------------------------------------------------------------------------------------------------------- |
| Copy-DefaultProductionRuleset-NetFx-ToProjects.ps1   | ​Copies the default production rule set, for .Net Framework projects, to each known .Net Framework source directory. |
| Copy-DefaultProductionRuleset-NetStd-ToProjects.ps1  | ​Copies the default production rule set, for .Net Standard projects, to each known .Net Standard source directory.   |
| Copy-DefaultProductionRuleset-NetCore-ToProjects.ps1 | Copies the default production rule set, for .Net Core projects, to each known .Net Core source directory.           |
|                                                      |                                                                                                                     |
| Copy-DefaultTestRuleset-NetFx-ToProjects.ps1         | ​Copies the default product rule set, for .Net Framework projects, to each known .Net Framework test directory.      | 
| (no such thing:  MSTest .Net Standard project)       |                                                                                                                     | 
| Copy-DefaultTestRuleset-NetCore-ToProjects.ps1       | ​Copies the default product rule set, for .Net Core projects, to each known .Net Core test directory.                | 
|                                                      |                                                                                                                     |
| Copy-FxCop-Rulesets.ps1                              | Executes all of the above as a batch.                                                                               |

###Suggestion:  Do not edit project *.ruleset files.  Pick the scope of impact and edit the *.ruleset files located in \build\BuildFiles and then execute Copy-FxCop-Rulesets.ps1
​	
Merging rulesets was possible, I have no idea if it is now.  I suggest this pattern until we find a rule Action specific to an individual project.

## Build scripts:

| Script                 | Description                                                                                        |
|:---------------------- |:-------------------------------------------------------------------------------------------------- |
| Build-clean.ps1		     | performs a dotnet clean, then follows up with an attempt to delete all .\bin and .\obj directories |
| Build-Debug.ps1        | performs a Build-Clean, then builds the debug version of PokerPro.Std.sln                          |
| Build-Release.ps1      | performs a Build-Clean, then builds the release version of PokerPro.Std.sln                        |
| Test-Debug.ps1         | performs a Build-Clean, then Build-Debug, then runs Check-In and Check-In-Non-Ide tests            |
| Test-Release.ps1       | performs a Build-Clean, then Build-Release, then runs Check-In and Check-In-Non-Ide tests          |

#TODO:  command line builds with dot net build fail on .Net Framework projects with NuGet references.  (MSBuild builds them just fine)
# at present, there is one non-unit test, test project this affects; it does not impact CI builds, only dev workstation builds from the command-line
