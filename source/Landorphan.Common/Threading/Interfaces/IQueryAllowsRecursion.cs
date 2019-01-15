namespace Landorphan.Common.Threading
{
   using System;

   /// <summary>
   /// Denotes an instance that indicates whether or not it allows recursion.
   /// </summary>
   public interface IQueryAllowsRecursion
   {
      /// <summary>
      /// Gets a value indicating whether or not this instance allows recursion.
      /// </summary>
      /// <value>
      /// <c>true</c> when this instance allows recursive calls into the lock from the same thread;
      /// otherwise, <c>false</c>.
      /// </value>
      Boolean AllowsRecursion { get; }
   }
}
