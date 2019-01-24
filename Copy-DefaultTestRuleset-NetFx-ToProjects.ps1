Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

Copy-Item -Path .\build\BuildFiles\Default.Test.NetFx.FxCop.15.0.WithSonarLint.ruleset -Destination .\test\Ioc.Collections.Performance.Tests\Ioc.Collections.Performance.Tests.NetFx.ruleset
