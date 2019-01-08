namespace Landorphan.Common
{
   using System;

   /// <summary>
   /// Denotes an instance that supports read-only behavior.
   /// </summary>
   public interface IQueryReadOnly
   {
      /// <summary>
      /// Gets a value indicating whether the Object is immutable.
      /// </summary>
      /// <value>
      /// <c>true</c> if the Object is immutable; otherwise, <c>false</c>.
      /// </value>
      Boolean IsReadOnly { get; }
   }
}
