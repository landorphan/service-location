using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   internal class NullSegment : Segment
   {
      public NullSegment() : base(null, null)
      {

      }
   }
}
