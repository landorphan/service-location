using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   public class NormalizedPath : IPath
   {
      internal static IPath EmptyPath { get; } = CreateEmptyPath();

      private static NormalizedPath CreateEmptyPath()
      {
         return new NormalizedPath()
         {
            RootSegment = Segment.EmptySegment,
            Status = PathStatus.Illegal
         };
      }

      public String SuppliedString { get; set; }
      public ISegment RootSegment { get; internal set; }
      public PathStatus Status { get; internal set; }
   }
}
