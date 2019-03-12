using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   public class Path : IPath
   {
      internal static IPath EmptyPath { get; } = CreateEmptyPath();

      private static Path CreateEmptyPath()
      {
         return new Path()
         {
            RootSegment = Segment.EmptySegment,
            Status = PathStatus.Illegal
         };
      }

      public ISegment RootSegment { get; internal set; }
      public PathStatus Status { get; internal set; }
   }
}
