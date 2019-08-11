namespace Landorphan.Instrumentation.Implementation
{
   using System;
   using Landorphan.Instrumentation.Interfaces;

   /// <inheritdoc cref="IInstrumentationContextManager"/>
   internal class InstrumentationContextManager : IInstrumentationContextManager
   {
      private readonly IInstrumentationStorage sessionStorage;
      private readonly IInstrumentationStorage asyncStorage;
      private readonly IInstrumentationIdentityManager identityManager;

      /// <summary>
      /// Initializes a new instance of the <see cref="InstrumentationContextManager"/> class.
      /// </summary>
      /// <param name="sessionStorage">
      /// The session storage system.
      /// </param>
      /// <param name="asyncStorage">
      /// The async storage system.
      /// </param>
      public InstrumentationContextManager(InstrumentationBootstrapData bootstrapData)
      {
         this.sessionStorage = bootstrapData.SessionStorage;
         this.sessionStorage.Set(nameof(SessionId), new Guid());
         this.asyncStorage = bootstrapData.AsyncStorage;
         this.ExecutingApplicationName = bootstrapData.ApplicationName;
         identityManager = bootstrapData.IdentityManager;
      }

      /// <inheritdoc />
      public string RootApplicationName => (string)asyncStorage.Get(nameof(RootApplicationName));

      /// <inheritdoc />
      public Guid SessionId => (Guid) sessionStorage.Get(nameof(SessionId));

      /// <inheritdoc />
      public string ExecutingApplicationName { get; internal set; }

      /// <inheritdoc /> 
      public string UserAnonymousIdentity { get; internal set; }

      /// <inheritdoc />
      public string UserIdentity => (string)sessionStorage.Get(nameof(UserIdentity));

      /// <inheritdoc />
      public void EnterSession()
      {
         sessionStorage.Set(nameof(SessionId), Guid.NewGuid());
         UserAnonymousIdentity = identityManager.GetAnonymousUserId();
      }

      /// <inheritdoc />
      public void IdentifyUser(string userId)
      {
         sessionStorage.Set(nameof(UserIdentity), userId);
      }
   }  
}
