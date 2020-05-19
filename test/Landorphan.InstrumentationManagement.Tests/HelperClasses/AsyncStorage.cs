namespace Landorphan.InstrumentationManagement.Tests.HelperClasses
{
    // ReSharper disable RedundantUsingDirective
#pragma warning disable S1128 // Unused "using" should be removed
#pragma warning disable CS8019 // Unnecessary using directive -- This is needed for some targets and not others.  Keeping in for all.
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using Landorphan.InstrumentationManagement.PlugIns;
#if NETFX
    using System.Runtime.Remoting.Messaging;
#endif

#pragma warning restore CS8019 // Unnecessary using directive
#pragma warning restore S1128 // Unused "using" should be removed
    public class AsyncStorage : IInstrumentationPluginStorage
    {
#if NETCORE
        private readonly AsyncLocal<ConcurrentDictionary<string, object>> asyncLocal = new AsyncLocal<ConcurrentDictionary<string, object>>();
#endif

        public object Get(string name)
        {
#if NETFX
            object retval = null;
            if (CallContext.GetData(nameof(AsyncStorage)) is ConcurrentDictionary<string, object> localStorage)
            {
                retval = localStorage[name];
            }

            return retval;
#elif NETCORE
            object retval = null;
            if (asyncLocal.Value is ConcurrentDictionary<string, object> localStorage)
            {
                retval = localStorage[name];
            }
            return retval;
#else
#pragma warning disable S3717 // Track use of "NotImplementedException"
            throw new NotImplementedException("This operation is not implemented for this runtime.");
#pragma warning restore S3717 // Track use of "NotImplementedException"
#endif
        }

        public void Set(string name, object value)
        {
#if NETFX
            if (!(CallContext.GetData(nameof(AsyncStorage)) is ConcurrentDictionary<string, object> localStorage))
            {
                localStorage = new ConcurrentDictionary<string, object>();
                CallContext.SetData(nameof(AsyncStorage), localStorage);
            }

            localStorage[name] = value;
#elif NETCORE
            if (!(asyncLocal.Value is ConcurrentDictionary<string, object> localStorage))
            {
                localStorage = new ConcurrentDictionary<string, object>();
                asyncLocal.Value = localStorage;
            }
            localStorage[name] = value;
#else
#pragma warning disable S3717 // Track use of "NotImplementedException"
            throw new NotImplementedException("This operation is not implemented for this runtime.");
#pragma warning restore S3717 // Track use of "NotImplementedException"
#endif
        }
    }
}
