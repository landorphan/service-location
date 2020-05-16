﻿namespace Landorphan.Abstractions.IO.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using Landorphan.Abstractions.IO.Interfaces;
    using Landorphan.Common;
    using Landorphan.Ioc.ServiceLocation;

    internal static class IOStringUtilities
   {
       internal static string ConditionallyTrimSpaceFromPath(string path)
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
      internal static bool DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(string path)
      {
         // Not a concept that applies to non Windows OS variants
         if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
         {
            return false;
         }

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

         bool rv;
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

      internal static string RemoveOneTrailingDirectorySeparatorCharacter(string path)
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

      internal static string StandardizeDirectorySeparatorCharacters(string path)
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
      [SuppressMessage("SonarLint.CodeSmell", "S2737: Catch clauses should do more than rethrow")]
      internal static string ValidateCanonicalPath(string path, string argumentName)
      {
         // returns a cleaned string if it does not throw.

         // Error messages are inconsistent across Directory methods.
         // This method attempts to standardize the handling of directory path structural validation.
         // It does not check security, existence, etc.

         // THERE BE DRAGONS HERE:
         // Sequential cohesion.  Run tests to confirm behavior.

         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();

         argumentName = argumentName ?? string.Empty;

         path.ArgumentNotNull(argumentName);

         var cleanedPath = ConditionallyTrimSpaceFromPath(path);

         if (cleanedPath.Trim().Length == 0)
         {
            throw new ArgumentException(@"The path is not well-formed (cannot be empty or all whitespace).", argumentName);
         }

         const int indexNotFound = -1;

         // GetInvalidPathCharacters() excludes the following: '|' and 30'ish more unprintable characters
         if (indexNotFound != cleanedPath.IndexOfAny(pathUtilities.GetInvalidPathCharacters().ToArray()))
         {
            throw new ArgumentException(@"The path is not well-formed (invalid characters).", argumentName);
         }

         // Check for improper ':'
         if (DoesPathContainsVolumeSeparatorCharacterThatIsNotPartOfTheDriveLabel(cleanedPath))
         {
            var msg = string.Format(
               CultureInfo.InvariantCulture,
               @"The path is not well-formed ('{0}' used outside the drive label).",
               pathUtilities.VolumeSeparatorCharacter);
            throw new ArgumentException(msg, argumentName);
         }

         // Path.GetFullPath(cleanedPath) does not throw on non-Windows platforms when path exceeds max length.
         if (cleanedPath.Length > short.MaxValue)
         {
            var msg = $"The path '{cleanedPath}' is too long, or a component of the specified path is too long.";
            throw new PathTooLongException(msg);
         }

         // Leading spaces allowed on resource names, but not trailing.  Whitespace only resource names not allowed.
         // (I do not know how to recognize a directory name versus a resource names canonically)
         // Leading spaces on directory names not allowed, embedded spaces allowed.

         // a directory separator character, followed by one or more whitespace characters followed by word character;
         var pattern = @"[\" + pathUtilities.DirectorySeparatorCharacter + @"\" + pathUtilities.AltDirectorySeparatorCharacter + @"]+\s+\w";
         MatchEvaluator evaluator = ReplaceLeadingWhitespace;
         cleanedPath = Regex.Replace(cleanedPath, pattern, evaluator, RegexOptions.IgnoreCase);

         // .Net Standard 2.0 throws IOExceptions path not found on directory names with trailing spaces.
         // word character(s) followed by space(s) followed by directory separator character
         // Original pattern was @"\w+ ... " replaced with @"\w" as for this case will yield same result
         pattern = @"\w\s+[\" + pathUtilities.DirectorySeparatorCharacter + @"\" + pathUtilities.AltDirectorySeparatorCharacter + @"]+";
         evaluator = ReplaceTrailingWhitespace;
         cleanedPath = Regex.Replace(cleanedPath, pattern, evaluator, RegexOptions.IgnoreCase);

         // preserve @"/", @"\", and @"\\" as results.
         if (string.Equals(cleanedPath, @"\\", StringComparison.Ordinal) || string.Equals(cleanedPath, @"\", StringComparison.Ordinal) || string.Equals(cleanedPath, @"/", StringComparison.Ordinal))
         {
            return cleanedPath;
         }

         // preserve {drive letter}:\ and {drive letter}:/ as results
         if (cleanedPath.Length == 3 &&
             char.IsLetter(cleanedPath[0]) &&
             cleanedPath[1] == pathUtilities.VolumeSeparatorCharacter &&
             (cleanedPath[2] == pathUtilities.DirectorySeparatorCharacter || cleanedPath[2] == pathUtilities.AltDirectorySeparatorCharacter))
         {
            return cleanedPath;
         }

         cleanedPath = RemoveOneTrailingDirectorySeparatorCharacter(cleanedPath);

         // Avoid mixed directory separator characters.
         cleanedPath = StandardizeDirectorySeparatorCharacters(cleanedPath);

         return cleanedPath;
      }

      private static string RemoveLeadingSpacesOnly(string value)
      {
         // does not remove \t which is an invalid path character
         if (value == null)
         {
            return null;
         }

         if (value.Length == 0)
         {
            return string.Empty;
         }

         var sb = new StringBuilder(value);
         while (sb.Length > 0 && sb[0] == ' ')
         {
            sb.Remove(0, 1);
         }

         return sb.ToString();
      }

      [SuppressMessage("SonarLint.CodeSmell", "S3242: Method parameters should be declared with base types")]
      private static string ReplaceLeadingWhitespace(Match m)
      {
         // match will have a value unescaped like the following "\   x" or "/    x"
         var sb = new StringBuilder(m.Value);
         for (var i = sb.Length - 1; i >= 0; i--)
         {
            if (string.IsNullOrWhiteSpace(sb[i].ToString(CultureInfo.InvariantCulture)))
            {
               sb.Replace(sb[i].ToString(CultureInfo.InvariantCulture), string.Empty, i, 1);
            }
         }

         return sb.ToString();
      }

      [SuppressMessage("SonarLint.CodeSmell", "S3242: Method parameters should be declared with base types")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      private static string ReplaceTrailingWhitespace(Match m)
      {
         var pathUtilities = IocServiceLocator.Resolve<IPathUtilities>();
         var primary = pathUtilities.DirectorySeparatorCharacter;
         var alternate = pathUtilities.AltDirectorySeparatorCharacter;
         // match will have a value unescaped like the following "temp   \" or "temp    /"
         var sb = new StringBuilder(m.Value);
         var whiteSpaceStarted = false;
         for (var i = sb.Length - 1; i >= 0; i--)
         {
            if (!whiteSpaceStarted)
            {
               if (sb[0] == primary || sb[i] == alternate)
               {
                  // technically not needed, but I think it shows intent so I am leaving it
                  continue;
               }

               if (string.IsNullOrWhiteSpace(sb[i].ToString(CultureInfo.InvariantCulture)))
               {
                  whiteSpaceStarted = true;
               }
            }

            if (whiteSpaceStarted)
            {
               if (string.IsNullOrWhiteSpace(sb[i].ToString(CultureInfo.InvariantCulture)))
               {
                  sb.Replace(sb[i].ToString(CultureInfo.InvariantCulture), string.Empty, i, 1);
               }
               else
               {
                  // whitespace stopped, stop replacing
                  break;
               }
            }
         }

         return sb.ToString();
      }
   }
}
