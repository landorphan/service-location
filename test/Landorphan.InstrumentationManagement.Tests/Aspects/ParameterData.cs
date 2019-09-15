namespace Landorphan.InstrumentationManagement.Tests.Aspects
{
   using Landorphan.InstrumentationManagement;
   using PostSharp.Serialization;

   /// <summary>
   /// Provides data on a methods Parameters 
   /// </summary>
   [PSerializable]
   public class ParameterData : IParameterData
   {
      /// <summary>
      /// Gets or sets the name of the Parameter
      /// </summary>
      public string ParameterName { get; set; }

      /// <summary>
      /// Gets or sets the name of the Parameter Type
      /// </summary>
      public string ParameterTypeFullName { get; set; }
   }
}
