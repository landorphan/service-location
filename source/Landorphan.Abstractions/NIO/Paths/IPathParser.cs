using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths
{
   public interface IPathParser
   {
      IPath Parse(string pathString);
   }
}
