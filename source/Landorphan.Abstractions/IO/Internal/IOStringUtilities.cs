namespace Landorphan.Abstractions.IO.Internal
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using System.Text;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;

   internal static class IOStringUtilities
   {
      internal static String ConditionallyTrimSpaceFromPath(String path)
      {
         if (path == null)
         {
            return null;
         }

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

         // Remove trailing spaces and conditionally leading spaces (LeftTrim if starts with relative root, unc, or drive label).
         var rv = path.RightTrim(' ');
         var leftTrimmed = rv.LeftTrim(' ');
         if (leftTrimmed.Length == 0)
         {
            rv = leftTrimmed;
         }
         else if (leftTrimmed.First() == pathUtilities.DirectorySeparatorCharacter)
         {
            rv = leftTrimmed;
         }
         else if (leftTrimmed.First() == pathUtilities.AltDirectorySeparatorCharacter)
         {
            rv = leftTrimmed;
         }
         else if (leftTrimmed.First() == '.')
         {
            rv = leftTrimmed;
         }
         else if (leftTrimmed.Length > 1 && leftTrimmed[1] == pathUtilities.VolumeSeparatorCharacter)
         {
            // assume drive label
            rv = leftTrimmed;
         }
         // ReSharper disable once CommentTypo
         // otherwise leave leading whitespace (e.g. a file named "   myfile.txt")

         return rv;
      }

      [SuppressMessage("SonarLint.CodeSmell", "S109: Magic numbers should not be used")]
      internal static Boolean DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(String path)
      {
         if (path == null)
         {
            return false;
         }

         path = path.RightTrim(' ');
         if (path.Length == 0)
         {
            return false;
         }

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

         // allow for leading spaces b/c both the BCL and the OS allow for leading spaces.
         path = RemoveLeadingSpacesOnly(path);

         Boolean rv;
         var idxColon = path.IndexOf(pathUtilities.VolumeSeparatorCharacter);
         switch (idxColon)
         {
            case -1:
               // not found
               rv = false;
               break;

            case 1:
               // drive label
               if (path.Length <= 2)
               {
                  rv = false;
                  break;
               }

               idxColon = path.IndexOf(pathUtilities.VolumeSeparatorCharacter, 2);
               rv = -1 != idxColon;
               break;

            default:
               rv = true;
               break;
         }

         return rv;
      }

      internal static String RemoveOneTrailingDirectorySeparatorCharacter(String path)
      {
         if (path == null)
         {
            return null;
         }

         var rv = path;

         if (rv.Length > 0)
         {
            var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

            if (rv.Last() == pathUtilities.AltDirectorySeparatorCharacter || rv.Last() == pathUtilities.DirectorySeparatorCharacter)
            {
               rv = rv.Substring(0, rv.Length - 1);
            }
         }

         return rv;
      }

      internal static String StandardizeDirectorySeparatorCharacters(String path)
      {
         path.ArgumentNotNull(nameof(path));

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

         if (path.Contains(pathUtilities.DirectorySeparatorCharacter) && path.Contains(pathUtilities.AltDirectorySeparatorCharacter))
         {
            path = path.Replace(pathUtilities.AltDirectorySeparatorCharacter, pathUtilities.DirectorySeparatorCharacter);
         }

         return path;
      }

      /// <summary>
      /// Validates the canonical path.
      /// </summary>
      /// <param name="path">The path.</param>
      /// <param name="argumentName">Name of the argument.</param>
      /// <remarks>
      /// Treats both '?' and '*' as invalid characters.
      /// </remarks>
      /// <exception cref="ArgumentNullException">
      /// <paramref name="path"/> is <c>null</c>.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// The path is not well-formed (cannot be empty or all whitespace).
      /// -or-
      /// The path is not well-formed (invalid characters).
      /// -or-
      /// The path is not well-formed (':' used outside the drive label).
      /// </exception>
      /// <exception cref="PathTooLongException">
      /// The specified path, file name, or both exceed the system-defined maximum length.
      /// </exception>
      /// <returns>
      /// A cleansed copy of <paramref name="path"/>.
      /// </returns>
      [SuppressMessage("SonarLint.CodeSmell", "S109: Magic numbers should not be used")]
      [SuppressMessage("SonarLint.CodeSmell", "S1067: Expressions should not be too complex")]
      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      internal static String ValidateCanonicalPath(String path, String argumentName)
      {
         // returns a cleaned string if it does not throw.

         // Error messages are inconsistent across Directory methods.  
         // This method attempts to standardize the handling of directory path structural validation.
         // It does not check security, existence, etc.

         // THERE BE DRAGONS HERE:
         // Sequential cohesion.  Run tests to confirm behavior.

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

         argumentName = argumentName ?? String.Empty;

         path.ArgumentNotNull(argumentName);

         var cleanedPath = ConditionallyTrimSpaceFromPath(path);

         if (cleanedPath.Trim().Length == 0)
         {
            throw new ArgumentException(@"The path is not well-formed (cannot be empty or all whitespace).", argumentName);
         }

         const Int32 indexNotFound = -1;

         // GetInvalidPathCharacters() excludes the following: '|'
         // REFACTOR:  now allowing: '\\', '/', ':', '*', '?', '<', '>' which are not allowed by Windows File Explorer
         if (indexNotFound != cleanedPath.IndexOfAny(pathUtilities.GetInvalidPathCharacters().ToArray()))
         {
            throw new ArgumentException(@"The path is not well-formed (invalid characters).", argumentName);
         }

         // Check for improper ':'
         if (DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(cleanedPath))
         {
            var msg = String.Format(
               CultureInfo.InvariantCulture,
               @"The path is not well-formed ('{0}' used outside the drive label).",
               pathUtilities.VolumeSeparatorCharacter);
            throw new ArgumentException(msg, argumentName);
         }

         // TODO: Need to remove embedded leading whitespace on resource names here.

         // TODO: invalid multiple-toothpicks  AltDirectorySeparatorCharacter and AltDirectorySeparatorCharacter
         // Leading spaces allowed on resource names, but not trailing.  Whitespace only resource names not allowed.

         // Adjustment under consideration, always remove one trailing directory separator character

         // preserve @"/", @"\", and @"\\" as results.
         if (String.Equals(cleanedPath, @"\\", StringComparison.Ordinal) || String.Equals(cleanedPath, @"\", StringComparison.Ordinal) || String.Equals(cleanedPath, @"/", StringComparison.Ordinal))
         {
            return cleanedPath;
         }

         // preserve {drive letter}:\ and {drive letter}:/ as results
         if (cleanedPath.Length == 3 &&
             Char.IsLetter(cleanedPath[0]) &&
             cleanedPath[1] == pathUtilities.VolumeSeparatorCharacter &&
             (cleanedPath[2] == pathUtilities.DirectorySeparatorCharacter || cleanedPath[2] == pathUtilities.AltDirectorySeparatorCharacter))
         {
            return cleanedPath;
         }

         cleanedPath = RemoveOneTrailingDirectorySeparatorCharacter(cleanedPath);

#pragma warning disable S2737 // "catch" clauses should do more than rethrow
         // this call will throw a PathTooLongException as needed.
         // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
         Path.GetFullPath(cleanedPath);
#pragma warning restore S2737 // "catch" clauses should do more than rethrow

         // Avoid mixed directory separator characters.
         cleanedPath = StandardizeDirectorySeparatorCharacters(cleanedPath);

         // REFACTOR:  was using GetFullPath to remove invalid trailing whitespace and validate, but losing relative path
         // now risking embedded trailing whitespace
         return cleanedPath;
      }

      private static String RemoveLeadingSpacesOnly(String value)
      {
         // does not remove \t which is an invalid path character
         if (value == null)
         {
            return null;
         }

         if (value.Length == 0)
         {
            return String.Empty;
         }

         var sb = new StringBuilder(value);
         while (sb.Length > 0 && sb[0] == ' ')
         {
            sb.Remove(0, 1);
         }

         return sb.ToString();
      }
   }
}
