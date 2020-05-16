namespace Landorphan.InstrumentationManagement
{
   /// <summary>
   /// Provides data on a methods Parameters 
   /// </summary>
   public interface IParameterData
   {
       /// <summary>
      /// Gets or sets the name of the Parameter
      /// </summary>
      string ParameterName { get; set; }

       /// <summary>
      /// Gets or sets the name of the Parameter Type
      /// </summary>
      string ParameterTypeFullName { get; set; }
   }
}
