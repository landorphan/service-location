# requires -Version 5.1

<#
  .SYNOPSIS
    Gets each .Net Core C# project in the test directory
  .EXAMPLE
    & Get-TestCSharpProjectsNetFx.ps1
  .INPUTS
    (None)
  .OUTPUTS
    [System.String[]]
#>
[CmdletBinding()]
param()
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
    $tests = & $getTestScript
    & $setVarScript
    foreach ($test in $tests)
    {
      if ($test.ContainsKey('TargetFrameworkVersion'))
      {
        Write-Output $test['Project']
      }
      else
      {
        Write-Debug "$test does not have a ''TargetFrameworkVersion'' key"
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
