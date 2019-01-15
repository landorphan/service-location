namespace Landorphan.Common
{
   using System;
   using System.Collections.Generic;

   /// <summary>
   /// Represents a reference equality comparison operation.
   /// </summary>
   /// <remarks>
   /// Intended for use with <see cref="ISet{T}"/> which takes an <see cref="IEqualityComparer{T}"/> . However, the implementation only
   /// examines reference equality.
   /// </remarks>
   /// <typeparam name="T">
   /// The types to compare.
   /// </typeparam>
   public sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
   {
      /// <summary>
      /// Determines whether the specified objects are equal.
      /// </summary>
      /// <param name="x">
      /// The first Object of type <typeref name="T"/> to compare.
      /// </param>
      /// <param name="y">
      /// The second Object of type <typeref name="T"/> to compare.
      /// </param>
      /// <returns>
      /// true if the specified objects are equal; otherwise, false.
      /// </returns>
      public Boolean Equals(T x, T y)
      {
         return ReferenceEquals(x, y);
      }

      /// <summary>
      /// Returns a hash code for the specified Object unique to the instance.
      /// </summary>
      /// <param name="obj">
      /// The <see cref="Object"/> for which a hash code is to be returned.
      /// </param>
      /// <returns>
      /// A hash code for the specified Object unique to the instance.
      /// </returns>
      public Int32 GetHashCode(T obj)
      {
         return ReferenceEquals(obj, null) ? 0 : obj.GetHashCode();
      }
   }
}
