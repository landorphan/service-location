#Requires -Version 5.1
#Requires -RunAsAdministrator

#TODO: consider renaming Build- verbs to Set- verbs, makes the PS Lint tools happy, Build is an unrecognized verb.

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

# Test for elevated status:
$identity = [Security.Principal.WindowsIdentity]::GetCurrent()
$principal = New-Object Security.Principal.WindowsPrincipal -ArgumentList $identity
if (!$principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator))
{
  Write-Error "This script requires elevated privileges."
  return
}

# WARNING: when something is not quite correct with ACLs, the operation silently fails (no warning, no exception)

<# *************************************************************************************************************************
Create the following folder\file structure: (C:\ is not hard-coded, but typical)
  C:\Landorphan.Abstractions.Test.UnitTestTarget
  C:\Landorphan.Abstractions.Test.UnitTestTarget\EveryoneFullControl
  C:\Landorphan.Abstractions.Test.UnitTestTarget\EveryoneFullControl\OwnerOnly.txt
  C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions
  C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\OuterExtantFile.txt
  C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\InnerNoPermissions
  C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\InnerNoPermissions\InnerExtantFile.txt
  C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\ReadExecuteListFolderContents
  C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\ReadExecuteListFolderContents\ExtantFolder
  C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\ReadExecuteListFolderContents\ExtantFile.txt
************************************************************************************************************************* #>

# [Enum]::GetNames([System.Security.AccessControl.FileSystemRights])
# AppendData                    ListDirectory           Synchronize
# ChangePermissions             Modify                  TakeOwnership
# CreateDirectories             Read                    Traverse
# CreateFiles                   ReadAndExecute          Write
# Delete                        ReadAttributes          WriteAttributes
# DeleteSubdirectoriesAndFiles  ReadData                WriteData
# ExecuteFile                   ReadExtendedAttributes  WriteExtendedAttributes
# FullControl                   ReadPermissions

# ╔═════════════╦═════════════╦═══════════════════════════════╦════════════════════════╦══════════════════╦═══════════════════════╦═════════════╦═════════════╗
# ║             ║ folder only ║ folder, sub-folders and files ║ folder and sub-folders ║ folder and files ║ sub-folders and files ║ sub-folders ║    files    ║
# ╠═════════════╬═════════════╬═══════════════════════════════╬════════════════════════╬══════════════════╬═══════════════════════╬═════════════╬═════════════╣
# ║ Inheritance ║ none        ║ Container|Object              ║ Container              ║ Object           ║ Container|Object      ║ Container   ║ Object      ║
# ║ Propagation ║ none        ║ none                          ║ none                   ║ none             ║ InheritOnly           ║ InheritOnly ║ InheritOnly ║
# ╚═════════════╩═════════════╩═══════════════════════════════╩════════════════════════╩══════════════════╩═══════════════════════╩═════════════╩═════════════╝

function Build-Root
{
  param([string]$path)
  [System.Security.AccessControl.DirectorySecurity]$security = Get-Acl -Path $path
  [System.Security.AccessControl.AuthorizationRuleCollection]$ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    if ($EVERYONE_SID -eq $rule.IdentityReference)
    {
      $security.RemoveAccessRule($accessRule) > $null
      break
    }
  }
}

function Build-RootShare
{
  param([string]$name,[string]$path)
  # New-SMBShare –Name $name –Path $path –ContinuouslyAvailable –FullAccess 'Everyone' -ChangeAccess domain\deptusers -ReadAccess “domain\authenticated users”
  New-SmbShare –Name $name –Path $path –FullAccess 'Everyone' > $null
}

function Build-Root-EveryoneFullControl
{
  param([string]$path)
  [System.Security.AccessControl.DirectorySecurity]$security = Get-Acl -Path $path
  $ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    if ($EVERYONE_SID -eq $rule.IdentityReference)
    {
      $security.RemoveAccessRule($accessRule) > $null
      break
    }
  }
  $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule ('EVERYONE','FullControl','ContainerInherit, ObjectInherit','None','Allow')
  $security.AddAccessRule($accessRule)
  Set-Acl -Path $path -AclObject $security
}

function Build-Root-EveryoneFullControl-OwnerOnlyFile
{
  param([string]$path)
  [System.Security.AccessControl.FileSecurity]$security = Get-Acl -Path $path
  # Remove the inheritance but copy the ACEs
  $security.SetAccessRuleProtection($True,$True)
  Set-Acl -Path $path -AclObject $security
  # Refresh the ACLs
  $security = Get-Acl -Path $path
  $ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    # leave admins, system, and the like alone, remove permisions for current user, everyone, users, and authenticated users
    $bracedSid = "{" + $rule.IdentityReference.Value + "}"
    if ($CURRENT_USER_SID -eq $bracedSid -or $EVERYONE_SID -eq $bracedSid -or $USERS_SID -eq $bracedSid -or $AUTHENTICATED_USERS_SID -eq $bracedSid)
    {
      $security.RemoveAccessRule($rule) > $null
    }
  }
  $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule ("$Env:USERDOMAIN\$Env:USERNAME",'FullControl','None','None','Allow')
  $security.AddAccessRule($accessRule)
  Set-Acl -Path $path -AclObject $security
}

function Build-Root-OuterNoPermissions
{
  param([string]$path)
  [System.Security.AccessControl.DirectorySecurity]$security = Get-Acl -Path $path
  # Remove the inheritance but copy the ACEs
  $security.SetAccessRuleProtection($True,$True)
  Set-Acl -Path $path -AclObject $security
  # Refresh the ACLs
  $security = Get-Acl -Path $path
  $ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    # leave admins, system, and the like alone, remove permisions for current user, everyone, users, and authenticated users
    $bracedSid = "{" + $rule.IdentityReference.Value + "}"
    if ($CURRENT_USER_SID -eq $bracedSid -or $EVERYONE_SID -eq $bracedSid -or $USERS_SID -eq $bracedSid -or $AUTHENTICATED_USERS_SID -eq $bracedSid)
    {
      $security.RemoveAccessRule($rule) > $null
    }
  }
  Set-Acl -Path $path -AclObject $security
}

function Build-Root-OuterNoPermissions-ExtantFile
{
  param([string]$path)
  [System.Security.AccessControl.FileSecurity]$security = Get-Acl -Path $path
  # Remove the inheritance but copy the ACEs
  $security.SetAccessRuleProtection($True,$True)
  Set-Acl -Path $path -AclObject $security
  # Refresh the ACLs
  $security = Get-Acl -Path $path
  $ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    # leave admins, system, and the like alone, remove permisions for current user, everyone, users, and authenticated users
    $bracedSid = "{" + $rule.IdentityReference.Value + "}"
    if ($CURRENT_USER_SID -eq $bracedSid -or $EVERYONE_SID -eq $bracedSid -or $USERS_SID -eq $bracedSid -or $AUTHENTICATED_USERS_SID -eq $bracedSid)
    {
      $security.RemoveAccessRule($rule) > $null
    }
  }
  Set-Acl -Path $path -AclObject $security
}

function Build-Root-OuterNoPermissions-InnerNoPermissions
{
  param([string]$path)
  [System.Security.AccessControl.DirectorySecurity]$security = Get-Acl -Path $path
  # Remove the inheritance but copy the ACEs
  $security.SetAccessRuleProtection($True,$True)
  Set-Acl -Path $path -AclObject $security
  # Refresh the ACLs
  $security = Get-Acl -Path $path
  $ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    # leave admins, system, and the like alone, remove permisions for current user, everyone, users, and authenticated users
    $bracedSid = "{" + $rule.IdentityReference.Value + "}"
    if ($CURRENT_USER_SID -eq $bracedSid -or $EVERYONE_SID -eq $bracedSid -or $USERS_SID -eq $bracedSid -or $AUTHENTICATED_USERS_SID -eq $bracedSid)
    {
      $security.RemoveAccessRule($rule) > $null
    }
  }
  Set-Acl -Path $path -AclObject $security
}

function Build-Root-OuterNoPermissions-InnerNoPermissions-ExtantFile
{
  param([string]$path)
  [System.Security.AccessControl.FileSecurity]$security = Get-Acl -Path $path
  # Remove the inheritance but copy the ACEs
  $security.SetAccessRuleProtection($True,$True)
  Set-Acl -Path $path -AclObject $security
  # Refresh the ACLs
  $security = Get-Acl -Path $path
  $ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    # leave admins, system, and the like alone, remove permisions for current user, everyone, users, and authenticated users, remove permisions for current user, everyone, users, and authenticated users
    $bracedSid = "{" + $rule.IdentityReference.Value + "}"
    if ($CURRENT_USER_SID -eq $bracedSid -or $EVERYONE_SID -eq $bracedSid -or $USERS_SID -eq $bracedSid -or $AUTHENTICATED_USERS_SID -eq $bracedSid)
    {
      $security.RemoveAccessRule($rule) > $null
    }
  }
  Set-Acl -Path $path -AclObject $security
}

function Build-Root-OuterNoPermissions-ReadExecute
{
  param([string]$path)
  [System.Security.AccessControl.DirectorySecurity]$security = Get-Acl -Path $path
  # Remove the inheritance but copy the ACEs
  $security.SetAccessRuleProtection($True,$True)
  Set-Acl -Path $path -AclObject $security
  # Refresh the ACLs
  $security = Get-Acl -Path $path
  $ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    # leave admins, system, and the like alone, remove permisions for current user, everyone, users, and authenticated users
    $bracedSid = "{" + $rule.IdentityReference.Value + "}"
    if ($CURRENT_USER_SID -eq $bracedSid -or $EVERYONE_SID -eq $bracedSid -or $USERS_SID -eq $bracedSid -or $AUTHENTICATED_USERS_SID -eq $bracedSid)
    {
      $security.RemoveAccessRule($rule) > $null
    }
  }
  $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule ('EVERYONE','ReadAndExecute,ListDirectory,ExecuteFile','ContainerInherit, ObjectInherit','None','Allow')
  $security.AddAccessRule($accessRule)
  Set-Acl -Path $path -AclObject $security
}

function Build-Root-OuterNoPermissions-ReadExecute-ExtantFile
{
  param([string]$path)
  [System.Security.AccessControl.FileSecurity]$security = Get-Acl -Path $path
  # Remove the inheritance but copy the ACEs
  $security.SetAccessRuleProtection($True,$True)
  Set-Acl -Path $path -AclObject $security
  # Refresh the ACLs
  $security = Get-Acl -Path $path
  $ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    # leave admins, system, and the like alone, remove permisions for current user, everyone, users, and authenticated users
    $bracedSid = "{" + $rule.IdentityReference.Value + "}"
    if ($CURRENT_USER_SID -eq $bracedSid -or $EVERYONE_SID -eq $bracedSid -or $USERS_SID -eq $bracedSid -or $AUTHENTICATED_USERS_SID -eq $bracedSid)
    {
      $security.RemoveAccessRule($rule) > $null
    }
  }
  $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule ('EVERYONE','FullControl','None','None','Allow')
  $security.AddAccessRule($accessRule)
  Set-Acl -Path $path -AclObject $security
}

function Build-Root-OuterNoPermissions-ReadExecute-ExtantFolder
{
  param([string]$path)
  [System.Security.AccessControl.DirectorySecurity]$security = Get-Acl -Path $path
  # Remove the inheritance but copy the ACEs
  $security.SetAccessRuleProtection($True,$True)
  Set-Acl -Path $path -AclObject $security
  # Refresh the ACLs
  $security = Get-Acl -Path $path
  $ruleCollection = $security.GetAccessRules($INCLUDE_EXPLICIT,$EXCLUDE_INHERITED,$RULES_SID_RESULT)
  foreach ($rule in $ruleCollection)
  {
    # leave admins, system, and the like alone, remove permisions for current user, everyone, users, and authenticated users
    $bracedSid = "{" + $rule.IdentityReference.Value + "}"
    if ($CURRENT_USER_SID -eq $bracedSid -or $EVERYONE_SID -eq $bracedSid -or $USERS_SID -eq $bracedSid -or $AUTHENTICATED_USERS_SID -eq $bracedSid)
    {
      $security.RemoveAccessRule($rule) > $null
    }
  }
  $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule ('EVERYONE','FullControl','ContainerInherit, ObjectInherit','None','Allow')
  $security.AddAccessRule($accessRule)
  Set-Acl -Path $path -AclObject $security
}

$myID = New-Object System.Security.Principal.NTAccount ("$Env:USERDOMAIN\$Env:USERNAME")
$currentSid = $MyID.Translate([System.Security.Principal.SecurityIdentifier]).ToString()
New-Variable -Name CURRENT_USER_SID -Value $currentSid -Option Constant
Remove-Variable -Name myId
Remove-Variable -Name currentSid

#New-Variable -Name ADMINISTRATORS_SID -Value '{S-1-5-32-544}' -Option Constant
New-Variable -Name AUTHENTICATED_USERS_SID -Value '{S-1-5-11}' -Option Constant
New-Variable -Name EVERYONE_SID -Value '{S-1-1-0}' -Option Constant
#New-Variable -Name LOCAL_SYSTEM_SID -Value '{S-1-5-18}' -Option Constant
New-Variable -Name USERS_SID -Value '{S-1-5-32-545}' -Option Constant

New-Variable -Name INCLUDE_EXPLICIT -Value $true -Option Constant
New-Variable -Name EXCLUDE_INHERITED -Value $false -Option Constant
#New-Variable -Name INCLUDE_INHERITED -Value $true -Option Constant
New-Variable -Name RULES_SID_RESULT -Value ([System.Type]::GetType('System.Security.Principal.SecurityIdentifier')) -Option Constant

# usually C:\
[string]$rootSystem = [System.IO.Path]::GetPathRoot([Environment]::GetFolderpath("System"));

# C:\Landorphan.Abstractions.Test.UnitTestTarget
[string]$rootTestFolder = New-Item -Path (Join-Path -Path $rootSystem -ChildPath "Landorphan.Abstractions.Test.UnitTestTarget") -ItemType Directory -Force
Build-Root $rootTestFolder
#  remove c:\ from the folder name and make that the share name.
$rootShareName = Split-Path $rootTestFolder -NoQualifier
$rootShareName = $rootShareName.Substring(1,$rootShareName.Length - 1)
Build-RootShare $rootShareName $rootTestFolder

# C:\Landorphan.Abstractions.Test.UnitTestTarget\EveryoneFullControl
[string]$rootEveryoneFullControlFolder = New-Item -Path (Join-Path -Path $rootTestFolder -ChildPath "EveryoneFullControl") -ItemType Directory -Force
Build-Root-EveryoneFullControl $rootEveryoneFullControlFolder

# C:\Landorphan.Abstractions.Test.UnitTestTarget\EveryoneFullControl\OwnerOnly.txt
[string]$rootEveryoneFullControlFolderOwnerOnlyFile = New-Item -Path (Join-Path $rootEveryoneFullControlFolder -ChildPath "OwnerOnly.txt") -ItemType File -Force
Build-Root-EveryoneFullControl-OwnerOnlyFile $rootEveryoneFullControlFolderOwnerOnlyFile

# C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions
[string]$outerNoPermissions = New-Item -Path (Join-Path -Path $rootTestFolder -ChildPath "OuterNoPermissions") -ItemType Directory -Force
Build-Root-OuterNoPermissions $outerNoPermissions

# C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\OuterExtantFile.txt
[string]$outerExtantFile = New-Item -Path (Join-Path -Path $outerNoPermissions -ChildPath "OuterExtantFile.txt") -ItemType File -Force
Build-Root-OuterNoPermissions-ExtantFile $outerExtantFile

# C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\InnerNoPermissions
[string]$outerInnerFolder = New-Item -Path (Join-Path -Path $outerNoPermissions -ChildPath "InnerNoPermissions") -ItemType Directory -Force
Build-Root-OuterNoPermissions-InnerNoPermissions $outerInnerFolder

# C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\InnerNoPermissions\InnerExistingFile.txt
[string]$innerExtantFile = New-Item -Path (Join-Path -Path $outerInnerFolder -ChildPath "InnerExtantFile.txt") -ItemType File -Force
Build-Root-OuterNoPermissions-InnerNoPermissions-ExtantFile $innerExtantFile

# C:\Landorphan.Abstractions.Test.UnitTestTarget\OuterNoPermissions\ReadExecuteListFolderContents
[string]$outerNoPermissionsReadExecuteFolder = New-Item -Path (Join-Path -Path $outerNoPermissions -ChildPath "ReadExecuteListFolderContents") -ItemType Directory -Force
Build-Root-OuterNoPermissions-ReadExecute $outerNoPermissionsReadExecuteFolder

[string]$outerNoPermissionsReadExecuteFolderExtantFile = New-Item -Path (Join-Path -Path $outerNoPermissionsReadExecuteFolder -ChildPath "ExtantFile.txt") -ItemType File -Force
Build-Root-OuterNoPermissions-ReadExecute-ExtantFile $outerNoPermissionsReadExecuteFolderExtantFile

[string]$outerNoPermissionsReadExecuteFolderExtantFolder = New-Item -Path (Join-Path -Path $outerNoPermissionsReadExecuteFolder -ChildPath "ExtantFolder") -ItemType Directory -Force
Build-Root-OuterNoPermissions-ReadExecute-ExtantFolder $outerNoPermissionsReadExecuteFolderExtantFolder
