<#
  .SYNOPSIS
    Copies the default test .Net Core ruleset to all .Net Core projects under the test directory.
  .EXAMPLE
    .\Copy-DefaultTestRuleset-NetCore-ToProjets.ps1
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

    $testRulesetPath = Join-Path -Path $buildCodeAnalysisDirectory -ChildPath 'Default.Test.NetCore.FxCop.15.0.WithSonarLint.ruleset'
    if (Test-Path $testRulesetPath)
    {
      $getTestProjectScript = Join-Path -Path $buildDirectory -ChildPath 'Get-TestCSharpProjectsNetCore.ps1'
      $testProjects = & $getTestProjectScript -SolutionFileName $SolutionFileName
      foreach ($testProjectFile in $testProjects)
      {
        if (Test-Path $testProjectFile)
        {
          # assumes projects are named the same as the containing folder
          $testProjectDir = Split-Path -Path $testProjectFile
          $name = Split-Path -Path $testProjectDir -Leaf
          [string]$rulesetName = $name + '.NetCore.ruleset'
          [string]$destinationPath = Join-Path -Path $testProjectDir -ChildPath $rulesetName
          Copy-Item -Path $testRulesetPath -Destination $destinationPath
        }
        else
        {
          Write-Error "Could not find directory $testProjectFile"
        }
      }
    }
    else
    {
      Write-Error "Could not find file $testRulesetPath"
    }
  }
  finally
  {
    & $removeVarScript
  }
}
