namespace Landorphan.TestUtilities
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using Landorphan.Common;

    /// <summary>
    ///    Extension methods for <see cref="Type" /> instances.
    /// </summary>
    public static class TestTypeExtensions
    {
        /// <summary>
        /// Gets all constants declared in the given type.
        /// </summary>
        /// <param name="type">
        /// The type to inspected.
        /// </param>
        /// <returns>
        /// A non-null set of <see cref="FieldInfo" /> representing the constants.
        /// </returns>
        public static ImmutableHashSet<FieldInfo> GetPublicAndPrivateConstantsDeclaredOnly(this Type type)
        {
            type.ArgumentNotNull(nameof(type));

            var bindingFlags = BindingFlags.Public |
                               BindingFlags.NonPublic |
                               BindingFlags.Static |
                               BindingFlags.Instance |
                               BindingFlags.GetField;
            var fieldInfos = type.GetFields(bindingFlags).ToImmutableList();

            // IsLiteral determines if its value is written at compile time and not changeable
            // IsInitOnly determine if the field can be set in the body of the constructor
            // -> C# 
            //     for a readonly field, both would be true
            //     for a constant only IsLiteral will be true
            var rv = (from fi in fieldInfos where fi.IsLiteral && !fi.IsInitOnly select fi).ToImmutableHashSet();
            return rv;
        }

        /// <summary>
        /// Gets all constants declared in the given type and its ancestors.
        /// </summary>
        /// <param name="type">
        /// The type to inspected.
        /// </param>
        /// <returns>
        /// A non-null set of <see cref="FieldInfo" /> representing the constants.
        /// </returns>
        public static ImmutableHashSet<FieldInfo> GetPublicAndPrivateConstantsIncludingInherited(this Type type)
        {
            type.ArgumentNotNull(nameof(type));

            var bindingFlags = BindingFlags.Public |
                               BindingFlags.NonPublic |
                               BindingFlags.Static |
                               BindingFlags.GetField |
                               BindingFlags.FlattenHierarchy;
            var fieldInfos = type.GetFields(bindingFlags).ToImmutableList();

            // IsLiteral determines if its value is written at compile time and not changeable
            // IsInitOnly determine if the field can be set in the body of the constructor
            // -> C# 
            //     for a readonly field, both would be true
            //     for a constant only IsLiteral will be true
            var rv = (from fi in fieldInfos where fi.IsLiteral && !fi.IsInitOnly select fi).ToImmutableHashSet();
            return rv;
        }

        /// <summary>
        /// A Type extension method that queries if 'type' is nullable of t.
        /// </summary>
        /// <param name="type">
        /// The type to act on.
        /// </param>
        /// <returns>
        /// <c>true</c>if nullable{T}; otherwise <c>false</c>.
        /// </returns>
        public static bool IsNullableOfT(this Type type)
        {
            type.ArgumentNotNull(nameof(Type));
            return type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>));
        }

        /// <summary>
        /// A Type extension that determines if the given type is a member of the set of ordinal types.
        /// </summary>
        /// <param name="type">
        /// The type to inspected.
        /// </param>
        /// <returns>
        /// <c>true</c> if the type is ordinal; otherwise <c>false</c>.
        /// </returns>
        public static bool IsOrdinalType(this Type type)
        {
            var rv = false;

            if (typeof(byte) == type)
            {
                rv = true;
            }
            else if (typeof(sbyte) == type)
            {
                rv = true;
            }

            else if (typeof(short) == type)
            {
                rv = true;
            }
            else if (typeof(ushort) == type)
            {
                rv = true;
            }

            else if (typeof(int) == type)
            {
                rv = true;
            }
            else if (typeof(uint) == type)
            {
                rv = true;
            }

            else if (typeof(long) == type)
            {
                rv = true;
            }
            else if (typeof(ulong) == type)
            {
                rv = true;
            }

            return rv;
        }
    }
}
