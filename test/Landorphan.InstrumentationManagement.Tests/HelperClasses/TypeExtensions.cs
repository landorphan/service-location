namespace Landorphan.InstrumentationManagement.Tests.HelperClasses
{
   using System;
   using System.Reflection;

   public static class TypeExtensions
   {
      public static PropertyInfo GetFirstPropertyByName(this Type type, string name)
      {
         var properties = type.GetProperties();
         // Why not use Linq...
         // This pattern was used to help with debugging when a property can't be found.
         foreach (var propertyInfo in properties)
         {
            if (propertyInfo.Name == name)
            {
               return propertyInfo;
            }
         }

         return null;
      }
   }
}
