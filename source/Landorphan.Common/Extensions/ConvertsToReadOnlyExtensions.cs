namespace Landorphan.Common
{
   /// <summary>
   /// Extension methods for working with <see cref="IConvertsToReadOnly"/> instances.
   /// </summary>
   public static class ConvertsToReadOnlyExtensions
   {
      /// <summary>
      /// Fluently calls <see cref="IConvertsToReadOnly.MakeReadOnly"/> on an instance.
      /// </summary>
      /// <typeparam name="T">
      /// An type implementing <see cref="IConvertsToReadOnly"/>.
      /// </typeparam>
      /// <param name="instance">
      /// The instance.
      /// </param>
      /// <returns>
      /// The instance after calling <see cref="IConvertsToReadOnly.MakeReadOnly"/> on the instance.
      /// </returns>
      public static T MakeReadOnly<T>(this T instance) where T : IConvertsToReadOnly
      {
         instance.ArgumentNotNull(nameof(instance));
         instance.MakeReadOnly();
         return instance;
      }
   }
}