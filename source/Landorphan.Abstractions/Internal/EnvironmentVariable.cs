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
      public EnvironmentVariable(string name, string value)
      {
         Name = name.TrimNullToEmpty();
         Value = value.TrimNullToNull();
      }

       /// <inheritdoc/>
      public string Name { get; }

       /// <inheritdoc/>
      public string Value { get; }

       /// <inheritdoc/>
      public bool Equals(IEnvironmentVariable other)
      {
         if (ReferenceEquals(null, other))
         {
            return false;
         }

         return string.Equals(Name, other.Name.TrimNullToEmpty(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Value, other.Value.TrimNullToNull(), StringComparison.OrdinalIgnoreCase);
      }

       /// <inheritdoc/>
      public override bool Equals(object obj)
      {
         return Equals(obj as IEnvironmentVariable);
      }

       /// <inheritdoc/>
      public override int GetHashCode()
      {
         unchecked
         {
            return ((Name?.ToUpperInvariant().GetHashCode() ?? 0) * 397) ^
                   (Value?.ToUpperInvariant().GetHashCode() ?? 0);
         }
      }

       /// <inheritdoc/>
      public override string ToString()
      {
         return string.Format(CultureInfo.InvariantCulture, "Name: {0}, Value: {1}", Name, Value ?? string.Empty);
      }
   }
}
