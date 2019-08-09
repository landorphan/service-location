namespace Landorphan.Instrumentation.Interfaces
{
   /// <summary>
   /// This provides asynchronous storage to track a call through
   /// all paths within the system.
   ///
   /// In order for this system to target the largest variety of frameworks
   /// and as this differs from NetFx to Core (and Web), this must be
   /// supplied by the caller.
   /// </summary>
   public interface IInstrumentationAsyncStorage
   {
      /// <summary>
      /// Used to set an Async variable.
      /// </summary>
      /// <param name="name">
      /// The name of the value to set.
      /// </param>
      /// <param name="value">
      /// The value to set.
      /// </param>
      void Set(string name, object value);

      /// <summary>
      /// Used to get an Async variable.
      /// </summary>
      /// <param name="name">
      /// The name of the value.
      /// </param>
      /// <returns>
      /// The value of the variable previously set.
      /// </returns>
      object Get(string name);
   }
}
