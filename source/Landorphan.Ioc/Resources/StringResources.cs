namespace Landorphan.Ioc.Resources
{
   using System;

   // interim replacement for a string resource file until I understand why incremental builds are defeated by the presence of a resource file.

   internal static class StringResources
   {
      internal const String AbstractAssemblyRegistrarExceptionDefaltMessageFmt =
         "Service location assemblies must not have abstract implementations of IAssemblyRegistrar.  The type ('{0}') is abstract.";

      internal const String ArgumentExceptionNoParamNameSuffix = "";

      internal const String ArgumentExceptionWithParamNameSuffixFmt = "\r\nParameter Name: {0}";

      internal const String AssemblyRegistrarMustHavePublicDefaultConstructorExceptionDefaultMessageFmt =
         "Types that implement IAssemblyRegistrar must have a public default (parameterless) constructor.  The type ('{0}') does not.";

      internal const String CannotPrecludeRegisteredTypeArgumentExceptionDefaultMessageFmt =
         "Cannot precluded a registered type '{0}'.{1}";

      internal const String ContainerConfigurationNamedImplementationsDisabledExceptionDefaultMessageFmt =
         "The container ({0}:{1}) dissallows the use of named immplentations or instance.  This setting is configured in the container.Manager.Configuration";

      internal const String ContainerConfigurationPrecludedTypesDisabledExceptionDefaultMessageFmt =
         "The container ({0}:{1}) dissallows the preclusion of types.  This setting is configured in the container.Manager.Configuration.";

      internal const String ContainerTypeNameAlreadyRegisteredArgumentExceptionFmt =
         "The container ({0}:{1}) has already registered the type '{2}' under the name '{3}' ('' represents the default instance).{4}";

      internal const String ContainerTypePrecludedArgumentExceptionFmt = "The container ({0}:{1}) precludes the registration of type '{2}'.{3}";

      internal const String FromTypeMustBeInterfaceOrAbstractTypeArgumentExceptionNameTypeFmt =
         "Landorphan.Ioc service location only supports the location of interfaces and abstract types.  '{0}' is neither an interface nor an abstract type.{1}";

      internal const String InstanceMustImplementTypeArgumentExceptionDefaultMessageFmt =
         "The instance is required to implement the interface or abstract type '{0}' but is of type '{1}' which does not.{2}";

      internal const String IocContainerConfigurationLockedExceptionDefaltMessageFmt =
         "The container ({0}:{1}) has a locked configuration.  Attempting to edit the configuration causes this exception.";

      internal const String MultipleAssemblyRegistrarDetectedFmt =
         "Service location assemblies must not have more than one implementation of IAssemblyRegistrar.  The assembly {0}, Version={1} has more than one including {2}.";

      internal const String NullReplacementValue = "null";
      internal const String RegisteredTypeIsNullCannotLazilyCreate = "Registered Implementation type is null, cannot lazily create";

      internal const String ResolutionIocExceptionDefaultMessageFmt = "Unable to resolve the requested type/name: {0}/'{1}'. ('' indicates a default registration).";

      internal const String ToTypeMustHavePublicDefaultConstructorArgumentExceptionDefaultMessageFmt =
         "Registered implementation types must have a public default constructor. '{0}' does not have a public default (parameterless) constructor.  Either add such a constructor, " +
         "or instantiate an instance and use a RegisterInstance method instead of the RegisterImplementation methods.{1}";

      internal const String ToTypeMustImplementTypeArgumentExceptionDefaultMessageFmt =
         "The interface or abstract type '{0}' must be implemented by '{1}' but '{1}' does meet this requirement.{2}";

      internal const String ToTypeMustNotBeAbstractOrInterfaceArgumentExceptionDefaultMessageFmt =
         "The given type '{0}' must not be an abstract or interface type, but is such a type.{1}";

      internal const String TypeMustNotBeOpenGenericDefaultMessageFmt =
         "The given type '{0}' must not be an open generic, but is an open generic (e.g. IList<T>).  " +
         "Replace with a closed type (e.g. IList<Int32>).{1}";
   }
}
