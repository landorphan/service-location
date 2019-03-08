# requires -Version 5.1

<#
  .SYNOPSIS
    Gets the root directory of all .Net Core source projects.
  .EXAMPLE
    & Get-SourceCSharpProjects.ps1
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
}
process
{
  try
  {
    & $setVarScript -SolutionFileName $SolutionFileName

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
    & $removeVarScript
  }
}
end
{
}
