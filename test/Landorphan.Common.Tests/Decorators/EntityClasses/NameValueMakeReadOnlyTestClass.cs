namespace Landorphan.Common.Tests.Decorators.EntityClasses
{
   using System;
   using System.Collections.Generic;

   internal class NameValueMakeReadOnlyTestClass : IConvertsToReadOnly, IQueryReadOnly, ICloneable, IEquatable<NameValueMakeReadOnlyTestClass>
   {
      private readonly SupportsReadOnlyHelper _supportsReadOnlyHelper = new SupportsReadOnlyHelper();
      private String _name;
      private Int32 _value;

      public Object Clone()
      {
         return new NameValueMakeReadOnlyTestClass {Name = Name, Value = Value};
      }

      public Boolean IsReadOnly => _supportsReadOnlyHelper.IsReadOnly;

      public String Name
      {
         get => _name;
         set
         {
            _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
            _name = value;
         }
      }

      public Int32 Value
      {
         get => _value;
         set
         {
            _supportsReadOnlyHelper.ThrowIfReadOnlyInstance();
            _value = value;
         }
      }

      public void MakeReadOnly()
      {
         _supportsReadOnlyHelper.MakeReadOnly();
      }

      public Boolean Equals(NameValueMakeReadOnlyTestClass other)
      {
         if (other.IsNull())
         {
            return false;
         }

         var rv = EqualityComparer<String>.Default.Equals(Name, other.Name) && EqualityComparer<Int32>.Default.Equals(Value, other.Value);
         return rv;
      }
   }
}