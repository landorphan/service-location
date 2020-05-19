// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1050: Declare types in namespaces", Justification = "External Code", Scope = "type", Target = "~T:MSTestAssemblyHooks")]
[assembly: SuppressMessage("Microsoft.Design", "CA1052: Static holder types should be Static or NotInheritable", Justification = "External Code", Scope = "type", Target = "~T:MSTestAssemblyHooks")]
[assembly: SuppressMessage("Sonar.CodeSmell", "S1118: Utility classes should not have public constructors", Justification = "External Code", Scope = "type", Target = "~T:MSTestAssemblyHooks")]
[assembly: SuppressMessage("Sonar.Bug", "S3903: Types should be defined in named namespaces", Justification = "External Code", Scope = "type", Target = "~T:MSTestAssemblyHooks")]
