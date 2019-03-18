using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths
{
   using Landorphan.Abstractions.NIO.Paths.Internal;

   public interface ISegment
   {
      SegmentType SegmentType { get; }

      string Name { get; }

      ISegment NextSegment { get; }
   }
}
