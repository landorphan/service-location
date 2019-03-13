using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   internal class DeviceSegment : Segment
   {
      public DeviceSegment(string device) : base(device, null)
      {
      }
   }
}
