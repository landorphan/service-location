Key Implementation: Landorphan.Ioc.ServiceLocation.Internal.IocContainer

Data:
   1 "interface"
      0 to many names
         1 implementation type XOR instance per type/name pair

Sample Implementation:
   private IImmutableDictionary<RegistrationKeyTypeNamePair, RegistrationValueTypeInstancePair> _registrations;
      
   _registrations.TryGetValue(key, out var value))


other collection:

_precludedTypes = ImmutableHashSet<Type>.Empty;

_precludedTypes.Contains(fromType)