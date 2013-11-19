using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Cirrious.MvvmCross.Plugins.File.Wpf;
using Microsoft.VisualStudio.TestTools.UITesting;
using TechTalk.SpecFlow;
using ToDoMvvm;
using ToDoWpfView;

namespace TodoUiTest.steps
{
    /// <summary>
    /// Steps that are used to setup/start application 
    /// </summary>
    [Binding]
    public static class SetupSteps
    {
        //application reference
        private static ApplicationUnderTest _a;
        //app process name
        private const string ProcessName = "ToDoWpfView";
        //app binary
        private const string AppBinary = "ToDoWpfView.exe";


        /// <summary>
        /// Setup code
        /// Close app and launch app
        /// </summary>
        [BeforeScenario]
        public static  void SpecflowBeforeTestRun()
        {
            CloseExistingApp();
            ClearExisitngTasks();
            LaunchApp();
            _a.CloseOnPlaybackCleanup = true;
        }

        /// <summary>
        /// Clear exisitng tasks in file used by app
        /// </summary>
        private static void ClearExisitngTasks()
        {
            var tr = new TaskRepository(new MvxWpfFileStore(), new AppSettings());
            tr.SaveTasks(new List<TaskItem>()).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Launch an app
        /// Assusmes that the app is not already open
        /// </summary>
        private static void LaunchApp()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), AppBinary);
            _a = ApplicationUnderTest.Launch(path);
        }

        /// <summary>
        /// Close existing app
        /// </summary>
        private static void CloseExistingApp()
        {
            //get all apps with same name
            foreach (Process p in Process.GetProcessesByName(ProcessName))
            {
                try
                {
                    p.Kill();
                    p.WaitForExit(); // no timeout
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Close and reopen app
        /// </summary>
        public static void ReopenApp()
        {
            CloseExistingApp();
            LaunchApp();
        }

        /// <summary>
        /// Cleanup resources
        /// </summary>
        [AfterScenario]
        public static void SpecflowAfterTestRun()
        {
            Playback.Cleanup();
        }
    }
}