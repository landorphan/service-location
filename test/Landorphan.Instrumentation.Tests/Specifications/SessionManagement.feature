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

Scenario: General Data can be associated with the session
     When I set a session data value of 'Location' to 'Here'
     Then the session data value 'Location' should be 'Here'

Scenario Outline: Well known properties (such as SessionId) can not be overridden by setting session data
     When I set a session data value of '<Well Known Property>' to '<Set Value>'
     Then the value of <Context Reference> should be '<Expected Value>'
          # Because having a 'well known property' defined in two separate locations could confuse the
          # logging system they will not be allowed even in the session data collection which is kept
          # separate from other session data used for well known properties.
      And the session data value '<Well Known Property>' should be '(null)'
Examples:
| Well Known Property      | Context Reference                | Set Value                            | Expected Value                       |
| SessionId                | Context.SessionId                | 00000000-0000-0000-0000-000000000001 | 00000000-0000-0000-0000-000000000000 |
| RootApplicationName      | Context.RootApplicationName      | SomeApplicationName                  | InstrumentationTestApplication       |
| ExecutingApplicationName | Context.ExecutingApplicationName | SomeApplicaitonName                  | InstrumentationTestApplication       |
# As no user identification methods have been called, then the expected value of all user data should be null
| UserAnonymousIdentity    | Context.UserAnonymousIdentity    | notme@notmydomain.com                | (null)                               |
| UserIdentity             | Context.UserIdentity             | not_a_user_identity                  | (null)                               |
| UserData                 | Context.UserData                 | not_valid_user_data                  | (null)                               |
