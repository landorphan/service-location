namespace Landorphan.Common
{
   /// <summary>
   /// Denotes an instance that can be converted to a read-only instance.
   /// </summary>
   public interface IConvertsToReadOnly : IQueryReadOnly
   {
      /// <summary>
      /// Transforms the Object to an immutable instance.
      /// </summary>
      void MakeReadOnly();
   }
}
