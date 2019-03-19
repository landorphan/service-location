using System;
using System.Collections.Generic;
using System.Text;

namespace Landorphan.Abstractions.NIO.Paths.Internal
{
   using System.Linq;

   internal class Segment : ISegment
   {
      public static readonly string[] DeviceNames = new String[]
      {
         "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8",
         "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
      };
      public static readonly Segment NullSegment = new Segment(SegmentType.NullSegment, null);
      public static readonly Segment EmptySegment = new Segment(SegmentType.EmptySegment, string.Empty);
      public static readonly Segment SelfSegment = new Segment(SegmentType.SelfSegment, ".");
      public static readonly Segment ParentSegment = new Segment(SegmentType.ParentSegment, "..");

      public static Segment ParseFromString(string input)
      {
         if (input == ".")
         {
            return SelfSegment;
         }
         if (input == "..")
         {
            return ParentSegment;
         }
         if (input == null)
         {
            return NullSegment;
         }
         if (input.Length == 0)
         {
            return EmptySegment;
         }
         else if (IsDeviceSegment(input))
         {
            return new Segment(SegmentType.DeviceSegment, input);
         }
         else
         {
            return new Segment(SegmentType.GenericSegment, input);
         }
      }

      public static bool IsDeviceSegment(string input)
      {
         return DeviceNames.Contains(input);
      }

      public Segment(SegmentType type, string name)
      {
         this.SegmentType = type;
         this.Name = name;
      }

      public SegmentType SegmentType { get; protected set; }
      public String Name { get; protected set; }
      public ISegment NextSegment { get; set; }
   }
}
