Feature: Completing a task
	I want my tasklist to reflect that I have completed a task


	Background: 
	Given I have the following tasks
		| Description | Status   |
		| Buy Milk    | Active   |
		| Buy fish    | Complete |
	
Scenario: Completing a task
	When  I complete task "Buy Milk" 
	Then I should not see the task in "Active" Pane 
	And I should see the task in "Complete" Pane 
	And I should see the task in "All" Pane 
	And remaining task message to be ""
	And completed task message to be "Clear Completed (2)"


Scenario: Uncompleting a task
	When  I uncomplete task "Buy fish" 
	Then I should see the task in "Active" Pane 
	And I should not see the task in "Complete" Pane 
	And I should see the task in "All" Pane 
	And remaining task message to be "2 task left"
	And clear completed should be disabled