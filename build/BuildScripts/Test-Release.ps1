# requires -Version 5.1

<#
  .SYNOPSIS
    Cleans, builds release, executes all tests.
  .EXAMPLE
    Test-Release.ps1
  .EXAMPLE
    Test-Release.ps1 -SolutionFileName 'HelloWorld.sln'
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
  $thisScriptDirectory = Split-Path $script:MyInvocation.MyCommand.Path

  if ($null -eq (Get-Module -Name 'CSharpBuild'))
  {
    Import-Module -Name (Join-Path -Path $thisScriptDirectory -ChildPath '../CSharpBuild')
  }
  Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  $buildReleaseScript = Join-Path -Path $thisScriptDirectory -ChildPath 'Build-Release.ps1'
}
process
{
  Set-BuildVariable -SolutionFileName $SolutionFileName
  try
  {
    & $buildReleaseScript -SolutionFileName $SolutionFileName

    $testBinaries = Get-TestBinaryRelease -SolutionFile $SolutionFileName | ForEach-Object { $_.FullName }
    $results = Join-Path -Path $buildSolutionDirectory -ChildPath 'TestResults'

    if ($null -eq $buildSolution)
    {
      Write-Error 'No Visual Studio solution found.'
      return 1
    }

    if ($buildSolution -is [array])
    {
      Write-Error 'Multiple Visual Studio solutions found; this script expects a single solution file.'
      return 2
    }

    if ($VSTest)
    {
      # vstest requires a list of binaries
      # ~5 minutes on my machine
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
      Write-Output "Executing tests: $buildSolution --configuration release --logger trx --results-directory $results --verbosity detailed --filter ""(TestCategory=Check-In|Check-In-Non-Ide)"""
      Write-Output ''
      dotnet test $buildSolution --configuration debug --logger trx --results-directory $results --verbosity detailed --filter "(TestCategory=Check-In|Check-In-Non-Ide)"
    }
  }
  finally
  {
    Clear-BuildVariable
  }
}
end
{
  $completed = [datetime]::UtcNow
  $elapsed = $completed - $started
  'Test-Release:'
  "  Elapsed        := $elapsed"
  "  Started   (UTC):= $started"
  "  Completed (UTC):= $completed"
}
