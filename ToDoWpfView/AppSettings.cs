using System.Configuration;
using ToDoMvvm;

namespace ToDoWpfView
{
    public class AppSettings :IAppSettings
    {

        public AppSettings()
        {
            TaskDatabaseName = ConfigurationManager.AppSettings["taskdatabase"];
        }

        public string TaskDatabaseName { get; private set; }
    }
   
}
