using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths
{
   public enum PathStatus
   {
      Legal,
      Discouraged,
      Illegal
   }

   public interface IPath
   {
      string SuppliedPathString { get; }

      ISegment LeadingSegment { get; }

      PathStatus Status { get; }

      ISegment[] Segments { get; }
   }
}
