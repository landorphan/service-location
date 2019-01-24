Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'


& .\Copy-DefaultProductionRuleset-NetCore-ToProjects.ps1
& .\Copy-DefaultProductionRuleset-NetFx-ToProjects.ps1
& .\Copy-DefaultProductionRuleset-NetStd-ToProjects.ps1

& .\Copy-DefaultTestRuleset-NetCore-ToProjects.ps1
& .\Copy-DefaultTestRuleset-NetFx-ToProjects.ps1
# there is no such thing as a .Net Standard test project at this time (v2.2)

