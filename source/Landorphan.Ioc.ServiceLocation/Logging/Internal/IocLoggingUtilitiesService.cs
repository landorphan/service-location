namespace Landorphan.Ioc.Logging.Internal
{
    using System;
    using System.Globalization;
    using System.Threading;

    internal sealed class IocLoggingUtilitiesService : IIocLoggingUtilitiesService
    {
        internal IocLoggingUtilitiesService()
        {
            LoggingUtilitiesForIocIocServiceLocator = new LoggingUtilitiesForIocServiceLocator(this);
            LoggingUtilitiesForIocContainer = new LoggingUtilitiesForIocContainer(this);
        }

        ///<inheritdoc/>
        public ILoggingUtilitiesForIocContainer LoggingUtilitiesForIocContainer { get; }

        ///<inheritdoc/>
        public ILoggingUtilitiesForIocServiceLocator LoggingUtilitiesForIocIocServiceLocator { get; }

        /////<inheritdoc/>
        //public EventId GetEventId(Int32 iocEventIdCode)
        //{
        //   // lazily load 
        //   var was = _eventIds;
        //   if (was.TryGetValue(iocEventIdCode, out var eventId))
        //   {
        //      return eventId;
        //   }

        //   {
        //      // build the struct (EventId) and add it to the set
        //      // TODO: MWP improve the implementation of name
        //      var name = String.Empty;
        //      var newEventId = new EventId(iocEventIdCode, name);
        //      var builder = _eventIds.ToBuilder();
        //      builder.Add(newEventId);
        //      _eventIds = builder.ToImmutable();
        //      return newEventId;
        //   }
        //}

        ///<inheritdoc/>
        public string GetThreadId()
        {
            var rv = Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture);
            return rv;
        }

        ///<inheritdoc/>
        public string GetTimestamp()
        {
            var rv = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
            return rv;
        }
    }
}
