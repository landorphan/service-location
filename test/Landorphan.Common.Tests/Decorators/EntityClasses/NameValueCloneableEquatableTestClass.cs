namespace Landorphan.Common.Tests.Decorators.EntityClasses
{
   using System;
   using System.Collections.Generic;

   internal class NameValueCloneableEquatableTestClass : ICloneable, IEquatable<NameValueCloneableEquatableTestClass>
   {
      public Object Clone()
      {
         return new NameValueCloneableEquatableTestClass {Name = Name, Value = Value};
      }

      public String Name { get; set; }

      public Int32 Value { get; set; }

      public Boolean Equals(NameValueCloneableEquatableTestClass other)
      {
         if (other.IsNull())
         {
            return false;
         }

         var rv = EqualityComparer<String>.Default.Equals(Name, other.Name) && EqualityComparer<Int32>.Default.Equals(Value, other.Value);
         return rv;
      }

      public override Boolean Equals(Object obj)
      {
         return Equals(obj as NameValueCloneableEquatableTestClass);
      }

      public override Int32 GetHashCode()
      {
         unchecked
         {
            return ((Name?.GetHashCode() ?? 0) * 397) ^ Value;
         }
      }
   }
}
