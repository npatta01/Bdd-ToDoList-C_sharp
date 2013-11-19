using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using ToDoMvvm;

namespace TodoUiTest.steps
{
    [Binding]
    public class TaskSteps
    {
        private UIMap _map;

        private TaskListContext _tlc;

        public TaskSteps(TaskListContext tlc)
        {
            _tlc = tlc;
            _map = new UIMap();
        }


        [When(@"I edit task ""(.*)"" to ""(.*)""")]
        public void WhenIEditTaskTo(string oldTaskDescription, string newTaskDescription)
        {
            var id = _tlc.TaskId(oldTaskDescription);
            _tlc.UpdateTaskDescription(id, newTaskDescription);
            _map.EditTaskMessage(id, newTaskDescription);
        }

        [When(@"I delete task ""(.*)""")]
        public void WhenIDeleteTask(string taskDescription)
        {
            var id = _tlc.TaskId(taskDescription);
            _map.DeleteTask(id);
            _tlc.LastUpdatedTaskId = id;
        }


        [When(@"I add a task")]
        public void WhenIAddATask()
        {
            const string taskDescription = "Buy Milk";
            _tlc.AddTask(taskDescription);

            _map.AddTask(taskDescription);
        }


        [Then(@"the task should be ""(.*)""")]
        public void ThenTheTaskShouldBe(string taskStatus)
        {
            bool actualStatus = _map.IsTaskChecked(_tlc.LastAddedtTaskId);
            if (taskStatus == "Active")
            {
                Assert.IsTrue(actualStatus == false);
            }
            else
            {
                Assert.IsTrue(actualStatus);
            }
        }

        [Then(@"I should not see the task ""(.*)""")]
        public void ThenIShouldNotSeeTheTask(string taskDescription)
        {
            var id = _tlc.TaskId(taskDescription);
            Assert.IsFalse(_map.IsTaskVisible(id));
        }



        [Then(@"I should not see the task")]
        public void ThenIShouldNotSeeTheTask()
        {
            int id = _tlc.LastUpdatedTaskId;
            Assert.IsFalse(_map.IsTaskVisible(id));
        }

        [Then(@"I should see the task ""(.*)""")]
        public void ThenIShouldSeeTheTask(string taskDescription)
        {
            var id = _tlc.TaskId(taskDescription);

            Assert.AreEqual(_map.GetTaskMessage(id), taskDescription);
        }

        [Then(@"I should see the added task")]
        public void ThenIShouldSeeTheTask()
        {
            TaskItem ti = _tlc.LastAddedTask;

            Assert.AreEqual(_map.GetTaskMessage(ti.Id), ti.Description);
            Assert.AreEqual(_map.IsTaskChecked(ti.Id), ti.Completed);
        }
    }
}
