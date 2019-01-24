Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

Copy-Item -Path .\build\BuildFiles\Default.Production.NetStd.FxCop.15.0.WithSonarLint.ruleset -Destination .\source\Landorphan.Ioc.ServiceLocation\Landorphan.Ioc.ServiceLocation.NetStd.ruleset
Copy-Item -Path .\build\BuildFiles\Default.Production.NetStd.FxCop.15.0.WithSonarLint.ruleset -Destination .\source\Landorphan.Ioc.ServiceLocation.Testability\Landorphan.Ioc.ServiceLocation.Testability.NetStd.ruleset
Copy-Item -Path .\build\BuildFiles\Default.Production.NetStd.FxCop.15.0.WithSonarLint.ruleset -Destination .\source\Landorphan.TestUtilities.MSTest\Landorphan.TestUtilities.MSTest.NetStd.ruleset
