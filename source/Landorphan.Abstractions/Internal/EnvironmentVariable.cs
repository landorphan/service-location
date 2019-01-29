namespace Landorphan.Abstractions.Internal
{
   using System;
   using System.Diagnostics;
   using System.Globalization;
   using Landorphan.Abstractions.Interfaces;
   using Landorphan.Common;

   /// <summary>
   /// Default implementation of <see cref="IEnvironmentVariable"/>
   /// </summary>
   [DebuggerDisplay("{Name}={Value}")]
   internal sealed class EnvironmentVariable : IEnvironmentVariable
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="EnvironmentVariable"/> class.
      /// </summary>
      /// <param name="name"> The name. </param>
      /// <param name="value"> The value. </param>
      public EnvironmentVariable(String name, String value)
      {
         Name = name.TrimNullToEmpty();
         Value = value.TrimNullToNull();
      }

      /// <inheritdoc/>
      public String Name { get; }

      /// <inheritdoc/>
      public String Value { get; }

      /// <inheritdoc/>
      public Boolean Equals(IEnvironmentVariable other)
      {
         if (ReferenceEquals(null, other))
         {
            return false;
         }

         return String.Equals(Name, other.Name.TrimNullToEmpty(), StringComparison.OrdinalIgnoreCase) &&
                String.Equals(Value, other.Value.TrimNullToNull(), StringComparison.OrdinalIgnoreCase);
      }

      /// <inheritdoc/>
      public override Boolean Equals(Object obj)
      {
         return Equals(obj as IEnvironmentVariable);
      }

      /// <inheritdoc/>
      public override Int32 GetHashCode()
      {
         unchecked
         {
            return ((Name?.ToUpperInvariant().GetHashCode() ?? 0) * 397) ^
                   (Value?.ToUpperInvariant().GetHashCode() ?? 0);
         }
      }

      /// <inheritdoc/>
      public override String ToString()
      {
         return String.Format(CultureInfo.InvariantCulture, "Name: {0}, Value: {1}", Name, Value ?? String.Empty);
      }
   }
}
