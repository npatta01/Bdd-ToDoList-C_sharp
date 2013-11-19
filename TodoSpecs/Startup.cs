using System;
using System.IO;

namespace ToDoSpecs
{
    public class Startup
    {
        static private void CopyAddin()
        {
            if (!Directory.Exists("addins"))
            {
                Directory.CreateDirectory("addins");
            }

            File.Copy("NSpecAddin.dll", Path.Combine("addins", "NSpecAddin.dll"), true);
        }

        [STAThread]
        static public void Main(string[] args)
        {
            CopyAddin();

            NUnit.Gui.AppEntry.Main(new[]
            {
                "Test.nunit"
            });
        }
    }
}
