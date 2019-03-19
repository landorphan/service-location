using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths
{
   using System.Net.Http.Headers;
   using Landorphan.Abstractions.NIO.Paths.Internal;

   public class ParsedPath : IPath
   {
      internal static IPath CreateFromSegments(string suppliedPath, Segment[] segments)
      {
         Segment lastSegment = null;
         foreach (var segment in segments)
         {
            if (lastSegment != null)
            {
               lastSegment.NextSegment = segment;
            }

            lastSegment = segment;
         }

         ParsedPath retval = new ParsedPath();
         retval.Segments = segments;
         retval.LeadingSegment = segments[0];
         retval.Status = PathStatus.Undetermined;
         return retval;
      }

      public String SuppliedPathString { get; private set; }
      public ISegment LeadingSegment { get; private set; }
      public PathStatus Status { get; internal set; }
      public ISegment[] Segments { get; private set; }

      public IPath SuppliedPath => this;
   }
}
