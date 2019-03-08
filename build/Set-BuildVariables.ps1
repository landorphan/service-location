# requires -Version 5.1

<#
  .SYNOPSIS
    Sets the following gobal build variables (or increments $buildSetVarInvocationCount):
      $buildSetVarInvocationCount       The number of times this script has been called (works in tandem with Remove-BuildVariables.ps1)
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
}
process
{
  if ($null -eq (Get-Variable -Name buildSetVarInvocationCount -Scope Global -ErrorAction SilentlyContinue))
  {
    New-Variable -Name buildSetVarInvocationCount -Scope Global -Value 1
  }
  else
  {
    $buildSetVarInvocationCount += 1
  }

  if ($null -eq (Get-Variable -Name buildDirectory -Scope Global -ErrorAction SilentlyContinue))
  {
    New-Variable -Name buildDirectory -Scope Global -Value (Split-Path -Path $script:MyInvocation.MyCommand.Path)
    if (!(Test-Path $buildDirectory))
    {
      Write-Warning "Build directory not found: [$buildDirectory]"
    }
  }

  if ($null -eq (Get-Variable -Name buildCodeAnalysisDirectory -Scope Global -ErrorAction SilentlyContinue))
  {
    New-Variable -Name buildCodeAnalysisDirectory -Scope Global -Value (Join-Path -Path $buildDirectory -ChildPath 'CodeAnalysis')
    if (!(Test-Path $buildCodeAnalysisDirectory))
    {
      Write-Warning "Build Code Analysis directory not found: [$buildCodeAnalysisDirectory]"
    }
  }

  if ($null -eq (Get-Variable -Name buildProjectSpecificDirectory -Scope Global -ErrorAction SilentlyContinue))
  {
    New-Variable -Name buildProjectSpecificDirectory -Scope Global -Value (Join-Path -Path $buildDirectory -ChildPath 'ProjectSpecific')
    if (!(Test-Path $buildProjectSpecificDirectory))
    {
      Write-Information "(Optional) Build Project Specific directory not found: [$buildProjectSpecificDirectory]"
    }
  }

  if ($null -eq (Get-Variable -Name buildScriptsDirectory -Scope Global -ErrorAction SilentlyContinue))
  {
    New-Variable -Name buildScriptsDirectory -Scope Global -Value (Join-Path -Path $buildDirectory -ChildPath 'BuildScripts')
    if (!(Test-Path $buildScriptsDirectory))
    {
      Write-Warning "Build Scripts directory not found: [$buildScriptsDirectory]"
    }
  }

  if ($null -eq (Get-Variable -Name buildSolutionDirectory -Scope Global -ErrorAction SilentlyContinue))
  {
    New-Variable -Name buildSolutionDirectory -Scope Global -Value (Split-Path $buildDirectory)
    if (!(Test-Path $buildSolutionDirectory))
    {
      Write-Warning "Build Solution directory not found: [$buildSolutionDirectory]"
    }
  }

  if ($null -eq (Get-Variable -Name buildSolution -Scope Global -ErrorAction SilentlyContinue))
  {
    if (!$SolutionFileName)
    {
      # Solution File Name NOT specified; do a search
      $searchPath = [System.IO.Path]::Combine($buildSolutionDirectory,"*.sln")
      New-Variable -Name buildSolution -Scope Global -Value (Get-ChildItem -Path $searchPath -File)
      if ($null -eq $buildSolution)
      {
        Write-Warning "No Visual Studio Solutions found in [$buildSolutionDirectory]"
      }
      elseif ($buildSolution -is [System.Array])
      {
        Write-Warning "More than one Visual Studio Solutions were found in [$buildSolutionDirectory]"
      }
    }
    else
    {
      # Solution File Name specified
      Write-Debug "`$SolutionFileName = $SolutionFileName"
      if (![System.IO.Path]::IsPathRooted($SolutionFileName))
      {
        Write-Debug "The path $SolutionFileName is NOT rooted"
        $SolutionFileName = Join-Path -Path $buildSolutionDirectory -ChildPath $SolutionFileName
      }
      else
      {
        Write-Debug "The path $SolutionFileName is rooted"
      }

      if (![System.IO.File]::Exists($SolutionFileName))
      {
        $msg = "Could not load file {0}. The system cannot find the file specified." -f $SolutionFileName
        throw [System.IO.FileNotFoundException]::new($msg,$SolutionFileName)
      }

      New-Variable -Name buildSolution -Scope Global -Value (Resolve-Path -Path $SolutionFileName)
    }
  }

  if ($null -eq (Get-Variable -Name buildSourceDirectory -Scope Global -ErrorAction SilentlyContinue))
  {
    New-Variable -Name buildSourceDirectory -Scope Global -Value (Join-Path -Path $buildSolutionDirectory -ChildPath 'source')
    if (!(Test-Path $buildSourceDirectory))
    {
      Write-Warning "Build Source directory not found: [$buildSourceDirectory]"
    }
  }

  if ($null -eq (Get-Variable -Name buildTestDirectory -Scope Global -ErrorAction SilentlyContinue))
  {
    New-Variable -Name buildTestDirectory -Scope Global -Value (Join-Path -Path $buildSolutionDirectory -ChildPath 'test')
    if (!(Test-Path $buildTestDirectory))
    {
      Write-Warning "Build Test directory not found: [$buildTestDirectory]"
    }
  }
}
