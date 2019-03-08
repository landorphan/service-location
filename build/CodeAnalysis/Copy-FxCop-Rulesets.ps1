<#
  .SYNOPSIS
    Copies all default rulesets to the respective projects in source and test.
  .EXAMPLE
    .\Copy-FxCop-Rulesets.ps1
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

  if ($null -eq (Get-Module -Name 'mwp.utilities'))
  {
    $ConfirmPreference = "High" #([High], Medium, Low, None)
    $DebugPreference = "Continue" #([SilentlyContinue], Continue, Inquire, Stop)
    $ErrorActionPreference = "Continue" #(SilentlyContinue, [Continue], Suspend <!--NOT ALLOWED -->, Inquire, Stop)
    $InformationPreference = "Continue" #(SilentlyContinue, Continue, Inquire, Stop)
    $VerbosePreference = "Continue" #([SilentlyContinue], Continue, Inquire, Stop)
    $WarningPreference = "Inquire" #(SilentlyContinue, [Continue], Inquire, Stop)
  }
  else
  {
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }

  $thisScriptDirectory = Split-Path $script:MyInvocation.MyCommand.Path
  $setVarScript = Join-Path -Path (Split-Path $thisScriptDirectory) -ChildPath 'Set-BuildVariables.ps1'
  $removeVarScript = Join-Path -Path (Split-Path $thisScriptDirectory) -ChildPath 'Remove-BuildVariables.ps1'
}
process
{
  try
  {
    & $setVarScript -SolutionFileName $SolutionFileName

    & (Join-Path -Path $thisScriptDirectory -ChildPath 'Copy-DefaultProductionRuleset-NetCore-ToProjects.ps1') -SolutionFileName $SolutionFileName
    & (Join-Path -Path $thisScriptDirectory -ChildPath 'Copy-DefaultProductionRuleset-NetFx-ToProjects.ps1') -SolutionFileName $SolutionFileName
    & (Join-Path -Path $thisScriptDirectory -ChildPath 'Copy-DefaultProductionRuleset-NetStd-ToProjects.ps1') -SolutionFileName $SolutionFileName

    & (Join-Path -Path $thisScriptDirectory -ChildPath 'Copy-DefaultTestRuleset-NetCore-ToProjects.ps1') -SolutionFileName $SolutionFileName
    & (Join-Path -Path $thisScriptDirectory -ChildPath 'Copy-DefaultTestRuleset-NetFx-ToProjects.ps1') -SolutionFileName $SolutionFileName
    # there is no such thing as a .Net Standard test project at this time (v2.2)
  }
  finally
  {
    & $removeVarScript
  }
}
