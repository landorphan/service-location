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


