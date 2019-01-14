namespace Landorphan.Common
{
   using System;

   /// <summary>
   /// Represents an Object that may support ignoring null values.
   /// </summary>
   public interface IQueryIgnoresNullValues
   {
      /// <summary>
      /// Gets a value indicating whether this instance ignores null values.
      /// </summary>
      /// <value>
      /// <c> true </c>if this instance ignores null values; otherwise <c> false </c>.
      /// </value>
      Boolean IgnoresNullValues { get; }
   }
}
