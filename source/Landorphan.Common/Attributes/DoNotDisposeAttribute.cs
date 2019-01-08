namespace Landorphan.Common
{
   using System;

   /// <summary>
   /// Instructs the framework that field is not owned by this instance and should not be disposed.
   /// </summary>
   [AttributeUsage(AttributeTargets.Field)]
   public sealed class DoNotDisposeAttribute : Attribute
   {
   }
}