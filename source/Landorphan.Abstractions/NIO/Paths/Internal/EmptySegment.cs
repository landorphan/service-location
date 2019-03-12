using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   internal class EmptySegment : Segment
   {
      public EmptySegment() : base(string.Empty, null)
      {
      }
   }
}
