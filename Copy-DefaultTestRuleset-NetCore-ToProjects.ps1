Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

[String] $sourceRulesetPath = ".\build\BuildFiles\Default.Test.NetCore.FxCop.15.0.WithSonarLint.ruleset"
if(Test-Path $sourceRulesetPath)
{
  [String] $dirPath = ".\test\Landorphan.Ioc.ServiceLocation.Tests"
  if(Test-Path $dirPath)
  {
    Copy-Item -Path $sourceRulesetPath -Destination "$dirPath\Landorphan.Ioc.ServiceLocation.Tests.NetCore.ruleset"
  }
  else
  {
    Write-Error "Could not find directory $dirPath"
  }

  $dirPath = ".\test\Landorphan.TestUtilities.MSTest.Tests"
  if(Test-Path $dirPath)
  {
    Copy-Item -Path $sourceRulesetPath -Destination "$dirPath\Landorphan.TestUtilities.MSTest.Tests.NetCore.ruleset"
  }
  else
  {
    Write-Error "Could not find directory $dirPath"
  }

  $dirPath = ".\test\Landorphan.Ioc.ServiceLocation.Testability.Tests"
  if(Test-Path $dirPath)
  {
    Copy-Item -Path $sourceRulesetPath -Destination "$dirPath\Landorphan.Ioc.ServiceLocation.Testability.Tests.NetCore.ruleset"
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
