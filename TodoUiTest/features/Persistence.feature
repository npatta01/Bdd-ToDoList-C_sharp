Feature: Persistence
	I want my tasklist to be persisted 

Scenario: Persisting a task and verifying it exists
	Given I have no tasks
	When I add a task 
	And I close and reopen the app
	Then I should see the added task