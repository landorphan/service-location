@Check-In
Feature: Bootstrap
	In order to Instrument my code
	As a developer
	I want to bootstrap the Instrumentation system

Scenario: Bootstraping is required to use the system
	Given I do nothing
    When I evaluate Instrumentation.IsBootstraped
	 Then the return value should be 'False'

#Scenario: When I bootstrap the system the Applicaiton Name is set
#   Given I bootstrap Instrumentation
#    When I evaluate Instrumentation.Context.ApplicationName
#    Then the return value should be 'instrumentationTest'
