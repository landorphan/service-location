namespace Landorphan.Common
{
   using System;

   /// <summary>
   /// Represents an instance that supports extended disposable behavior and event notifications of dispose.
   /// </summary>
   public interface INotifyingQueryDisposable : IQueryDisposable
   {
      /// <summary>
      /// Event queue for all listeners interested in disposing events.
      /// </summary>
      /// <remarks>
      /// Fired at the outset of the disposal of an instance.
      /// </remarks>
      event EventHandler<EventArgs> Disposing;
   }
}
