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
      void Set(string name, object value);
      object Get(string name);
   }
}
