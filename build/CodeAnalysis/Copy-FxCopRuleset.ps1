<#
  .SYNOPSIS
    Copies all default rulesets to the respective projects in source and test.
  .EXAMPLE
    Copy-FxCop-Rulesets.ps1
  .EXAMPLE
    Copy-FxCop-Rulesets.ps1 -SolutionFileName 'HelloWorld.sln'
  .INPUTS
    (NONE)
  .OUTPUTS
    (NONE)
#>
[CmdletBinding()]
param
(
  [Parameter(Position = 0,HelpMessage = 'The solution file to use (needed when more than one solution file exists).')]
  [System.String]$SolutionFileName
)
begin
{
  Set-StrictMode -Version Latest
  $started = [datetime]::UtcNow
  $thisScriptDirectory = Split-Path $script:MyInvocation.MyCommand.Path

  if ($null -eq (Get-Module -Name 'CSharpBuild'))
  {
    Import-Module -Name (Join-Path -Path $thisScriptDirectory -ChildPath '../CSharpBuild')
  }
  Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
}
process
{
  Set-BuildVariable -SolutionFileName $SolutionFileName
  try
  {
    Copy-SourceRulesetNetCore -SolutionFileName $SolutionFileName
    Copy-SourceRulesetNetFx -SolutionFileName $SolutionFileName
    Copy-SourceRulesetNetStd -SolutionFileName $SolutionFileName

    Copy-TestRulesetNetCore -SolutionFileName $SolutionFileName
    return
    Copy-TestRulesetNetFx -SolutionFileName $SolutionFileName
    # there is no such thing as a .Net Standard test project at this time (v2.2)
  }
  finally
  {
    Write-Debug "Finally"
    Clear-BuildVariable
  }
  end
  {
    $completed = [datetime]::UtcNow
    $elapsed = $completed - $started
    'Copy-FxCopyRuleset:'
    "  Elapsed        := $elapsed"
    "  Started   (UTC):= $started"
    "  Completed (UTC):= $completed"
  }
}
