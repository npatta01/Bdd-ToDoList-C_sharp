using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoMvvm.Design
{


  /// <summary>
  /// A Mock repisotry used dusing design time
  /// </summary>
    public class DesignTaskRepository : ITaskRepository
    {
        //task list
        private readonly IList<TaskItem> _tasklList;

        /// <summary>
        /// Create some dummy tasks
        /// </summary>
        public DesignTaskRepository()
        {
            _tasklList = new List<TaskItem>
            {
                new TaskItem(1, "Task1", true),
                new TaskItem(2, "Task2"),
                new TaskItem(3, "Task3")
            };
        }

        /// <inheritdoc/>
        public Task<IList<TaskItem>> GetTasks()
        {
            return Task.FromResult(_tasklList);
        }

        /// <inheritdoc/>
        public async Task SaveTasks(IList<TaskItem> tasks)
        {
            //  throw new NotImplementedException();
          
        }

      /// <inheritdoc/>
        public TaskItem CreateTaskItem(string taskdescription)
        {
            return null;
        }

        /// <inheritdoc/>
        public int GetLastTaskId()
        {
            return 3;
        }
    }
}