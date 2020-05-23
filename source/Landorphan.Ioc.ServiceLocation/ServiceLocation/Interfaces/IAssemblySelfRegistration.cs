namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
    /// <summary>
    /// Represents the capacity for an assembly to self-register services.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Assemblies that implement this class do so to register the default implementation of their service interfaces.
    /// </para>
    /// <para>
    /// Current design limitations for implementation:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Must have public default constructor.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// No more than one implementation per assembly.
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The primary purpose of this interface is to allow class libraries to register their (dependent) services, so that the application consuming the library gets a viable default production
    /// implementation by default without first wading into the complexities of boot-strapping service location for multiple class libraries in configure files.
    /// </para>
    /// </remarks>
    public interface IAssemblySelfRegistration
    {
        /// <summary>
        /// Registers service interface types with instances, or implementation types with default constructors, for the assembly that defines a single class that implements this interface.
        /// </summary>
        /// <param name="registrar">
        /// The inversion of control container registrar.
        /// </param>
        /// <remarks>
        /// Use the <paramref name="registrar"/> to register types defined in the assembly as well as overrides of types registered in other assemblies.
        /// Use the registrar.Resolver to resolve types already registered.
        /// </remarks>
        void RegisterServiceInstances(IIocContainerRegistrar registrar);
    }
}
