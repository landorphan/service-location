namespace Landorphan.InstrumentationManagement.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Landorphan.InstrumentationManagement.Interfaces;

    /// <inheritdoc cref="IInstrumentationContextManager"/>
    internal class InstrumentationContextManager : IInstrumentationContextManager
    {
        private readonly InstrumentationBootstrapData bootstrapData;
        //private readonly IInstrumentationPluginStorage sessionStorage;
        //private readonly IInstrumentationPluginStorage asyncStorage;
        //private readonly IInstrumentationPluginIdentityManager identityManager;
        //private readonly IInstrumentationPluginEntryPointStorage entryPointStorage;
        private readonly HashSet<string> propertyNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstrumentationContextManager"/> class.
        /// </summary>
        /// <param name="bootstrapData">
        /// The bootstrap data.
        /// </param>
        public InstrumentationContextManager(InstrumentationBootstrapData bootstrapData)
        {
            this.bootstrapData = bootstrapData;
            // this.sessionStorage = bootstrapData.SessionStorage;
            this.bootstrapData.SessionStorage.Set(nameof(SessionId), Guid.Empty);
            this.bootstrapData.SessionStorage.Set(nameof(SessionData), new Dictionary<string, string>());
            // this.asyncStorage = bootstrapData.AsyncStorage;
            // this.entryPointStorage = bootstrapData.EntryPointStorage;
            ExecutingApplicationName = bootstrapData.ApplicationName;
            // identityManager = bootstrapData.IdentityManager;
            propertyNames = new HashSet<string>();
            var propertyInfos = GetType().GetRuntimeProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                propertyNames.Add(propertyInfo.Name);
            }
        }

        public IEntryPointExecution ApplicationEntryPoint { get; internal set; }
        public IEntryPointExecution CurrentEntryPoint => bootstrapData.EntryPointStorage.CurrentEntryPoint;

        /// <inheritdoc />
        public string ExecutingApplicationName { get; internal set; }

        public bool IsInSession => SessionId != Guid.Empty;

        /// <inheritdoc />
        public string RootApplicationName => (string)bootstrapData.AsyncStorage.Get(nameof(RootApplicationName));

        /// <inheritdoc />
        public Guid SessionId => (Guid)bootstrapData.SessionStorage.Get(nameof(SessionId));

        /// <inheritdoc /> 
        public string UserAnonymousIdentity { get; internal set; }

        /// <inheritdoc />
        public object UserData => bootstrapData.SessionStorage.Get(nameof(UserData));

        /// <inheritdoc />
        public string UserIdentity => (string)bootstrapData.SessionStorage.Get(nameof(UserIdentity));

        private IDictionary<string, string> SessionData => (IDictionary<string, string>)bootstrapData.SessionStorage.Get(nameof(SessionData));

        public IEntryPointExecution CreateEntryPoint(string entryPointName)
        {
            var trace = bootstrapData.ApplicationPerformanceManager.StartTrace(entryPointName);
            var entryPoint = new EntryPointExecution(trace, entryPointName);
            return entryPoint;
        }

        /// <inheritdoc />
        public void EnterSession()
        {
            bootstrapData.SessionStorage.Set(nameof(SessionId), Guid.NewGuid());
            UserAnonymousIdentity = bootstrapData.IdentityManager.GetAnonymousUserId();
        }

        public string GetSessionData(string name)
        {
            SessionData.TryGetValue(name, out var result);
            return result;
        }

        /// <inheritdoc />
        public void IdentifyUser(string userId)
        {
            IdentifyUser(userId, null);
        }

        public void IdentifyUser(string userId, object userData)
        {
            if (!IsInSession)
            {
                EnterSession();
            }

            bootstrapData.SessionStorage.Set(nameof(UserIdentity), userId);
            bootstrapData.IdentityManager.IdentifyUser(userId, userData);
        }

        /// <inheritdoc />
        public void SetSessionData(string key, string value)
        {
            if (!propertyNames.Contains(key))
            {
                SessionData[key] = value;
            }
        }
    }
}
