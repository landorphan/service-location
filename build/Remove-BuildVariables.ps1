# requires -Version 5.1

<#
  .SYNOPSIS
    Removes the following gobal build variables (or decrements $buildSetVarInvocationCount):
      $buildSetVarInvocationCount       The number of times Set-BuildVariables.ps1 has been called
      $buildDirectory                   (Enlistment)\build\
      $buildCodeAnalysisDirectory       (Enlistment)\build\CodeAnalysis\
      $buildProjectSpecificDirectory    (Enlistment)\build\ProjectSpecific\
      $buildScriptsDirectory            (Enlistment)\build\BuildScripts\
      $buildSolutionDirectory           (Enlistment)\
      $buildSolution                    (Enlistment)\*.sln
      $buildSourceDirectory             (Enlistment)\source
      $buildTestDirectory               (Enlistment)\test
  .INPUTS
    (None)
  .OUTPUTS
    (None)
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
}
process
{
  # decrement $buildSetVarInvocationCount, remove the build variables when it falls to zero or less.
  [System.Boolean]$remove = $false
  if ($null -eq (Get-Variable -Name buildSetVarInvocationCount -Scope global -ErrorAction 'SilentlyContinue'))
  {
    $remove = $true
  }
  else
  {
    $buildSetVarInvocationCount -= 1
    Write-Debug "`$buildSetVarInvocationCount=$buildSetVarInvocationCount"
    $remove = (0 -ge $buildSetVarInvocationCount)
  }

  if ($remove)
  {
    if ($null -ne (Get-Variable -Name buildDirectory -Scope global -ErrorAction 'SilentlyContinue'))
    {
      Remove-Variable -Name buildDirectory -Scope global -Force
    }

    if ($null -ne (Get-Variable -Name buildCodeAnalysisDirectory -Scope global -ErrorAction 'SilentlyContinue'))
    {
      Remove-Variable -Name buildCodeAnalysisDirectory -Scope global -Force
    }

    if ($null -ne (Get-Variable -Name buildProjectSpecificDirectory -Scope global -ErrorAction 'SilentlyContinue'))
    {
      Remove-Variable -Name buildProjectSpecificDirectory -Scope global -Force
    }

    if ($null -ne (Get-Variable -Name buildScriptsDirectory -Scope global -ErrorAction 'SilentlyContinue'))
    {
      Remove-Variable -Name buildScriptsDirectory -Scope global -Force
    }

    if ($null -ne (Get-Variable -Name buildSolutionDirectory -Scope global -ErrorAction 'SilentlyContinue'))
    {
      Remove-Variable -Name buildSolutionDirectory -Scope global -Force
    }

    if ($null -ne (Get-Variable -Name buildSolution -Scope global -ErrorAction 'SilentlyContinue'))
    {
      Remove-Variable -Name buildSolution -Scope global -Force
    }

    if ($null -ne (Get-Variable -Name buildSourceDirectory -Scope global -ErrorAction 'SilentlyContinue'))
    {
      Remove-Variable -Name buildSourceDirectory -Scope global -Force
    }

    if ($null -ne (Get-Variable -Name buildTestDirectory -Scope global -ErrorAction 'SilentlyContinue'))
    {
      Remove-Variable -Name buildTestDirectory -Scope global -Force
    }
  }
}
