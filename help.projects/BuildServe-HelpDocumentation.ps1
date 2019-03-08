Set-StrictMode -Version Latest
$ErrorActionPreference = 'Continue'

# assumes docfx is installed and in the %PATH%

docfx docfx_project/docfx.json
docfx serve docfx_project/_site
