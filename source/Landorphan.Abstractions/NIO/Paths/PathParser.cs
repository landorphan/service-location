using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths
{
   using System.Runtime.InteropServices;
   using Landorphan.Abstractions.NIO.Paths.Internal;

   public class PathParser : IPathParser
   {
      public IPath Parse(String pathString, OSPlatform platform)
      {
         var tokenizer = GetTokenizer(pathString, platform);
         var tokens = tokenizer.GetTokens();
         return null;
      }

      private PathTokenizer GetTokenizer(string pathString, OSPlatform platform)
      {
         if (platform == OSPlatform.Windows)
         {
            return new WindowsPathTokenizer(pathString);
         }

         return null;
      }
   }
}
