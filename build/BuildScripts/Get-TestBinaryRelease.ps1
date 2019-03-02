# requires -Version 5.1

<#
  .SYNOPSIS
    Retrieves all *.Test.dll files from the output release directory.
  .EXAMPLE
    & Get-TestBinaryDebug.ps1
  .INPUTS
    (None)
  .OUTPUTS
    [System.String[]]
#>
[CmdletBinding()]
[OutputType([System.String[]])]
param
(
  [Parameter(Position = 0,Mandatory = $false)]
  $Files
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
  $binReleaseDirectory = Join-Path -Path $thisScriptDirectory -ChildPath '../../bin/release'
}
process
{
  return Get-ChildItem -Path $binReleaseDirectory -Include '*.Tests.dll' -Recurse -File
}
