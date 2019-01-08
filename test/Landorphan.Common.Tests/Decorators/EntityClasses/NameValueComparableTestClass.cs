namespace Landorphan.Common.Tests.Decorators.EntityClasses
{
   using System;
   using System.Collections.Generic;

   internal class NameValueComparableTestClass : IComparable<NameValueComparableTestClass>
   {
      public String Name { get; set; }
      public Int32 Value { get; set; }

      public Int32 CompareTo(NameValueComparableTestClass other)
      {
         if (other.IsNull())
         {
            return 1;
         }

         var rv = Comparer<String>.Default.Compare(Name, other.Name);
         if (rv == 0)
         {
            rv = Comparer<Int32>.Default.Compare(Value, other.Value);
         }

         return rv;
      }
   }
}