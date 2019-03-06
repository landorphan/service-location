namespace Landorphan.Abstractions.Tests.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class WindowsTestOnlyAttribute : Attribute
    {
    }
}
