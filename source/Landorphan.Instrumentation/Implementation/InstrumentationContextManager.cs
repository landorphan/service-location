namespace Landorphan.Instrumentation.Implementation
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Reflection;
   using Landorphan.Instrumentation.Interfaces;

   /// <inheritdoc cref="IInstrumentationContextManager"/>
   internal class InstrumentationContextManager : IInstrumentationContextManager
   {
      private readonly IInstrumentationStorage sessionStorage;
      private readonly IInstrumentationStorage asyncStorage;
      private readonly IInstrumentationIdentityManager identityManager;
      private readonly HashSet<string> propertyNames;

      /// <summary>
      /// Initializes a new instance of the <see cref="InstrumentationContextManager"/> class.
      /// </summary>
      /// <param name="bootstrapData">
      /// The bootstrap data.
      /// </param>
      public InstrumentationContextManager(InstrumentationBootstrapData bootstrapData)
      {
         this.sessionStorage = bootstrapData.SessionStorage;
         this.sessionStorage.Set(nameof(SessionId), new Guid());
         this.sessionStorage.Set(nameof(SessionData), new Dictionary<string, string>());
         this.asyncStorage = bootstrapData.AsyncStorage;
         this.ExecutingApplicationName = bootstrapData.ApplicationName;
         identityManager = bootstrapData.IdentityManager;
         propertyNames = new HashSet<string>();
         var propertyInfos = this.GetType().GetRuntimeProperties();
         foreach (var propertyInfo in propertyInfos)
         {
            propertyNames.Add(propertyInfo.Name);
         }
      }

      /// <inheritdoc />
      public string RootApplicationName => (string)asyncStorage.Get(nameof(RootApplicationName));

      /// <inheritdoc />
      public Guid SessionId => (Guid) sessionStorage.Get(nameof(SessionId));

      public bool IsInSession => SessionId != Guid.Empty;

      /// <inheritdoc />
      public string ExecutingApplicationName { get; internal set; }

      /// <inheritdoc /> 
      public string UserAnonymousIdentity { get; internal set; }

      /// <inheritdoc />
      public string UserIdentity => (string)sessionStorage.Get(nameof(UserIdentity));

      /// <inheritdoc />
      public object UserData => sessionStorage.Get(nameof(UserData));

      private IDictionary<string, string> SessionData => (IDictionary<string,string>) sessionStorage.Get(nameof(SessionData));

      /// <inheritdoc />
      public void EnterSession()
      {
         sessionStorage.Set(nameof(SessionId), Guid.NewGuid());
         UserAnonymousIdentity = identityManager.GetAnonymousUserId();
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
         sessionStorage.Set(nameof(UserIdentity), userId);
         identityManager.IdentifyUser(userId, userData);
      }
   }  
}
