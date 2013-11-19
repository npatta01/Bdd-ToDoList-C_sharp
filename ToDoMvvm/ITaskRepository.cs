using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoMvvm
{
 
    
    /// <summary>
    /// Repository to store tasks
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Get all the tasks
        /// </summary>
        /// <returns></returns>
        Task<IList<TaskItem>> GetTasks();

        /// <summary>
        /// Save the given task to the repo
        /// </summary>
        /// <param name="tasks">list of tasks</param>
        /// <returns></returns>
        Task SaveTasks(IList<TaskItem> tasks);

        /// <summary>
        /// Create a task with the given description
        /// </summary>
        /// <param name="taskdescription"></param>
        /// <returns></returns>
        TaskItem CreateTaskItem(string taskdescription);

        /// <summary>
        /// Get the last added task id
        /// </summary>
        /// <returns></returns>
        int GetLastTaskId();
    }
}