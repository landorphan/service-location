namespace Landorphan.Common.Tests.Collections.TestClasses
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;

   internal sealed class EquatableTestItem : IEquatable<EquatableTestItem>
   {
      private String _name = String.Empty;

      public EquatableTestItem()
      {
      }

      public EquatableTestItem(EquatableTestItem other) : this()
      {
         other.ArgumentNotNull(nameof(other));

         Name = other.Name;
         Value = other.Value;
      }

      public String Name
      {
         get => _name;
         set => _name = value.TrimNullToEmpty();
      }

      public Int32 Value { get; set; }

      public Boolean Equals(EquatableTestItem other)
      {
         if (ReferenceEquals(null, other))
         {
            return false;
         }

         return Value == other.Value &&
                EqualityComparer<String>.Default.Equals(
                   _name.TrimNullToEmpty().ToUpperInvariant(),
                   other.Name.TrimNullToEmpty().ToUpperInvariant());
      }

      public override Boolean Equals(Object obj)
      {
         return Equals(obj as EquatableTestItem);
      }

      public override Int32 GetHashCode()
      {
         unchecked
         {
            return (Name.TrimNullToEmpty().ToUpperInvariant().GetHashCode() * 397) ^ Value;
         }
      }

      [SuppressMessage(
         "Microsoft.Globalization",
         "CA1305: Specify IFormatProvide",
         Justification = "This rule is disabled for this project and most other test projects, but the rule still emits warnings")]
      public override String ToString()
      {
         return $"Name: {Name}, Value: {Value}";
      }
   }
}
