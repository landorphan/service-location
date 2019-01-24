Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

Copy-Item -Path .\build\BuildFiles\Default.Test.NetCore.FxCop.15.0.WithSonarLint.ruleset -Destination .\test\Landorphan.Ioc.Tests\Landorphan.Ioc.Tests.NetCore.ruleset
Copy-Item -Path .\build\BuildFiles\Default.Test.NetCore.FxCop.15.0.WithSonarLint.ruleset -Destination .\test\Landorphan.TestUtilities.MSTest.Tests\Landorphan.TestUtilities.MSTest.Tests.NetCore.ruleset
