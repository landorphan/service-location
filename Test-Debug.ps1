Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

$started = [DateTime]::UtcNow

& .\Build-Debug.ps1

# assumes vstest.console.exe is in the path environment variable ($Env:Path)
# On my machine, the path is: C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe

$scriptDirectory = Split-Path $script:MyInvocation.MyCommand.Path
$LandorphanIocTests = Join-Path $scriptDirectory \bin\debug\Landorphan.Ioc.Tests\netcoreapp2.2\Landorphan.Ioc.Tests.dll
$LandorphanTestUtilitiesMSTestTests = Join-Path $scriptDirectory \bin\debug\Landorphan.TestUtilities.MSTest.Tests\netcoreapp2.2\Landorphan.TestUtilities.MSTest.dll
$results = Join-Path $scriptDirectory TestResults

# TODO: switch to dotnet test implementation
# TODO: figure out while the trx file is not being written
vstest.console.exe $LandorphanIocTests, $LandorphanTestUtilitiesMSTestTests /logger:trx /ResultsDirectory:$results /Parallel /TestCaseFilter:"TestCategory!=Nightly&TestCategory!=Manual&TestCategory!=IDE-Only"

$completed = [DateTime]::UtcNow
$elapsed = $completed - $started
"Test-Debug:"
"Elapsed:=        $elapsed"
"Started (UTC):=  $started"
"Completed (UTC):=$completed"
