using System;
using System.Collections.Generic;

namespace Landorphan.Instrumentation.Tests.HelperClasses
{
   using System.Collections.Concurrent;
   using Landorphan.Common;
   using Landorphan.Instrumentation.PlugIns;

   public class TestPerfSpan : DisposableObject, IPerfSpan
   {
      protected internal Guid spanId = Guid.NewGuid();

      public TestPerfSpan(string name)
      {
         this.Name = name;
         this.ParentSpan = this;
      }

      public TestPerfSpan(string name, TestPerfTrace trace, TestPerfSpan parent)
      {
         this.Name = name;
         this.ParentTrace = trace;
         this.ParentSpan = parent;
      }

      protected override void ReleaseManagedResources()
      {
         base.ReleaseUnmanagedResources();
         ((TestPerfTrace)this.ParentTrace).CurrentSpan = this.ParentSpan;
      }

      public string SpanId => spanId.ToString();

      public IPerfSpan ParentSpan { get; protected set; }
      public IPerfTrace ParentTrace { get; set; }
      public string Name { get; private set; }
   }

   public class TestPerfTrace : TestPerfSpan, IPerfTrace
   {
      internal static IDictionary<Guid, TestPerfTrace> traces = new ConcurrentDictionary<Guid, TestPerfTrace>();
      private readonly Guid traceId = Guid.NewGuid();

      public TestPerfTrace(string name) : base (name)
      {
         this.ParentTrace = this;
         traces.Add(traceId, this);
         CurrentSpan = this;
      }

      protected override void ReleaseUnmanagedResources()
      {
         base.ReleaseUnmanagedResources();
         traces.Remove(traceId);
      }

      public string TraceId => traceId.ToString();
      public IPerfSpan CurrentSpan { get; internal set; }   
   }

   public class TestPerfManager : IInstrumentationPluginPerfManager
   {
      public IPerfTrace StartTrace(string traceName)
      {
         return new TestPerfTrace(traceName);
      }
   }
}
