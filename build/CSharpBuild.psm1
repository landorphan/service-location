#Requires -Version 5.1
Set-StrictMode -Version Latest

#region Exported

function Clear-BuildVariable
{
<#
  .SYNOPSIS
    Removes the following gobal build variables (or decrements $buildSetVarInvocationCount):
      $buildSetVarInvocationCount       The number of times Set-BuildVariable.ps1 has been called
      $buildDirectory                   (Enlistment)\build\
      $buildCodeAnalysisDirectory       (Enlistment)\build\CodeAnalysis\
      $buildProjectSpecificDirectory    (Enlistment)\build\ProjectSpecific\
      $buildScriptsDirectory            (Enlistment)\build\BuildScripts\
      $buildSolutionDirectory           (Enlistment)\
      $buildSolution                    (Enlistment)\*.sln
      $buildSourceDirectory             (Enlistment)\source
      $buildTestDirectory               (Enlistment)\test
  .EXAMPLE
    Clear-BuildVariable
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
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

      $global:buildSetVarInvocationCount -= 1
      Write-Debug "CSharpBuild.psm1 Clear-BuildVariable `$global:buildSetVarInvocationCount=$global:buildSetVarInvocationCount"
      $remove = (0 -ge $buildSetVarInvocationCount)
    }

    if ($remove)
    {
      if ($null -ne (Get-Variable -Name buildSetVarInvocationCount -Scope global -ErrorAction 'SilentlyContinue'))
      {
        Remove-Variable -Name buildSetVarInvocationCount -Scope global -Force
      }

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
  end {}
}

function Copy-SourceRulesetNetCore
{
<#
  .SYNOPSIS
    Copies the default production .Net Core ruleset to all .Net Core projects under the source directory.
  .EXAMPLE
    Copy-SourceRulesetNetCore
  .EXAMPLE
    Copy-SourceRulesetNetCore -SolutionFileName 'HelloWorld.sln'
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $sourceRulesetPath = Join-Path -Path $buildCodeAnalysisDirectory -ChildPath 'Default.Source.NetCore.FxCop.15.0.WithSonarLint.ruleset'
      if (Test-Path $sourceRulesetPath)
      {
        $sourceProjects = Get-SourceCSharpProjectNetCore -SolutionFileName $SolutionFileName
        foreach ($sourceProjectFile in $sourceProjects)
        {
          # assumes projects are named the same as the containing folder
          $sourceProjectDir = Split-Path -Path $sourceProjectFile
          $name = Split-Path -Path $sourceProjectDir -Leaf
          [string]$rulesetName = $name + '.NetCore.ruleset'
          [string]$destinationPath = Join-Path -Path $sourceProjectDir -ChildPath $rulesetName
          Copy-Item -Path $sourceRulesetPath -Destination $destinationPath
        }
      }
      else
      {
        Write-Error "Could not find file $sourceRulesetPath"
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Copy-SourceRulesetNetFx
{
<#
  .SYNOPSIS
    Copies the default production .Net Framewokr ruleset to all .Net Framework projects under the source directory.
  .EXAMPLE
    Copy-SourceRulesetNetFx
  .EXAMPLE
    Copy-SourceRulesetNetFx -SolutionFileName 'HelloWorld.sln'
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $sourceRulesetPath = Join-Path -Path $buildCodeAnalysisDirectory -ChildPath 'Default.Source.NetFx.FxCop.15.0.WithSonarLint.ruleset'
      if (Test-Path $sourceRulesetPath)
      {
        $sourceProjects = Get-SourceCSharpProjectNetFx -SolutionFileName $SolutionFileName
        foreach ($sourceProjectFile in $sourceProjects)
        {
          # assumes projects are named the same as the containing folder
          $sourceProjectDir = Split-Path -Path $sourceProjectFile
          $name = Split-Path -Path $sourceProjectDir -Leaf
          [string]$rulesetName = $name + '.NetFx.ruleset'
          [string]$destinationPath = Join-Path -Path $sourceProjectDir -ChildPath $rulesetName
          Copy-Item -Path $sourceRulesetPath -Destination $destinationPath
        }
      }
      else
      {
        Write-Error "Could not find file $sourceRulesetPath"
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Copy-SourceRulesetNetStd
{
<#
  .SYNOPSIS
    Copies the default production .Net Standard ruleset to all .Net Core projects under the source directory.
  .EXAMPLE
    Copy-SourceRulesetNetStd
.EXAMPLE
    Copy-TestRulesetNetStd -SolutionFileName 'HelloWorld.sln'
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $sourceRulesetPath = Join-Path -Path $buildCodeAnalysisDirectory -ChildPath 'Default.Source.NetStd.FxCop.15.0.WithSonarLint.ruleset'
      if (Test-Path $sourceRulesetPath)
      {
        $sourceProjects = Get-SourceCSharpProjectNetStd -SolutionFileName $SolutionFileName
        foreach ($sourceProjectFile in $sourceProjects)
        {
          # assumes projects are named the same as the containing folder
          $sourceProjectDir = Split-Path -Path $sourceProjectFile
          $name = Split-Path -Path $sourceProjectDir -Leaf
          [string]$rulesetName = $name + '.NetStd.ruleset'
          [string]$destinationPath = Join-Path -Path $sourceProjectDir -ChildPath $rulesetName
          Copy-Item -Path $sourceRulesetPath -Destination $destinationPath
        }
      }
      else
      {
        Write-Error "Could not find file $sourceRulesetPath"
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Copy-TestRulesetNetCore
{
<#
  .SYNOPSIS
    Copies the default production .Net Core ruleset to all .Net Core projects under the test directory.
  .EXAMPLE
    Copy-TestRulesetNetCore
  .EXAMPLE
    Copy-TestRulesetNetCore -SolutionFileName 'HelloWorld.sln'
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $testRulesetPath = Join-Path -Path $buildCodeAnalysisDirectory -ChildPath 'Default.Test.NetCore.FxCop.15.0.WithSonarLint.ruleset'
      if (Test-Path $testRulesetPath)
      {
        $testProjects = Get-TestCSharpProjectNetCore -SolutionFileName $SolutionFileName
        foreach ($testProjectFile in $testProjects)
        {
          # assumes projects are named the same as the containing folder
          $testProjectDir = Split-Path -Path $testProjectFile
          $name = Split-Path -Path $testProjectDir -Leaf
          [string]$rulesetName = $name + '.NetCore.ruleset'
          [string]$destinationPath = Join-Path -Path $testProjectDir -ChildPath $rulesetName
          Copy-Item -Path $testRulesetPath -Destination $destinationPath
        }
      }
      else
      {
        Write-Error "Could not find file $testRulesetPath"
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Copy-TestRulesetNetFx
{
<#
  .SYNOPSIS
    Copies the default production .Net Fx ruleset to all .Net Fx projects under the test directory.
  .EXAMPLE
    Copy-TestRulesetNetFx
  .EXAMPLE
    Copy-TestRulesetNetFx -SolutionFileName 'HelloWorld.sln'
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $testRulesetPath = Join-Path -Path $buildCodeAnalysisDirectory -ChildPath 'Default.Test.NetFx.FxCop.15.0.WithSonarLint.ruleset'
      if (Test-Path $testRulesetPath)
      {
        $testProjects = Get-TestCSharpProjectNetFx -SolutionFileName $SolutionFileName
        foreach ($testProjectFile in $testProjects)
        {
          # assumes projects are named the same as the containing folder
          $testProjectDir = Split-Path -Path $testProjectFile
          $name = Split-Path -Path $testProjectDir -Leaf
          [string]$rulesetName = $name + '.NetFx.ruleset'
          [string]$destinationPath = Join-Path -Path $testProjectDir -ChildPath $rulesetName
          Copy-Item -Path $testRulesetPath -Destination $destinationPath
        }
      }
      else
      {
        Write-Error "Could not find file $testRulesetPath"
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Get-TestBinaryDebug
{
<#
  .SYNOPSIS
    Retrieves all *.Test.dll files from the output debug directory.
  .EXAMPLE
    Get-TestBinaryDebug
  .INPUTS
    (None)
  .OUTPUTS
    [System.String[]]
#>
  [CmdletBinding()]
  [OutputType([System.String[]])]
  param
  (
    [Parameter(Position = 0,HelpMessage = 'The solution file to use (needed when more than one solution file exists).')]
    [System.String]$SolutionFileName
  )
  begin
  {
    Set-StrictMode -Version Latest
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $binDebugDirectory = Join-Path -Path $buildSolutionDirectory -ChildPath './bin/debug'
      return Get-ChildItem -Path $binDebugDirectory -Include '*.Tests.dll' -Recurse -File
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Get-TestBinaryRelease
{
<#
  .SYNOPSIS
    Retrieves all *.Test.dll files from the output debug directory.
  .EXAMPLE
    Get-TestBinaryRelease
  .INPUTS
    (None)
  .OUTPUTS
    [System.String[]]
#>
  [CmdletBinding()]
  [OutputType([System.String[]])]
  param
  (
    [Parameter(Position = 0,HelpMessage = 'The solution file to use (needed when more than one solution file exists).')]
    [System.String]$SolutionFileName
  )
  begin
  {
    Set-StrictMode -Version Latest
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $binDebugDirectory = Join-Path -Path $buildSolutionDirectory -ChildPath './bin/release'
      return Get-ChildItem -Path $binDebugDirectory -Include '*.Tests.dll' -Recurse -File
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Set-BuildVariable
{
<#
  .SYNOPSIS
    Sets the following gobal build variables, (or increments $buildSetVarInvocationCount):
      $buildSetVarInvocationCount       The number of times this script has been called (works in tandem with Clear-BuildVariable.ps1)
      $buildDirectory                   (Enlistment)\build\
      $buildCodeAnalysisDirectory       (Enlistment)\build\CodeAnalysis\
      $buildProjectSpecificDirectory    (Enlistment)\build\ProjectSpecific\
      $buildScriptsDirectory            (Enlistment)\build\BuildScripts\
      $buildSolutionDirectory           (Enlistment)\
      $buildSolution                    (Enlistment)\*.sln
      $buildSourceDirectory             (Enlistment)\source
      $buildTestDirectory               (Enlistment)\test
  .EXAMPLE
    Set-BuildVariable
  .EXAMPLE
    Set-BuildVariable -SolutionFileName 'helloworld.sln'
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    if ($null -eq (Get-Variable -Name buildSetVarInvocationCount -Scope Global -ErrorAction SilentlyContinue))
    {
      Write-Debug "CSharpBuild.psm1 Allocating buildSetVarInvocationCount"
      New-Variable -Name buildSetVarInvocationCount -Scope Global -Value 1
    }
    else
    {
      $global:buildSetVarInvocationCount += 1
    }
    Write-Debug "CSharpBuild.psm1 Set-BuildVariable `$global:buildSetVarInvocationCount=$global:buildSetVarInvocationCount"
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
  end {}
}

function Use-CallerPreference
{
<#
    .Synopsis
      Fetches "Preference" variable values from the caller's scope.
    .DESCRIPTION
      Script module functions do not automatically inherit their caller's variables, but they can be
      obtained through the $PSCmdlet variable in Advanced Functions.  This function is a helper function
      for any script module Advanced Function; by passing in the values of $ExecutionContext.SessionState
      and $PSCmdlet, Use-CallerPreference will set the caller's preference variables locally.
    .PARAMETER Cmdlet
      The $PSCmdlet object from a script module Advanced Function.
    .PARAMETER SessionState
      The $ExecutionContext.SessionState object from a script module Advanced Function.  This is how the
      Use-CallerPreference function sets variables in its callers' scope, even if that caller is in a different
      script module.
    .PARAMETER Name
      Optional array of parameter names to retrieve from the caller's scope.  Default is to retrieve all
      Preference variables as defined in the about_Preference_Variables help file (as of PowerShell 4.0)
      This parameter may also specify names of variables that are not in the about_Preference_Variables
      help file, and the function will retrieve and set those as well.
    .EXAMPLE
      Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState

      Imports the default PowerShell preference variables from the caller into the local scope.
    .EXAMPLE
      Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState -Name 'ErrorActionPreference','SomeOtherVariable'

      Imports only the ErrorActionPreference and SomeOtherVariable variables into the local scope.
    .EXAMPLE
      'ErrorActionPreference','SomeOtherVariable' | Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState

      Same as Example 2, but sends variable names to the Name parameter via pipeline input.
    .INPUTS
      String
    .OUTPUTS
      None.  This function does not produce pipeline output.
    .LINK
       about_Preference_Variables
      http://powershell.org/wp/2014/01/20/revisited-script-modules-and-variable-scopes/
  #>

  # This function was downloaded from the Microsoft TechNet gallery on 2015-05-25 at:
  #   https://gallery.technet.microsoft.com/scriptcenter/Inherit-Preference-82343b9d
  # License: MICROSOFT LIMITED PUBLIC LICENSE  (TechNet terms of use)
  # Original Author: David Wyatt,  Microsoft's MVP (PowerShell)
  # Original Name: Get-CallerPreference

  [CmdletBinding(DefaultParameterSetName = 'AllVariables')]
  param(
    [Parameter(Mandatory = $true)]
    [ValidateScript({ $_.GetType().FullName -eq 'System.Management.Automation.PSScriptCmdlet' })]
    $Cmdlet,

    [Parameter(Mandatory = $true)]
    [System.Management.Automation.SessionState]
    $SessionState,

    [Parameter(ParameterSetName = 'Filtered',ValueFromPipeline = $true)]
    [System.String[]]
    $Name
  )
  begin
  {
    Set-StrictMode -Version Latest
    $filterHash = @{}
  }
  process
  {
    if ($null -ne $Name)
    {
      foreach ($string in $Name)
      {
        $filterHash[$string] = $true
      }
    }
  }
  end
  {
    # List of preference variables taken from the about_Preference_Variables help file in PowerShell version 4.0

    $vars = @{
      'ErrorView' = $null
      'FormatEnumerationLimit' = $null
      'LogCommandHealthEvent' = $null
      'LogCommandLifecycleEvent' = $null
      'LogEngineHealthEvent' = $null
      'LogEngineLifecycleEvent' = $null
      'LogProviderHealthEvent' = $null
      'LogProviderLifecycleEvent' = $null
      'MaximumAliasCount' = $null
      'MaximumDriveCount' = $null
      'MaximumErrorCount' = $null
      'MaximumFunctionCount' = $null
      'MaximumHistoryCount' = $null
      'MaximumVariableCount' = $null
      'OFS' = $null
      'OutputEncoding' = $null
      'ProgressPreference' = $null
      'PSDefaultParameterValues' = $null
      'PSEmailServer' = $null
      'PSModuleAutoLoadingPreference' = $null
      'PSSessionApplicationName' = $null
      'PSSessionConfigurationName' = $null
      'PSSessionOption' = $null

      'ErrorActionPreference' = 'ErrorAction'
      'DebugPreference' = 'Debug'
      'ConfirmPreference' = 'Confirm'
      'WhatIfPreference' = 'WhatIf'
      'VerbosePreference' = 'Verbose'
      'WarningPreference' = 'WarningAction'
    }


    foreach ($entry in $vars.GetEnumerator())
    {
      if (([System.String]::IsNullOrEmpty($entry.Value) -or -not $Cmdlet.MyInvocation.BoundParameters.ContainsKey($entry.Value)) -and
        ($PSCmdlet.ParameterSetName -eq 'AllVariables' -or $filterHash.ContainsKey($entry.Name)))
      {
        $variable = $Cmdlet.SessionState.PSVariable.Get($entry.Key)

        if ($null -ne $variable)
        {
          if ($SessionState -eq $ExecutionContext.SessionState)
          {
            # -Scope 1 = Parent Scope
            Set-Variable -Scope 1 -Name $variable.Name -Value $variable.Value -Force -Confirm:$false -WhatIf:$false
          }
          else
          {
            $SessionState.PSVariable.Set($variable.Name,$variable.Value)
          }
        }
      }
    }

    if ($PSCmdlet.ParameterSetName -eq 'Filtered')
    {
      foreach ($varName in $filterHash.Keys)
      {
        if (-not $vars.ContainsKey($varName))
        {
          $variable = $Cmdlet.SessionState.PSVariable.Get($varName)

          if ($null -ne $variable)
          {
            if ($SessionState -eq $ExecutionContext.SessionState)
            {
              # -Scope 1 = Parent Scope
              Set-Variable -Scope 1 -Name $variable.Name -Value $variable.Value -Force -Confirm:$false -WhatIf:$false
            }
            else
            {
              $SessionState.PSVariable.Set($variable.Name,$variable.Value)
            }
          }
        }
      }
    }
  }
}

#endregion Exported

function Get-SourceCSharpProject
{
<#
  .SYNOPSIS
    Gets the root directory of all .Net Core source projects.
  .EXAMPLE
    Get-SourceCSharpProject
  .INPUTS
    (None)
  .OUTPUTS
    [System.Collections.HashTable[]]
    There are two structures:
      Project=(FullPath);TargetFramework=(.NetCore or .NetStandard version)
      Project=(FullPath);TargetFrameworkVersion=(.Net Framework version)
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $sourceProjectDirectories = $null
      Get-ChildItem -Path $buildSourceDirectory -Directory -OutVariable sourceProjectDirectories | Out-Null
      foreach ($projectDir in $sourceProjectDirectories)
      {
        $project = $null
        Get-ChildItem -Path (Join-Path -Path $buildSourceDirectory -ChildPath (Join-Path -Path $projectDir -ChildPath '*.csproj')) -File -OutVariable project | Out-Null
        if (($null -eq $project) -or (0 -eq $project.Count))
        {
          Write-Warning "No C# project found in $projectDir"
        }
        elseif (1 -lt $project.Count)
        {
          Write-Warning "More than one C# project found in $projectDir"
        }
        else
        {
          [System.String]$fullPath = Resolve-Path -Path $project
          [System.Xml.XmlDocument]$xmlDoc = Get-Content -Path $fullPath
          [System.Xml.XmlNode]$node = $null
          # Try .Net Core and .Net Standard first
          # Namespace URI is empty
          $node = $xmlDoc.SelectSingleNode("/Project/PropertyGroup/TargetFramework")
          if ($null -eq $node)
          {
            # Namespace URI is not empty
            [System.Xml.XmlNamespaceManager]$ns = New-Object System.Xml.XmlNamespaceManager $xmldoc.NameTable
            $ns.AddNamespace("msbld","http://schemas.microsoft.com/developer/msbuild/2003");

            # Not a .Net Core nor .Net Standard C# project
            # Try .Net Framework
            $node = $xmlDoc.SelectSingleNode("/msbld:Project/msbld:PropertyGroup/msbld:TargetFrameworkVersion",$ns);
            if ($null -eq $node)
            {
              Write-Warning "The project $project does not appear to be any of the following project types:  .Net Core, .Net Standard, .Net Framework"
            }
            else
            {
              [System.String]$targetFrameworkVersion = $node.InnerText
              Write-Output @{ Project = $fullPath; TargetFrameworkVersion = $targetFrameworkVersion }
            }
          }
          else
          {
            [System.String]$targetFramework = $node.InnerText
            Write-Output @{ Project = $fullPath; TargetFramework = $targetFramework }
          }
        }
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Get-SourceCSharpProjectNetCore
{
<#
  .SYNOPSIS
    Gets each .Net Core C# project in the source directory
  .EXAMPLE
    Get-SourceCSharpProjectNetCore
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $sources = Get-SourceCSharpProject -SolutionFileName $SolutionFileName
      foreach ($source in $sources)
      {
        if ($source.ContainsKey('TargetFramework'))
        {
          $value = $source['TargetFramework']
          if ($value.StartsWith('netcore'))
          {
            Write-Output $source['Project']
          }
          else
          {
            Write-Debug "$source ''TargetFramework'' value does not start with ''netcore''"
          }
        }
        else
        {
          Write-Debug "$source does not have a ''TargetFramework'' key"
        }
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Get-SourceCSharpProjectNetFx
{
<#
  .SYNOPSIS
    Gets each .Net Core C# project in the source directory
  .EXAMPLE
    Get-SourceCSharpProjectNetFx
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $sources = Get-SourceCSharpProject -SolutionFileName $SolutionFileName
      foreach ($source in $sources)
      {
        if ($source.ContainsKey('TargetFrameworkVersion'))
        {
          Write-Output $source['Project']
        }
        else
        {
          Write-Debug "$source does not have a ''TargetFrameworkVersion'' key"
        }
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Get-SourceCSharpProjectNetStd
{
<#
  .SYNOPSIS
    Gets each .Net Core C# project in the source directory
  .EXAMPLE
    Get-SourceCSharpProjectNetStd
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $sources = Get-SourceCSharpProject -SolutionFileName $SolutionFileName
      foreach ($source in $sources)
      {
        if ($source.ContainsKey('TargetFramework'))
        {
          $value = $source['TargetFramework']
          if ($value.StartsWith('netstandard'))
          {
            Write-Output $source['Project']
          }
          else
          {
            Write-Debug "$source ''TargetFramework'' value does not start with ''netstandard''"
          }
        }
        else
        {
          Write-Debug "$source does not have a ''TargetFramework'' key"
        }
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Get-TestCSharpProject
{
<#
  .SYNOPSIS
    Gets the root directory of all .Net Core test projects.
  .EXAMPLE
    Get-TestCSharpProject
  .INPUTS
    (None)
  .OUTPUTS
    [System.Collections.HashTable[]]
    There are two structures:
      Project=(FullPath);TargetFramework=(.NetCore or .NetStandard version)
      Project=(FullPath);TargetFrameworkVersion=(.Net Framework version)
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $testProjectDirectories = $null
      Get-ChildItem -Path $buildTestDirectory -Directory -OutVariable testProjectDirectories | Out-Null
      foreach ($projectDir in $testProjectDirectories)
      {
        $project = $null
        Get-ChildItem -Path (Join-Path -Path $buildTestDirectory -ChildPath (Join-Path -Path $projectDir -ChildPath '*.csproj')) -File -OutVariable project | Out-Null
        if (($null -eq $project) -or (0 -eq $project.Count))
        {
          Write-Warning "No C# project found in $projectDir"
        }
        elseif (1 -lt $project.Count)
        {
          Write-Warning "More than one C# project found in $projectDir"
        }
        else
        {
          [System.String]$fullPath = Resolve-Path -Path $project
          [System.Xml.XmlDocument]$xmlDoc = Get-Content -Path $fullPath
          [System.Xml.XmlNode]$node = $null
          # Try .Net Core and .Net Standard first
          # Namespace URI is empty
          $node = $xmlDoc.SelectSingleNode("/Project/PropertyGroup/TargetFramework")
          if ($null -eq $node)
          {
            # Namespace URI is not empty
            [System.Xml.XmlNamespaceManager]$ns = New-Object System.Xml.XmlNamespaceManager $xmldoc.NameTable
            $ns.AddNamespace("msbld","http://schemas.microsoft.com/developer/msbuild/2003");

            # Not a .Net Core nor .Net Standard C# project
            # Try .Net Framework
            $node = $xmlDoc.SelectSingleNode("/msbld:Project/msbld:PropertyGroup/msbld:TargetFrameworkVersion",$ns);
            if ($null -eq $node)
            {
              Write-Warning "The project $project does not appear to be any of the following project types:  .Net Core, .Net Standard, .Net Framework"
            }
            else
            {
              [System.String]$targetFrameworkVersion = $node.InnerText
              Write-Output @{ Project = $fullPath; TargetFrameworkVersion = $targetFrameworkVersion }
            }
          }
          else
          {
            [System.String]$targetFramework = $node.InnerText
            Write-Output @{ Project = $fullPath; TargetFramework = $targetFramework }
          }
        }
      }
    }
    finally
    {
      Clear-BuildVariable
    }
  }
  end {}
}

function Get-TestCSharpProjectNetCore
{
<#
  .SYNOPSIS
    Gets each .Net Core C# project in the test directory
  .EXAMPLE
    Get-TestCSharpProjectNetCore
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $tests = Get-TestCSharpProject -SolutionFileName $SolutionFileName
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
      Clear-BuildVariable
    }
  }
  end {}
}

function Get-TestCSharpProjectNetFx
{
<#
  .SYNOPSIS
    Gets each .Net Core C# project in the test directory
  .EXAMPLE
    Get-TestCSharpProjectNetFx
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
    Use-CallerPreference -Cmdlet $PSCmdlet -SessionState $ExecutionContext.SessionState
  }
  process
  {
    Set-BuildVariable -SolutionFileName $SolutionFileName
    try
    {
      $tests = Get-TestCSharpProject -SolutionFileName $SolutionFileName
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
      Clear-BuildVariable
    }
  }
  end {}
}

Export-ModuleMember -Function Clear-BuildVariable
Export-ModuleMember -Function Copy-SourceRulesetNetCore
Export-ModuleMember -Function Copy-SourceRulesetNetFx
Export-ModuleMember -Function Copy-SourceRulesetNetStd
Export-ModuleMember -Function Copy-TestRulesetNetCore
Export-ModuleMember -Function Copy-TestRulesetNetFx
Export-ModuleMember -Function Get-TestBinaryDebug
Export-ModuleMember -Function Get-TestBinaryRelease
Export-ModuleMember -Function Set-BuildVariable
Export-ModuleMember -Function Use-CallerPreference
