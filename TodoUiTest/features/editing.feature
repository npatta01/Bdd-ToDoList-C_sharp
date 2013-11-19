Feature: Editing a task description
	I want to be able to edit a task description

Background: 
Given I have the following tasks
		| Description | Status   |
		| Buy Milk    | Active   |

	
Scenario: Editing a task
	When  I edit task "Buy Milk" to "Buy LowfatMilk"
	Then I should see the task "Buy LowfatMilk"
	And remaining task message to be "1 task left"