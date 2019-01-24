namespace Ioc.Collections.Performance.Tests
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.Text;
   using System.Threading;

   // ReSharper disable UseFormatSpecifierInInterpolation
   // ReSharper disable ConditionIsAlwaysTrueOrFalse
   // ReSharper disable IdentifierTypo
   [SuppressMessage("SonarLint.CodeSmell", "S1144: Unused private types or members should be removed")]
   internal static class Program
   {
      // the *key* in IOC registrations is a type, with an optional name.
      // off the top of my head, that leaves n obvious implementation choices
      //    Dictionary<Type, String>
      //    ImmutableDictionary<Type, String>
      //    HashSet<ValueType: composed of Type, String>
      //    ImmutableHashSet<ValueType: composed of Type, String>
      //    List variations of the same
      private static void Main()
      {
         var results = RunRandom();
         Console.WriteLine(results);

         Console.WriteLine("Press any key to continue...");
         Console.ReadKey();
      }

      // ReSharper disable once UnusedMember.Local
      private static String RunRandom()
      {
         var rnd = new Random();

         var count = 10000;
         IList<KeyValuePair<Type, Type>> list;
         using (var builder = new TestTypesBuilder())
         {
            // ReSharper disable once NotAccessedVariable
            builder.BuildTypePairs(count, out var asmName, out list);
         }

         var allowNamedImplementations = true;
         var allowPreclusionOfTypes = true;
         var throwOnRegistrationCollision = true;

         using (var target = new ImmutableDictionaryTarget(allowNamedImplementations, allowPreclusionOfTypes, throwOnRegistrationCollision))
         {
            foreach (var pair in list)
            {
               target.RegisterImplementationImplementation(pair.Key, "fromType", null, pair.Value, "toType", false);
            }

            // ReSharper disable once NotAccessedVariable
            for (var resolveCount = 1; resolveCount <= count; resolveCount++)
            {
               var randomIndex = rnd.Next(0, count - 1);
               var fromType = list[randomIndex].Key;
               target.ResolveImplementation(fromType, nameof(fromType), null, false, out var instance);
            }

            foreach (var pair in list)
            {
               target.UnregisterImplementation(pair.Key, null);
            }

            Thread.Sleep(0);

            var sb = new StringBuilder();
            sb.AppendLine($"AllowNamedImplementations:={target.AllowNamedImplementations}");
            sb.AppendLine($"AllowPreclusionOfTypes:={target.AllowPreclusionOfTypes}");
            sb.AppendLine($"ThrowOnRegistrationCollision:={target.ThrowOnRegistrationCollision}");
            sb.AppendLine("--------------------");

            target.GetRegistrationTotalStats(out var registrationTotalTime, out var registrationTotalCount);
            var sRegistrationTotalTimeMs = $"{registrationTotalTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Registration Total Time(ms):={sRegistrationTotalTimeMs}");
            target.GetRegistrationValidationStats(out var registrationValidationTime);
            var sRegistrationValidationTimeMs = $"{registrationValidationTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Registration Validation Time(ms):={sRegistrationValidationTimeMs}");
            target.GetRegistrationOverwriteStats(out var registrationOverwriteTime, out var registrationOverwriteCount);
            var sRegistrationOverwriteTimeMs = $"{registrationOverwriteTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Registration Over-write Time(ms):={sRegistrationOverwriteTimeMs}");
            sb.AppendLine($"Registration Count:={registrationTotalCount.ToString("N0", CultureInfo.InvariantCulture)}");
            sb.AppendLine($"Over-write Count:={registrationOverwriteCount.ToString("N0", CultureInfo.InvariantCulture)}");
            sb.AppendLine("--------------------");

            target.GetResolutionTotalStats(out var resolutionTotalTime, out var resolutionTotalCount);
            var sResolutionTotalTimeMs = $"{resolutionTotalTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Resolution Total Time(ms):={sResolutionTotalTimeMs}");
            target.GetResolutionValidationStats(out var resolutionValidationTime);
            var sResolutionValidationTimeMs = $"{resolutionValidationTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Resolution Validation Time(ms):={sResolutionValidationTimeMs}");
            target.GetResolutionOverwriteStats(out var resolutionOverwriteTime, out var resolutionNewInstancesCount);
            var sResolutionOverwriteTimeMs = $"{resolutionOverwriteTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Resolution Over-write with new instance Time(ms):={sResolutionOverwriteTimeMs}");
            sb.AppendLine($"Resolution Count:= {resolutionTotalCount.ToString("N0", CultureInfo.InvariantCulture)} (random)");
            sb.AppendLine($"New Instances Created := {resolutionNewInstancesCount.ToString("N0", CultureInfo.InvariantCulture)}");
            sb.AppendLine("--------------------");

            target.GetUnregistrationStats(out var unregistrationTotalTime, out var unregistrationTotalCount);

            var sUnregistrationTotalTimeMs = $"{unregistrationTotalTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Unregister Total Time(ms):={sUnregistrationTotalTimeMs}");
            sb.AppendLine($"Unregister Count :={unregistrationTotalCount.ToString("N0", CultureInfo.InvariantCulture)}");
            sb.AppendLine("--------------------");
            sb.AppendLine();

            return sb.ToString();
         }
      }

      // ReSharper disable once UnusedMember.Local
      private static String RunSequential()
      {
         var count = 10000;
         // ReSharper disable once NotAccessedVariable
         IList<KeyValuePair<Type, Type>> list;
         using (var builder = new TestTypesBuilder())
         {
            builder.BuildTypePairs(count, out var asmName, out list);
         }

         var allowNamedImplementations = true;
         var allowPreclusionOfTypes = true;
         var throwOnRegistrationCollision = true;

         using (var target = new ImmutableDictionaryTarget(allowNamedImplementations, allowPreclusionOfTypes, throwOnRegistrationCollision))
         {
            foreach (var pair in list)
            {
               target.RegisterImplementationImplementation(pair.Key, "fromType", null, pair.Value, "toType", false);
            }

            // ReSharper disable once NotAccessedVariable
            foreach (var pair in list)
            {
               target.ResolveImplementation(pair.Key, "fromType", null, false, out var instance);
            }

            foreach (var pair in list)
            {
               target.UnregisterImplementation(pair.Key, null);
            }

            Thread.Sleep(0);

            var sb = new StringBuilder();
            sb.AppendLine($"AllowNamedImplementations:={target.AllowNamedImplementations}");
            sb.AppendLine($"AllowPreclusionOfTypes:={target.AllowPreclusionOfTypes}");
            sb.AppendLine($"ThrowOnRegistrationCollision:={target.ThrowOnRegistrationCollision}");
            sb.AppendLine("--------------------");

            target.GetRegistrationTotalStats(out var registrationTotalTime, out var registrationTotalCount);
            var sRegistrationTotalTimeMs = $"{registrationTotalTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Registration Total Time(ms):={sRegistrationTotalTimeMs}");
            target.GetRegistrationValidationStats(out var registrationValidationTime);
            var sRegistrationValidationTimeMs = $"{registrationValidationTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Registration Validation Time(ms):={sRegistrationValidationTimeMs}");
            target.GetRegistrationOverwriteStats(out var registrationOverwriteTime, out var registrationOverwriteCount);
            var sRegistrationOverwriteTimeMs = $"{registrationOverwriteTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Registration Over-write Time(ms):={sRegistrationOverwriteTimeMs}");
            sb.AppendLine($"Registration Count:={registrationTotalCount.ToString("N0", CultureInfo.InvariantCulture)}");
            sb.AppendLine($"Over-write Count:={registrationOverwriteCount.ToString("N0", CultureInfo.InvariantCulture)}");
            sb.AppendLine("--------------------");

            target.GetResolutionTotalStats(out var resolutionTotalTime, out var resolutionTotalCount);
            var sResolutionTotalTimeMs = $"{resolutionTotalTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Resolution Total Time(ms):={sResolutionTotalTimeMs}");
            target.GetResolutionValidationStats(out var resolutionValidationTime);
            var sResolutionValidationTimeMs = $"{resolutionValidationTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Resolution Validation Time(ms):={sResolutionValidationTimeMs}");
            target.GetResolutionOverwriteStats(out var resolutionOverwriteTime, out var resolutionNewInstancesCount);
            var sResolutionOverwriteTimeMs = $"{resolutionOverwriteTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Resolution Over-write with new instance Time(ms):={sResolutionOverwriteTimeMs}");
            sb.AppendLine($"Resolution Count:= {resolutionTotalCount.ToString("N0", CultureInfo.InvariantCulture)} (sequential)");
            sb.AppendLine($"New Instances Created := {resolutionNewInstancesCount.ToString("N0", CultureInfo.InvariantCulture)}");
            sb.AppendLine("--------------------");

            target.GetUnregistrationStats(out var unregistrationTotalTime, out var unregistrationTotalCount);

            var sUnregistrationTotalTimeMs = $"{unregistrationTotalTime.TotalMilliseconds.ToString("N0", CultureInfo.InvariantCulture)}";
            sb.AppendLine($"Unregister Total Time(ms):={sUnregistrationTotalTimeMs}");
            sb.AppendLine($"Unregister Count :={unregistrationTotalCount.ToString("N0", CultureInfo.InvariantCulture)}");
            sb.AppendLine("--------------------");
            sb.AppendLine();

            return sb.ToString();
         }
      }
   }
}
