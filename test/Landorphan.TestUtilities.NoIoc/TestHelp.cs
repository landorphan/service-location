namespace Landorphan.TestUtilities
{
   using System;
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Helper methods for test projects.
   /// </summary>
   public static class TestHelp
   {
      /// <summary>
      /// Used by tests to suppress CA1801:ReviewUnusedParameters.
      /// </summary>
      /// <param name="inputs">
      /// The inputs.
      /// </param>
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "inputs")]
      public static void DoNothing(params Object[] inputs)
      {
         // this method used to avoid code analysis warnings.
      }
   }
}