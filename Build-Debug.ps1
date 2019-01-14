Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

$started = [DateTime]::UtcNow

& .\Build-Clean.ps1

dotnet build Landorphan.sln -c debug

$completed = [DateTime]::UtcNow
$elapsed = $completed - $started
"Build-Debug:"
"Elapsed:=        $elapsed"
"Started (UTC):=  $started"
"Completed (UTC):=$completed"
