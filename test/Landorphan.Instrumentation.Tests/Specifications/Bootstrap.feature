@Check-In
Feature: Bootstrap
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

Scenario: Bootstrapping is required to use the system
	Given I do nothing
    Then the value of Instrumentation.IsBootstrapped should be 'False'

Scenario: When I bootstrap the system the Application Name is set
   Given I bootstrap Instrumentation
    Then the value of Context.RootApplicationName should be 'InstrumentationTestApplication'

Scenario: When I bootstrap the system without an AsyncStorage, the  system should not bootstrap
   Given I override bootstrap data with:
         | Field           | Value |
         | SetAsyncStorage | False |
     And I bootstrap Instrumentation
    Then the value of Instrumentation.IsBootstrapped should be 'False'

Scenario: When I bootstrap the system without an SetSessionStorage, the  system should not bootstrap
   Given I override bootstrap data with:
         | Field             | Value |
         | SetSessionStorage | False |
     And I bootstrap Instrumentation
    Then the value of Instrumentation.IsBootstrapped should be 'False'

Scenario: When I bootstrap the system without an Bootstrap Data, the  system should not bootstrap
   Given I override bootstrap data with:
         | Field            | Value |
         | SetBootstrapData | False |
     And I bootstrap Instrumentation
    Then the value of Instrumentation.IsBootstrapped should be 'False'
