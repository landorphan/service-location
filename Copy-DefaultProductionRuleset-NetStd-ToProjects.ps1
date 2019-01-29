Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

[String] $sourceRulesetPath = ".\build\BuildFiles\Default.Production.NetStd.FxCop.15.0.WithSonarLint.ruleset"
if(Test-Path $sourceRulesetPath)
{
  [String] $dirPath = ".\source\Landorphan.Ioc.ServiceLocation"
  if(Test-Path $dirPath)
  {
    Copy-Item -Path $sourceRulesetPath -Destination "$dirPath\Landorphan.Ioc.ServiceLocation.NetStd.ruleset"
  }
  else
  {
    Write-Error "Could not find directory $dirPath"
  }

  $dirPath = ".\source\Landorphan.Ioc.ServiceLocation.Testability"
  if(Test-Path $dirPath)
  {
    Copy-Item -Path $sourceRulesetPath -Destination "$dirPath\Landorphan.Ioc.ServiceLocation.Testability.NetStd.ruleset"
  }
  else
  {
    Write-Error "Could not find directory $dirPath"
  }

  $dirPath = ".\source\Landorphan.TestUtilities.MSTest"
  if(Test-Path $dirPath)
  {
    Copy-Item -Path $sourceRulesetPath -Destination "$dirPath\Landorphan.TestUtilities.MSTest.NetStd.ruleset"
  }
  else
  {
    Write-Error "Could not find directory $dirPath"
  }

  $dirPath = ".\source\Landorphan.Abstractions"
  if(Test-Path $dirPath)
  {
    Copy-Item -Path $sourceRulesetPath -Destination "$dirPath\Landorphan.Abstractions.NetStd.ruleset"
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
