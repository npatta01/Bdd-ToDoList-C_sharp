Feature: Deleting
	In order to make my tasklist cleaner
	I want to delete all completed task
	I aslo want to delete an individual task
	
Background: 
 Given I have the following tasks
	| Description | Status   |
	| Buy Milk    | Active   |
	| Buy fish    | Complete |
	
Scenario: Deleting all completed task
	When  I clear completed tasks
	Then I should not see the task "Buy fish" 
	And remaining task message to be "1 task left"
	And completed task message to be ""


Scenario: Deleting a task
	When  I delete task "Buy Milk" 
	Then I should not see the task 
	And remaining task message to be ""
	And completed task message to be "Clear Completed (1)"

