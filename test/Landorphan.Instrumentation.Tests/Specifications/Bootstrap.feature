Feature: Bootstrap
	In order to Instrument my code
	As a developer
	I want to bootstrap the Instrumentation system

Scenario: Bootstraping is required to use the system
	Given I do nothing
    When I call IsBootstraped
	 Then the return value should be 'false'

