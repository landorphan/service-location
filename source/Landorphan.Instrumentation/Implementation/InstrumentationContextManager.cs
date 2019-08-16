namespace Landorphan.Instrumentation.Implementation
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Reflection;
   using Landorphan.Instrumentation.Interfaces;
   using Landorphan.Instrumentation.PlugIns;

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
         this.bootstrapData.SessionStorage.Set(nameof(SessionId), new Guid());
         this.bootstrapData.SessionStorage.Set(nameof(SessionData), new Dictionary<string, string>());
         // this.asyncStorage = bootstrapData.AsyncStorage;
         // this.entryPointStorage = bootstrapData.EntryPointStorage;
         this.ExecutingApplicationName = bootstrapData.ApplicationName;
         // identityManager = bootstrapData.IdentityManager;
         propertyNames = new HashSet<string>();
         var propertyInfos = this.GetType().GetRuntimeProperties();
         foreach (var propertyInfo in propertyInfos)
         {
            propertyNames.Add(propertyInfo.Name);
         }
      }

      /// <inheritdoc />
      public string RootApplicationName => (string)this.bootstrapData.AsyncStorage.Get(nameof(RootApplicationName));

      public IEntryPointExecution ApplicationEntryPoint { get; internal set; }
      public IEntryPointExecution CurrentEntryPoint => this.bootstrapData.EntryPointStorage.CurrentEntryPoint;

      /// <inheritdoc />
      public Guid SessionId => (Guid) this.bootstrapData.SessionStorage.Get(nameof(SessionId));

      public bool IsInSession => SessionId != Guid.Empty;

      /// <inheritdoc />
      public string ExecutingApplicationName { get; internal set; }

      /// <inheritdoc /> 
      public string UserAnonymousIdentity { get; internal set; }

      /// <inheritdoc />
      public string UserIdentity => (string)this.bootstrapData.SessionStorage.Get(nameof(UserIdentity));

      /// <inheritdoc />
      public object UserData => this.bootstrapData.SessionStorage.Get(nameof(UserData));

      private IDictionary<string, string> SessionData => (IDictionary<string,string>)this.bootstrapData.SessionStorage.Get(nameof(SessionData));

      /// <inheritdoc />
      public void EnterSession()
      {
         this.bootstrapData.SessionStorage.Set(nameof(SessionId), Guid.NewGuid());
         UserAnonymousIdentity = this.bootstrapData.IdentityManager.GetAnonymousUserId();
      }

      /// <inheritdoc />
      public void SetSessionData(string key, string value)
      {
         if (!propertyNames.Contains(key))
         {
            SessionData[key] = value;
         }
      }

      public string GetSessionData(string key)
      {
         SessionData.TryGetValue(key, out var result);
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
         this.bootstrapData.SessionStorage.Set(nameof(UserIdentity), userId);
         this.bootstrapData.IdentityManager.IdentifyUser(userId, userData);
      }

      public IEntryPointExecution CreateEntryPoint(string entryPointName)
      {
         var trace = this.bootstrapData.ApplicationPerformanceManager.StartTrace(entryPointName);
         return new EntryPointExecution(trace, entryPointName);
      }
   }  
}
