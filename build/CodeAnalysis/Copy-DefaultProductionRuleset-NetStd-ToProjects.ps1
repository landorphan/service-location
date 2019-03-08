<#
  .SYNOPSIS
    Copies the default production .Net Standard ruleset to all .Net Core projects under the source directory.
  .EXAMPLE
    .\Copy-DefaultProductionRuleset-NetStd-ToProjets.ps1
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

    $sourceRulesetPath = Join-Path -Path $buildCodeAnalysisDirectory -ChildPath 'Default.Production.NetStd.FxCop.15.0.WithSonarLint.ruleset'
    if (Test-Path $sourceRulesetPath)
    {
      $getSourceProjectScript = Join-Path -Path $buildDirectory -ChildPath 'Get-SourceCSharpProjectsNetStd.ps1'
      $sourceProjects = & $getSourceProjectScript -SolutionFileName $SolutionFileName
      foreach ($sourceProjectFile in $sourceProjects)
      {
        if (Test-Path $sourceProjectFile)
        {
          # assumes projects are named the same as the containing folder
          $sourceProjectDir = Split-Path -Path $sourceProjectFile
          $name = Split-Path -Path $sourceProjectDir -Leaf
          [string]$rulesetName = $name + '.NetStd.ruleset'
          [string]$destinationPath = Join-Path -Path $sourceProjectDir -ChildPath $rulesetName
          Copy-Item -Path $sourceRulesetPath -Destination $destinationPath
        }
        else
        {
          Write-Error "Could not find directory $sourceProjectFile"
        }
      }
    }
    else
    {
      Write-Error "Could not find file $sourceRulesetPath"
    }
  }
  finally
  {
    & $removeVarScript
  }
}
