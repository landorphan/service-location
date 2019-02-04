Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

# Test for elevated status:
$identity = [Security.Principal.WindowsIdentity]::GetCurrent()
$principal = New-Object Security.Principal.WindowsPrincipal -ArgumentList $identity
if (!$principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator))
{
  # TODO: something has changed with expansion and or determining script path at run-time
  Write-Error "This script requires elevated privileges."
  return
}

<# *************************************************************************************************************************
Delete the following folder recursively (C:\ is not hard-coded, but typical)
  C:\Landorphan.Abstractions.Test.UnitTestTarget
Remove the share: 
  \\localhost\Landorphan.Abstractions.Test.UnitTestTarget
************************************************************************************************************************* #>
# usually C:\
[string]$rootSystem = [System.IO.Path]::GetPathRoot([Environment]::GetFolderpath("System"));

# C:\Landorphan.Abstractions.Test.UnitTestTarget
[string]$rootTestFolder = Join-Path -Path $rootSystem -ChildPath "Landorphan.Abstractions.Test.UnitTestTarget"
if (Test-Path $rootTestFolder)
{
  # recursively delete the root test folder
  Remove-Item -Recurse -Force $rootTestFolder
}

$rootShareName = Split-Path $rootTestFolder -NoQualifier
$rootShareName = $rootShareName.Substring(1, $rootShareName.Length -1)
$fullShareName = "\\localHost\" + $rootShareName

if(Test-Path $fullShareName)
{
  Remove-SmbShare -Name $rootShareName -Force
}
