using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   internal abstract class PathTokenizer
   {
      private char[] path;
      private int loc = 0;

      protected PathTokenizer(string path)
      {
      }
   }
}
