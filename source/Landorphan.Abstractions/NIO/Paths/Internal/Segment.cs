using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   internal class Segment : ISegment
   {
      public static ISegment NullSegment { get; } = new NullSegment();

      public static ISegment EmptySegment { get; } = new EmptySegment();

      public Segment(char character) : this(character.ToString())
      {
      }

      public Segment(string name)
      {
         this.Name = name;
      }

      public Segment(string name, ISegment nextSegment) : this(name)
      {
         this.NextSegment = nextSegment;
      }

      public String Name { get; protected set; }
      public ISegment NextSegment { get; set; }
   }
}
