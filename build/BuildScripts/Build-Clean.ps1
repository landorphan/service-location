# requires -Version 5.1

<#
  .SYNOPSIS
    Cleans the output of a solution.
  .DESCRIPTION
    This function attemps to clean the output of the previous build.  Both intermediate (obj) and final output (bin) folders are cleaned, as well as,
    packages and testresults
  .EXAMPLE
    Build-Clean.ps1
  .EXAMPLE
    Build-Clean.ps1 -SolutionFileName 'HelloWorld.sln'
  .INPUTS
    (None)
  .OUTPUTS
    (None)
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
  $thisScriptDirectory = Split-Path $script:MyInvocation.MyCommand.Path

  if ($null -eq (Get-Module -Name 'CSharpBuild'))
  {
    Import-Module -Name (Join-Path -Path $thisScriptDirectory -ChildPath '../CSharpBuild')
  }
  Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
}
process
{
  Set-BuildVariable -SolutionFileName $SolutionFileName
  try
  {
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

    dotnet clean $buildSolution > $null 2>$null
    dotnet clean $buildSolution -c debug > $null 2>$null
    dotnet clean $buildSolution -c release > $null 2>$null

    Get-ChildItem -inc bin,obj,packages -rec | Remove-Item -rec -Force 2>$null
    Get-ChildItem -inc testresults | Remove-Item -Force 2>$null
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
  'Build-Clean:'
  "  Elapsed        := $elapsed"
  "  Started   (UTC):= $started"
  "  Completed (UTC):= $completed"
}
