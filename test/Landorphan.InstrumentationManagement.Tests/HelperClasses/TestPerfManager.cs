namespace Landorphan.InstrumentationManagement.Tests.HelperClasses
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Landorphan.Common;
    using Landorphan.InstrumentationManagement.PlugIns;

    public class TestPerfSpan : DisposableObject, IPerfSpan
   {
       protected internal Guid spanId = Guid.NewGuid();

       public TestPerfSpan(string name)
      {
         Name = name;
         ParentSpan = this;
      }

       public TestPerfSpan(string name, TestPerfTrace trace, TestPerfSpan parent)
      {
         Name = name;
         ParentTrace = trace;
         ParentSpan = parent;
      }

       protected override void ReleaseManagedResources()
      {
         base.ReleaseUnmanagedResources();
         ((TestPerfTrace)ParentTrace).CurrentSpan = ParentSpan;
      }

       public string Name { get; private set; }

       public IPerfSpan ParentSpan { get; protected set; }
       public IPerfTrace ParentTrace { get; set; }

       public string SpanId => spanId.ToString();
   }

   public class TestPerfTrace : TestPerfSpan, IPerfTrace
   {
       internal static IDictionary<Guid, TestPerfTrace> traces = new ConcurrentDictionary<Guid, TestPerfTrace>();
       private readonly Guid traceId = Guid.NewGuid();

       public TestPerfTrace(string name) : base (name)
      {
         ParentTrace = this;
         traces.Add(traceId, this);
         CurrentSpan = this;
      }

       protected override void ReleaseUnmanagedResources()
      {
         base.ReleaseUnmanagedResources();
         traces.Remove(traceId);
      }

       public IPerfSpan CurrentSpan { get; internal set; }

       public string TraceId => traceId.ToString();
   }

   public class TestPerfManager : IInstrumentationPluginPerfManager
   {
       public IPerfTrace StartTrace(string traceName)
      {
         return new TestPerfTrace(traceName);
      }
   }
}
