@Check-In
Feature: Session Management
	In order to Instrument my code
	As a developer
	I want to interact with sessions

Background: Given a proper Bootstrap is setup
     Given I setup bootstrap data with:
           | Field              | Value                          |
           | Application        | InstrumentationTestApplication |
           | SetBootstrapData   | True                           |
           | SetAsyncStorage    | True                           |
           | SetSessionStorage  | True                           |
           | SetIdentityManager | True                           |
       And I bootstrap Instrumentation
       And Instrumentation is bootstrapped

Scenario: The default session ID should be an empty guid
     Then the value of Context.SessionId should be '00000000-0000-0000-0000-000000000000'

Scenario: When I Enter a new session, the Session Id should change
     When I enter a session
     Then the value of Context.SessionId should not be '00000000-0000-0000-0000-000000000000'

Scenario: Session Data Storage is used to store the Session Id
     When I enter a session
      And I set the Session Storage Value 'SessionId' to '00000000-0000-0000-0000-000000000001'
     Then the value of Context.SessionId should be '00000000-0000-0000-0000-000000000001'

Scenario: Async Data Storage is used to store the RootApplicationName
     When I enter a session
      And I set the Async Storage Value 'RootApplicationName' to 'Foo'
     Then the value of Context.RootApplicationName should be 'Foo'

Scenario: Executing Application is held in neither the Session or the Async storage
     When I set the Async Storage Value 'ExecutingApplicationName' to 'Foo'
      And I set the Session Storage Value 'ExecutingApplicationName' to 'Bar'
     Then the value of Context.ExecutingApplicationName should be 'InstrumentationTestApplication'

Scenario: The RootApplicaitonName should be InstrumentationTestApplication
     Then the value of Context.RootApplicationName should be 'InstrumentationTestApplication'
