using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;
using ToDoMvvm;

namespace TodoUiTest.steps
{
    [Binding]
    public class TaskListSteps
    {
        private UIMap _map;

       
        //context
        private readonly TaskListContext _tlc;

        public TaskListSteps(TaskListContext tlc)
        {
            _tlc = tlc;
            _map = new UIMap();
        }

        [Given(@"I have no tasks")]
        public void GivenIHaveNoTasks()
        {
            Assert.AreEqual(_map.TaskCount(), 0);
        }

        [Given(@"I have the following tasks")]
        public void GivenIHaveTheFollowingTasks(Table table)
        {
            //add list of task
            foreach (var taskName in table.Rows)
            {
                string taskDescription = taskName["Description"];
                string status;
                taskName.TryGetValue("Status", out status);
                bool boolstatus = status == "Complete";
                _tlc.AddTask(taskDescription, boolstatus);
                _map.AddTask(taskDescription);
                if (boolstatus)
                {
                    _map.ToggleTask(_tlc.LastAddedtTaskId);
                }
            }
        }

       

       

        [Then(@"clear completed should be disabled")]
        public void ThenClearCompletedShouldBeDisabled()
        {
            Assert.IsFalse(_map.IsClearCompletedTaskVisible());
        }

        [When(@"I uncomplete task ""(.*)""")]
        [When(@"I complete task ""(.*)""")]
        public void WhenICompleteTask(string taskDescription)
        {
            var id = _tlc.TaskId(taskDescription);
            _map.ToggleTask(id);
            _tlc.LastUpdatedTaskId = id;
        }

        [Then(@"I should not see the task in ""(.*)"" Pane")]
        public void ThenIShouldNotSeeTheTaskInPane(string pane)
        {
            SwitchPane(pane);
            var lastUpdatedId = _tlc.LastUpdatedTaskId;
            Assert.IsFalse(_map.IsTaskVisible(lastUpdatedId));
        }

        /// <summary>
        /// Switch task pane
        /// </summary>
        /// <param name="pane"></param>
        private void SwitchPane(string pane)
        {
            switch (pane)
            {
                case "Active":
                    _map.SwitchPane(TaskListState.Active);
                    break;

                case "Complete":
                    _map.SwitchPane(TaskListState.Completed);
                    break;

                case "All":
                    _map.SwitchPane(TaskListState.All);
                    break;

                default:
                    throw new Exception("Unexpected");
            }
        }

        [Then(@"I should see the task in ""(.*)"" Pane")]
        public void ThenIShouldSeeTheTaskInPane(string pane)
        {
            int id = _tlc.LastUpdatedTaskId;
            SwitchPane(pane);
            Assert.IsTrue(_map.IsTaskVisible(id));
        }

        [Then(@"I should see the clear completed button")]
        public void ThenIShouldSeeTheClearCompletedButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"completed task message to be ""(.*)""")]
        public void ThenCompletedTaskMessageToBe(string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, _map.ClearCompletedMessage());
        }

        [When(@"I clear completed tasks")]
        public void WhenIClearCompletedTasks()
        {
            _map.ClearCompletedTasks();
        }

        [When(@"I close and reopen the app")]
        public void WhenICloseAndReopenTheApp()
        {
            SetupSteps.ReopenApp();
        }

     

        [Then(@"remaining task message to be ""(.*)""")]
        public void ThenRemainingTaskMessageToBe(string p0)
        {
            Assert.AreEqual(_map.TaskRemainingMessage(), p0);
        }

     

        
    }
}