using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths
{
   using System.Runtime.InteropServices;

   public interface IPathParser
   {
      IPath Parse(string pathString, OSPlatform platform);
   }
}
