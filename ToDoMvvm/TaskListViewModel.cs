using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMvvm
{
    /// <summary>
    /// View model for managing a list of tasks
    /// </summary>
    public class TaskListViewModel : ViewModelBase
    {
        #region Task Filter

        //only complete tasks
        private static readonly Predicate<TaskItem> CompleteFilter = t => t.Completed;

        //only active tasks
        private static readonly Predicate<TaskItem> ActiveFilter = t => !t.Completed;

        //only all tasks
        private static readonly Predicate<TaskItem> AllFilter = t => true;

        #endregion Task Filter

        #region action

        //add new task
        public RelayCommand AddNewTask { get; private set; }

        //delete a given task
        public RelayCommand<TaskItem> DeleteTask { get; private set; }

        //toggle state of a given task
        public RelayCommand<TaskItem> ToggleStateOfTask { get; private set; }

        //delete all completed tasks
        public RelayCommand DeleteCompleted { get; private set; }

        #endregion action

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="taskRepository">persistence layer of task repo</param>
        /// <param name="factory"></param>
        public TaskListViewModel(ITaskRepository taskRepository, ICollectionViewSourceFactory factory)
        {
            _taskRepository = taskRepository;
            //default task is empty
            NewTaskDescription = string.Empty;
            _activeTaskListState = TaskListState.All;
            VisibleTasks = factory.CreateTaskListViewSource();
            _currentActiveFilter = AllFilter;

            //relay commands
            AddNewTask = new RelayCommand(CreateNewTask);
            DeleteTask = new RelayCommand<TaskItem>(DeleteIndividualTask);
            ToggleStateOfTask = new RelayCommand<TaskItem>(ToggleCopleteTask);
            DeleteCompleted = new RelayCommand(DeleteCompletedTask, () => ClearCompletedTasksEnabled);

            FetchTasks();
            
           
        }

        /// <summary>
        /// Read content from task repo 
        /// </summary>
        /// <returns></returns>
        private async Task FetchTasks()
        {
            Tasks = new ObservableCollection<TaskItem>();
            VisibleTasks.SetSource(Tasks);
            IList<TaskItem> tasks = await _taskRepository.GetTasks();

            foreach (TaskItem t in tasks)
            {
                Tasks.Add(t);
            }
            //update views
            UpdateCounts();
        }


        #region private fields
        //active task list state
        private TaskListState _activeTaskListState;
        //task repository
        private readonly ITaskRepository _taskRepository;
        //tasks left
        private string _taskLeftMessage;
        //clear completed message
        private string _clearCompletedMessage;
        //new task description
        private string _newTaskDescription;
        //can clear completed tasks
        private bool _clearCompletedTasksEnabled;

        /// <summary>
        /// Get current filter on collection list
        /// </summary>
        private Predicate<TaskItem> _currentActiveFilter;
        #endregion


        #region properties
        /// <summary>
        /// Visible tasks with current filter
        /// </summary>
        public IWrappedCollectionViewSource<TaskItem> VisibleTasks { get; private set; }

       
        /// <summary>
        /// Selected TaskList enum
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return (int)_activeTaskListState;
            }
            set
            {
                _activeTaskListState = (TaskListState)value;
                //update view
                ChangeVisibleTask();
                RaisePropertyChanged(() => SelectedIndex);
            }
        }
       
       

        /// <summary>
        /// tasks
        /// </summary>
        public ObservableCollection<TaskItem> Tasks { get; set; }

       
        /// <summary>
        /// message to display if tasks left
        /// </summary>
        public string TasksLeftMessage
        {
            get { return _taskLeftMessage; }
            set
            {
                _taskLeftMessage = value;
                RaisePropertyChanged(() => TasksLeftMessage);
            }
        }

        /// <summary>
        /// message to display if completed tasks
        /// </summary>
        public string ClearCompletedMessage
        {
            get { return _clearCompletedMessage; }
            set
            {
                _clearCompletedMessage = value;
                RaisePropertyChanged(() => ClearCompletedMessage);
            }
        }


        /// <summary>
        /// description of possible new task
        /// </summary>
        public string NewTaskDescription
        {
            get { return _newTaskDescription; }
            set
            {
                _newTaskDescription = value;
                RaisePropertyChanged(() => NewTaskDescription);
            }
        }



        /// <summary>
        /// Option to delete completed tasks
        /// </summary>
        public bool ClearCompletedTasksEnabled
        {
            get { return _clearCompletedTasksEnabled; }

            set
            {
                _clearCompletedTasksEnabled = value;

                RaisePropertyChanged(() => ClearCompletedTasksEnabled);
            }
        }

        #endregion

        #region helper methods

        /// <summary>
        /// Delete completed tasks
        /// </summary>
        private void DeleteCompletedTask()
        {
            for (int x = Tasks.Count - 1; x >= 0; x--)
            {
                TaskItem t = Tasks[x];
                if (t.Completed)
                {
                    Tasks.Remove(t);
                }
            }
            UpdateCounts();
        }

        /// <summary>
        /// update the current filter
        /// </summary>
        private void ChangeVisibleTask()
        {
            switch (_activeTaskListState)
            {
                case TaskListState.Completed:
                    _currentActiveFilter = CompleteFilter;
                    break;

                case TaskListState.Active:
                    _currentActiveFilter = ActiveFilter;
                    break;

                default:
                    _currentActiveFilter = AllFilter;
                    break;
            }
            VisibleTasks.ChangeFilter(generifyPredicate(_currentActiveFilter));
            UpdateCounts();
            RaisePropertyChanged(() => VisibleTasks.View);
        }

        /// <summary>
        /// Create a new tasks
        /// </summary>
        private void CreateNewTask()
        {
            TaskItem taskItem = _taskRepository.CreateTaskItem(NewTaskDescription);
            Tasks.Add(taskItem);

            NewTaskDescription = string.Empty;
            UpdateCounts();
        }

        /// <summary>
        /// CollectionViewSource reqquires filter to work on type object
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private Predicate<object> generifyPredicate(Predicate<TaskItem> t)
        {
            Predicate<object> p = l => t((TaskItem)l);
            return p;
        }

        /// <summary>
        /// delete given task
        /// </summary>
        /// <param name="t"></param>
        private void DeleteIndividualTask(TaskItem t)
        {
            Tasks.Remove(t);

            UpdateCounts();
        }

        /// <summary>
        /// Update Counts and any messages
        /// </summary>
        private async void UpdateCounts()
        {
            //update number of completed tasks
            int completedTasks = Tasks.Count(t => CompleteFilter(t));

            //set completion messages
            if (completedTasks > 0)
            {
                ClearCompletedMessage = "Clear Completed (" + completedTasks + ")";
                ClearCompletedTasksEnabled = true;
            }
            else
            {
                ClearCompletedMessage = "";
                ClearCompletedTasksEnabled = false;
            }
            DeleteCompleted.RaiseCanExecuteChanged();

            //update number of active tasks
            int activeTask = Tasks.Count(t => ActiveFilter(t));

            TasksLeftMessage = activeTask > 0 ? string.Format("{0} task left", activeTask) : "";

            //refresh the list
            VisibleTasks.Refresh();

            await _taskRepository.SaveTasks(Tasks);
        }

        /// <summary>
        /// togle the state of given task
        /// </summary>
        /// <param name="t"></param>
        private void ToggleCopleteTask(TaskItem t)
        {
            t.Completed = !t.Completed;
            UpdateCounts();
        }


        #endregion
    }
}