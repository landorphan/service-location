## Build scripts:

| Script                      | Description                                                                       |
|:--------------------------- |:--------------------------------------------------------------------------------- |
| Build-clean.ps1		         | Attempts to remove all output files and folders                                    |
| Build-Debug.ps1             | Cleans the output, then builds the debug version of the solution                  |
| Build-Release.ps1           | Cleans the output, then builds the release version of the solution                |
|                             |                                                                                   |
| Test-Debug.ps1              | Cleans the output, builds the debug version of the solution and executes tests.   |
| Test-Release.ps1            | Cleans the output, builds the release version of the solution and executes tests. |


## TODO:  
dotnet builds of .Net Framework projects are failing, switch to MSBuild directly (not through dotnet build)

WBN:  Choice of Runners, DEV Workstation versus Build Server for test scripts

