using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
using ToDoMvvm;

namespace TodoUiTest
{
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using Microsoft.VisualStudio.TestTools.UITesting;

    public partial class UIMap
    {
        public void AddTask(string task)
        {
            this.UIToDoListWindow.NewTaskForm.Text = task;

          Mouse.Click(this.UIToDoListWindow.AddTaskBtn);


        }

        public string TaskRemainingMessage()
        {
            return UIToDoListWindow.TaskLeftLabel.DisplayText;
        }


        public string ClearCompletedMessage()
        {
            return UIToDoListWindow.ClearCompletedButton.DisplayText;
            // return UIToDoListWindow.ClearCompletedButton.C
        }

       

        public string GetTaskMessage(int id)
        {
            //WpfEdit uiTask1DescriptionEdit = UIToDoListWindow.TaskList.TaskItemListItem.UITask_1DescriptionEdit;
            string seekedAutomationId = "Task_" + id + "-Description";

            var listItem = getListItem(id);
            var t = new WpfEdit(listItem);

            t.SearchProperties[WpfEdit.PropertyNames.AutomationId] = seekedAutomationId;
            t.WindowTitles.Add("ToDoList");

            return t.Text;
        }



        public void EditTaskMessage(int id, string newDescription)
        {
            //WpfEdit uiTask1DescriptionEdit = UIToDoListWindow.TaskList.TaskItemListItem.UITask_1DescriptionEdit;
            string seekedAutomationId = "Task_" + id + "-Description";

            var listItem = getListItem(id);
            var t = new WpfEdit(listItem);

            t.SearchProperties[WpfEdit.PropertyNames.AutomationId] = seekedAutomationId;
            t.WindowTitles.Add("ToDoList");

            t.Text = newDescription;
        }



        public void SwitchPane(TaskListState state)
        {
            var options = this.UIToDoListWindow.UIItemList;
            switch (state)
            {
                case TaskListState.All:
                    Mouse.Click(options.UIAllListItem);
                    break;
                case TaskListState.Active:
                    Mouse.Click(options.UIActiveListItem);
                    break;
                case TaskListState.Completed:
                    Mouse.Click(options.UICompletedListItem);
                    break;

            }
        }

        public void DeleteTask(int id)
        {
            var listItem = getListItem(id);
            var t = new WpfButton(listItem);
            string auto_id = "Task_" + id + "-Delete";
            t.SearchProperties[WpfButton.PropertyNames.AutomationId] = auto_id;
            t.WindowTitles.Add("ToDoList");

            Mouse.Click(t);
        }

        public void ToggleTask(int id)
        {
            var taskItem = GetTaskItemStatus(id);

            taskItem.Checked = !taskItem.Checked;
            
        }


        public void ClearCompletedTasks()
        {
            Mouse.Click(UIToDoListWindow.ClearCompletedButton);
        }


        public bool IsTaskChecked(int id)
        {
            return GetTaskItemStatus(id).Checked;
        }

        private WpfCheckBox GetTaskItemStatus(int id)
        {
            var listItem = getListItem(id);
            var t = new WpfCheckBox(listItem);
            string auto_id = "Task_" + id + "-Toggle";
            t.SearchProperties[WpfCheckBox.PropertyNames.AutomationId] = auto_id;
            t.WindowTitles.Add("ToDoList");
            return t;
        }

        public int TaskCount()
        {
            return UIToDoListWindow.TaskList.Items.Count;
        }

        private WpfListItem getListItem(int id)
        {
            var t = new WpfListItem(this.UIToDoListWindow.TaskList);
            string auto_id = "Task_" + id;
            t.SearchProperties[WpfListItem.PropertyNames.AutomationId] = auto_id;
            t.WindowTitles.Add("ToDoList");

            return t;
        }


        public bool IsTaskVisible(int id)
        {
            var t = new WpfListItem(this.UIToDoListWindow.TaskList);
            string auto_id = "Task_" + id;
            t.SearchProperties[WpfListItem.PropertyNames.AutomationId] = auto_id;
            t.WindowTitles.Add("ToDoList");
            return t.Exists;

        }



        public bool IsClearCompletedTaskVisible()
        {
            var t = UIToDoListWindow.ClearCompletedButton;
            Point point;
            return t.TryGetClickablePoint(out point);
           

        }
        
    }





  

   


}