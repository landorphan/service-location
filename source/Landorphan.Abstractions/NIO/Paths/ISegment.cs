using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths
{
   public interface ISegment
   {
      string Name { get; }

      ISegment NextSegment { get; }
   }
}
