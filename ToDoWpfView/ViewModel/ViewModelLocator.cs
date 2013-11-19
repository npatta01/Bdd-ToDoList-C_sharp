/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:TodoView"
                           x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System.Diagnostics.CodeAnalysis;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.File.Wpf;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ToDoMvvm;
using ToDoMvvm.Design;

namespace ToDoWpfView.ViewModel
{
    
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //real app
            if (ViewModelBase.IsInDesignModeStatic)
            {
                //task repository
                SimpleIoc.Default.Register<ITaskRepository, DesignTaskRepository>();
            }
            else
            {
                
                //register file system manager
                SimpleIoc.Default.Register<IMvxFileStore, MvxWpfFileStore>();
                //use actual task repository
                SimpleIoc.Default.Register<ITaskRepository, TaskRepository>();
            }


            if (!SimpleIoc.Default.IsRegistered<IAppSettings>())
            {
                //register appsettings
                SimpleIoc.Default.Register<IAppSettings, AppSettings>();
            }
            

            //collection view source
            if (!SimpleIoc.Default.IsRegistered<IWrappedCollectionViewSource<TaskItem>>())
            {
                //register appsettings
                SimpleIoc.Default.Register<IWrappedCollectionViewSource<TaskItem>, WrappedCollectionViewSource<TaskItem>>();
            }


           
            SimpleIoc.Default.Register<ICollectionViewSourceFactory,CollectionViewSourceFactory>();
            //register main view model
            SimpleIoc.Default.Register<TaskListViewModel>();
         


        }


        /// <summary>
        /// The Main View Model
        /// </summary>
        public TaskListViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TaskListViewModel>();
            }
        }

        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<TaskListViewModel>();

            // TODO Clear the ViewModels
        }
    }
}