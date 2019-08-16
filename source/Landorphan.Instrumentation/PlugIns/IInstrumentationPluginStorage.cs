namespace Landorphan.Instrumentation.PlugIns
{
   /// <summary>
   /// This provides storage to track a call through
   /// all paths within the system.
   ///
   /// In order for this system to target the largest variety of frameworks
   /// and as this differs from NetFx to Core (and Web), this must be
   /// supplied by the caller.
   ///
   /// The caller must supply two instances of this class one for Asynchronous storage
   /// and one for session storage
   /// </summary>
   public interface IInstrumentationPluginStorage
   {
      /// <summary>
      /// Used to set an Async or Session variable.
      /// </summary>
      /// <param name="name">
      /// The name of the value to set.
      /// </param>
      /// <param name="value">
      /// The value to set.
      /// </param>
      void Set(string name, object value);

      /// <summary>
      /// Used to get an Async or Session variable.
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
