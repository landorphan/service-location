using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   public class UnNormalizedPath : IPath
   {
      public String SuppliedString { get; private set; }

      public ISegment RootSegment { get; private set; }
      public PathStatus Status { get; private set; }
   }
}
