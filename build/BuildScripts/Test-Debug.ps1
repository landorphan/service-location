# requires -Version 5.1

<#
  .SYNOPSIS
    Cleans, builds debug, executes all tests.
  .EXAMPLE
    & Test-Debug.ps1
  .EXAMPLE
    ./build/BuildScript/Test-Release.ps1 -SolutionFile './Landorphan.Ioc.ServiceLocation.XPlat.sln' -VSTest
  .INPUTS
    (None)
  .OUTPUTS
    [System.String[]]
#>
[CmdletBinding()]
param
(
  [Parameter(Position = 0,HelpMessage = 'The solution file to use (needed when more than one solution file exists).')]
  [System.String]$SolutionFileName,
  [Parameter(HelpMessage = 'When set (default $false) the tests are executed via vstest.exe instead of dotnet test.')]
  [System.Management.Automation.SwitchParameter]$VSTest
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

    if ($VSTest)
    {
      Write-Debug "$($VSTest.GetType().FullName)"
      # vstest requires a list of binaries
      # ~6 minutes on my machine
      # assumes vstest.console.exe is in the path environment variable ($Env:Path)
      # On my Windows machine, the path is: C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe
      Write-Output ''
      Write-Output "Executing tests: vstest.console.exe $testBinaries /logger:trx /ResultsDirectory:$results /Parallel /TestCaseFilter:""(TestCategory=Check-In|Check-In-Non-Ide)"""
      Write-Output ''
      vstest.console.exe $testBinaries /logger:trx /ResultsDirectory:$results /Parallel /TestCaseFilter:"(TestCategory=Check-In|Check-In-Non-Ide)"
    }
    else
    {
      # dotnet test requires a solution file and a configuration
      # ~9 minutes on my machine
      Write-Output ''
      Write-Output "Executing tests: $buildSolution --configuration debug --logger trx --results-directory $results --verbosity detailed --filter ""(TestCategory=Check-In|Check-In-Non-Ide)"""
      Write-Output ''
      dotnet test $buildSolution --configuration debug --logger trx --results-directory $results --verbosity detailed --filter "(TestCategory=Check-In|Check-In-Non-Ide)"
    }
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
