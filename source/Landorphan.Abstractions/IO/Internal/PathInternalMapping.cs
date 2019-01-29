namespace Landorphan.Abstractions.IO.Internal
{
   using System;
   using System.Collections.Immutable;
   using System.Diagnostics.CodeAnalysis;
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
      private static readonly Lazy<IImmutableSet<Char>> t_invalidFileNameCharacters = new Lazy<IImmutableSet<Char>>(BuildInvalidFileNameCharacters);
      private static readonly Lazy<IImmutableSet<Char>> t_invalidPathCharacters = new Lazy<IImmutableSet<Char>>(BuildInvalidPathCharacters);
      private static readonly Lazy<IImmutableSet<Char>> t_additionalWindowsInvalidPathAndFileNameChars = new Lazy<IImmutableSet<Char>>(BuildAdditionalWindowsInvalidPathAndFileNameCharacters);

      /// <inheritdoc/>
      public Char AltDirectorySeparatorCharacter => Path.AltDirectorySeparatorChar;

      /// <inheritdoc/>
      public Char DirectorySeparatorCharacter => Path.DirectorySeparatorChar;

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

            // .Net Standard 2.0 on Windows: GetInvalidPathCharacters() excludes the following: '|'
            // .Net Standard 2.0 on Windows: GetInvalidFileNameCharacters() excludes the following: ',' '<' '>' '|'
            // TODO: But there are more characters disallowed on windows, and what about linux?
            if (indexNotFound != extension.IndexOfAny(GetInvalidPathCharacters().ToArray()) ||
                indexNotFound != extension.IndexOfAny(GetInvalidFileNameCharacters().ToArray()) ||
                indexNotFound != extension.IndexOfAny(GetAdditionalWindowsInvalidFileNameCharacters().ToArray()))
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
         // BCL Implementation notes:
         //    @"c:"          =>    null
         //    @"c:\"         =>    null
         //    @"c:\temp"     =>    @"c:\"
         //    @"c:\temp\"    =>    @"c:\temp\"
         //    @"c:\temp\\"   =>    @"c:\temp\"

         // adjust the behavior so that c:\temp\ yields c:\
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         cleanedPath = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(cleanedPath);

         var rv = Path.GetDirectoryName(cleanedPath);
         if (rv.IsNotNullNorEmpty())
         {
            rv = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(rv);
         }

         return rv;
      }

      /// <inheritdoc/>
      public String GetRootPath(String path)
      {
         var cleanedPath = IOStringUtilities.ValidateCanonicalPath(path, nameof(path));
         cleanedPath = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(cleanedPath);

         var rv = Path.GetPathRoot(cleanedPath);
         if (rv.IsNotNullNorEmpty())
         {
            rv = IOStringUtilities.RemoveOneTrailingDirectorySeparatorCharacter(rv);
         }

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
      public Boolean IsPathRooted(String path)
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

         return Path.IsPathRooted(cleanedPath);
      }

      private static IImmutableSet<Char> BuildAdditionalWindowsInvalidPathAndFileNameCharacters()
      {
         // taken from Windows File Explorer
         var rv = new[] {'\\', '/', ':', '*', '?', '<', '>', '|'}.ToImmutableHashSet();
         return rv;
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

      private IImmutableSet<Char> GetAdditionalWindowsInvalidFileNameCharacters()
      {
         return t_additionalWindowsInvalidPathAndFileNameChars.Value;
      }
   }
}
