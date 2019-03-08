# requires -Version 5.1

<#
  .SYNOPSIS
    Cleans, builds debug, executes all tests.
  .EXAMPLE
    & Test-Debug.ps1
  .EXAMPLE
    & Test-Debug.ps1 -SolutionFileName 'foo.sln'    
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

  $started = [datetime]::UtcNow

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
  $buildDebugScript = Join-Path -Path $thisScriptDirectory -ChildPath 'Build-Debug.ps1'
  $getTestBinaryDebugScript = Join-Path -Path $thisScriptDirectory -ChildPath 'Get-TestBinaryDebug.ps1'
}
process
{
  try
  {
    & $buildDebugScript -SolutionFileName $SolutionFileName
    & $setVarScript -SolutionFileName $SolutionFileName

    $testBinaries = & $getTestBinaryDebugScript | ForEach-Object { $_.FullName }
    foreach ($testBinary in $testBinaries)
    {
      if (!(Test-Path $testBinary))
      {
        throw "Could not find test project at: $testBinary"
      }
    }

    $results = Join-Path -Path $buildSolutionDirectory -ChildPath 'TestResults'

    # assumes vstest.console.exe is in the path environment variable ($Env:Path)
    # On my machine, the path is: C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe

    # TODO: switch to dotnet test implementation, if possible
    # TODO: figure out while the trx file is not being written
    Write-Output ''
    Write-Output "Executing tests: vstest.console.exe $testBinaries /logger:trx /ResultsDirectory:$results /Parallel /TestCaseFilter:""(TestCategory=Check-In|Check-In-Non-Ide)"""
    Write-Output ''
    vstest.console.exe $testBinaries /logger:trx /ResultsDirectory:$results /Parallel /TestCaseFilter:"(TestCategory=Check-In|Check-In-Non-Ide)"
  }
  finally
  {
    & $removeVarScript
  }
}
end
{
  $completed = [datetime]::UtcNow
  $elapsed = $completed - $started
  'Test-Debug:'
  "  Elapsed        := $elapsed"
  "  Started   (UTC):= $started"
  "  Completed (UTC):= $completed"
}
