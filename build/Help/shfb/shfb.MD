You can find the current version of SHFB here:  https://github.com/EWSoftware/SHFB

The guided install worked well for me, but did not detect the lack of MSBuild Tools 2015:  https://www.microsoft.com/en-us/download/confirmation.aspx?id=48159

The project structure does not work well with .Net Core, .Net Standard, nor Microsoft Nuget packages.  For example, member documentation that references
System.Collections.Immutable is removed.