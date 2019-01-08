namespace Landorphan.Common
{
   using System;

   /// <summary>
   /// Represents an instance that supports extended disposable behavior.
   /// </summary>
   public interface IQueryDisposable : IDisposable
   {
      /// <summary>
      /// Gets a value indicating whether this instance has been disposed or is in the process of disposing.
      /// </summary>
      /// <value>
      /// <c> true </c> if this Object is disposed or in the process of disposing; otherwise <c> false </c>.
      /// </value>
      Boolean IsDisposed { get; }

      /// <summary>
      /// Gets a value indicating whether this instance is actively cleaning up resources.
      /// </summary>
      /// <value>
      /// <c> true </c> if this Object in the process of disposing; otherwise <c> false </c>.
      /// </value>
      Boolean IsDisposing { get; }
   }
}