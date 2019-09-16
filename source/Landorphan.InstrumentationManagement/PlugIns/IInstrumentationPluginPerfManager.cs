namespace Landorphan.InstrumentationManagement.PlugIns
{
   using System;

   /// <summary>
   /// Provides for a current span that is executing inside of a trace.
   /// </summary>
   public interface IPerfSpan : IDisposable
   {
      /// <summary>
      /// Gets the span id for the current span.
      /// </summary>
      string SpanId { get; }
      /// <summary>
      /// Gets the parent span for the current span.
      /// </summary>
      IPerfSpan ParentSpan { get; }

      /// <summary>
      /// Gets the parent trace for this span.
      /// </summary>
      IPerfTrace ParentTrace { get; }

      /// <summary>
      /// Gets the name of the span.
      /// </summary>
      string Name { get; }
   }

   /// <summary>
   /// Provides an interface for the current trace.
   /// </summary>
   public interface IPerfTrace : IPerfSpan
   {
      /// <summary>
      /// Gets the trace Id for the current trace.
      /// </summary>
      string TraceId { get; }

      /// <summary>
      /// Gets the current span within the trace.
      /// </summary>
      IPerfSpan CurrentSpan { get;  }
   }

   /// <summary>
   /// Provides an interface for inter-operating with an
   /// application performance monitoring system.
   /// </summary>
   public interface IInstrumentationPluginPerfManager
   {
      /// <summary>
      /// Starts an APM trace.
      /// </summary>
      /// <returns>
      /// The trace object.
      /// </returns>
      IPerfTrace StartTrace(string traceName);
   }
}
