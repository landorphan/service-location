Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

[String] $sourceRulesetPath = ".\build\BuildFiles\Default.Test.NetFx.FxCop.15.0.WithSonarLint.ruleset"
if(Test-Path $sourceRulesetPath)
{
  [String] $dirPath = ".\test\Ioc.Collections.Performance.Tests"
  if(Test-Path $dirPath)
  {
    Copy-Item -Path $sourceRulesetPath -Destination "$dirPath\Ioc.Collections.Performance.Tests.NetFx.ruleset"
  }
  else
  {
    Write-Error "Could not find directory $dirPath"
  }
}
else
{
  Write-Error "Could not find file $sourceRulesetPath"
}
