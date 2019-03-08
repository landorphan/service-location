# requires -Version 5.1

<#
  .SYNOPSIS
    Gets each .Net Core C# project in the test directory
  .EXAMPLE
    & Get-TestCSharpProjectsNetCore.ps1
  .INPUTS
    (None)
  .OUTPUTS
    [System.String[]]
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
  $setVarScript = Join-Path -Path $thisScriptDirectory -ChildPath 'Set-BuildVariables.ps1'
  $removeVarScript = Join-Path -Path $thisScriptDirectory -ChildPath 'Remove-BuildVariables.ps1'
  $getTestScript = Join-Path -Path $thisScriptDirectory -ChildPath 'Get-TestCSharpProjects.ps1'
}
process
{
  try
  {
    $tests = & $getTestScript -SolutionFileName $SolutionFileName
    & $setVarScript -SolutionFileName $SolutionFileName
    foreach ($test in $tests)
    {
      if ($test.ContainsKey('TargetFramework'))
      {
        $value = $test['TargetFramework']
        if ($value.StartsWith('netcore'))
        {
          Write-Output $test['Project']
        }
        else
        {
          Write-Debug "$test ''TargetFramework'' value does not start with ''netcore''"
        }
      }
      else
      {
        Write-Debug "$test does not have a ''TargetFramework'' key"
      }
    }
  }
  finally
  {
    & $removeVarScript
  }
}
end
{
}
