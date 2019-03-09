namespace Landorphan.Abstractions.IO.Internal
{
   using System;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.IO;
   using System.Linq;
   using System.Text;
   using Landorphan.Abstractions.IO.Interfaces;
   using Landorphan.Common;

   /// <summary>
   /// Performs operations on <see cref="string"/> instances that contain file or directory path information.
   /// These operations are performed in a cross-platform manner.
   /// </summary>
   /// <remarks>
   /// Provides a one-to-one mapping to <see cref="Path"/> but in an object instance (as opposed to static) to support testability.
   /// </remarks>
   // Unlike File and Directory, the internal mapping presents the public interface.
   internal sealed class PathInternalMapping : IPathUtilities
   {
      private const Int32 IndexNotFound = -1;

      // skip leading @"/" or @"\"
      private const Int32 SkipPrefixCharacters = 2;

      private static readonly Lazy<IImmutableSet<Char>> t_invalidFileNameCharacters = new Lazy<IImmutableSet<Char>>(BuildInvalidFileNameCharacters);
      private static readonly Lazy<IImmutableSet<Char>> t_invalidPathCharacters = new Lazy<IImmutableSet<Char>>(BuildInvalidPathCharacters);

      /// <inheritdoc/>
      public Char AltDirectorySeparatorCharacter => Path.AltDirectorySeparatorChar;

      /// <inheritdoc/>
      public string AltDirectorySeparatorString
      {
         get 
         { 
            return AltDirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture);
         }
      }

      /// <inheritdoc/>
      public Char DirectorySeparatorCharacter => Path.DirectorySeparatorChar;

      /// <inheritdoc/>
      public string DirectorySeparatorString 
      {
         get 
         {
            return DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture);
         }
      }

      /// <inheritdoc/>
      public Char PathSeparatorCharacter => Path.PathSeparator;

      /// <inheritdoc/>
      public Char VolumeSeparatorCharacter => Path.VolumeSeparatorChar;

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      public String ChangeExtension(String path, String extension)
      {
         // edge-case implementation choice, an extension of "   .Foo" returns a {path}.   .Foo result.

         if (path.IsNullOrEmpty())
         {
            return path;
         }

         // allow for a string.Empty path and a spaces path.
         var cleanedPath = path.Trim(' ');
         if (cleanedPath.Length > 0)
         {
            // invoke method throws on string.Empty paths
            cleanedPath = IOStringUtilities.ValidateCanonicalPath(cleanedPath, nameof(path));
         }
         // KNOWN: path is canonically valid.

         String cleanedExtension;
         if (extension.IsNull() || extension.Length == 0 || extension.RightTrim(' ').Length == 0 || String.Equals(extension.RightTrim(' '), ".", StringComparison.Ordinal))
         {
            // an extension value of null removes the extension
            cleanedExtension = null;
         }
         else
         {
            const Int32 indexNotFound = -1;
            if (indexNotFound != extension.IndexOfAny(GetInvalidPathCharacters().ToArray()) ||
                indexNotFound != extension.IndexOfAny(GetInvalidFileNameCharacters().ToArray()))
            {
               throw new ArgumentException(@"The extension is not well-formed (invalid characters).", nameof(extension));
            }

            cleanedExtension = extension.RightTrim(' ');
            if (cleanedExtension[0] != '.')
            {
               cleanedExtension = '.' + cleanedExtension;
            }
         }
         // KNOWN: the extension is null (remove if exists), or canonically valid.  (Edge case, it is '.')

         if (cleanedPath.Length == 0)
         {
            return cleanedExtension ?? String.Empty;
         }

         // BCL implementation notes:  thrown argument exceptions have a parameter name of null.
         return Path.ChangeExtension(cleanedPath, cleanedExtension);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S109: Magic numbers should not be used")]
      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      public String Combine(params String[] paths)
      {
         // c:{additional path specification} is a path of the current directory on the c drive + specification
         // c:\{additional path specification} is a path on the c drive + specification
         paths.ArgumentNotNullNorContainsNull(nameof(paths));

         var sb = new StringBuilder();
         foreach (var path in paths)
         {
            var cleanedPath = IOStringUtilities.ConditionallyTrimSpaceFromPath(path);
            if (cleanedPath.Length == 0)
            {
               continue;
            }

            // allow for search(do not throw on ? *
            if (-1 != cleanedPath.IndexOfAny(GetInvalidPathCharacters().ToArray()))
            {
               throw new ArgumentException(@"The path is not well-formed (invalid characters).", nameof(paths));
            }

            if (cleanedPath.Last() != DirectorySeparatorCharacter && cleanedPath.Last() != AltDirectorySeparatorCharacter)
            {
               if (cleanedPath.Length == 2 && Char.IsLetter(cleanedPath[0]) && cleanedPath[1] == VolumeSeparatorCharacter)
               {
                  // preserve @"c:" as a result by not appending a directory separator character
               }
               else
               {
                  cleanedPath += DirectorySeparatorCharacter;
               }
            }

            sb.Append(cleanedPath);
         }

         // preserve string.Empty as result.
         if (sb.Length == 0)
         {
            return String.Empty;
         }

         var rv = sb.ToString();
         if (rv.Length <= 3)
         {
            // this handles \ / \\ c: and c:\
            return rv;
         }

         rv = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(rv);
         return rv;
      }

      /// <inheritdoc/>
      public String GetExtension(String path)
      {
         path.ArgumentNotNull(nameof(path));

         // allow for a string.Empty path and a spaces path.
         var cleanedPath = path.Trim(' ');
         if (cleanedPath.Length > 0)
         {
            // invoke method throws on string.Empty paths
            cleanedPath = IOStringUtilities.ValidateCanonicalPath(cleanedPath, nameof(path));
         }
         // KNOWN: path is string.Empty or canonically valid.

         if (cleanedPath.Length == 0)
         {
            return String.Empty;
         }

         var rv = Path.GetExtension(cleanedPath);
         return rv;
      }

      /// <inheritdoc/>
      public String GetFileName(String path)
      {
         path.ArgumentNotNull(nameof(path));

         // allow for a string.Empty path and a spaces path.
         var cleanedPath = path.Trim(' ');
         if (cleanedPath.Length > 0)
         {
            // invoke method throws on string.Empty paths
            cleanedPath = IOStringUtilities.ValidateCanonicalPath(cleanedPath, nameof(path));
         }
         // KNOWN: path is string.Empty or canonically valid.

         return Path.GetFileName(cleanedPath);
      }

      /// <inheritdoc/>
      public String GetFileNameWithoutExtension(String path)
      {
         path.ArgumentNotNull(nameof(path));

         // allow for a string.Empty path and a spaces path.
         var cleanedPath = path.Trim(' ');
         if (cleanedPath.Length > 0)
         {
            // invoke method throws on string.Empty paths
            cleanedPath = IOStringUtilities.ValidateCanonicalPath(cleanedPath, nameof(path));
         }
         // KNOWN: path is string.Empty or canonically valid.

         return Path.GetFileNameWithoutExtension(cleanedPath);
      }

      /// <inheritdoc/>
      public String GetFullPath(String path)
      {
         path.ArgumentNotNull(nameof(path));

         // allow for a string.Empty path and a spaces path.
         var cleanedPath = path.Trim(' ');
         if (cleanedPath.Length > 0)
         {
            // invoke method throws on string.Empty paths
            cleanedPath = IOStringUtilities.ValidateCanonicalPath(cleanedPath, nameof(path));
         }
         // KNOWN: path is string.Empty or canonically valid.

         if (cleanedPath.Length == 0)
         {
            return String.Empty;
         }

         // BCL behavior:
         //    leading spaces are left alone but trailing spaces are trimmed.
         //       @"c:\temp\ a \ b \" becomes @"c:\temp\ a\ b\"
         //    a trailing backslash is returned if one is given.
         //    a trailing backslash is not returned if none is given.
         return Path.GetFullPath(cleanedPath);
      }

      /// <inheritdoc/>
      public IImmutableSet<Char> GetInvalidFileNameCharacters()
      {
         // '"', '<', '>', '|', char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011',
         // '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F', ':', '*', '?', '\\', '/'
         return t_invalidFileNameCharacters.Value;
      }

      /// <inheritdoc/>
      public IImmutableSet<Char> GetInvalidPathCharacters()
      {
         // '|', char.MinValue, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\x000E', '\x000F', '\x0010', '\x0011', '\x0012', '\x0013',
         // '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001A', '\x001B', '\x001C', '\x001D', '\x001E', '\x001F' 
         return t_invalidPathCharacters.Value;
      }

      /// <inheritdoc/>
      public String GetParentPath(String path)
      {
         // ReSharper disable CommentTypo

         // want consistent treatment across rooted unc and relative paths (should aling with GetRootPath).

         //    Input                           Path.GetDirectoryName       IPathUtilities.GetParentPath
         // 
         //    @"c:"                      =>   null                        null                 
         //    @"c:\"                     =>   null                        null                 
         //    @"c:\temp"                 =>   @"c:\"                      @"c:\"               
         //    @"c:\temp\"                =>   @"c:\temp                   @"c:\"               
         //    @"c:\temp\\"               =>   @"c:\temp                   @"c:\"               invalid, throw?
         //    @"\\share"                 =>   null                        null                 
         //    @"\\share\file.txt"        =>   null                        @"\\share"           
         //    @"\\share\folder\file.txt" =>   @"\\share\folder            @"\\share\folder"    
         //    @"\relative\path"          =>   @"\relative                 @"\relative"         

         // ReSharper restore CommentTypo

         // adjust the behavior so that c:\temp\ yields c:\
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         cleanedPath = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(cleanedPath);

         var rv = Path.GetDirectoryName(cleanedPath);
         if (rv == null && cleanedPath.Length > SkipPrefixCharacters)
         {
            // special handling for \\share\file.txt and \\share\folder
            // Path.GetDirectoryName returns null in both of these cases by design
            // return \\share instead
            // cannot use Path.GetPathRoot because Path.GetPathRoot(@"\\share\file.txt") returns "\\share\file.txt"
            var separators = new[] {DirectorySeparatorCharacter, AltDirectorySeparatorCharacter};
            var length = cleanedPath.Length;
            var lastCharacterIndex = length - 1;
            var countSkippingPrefix = length - SkipPrefixCharacters;

            // searches backwards
            var idx = cleanedPath.LastIndexOfAny(separators, lastCharacterIndex, countSkippingPrefix);
            if (idx != IndexNotFound)
            {
               rv = cleanedPath.Substring(0, idx);
            }
         }

         if (rv != null)
         {
            const Int32 rootDrivePathLength = 3;
            if (rv.Length == rootDrivePathLength && rv[1] == VolumeSeparatorCharacter)
            {
               // want to return c:\ instead of c:
            }
            else if (rv.Length == rootDrivePathLength - 1 && rv[1] == VolumeSeparatorCharacter)
            {
               // want to return c:\ instead of c:
               rv += DirectorySeparatorCharacter;
            }
            else
            {
               // want to return c:\windows\system32 instead of c:\windows\system32\
               IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(cleanedPath);
            }
         }

         return rv;
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S1067: Expressions should not be too complex")]
      [SuppressMessage("SonarLint.CodeSmell", "S1541: Methods and properties should not be too complex")]
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S1871: Two branches in a conditional structure should not have exactly the same implementation",
         Justification = "The conditions are complex, combinging them would just make readability worse.")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high")]
      public String GetRootPath(String path)
      {
         // ReSharper disable CommentTypo

         // want consistent treatment across rooted unc and relative paths.

         //    Input                           Path.GetPathRoot            IPathUtilities.GetRootPath
         // 
         //    @"c:"                      =>    @"c:"                      @"c:\"
         //    @"c:\"                     =>    @"c:\"                     @"c:\"
         //    @"c:\temp"                 =>    @"c:\"                     @"c:\"
         //    @"c:\temp\"                =>    @"c:\"                     @"c:\"
         //    @"c:\temp\\"               =>    @"c:\"                     @"c:\"                        invalid, throw?
         //    @"\\share"                 =>    @"\\share"                 @"\\share"
         //    @"\\share\file.txt"        =>    @"\\share\file.txt"        @"\\share"
         //    @"\\share\folder\file.txt" =>    @"\\share\folder"          @"\\share"   
         //    @"\relative\path"          =>    @"\"                       should be @"\"
         //    @".\relative\path"         =>    String.Empty               should be @"\"  
         //    @"/relative/path"          =>    @"\"                       should be @"\"  
         //    @"./relative/path"         =>    String.Empty               should be @"\"
         //    @"somefileorfolder"        =>    String.Empty               should be String.Empty

         // ReSharper restore CommentTypo

         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         cleanedPath = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(cleanedPath);

         var rv = Path.GetPathRoot(cleanedPath);
         if (rv != null && rv.Length == 0 && cleanedPath.Length > @".\".Length && cleanedPath[0] == '.')
         {
            // occurs on relative paths
            if (cleanedPath[1] == DirectorySeparatorCharacter)
            {
               rv = DirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture);
            }
            else if (cleanedPath[1] == AltDirectorySeparatorCharacter)
            {
               rv = AltDirectorySeparatorCharacter.ToString(CultureInfo.InvariantCulture);
            }
         }

         var separators = new[] {DirectorySeparatorCharacter, AltDirectorySeparatorCharacter};

         if (cleanedPath.Length > SkipPrefixCharacters && String.Equals(rv, cleanedPath, StringComparison.Ordinal))
         {
            // special handling unc
            // Path.GetPathRoot(@"\\share")          returns @"\\share"
            // Path.GetPathRoot(@"\\share\folder")   returns @"\\share\folder"
            // Path.GetPathRoot(@"\\share\file.txt") returns @"\\share\file.txt"
            var idx = cleanedPath.IndexOfAny(separators, SkipPrefixCharacters);
            if (idx != IndexNotFound)
            {
               rv = cleanedPath.Substring(0, idx);
            }
         }
         else if (cleanedPath.Length > SkipPrefixCharacters &&
                  (
                     cleanedPath[0] == DirectorySeparatorCharacter && cleanedPath[1] == DirectorySeparatorCharacter ||
                     cleanedPath[0] == AltDirectorySeparatorCharacter && cleanedPath[1] == AltDirectorySeparatorCharacter))
         {
            // more special handling for unc
            // Path.GetPathRoot(@"\\share\folder\file.txt") returns @"\\share\folder"
            var idx = cleanedPath.IndexOfAny(separators, SkipPrefixCharacters);
            if (idx != IndexNotFound)
            {
               rv = cleanedPath.Substring(0, idx);
            }
         }

         const Int32 rootDrivePathLength = 3;
         if (rv != null && rv.Length == rootDrivePathLength && rv[1] == VolumeSeparatorCharacter)
         {
            // want to return c:\ instead of c:
         }
         else if (rv != null && rv.Length == rootDrivePathLength - 1 && rv[1] == VolumeSeparatorCharacter)
         {
            // want to return c:\ instead of c:
            rv += DirectorySeparatorCharacter;
         }

         // rv = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(rv);

         return rv;
      }

      /// <inheritdoc/>
      public Boolean HasExtension(String path)
      {
         path.ArgumentNotNull(nameof(path));

         // allow for a string.Empty path and a spaces path.
         var cleanedPath = path.Trim(' ');
         if (cleanedPath.Length > 0)
         {
            // invoke method throws on string.Empty paths
            cleanedPath = IOStringUtilities.ValidateCanonicalPath(cleanedPath, nameof(path));
         }
         // KNOWN: path is string.Empty or canonically valid.

         if (cleanedPath.Length == 0)
         {
            return false;
         }

         return Path.HasExtension(cleanedPath);
      }

      /// <inheritdoc/>
      public Boolean IsPathRelative(String path)
      {
         path.ArgumentNotNull(nameof(path));

         // allow for a string.Empty path and a spaces path.
         var cleanedPath = path.Trim();
         if (cleanedPath.Length > 0)
         {
            cleanedPath = IOStringUtilities.ValidateCanonicalPath(cleanedPath, nameof(path));
         }

         if (cleanedPath.IsNullOrEmpty())
         {
            return true;
         }

         // avoid Path.IsPathRooted altogether (look at the implementation if curious)
         var root = GetRootPath(cleanedPath);
         if (root != null && root.Length == 1 && (root[0] == DirectorySeparatorCharacter || root[0] == AltDirectorySeparatorCharacter))
         {
            // GetRootPath returns / and \ on relative paths
            return true;
         }

         return root.IsNullOrEmpty();
      }

      private static IImmutableSet<Char> BuildInvalidFileNameCharacters()
      {
         var builder = ImmutableHashSet<Char>.Empty.ToBuilder();
         foreach (var ch in Path.GetInvalidFileNameChars())
         {
            builder.Add(ch);
         }

         return builder.ToImmutable();
      }

      private static IImmutableSet<Char> BuildInvalidPathCharacters()
      {
         var builder = ImmutableHashSet<Char>.Empty.ToBuilder();
         foreach (var ch in Path.GetInvalidPathChars())
         {
            builder.Add(ch);
         }

         return builder.ToImmutable();
      }
   }
}
