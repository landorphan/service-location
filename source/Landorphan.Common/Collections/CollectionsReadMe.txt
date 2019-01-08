HashSet<T>
    Contains only works on elements whose GetHashCode algorithm matches that of the HashSet<T> instance's IEqualityComparer<T>.GetHashCode implementation
        when elements are mutated into equivalence.  Rebuild the set to fix.  (TODO: Check mutable dictionary keys, done, behavior is the same.).
    uniqueness is enforced on add, mutable elements allow the contract to be defeated

SortedSet<T>
    uniqueness is enforced on the sort position only
    uniqueness is enforced on add, mutable elements allow the contract to be defeated

