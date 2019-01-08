namespace Landorphan.Common
{
   using System;

   /// <summary>
   /// Denotes an instance that supports is thread-safety awareness.
   /// </summary>
   public interface IQueryThreadSafe
   {
      /// <summary>
      /// Gets a value indicating whether instance methods of this Object are thread-safe.
      /// </summary>
      /// <value>
      /// true if this Object is thread safe, false if not.
      /// </value>
      /// <remarks>
      /// Static members are assumed to be thread-safe.
      /// </remarks>
      Boolean IsThreadSafe { get; }
   }
}