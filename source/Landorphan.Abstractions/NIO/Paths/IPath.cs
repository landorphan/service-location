﻿using System;
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
      ISegment RootSegment { get; }

      PathStatus Status { get; }
   }
}
