using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths
{
   using System.Linq;
   using System.Runtime.InteropServices;
   using Landorphan.Abstractions.NIO.Paths.Internal;

   public class PathParser : IPathParser
   {
      public IPath Parse(String pathString, OSPlatform platform)
      {
         var tokenizer = GetTokenizer(pathString, platform);
         var tokens = tokenizer.GetTokens();
         var rawSegments = this.GetSegments(tokens, platform);
         return ParsedPath.CreateFromSegments(pathString, rawSegments);
         return null;
      }

      internal Segment[] GetSegments(string [] tokens, OSPlatform platform)
      {
         if (OSPlatform.Windows == platform)
         {
            return GetWindowsSegments(tokens);
         }
         else
         {
            return GetPosixSegments(tokens);
         }
      }

      internal Segment[] GetPosixSegments(string[] tokens)
      {
         throw new NotImplementedException();
      }

      internal Segment[] GetWindowsSegments(string[] tokens)
      {
         IList<Segment> segments = new List<Segment>();

         for (int i = 0; i<tokens.Length; i++)
         {
            if (i == 0)
            {
               if (tokens[i].StartsWith("UNC:"))
               {
                  segments.Add(new Segment(SegmentType.UncSegment, tokens[i].Substring(4)));
               }
               else if (tokens[i].Contains(":"))
               {
                  var parts = tokens[i].Split(':');
                  if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
                  {
                     segments.Add(new Segment(SegmentType.VolumeRelativeSegment, parts[0] + ":"));
                     segments.Add(Segment.ParseFromString(parts[1]));
                  }
                  else
                  {
                     segments.Add(new Segment(SegmentType.RootSegment, parts[0] + ":"));
                  }
               }
               else if (tokens.Length == 1)
               {
                  if (tokens[i] == null)
                  {
                     segments.Add(Segment.NullSegment);
                  }
                  if (tokens[i] == string.Empty)
                  {
                     segments.Add(Segment.EmptySegment);
                  }
                  else
                  {
                     segments.Add(Segment.ParseFromString(tokens[i]));
                  }
               }
               else if (tokens[i] == string.Empty)
               {
                  segments.Add(Segment.EmptySegment);
                  var name = tokens[++i];
                  segments.Add(new Segment(SegmentType.VolumelessRootSegment, name));
               }
               else
               {
                  segments.Add(Segment.ParseFromString(tokens[i]));
               }
            }
            else if (i >= 1)
            {
               segments.Add(Segment.ParseFromString(tokens[i]));
            }
         }

         return segments.ToArray();
      }

      private PathTokenizer GetTokenizer(string pathString, OSPlatform platform)
      {
         if (platform == OSPlatform.Windows)
         {
            return new WindowsPathTokenizer(pathString);
         }

         return null;
      }
   }
}
