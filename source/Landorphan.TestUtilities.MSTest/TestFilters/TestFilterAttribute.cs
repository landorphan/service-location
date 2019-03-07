namespace Landorphan.TestUtilities.TestFilters
{
   using System;

   /// <summary>
   /// Allows a developer to suppress a test based on runtime evaluated criteria.
   /// This attribute will be respected only for test that derive from <see cref="TestBase"/>.
   /// A test will be suppressed if any TestFilterAttribute is applied to the class
   /// that evaluates to true when the <see cref="ReturnInconclusiveTestResult"/> method is called.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
   public abstract class TestFilterAttribute : Attribute
   {
      /// <summary>
      /// When overriden by a derived class and applied to a class decorated with the
      /// <see cref="Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute"/>
      /// or a method decorated with the <see cref="Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute"/>
      /// this method will be called by the test framework.  If the derived override
      /// returns true, then the test will be ignored (an Inconclusive result will be returned).
      /// All <see cref="TestFilterAttribute"/> are evaluated when a test method is being
      /// executed.  If any one returns true, then the test method will return Inconclusive.
      /// </summary>
      /// <returns>
      /// True if the test method should return
      /// Inconclusive otherwise false.
      /// </returns>
      public abstract bool ReturnInconclusiveTestResult();
   }
}
