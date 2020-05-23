namespace Landorphan.Ioc.ServiceLocation.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Landorphan.Common;
    //[SuppressMessage("Microsoft.?", "IDE0048: add parentheses for clarity")]
    [SuppressMessage(
        "SonarLint.CodeSmell",
        "S1067: Expressions should not be too complex",
        Justification = "Thanks for sharing, I've wrapped this up in a implementation so it never need be done again (MWP)")]
    internal class AssemblyNameEqualityComparer : IEqualityComparer<AssemblyName>
    {
        public bool Equals(AssemblyName x, AssemblyName y)
        {
            if (x == null)
            {
                return y == null;
            }

            if (y == null)
            {
                return false;
            }

            if (x.Name.Equals(y.Name, StringComparison.Ordinal) &&
                x.Version.Equals(y.Version) &&
                x.CultureInfo.Equals(y.CultureInfo) &&
                (ReferenceEquals(x.KeyPair, y.KeyPair) ||
                 x.KeyPair.IsNotNull() &&
                 y.KeyPair.IsNotNull() &&
                 x.KeyPair.PublicKey.SequenceEqual(y.KeyPair.PublicKey)
                ))
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(AssemblyName obj)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (obj != null)
            {
                return obj.GetHashCode();
            }

            return 0;
        }
    }
}
