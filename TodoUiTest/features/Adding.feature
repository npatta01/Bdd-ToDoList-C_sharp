Feature: Adding a task
	In order to be on schedule 
	I want to be be able to add a task

Scenario: Adding a single task to empty task list should update count
	Given I have no tasks
	When I add a task 
	Then I should see the added task 
	And the task should be "Active"
	And remaining task message to be "1 task left" 


Scenario: Adding multiple tasks to preexisting list should update count
	Given I have the following tasks
		| Description |
		| Buy Milk    |
		| Buy fish    |
    Then remaining task message to be "2 task left"
    When I add a task 
	Then remaining task message to be "3 task left"