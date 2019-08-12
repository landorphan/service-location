@Check-In
Feature: User Identity Manager
	In order to Instrument my code
	As a developer
	I want to bootstrap the Instrumentation system

Background: Given a proper Bootstrap is setup
     Given I setup bootstrap data with:
           | Field              | Value                          |
           | Application        | InstrumentationTestApplication |
           | SetBootstrapData   | True                           |
           | SetAsyncStorage    | True                           |
           | SetSessionStorage  | True                           |
           | SetIdentityManager | True                           |
      And I mock the user id to be '12345'

Scenario: When I bootstrap the system without an IdentityManager, the  system should not bootstrap
   Given I override bootstrap data with:
         | Field              | Value |
         | SetIdentityManager | False |
     And I bootstrap Instrumentation
    Then the value of Instrumentation.IsBootstrapped should be 'False'

Scenario: Identity Manager is not called on bootstrap
   Given I bootstrap Instrumentation
    Then the value of Context.UserAnonymousIdentity should be '(null)'

Scenario: Anonymous Identity is only called on session entry
   Given I bootstrap Instrumentation
     And I enter a session
    Then the value of Context.UserAnonymousIdentity should be '12345'

Scenario: When I'm not in a session then Is In Session should be false
   Given I bootstrap Instrumentation
    Then the value of Context.IsInSession should be 'False'

Scenario: When I enter a session then Is in session should be true
   Given I bootstrap Instrumentation
     And I enter a session
    Then the value of Context.IsInSession should be 'True'

Scenario: When I call identify user information is associated with the Anonymous identity
   Given I bootstrap Instrumentation
     And I enter a session 
    When I identify the user as 'me@mydomain.com'
    Then the value of Context.UserIdentity should be 'me@mydomain.com'

Scenario: When I call identify user and I'm not in a session, one is created
   Given I bootstrap Instrumentation
    When I identify the user as 'me@mydomain.com'
    Then the value of Context.IsInSession should be 'True'

Scenario: When I call identify user, the back end user system is alerted
   Given I bootstrap Instrumentation
     And I enter a session
    When I identify the user as 'me@mydomain.com'
    Then the value of Test.UserIdentity should be 'me@mydomain.com'

Scenario: If I don't set user data then no user data should be created
   Given I bootstrap Instrumentation
     And I enter a session
    When I identify the user as 'me@mydomain.com'
    Then the value of Test.UserData should be '(null)'

Scenario: User Data can be associated with a user's identity
   Given I bootstrap Instrumentation
     And I enter a session
     And I set the value of InternalUserData.Email to 'me@my.com'
    When I identify the user as 'me@mydomain.com'
    Then the value of Test.UserData should not be '(null)'
     And the value of UserData.Email should be 'me@my.com'
