namespace Landorphan.Ioc.ServiceLocation.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Landorphan.Common;
    using Landorphan.Ioc.ServiceLocation.Exceptions;
    using Landorphan.Ioc.ServiceLocation.Interfaces;

    internal sealed class AssemblyRegistrarRepository
    {
        // TODO: rewrite this like RegistrationKeyTypeNamePair
        // This class was written before System.Collections.Immutable.
        // Should this be a set?  A sorted set?
        private IImmutableList<AssemblyRegistrarRecord> _assemblyRegistrarRecords = ImmutableList<AssemblyRegistrarRecord>.Empty;

        internal IImmutableList<IAssemblySelfRegistration> AssemblyRegistrarInstances
        {
            get
            {
                var snapshot = _assemblyRegistrarRecords;
                IImmutableList<IAssemblySelfRegistration> rv = (
                    from r in snapshot
                    select r.Instance).ToImmutableList();
                return rv;
            }
        }

        internal IImmutableList<Type> AssemblyRegistrarTypes
        {
            get
            {
                var snapshot = _assemblyRegistrarRecords;
                IImmutableList<Type> rv = (
                    from r in snapshot
                    select r.AssemblyRegistrarType).ToImmutableList();
                return rv;
            }
        }

        internal bool AreNewAssemblyRegistrarsLoaded()
        {
            // Determine if there are any loaded assemblies referencing this assembly that have an implementation of IAssemblyRegistrar.
            var currentAssemblyRegistrarTypes = new HashSet<Type>(GetLoadedAssemblyRegistrarTypesUnvalidated());
            var knownAssemblyRegistrarTypes = AssemblyRegistrarTypes;

            // remove all matching entries from current
            currentAssemblyRegistrarTypes.ExceptWith(knownAssemblyRegistrarTypes);

            // if any remain, there are new assemblies present with IAssemblyRegistrar implementations.
            var rv = currentAssemblyRegistrarTypes.Any();
            return rv;
        }

        internal IImmutableList<IAssemblySelfRegistration> RefreshAssemblyRegistrars()
        {
            var ordered = GetLoadedAssemblyRegistrarTypesOrdered();
            var recBuilder = ImmutableList<AssemblyRegistrarRecord>.Empty.ToBuilder();
            foreach (var mr in ordered)
            {
                var instance = (IAssemblySelfRegistration)Activator.CreateInstance(mr);
                var rec = new AssemblyRegistrarRecord(instance);
                recBuilder.Add(rec);
            }

            var was = _assemblyRegistrarRecords;
            _assemblyRegistrarRecords = recBuilder.ToImmutable();
            var newSelfRegistrationInstances = (
                from arr in _assemblyRegistrarRecords.Except(was)
                select arr.Instance).ToImmutableList();
            return newSelfRegistrationInstances;
        }

        private static IList<Type> GetTypesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            var rv = new List<Type>();
            foreach (var assembly in assemblies)
            {
                rv.AddRange(assembly.SafeGetTypes());
            }

            return rv;
        }

        private IList<Assembly> GetLoadedAssembliesReferencingThisAssembly()
        {
            var thisAssembly = GetType().Assembly;
            var thisAssemblyName = thisAssembly.GetName();

            var allLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblyNameEqualityComparer = new AssemblyNameEqualityComparer();
            var rv = (
                from a in allLoadedAssemblies
                where a.GetReferencedAssemblies().Contains(thisAssemblyName, assemblyNameEqualityComparer)
                select a).ToList();

            // No need to add this module, it does not have an IAssemblyRegistrar implementation.
            // (If this assembly is collapsed into another assembly, that may change).
            return rv;
        }

        private IList<Type> GetLoadedAssemblyRegistrarTypesOrdered()
        {
            // order the registrars from dependency assemblies (first) to dependent assemblies (last).
            var unordered = new Queue<Type>(GetLoadedAssemblyRegistrarTypesValidated());

            var assemblyNameEqualityComparer = new AssemblyNameEqualityComparer();
            var rv = new List<Type>();

            // add the internal registrar.
            rv.Add(typeof(AssemblySelfRegistration));

            while (unordered.Count > 0)
            {
                var currentAssemblyRegistrarType = unordered.Dequeue();
                var add = true;
                foreach (var t in unordered)
                {
                    if (currentAssemblyRegistrarType.Assembly.GetReferencedAssemblies().Contains(t.Assembly.GetName(), assemblyNameEqualityComparer))
                    {
                        add = false;
                        unordered.Enqueue(currentAssemblyRegistrarType);
                        break;
                    }
                }

                if (add)
                {
                    rv.Add(currentAssemblyRegistrarType);
                }
            }

            return rv;
        }

        private IList<Type> GetLoadedAssemblyRegistrarTypesUnvalidated()
        {
            var loadedAssembliesReferencingThisAssembly = GetLoadedAssembliesReferencingThisAssembly();
            var typesInLoadedAssemblies = GetTypesFromAssemblies(loadedAssembliesReferencingThisAssembly);

            // Find all types that implement IAssemblyRegistrar
            var rv = new List<Type>();
            foreach (var currentType in typesInLoadedAssemblies)
            {
                if (currentType.IsInterface)
                {
                    continue;
                }

                if (typeof(IAssemblySelfRegistration).IsAssignableFrom(currentType))
                {
                    // no business rules applied
                    rv.Add(currentType);
                }
            }

            return rv;
        }

        private IList<Type> GetLoadedAssemblyRegistrarTypesValidated()
        {
            // Ensure the following:
            //      No implementations of IAssemblyRegistrar are abstract.
            //      All implementations have a public default constructor.
            //      No assembly has more than one implementation of IAssemblyRegistrar.
            var rv = GetLoadedAssemblyRegistrarTypesUnvalidated();
            var working = new Dictionary<AssemblyName, HashSet<Type>>(new AssemblyNameEqualityComparer());
            foreach (var registrarType in rv)
            {
                if (registrarType.IsAbstract)
                {
                    throw new AbstractAssemblyRegistrarException(registrarType);
                }

#pragma warning disable CA1825 // Avoid zero-length array allocations.
                if (ReferenceEquals(registrarType.GetConstructor(new Type[0]), null))
#pragma warning restore CA1825 // Avoid zero-length array allocations.
                {
                    throw new AssemblyRegistrarMustHavePublicDefaultConstructorException(registrarType);
                }

                var assemblyName = registrarType.Assembly.GetName();
                if (!working.ContainsKey(assemblyName))
                {
                    working.Add(assemblyName, new HashSet<Type>());
                }

                working[assemblyName].Add(registrarType);

                if (working[assemblyName].Count > 1)
                {
                    throw new MultipleAssemblyRegistrarException(working[assemblyName]);
                }
            }

            return rv;
        }

        [SuppressMessage(
            "SonarLint.CodeSmell",
            "S1210: 'Equals' and the comparison operators should be overridden when implementing 'IComparable'",
            Justification = "Private nested class, operators not used/needed (MWP)")]
        private sealed class AssemblyRegistrarRecord : ICloneable, IComparable, IComparable<AssemblyRegistrarRecord>, IEquatable<AssemblyRegistrarRecord>
        {
            // Identity consists solely of the AssemblyRegistrarType, Instance values not considered.

            internal AssemblyRegistrarRecord(IAssemblySelfRegistration instance)
            {
                instance.ArgumentNotNull("instance");

                AssemblyRegistrarType = instance.GetType();
                Instance = instance;
            }

            // ReSharper disable once MemberCanBePrivate.Local
            internal AssemblyRegistrarRecord(AssemblyRegistrarRecord other)
            {
                other.ArgumentNotNull(nameof(other));

                AssemblyRegistrarType = other.AssemblyRegistrarType;
                Instance = other.Instance;
            }

            /// <inheritdoc/>
            public object Clone()
            {
                return new AssemblyRegistrarRecord(this);
            }

            internal Type AssemblyRegistrarType { get; }

            internal IAssemblySelfRegistration Instance { get; }

            /// <inheritdoc/>
            int IComparable.CompareTo(object obj)
            {
                if (ReferenceEquals(obj, null))
                {
                    // this instance is greater than null.
                    return 1;
                }

                if (obj is AssemblyRegistrarRecord assemblyRegistrarRecord)
                {
                    return CompareTo(assemblyRegistrarRecord);
                }

                throw new ArgumentException($"'{nameof(obj)}' must be of type {GetType().FullName}.", nameof(obj));
            }

            /// <inheritdoc/>
            public int CompareTo(AssemblyRegistrarRecord other)
            {
                if (other == null)
                {
                    // this instance is greater than null.
                    return 1;
                }

                var thisTypeFullName = AssemblyRegistrarType == null ? string.Empty : AssemblyRegistrarType.FullName;
                var otherTypeFullName = other.AssemblyRegistrarType == null ? string.Empty : other.AssemblyRegistrarType.FullName;
                var rv = string.Compare(thisTypeFullName, otherTypeFullName, StringComparison.Ordinal);

                return rv;
            }

            /// <inheritdoc/>
            public bool Equals(AssemblyRegistrarRecord other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                return AssemblyRegistrarType == other.AssemblyRegistrarType;
            }

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                return obj is AssemblyRegistrarRecord record && Equals(record);
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return AssemblyRegistrarType != null ? AssemblyRegistrarType.GetHashCode() : 0;
            }
        }
    }
}
