using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSpec;
using NSubstitute;
using ToDoMvvm;
using ToDoWpfView;

namespace ToDoSpecs.Specs
{
    class TaskListViewModel_spec :nspec
    {


        private ITaskRepository _taskRepository;
        private IList<TaskItem> _tasks;
        private TaskListViewModel _taskListViewModel;

        private ICollectionViewSourceFactory _collectionSource;

        /// <summary>
        /// setup code
        /// </summary>
        private void before_each()
        {
            _tasks = new List<TaskItem>();
            _taskRepository = Substitute.For<ITaskRepository>();
            _collectionSource = Substitute.For<ICollectionViewSourceFactory>();

            var wrappedCollectionViewSource = new WrappedCollectionViewSource<TaskItem>();
            _collectionSource.CreateTaskListViewSource().Returns(wrappedCollectionViewSource);
            _taskListViewModel = null;


        }

        /// <summary>
        /// Verifies default state of task list
        /// </summary>
        void default_options()
        {
            before = () =>
            {
                _taskListViewModel = new TaskListViewModel(_taskRepository, _collectionSource);
         
            };
            it["selected all task state"] = () => _taskListViewModel.SelectedIndex.should_be((int)TaskListState.All);
        }


        /// <summary>
        /// State when no task
        /// </summary>
        void no_task()
        {

            before =  () =>
            {
                _tasks = new List<TaskItem>();
                _taskRepository.GetTasks().Returns(Task.FromResult(_tasks));
                _taskListViewModel = new TaskListViewModel(_taskRepository, _collectionSource);
               };

            it["clear completed should be disabled"] =() => _taskListViewModel.ClearCompletedTasksEnabled.should_be_false();


            it["active message is empty"] = () => _taskListViewModel.TasksLeftMessage.should_be_empty();

            it["clear completed message is empty"] = () => _taskListViewModel.ClearCompletedMessage.should_be_empty();

            it["have no visible tasks"] = () =>
            {
                _taskListViewModel.VisibleTasks.ChangeFilter(l=>true);
                _taskListViewModel.VisibleTasks.IsEmpty().is_true();
            };
        }

        /// <summary>
        /// Test when application has code and active tasks
        /// </summary>
        void completed_and_active_tasks_exist()
        {

            before =  () =>
            {
                _tasks = new List<TaskItem>();
                _tasks.Add(new TaskItem(1,"task1",false));
                _tasks.Add(new TaskItem(2, "task2", true));
                
                _taskRepository.GetTasks().Returns(Task.FromResult(_tasks));
                _taskListViewModel = new TaskListViewModel(_taskRepository, _collectionSource);
              
            };

            it["clear completed should be enabled"] = () => _taskListViewModel.ClearCompletedTasksEnabled.should_be_true();


            it["active message show appropriate message"] = () => _taskListViewModel.TasksLeftMessage.should_be("1 task left");

            it["clear completed shows appropriate message"] = () => _taskListViewModel.ClearCompletedMessage.should_be("Clear Completed (" + 1 + ")");

            it["have visible tasks"] = () =>
            {
                _taskListViewModel.VisibleTasks.ChangeFilter(l => true);
                _taskListViewModel.VisibleTasks.IsEmpty().is_false();
            };


            context["correct items shown in different view"] = () =>
            {
                it["shows all items in All task View"] = () =>
                {
                    _taskListViewModel.SelectedIndex = (int) TaskListState.All;
                    List<TaskItem> retrievedItems = _taskListViewModel.VisibleTasks.Items.ToList();
                    retrievedItems.Count.should_be(2);
                };

                it["shows only active items in Active Task View"] = () =>
                {
                    _taskListViewModel.SelectedIndex = (int)TaskListState.Active;
                    List<TaskItem> retrievedItems = _taskListViewModel.VisibleTasks.Items.ToList();
                    retrievedItems.Count.should_be(1);
                    retrievedItems[0].Id.should_be(_tasks[0].Id);
                    retrievedItems[0].Description.should_be(_tasks[0].Description);
                    retrievedItems[0].Completed.should_be(_tasks[0].Completed);
                };

                it["shows only active items in Completed Task View"] = () =>
                {
                  

                    _taskListViewModel.SelectedIndex = (int)TaskListState.Completed;
                    List<TaskItem> retrievedItems = _taskListViewModel.VisibleTasks.Items.ToList();
                    retrievedItems.Count.should_be(1);
                    retrievedItems[0].Id.should_be(_tasks[1].Id);
                    retrievedItems[0].Description.should_be(_tasks[1].Description);
                    retrievedItems[0].Completed.should_be(_tasks[1].Completed);
                };
            };
        }


       /// <summary>
       /// Test behaviour when updating a task
       /// </summary>
        void updating_a_task()
        {
            context["completing a task"] = () =>
            {
                before =  () =>
                {
                    _tasks = new List<TaskItem> {new TaskItem(1, "task1", false)};
                    _taskRepository.GetTasks().Returns(Task.FromResult(_tasks));
                    
                    _taskListViewModel = new TaskListViewModel(_taskRepository, _collectionSource);
                   _taskListViewModel.VisibleTasks.Items.Count().should_be(1);
                    _taskListViewModel.ClearCompletedMessage.should_be_empty();
                    _taskListViewModel.ToggleStateOfTask.Execute(_tasks[0]);
                    
                };
               

                it["item not visible in active"] = () =>
                {
                    _taskListViewModel.SelectedIndex = (int) TaskListState.Active;
                    _taskListViewModel.VisibleTasks.Items.Count().should_be(0);
                };

                it["item visible in all"] = () =>
                {
                    _taskListViewModel.SelectedIndex = (int)TaskListState.All;
                    _taskListViewModel.VisibleTasks.Items.Count().should_be(1);
                };

                it["item visible in complete"] = () =>
                {
                    _taskListViewModel.SelectedIndex = (int)TaskListState.Completed;
                    _taskListViewModel.VisibleTasks.Items.Count().should_be(1);
                };


                it["correct active message"] = () => _taskListViewModel.TasksLeftMessage.should_be_empty();

                it["correct completed message"] = () => _taskListViewModel.ClearCompletedMessage.should_be("Clear Completed (" + 1 + ")");

            
            };

           
            context["deleting a task"] = () =>
            {
                before =  () =>
                {
                    _tasks = new List<TaskItem>();
                    TaskItem taskItem = new TaskItem(1, "task1", true);
                    _tasks.Add(taskItem);
                    _taskRepository.GetTasks().Returns(Task.FromResult(_tasks));
                    _taskListViewModel = new TaskListViewModel(_taskRepository, _collectionSource);
                   _taskListViewModel.VisibleTasks.Items.ToArray().Count().should_be(1);
                    _taskListViewModel.DeleteTask.Execute(taskItem);

                };

                it["item not visible"] = () => _taskListViewModel.VisibleTasks.IsEmpty().should_be_true();

                it["correct active message"] = () => _taskListViewModel.TasksLeftMessage.should_be_empty();

                it["correct completed message"] = () => _taskListViewModel.ClearCompletedMessage.should_be_empty();

            };

            context["editing a task"] = () =>
            {
                TaskItem taskItem = new TaskItem(1, "task1", true);
                before = () =>
                {
                    _tasks = new List<TaskItem> {taskItem};
                    _taskListViewModel = new TaskListViewModel(_taskRepository, _collectionSource);

                };

                it["shows updated task message"] = () =>
                {
                    _tasks[0].Description = "new task description";
                    _tasks[0].Description.should_be("new task description");

                };
            };

           
        }

        void clear_all_completed()
        {
            before = () =>
            {
                _tasks = new List<TaskItem>();
                _tasks.Add(new TaskItem(1, "task1", true));
                _tasks.Add(new TaskItem(2, "task2", true));

                _taskRepository.GetTasks().Returns(Task.FromResult(_tasks));
                _taskListViewModel = new TaskListViewModel(_taskRepository, _collectionSource);
            };

            it["clear completed shows appropriate message when clicked"] = () =>
            {
                _taskListViewModel.ClearCompletedMessage.should_be("Clear Completed (" + 2 + ")");
                _taskListViewModel.DeleteCompleted.Execute(null);
                _taskListViewModel.ClearCompletedMessage.should_be_empty();

            };
        }

       
    }
}
