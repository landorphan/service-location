@Check-In
Feature: Entry Point Management
	In order to Instrument my code
	As a developer
	I want to be able to manage entry points

Background: Given a proper Bootstrap is setup
     Given I setup bootstrap data with:
           | Field                     | Value                          |
           | Application               | InstrumentationTestApplication |
           | SetBootstrapData          | True                           |
           | SetAsyncStorage           | True                           |
           | SetSessionStorage         | True                           |
           | SetIdentityManager        | True                           |
           | ApplicationEntryPointName | Main                           |
           | SetEntryPointStorage      | True                           |
           | SetPerfManager            | True                           |
           | SetLogger                 | True                           |

Scenario: When I bootstrap the system the Application Name is set 
   Given I bootstrap Instrumentation
    Then the value of Instrumentation.IsBootstrapped should be 'True'
     And the value of ApplicationEntryPoint.EntryPointName should be 'Main'
     And the value of CurrentEntryPoint.EntryPointName should be 'Main'

Scenario: The current entry point can be changed by entering a new entry point (desktop).
   Given I bootstrap Instrumentation
    When the user opens the form 'MyForm'
    Then the value of ApplicationEntryPoint.EntryPointName should be 'Main'
     And the value of CurrentEntryPoint.EntryPointName should be 'MyForm'

Scenario: The current entry point can be changed by entering a new entry point (web).
   Given I bootstrap Instrumentation
    When the user requests an action from the web method 'GetWebMethod1'
    Then the value of ApplicationEntryPoint.EntryPointName should be 'Main'
     And the value of CurrentEntryPoint.EntryPointName should be 'GetWebMethod1'

Scenario Outline: The current entry point can be changed (back and forth) by entering a new entry point (desktop).
   Given I bootstrap Instrumentation
    When the user opens the form 'MyForm1'
     And the user opens the form 'MyForm2'
     And actions occur within the form '<Form>'
    Then the value of ApplicationEntryPoint.EntryPointName should be 'Main'
     And the value of CurrentEntryPoint.EntryPointName should be '<Form>'
Examples: 
   | Form    |
   | MyForm1 |
   | MyForm2 |
   
Scenario Outline: The current entry point can be changed (back and forth) by entering a new entry point (web).
   Given I bootstrap Instrumentation
    When the user requests an action from the web method 'PostWebMethod1'
     And the user requests an action from the web method 'PostWebMethod2'
     And actions occur within the web method '<WebMethod>'
    Then the value of ApplicationEntryPoint.EntryPointName should be 'Main'
     And the value of CurrentEntryPoint.EntryPointName should be '<WebMethod>'
Examples: 
   | WebMethod    |
   | PostWebMethod1 |
   | PostWebMethod2 |


