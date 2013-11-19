using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.File.Wpf;
using ToDoMvvm;
using ToDoWpfView;

namespace TodoUiTest.steps
{
    /// <summary>
    /// Context that is used between different steps
    /// </summary>
   public  class TaskListContext
    {

       /// <summary>
       /// Task List Context
       /// </summary>
       public TaskListContext()
       {
           ITaskRepository iTaskRepository = new TaskRepository(new MvxWpfFileStore(), new AppSettings());
           LastAddedtTaskId = iTaskRepository.GetLastTaskId();
           AllTasks = new List<TaskItem>();
          
       }

       /// <summary>
       /// last added task's id
       /// </summary>
       public int LastAddedtTaskId { get; private set; }

       //last added task
       public TaskItem LastAddedTask
       {
           get
           {
               return AllTasks.Last();
           } 
          
       }
       /// <summary>
       /// Last Updated Task
       /// </summary>
       public TaskItem LastUpdatedTask { get; set; }

       /// <summary>
       /// All added tasks
       /// </summary>
       public IList<TaskItem> AllTasks { get; set; }

       /// <summary>
       /// Add task
       /// </summary>
       /// <param name="description"></param>
       /// <param name="completed"></param>
       public void AddTask(string description, bool completed = false)
       {
           LastAddedtTaskId++;
           TaskItem ti = new TaskItem(LastAddedtTaskId,description,completed);
           AllTasks.Add(ti);
       }

       /// <summary>
       /// Update the given task (by id)  description
       /// </summary>
       /// <param name="id"></param>
       /// <param name="description"></param>
       public void UpdateTaskDescription(int id, string description)
       {
           TaskItem ti = AllTasks.First(s => s.Id == id);
           ti.Description = description;
           LastUpdatedTask = ti;
           LastUpdatedTaskId = ti.Id;
       }

       /// <summary>
       /// Get task Id given a description
       /// </summary>
       /// <param name="taskDecription"></param>
       /// <returns></returns>
       public int TaskId(string taskDecription)
       {
           TaskItem matchedTask =AllTasks.First(t => t.Description == taskDecription);
           return matchedTask.Id;
       }

       /// <summary>
       /// Last Modifed task Id
       /// </summary>
       public int LastUpdatedTaskId
       {
           get; set;
       }
    }
    
}
