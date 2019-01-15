namespace Landorphan.Common.Tests.Collections.TestClasses
{
   using System;
   using System.Collections.Generic;

   internal sealed class EquatableTestItemEqualityComparer : IEqualityComparer<EquatableTestItem>
   {
      public Boolean Equals(EquatableTestItem x, EquatableTestItem y)
      {
         if (x.IsNull())
         {
            if (y.IsNull())
            {
               return true;
            }

            return false;
         }

         if (y.IsNull())
         {
            return false;
         }

         return x.Value == y.Value && EqualityComparer<String>.Default.Equals(x.Name, y.Name);
      }

      public Int32 GetHashCode(EquatableTestItem obj)
      {
         if (obj.IsNull())
         {
            return 0;
         }

         unchecked
         {
            return (EqualityComparer<String>.Default.GetHashCode(obj.Name) * 397) ^ obj.Value;
         }
      }
   }
}
