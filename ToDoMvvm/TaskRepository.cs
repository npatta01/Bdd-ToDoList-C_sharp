using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMvvm
{
    public class TaskRepository : ITaskRepository
    {
        //reference to filesystem
        private readonly IMvxFileStore _iFileStore;

        //task list
        private IList<TaskItem> _tasks;

        //file na,e
        private readonly string _fileName;

        //last created task id
        private int _lastTaskId;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iFileStore">file system reference</param>
        /// <param name="appSettings">Application settings</param>
        public TaskRepository(IMvxFileStore iFileStore, IAppSettings appSettings)
        {
            _iFileStore = iFileStore;
            _tasks = new List<TaskItem>();
            _lastTaskId = 0;
            //file name of tasks from repo
            _fileName = appSettings.TaskDatabaseName;
        }

        /// <inheritdoc/>
        public async Task<IList<TaskItem>> GetTasks()
        {
            return await Task.Run(() =>
            {
                //get contents from file
                string contents;
                _iFileStore.TryReadTextFile(_fileName, out contents);

                //if file has content, try parsing
                if (!String.IsNullOrWhiteSpace(contents))
                {
                    _tasks = JsonConvert.DeserializeObject<List<TaskItem>>(contents);

                    //set task id to last read task item
                    if (_tasks.Count != 0)
                    {
                        _lastTaskId = LastTaskId();
                    }
                }
                return _tasks;
            });
        }

        /// <inheritdoc/>
        private int LastTaskId()
        {
            if (_tasks.Count == 0)
            {
                return 0;
            }
            if (_tasks.Count == 1)
            {
                return _tasks.First().Id;
            }
            //get highest task id
            TaskItem taskItem = _tasks.Aggregate((i1, i2) => i1.Id > i2.Id ? i1 : i2);
            return taskItem.Id;
        }

        /// <inheritdoc/>
        public async Task SaveTasks(IList<TaskItem> tasks)
        {
            _tasks = tasks;
            _lastTaskId = LastTaskId();

            await Task.Run(() =>
            {
                //serialize list
                string contents = JsonConvert.SerializeObject(_tasks);

                //save task
                try
                {
                    _iFileStore.WriteFile(_fileName, contents);
                }
                catch (IOException)
                {
                }
            });
        }

        /// <inheritdoc/>
        public TaskItem CreateTaskItem(string taskdescription)
        {
            _lastTaskId = _lastTaskId + 1;
            return new TaskItem(_lastTaskId, taskdescription);
        }

        /// <inheritdoc/>
        public int GetLastTaskId()
        {
            return _lastTaskId;
        }
    }
}