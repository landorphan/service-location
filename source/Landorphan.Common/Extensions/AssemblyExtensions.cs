namespace Landorphan.Common
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Linq;
   using System.Reflection;

   /// <summary>
   /// Extension methods for working with <see cref="Assembly"/> instances.
   /// </summary>
   public static class AssemblyExtensions
   {
      /// <summary>
      /// Safely gets the distinct types defined in the given <paramref name="assembly"/>.
      /// </summary>
      /// <param name="assembly">
      /// The assembly to inspect.
      /// </param>
      /// <returns>
      /// A non-null collection of unique non-null <see cref="Type"/> elements found in the given <paramref name="assembly"/>.
      /// </returns>
      public static IImmutableSet<Type> SafeGetTypes(this Assembly assembly)
      {
         assembly.ArgumentNotNull(nameof(assembly));

         List<Type> types;
         try
         {
            types = assembly.GetTypes().ToList();
         }
         catch (ReflectionTypeLoadException loadEx)
         {
            // example:  
            // 
            // {
            //    "Could not load file or assembly 'Microsoft.Practices.ServiceLocation, Version=1.2.0.0, Culture=neutral, 
            //    PublicKeyToken=31bf3856ad364e35' or one of its dependencies. The system cannot find the file specified.":
            //    "Microsoft.Practices.ServiceLocation, Version=1.2.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
            // }
            types = (from t in loadEx.Types where !ReferenceEquals(null, t) select t).ToList();
         }

         var builder = ImmutableHashSet<Type>.Empty.ToBuilder();
         foreach (var t in types)
         {
            builder.Add(t);
         }

         return builder.ToImmutable();
      }
   }
}
