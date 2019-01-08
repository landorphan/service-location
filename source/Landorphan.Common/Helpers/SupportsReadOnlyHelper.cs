namespace Landorphan.Common
{
   using System;
   using Landorphan.Common.Resources;
   using Landorphan.Common.Threading;

   /// <summary>
   /// Helper class for types that support converting instances to read-only instances.
   /// </summary>
   /// <remarks>
   /// Intended to be aggregated.
   /// </remarks>
   public sealed class SupportsReadOnlyHelper : IQueryReadOnly, IConvertsToReadOnly
   {
      private InterlockedBoolean _isReadOnly = new InterlockedBoolean(false);

      /// <inheritdoc/>
      public Boolean IsReadOnly => _isReadOnly.GetValue();

      /// <inheritdoc/>
      public void MakeReadOnly()
      {
         _isReadOnly.SetValue(true);
      }

      /// <summary>
      /// Throws a <see cref="NotSupportedException"/> if the current instance is a read-only instance.
      /// </summary>
      /// <exception cref="NotSupportedException">
      /// Thrown when the requested operation is not supported.
      /// </exception>
      public void ThrowIfReadOnlyInstance()
      {
         if (_isReadOnly.GetValue())
         {
            throw new NotSupportedException(StringResources.TheCurrentInstanceIsReadOnly);
         }
      }
   }
}