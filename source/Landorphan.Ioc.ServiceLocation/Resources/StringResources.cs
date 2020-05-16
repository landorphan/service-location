﻿namespace Landorphan.Ioc.Resources
{
    // interim replacement for a string resource file until I understand why incremental builds are defeated by the presence of a resource file.

   internal static class StringResources
   {
       internal const string AbstractAssemblyRegistrarExceptionDefaltMessageFmt =
         "Service location assemblies must not have abstract implementations of IAssemblyRegistrar.  The type ('{0}') is abstract.";

       internal const string ArgumentExceptionNoParamNameSuffix = "";

       internal const string ArgumentExceptionWithParamNameSuffixFmt = "\r\nParameter Name: {0}";

       internal const string AssemblyRegistrarMustHavePublicDefaultConstructorExceptionDefaultMessageFmt =
         "Types that implement IAssemblyRegistrar must have a public default (parameterless) constructor.  The type ('{0}') does not.";

       internal const string CannotPrecludeRegisteredTypeArgumentExceptionDefaultMessageFmt =
         "Cannot precluded a registered type '{0}'.{1}";

       internal const string ContainerConfigurationNamedImplementationsDisabledExceptionDefaultMessageFmt =
         "The container ({0}:{1}) dissallows the use of named immplentations or instance.  This setting is configured in the container.Manager.Configuration";

       internal const string ContainerConfigurationPrecludedTypesDisabledExceptionDefaultMessageFmt =
         "The container ({0}:{1}) dissallows the preclusion of types.  This setting is configured in the container.Manager.Configuration.";

       internal const string ContainerTypeNameAlreadyRegisteredArgumentExceptionFmt =
         "The container ({0}:{1}) has already registered the type '{2}' under the name '{3}' ('' represents the default instance).{4}";

       internal const string ContainerTypePrecludedArgumentExceptionFmt = "The container ({0}:{1}) precludes the registration of type '{2}'.{3}";

       internal const string FromTypeMustBeInterfaceOrAbstractTypeArgumentExceptionNameTypeFmt =
         "Landorphan.Ioc service location only supports the location of interfaces and abstract types.  '{0}' is neither an interface nor an abstract type.{1}";

       internal const string InstanceMustImplementTypeArgumentExceptionDefaultMessageFmt =
         "The instance is required to implement the interface or abstract type '{0}' but is of type '{1}' which does not.{2}";

       internal const string IocContainerConfigurationLockedExceptionDefaltMessageFmt =
         "The container ({0}:{1}) has a locked configuration.  Attempting to edit the configuration causes this exception.";

       internal const string MultipleAssemblyRegistrarDetectedFmt =
         "Service location assemblies must not have more than one implementation of IAssemblyRegistrar.  The assembly {0}, Version={1} has more than one including {2}.";

       internal const string NullReplacementValue = "null";

       internal const string PublicKey = "0024000004800000140100000602000000240000525341310008000001000100c33ba254dba2d79dba4c1fe136efb36588f73cc16afd66a43aebf7489ac82ab66c2fff42d8503459eff95139e0" +
                                         "ff790b6688513b19edf991760ef7ad02740a296c3ce5d098586b236b6abaa75038eb03af7d643d9a84b47bca0ed1cabbf4de9605c76007aab2e3abe7633d90860648ab1e38035fda5943971c64" +
                                         "71249a0bd80f2ab04686c2110ebb10f909fb773e3d87f67fb1e2f33ee791e4d8284fe9c6848ea81b3cf6a081df100716a10c68e0dd5219ff1657995777bf03961afdc3c09b040edb5a36baab50" +
                                         "75410507f3ba1d9f59c6bd67401819abbbe7712b5f473e052b96efe98c39210bb485c1ba0489ed396983beb914a3b2443f6aa2be4f49a88bb0";

       internal const string RegisteredTypeIsNullCannotLazilyCreate = "Registered Implementation type is null, cannot lazily create";

       internal const string ResolutionIocExceptionDefaultMessageFmt = "Unable to resolve the requested type/name: {0}/'{1}'. ('' indicates a default registration).";

       internal const string ToTypeMustHavePublicDefaultConstructorArgumentExceptionDefaultMessageFmt =
         "Registered implementation types must have a public default constructor. '{0}' does not have a public default (parameterless) constructor.  Either add such a constructor, " +
         "or instantiate an instance and use a RegisterInstance method instead of the RegisterImplementation methods.{1}";

       internal const string ToTypeMustImplementTypeArgumentExceptionDefaultMessageFmt =
         "The interface or abstract type '{0}' must be implemented by '{1}' but '{1}' does meet this requirement.{2}";

       internal const string ToTypeMustNotBeAbstractOrInterfaceArgumentExceptionDefaultMessageFmt =
         "The given type '{0}' must not be an abstract or interface type, but is such a type.{1}";

       internal const string TypeMustNotBeOpenGenericDefaultMessageFmt =
         "The given type '{0}' must not be an open generic, but is an open generic (e.g. IList<T>).  " +
         "Replace with a closed type (e.g. IList<Int32>).{1}";
   }
}
