Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

$started = [DateTime]::UtcNow

 & .\Build-Release.ps1

# assumes vstest.console.exe is in the path environment variable ($Env:Path)
# On my machine, the path is: C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe

$scriptDirectory = Split-Path $script:MyInvocation.MyCommand.Path
$LandorphanIocTests = Join-Path $scriptDirectory \bin\release\Landorphan.Ioc.ServiceLocation.Tests\netcoreapp2.2\Landorphan.Ioc.ServiceLocation.Tests.dll
if(!(Test-Path $LandorphanIocTests))
{
  Write-Error "Could not find IOC Service Location Tests at: $LandorphanIocTests"
}
$LandorphanIocTestabilityTests = Join-Path $scriptDirectory \bin\release\Landorphan.Ioc.ServiceLocation.Testability.Tests\netcoreapp2.2\Landorphan.Ioc.ServiceLocation.Testability.Tests.dll
if(!(Test-Path $LandorphanIocTestabilityTests))
{
  Write-Error "Could not find IOC Service Location Testability Tests at: $LandorphanIocTestabilityTests"
}
$LandorphanTestUtilitiesMSTestTests = Join-Path $scriptDirectory \bin\release\Landorphan.TestUtilities.MSTest.Tests\netcoreapp2.2\Landorphan.TestUtilities.MSTest.Tests.dll
if(!(Test-Path $LandorphanTestUtilitiesMSTestTests))
{
  Write-Error "Could not find TestUtilities Tests at: $LandorphanIocTests"
}
$LandorphanAbstractionsTests = Join-Path $scriptDirectory \bin\release\Landorphan.Abstractions.Tests\netcoreapp2.2\Landorphan.Abstractions.Tests.dll
if(!(Test-Path $LandorphanAbstractionsTests))
{
  Write-Error "Could not find TestUtilities Tests at: $LandorphanAbstractionsTests"
}

$results = Join-Path $scriptDirectory TestResults

# TODO: switch to dotnet test implementation
# TODO: figure out while the trx file is not being written
vstest.console.exe $LandorphanIocTests, $LandorphanIocTestabilityTests, $LandorphanTestUtilitiesMSTestTests $LandorphanAbstractionsTests /logger:trx /ResultsDirectory:$results /Parallel /TestCaseFilter:"(TestCategory=Check-In|Check-In-Non-Ide)"

$completed = [DateTime]::UtcNow
$elapsed = $completed - $started
"Test-Release:"
"Elapsed:=        $elapsed"
"Started (UTC):=  $started"
"Completed (UTC):=$completed"
