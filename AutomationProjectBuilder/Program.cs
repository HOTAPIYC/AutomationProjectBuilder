using AutomationProjectBuilder.Data.Services;
using AutomationProjectBuilder.Export.CodeGenerator;
using System;

namespace AutomationProjectBuilder
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Automation project builder version 0.0.1.\nDon't close this window while the application is running!");

            var dataService = new DataService();
            var plcExport = new CodeGenService(dataService);

            var application = new App();

            application.SetDataService(dataService);

            application.Run();
        }
    }
}
