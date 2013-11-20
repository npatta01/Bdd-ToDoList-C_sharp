using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using NSpec;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoMvvm;

namespace ToDoSpecs.Specs
{
    internal class TaskRepository_spec : nspec
    {
        private IAppSettings _appSettings;
        private IMvxFileStore _ifileStore;
        private IList<TaskItem> _taskItem;
        private string _fileName;
        private ITaskRepository _taskRepository;
        private string _content;

        /// <summary>
        /// setup code
        /// </summary>
        private void before_each()
        {
            //mock the appsettings and filesystem
            _fileName = "tmp.txt";
            _taskRepository = null;
            _content = "";
            
            _appSettings = Substitute.For<IAppSettings>();
            _appSettings.TaskDatabaseName.Returns(_fileName);
           _ifileStore = Substitute.For<IMvxFileStore>();
           
        }

        /// <summary>
        /// Checks correct behaviour when reading tasks
        /// </summary>
        private async Task reading_tasks()
        {
            it["retrieved file from persistence"] = () =>
            {
                _taskRepository = new TaskRepository(_ifileStore, _appSettings);
                //compiler wants to assign to something
                var tmp = _appSettings.Received().TaskDatabaseName;
                _ifileStore.ReceivedWithAnyArgs().TryReadTextFile(_fileName, out _content);
            };

            context["given no tasks in persistence"] = () =>
            {
                before = async () =>
               {
                   _ifileStore.TryReadTextFile(_fileName, out _content).Returns(x => x[1] = "");
                   _taskRepository = new TaskRepository(_ifileStore, _appSettings);

                   _taskItem = await _taskRepository.GetTasks();
                   _content = "";
               };

                it["task count is 0"] = () => _taskItem.Count.should_be(0);

                it["last task id is 0"] = () => _taskRepository.GetLastTaskId().should_be(0);
            };

            context["given existing tasks in persistence"] = () =>
            {
                IList<TaskItem> retrievedItems = null;

                before = async () =>
            {
                _taskItem = new List<TaskItem> { new TaskItem(5, "task 5"), new TaskItem(9, "task 9", true) };

                _ifileStore.TryReadTextFile(_fileName, out _content).Returns(x => x[1] = JsonConvert.SerializeObject(_taskItem));
                _taskRepository = new TaskRepository(_ifileStore, _appSettings);
                _content = "";
                retrievedItems = await _taskRepository.GetTasks();
            };

                it["task list has correct items"] = () =>
                {
                    retrievedItems.Count.should_be(2);
                    TaskItem ti = retrievedItems[1];
                    ti.Completed.should_be_true();
                    ti.Id.should_be(9);
                    ti.Description.should_be("task 9");
                };

                it["last id is last id of item"] = () => _taskRepository.GetLastTaskId().should_be(9);
            };
        }

        /// <summary>
        /// Checks correct behaviour when saving tasks
        /// </summary>
        private async Task save_tasks()
        {
            before = () =>
            {
                //Arrange
                //create task and expected content
                _taskItem = new List<TaskItem> { new TaskItem(5, "task 5"), new TaskItem(9, "task 9") };
                _taskRepository = new TaskRepository(_ifileStore, _appSettings);
                _content = JsonConvert.SerializeObject(_taskItem);
                //Act
                _taskRepository.SaveTasks(_taskItem);
            };

            it["writes to file correctly"] = async () =>
            {
                _ifileStore.Received().WriteFile(_fileName, _content);

                //compare written items
                IList<TaskItem> retrievedItems = await _taskRepository.GetTasks();
                retrievedItems.Count.should_be(_taskItem.Count);
                //task1
                retrievedItems[0].Id.should_be(_taskItem[0].Id);
                retrievedItems[0].Description.should_be(_taskItem[0].Description);
                retrievedItems[0].Completed.should_be(_taskItem[0].Completed);

                //task 2
                retrievedItems[1].Id.should_be(_taskItem[1].Id);
                retrievedItems[1].Description.should_be(_taskItem[1].Description);
                retrievedItems[1].Completed.should_be(_taskItem[1].Completed);
            };

            it["last id is largest of task id"] = () => _taskRepository.GetLastTaskId().should_be(9);
        }
    }
}