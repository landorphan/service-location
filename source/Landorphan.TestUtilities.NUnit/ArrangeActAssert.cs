namespace Landorphan.TestUtilities
{
   /// <summary>
   /// Provides common services for BDD-style (arrange/act/assert) tests.
   /// </summary>
   /// <remarks>
   /// Serves as an adapter between the MSTest framework and BDD-style tests.
   /// </remarks>
   public abstract class ArrangeActAssert : TestBase
   {
       /// <summary>
      /// Acts on the context for a single test method invocation.
      /// </summary>
      protected virtual void ActMethod()
      {
      }

       /// <summary>
      /// Further refines the context for a single test method invocation.
      /// </summary>
      protected virtual void ArrangeMethod()
      {
      }

       /// <inheritdoc/>
      protected override void InitializeTestMethod()
      {
         base.InitializeTestMethod();
         ArrangeMethod();
         ActMethod();
      }
   }
}
